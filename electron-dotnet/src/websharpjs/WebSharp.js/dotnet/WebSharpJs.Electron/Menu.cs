using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Menu : NodeJsObject
    {
        protected override string ScriptProxy =>
                        @"function () { 

                            if (options && options.length > 0)
                            {
                                return Menu.buildFromTemplate(options);
                            }
                            else
                                return new Menu(); 
                        }()";
        protected override string Requires => require;

        string require = string.Empty;
        static readonly string mainRequire = @"const {Menu} = require('electron')";
        static readonly string remoteRequire = @"const {remote} = require('electron');
                                                const { Menu} = remote;"; 

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<Menu> CreateMain()
        {
            var proxy = new Menu();
            proxy.require = mainRequire;
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<Menu> CreateRemote()
        {
            var proxy = new Menu();
            proxy.require = remoteRequire;
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<Menu> BuildFromTemplate(MenuItemOptions[] template)
        {
            var proxy = new Menu();
            proxy.require = remoteRequire;
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(template);
            return proxy;

        }


        [ScriptableMember(ScriptAlias = "append")]
        public async void Append(MenuItem menuItem)
        {
            await Invoke<object>("Append", menuItem);
        }

        [ScriptableMember(ScriptAlias = "insert")]
        public async void Insert(int pos, MenuItem menuItem)
        {
            await Invoke<object>("Insert", pos, menuItem);
        }

        public async Task<ScriptObjectCollection<MenuItem>> Items()
        {
            return await GetProperty<ScriptObjectCollection<MenuItem>>("items"); ;
        }

        //[ScriptableMember(ScriptAlias = "items")]
        public object[] Itemss
        {
            get
            {
                return new object[0];// GetProperty<object[]>("Items");
            }
        }

        public async void Popup()
        {
            await Invoke<object>("popup");
        }
    }
}
