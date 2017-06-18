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

        public async Task Destroy()
        {
            await Invoke<object>("destroy");
        }

        public async Task Close()
        {
            await Invoke<object>("close");
        }

        public async Task Focus()
        {
            await Invoke<object>("focus");
        }

        public async Task Blur()
        {
            await Invoke<object>("blur");
        }

        public async Task<bool> IsFocused()
        {
            return await Invoke<bool>("isFocused");
        }

        public async Task<bool> IsDestroyed()
        {
            return await Invoke<bool>("isDestroyed");
        }

        public async Task Show()
        {
            await Invoke<object>("show");
        }

        public async Task ShowInactive()
        {
            await Invoke<object>("showInactive");
        }

        public async Task Hide()
        {
            await Invoke<object>("hide");
        }

        public async Task<bool> IsVisible()
        {
            return await Invoke<bool>("isVisible");
        }

        public async Task<bool> IsModal()
        {
            return await Invoke<bool>("isModal");
        }

        public async Task Maximize()
        {
            await Invoke<object>("maximize");
        }

        public async Task UnMaximize()
        {
            await Invoke<object>("unmaximize");
        }

        public async Task<bool> IsMaximized()
        {
            return await Invoke<bool>("isMaximized");
        }

        public async Task Minimize()
        {
            await Invoke<object>("minimize");
        }

        public async Task Restore()
        {
            await Invoke<object>("restore");
        }

        public async Task<bool> IsMinimized()
        {
            return await Invoke<bool>("isMinimized");
        }

        public async Task SetFullScreen(bool flag)
        {
            await Invoke<object>("setFullScreen", flag);
        }

        public async Task<bool> IsFullScreen()
        {
            return await Invoke<bool>("isFullScreen");
        }

        public async Task SetAspectRatio(float aspect, Size size = null)
        {
            if (size != null)
                await Invoke<object>("setAspectRatio", aspect, size );
            else
                await Invoke<object>("setAspectRatio", aspect );
        }

        public async Task PreviewFile(string path, string displayName = null)
        {
            if (displayName != null)
                await Invoke<object>("previewFile", path, displayName);
            else
                await Invoke<object>("previewFile", displayName);
        }

        public async Task CloseFilePreview()
        {
            await Invoke<object>("closeFilePreview");
        }

        public async Task SetBounds(Rectangle options, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                await Invoke<object>("setBounds", options, animate.Value);
            else
                await Invoke<object>("setBounds", options);
        }

        public async Task<Rectangle> GetBounds()
        {
            var obj = await Invoke<object>("getBounds");
            return new Rectangle(obj);
            
        }

        public async Task SetContentBounds(Rectangle options, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                await Invoke<object>("setContentBounds", options, animate.Value);
            else
                await Invoke<object>("setContentBounds", options);
        }

        public async Task<Rectangle> GetContentBounds()
        {
            var obj = await Invoke<object>("getContentBounds");
            return new Rectangle(obj);

        }

        public async Task SetSize(int width, int height, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                await Invoke<object>("setSize", width, height, animate.Value);
            else
                await Invoke<object>("setSize", width, height);
        }

        public async Task SetSize(Size size, bool? animate = null)
        {
            await SetSize((int)size.Width, (int)size.Height, animate);
        }

        public async Task<Size> GetSize()
        {
            var obj = await Invoke<object>("getSize");
            return new Size(obj);

        }

        public async Task SetContentSize(int width, int height, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                await Invoke<object>("setContentSize", width, height, animate.Value);
            else
                await Invoke<object>("setContentSize", width, height);
        }

        public async Task SetContentSize(Size size, bool? animate = null)
        {
            await SetContentSize((int)size.Width, (int)size.Height, animate);
        }

        public async Task<Size> GetContentSize()
        {
            var obj = await Invoke<object>("getContentSize");
            return new Size(obj);

        }

        public async Task SetMinimumSize(int width, int height)
        {
             await Invoke<object>("setMinimumSize", width, height);
        }

        public async Task SetMinimumSize(Size size)
        {
            await SetMinimumSize((int)size.Width, (int)size.Height);
        }

        public async Task<Size> GetMinimumSize()
        {
            var obj = await Invoke<object>("getMinimumSize");
            return new Size(obj);

        }

        public async Task SetMaximumSize(int width, int height)
        {
            await Invoke<object>("setMaximumSize", width, height);
        }

        public async Task SetMaximumSize(Size size)
        {
            await SetMaximumSize((int)size.Width, (int)size.Height);
        }

        public async Task<Size> GetMaximumSize()
        {
            var obj = await Invoke<object>("getMaximumSize");
            return new Size(obj);

        }

        public async Task SetResizable(bool resizable)
        {
            await Invoke<object>("setResizable", resizable);
        }

        public async Task<bool> IsResizable()
        {
            return await Invoke<bool>("isResizable");
        }

        public async Task SetMovable(bool movable)
        {
            await Invoke<object>("setMovable", movable);
        }

        public async Task<bool> IsMovable()
        {
            return await Invoke<bool>("isMovable");
        }

        public async Task SetMinimizable(bool minimizable)
        {
            await Invoke<object>("setMinimizable", minimizable);
        }

        public async Task<bool> IsMinimizable()
        {
            return await Invoke<bool>("isMinimizable");
        }

        public async Task SetMaximizable(bool maximizable)
        {
            await Invoke<object>("setMaximizable", maximizable);
        }

        public async Task<bool> IsMaximizable()
        {
            return await Invoke<bool>("isMaximizable");
        }

        public async Task SetFullScreenable(bool fullScreenable)
        {
            await Invoke<object>("setFullScreenable", fullScreenable);
        }

        public async Task<bool> IsFullScreenable()
        {
            return await Invoke<bool>("isFullScreenable");
        }

        public async Task SetClosable(bool closable)
        {
            await Invoke<object>("setClosable", closable);
        }

        public async Task<bool> IsClosable()
        {
            return await Invoke<bool>("isClosable");
        }

        public async Task SetAlwaysOnTop(bool flag, WindowLevel? level = null)
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

                await Invoke<object>("setAlwaysOnTop", flag, lvl);
            }
            else
                await Invoke<object>("setAlwaysOnTop", flag);
        }

        public async Task<bool> IsAlwaysOnTop()
        {
            return await Invoke<bool>("isAlwaysOnTop");
        }

        public async Task Center()
        {
            await Invoke<string>("center");
        }

        public async Task SetPosition(int x, int y, bool? animate = null)
        {
            if (animate != null && animate.HasValue)
                await Invoke<object>("setPosition", x, y, animate.Value);
            else
                await Invoke<object>("setPosition", x, y);
        }

        public async Task<Point> GetPosition()
        {
            var obj = await Invoke<object[]>("getPosition");
            return new Point(System.Convert.ToInt32(obj[0]), System.Convert.ToInt32(obj[1]));

        }

        public async Task SetProgressBar(float progress, ProgressBarMode? mode = null)
        {
            if (mode != null && mode.HasValue)
                await Invoke<object>("setProgressBar", progress, mode.Value);
            else
                await Invoke<object>("setProgressBar", progress);
        }

        public async Task<string> GetTitle()
        {
            return await Invoke<string>("getTitle");
        }
        public async Task SetTitle(string title)
        {
            await Invoke<object>("setTitle", title);
        }

        public async Task SetSheetOffset(int offsetX, int offsetY)
        {
            await Invoke<object>("setSheetOffset", offsetX, offsetY);
        }

        public async Task SetSheetOffset(Point size)
        {
            await SetSheetOffset((int)size.X, (int)size.Y);
        }

        public async Task FlashFrame(bool flag)
        {
            await Invoke<object>("flashFrame", flag);
        }

        public async Task SetSkipTaskbar(bool skip)
        {
            await Invoke<object>("setSkipTaskbar", skip);
        }

        public async Task SetKiosk(bool flag)
        {
            await Invoke<object>("setKiosk", flag);
        }

        public async Task<bool> IsKiosk()
        {
            return await Invoke<bool>("isKiosk");
        }

        public async Task<byte[]> GetNativeWindowHandle()
        {
            return await Invoke<byte[]>("getNativeWindowHandle");
        }

        public async Task HookWindowMessage(int message, ScriptObjectCallback callback)
        {
            await Invoke<object>("hookWindowMessage", message, callback);
        }

        public async Task<bool> IsWindowMessageHooked (int message)
        {
            return await Invoke<bool>("isWindowMessageHooked", message);
        }

        public async Task UnhookWindowMessage (int message)
        {
            await Invoke<object>("unhookWindowMessage", message);
        }

        public async Task UnhookAllWindowMessages ()
        {
            await Invoke<object>("unhookAllWindowMessages");
        }

        public async Task<string> GetRepresentedFilename()
        {
            return await Invoke<string>("getRepresentedFilename");
        }

        public async Task SetRepresentedFilename(string filename)
        {
            await Invoke<object>("setRepresentedFilename", filename);
        }

        public async Task SetDocumentEdited(bool edited)
        {
            await Invoke<object>("setDocumentEdited", edited);
        }

        public async Task<bool> IsDocumentEdited()
        {
            return await Invoke<bool>("isDocumentEdited");
        }

        public async Task FocusOnWebView()
        {
            await Invoke<object>("focusOnWebView");
        }

        public async Task BlurWebView()
        {
            await Invoke<object>("blurWebView");
        }

        public async Task CapturePage(Rectangle rect = null, ScriptObjectCallback<NativeImage> callback = null)
        {
            if (rect != null)
                await Invoke<object>("capturePage", rect, callback);
            else
                await Invoke<object>("capturePage", callback);
        }

        public async Task LoadURL(string url, LoadURLOptions urlOptions = null)
        {
            if (urlOptions == null)
                await Invoke<object>("loadURL", url);
            else
                await Invoke<object>("loadURL", url, urlOptions);
        }

        public async Task Reload()
        {
            await Invoke<object>("reload");
        }

        public async Task SetMenu(Menu menu)
        {
            if (menu == null)
                await Invoke<object>("setMenu", null);
            else
                await Invoke<object>("setMenu", menu);
        }

        public async Task SetOverlayIcon(NativeImage overlay, string description)
        {
            await Invoke<object>("setOverlayIcon", overlay, description ?? string.Empty);
        }

        public async Task SetHasShadow(bool hasShadow)
        {
            await Invoke<object>("setHasShadow", hasShadow);
        }

        public async Task<bool> IsHasShadow()
        {
            return await Invoke<bool>("hasShadow");
        }

        public async Task<bool> SetThumbarButtons(ThumbarButton[] buttons)
        {
            return await Invoke<bool>("setThumbarButtons", buttons);
        }

        public async Task SetThumbnailClip(Rectangle clip)
        {
            await Invoke<object>("setThumbnailClip", clip);
        }

        public async Task SetThumbnailToolTip(string tooltip)
        {
            await Invoke<object>("setThumbnailToolTip", tooltip);
        }

        public async Task SetAppDetails(AppDetailsOptions options)
        {
            await Invoke<object>("setAppDetails", options);
        }

        public async Task ShowDefinitionForSelection()
        {
            await Invoke<object>("showDefinitionForSelection");
        }

        public async Task SetIcon(NativeImage icon)
        {
            await Invoke<object>("setIcon", icon);
        }

        public async Task SetAutoHideMenuBar(bool hide)
        {
            await Invoke<object>("setAutoHideMenuBar", hide);
        }

        public async Task<bool> IsAutoHideMenuBar()
        {
            return await Invoke<bool>("isAutoHideMenuBar");
        }

        public async Task SetMenuBarVisibility(bool visible)
        {
            await Invoke<object>("setMenuBarVisibility", visible);
        }

        public async Task<bool> IsMenuBarVisible()
        {
            return await Invoke<bool>("isMenuBarVisible");
        }

        public async Task SetVisibleOnAllWorkspaces(bool visible)
        {
            await Invoke<object>("setVisibleOnAllWorkspaces", visible);
        }

        public async Task<bool> IsVisibleOnAllWorkspaces()
        {
            return await Invoke<bool>("isVisibleOnAllWorkspaces");
        }

        public async Task SetIgnoreMouseEvents(bool ignore)
        {
            await Invoke<object>("setIgnoreMouseEvents", ignore);
        }

        public async Task SetContentProtection(bool enable)
        {
            await Invoke<object>("setContentProtection", enable);
        }

        public async Task SetFocusable(bool focusable)
        {
            await Invoke<object>("setFocusable", focusable);
        }

 
        public async Task SetParentWindow(BrowserWindow parent)
        {
            await Invoke<object>("setParentWindow", parent);
        }

        public async Task<BrowserWindow> GetParentWindow()
        {
            return await Invoke<BrowserWindow>("getParentWindow");
        }

        public async Task<ScriptObjectCollection<BrowserWindow>> GetChildWindows()
        {
            return await Invoke<ScriptObjectCollection<BrowserWindow>>("getChildWindows");
        }

        public async Task SetVibrancyType(VibrancyType type)
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
            await Invoke<object>("setVibrancy", vType);
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
    public struct ProgressBarOptions
    {
        [ScriptableMember(ScriptAlias = "mode", EnumValue = ConvertEnum.ToLower)]
        public ProgressBarMode? Mode { get; set; }
    }

    public enum ProgressBarMode
    {
        None,
        Normal,
        InDeterminate,
        Error

    }

    [ScriptableType]
    public struct AppDetailsOptions
    {
        [ScriptableMember(ScriptAlias = "appId")]
        public string AppId { get; set; }
        [ScriptableMember(ScriptAlias = "appIconPath")]
        public string AppIconPath { get; set; }
        [ScriptableMember(ScriptAlias = "appIconIndex")]
        public int AppIconIndex { get; set; }
        [ScriptableMember(ScriptAlias = "relaunchCommand")]
        public string RelaunchCommand { get; set; }
        [ScriptableMember(ScriptAlias = "relaunchDisplayName")]
        public string RelaunchDisplayName { get; set; }
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
