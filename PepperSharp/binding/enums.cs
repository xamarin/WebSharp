public enum PPAudioBuffer_SampleRate
{
    Unknown = 0,
    _8000 = 8000,
    _16000 = 16000,
    _22050 = 22050,
    _32000 = 32000,
    _44100 = 44100,
    _48000 = 48000,
    _96000 = 96000,
    _192000 = 192000,
}
public enum PPAudioBuffer_SampleSize
{
    Unknown = 0,
	_16_bits = 2,
}
public enum PPAudioFrameSize
{
    PP_AUDIOMINSAMPLEFRAMECOUNT = 64,
    PP_AUDIOMAXSAMPLEFRAMECOUNT = 32768,
}
public enum PPAudioSampleRate
{
    None = 0,
    _44100 = 44100,
    _48000 = 48000,
}
public enum PPBlendMode
{
    None,
    Src_over,
    Last = Src_over,
}
public enum PPLogLevel
{
    Tip = 0,
    Log = 1,
    Warning = 2,
    Error = 3,
}
public enum PPFileOpenFlags
{
    PP_FILEOPENFLAG_READ = 1 << 0,
    PP_FILEOPENFLAG_WRITE = 1 << 1,
    PP_FILEOPENFLAG_CREATE = 1 << 2,
    PP_FILEOPENFLAG_TRUNCATE = 1 << 3,
    PP_FILEOPENFLAG_EXCLUSIVE = 1 << 4,
    PP_FILEOPENFLAG_APPEND = 1 << 5,
}
public enum PPMakeDirectoryFlags
{
    PP_MAKEDIRECTORYFLAG_NONE = 0 << 0,
    PP_MAKEDIRECTORYFLAG_WITH_ANCESTORS = 1 << 0,
    PP_MAKEDIRECTORYFLAG_EXCLUSIVE = 1 << 1,
}
public enum PPHostResolver_Flag
{
    Canonname = 1 << 0,
}
public enum PPImageDataFormat
{
    Bgra_premul,
    Rgba_premul,
}
public enum PPInputEvent_Type
{
    Undefined = -1,
    Mousedown = 0,
    Mouseup = 1,
    Mousemove = 2,
    Mouseenter = 3,
    Mouseleave = 4,
    Wheel = 5,
    Rawkeydown = 6,
    Keydown = 7,
    Keyup = 8,
    Char = 9,
    Contextmenu = 10,
    Ime_composition_start = 11,
    Ime_composition_update = 12,
    Ime_composition_end = 13,
    Ime_text = 14,
    Touchstart = 15,
    Touchmove = 16,
    Touchend = 17,
    Touchcancel = 18,
}
public enum PPInputEvent_Modifier
{
    Shiftkey = 1 << 0,
    Controlkey = 1 << 1,
    Altkey = 1 << 2,
    Metakey = 1 << 3,
    Iskeypad = 1 << 4,
    Isautorepeat = 1 << 5,
    Leftbuttondown = 1 << 6,
    Middlebuttondown = 1 << 7,
    Rightbuttondown = 1 << 8,
    Capslockkey = 1 << 9,
    Numlockkey = 1 << 10,
    Isleft = 1 << 11,
    Isright = 1 << 12,
}
public enum PPInputEvent_MouseButton
{
    None = -1,
    Left = 0,
    Middle = 1,
    Right = 2,
}
public enum PPInputEvent_Class
{
    Mouse = 1 << 0,
    Keyboard = 1 << 1,
    Wheel = 1 << 2,
    Touch = 1 << 3,
    Ime = 1 << 4,
}
public enum PPTouchListType
{
    PP_TOUCHLIST_TYPE_TOUCHES = 0,
    PP_TOUCHLIST_TYPE_CHANGEDTOUCHES = 1,
    PP_TOUCHLIST_TYPE_TARGETTOUCHES = 2,
}
public enum PPMediaStreamAudioTrack_Attrib
{
    None = 0,
    Buffers = 1,
    Sample_rate = 2,
    Sample_size = 3,
    Channels = 4,
    Duration = 5,
}
public enum PPMediaStreamVideoTrack_Attrib
{
    None = 0,
    Buffered_frames = 1,
    Width = 2,
    Height = 3,
    Format = 4,
}
public enum PPMouseCursor_Type
{
    Custom = -1,
    Pointer = 0,
    Cross = 1,
    Hand = 2,
    Ibeam = 3,
    Wait = 4,
    Help = 5,
    Eastresize = 6,
    Northresize = 7,
    Northeastresize = 8,
    Northwestresize = 9,
    Southresize = 10,
    Southeastresize = 11,
    Southwestresize = 12,
    Westresize = 13,
    Northsouthresize = 14,
    Eastwestresize = 15,
    Northeastsouthwestresize = 16,
    Northwestsoutheastresize = 17,
    Columnresize = 18,
    Rowresize = 19,
    Middlepanning = 20,
    Eastpanning = 21,
    Northpanning = 22,
    Northeastpanning = 23,
    Northwestpanning = 24,
    Southpanning = 25,
    Southeastpanning = 26,
    Southwestpanning = 27,
    Westpanning = 28,
    Move = 29,
    Verticaltext = 30,
    Cell = 31,
    Contextmenu = 32,
    Alias = 33,
    Progress = 34,
    Nodrop = 35,
    Copy = 36,
    None = 37,
    Notallowed = 38,
    Zoomin = 39,
    Zoomout = 40,
    Grab = 41,
    Grabbing = 42,
}
public enum PPNetworkList_Type
{
    Unknown = 0,
    Ethernet = 1,
    Wifi = 2,
    Cellular = 3,
}
public enum PPNetworkList_State
{
    Down = 0,
    Up = 1,
}
public enum PPNetAddress_Family
{
    Unspecified = 0,
    Ipv4 = 1,
    Ipv6 = 2,
}
public enum PPTCPSocket_Option
{
    No_delay = 0,
    Send_buffer_size = 1,
    Recv_buffer_size = 2,
}
public enum PPTextInput_Type
{
    None = 0,
    Text = 1,
    Password = 2,
    Search = 3,
    Email = 4,
    Number = 5,
    Telephone = 6,
    Url = 7,
}
public enum PPUDPSocket_Option
{
    Address_reuse = 0,
    Broadcast = 1,
    Send_buffer_size = 2,
    Recv_buffer_size = 3,
    Multicast_loop = 4,
    Multicast_ttl = 5,
}
public enum PPURLRequestProperty
{
    Url = 0,
    Method = 1,
    Headers = 2,
    Streamtofile = 3,
    Followredirects = 4,
    Recorddownloadprogress = 5,
    Recorduploadprogress = 6,
    Customreferrerurl = 7,
    Allowcrossoriginrequests = 8,
    Allowcredentials = 9,
    Customcontenttransferencoding = 10,
    Prefetchbufferupperthreshold = 11,
    Prefetchbufferlowerthreshold = 12,
    Customuseragent = 13,
}
public enum PPURLResponseProperty
{
    Url = 0,
    Redirecturl = 1,
    Redirectmethod = 2,
    Statuscode = 3,
    Statusline = 4,
    Headers = 5,
}
public enum PPVideoFrame_Format
{
    Unknown = 0,
    Yv12 = 1,
    I420 = 2,
    Bgra = 3,
    Last = Bgra,
}
public enum PPWebSocketReadyState
{
    Invalid = -1,
    Connecting = 0,
    Open = 1,
    Closing = 2,
    Closed = 3,
}
public enum PPWebSocketCloseCode
{
    PP_WEBSOCKETSTATUSCODE_NOT_SPECIFIED = 1005,
    PP_WEBSOCKETSTATUSCODE_NORMAL_CLOSURE = 1000,
    PP_WEBSOCKETSTATUSCODE_GOING_AWAY = 1001,
    PP_WEBSOCKETSTATUSCODE_PROTOCOL_ERROR = 1002,
    PP_WEBSOCKETSTATUSCODE_UNSUPPORTED_DATA = 1003,
    PP_WEBSOCKETSTATUSCODE_NO_STATUS_RECEIVED = 1005,
    PP_WEBSOCKETSTATUSCODE_ABNORMAL_CLOSURE = 1006,
    PP_WEBSOCKETSTATUSCODE_INVALID_FRAME_PAYLOAD_DATA = 1007,
    PP_WEBSOCKETSTATUSCODE_POLICY_VIOLATION = 1008,
    PP_WEBSOCKETSTATUSCODE_MESSAGE_TOO_BIG = 1009,
    PP_WEBSOCKETSTATUSCODE_MANDATORY_EXTENSION = 1010,
    PP_WEBSOCKETSTATUSCODE_INTERNAL_SERVER_ERROR = 1011,
    PP_WEBSOCKETSTATUSCODE_TLS_HANDSHAKE = 1015,
    PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MIN = 3000,
    PP_WEBSOCKETSTATUSCODE_USER_REGISTERED_MAX = 3999,
    PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MIN = 4000,
    PP_WEBSOCKETSTATUSCODE_USER_PRIVATE_MAX = 4999,
}
public enum PPBool
{
    PP_FALSE = 0,
    PP_TRUE = 1,
}
public enum PPVideoProfile
{
    H264baseline = 0,
    H264main = 1,
    H264extended = 2,
    H264high = 3,
    H264high10profile = 4,
    H264high422profile = 5,
    H264high444predictiveprofile = 6,
    H264scalablebaseline = 7,
    H264scalablehigh = 8,
    H264stereohigh = 9,
    H264multiviewhigh = 10,
    Vp8_any = 11,
    Vp9_any = 12,
    Max = Vp9_any,
}
public enum PPAudioProfile
{
    Opus = 0,
    Max = Opus,
}
public enum PPHardwareAcceleration
{
    Only = 0,
    Withfallback = 1,
    None = 2,
    Last = None,
}
public enum PPCompletionCallback_Flag
{
    None = 0 << 0,
    Optional = 1 << 0,
}
public enum PPError
{
    PP_OK = 0,
    PP_OK_COMPLETIONPENDING = -1,
    Failed = -2,
    Aborted = -3,
    Badargument = -4,
    Badresource = -5,
    Nointerface = -6,
    Noaccess = -7,
    Nomemory = -8,
    Nospace = -9,
    Noquota = -10,
    Inprogress = -11,
    Notsupported = -12,
    Blocks_main_thread = -13,
    Malformed_input = -14,
    Resource_failed = -15,
    Filenotfound = -20,
    Fileexists = -21,
    Filetoobig = -22,
    Filechanged = -23,
    Notafile = -24,
    Timedout = -30,
    Usercancel = -40,
    No_user_gesture = -41,
    Context_lost = -50,
    No_message_loop = -51,
    Wrong_thread = -52,
    Would_block_thread = -53,
    Connection_closed = -100,
    Connection_reset = -101,
    Connection_refused = -102,
    Connection_aborted = -103,
    Connection_failed = -104,
    Connection_timedout = -105,
    Address_invalid = -106,
    Address_unreachable = -107,
    Address_in_use = -108,
    Message_too_big = -109,
    Name_not_resolved = -110,
}
public enum PPFileType
{
    Regular = 0,
    Directory = 1,
    Other = 2,
}
public enum PPFileSystemType
{
    Invalid = 0,
    External = 1,
    Localpersistent = 2,
    Localtemporary = 3,
    Isolated = 4,
}
public enum PPGraphics3DAttrib
{
    Alpha_size = 0x3021,
    Blue_size = 0x3022,
    Green_size = 0x3023,
    Red_size = 0x3024,
    Depth_size = 0x3025,
    Stencil_size = 0x3026,
    Samples = 0x3031,
    Sample_buffers = 0x3032,
    None = 0x3038,
    Height = 0x3056,
    Width = 0x3057,
    Swap_behavior = 0x3093,
    Buffer_preserved = 0x3094,
    Buffer_destroyed = 0x3095,
    Gpu_preference = 0x11000,
    Gpu_preference_low_power = 0x11001,
    Gpu_preference_performance = 0x11002,
}
public enum PPVarType
{
    Undefined = 0,
    Null = 1,
    Bool = 2,
    Int32 = 3,
    Double = 4,
    String = 5,
    Object = 6,
    Array = 7,
    Dictionary = 8,
    Array_buffer = 9,
    Resource = 10,
}
