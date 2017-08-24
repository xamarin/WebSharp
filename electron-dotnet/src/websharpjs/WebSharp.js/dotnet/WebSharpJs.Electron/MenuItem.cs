using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{

    public class MenuItem : NodeJsObject
    {

        protected override string ScriptProxy => @"new MenuItem(options[0])";
        protected override string Requires => @"const {MenuItem} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<MenuItem> Create(MenuItemOptions options)
        {
            var proxy = new MenuItem();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(options);
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
        public ScriptObjectCallback<MenuItem, BrowserWindow, Event> Click { get; set; }

        [ScriptableMember(ScriptAlias = "role")]
        public string RoleDescription
        {
            get
            {
                if (Role == null || !Role.HasValue)
                    return string.Empty;

                return Role.Value.ToString().ToLower();
            }
        }

        public MenuItemRole? Role { get; set; }

        [ScriptableMember(ScriptAlias = "type", EnumValue = ConvertEnum.ToLower)]
        public MenuItemType? Type { get; set; }

        [ScriptableMember(ScriptAlias = "label")]
        public string Label { get; set; }

        [ScriptableMember(ScriptAlias = "sublabel")]
        public string SubLabel { get; set; }

        [ScriptableMember(ScriptAlias = "accelerator")]
        public string ShortCut { get { return (Accelerator != null) ? Accelerator.ToString() : null; } }

        public Accelerator Accelerator { get; set; }

        [ScriptableMember(ScriptAlias = "icon")]
        public object IconPathOrNativeImage
        {
            get
            {
                if (!string.IsNullOrEmpty(IconPath))
                    return IconPath;
                if (IconImage != null)
                    return IconImage;
                return null;
            }
        }

        public string IconPath { get; set; }

        public NativeImage IconImage { get; set; }

        [ScriptableMember(ScriptAlias = "enabled")]
        public bool Enabled { get; set; } = true;


        [ScriptableMember(ScriptAlias = "visible")]
        public bool Visible { get; set; } = true;

        [ScriptableMember(ScriptAlias = "checked")]
        public bool Checked { get; set; }

        [ScriptableMember(ScriptAlias = "submenu")]
        public object SubMenuMenuOrOptions
        {
            get
            {
                if (SubMenuOptions != null && SubMenuOptions.Length > 0)
                    return SubMenuOptions;
                if (SubMenu != null)
                    return SubMenu;
                return null;
            }
        }

        public MenuItemOptions[] SubMenuOptions { get; set; }

        public Menu SubMenu { get; set; }

        [ScriptableMember(ScriptAlias = "id")]
        public string Id { get; set; }

        [ScriptableMember(ScriptAlias = "position")]
        public string Position { get; set; }

    }
}
