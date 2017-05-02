using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class App : EventEmitter
    {
        protected override string ScriptProxy => @"app;";
        protected override string Requires => @"const {app} = require('electron');";

        static App proxy;

        public static async Task<App> Instance()
        {
            if (proxy == null)
            {
                proxy = new App();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected App() : base() { }

        protected App(object scriptObject) : base(scriptObject)
        { }

        public App(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator App(ScriptObjectProxy sop)
        {
            return new App(sop);
        }

        public async Task<object> Quit()
        {
            return await Invoke<object>("quit");
        }

        public async Task<object> Exit(int exitCode = 0)
        {
            return await Invoke<object>("exit", exitCode);
        }

        public async Task<bool> IsReady()
        {
            return await Invoke<bool>("isReady");
        }

        public async Task<object> Focus()
        {
            return await Invoke<object>("focus");
        }

        public async Task<object> Show()
        {
            return await Invoke<object>("show");
        }

        public async Task<object> Hide()
        {
            return await Invoke<object>("hide");
        }

        public async Task<string> GetAppPath()
        {
            return await Invoke<string>("getAppPath");
        }

        public async Task<string> GetPath(AppPathName name)
        {
            var pathName = name.ToString().ToLower();
            switch (name)
            {
                case AppPathName.AppData:
                    pathName = "appData";
                    break;
                case AppPathName.UserData:
                    pathName = "userData";
                    break;
                case AppPathName.PepperFlashSystemPlugin:
                    pathName = "pepperFlashSystemPlugin";
                    break;
            }
            return await Invoke<string>("getPath", pathName);
        }

        public async Task<object> SetPath(AppPathName name, string path)
        {
            var pathName = name.ToString().ToLower();
            switch (name)
            {
                case AppPathName.AppData:
                    pathName = "appData";
                    break;
                case AppPathName.UserData:
                    pathName = "userData";
                    break;
                case AppPathName.PepperFlashSystemPlugin:
                    pathName = "pepperFlashSystemPlugin";
                    break;
            }
            return await Invoke<object>("setPath", pathName, path);
        }

        public async Task<string> GetVersion()
        {
            return await Invoke<string>("getVersion");
        }

        public async Task<string> GetName()
        {
            return await Invoke<string>("getName");
        }

        public async Task<object> SetName(string name)
        {
            return await Invoke<object>("setName", name);
        }

        public async Task<string> GetLocale()
        {
            return await Invoke<string>("getLocale");
        }

        public async Task<object> AddRecentDocument(string path)
        {
            return await Invoke<object>("addRecentDocument", path);
        }

        public async Task<object> ClearRecentDocuments()
        {
            return await Invoke<object>("clearRecentDocuments");
        }

        public async Task<bool> SetAsDefaultProtocolClient(string protocol, string path = null, string[] args = null)
        {
            return await Invoke<bool>("setAsDefaultProtocolClient", protocol, path, args);
        }

        public async Task<bool> RemoveAsDefaultProtocolClient(string protocol, string path = null, string[] args = null)
        {
            return await Invoke<bool>("removeAsDefaultProtocolClient", protocol, path, args);
        }

        public async Task<bool> IsDefaultProtocolClient(string protocol, string path = null, string[] args = null)
        {
            return await Invoke<bool>("isDefaultProtocolClient", protocol, path, args);
        }

        public async Task<object> SetAppUserModelId(string id)
        {
            return await Invoke<object>("setAppUserModelId", id);
        }

        public async Task<bool> IsUnityRunning()
        {
            return await Invoke<bool>("isUnityRunning");
        }

        public async Task<bool> IsAccessibilitySupportEnabled()
        {
            return await Invoke<bool>("isAccessibilitySupportEnabled");
        }

        public async Task<LoginItemSettings> GetLoginItemSettings()
        {
            var settings = await Invoke<object>("getLoginItemSettings");
            var lis = new LoginItemSettings();
            ScriptObjectHelper.DictionaryToScriptableType((System.Collections.Generic.IDictionary<string, object>)settings, lis);
            return lis;
        }

        public async Task<object> SetLoginItemSettings(LoginItemSettings settings)
        {
            return await Invoke<object>("setLoginItemSettings", settings);
        }


        public async Task<object> SetAboutPanelOptions(AboutPanelOptions options)
        {
            return await Invoke<object>("setAboutPanelOptions", options);
        }
    }

    public enum AppPathName
    {
        Home,
        AppData,
        UserData,
        Temp,
        Exe,
        Module,
        Desktop,
        Documents,
        Downloads,
        Music,
        Pictures,
        Videos,
        PepperFlashSystemPlugin
    }

    [ScriptableType]
    public struct AboutPanelOptions
    {
        [ScriptableMember(ScriptAlias = "applicationName")]
        public string ApplicationName { get; set; }
        [ScriptableMember(ScriptAlias = "applicationVersion")]
        public string ApplicationVersion { get; set; }
        [ScriptableMember(ScriptAlias = "copyright")]
        public string Copyright { get; set; }
        [ScriptableMember(ScriptAlias = "credits")]
        public string Credits { get; set; }
        [ScriptableMember(ScriptAlias = "version")]
        public string Version { get; set; }
    }

    [ScriptableType]
    public struct LoginItemSettings
    {
        [ScriptableMember(ScriptAlias = "openAtLogin")]
        public bool OpenAtLogin { get; set; }
        [ScriptableMember(ScriptAlias = "openAsHidden")]
        public bool OpenAsHidden { get; set; }
        [ScriptableMember(ScriptAlias = "wasOpenedAtLogin")]
        public bool? WasOpenedAtLogin { get; set; }
        [ScriptableMember(ScriptAlias = "wasOpenedAsHidden")]
        public bool? wasOpenedAsHidden { get; set; }
        [ScriptableMember(ScriptAlias = "restoreState")]
        public bool? restoreState { get; set; }
    }


}
