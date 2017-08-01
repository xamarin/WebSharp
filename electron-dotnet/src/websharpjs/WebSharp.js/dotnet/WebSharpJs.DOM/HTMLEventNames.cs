// HTMLEventNames.cs - Predefined event names https://developer.mozilla.org/en-US/docs/Web/Events

using System;

namespace WebSharpJs.DOM
{
    public static class HTMLEventNames
    {

        #region MouseEvents

        public static readonly String MouseUp = "mouseup";

        public static readonly String MouseOver = "mouseover";

        public static readonly String MouseOut = "mouseout";

        public static readonly String MouseMove = "mousemove";

        public static readonly String MouseLeave = "mouseleave";

        public static readonly String MouseEnter = "mouseenter";

        public static readonly String MouseDown = "mousedown";

        public static readonly String Click = "click";

        public static readonly String DblClick = "dblclick";

        public static readonly String ContextMenuy = "contextmenu";

        public static readonly String Wheel = "wheel";

        public static readonly String PointerLockChange = "pointerlockchange";

        public static readonly String PointerLockError = "pointerlockerror";

        public static readonly String Select = "select";

        #endregion

        #region Drag & Drop Events

        public static readonly String Drag = "drag";

        public static readonly String DragEnd = "dragend";

        public static readonly String DragEnter = "dragenter";

        public static readonly String DragExit = "dragexit";

        public static readonly String DragLeave = "dragleave";

        public static readonly String DragOver = "dragover";

        public static readonly String DragStart = "dragstart";

        public static readonly String Drop = "drop";


        #endregion

        #region Keyboard Events

        public static readonly String Keydown = "keydown";

        public static readonly String Keypress = "keypress";

        public static readonly String Keyup = "keyup";



        #endregion

        #region Progress

        public static readonly String LoadEnd = "loadend";

        public static readonly String LoadStart = "loadstart";

        public static readonly String Progress = "progress";

        public static readonly String Error = "error";

        public static readonly String Abort = "abort";

        public static readonly String Load = "load";

        public static readonly String TimeOut = "timeout";

        #endregion

        #region View Events

        public static readonly String FullscreenChange = "fullscreenchange";

        public static readonly String FullscreenError = "fullscreenerror";

        public static readonly String Scroll = "scroll";

        public static readonly String Resize = "resize";

        #endregion

        #region Focus Events

        public static readonly String Blur = "blur";

        public static readonly String Focus = "focus";

        #endregion

        #region Media Events


        public static readonly String DurationChange = "durationchange";

        public static readonly String CanPlayThrough = "canplaythrough";

        public static readonly String CanPlay = "canplay";

        public static readonly String CueChange = "cuechange";

        public static readonly String LoadedData = "loadeddata";

        public static readonly String LoadedMetaData = "loadedmetadata";

        public static readonly String Ended = "ended";

        public static readonly String Pause = "pause";

        public static readonly String Play = "play";

        public static readonly String Playing = "playing";

        public static readonly String RateChange = "ratechange";

        public static readonly String Waiting = "waiting";

        public static readonly String VolumeChange = "volumechange";

        public static readonly String Stalled = "stalled";

        public static readonly String TimeUpdate = "timeupdate";

        public static readonly String Suspend = "suspend";

        public static readonly String Seeking = "seeking";

        public static readonly String Seeked = "seeked";


        #endregion

        #region Network Events


        public static readonly String Offline = "offline";

        public static readonly String Online = "online";

        #endregion

        #region Form Events

        public static readonly String Submit = "submit";

        public static readonly String Reset = "reset";

        #endregion

        #region Session History Events
        public static readonly String PageHide = "pagehide";

        public static readonly String PageShow = "pageshow";

        public static readonly String PopState = "popstate";

        #endregion

        # region Value Change events

        public static readonly String HashChange = "hashchange";

        public static readonly String Input = "input";

        public static readonly String Change = "change";

        public static readonly String ReadyStateChanged = "readystatechanged";



        #endregion

        #region Printing

        public static readonly String AfterPrint = "afterprint";

        public static readonly String BeforePrint = "beforeprint";

        #endregion

        #region Uncategorized events

        public static readonly String Message = "message";

        public static readonly String Show = "show";

        public static readonly String Invalid = "invalid";

        #endregion

        public static readonly String DomContentLoaded = "DOMContentLoaded";

    }
}
