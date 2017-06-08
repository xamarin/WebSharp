using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Screen : EventEmitter
    {
        protected override string ScriptProxy => @"require('electron').screen;";
        protected override string Requires => @"";

        static Screen proxy;

        public static async Task<Screen> Instance()
        {
            if (proxy == null)
            {
                proxy = new Screen();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected Screen() : base() { }

        protected Screen(object scriptObject) : base(scriptObject)
        { }

        public Screen(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Screen(ScriptObjectProxy sop)
        {
            return new Screen(sop);
        }

        public async Task<Point> GetCursorScreenPoint()
        {
            return await Invoke<Point>("getCursorScreenPoint");
        }

        public async Task<Display> GetPrimaryDisplay()
        {
            return await Invoke<Display>("getPrimaryDisplay");
        }

        public async Task<Display[]> GetAllDisplays()
        {
            return await Invoke<Display[]>("getAllDisplays");
        }

        public async Task<Display> GetDisplayNearestPoint(Point point)
        {
            return await Invoke<Display>("getDisplayNearestPoint", point);
        }

        public async Task<Display> GetDisplayMatching(Rectangle rect)
        {
            return await Invoke<Display>("getDisplayMatching", rect);
        }

        public async Task<int> GetMenuBarHeight()
        {
            return await Invoke<int>("getMenuBarHeight");
        }

    }

    public enum TouchSupport
    {
        Available,
        UnAvailable,
        Unknown
    }
    
}
