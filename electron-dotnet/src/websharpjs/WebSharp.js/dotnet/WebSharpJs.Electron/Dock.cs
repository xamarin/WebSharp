using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Dock : NodeJsObject
    {

        protected Dock() : base() { }

        protected Dock(object scriptObject) : base(scriptObject)
        { }

        public Dock(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Dock(ScriptObjectProxy sop)
        {
            return new Dock(sop);
        }

        public async Task<int> Bounce(DockBounceType? bounceType)
        {
            if (bounceType != null && bounceType.HasValue)
            {
                return await Invoke<int>("bounce", bounceType.Value.ToString().ToLower());
            }
            else
            {
                return await Invoke<int>("bounce");
            }

        }

        public async Task CancelBounce(int id)
        {
            await Invoke<object>("cancelBounce", id);
        }

        public async Task DownloadFinished(string filePath)
        {
            await Invoke<object>("downloadFinished", filePath);
        }

        public async Task<bool> SetBadge(string text)
        {
            return await Invoke<bool>("setBadge", text);
        }

        public async Task<string> GetBadge()
        {
            return await Invoke<string>("getBadge");
        }

        public async Task Hide ()
        {
            await Invoke<object>("hide");
        }

        public async Task Show()
        {
            await Invoke<object>("show");
        }

        public async Task<bool> IsVisible()
        {
            return await Invoke<bool>("isVisible");
        }

        public async Task SetMenu(Menu menu)
        {
            await Invoke<object>("setMenu", menu);
        }

        public async Task SetIcon(NativeImage icon)
        {
            await Invoke<object>("setIcon", icon);
        }

        public async Task SetIcon(string icon)
        {
            await Invoke<object>("setIcon", icon);
        }
    }

    public enum DockBounceType
    {
        Critical,
        Informational
    }
}
