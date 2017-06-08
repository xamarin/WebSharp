using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;

namespace WebSharpJs.Electron
{

    public class Tray : EventEmitter
    {

        protected override string ScriptProxy =>
             @"function () { 
                            let tray = null;
                            tray = new Tray(options[0]);
                            // due to garbage collection we need to keep this object around
                            // what we will do is add a reference to the tray.
                            // See https://electron.atom.io/docs/faq/#my-apps-windowtray-disappeared-after-a-few-minutes
                            app.ws_tray = tray;
                            return tray;
                        }()";

        protected override string Requires => @"const {Tray, app} = require('electron');";

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<Tray> Create(string path)
        {
            var proxy = new Tray();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(path);
            return proxy;
        }

        public static async Task<Tray> Create(NativeImage icon)
        {
            var proxy = new Tray();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(icon);
            return proxy;
        }

        protected Tray() : base() { }

        protected Tray(object scriptObject) : base(scriptObject)
        { }

        public Tray(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Tray(ScriptObjectProxy sop)
        {
            return new Tray(sop);
        }

        public async Task Destroy()
        {
            await Invoke<object>("destroy");
        }

        public async Task SetImage(string image)
        {
            await Invoke<object>("setImage", image);
        }

        public async Task SetImage(NativeImage image)
        {
            await Invoke<object>("setImage", image);
        }

        public async Task SetPressedImage(NativeImage image)
        {
            await Invoke<object>("setPressedImage", image);
        }

        public async Task SetToolTip(string tooltip)
        {
            await Invoke<object>("setToolTip", tooltip);
        }

        public async Task SetTitle(string title)
        {
            await Invoke<object>("setTitle", title);
        }

        public async Task SetHighlightMode(TrayHighlightMode mode)
        {
            await Invoke<object>("setHighlightMode", mode.ToString().ToLower());
        }

        public async Task DisplayBalloon(BalloonOptions options)
        {
            await Invoke<object>("displayBalloon", options);
        }

        public async Task PopUpContextMenu(Menu menu = null, Point? position = null)
        {
            await Invoke<object>("popUpContextMenu", menu, position.HasValue ? position.Value : Point.Empty);
        }

        public async Task SetContextMenu(Menu menu)
        {
            await Invoke<object>("setContextMenu", menu);
        }

        public async Task<Rectangle> GetBounds()
        {
            return await Invoke<Rectangle>("getBounds");
        }

        public async Task<bool> IsDestroyed()
        {
            return await Invoke<bool>("isDestroyed");
        }

    }

    public enum TrayHighlightMode
    {
        Selection,
        Always,
        Never
    }
}