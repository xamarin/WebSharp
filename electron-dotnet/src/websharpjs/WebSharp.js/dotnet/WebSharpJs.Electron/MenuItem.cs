using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{

    public class MenuItem : NodeJsObject
    {

        protected override string ScriptProxy => @"new MenuItem(options[0])";
        protected override string Requires => require;

        string require = string.Empty;
        static readonly string mainRequire = @"const {MenuItem} = require('electron')";
        static readonly string remoteRequire = @"const {MenuItem} = require('electron').remote;";

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<MenuItem> CreateMain(MenuItemOptions options)
        {
            var proxy = new MenuItem();
            proxy.require = mainRequire;
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(options);
            return proxy;

        }

        public static async Task<MenuItem> CreateRemote(MenuItemOptions options)
        {
            var proxy = new MenuItem();
            proxy.require = remoteRequire;
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            await proxy.Initialize(options);
            scriptProxy = (NodeObjectProxy)proxy.ScriptObjectProxy;
            return proxy;
        }

        private MenuItem() : base() { }
        private MenuItem(object obj) : base(obj) { }

        private MenuItem(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator MenuItem(ScriptObjectProxy sop)
        {
            return new MenuItem(sop);
        }

        [ScriptableMember(ScriptAlias = "enabled")]
        public Task<bool> Enabled
        {
            get { return GetEnabled(); }
        }

        [ScriptableMember(ScriptAlias = "enabled")]
        public Task<bool> GetEnabled() { return GetProperty<bool>("Enabled"); }
        [ScriptableMember(ScriptAlias = "enabled")]
        public Task<bool> SetEnabled(bool value) { return SetProperty("Enabled", value); }

        [ScriptableMember(ScriptAlias = "visible")]
        public Task<bool> Visible
        {
            get { return GetVisible(); }
        }
        [ScriptableMember(ScriptAlias = "visible")]
        public Task<bool> GetVisible() { return GetProperty<bool>("Visble"); }
        [ScriptableMember(ScriptAlias = "visible")]
        public Task<bool> SetVisible(bool value) { return SetProperty("Visible", value); }

        public Task<string> GetLabel() { return GetProperty<string>("label"); }
        public Task<bool> SetLabel(string value) { return SetProperty("label", value); }

        [ScriptableMember(ScriptAlias = "checked")]
        public Task<bool> GetChecked() { return GetProperty<bool>("checked"); }
        [ScriptableMember(ScriptAlias = "checked")]
        public Task<bool> SetChecked(bool value) { return SetProperty("checked", value); }
    }

    //'normal' | 'separator' | 'submenu' | 'checkbox' | 'radio';
    public enum MenuItemType
    {
        Normal,
        Separator,
        Submenu,
        Checkbox,
        Radio
    }

    public enum MenuItemRole
    {
        None,
        Undo,
        Redo,
        Cut,
        Copy,
        Paste,
        PasteAndMatchStyle,
        SelectAll,
        Delete,
        Minimize,
        Close,
        Quit,
        ToggleFullScreen,
        ResetZoom,
        ZoomIn,
        ZoomOut,
        Reload,
        ToggleDevTools,
        About,
        Hide,
        HideOthers,
        UnHide,
        StartSpeaking,
        StopSpeaking,
        Front,
        Zoom,
        Window,
        Help,
        Services
    }

    [ScriptableType]
    public class MenuItemOptions
    {

        /// <summary>
        /// Will be called with click(menuItem, browserWindow, event) when the menu item is clicked.
        /// </summary>
        [ScriptableMember(ScriptAlias = "click")]
        public Func<object[], Task<object>> Click { get; set; }

        [ScriptableMember(ScriptAlias = "role")]
        public string RoleDescription
        {
            get
            {
                if (Role == MenuItemRole.None)
                    return string.Empty;

                return Role.ToString().ToLower();
            }
        }

        //role String(optional) - Define the action of the menu item, when specified the click property will be ignored.
        public MenuItemRole Role { get; set; }

        //type String (optional) - Can be normal, separator, submenu, checkbox or radio.
        [ScriptableMember(ScriptAlias = "type", EnumValue = ConvertEnum.ToLower)]
        public MenuItemType Type { get; set; }

        //label String - (optional)
        [ScriptableMember(ScriptAlias = "label")]
        public string Label { get; set; }

        //sublabel String - (optional)
        [ScriptableMember(ScriptAlias = "sublabel")]
        public string SubLabel { get; set; }

        //accelerator Accelerator(optional)
        //icon(NativeImage | String) (optional)
        [ScriptableMember(ScriptAlias = "icon")]
        public string Icon { get; set; }

        //enabled Boolean(optional) - If false, the menu item will be greyed out and unclickable.
        [ScriptableMember(ScriptAlias = "enabled")]
        public bool Enabled { get; set; } = true;


        //visible Boolean (optional) - If false, the menu item will be entirely hidden.
        [ScriptableMember(ScriptAlias = "visible")]
        public bool Visible { get; set; } = true;

        //checked Boolean(optional) - Should only be specified for checkbox or radio type menu items.
        [ScriptableMember(ScriptAlias = "checked")]
        public bool Checked { get; set; }

        //submenu(MenuItemConstructorOptions[] | Menu) (optional) - Should be specified for submenu type menu items.If submenu is specified, the type: 'submenu' can be omitted.If the value is not a Menu then it will be automatically converted to one using Menu.buildFromTemplate.
        //id String(optional) - Unique within a single menu.If defined then it can be used as a reference to this item by the position attribute.
        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }

        //position String (optional) - This field allows fine-grained definition of the specific location within a given menu.
        [ScriptableMember(ScriptAlias = "position")]
        public string Position { get; set; }

    }
}
