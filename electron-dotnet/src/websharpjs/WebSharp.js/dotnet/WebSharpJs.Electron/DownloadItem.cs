using System;
using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class DownloadItem : EventEmitter
    {

        protected override string ScriptProxy => string.Empty;

        protected override string Requires => string.Empty;


        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        private DownloadItem() : base() { }
        private DownloadItem(object obj) : base(obj) { }

        private DownloadItem(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator DownloadItem(ScriptObjectProxy sop)
        {
            return new DownloadItem(sop);
        }

        public async Task Cancel()
        {
            await Invoke<object>("cancel");
        }

        public async Task<bool> CanResume()
        {
            return await Invoke<bool>("canResume");
        }

        public async Task<string> GetContentDisposition()
        {
            return await Invoke<string>("getContentDisposition");
        }

        public async Task<string> GetETag()
        {
            return await Invoke<string>("getETag");
        }

        public async Task<string> GetFilename()
        {
            return await Invoke<string>("getFilename");
        }

        public async Task<string> GetLastModifiedTime()
        {
            return await Invoke<string>("getLastModifiedTime");
        }

        public async Task<string> GetMimeType()
        {
            return await Invoke<string>("getMimeType");
        }

        public async Task<int> GetReceivedBytes()
        {
            return await Invoke<int>("getReceivedBytes");
        }

        public async Task<double> GetStartTime()
        {
            return await Invoke<double>("getStartTime");
        }

        public async Task<string> GetState()
        {
            return await Invoke<string>("getState");
        }

        public async Task<int> GetTotalBytes()
        {
            return await Invoke<int>("getTotalBytes");
        }

        public async Task<string> GetURL()
        {
            return await Invoke<string>("getURL");
        }

        public async Task<string[]> GetURLChain()
        {
            return await Invoke<string[]>("getURLChain");
        }

        public async Task<string> GetContentDisposition()
        {
            return await Invoke<string>("getContentDisposition");
        }

        public async Task<bool> HasUserGesture()
        {
            return await Invoke<bool>("hasUserGesture");
        }

        public async Task<bool> IsPaused()
        {
            return await Invoke<bool>("isPaused");
        }

        public async Task Pause()
        {
            await Invoke<object>("pause");
        }

        public async Task Resume()
        {
            await Invoke<object>("resume");
        }

        public async Task SetSavePath(string path)
        {
            await Invoke<object>("setSavePath", path);
        }

    }

    public enum DownloadItemState
    {
        Progressing,
        Interrupted,
        Completed,
        Cancelled
    }
}
