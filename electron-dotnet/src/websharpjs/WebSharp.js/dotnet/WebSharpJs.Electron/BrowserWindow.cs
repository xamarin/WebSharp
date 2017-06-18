using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;
using System;

namespace WebSharpJs.Electron
{
    public class BrowserWindow : EventEmitter
    {
        static readonly string bwScriptProxy = @"function () { 
                            if (options && options.length > 0)
                            {

                                if (options[0] === 'BrowserWindow')
                                    return BrowserWindow;

                                return new BrowserWindow(options[0]);
                            }
                            else
                                return new BrowserWindow();
                        }()";

        static readonly string bwRequires = @"const {BrowserWindow} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        protected override string ScriptProxy => bwScriptProxy;
        protected override string Requires => bwRequires;

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<BrowserWindow> Create(BrowserWindowOptions options = null)
        {
            var proxy = new BrowserWindow();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            if (options != null)
                scriptProxy = await proxy.Initialize(options);
            else
                scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<ScriptObjectCollection<BrowserWindow>> GetAllWindows()
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<ScriptObjectCollection<BrowserWindow>>("getAllWindows");
        }

        public static async Task<BrowserWindow> GetFocusedWindow()
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<BrowserWindow>("getFocusedWindow");
        }

        public static async Task<BrowserWindow> FromId(int id)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<BrowserWindow>("fromId", id);
        }

        public static async Task<BrowserWindow> FromWebContents(WebContents webContents)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<BrowserWindow>("fromWebContents", webContents);
        }

        public static async Task<string> AddDevToolsExtension(string path)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<string>("addDevToolsExtension", path);
        }

        public static async Task<object> RemoveDevToolsExtension(string name)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<object>("removeDevToolsExtension", name);
        }

        public static async Task<object> GetDevToolsExtensions()
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(bwScriptProxy, bwRequires);

            await scriptProxy.GetProxyObject("BrowserWindow");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<object>("getDevToolsExtensions");
        }

        private BrowserWindow() : base() { }
        private BrowserWindow(object obj) : base(obj) { }

        private BrowserWindow(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator BrowserWindow(ScriptObjectProxy sop)
        {
            return new BrowserWindow(sop);
        }

        public async Task<WebContents> GetWebContents()
        {
            return await GetProperty<WebContents>("webContents");
        }

        public async Task<int> GetId()
        {
            return await GetProperty<int>("id");
        }

        public async Task<object> Destroy()
        {
            return await Invoke<object>("destroy");
        }

        public async Task<object> Close()
        {
            return await Invoke<object>("close");
        }

        public async Task<object> Focus()
        {
            return await Invoke<object>("focus");
        }

        public async Task<object> Blur()
        {
            return await Invoke<object>("blur");
        }

        public async Task<bool> IsFocused()
        {
            return await Invoke<bool>("isFocused");
        }

        public async Task<bool> IsDestroyed()
        {
            return await Invoke<bool>("isDestroyed");
        }

        public async Task<object> Show()
        {
            return await Invoke<object>("show");
        }

        public async Task<object> ShowInactive()
        {
            return await Invoke<object>("showInactive");
        }

        public async Task<object> Hide()
        {
            return await Invoke<object>("hide");
        }

        public async Task<bool> IsVisible()
        {
            return await Invoke<bool>("isVisible");
        }

        public async Task<bool> IsModal()
        {
            return await Invoke<bool>("isModal");
        }

        public async Task<object> Maximize()
        {
            return await Invoke<object>("maximize");
        }

        public async Task<object> UnMaximize()
        {
            return await Invoke<object>("unmaximize");
        }

        public async Task<bool> IsMaximized()
        {
            return await Invoke<bool>("isMaximized");
        }

        public async Task<object> Minimize()
        {
            return await Invoke<object>("minimize");
        }

        public async Task<object> Restore()
        {
            return await Invoke<object>("restore");
        }

        public async Task<bool> IsMinimized()
        {
            return await Invoke<bool>("isMinimized");
        }

        public async Task<object> SetFullScreen(bool flag)
        {
            return await Invoke<object>("setFullScreen", flag);
        }

        public async Task<bool> IsFullScreen()
        {
            return await Invoke<bool>("isFullScreen");
        }

        public async Task<object> SetAspectRatio(float aspect, Size size = null)
        {
            if (size != null)
                return await Invoke<object>("setAspectRatio", aspect, size );
            else
                return await Invoke<object>("setAspectRatio", aspect );
        }

        public async Task<object> PreviewFile(string path, string displayName = null)
        {
            if (displayName != null)
                return await Invoke<object>("previewFile", path, displayName);
            else
                return await Invoke<object>("previewFile", displayName);
        }

        public async Task<object> CloseFilePreview()
        {
            return await Invoke<object>("closeFilePreview");
        }

        public async Task<object> SetBounds(Rectangle options, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                return await Invoke<object>("setBounds", options, animate.Value);
            else
                return await Invoke<object>("setBounds", options);
        }

        public async Task<Rectangle> GetBounds()
        {
            var obj = await Invoke<object>("getBounds");
            return new Rectangle(obj);
            
        }

        public async Task<object> SetContentBounds(Rectangle options, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                return await Invoke<object>("setContentBounds", options, animate.Value);
            else
                return await Invoke<object>("setContentBounds", options);
        }

        public async Task<Rectangle> GetContentBounds()
        {
            var obj = await Invoke<object>("getContentBounds");
            return new Rectangle(obj);

        }

        public async Task<object> SetSize(int width, int height, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                return await Invoke<object>("setSize", width, height, animate.Value);
            else
                return await Invoke<object>("setSize", width, height);
        }

        public async Task<object> SetSize(Size size, bool? animate = null)
        {
            return await SetSize((int)size.Width, (int)size.Height, animate);
        }

        public async Task<Size> GetSize()
        {
            var obj = await Invoke<object>("getSize");
            return new Size(obj);

        }

        public async Task<object> SetContentSize(int width, int height, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                return await Invoke<object>("setContentSize", width, height, animate.Value);
            else
                return await Invoke<object>("setContentSize", width, height);
        }

        public async Task<object> SetContentSize(Size size, bool? animate = null)
        {
            return await SetContentSize((int)size.Width, (int)size.Height, animate);
        }

        public async Task<Size> GetContentSize()
        {
            var obj = await Invoke<object>("getContentSize");
            return new Size(obj);

        }

        public async Task<object> SetMinimumSize(int width, int height)
        {
             return await Invoke<object>("setMinimumSize", width, height);
        }

        public async Task<object> SetMinimumSize(Size size)
        {
            return await SetMinimumSize((int)size.Width, (int)size.Height);
        }

        public async Task<Size> GetMinimumSize()
        {
            var obj = await Invoke<object>("getMinimumSize");
            return new Size(obj);

        }

        public async Task<object> SetMaximumSize(int width, int height)
        {
            return await Invoke<object>("setMaximumSize", width, height);
        }

        public async Task<object> SetMaximumSize(Size size)
        {
            return await SetMaximumSize((int)size.Width, (int)size.Height);
        }

        public async Task<Size> GetMaximumSize()
        {
            var obj = await Invoke<object>("getMaximumSize");
            return new Size(obj);

        }

        public async Task<object> SetResizable(bool resizable)
        {
            return await Invoke<object>("setResizable", resizable);
        }

        public async Task<bool> IsResizable()
        {
            return await Invoke<bool>("isResizable");
        }

        public async Task<object> SetMovable(bool movable)
        {
            return await Invoke<object>("setMovable", movable);
        }

        public async Task<bool> IsMovable()
        {
            return await Invoke<bool>("isMovable");
        }

        public async Task<object> SetMinimizable(bool minimizable)
        {
            return await Invoke<object>("setMinimizable", minimizable);
        }

        public async Task<bool> IsMinimizable()
        {
            return await Invoke<bool>("isMinimizable");
        }

        public async Task<object> SetMaximizable(bool maximizable)
        {
            return await Invoke<object>("setMaximizable", maximizable);
        }

        public async Task<bool> IsMaximizable()
        {
            return await Invoke<bool>("isMaximizable");
        }

        public async Task<object> SetFullScreenable(bool fullScreenable)
        {
            return await Invoke<object>("setFullScreenable", fullScreenable);
        }

        public async Task<bool> IsFullScreenable()
        {
            return await Invoke<bool>("isFullScreenable");
        }

        public async Task<object> SetClosable(bool closable)
        {
            return await Invoke<object>("setClosable", closable);
        }

        public async Task<bool> IsClosable()
        {
            return await Invoke<bool>("isClosable");
        }

        public async Task<object> SetAlwaysOnTop(bool flag, WindowLevel? level = null)
        {
            if (level != null && level.HasValue)
            {

                string lvl = string.Empty;
                switch (level.Value)
                {
                    case WindowLevel.TornOffMenu:
                        lvl = "torn-off-menu";
                        break;
                    case WindowLevel.ModalPanel:
                        lvl = "modal-panel";
                        break;
                    case WindowLevel.MainMenu:
                        lvl = "main-menu";
                        break;
                    case WindowLevel.PopupMenu:
                        lvl = "pop-up-menu";
                        break;
                    case WindowLevel.ScreenSaver:
                        lvl = "screen-saver";
                        break;
                    default:
                        lvl = level.Value.ToString().ToLower();
                        break;
                }

                return await Invoke<object>("setAlwaysOnTop", flag, lvl);
            }
            else
                return await Invoke<object>("setAlwaysOnTop", flag);
        }

        public async Task<bool> IsAlwaysOnTop()
        {
            return await Invoke<bool>("isAlwaysOnTop");
        }

        public async Task<object> Center()
        {
            return await Invoke<string>("center");
        }

        public async Task<object> SetPosition(int x, int y, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                return await Invoke<object>("setPosition", x, y, animate.Value);
            else
                return await Invoke<object>("setPosition", x, y);
        }

        public async Task<Point> GetPosition()
        {
            var obj = await Invoke<object[]>("getPosition");
            return new Point(System.Convert.ToInt32(obj[0]), System.Convert.ToInt32(obj[1]));

        }

        public async Task<string> GetTitle()
        {
            return await Invoke<string>("getTitle");
        }
        public async Task<object> SetTitle(string title)
        {
            return await Invoke<object>("setTitle", title);
        }

        public async Task<object> SetSheetOffset(int offsetX, int offsetY)
        {
            return await Invoke<object>("setSheetOffset", offsetX, offsetY);
        }

        public async Task<object> SetSheetOffset(Point size)
        {
            return await SetSheetOffset((int)size.X, (int)size.Y);
        }

        public async Task<object> FlashFrame(bool flag)
        {
            return await Invoke<object>("flashFrame", flag);
        }

        public async Task<object> SetSkipTaskbar(bool skip)
        {
            return await Invoke<object>("setSkipTaskbar", skip);
        }

        public async Task<object> SetKiosk(bool flag)
        {
            return await Invoke<object>("setKiosk", flag);
        }

        public async Task<bool> IsKiosk()
        {
            return await Invoke<bool>("isKiosk");
        }

        public async Task<byte[]> GetNativeWindowHandle()
        {
            return await Invoke<byte[]>("getNativeWindowHandle");
        }

        public async Task<object> HookWindowMessage(int message, ScriptObjectCallback callback)
        {
            return await Invoke<object>("hookWindowMessage", message, callback);
        }

        public async Task<bool> IsWindowMessageHooked (int message)
        {
            return await Invoke<bool>("isWindowMessageHooked", message);
        }

        public async Task<object> UnhookWindowMessage (int message)
        {
            return await Invoke<object>("unhookWindowMessage", message);
        }

        public async Task<object> UnhookAllWindowMessages ()
        {
            return await Invoke<object>("unhookAllWindowMessages");
        }

        public async Task<string> GetRepresentedFilename()
        {
            return await Invoke<string>("getRepresentedFilename");
        }

        public async Task<object> SetRepresentedFilename(string filename)
        {
            return await Invoke<object>("setRepresentedFilename", filename);
        }

        public async Task<object> SetDocumentEdited(bool edited)
        {
            return await Invoke<object>("setDocumentEdited", edited);
        }

        public async Task<bool> IsDocumentEdited()
        {
            return await Invoke<bool>("isDocumentEdited");
        }

        public async Task<object> FocusOnWebView()
        {
            return await Invoke<object>("focusOnWebView");
        }

        public async Task<object> BlurWebView()
        {
            return await Invoke<object>("blurWebView");
        }

        public async Task<object> CapturePage(Rectangle rect = null, ScriptObjectCallback<NativeImage> callback = null)
        {
            if (rect != null)
                return await Invoke<object>("capturePage", rect, callback);
            else
                return await Invoke<object>("capturePage", callback);
        }

        public async Task<object> LoadURL(string url, LoadURLOptions urlOptions = null)
        {
            if (urlOptions == null)
                return await Invoke<object>("loadURL", url);
            else
                return await Invoke<object>("loadURL", url, urlOptions);
        }

        public async Task<object> Reload()
        {
            return await Invoke<object>("reload");
        }

        public async Task<object> SetMenu(Menu menu)
        {
            if (menu == null)
                return await Invoke<object>("setMenu", null);
            else
                return await Invoke<object>("setMenu", menu);
        }

        public async Task<object> SetOverlayIcon(NativeImage overlay, string description)
        {
            return await Invoke<object>("setOverlayIcon", overlay, description ?? string.Empty);
        }

        public async Task<object> SetHasShadow(bool hasShadow)
        {
            return await Invoke<object>("setHasShadow", hasShadow);
        }

        public async Task<bool> IsHasShadow()
        {
            return await Invoke<bool>("hasShadow");
        }

        public async Task<object> SetThumbnailClip(Rectangle clip)
        {
            return await Invoke<object>("setThumbnailClip", clip);
        }

        public async Task<object> SetThumbnailToolTip(string tooltip)
        {
            return await Invoke<object>("setThumbnailToolTip", tooltip);
        }

        public async Task<object> ShowDefinitionForSelection()
        {
            return await Invoke<object>("showDefinitionForSelection");
        }

        public async Task<object> SetIcon(NativeImage icon)
        {
            return await Invoke<object>("setIcon", icon);
        }

        public async Task<object> SetAutoHideMenuBar(bool hide)
        {
            return await Invoke<object>("setAutoHideMenuBar", hide);
        }

        public async Task<bool> IsAutoHideMenuBar()
        {
            return await Invoke<bool>("isAutoHideMenuBar");
        }

        public async Task<object> SetMenuBarVisibility(bool visible)
        {
            return await Invoke<object>("setMenuBarVisibility", visible);
        }

        public async Task<bool> IsMenuBarVisible()
        {
            return await Invoke<bool>("isMenuBarVisible");
        }

        public async Task<object> SetVisibleOnAllWorkspaces(bool visible)
        {
            return await Invoke<object>("setVisibleOnAllWorkspaces", visible);
        }

        public async Task<bool> IsVisibleOnAllWorkspaces()
        {
            return await Invoke<bool>("isVisibleOnAllWorkspaces");
        }

        public async Task<object> SetIgnoreMouseEvents(bool ignore)
        {
            return await Invoke<object>("setIgnoreMouseEvents", ignore);
        }

        public async Task<object> SetContentProtection(bool enable)
        {
            return await Invoke<object>("setContentProtection", enable);
        }

        public async Task<object> SetFocusable(bool focusable)
        {
            return await Invoke<object>("setFocusable", focusable);
        }

 
        public async Task<object> SetParentWindow(BrowserWindow parent)
        {
            return await Invoke<object>("setParentWindow", parent);
        }

        public async Task<BrowserWindow> GetParentWindow()
        {
            return await Invoke<BrowserWindow>("getParentWindow");
        }

        public async Task<ScriptObjectCollection<BrowserWindow>> GetChildWindows()
        {
            return await Invoke<ScriptObjectCollection<BrowserWindow>>("getChildWindows");
        }

        public async Task<object> SetVibrancyType(VibrancyType type)
        {
            string vType = string.Empty;

            switch (type)
            {
                case VibrancyType.AppearanceBased:
                    vType = "appearance-based";
                    break;
                case VibrancyType.MediumLight:
                    vType = "medium-light";
                    break;
                case VibrancyType.UltraDark:
                    vType = "ultra-dark";
                    break;
                default:
                    vType = type.ToString().ToLower();
                    break;

            }
            return await Invoke<object>("setVibrancy", vType);
        }

    }

    public enum VibrancyType
    {
        AppearanceBased,
        Light,
        Dark,
        Titlebar,
        Selection,
        Menu,
        Popover,
        Sidebar,
        MediumLight,
        UltraDark
    }

    [ScriptableType]
    public struct DevToolsExtensions
    {
        [ScriptableMember(ScriptAlias = "name")]
        public string Name { get; set; }
        [ScriptableMember(ScriptAlias = "value")]
        public string Value { get; set; }
    }   

    [ScriptableType]
    public class BrowserWindowOptions
    {
        /**
         * Window’s width in pixels.
         * Default: 800.
         */
        //width?: number;
        [ScriptableMember(ScriptAlias = "width")]
        public int? Width { get; set; }
        /**
		 * Window’s height in pixels.
		 * Default: 600.
		 */
        //height?: number;
        [ScriptableMember(ScriptAlias = "height")]
        public int? Height { get; set; }
        /**
		 * Window’s left offset from screen.
		 * Default: center the window.
		 */
        //x?: number;
        [ScriptableMember(ScriptAlias = "x")]
        public int? X { get; set; }
        /**
		 * Window’s top offset from screen.
		 * Default: center the window.
		 */
        //y?: number;
        [ScriptableMember(ScriptAlias = "y")]
        public int? Y { get; set; }

        /**
        * The width and height would be used as web page’s size, which means
        * the actual window’s size will include window frame’s size and be slightly larger.
        * Default: false.
        */
        //useContentSize?: boolean;
        [ScriptableMember(ScriptAlias = "useContentSize")]
        public bool? UseContentSize { get; set; }
        /**
		 * Show window in the center of the screen.
		 * Default: true
		 */
        //center?: boolean;
        [ScriptableMember(ScriptAlias = "center")]
        public bool? Center { get; set; }
        /**
		 * Window’s minimum width.
		 * Default: 0.
		 */
        //minWidth?: number;
        [ScriptableMember(ScriptAlias = "minWidth")]
        public int? MinWidth { get; set; }
        /**
		 * Window’s minimum height.
		 * Default: 0.
		 */
        //minHeight?: number;
        [ScriptableMember(ScriptAlias = "minHeight")]
        public int? MinHeight { get; set; }

        /**
		 * Window’s maximum width.
		 * Default: no limit.
		 */
        //maxWidth?: number;
        [ScriptableMember(ScriptAlias = "maxWidth")]
        public int? MaxWidth { get; set; }

        /**
		 * Window’s maximum height.
		 * Default: no limit.
		 */
        //maxHeight?: number;
        [ScriptableMember(ScriptAlias = "maxHeight")]
        public int? MaxHeight { get; set; }

        /**
		 * Whether window is resizable.
		 * Default: true.
		 */
        //resizable?: boolean;
        [ScriptableMember(ScriptAlias = "resizable")]
        public bool? Resizable { get; set; }

        /**
		 * Whether window is movable.
		 * Note: This is not implemented on Linux.
		 * Default: true.
		 */
        //movable?: boolean;
        [ScriptableMember(ScriptAlias = "movable")]
        public bool? Movable { get; set; }

        /**
		 * Whether window is minimizable.
		 * Note: This is not implemented on Linux.
		 * Default: true.
		 */
        //minimizable?: boolean;
        [ScriptableMember(ScriptAlias = "minizable")]
        public bool? Minimizable { get; set; }

        /**
		 * Whether window is maximizable.
		 * Note: This is not implemented on Linux.
		 * Default: true.
		 */
        //maximizable?: boolean;
        [ScriptableMember(ScriptAlias = "maximizable")]
        public bool? Maximizable { get; set; }

        /**
		 * Whether window is closable.
		 * Note: This is not implemented on Linux.
		 * Default: true.
		 */
        //closable?: boolean;
        [ScriptableMember(ScriptAlias = "closable")]
        public bool? Closable { get; set; }

        /**
		 * Whether the window can be focused.
		 * On Windows setting focusable: false also implies setting skipTaskbar: true.
		 * On Linux setting focusable: false makes the window stop interacting with wm,
		 * so the window will always stay on top in all workspaces.
		 * Default: true.
		 */
        //focusable?: boolean;
        [ScriptableMember(ScriptAlias = "focusable")]
        public bool? Focusable { get; set; }

        /**
		 * Whether the window should always stay on top of other windows.
		 * Default: false.
		 */
        //alwaysOnTop?: boolean;
        [ScriptableMember(ScriptAlias = "alwaysOnTop")]
        public bool? AlwaysOnTop { get; set; }

        /**
		 * Whether the window should show in fullscreen.
		 * When explicitly set to false the fullscreen button will be hidden or disabled on macOS.
		 * Default: false.
		 */
        //fullscreen?: boolean;
        [ScriptableMember(ScriptAlias = "fullscreen")]
        public bool? FullScreen { get; set; }

        /**
		 * Whether the window can be put into fullscreen mode.
		 * On macOS, also whether the maximize/zoom button should toggle full screen mode or maximize window.
		 * Default: true.
		 */
        //fullscreenable?: boolean;
        [ScriptableMember(ScriptAlias = "fullscreenable")]
        public bool? FullScreenable { get; set; }

        /**
		 * Whether to show the window in taskbar.
		 * Default: false.
		 */
        //skipTaskbar?: boolean;
        [ScriptableMember(ScriptAlias = "skipTaskbar")]
        public bool? SkipTaskbar { get; set; }

        /**
		 * The kiosk mode.
		 * Default: false.
		 */
        //kiosk?: boolean;
        [ScriptableMember(ScriptAlias = "kiosk")]
        public bool? Kiosk { get; set; }

        /**
		 * Default window title.
		 * Default: "Electron".
		 */
        //title?: string;
        [ScriptableMember(ScriptAlias = "title")]
        public string Title { get; set; }

        [ScriptableMember(ScriptAlias = "icon")]
        public object Icon
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
        /**
        * The window icon, when omitted on Windows the executable’s icon would be used as window icon.
        */
        //icon?: NativeImage|string;
        public string IconPath { get; set; }

        public NativeImage IconImage { get; set; }


        /**
         * Whether window should be shown when created.
         * Default: true.
         */
        [ScriptableMember(ScriptAlias = "show")]
        public bool? Show { get; set; }
        /**
		 * Specify false to create a Frameless Window.
		 * Default: true.
		 */
        //frame?: boolean;
        [ScriptableMember(ScriptAlias = "frame")]
        public bool? Frame { get; set; }
        /**
		 * Specify parent window.
		 * Default: null.
		 */
        //parent?: BrowserWindow;
        [ScriptableMember(ScriptAlias = "parent")]
        public BrowserWindow Parent { get; set; }
        /**
		 * Whether this is a modal window. This only works when the window is a child window.
		 * Default: false.
		 */
        //modal?: boolean;
        [ScriptableMember(ScriptAlias = "modal")]
        public bool? Modal { get; set; }
        /**
		 * Whether the web view accepts a single mouse-down event that simultaneously activates the window.
		 * Default: false.
		 */
        //acceptFirstMouse?: boolean;
        [ScriptableMember(ScriptAlias = "acceptFirstMouse")]
        public bool? AcceptFirstMouse { get; set; }
        /**
		 * Whether to hide cursor when typing.
		 * Default: false.
		 */
        //disableAutoHideCursor?: boolean;
        [ScriptableMember(ScriptAlias = "disableAutoHideCursor")]
        public bool? DisableAutoHideCursor { get; set; }
        /**
		 * Auto hide the menu bar unless the Alt key is pressed.
		 * Default: true.
		 */
        //autoHideMenuBar?: boolean;
        [ScriptableMember(ScriptAlias = "autoHideMenuBar")]
        public bool? AutoHideMenuBar { get; set; }
        /**
		 * Enable the window to be resized larger than screen.
		 * Default: false.
		 */
        //enableLargerThanScreen?: boolean;
        [ScriptableMember(ScriptAlias = "enableLargerThanScreen")]
        public bool? EnableLargerThanScreen { get; set; }
        /**
		 * Window’s background color as Hexadecimal value, like #66CD00 or #FFF or #80FFFFFF (alpha is supported).
		 * Default: #FFF (white).
		 */
        //backgroundColor?: string;
        [ScriptableMember(ScriptAlias = "backgroundColor")]
        public string BackgroundColor { get; set; }
        /**
		 * Whether window should have a shadow.
		 * Note: This is only implemented on macOS.
		 * Default: true.
		 */
        //hasShadow?: boolean;
        [ScriptableMember(ScriptAlias = "hasShadow")]
        public bool? HasShadow { get; set; }
        /**
		 * Forces using dark theme for the window.
		 * Note: Only works on some GTK+3 desktop environments.
		 * Default: false.
		 */
        //darkTheme?: boolean;
        [ScriptableMember(ScriptAlias = "darkTheme")]
        public bool? DarkTheme { get; set; }
        /**
		 * Makes the window transparent.
		 * Default: false.
		 */
        //transparent?: boolean;
        [ScriptableMember(ScriptAlias = "transparent")]
        public bool? Transparent { get; set; }
        /**
		 * The type of window, default is normal window.
		 */
        //type?: BrowserWindowType;
        [ScriptableMember(ScriptAlias = "type", EnumValue = ConvertEnum.ToLower)]
        public BrowserWindowType? Type { get; set; }

        [ScriptableMember(ScriptAlias = "titleBarStyle")]
        public string TitleBarStyleDescription
        {
            get
            {
                if (TitleBarStyle == null)
                    return null;
                if (TitleBarStyle == Electron.TitleBarStyle.HiddenInset)
                    return "hidden-inset";
                return TitleBarStyle.ToString().ToLower();
            }
        }
        /**
		 * The style of window title bar.
		 */
        //titleBarStyle?: 'default' | 'hidden' | 'hidden-inset';
        public TitleBarStyle? TitleBarStyle { get; set; }
        /**
		 * Use WS_THICKFRAME style for frameless windows on Windows
		 */
        //thickFrame?: boolean;
        [ScriptableMember(ScriptAlias = "thickFrame")]
        public bool? ThickFrame { get; set; }
        /**
		 * Add a type of vibrancy effect to the window, only on macOS
		 */
        [ScriptableMember(ScriptAlias = "vibrancy")]
        public string VibrancyDescription
        {
            get
            {
                if (Vibrancy == null)
                    return null;

                switch (Vibrancy)
                {
                    case VibrancyType.AppearanceBased:
                        return "appearance-based";
                    case VibrancyType.MediumLight:
                        return "medium-light";
                    case VibrancyType.UltraDark:
                        return "ultra-dark";
                    default:
                        return Vibrancy.ToString().ToLower();
                }
            }
        }

        public VibrancyType? Vibrancy { get; set; }

        [ScriptableMember(ScriptAlias = "zoomToPageWidth")]
        public bool? ZoomToPageWidth { get; set; }

        [ScriptableMember(ScriptAlias = "tabbingIdentifier")]
        public string TabbingIdentifier { get; set; }

        public WebPreferences WebPreferences { get; set; }
    }

    public enum BrowserWindowType
    {
        Desktop,
        Dock,
        Toolbar,
        Splash,
        Notification,
        Textured
    }

    public enum TitleBarStyle
    {
        Default,
        Hidden,
        HiddenInset,
    }


    public enum WindowLevel
    {
        Normal,
        Floating,
        TornOffMenu,
        ModalPanel,
        MainMenu,
        Status,
        PopupMenu,
        ScreenSaver,
        Dock
    }

    public class LoadURLOptions
    {
        /**
		 * HTTP Referrer URL.
		 */
        string HttpReferrer { get; set; }
		/**
		 * User agent originating the request.
		 */
		string UserAgent { get; set; }
		/**
		 * Extra headers separated by "\n"
		 */
		string ExtraHeaders { get; set; }
		/**
		 * POST data
		 */
		//postData?: (UploadRawData | UploadFileSystem | UploadBlob)[];
	}

    [ScriptableType]
    public struct WebPreferences
    {
        [ScriptableMember(ScriptAlias = "devTools")]
        public bool? DevTools { get; set; }

        [ScriptableMember(ScriptAlias = "nodeIntegration")]
        public bool? NodeIntegration { get; set; }

        [ScriptableMember(ScriptAlias = "nodeIntegrationInWorker")]
        public bool? NodeIntegrationInWorker { get; set; }

        [ScriptableMember(ScriptAlias = "preload")]
        public string Preload { get; set; }

        //[ScriptableMember(ScriptAlias = "session")]
        //public Session Session { get; set; }

        [ScriptableMember(ScriptAlias = "partition")]
        public string Partition { get; set; }

        [ScriptableMember(ScriptAlias = "zoomFactor")]
        public float? ZoomFactor { get; set; }

        [ScriptableMember(ScriptAlias = "javascript")]
        public bool? Javascript { get; set; }

        [ScriptableMember(ScriptAlias = "webSecurity")]
        public bool? WebSecurity { get; set; }

        [ScriptableMember(ScriptAlias = "allowDisplayingInsecureContent")]
        public bool? AllowDisplayingInsecureContent { get; set; }

        [ScriptableMember(ScriptAlias = "allowRunningInsecureContent")]
        public bool? AllowRunningInsecureContent { get; set; }

        [ScriptableMember(ScriptAlias = "images")]
        public bool? Images { get; set; }

        [ScriptableMember(ScriptAlias = "textAreasAreResizable")]
        public bool? TextAreasAreResizable { get; set; }

        [ScriptableMember(ScriptAlias = "webgl")]
        public bool? WebGL { get; set; }

        [ScriptableMember(ScriptAlias = "webaudio")]
        public bool? WebAudio { get; set; }

        [ScriptableMember(ScriptAlias = "plugins")]
        public bool? Plugins { get; set; }

        [ScriptableMember(ScriptAlias = "experimentalFeatures")]
        public bool? ExperimentalFeatures { get; set; }

        [ScriptableMember(ScriptAlias = "experimentalCanvasFeatures")]
        public bool? ExperimentalCanvasFeatures { get; set; }

        [ScriptableMember(ScriptAlias = "directWrite")]
        public bool? DirectWrite { get; set; }

        [ScriptableMember(ScriptAlias = "scrollBounce")]
        public bool? ScrollBounce { get; set; }

        [ScriptableMember(ScriptAlias = "blinkFeatures")]
        public string BlinkFeatures { get; set; }

        [ScriptableMember(ScriptAlias = "disableBlinkFeatures")]
        public bool? DisableBlinkFeatures { get; set; }

        [ScriptableMember(ScriptAlias = "defaultFontFamily")]
        public string DefaultFontFamily { get; set; }

        [ScriptableMember(ScriptAlias = "defaultFontSize")]
        public float? DefaultFontSize { get; set; }

        [ScriptableMember(ScriptAlias = "defaultMonospaceFontSize")]
        public float? DefaultMonospaceFontSize { get; set; }

        [ScriptableMember(ScriptAlias = "minimumFontSize")]
        public float? MinimumFontSize { get; set; }

        [ScriptableMember(ScriptAlias = "defaultEncoding")]
        public string DefaultEncoding { get; set; }

        [ScriptableMember(ScriptAlias = "backgroundThrottling")]
        public bool? BackgroundThrottling { get; set; }

        [ScriptableMember(ScriptAlias = "offscreen")]
        public bool? Offscreen { get; set; }

        [ScriptableMember(ScriptAlias = "sandbox")]
        public bool? Sandbox { get; set; }

        [ScriptableMember(ScriptAlias = "contextIsolation")]
        public bool? ContextIsolation { get; set; }

        [ScriptableMember(ScriptAlias = "nativeWindowOpen")]
        public bool? NativeWindowOpen { get; set; }

        [ScriptableMember(ScriptAlias = "webviewTag")]
        public bool? WebviewTag { get; set; }
    }
}
