using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.Script;

//namespace Pointer
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;
        
        HtmlWindow window;
        const float RADIUS = 20f;

        HtmlElement canvas;
        HtmlObject ctx;
        HtmlElement tracker;


        float X = 50;
        float Y = 50;

        int canvasWidth = 0;
        int canvasHeight = 0;

        ScriptObjectCallback animationCallback;
        ScriptObjectCallback animation;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

            try
            {

                var document = await HtmlPage.GetDocument();
                window = await HtmlPage.GetWindow();
                canvas = await document.QuerySelector("canvas");

                canvasWidth = await canvas.GetProperty<int>("width");
                canvasHeight = await canvas.GetProperty<int>("height");
                
                ctx = await canvas.Invoke<HtmlObject> ("getContext", "2d");

                animationCallback = new ScriptObjectCallback(
                        async (ar) =>
                        {
                             await CanvasDraw().ContinueWith(
                                 (t) => { animation = null; }
                             );
                        }

                    );            

                await CanvasDraw();
                
                await canvas.AttachEvent("click", new EventHandler(
                    async (sender, e) =>
                    {
                        if ((await document.GetProperty<HtmlObject>("pointerLockElement"))?.Handle == canvas.Handle)
                        {
                            await document.Invoke<object>("exitPointerLock");
                        }
                        else
                        {
                            await canvas.Invoke<object>("requestPointerLock");
                        }
                    })
                );

                // Hook pointer lock state change events
                await document.AttachEvent("pointerlockchange", new EventHandler(
                    async (sender, e) =>
                    {
                        if ((await document.GetProperty<HtmlObject>("pointerLockElement"))?.Handle == canvas.Handle)
                        {
                            await console.Log("The pointer lock status is now locked");
                            await document.AttachEvent("mousemove", UpdatePosition);
                        }
                        else
                        {
                            await console.Log("The pointer lock status is now unlocked");
                            await document.DetachEvent("mousemove", UpdatePosition);
                        }
                    })
                );

                tracker = await document.GetElementById("tracker");

                await console.Log($"Hello:  {input}");

                
  
 
                
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }

        async void UpdatePosition(object sender, HtmlEventArgs e)
        {
            X += e.MovementX;
            Y += e.MovementY;
            
            if (X > canvasWidth + RADIUS) {
                X = -RADIUS;
            }
            if (Y > canvasHeight + RADIUS) {
                Y = -RADIUS;
            }  
            if (X < -RADIUS) {
                X = canvasWidth + RADIUS;
            }
            if (Y < -RADIUS) {
                Y = canvasHeight + RADIUS;
            }
            
            await tracker.SetProperty("textContent", $"X position: {X} Y position: {Y}");

            if (animation == null)
            {
                animation = animationCallback;
                await window.Invoke<object>("requestAnimationFrame", animation);
            }
        }

        async Task CanvasDraw()
        {
            await ctx.SetProperty("fillStyle", "black");
            await ctx.Invoke<object>("fillRect", 0, 0, 
                    canvasWidth, canvasHeight);
            await ctx.SetProperty("fillStyle","#f00");
            await ctx.Invoke<object>("beginPath");
            await ctx.Invoke<object>("arc" ,
                    X, Y, RADIUS, 0, degreesToRadians(360), true);
            await ctx.Invoke<object>("fill");
        }
        static float degreesToRadians (float degrees)
        {
            return (float)Math.PI / 180f * degrees;
        }
    }

//}

