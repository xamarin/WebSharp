using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.Script;

//namespace Dragger
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

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

                HtmlElement dragged = null;

                // events fired on the draggable target */
                await document.AttachEvent(HtmlEventNames.Drag,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            
                        }
                    )
                
                );

                await document.AttachEvent(HtmlEventNames.DragStart,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // store a ref. on the dragged element
                            dragged = evt.Target as HtmlElement;

                            // make it half transparent
                            await dragged.SetStyleAttribute("opacity", ".5");
                            
                        }
                    )
                );

                await document.AttachEvent(HtmlEventNames.DragEnd,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // reset the transparency
                            await evt.Target.SetStyleAttribute("opacity", "");
                            
                        }
                    )
                );

                await document.AttachEvent(HtmlEventNames.DragExit,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // reset the transparency
                            await evt.Target.SetStyleAttribute("opacity", "");
                            
                        }
                    )
                );

                // Events fired on the drop targets
                await document.AttachEvent(HtmlEventNames.DragOver,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // prevent default to allow drop
                            evt.PreventDefault();
                            evt.StopPropagation();

                            // A DropEffect must be set
                            evt.DataTransfer.DropEffect = DropEffect.Link;
                        }
                    )

                );

                //Events fired on the drop targets
                await document.AttachEvent(HtmlEventNames.DragEnter,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // highlight potential drop target when the draggable element enters it
                            if ( await evt.Target.GetCssClass() == "dropzone" ) 
                            {
                               await evt.Target.SetStyleAttribute("background", "purple");
                            }
                            
                        }
                    )

                );

                await document.AttachEvent(HtmlEventNames.DragLeave,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // highlight potential drop target when the draggable element enters it
                            if ( await evt.Target.GetCssClass() == "dropzone" ) 
                            {
                                await evt.Target.SetStyleAttribute("background", "");
                            }
                            
                        }
                    )

                );

                await document.AttachEvent(HtmlEventNames.Drop,
                    new EventHandler<HtmlEventArgs> (
                        async (sender, evt) =>
                        {
                            // prevent default to allow drop
                            evt.PreventDefault();
                            evt.StopPropagation();

                            // move dragged elem to the selected drop target
                            if ( await evt.Target.GetCssClass() == "dropzone" ) 
                            {
                                await evt.Target.SetStyleAttribute("background", "");
                                await dragged.GetParent().ContinueWith(
                                    (t) => { t.Result?.RemoveChild( dragged ); }

                                );
                                //dragged.parentNode.removeChild( dragged );
                                await evt.Target.AppendChild( dragged );
                            }                            
                        }
                    )

                );

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
