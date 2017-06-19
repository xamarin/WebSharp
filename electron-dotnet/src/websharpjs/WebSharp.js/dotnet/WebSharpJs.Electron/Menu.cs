using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Menu : NodeJsObject
    {
        static readonly string menuScriptProxy = @"function () { 

                            if (options && options.length > 0)
                            {
                                if (options[0] === 'Menu.getApplicationMenu')
                                    return Menu.getApplicationMenu();

                                if (options[0] === 'Menu.setApplicationMenu')
                                    return Menu.setApplicationMenu(options[1]);

                                if (options[0] === 'Menu.sendActionToFirstResponder')
                                    return Menu.sendActionToFirstResponder(options[1]);

                                return Menu.buildFromTemplate(options);
                            }
                            else
                                return new Menu(); 
                        }()";

        static readonly string menuRequires = @"const {Menu} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        protected override string ScriptProxy => menuScriptProxy;

        protected override string Requires => menuRequires;


        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<Menu> Create()
        {
            var proxy = new Menu();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<Menu> BuildFromTemplate(MenuItemOptions[] template)
        {
            var proxy = new Menu();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(template);
            return proxy;

        }

        public static async Task<Menu> GetApplicationMenu()
        {
            var proxy = new Menu();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("Menu.getApplicationMenu");
            return proxy;
        }

        public static async Task SetApplicationMenu(Menu menu)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(menuScriptProxy, menuRequires);

            await scriptProxy.GetProxyObject("Menu.setApplicationMenu", menu);
        }

        public static async Task SendActionToFirstResponder(string action)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(menuScriptProxy, menuRequires);

            await scriptProxy.GetProxyObject("Menu.sendActionToFirstResponder", action);
        }

        private Menu() : base() { }
        private Menu(object obj) : base(obj) { }

        private Menu(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Menu(ScriptObjectProxy sop)
        {
            return new Menu(sop);
        }


        [ScriptableMember(ScriptAlias = "append")]
        public async Task<object> Append(MenuItem menuItem)
        {
            return await Invoke<object>("Append", menuItem);
        }

        [ScriptableMember(ScriptAlias = "insert")]
        public async Task<object> Insert(int pos, MenuItem menuItem)
        {
            return await Invoke<object>("Insert", pos, menuItem);
        }

        public async Task<ScriptObjectCollection<MenuItem>> Items()
        {
            return await GetProperty<ScriptObjectCollection<MenuItem>>("items"); ;
        }

        public async Task Popup(PopupOptions? popupOptions = null)
        {
            if (popupOptions != null && popupOptions.HasValue)
                await Invoke<object>("popup", popupOptions.Value);
            else
                await Invoke<object>("popup");
        }

        public async Task Popup(BrowserWindow browserWindow, PopupOptions? popupOptions = null)
        {
            if (popupOptions != null && popupOptions.HasValue)
                await Invoke<object>("popup", browserWindow, popupOptions.Value);
            else
                await Invoke<object>("popup", browserWindow);
        }

        public async Task ClosePopup(BrowserWindow browserWindow = null)
        {
            if (browserWindow == null)
                await Invoke<object>("closePopup");
            else
                await Invoke<object>("closePopup", browserWindow);
        }

    }

    [ScriptableType]
    public struct PopupOptions
    {
        [ScriptableMember(ScriptAlias = "x")]
        public int? X { get; set; }
        [ScriptableMember(ScriptAlias = "y")]
        public int? Y { get; set; }
        [ScriptableMember(ScriptAlias = "async")]
        public bool? Async { get; set; }
        [ScriptableMember(ScriptAlias = "positioningItem")]
        public int? PositioningItem { get; set; }

    }

}
