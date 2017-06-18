using System.Threading.Tasks;

using WebSharpJs.Script;
using WebSharpJs.NodeJS;
using System;

namespace WebSharpJs.Electron
{
    public class WebContents : EventEmitter
    {
        static readonly string wcScriptProxy = @"webContents;";

        static readonly string wcRequires = @"const {webContents} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        protected override string ScriptProxy => wcScriptProxy;
        protected override string Requires => wcRequires;

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<ScriptObjectCollection<WebContents>> GetAllWebContents()
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(wcScriptProxy, wcRequires);

            await scriptProxy.GetProxyObject("WebContents");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<ScriptObjectCollection<WebContents>>("getAllWebContents");
        }

        public static async Task<WebContents> GetFocusedWebContents()
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(wcScriptProxy, wcRequires);

            await scriptProxy.GetProxyObject("WebContents");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<WebContents>("getFocusedWebContents");
        }

        public static async Task<WebContents> FromId(int id)
        {
            if (scriptProxy == null)
                scriptProxy = new NodeObjectProxy(wcScriptProxy, wcRequires);

            await scriptProxy.GetProxyObject("WebContents");
            var scriptObject = new ScriptObject();
            scriptObject.ScriptObjectProxy = scriptProxy;
            return await scriptObject.Invoke<WebContents>("fromId", id);
        }

        private WebContents() : base() { }
        private WebContents(object obj) : base(obj) { }

        private WebContents(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator WebContents(ScriptObjectProxy sop)
        {
            return new WebContents(sop);
        }

        public async Task<object> LoadURL(string url, LoadURLOptions urlOptions = null)
        {
            if (urlOptions == null)
                return await Invoke<object>("loadURL", url);
            else
                return await Invoke<object>("loadURL", url, urlOptions);
        }

        public async Task<object> DownloadURL(string url)
        {
            return await Invoke<string>("downloadURL", url);
        }

        public async Task<string> GetURL()
        {
            return await Invoke<string>("getURL");
        }

        public async Task<string> GetTitle()
        {
            return await Invoke<string>("getTitle");
        }

        public async Task<bool> IsDestroyed()
        {
            return await Invoke<bool>("isDestroyed");
        }

        public async Task<bool> IsFocused()
        {
            return await Invoke<bool>("isFocused");
        }

        public async Task<bool> IsLoading()
        {
            return await Invoke<bool>("isLoading");
        }

        public async Task<bool> IsLoadingMainFrame()
        {
            return await Invoke<bool>("isLoadingMainFrame");
        }

        public async Task<bool> IsWaitingForResponse()
        {
            return await Invoke<bool>("isWaitingForResponse");
        }

        public async Task<object> Stop()
        {
            return await Invoke<object>("stop");
        }

        public async Task<object> Reload()
        {
            return await Invoke<object>("reload");
        }

        public async Task<object> ReloadIgnoringCache()
        {
            return await Invoke<object>("reloadIgnoringCache");
        }

        public async Task<object> CanGoBack()
        {
            return await Invoke<object>("canGoBack");
        }

        public async Task<object> CanGoForward()
        {
            return await Invoke<object>("canGoForward");
        }

        public async Task<object> CanGoToOffset()
        {
            return await Invoke<object>("canGoToOffset");
        }

        public async Task<object> ClearHistory()
        {
            return await Invoke<object>("clearHistory");
        }

        public async Task<object> GoBack()
        {
            return await Invoke<object>("goBack");
        }

        public async Task<object> GoForward()
        {
            return await Invoke<object>("goForward");
        }

        public async Task<object> GoToIndex(int index)
        {
            return await Invoke<object>("goToIndex", index);
        }

        public async Task<object> GoToOffset(int offset)
        {
            return await Invoke<object>("goToOffset", offset);
        }

        public async Task<bool> IsCrashed()
        {
            return await Invoke<bool>("isCrashed");
        }


        public async Task<object> SetUserAgent(string userAgent)
        {
            return await Invoke<object>("setUserAgent", userAgent);
        }

        public async Task<string> GetUserAgent()
        {
            return await Invoke<string>("getUserAgent");
        }

        public async Task<object> InsertCSS(string css)
        {
            return await Invoke<object>("insertCSS", css);
        }

        public async Task<object> SetAudioMuted(bool muted)
        {
            return await Invoke<object>("setAudioMuted", muted);
        }

        public async Task<bool> IsAudioMuted()
        {
            return await Invoke<bool>("isAudioMuted");
        }

        public async Task<object> SetZoomFactor(float factor)
        {
            return await Invoke<object>("setZoomFactor", factor);
        }

        public async Task<object> GetZoomFactor(ScriptObjectCallback<float> callback)
        {
            return await Invoke<object>("getZoomFactor", callback);
        }

        public async Task<object> SetZoomLevel(float level)
        {
            return await Invoke<object>("setZoomLevel", level);
        }

        public async Task<object> GetZoomLevel(ScriptObjectCallback<float> callback)
        {
            return await Invoke<object>("getZoomLevel", callback);
        }

        public async Task<object> SetVisualZoomLevelLimits(float minimum, float maximum)
        {
            return await Invoke<object>("setVisualZoomLevelLimits", minimum, maximum);
        }

        public async Task<object> SetLayoutZoomLevelLimits(float minimum, float maximum)
        {
            return await Invoke<object>("setLayoutZoomLevelLimits", minimum, maximum);
        }

        public async Task<object> Undo()
        {
            return await Invoke<object>("undo");
        }

        public async Task<object> Redo()
        {
            return await Invoke<object>("redo");
        }

        public async Task<object> Cut()
        {
            return await Invoke<object>("cut");
        }

        public async Task<object> Copy()
        {
            return await Invoke<object>("copy");
        }

        public async Task<object> CopyImageAt(int x, int y)
        {
            return await Invoke<object>("copyImagAt", x, y);
        }

        public async Task<object> Paste()
        {
            return await Invoke<object>("paste");
        }

        public async Task<object> PasteAndMatchStyle()
        {
            return await Invoke<object>("pasteAndMatchStyle");
        }

        public async Task<object> Delete()
        {
            return await Invoke<object>("delete");
        }

        public async Task<object> SelectAll()
        {
            return await Invoke<object>("selectAll");
        }

        public async Task<object> Unselect()
        {
            return await Invoke<object>("unselect");
        }

        public async Task<object> Replace(string text)
        {
            return await Invoke<object>("replace", text);
        }

        public async Task<object> ReplaceMisspelling(string text)
        {
            return await Invoke<object>("replaceMisspelling", text);
        }

        public async Task<object> InsertText(string text)
        {
            return await Invoke<object>("insertText", text);
        }

        public async Task<object> FindInPage(string text, FindInPageOptions? options = null)
        {
            if (options != null && options.HasValue)
                return await Invoke<object>("findInPage", text, options.Value);
            else
                return await Invoke<object>("findInPage", text);
        }

        public async Task<object> StopFindInPage(StopFindInPageAction action)
        {
            string aType = string.Empty;
            //type StopFindInPageAtion = 'clearSelection' | 'keepSelection' | 'activateSelection';
            switch (action)
            {
                case StopFindInPageAction.ClearSelection:
                    aType = "clearSelection-based";
                    break;
                case StopFindInPageAction.KeepSelection:
                    aType = "keepSelection";
                    break;
                case StopFindInPageAction.ActiveSelection:
                    aType = "activateSelection";
                    break;
                default:
                    aType = action.ToString().ToLower();
                    break;

            }
            return await Invoke<object>("stopFindInPage", aType);
        }

        public async Task<object> HasServiceWorker(ScriptObjectCallback<bool> hasServiceWorker)
        {
            return await Invoke<object>("hasServiceWorker", hasServiceWorker);
        }

        public async Task<object> UnregisterServiceWorker(ScriptObjectCallback<bool> fullfilled)
        {
            return await Invoke<object>("unregisterServiceWorker", fullfilled);
        }

        public async Task Print(PrintOptions? options = null)
        {
            if (options != null && options.HasValue)
                await Invoke<object>("print", options.Value);
            else
                await Invoke<object>("print");
        }

        /**
        * Prints windows' web page as PDF with Chromium's preview printing custom settings.
        */
        //printToPDF(options: PrintToPDFOptions, callback: (error: Error, data: Buffer) => void): void;
        public async Task PrintToPDF(PrintToPDFOptions options, ScriptObjectCallback<Error, byte[]> callback)
        {
            await Invoke<object>("printToPDF", options, callback);
        }

        public async Task AddWorkSpace(string path)
        {
            await Invoke<object>("addWorkSpace", path);
        }

        public async Task RemoveWorkSpace(string path)
        {
            await Invoke<object>("removeWorkSpace", path);
        }

        // devtools
        /**
		 * Opens the developer tools.
		 */
        public async Task OpenDevTools (DevToolsMode? mode = null)
        {
            if (mode != null && mode.HasValue)
                await Invoke<object>("openDevTools", new { mode = mode.ToString().ToLower() });
            else
                await Invoke<object>("openDevTools");
        }

        /**
		 * Closes the developer tools.
		 */
        //closeDevTools(): void;
        public async Task CloseDevTools()
        {
            await Invoke<object>("closeDevTools");
        }
        /**
		 * Returns whether the developer tools are opened.
		 */
        //isDevToolsOpened(): boolean;
        public async Task<bool> IsDevToolsOpened()
        {
            return await Invoke<bool>("isDevToolsOpened");
        }
        /**
		 * Returns whether the developer tools are focussed.
		 */
        //isDevToolsFocused(): boolean;
        public async Task<bool> IsDevToolsFocused()
        {
            return await Invoke<bool>("isDevToolsFocused");
        }

        public async Task ToggleDevTools ()
        {
            await Invoke<object>("toggleDevTools");
        }

        public async Task InspectElement(int x, int y)
        {
            await Invoke<object>("inspectElement", x, y);
        }

        public async Task InspectServiceWorker()
        {
            await Invoke<object>("inspectServiceWorker");
        }

        public async Task Send(string channel, params object[] args )
        {
            await Invoke<object>("send", channel, args);
        }

        public async Task EnableDeviceEmulation(DeviceEmulationParameters parameters)
        {
            await Invoke<object>("enableDeviceEmulation", parameters);
        }

        public async Task DisableDeviceEmulation()
        {
            await Invoke<object>("disableDeviceEmulation");
        }

        public async Task beginFrameSubscription(ScriptObjectCallback<byte[], Rectangle> callback)
        {
            await Invoke<bool>("beginFrameSubscription", callback);
        }

        public async Task beginFrameSubscription(bool onlyDirty, ScriptObjectCallback<byte[], Rectangle> callback)
        {
            await Invoke<bool>("beginFrameSubscription", onlyDirty, callback);
        }

        public async Task EndFrameSubscription()
        {
            await Invoke<object>("endFrameSubscription");
        }

        public async Task StartDrag(Item item)
        {
            await Invoke<object>("startDrag", item);
        }

        public async Task<bool> SavePage(string fullpath, SavePageType saveType, ScriptObjectCallback<Error> callback)
        {
            return await Invoke<bool>("SavePage", fullpath, saveType.ToString(), callback);
        }

        public async Task<object> ShowDefinitionForSelection()
        {
            return await Invoke<object>("showDefinitionForSelection");
        }

        public async Task SetSize(SizeOptions options)
        {
            await Invoke<object>("setSize", options);
        }

        public async Task<bool> IsOffscreen()
        {
            return await Invoke<bool>("isOffscreen");
        }

        public async Task<object> StartPainting()
        {
            return await Invoke<object>("startPainting");
        }

        public async Task<object> StopPainting()
        {
            return await Invoke<object>("stopPainting");
        }

        public async Task<object> SetFrameRate(float fps)
        {
            return await Invoke<object>("setFrameRate", fps);
        }

        public async Task<float> GetFrameRate()
        {
            return await Invoke<float>("getFrameRate");
        }

        public async Task<object> Invalidate()
        {
            return await Invoke<object>("invalidate");
        }

        public async Task<WebRTCIPHandlingPolicy> GetWebRTCIPHandlingPolicy()
        {

            WebRTCIPHandlingPolicy policy = WebRTCIPHandlingPolicy.Default;

            var stringPolicy = await Invoke<object>("getWebRTCIPHandlingPolicy");
            switch (stringPolicy)
            {
                case "default":
                    policy = WebRTCIPHandlingPolicy.Default;
                    break;
                case "default_public_interface_only":
                    policy = WebRTCIPHandlingPolicy.DefaultPublicInterfaceOnly;
                    break;
                case "default_public_and_private_interfaces":
                    policy = WebRTCIPHandlingPolicy.DefaultPublicAndPrivateInterfaces;
                    break;
                case "disable_non_proxied_udp":
                    policy = WebRTCIPHandlingPolicy.DisableNonProxiedUdp;
                    break;
            }

            return policy;

        }

        public async Task SetWebRTCIPHandlingPolicy(WebRTCIPHandlingPolicy policy)
        {
            string stringPolicy = string.Empty;

            switch (policy)
            {
                case WebRTCIPHandlingPolicy.Default:
                    stringPolicy = "default";
                    break;
                case WebRTCIPHandlingPolicy.DefaultPublicInterfaceOnly:
                    stringPolicy = "default_public_interface_only";
                    break;
                case WebRTCIPHandlingPolicy.DefaultPublicAndPrivateInterfaces:
                    stringPolicy = "default_public_and_private_interfaces";
                    break;
                case WebRTCIPHandlingPolicy.DisableNonProxiedUdp:
                    stringPolicy = "disable_non_proxied_udp";
                    break;
                default:
                    stringPolicy = policy.ToString().ToLower();
                    break;

            }
            await Invoke<object>("setWebRTCIPHandlingPolicy", stringPolicy);
        }

        public async Task<int> GetId()
        {
            return await GetProperty<int>("id");
        }

        public async Task<object> CapturePage(ScriptObjectCallback<NativeImage> image)
        {
            return await Invoke<object>("capturePage", image);
        }

        public async Task<object> CapturePage(Rectangle rect, ScriptObjectCallback<NativeImage> image)
        {
            return await Invoke<object>("capturePage", rect, image);
        }

        public async Task<WebContents> HostWebContents()
        {
            return await Invoke<WebContents>("hostWebContents");
        }

        public async Task<WebContents> DevToolsWebContents()
        {
            return await Invoke<WebContents>("devToolsWebContents");
        }

        //public async Task<Debugger> Debugger()
        //{
        //    return await Invoke<Debugger>("debugger");
        //}


    }

    public enum DevToolsMode
    {
        Right,
        Bottom,
        Undocked,
        Detach
    }

    [ScriptableType]
    public struct FindInPageOptions
    {
        /**
		 * Whether to search forward or backward, defaults to true
		 */
        //forward?: boolean;
        [ScriptableMember(ScriptAlias = "forward")]
        public bool? Forward { get; set; }
        /**
		 * Whether the operation is first request or a follow up, defaults to false.
		 */
        //findNext?: boolean;
        [ScriptableMember(ScriptAlias = "findNext")]
        public bool? FindNext { get; set; }
        /**
		 * Whether search should be case-sensitive, defaults to false.
		 */
        //matchCase?: boolean;
        [ScriptableMember(ScriptAlias = "matchCase")]
        public bool? MatchCase { get; set; }
        /**
		 * Whether to look only at the start of words. defaults to false.
		 */
        //wordStart?: boolean;
        [ScriptableMember(ScriptAlias = "wordStart")]
        public bool? WordStart { get; set; }
        /**
		 * When combined with wordStart, accepts a match in the middle of a word
		 * if the match begins with an uppercase letter followed by a lowercase
		 * or non-letter. Accepts several other intra-word matches, defaults to false.
		 */
        //medialCapitalAsWordStart?: boolean;
        [ScriptableMember(ScriptAlias = "medialCapitalAsWordStart")]
        public bool? MedialCapitalAsWordStart { get; set; }
    }

    [ScriptableType]
    public struct PrintOptions
    {
        /**
		 * Don't ask user for print settings.
		 * Defaults: false.
		 */
        //silent?: boolean;
        [ScriptableMember(ScriptAlias = "silent")]
        public bool? Silent { get; set; }

        /**
		 * Also prints the background color and image of the web page.
		 * Defaults: false.
		 */
        //printBackground?: boolean;
        [ScriptableMember(ScriptAlias = "printBackground")]
        public bool? PrintBackground { get; set; }
    }

    public enum StopFindInPageAction
    {
        ClearSelection,
        KeepSelection,
        ActiveSelection
    }

    public enum ScreenPosition
    {
        Desktop,
        Mobile
    }

    [ScriptableType]
    public struct DeviceEmulationParameters
    {
        [ScriptableMember(ScriptAlias = "screenPosition", EnumValue = ConvertEnum.ToLower)]
        public ScreenPosition? ScreenPosition { get; set; }
        [ScriptableMember(ScriptAlias = "screenSize")]
        public Size ScreenSize { get; set; }
        [ScriptableMember(ScriptAlias = "viewPosition")]
        public Point ViewPosition { get; set; }
        [ScriptableMember(ScriptAlias = "deviceScaleFactor")]
        public float DeviceScaleFactor { get; set; }
        [ScriptableMember(ScriptAlias = "viewSize")]
        public Size ViewSize { get; set; }
        [ScriptableMember(ScriptAlias = "fitToView")]
        public bool? FitToView { get; set; }
        [ScriptableMember(ScriptAlias = "offset")]
        public Point Offset { get; set; }
        [ScriptableMember(ScriptAlias = "scale")]
        public float? Scale { get; set; }
    }
    [ScriptableType]
    public struct PrintToPDFOptions
    {
        [ScriptableMember(ScriptAlias = "marginsType", EnumValue = ConvertEnum.ToLower)]
        public MarginsType? MarginsType { get; set; }
        [ScriptableMember(ScriptAlias = "pageSize", EnumValue = ConvertEnum.Default)]
        public PageSize? PageSize { get; set; }
        [ScriptableMember(ScriptAlias = "printBackground")]
        public bool? PrintBackground { get; set; }
        [ScriptableMember(ScriptAlias = "printSelectionOnly")]
        public bool? PrintSelectionOnly { get; set; }
        [ScriptableMember(ScriptAlias = "landscape")]
        public bool? Landscape { get; set; }

    }

    public enum MarginsType
    {
        Default,
        None,
        Minimum
    }

    public enum PageSize
    {
        A3,
        A4,
        A5,
        Legal,
        Letter,
        Tabloid
    }

    [ScriptableType]
    public struct SizeOptions
    {
        [ScriptableMember(ScriptAlias = "normal")]
        public Normal Normal { get; set; }

    }

    public enum SavePageType
    {
        HTMLOnly,
        HTMLComplete,
        MHTML
    }

    [ScriptableType]
    public struct Item
    {
        [ScriptableMember(ScriptAlias = "file")]
        public string File { get; set; }
        [ScriptableMember(ScriptAlias = "icon")]
        public NativeImage Icon { get; set; }

    }

    public enum WebRTCIPHandlingPolicy
    {
        Default,
        DefaultPublicInterfaceOnly,
        DefaultPublicAndPrivateInterfaces,
        DisableNonProxiedUdp
    }

}
