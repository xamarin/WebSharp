========================================================================
    DYNAMIC LINK LIBRARY : PepperPlugin Project Overview
========================================================================

AppWizard has created this PepperPlugin DLL for you.

This file contains a summary of what you will find in each of the files that
make up your PepperPlugin application.


PepperPlugin.vcxproj
    This is the main project file for VC++ projects generated using an Application Wizard.
    It contains information about the version of Visual C++ that generated the file, and
    information about the platforms, configurations, and project features selected with the
    Application Wizard.

PepperPlugin.vcxproj.filters
    This is the filters file for VC++ projects generated using an Application Wizard. 
    It contains information about the association between the files in your project 
    and the filters. This association is used in the IDE to show grouping of files with
    similar extensions under a specific node (for e.g. ".cpp" files are associated with the
    "Source Files" filter).

PepperPlugin.cpp
    This is the main DLL source file.

	When created, this DLL does not export any symbols. As a result, it
	will not produce a .lib file when it is built. If you wish this project
	to be a project dependency of some other project, you will either need to
	add code to export some symbols from the DLL so that an export library
	will be produced, or you can set the Ignore Input Library property to Yes
	on the General propert page of the Linker folder in the project's Property
	Pages dialog box.

Other notes:

AppWizard uses "TODO:" comments to indicate parts of the source code you
should add to or customize.

/////////////////////////////////////////////////////////////////////////////

TODO
---

Instance
---

[] AddPerInstanceObject(const std::string &interface_name, void *object)
[x] BindGraphics(const Graphics2D &graphics)	
[] BindGraphics(const Graphics3D &graphics)	
[] BindGraphics(const Compositor &compositor)	
[x] ClearInputEventRequest(uint32_t event_classes)	
[x] DidChangeFocus(bool has_focus)	
[x] DidChangeView(const View &view)	
[x] DidChangeView(const Rect &position, const Rect &clip)	
[] GetPerInstanceObject(PP_Instance instance, const std::string &interface_name)	
[x] HandleDocumentLoad(const URLLoader &url_loader)	
[x] HandleInputEvent(const pp::InputEvent &event)	
[x] HandleMessage(const Var &message)	
[x] Init(uint32_t argc, const char *argn[], const char *argv[])
[x] Instance(PP_Instance instance)
[x] IsFullFrame()		
[x] LogToConsole(PP_LogLevel level, const Var &value)
[x] LogToConsoleWithSource(PP_LogLevel level, const Var &source, const Var &value)
[x] PostMessage(const Var &message)	
[] pp_instance() 
[] RegisterMessageHandler(MessageHandler *message_handler, const MessageLoop &message_loop)
[] RemovePerInstanceObject(const std::string &interface_name, void *object)
[] RemovePerInstanceObject(const InstanceHandle &instance, const std::string &interface_name, void *object)
[x] RequestFilteringInputEvents(uint32_t event_classes)
[x] RequestInputEvents(uint32_t event_classes)
[] UnregisterMessageHandler()
[x] ~Instance()

View
---

[x] 	GetRect ()
[x] 	IsFullscreen ()
[x] 	IsVisible ()
[x] 	IsPageVisible () 
[x] 	GetClipRect ()
[x] 	GetDeviceScale () 
[x] 	GetCSSScale ()
[x] 	GetScrollOffset () 

Graphics2D
---

[x] Flush(const CompletionCallback &cc)	
[x] GetScale()
[x] Graphics2D(const Graphics2D &other)
[x] Graphics2D(const InstanceHandle &instance, const Size &size, bool is_always_opaque)
[] operator=(const Graphics2D &other)
[x] PaintImageData(const ImageData &image, const Point &top_left) 
[x] PaintImageData(const ImageData &image, const Point &top_left, const Rect &src_rect) 
[x] ReplaceContents(ImageData *image) 
[x] Scroll(const Rect &clip, const Point &amount) 
[x] SetScale(float scale) 
[x] size() 

ImageData
---

[] ImageData();
[] ImageData(PassRef, PP_Resource resource);
[] ImageData(const ImageData& other);
[x] ImageData(const InstanceHandle& instance, PP_ImageDataFormat format, const Size& size, bool init_to_zero);
[x] ImageData& operator=(const ImageData& other);
[x] static bool IsImageDataFormatSupported(PP_ImageDataFormat format);
[x] static PP_ImageDataFormat GetNativeImageDataFormat();
[x] PP_ImageDataFormat format() const { return desc_.format; }
[x] pp::Size size() const { return desc_.size; }
[x] int32_t stride() const { return desc_.stride; }
[x] data()
[x] GetAddr32(const Point& coord);

Core
---

[x] void AddRefResource(PP_Resource resource) 
[x] void ReleaseResource(PP_Resource resource)
[x] PP_Time GetTime()
[x] PP_TimeTicks GetTimeTicks();
[x] void CallOnMainThread(int32_t delay_in_milliseconds,
                        const CompletionCallback& callback,
                        int32_t result = 0);
[x] bool IsMainThread();

MouseCursor
----

[x] static bool SetCursor(const InstanceHandle& instance,
                        PP_MouseCursor_Type type,
                        const ImageData& image = ImageData(),
                        const Point& hot_spot = Point(0, 0));  *Note* this implemented as SetCursor on Instance

InputEvent
---

[x] PP_InputEvent_Type GetType() const;
[x] PP_TimeTicks GetTimeStamp()
[x] uint32_t GetModifiers() const;

MouseInputEvent
---

[x] explicit MouseInputEvent(const InputEvent& event);
[] MouseInputEvent(const InstanceHandle& instance,
                  PP_InputEvent_Type type,
                  PP_TimeTicks time_stamp,
                  uint32_t modifiers,
                  PP_InputEvent_MouseButton mouse_button,
                  const Point& mouse_position,
                  int32_t click_count,
                  const Point& mouse_movement);
[x] PP_InputEvent_MouseButton GetButton() const;
[x] Point GetPosition() const;
[x] int32_t GetClickCount() const;
[x] Point GetMovement() const;

WheelInputEvent
---

[x] explicit WheelInputEvent(const InputEvent& event);
[ ] WheelInputEvent(const InstanceHandle& instance,
                  PP_TimeTicks time_stamp,
                  uint32_t modifiers,
                  const FloatPoint& wheel_delta,
                  const FloatPoint& wheel_ticks,
                  bool scroll_by_page);
[x] FloatPoint GetDelta() const;
[x] FloatPoint GetTicks() const;
[x] bool GetScrollByPage() const;

KeyboardInputEvent
---

[x] explicit KeyboardInputEvent(const InputEvent& event);
[ ] KeyboardInputEvent(const InstanceHandle& instance,
                     PP_InputEvent_Type type,
                     PP_TimeTicks time_stamp,
                     uint32_t modifiers,
                     uint32_t key_code,
                     const Var& character_text);
[ ] KeyboardInputEvent(const InstanceHandle& instance,
                     PP_InputEvent_Type type,
                     PP_TimeTicks time_stamp,
                     uint32_t modifiers,
                     uint32_t key_code,
                     const Var& character_text,
                     const Var& code);
[x] uint32_t GetKeyCode() const;
[x] Var GetCharacterText() const;
[x] Var GetCode() const;

Fullscreen
---

[x] bool IsFullscreen();
[x] bool SetFullscreen(bool fullscreen);
[x] bool GetScreenSize(Size* size);

TouchInputEvent
---

[x] explicit TouchInputEvent(const InputEvent& event);
[ ] TouchInputEvent(const InstanceHandle& instance,
                  PP_InputEvent_Type type,
                  PP_TimeTicks time_stamp,
                  uint32_t modifiers);
[x] void AddTouchPoint(PP_TouchListType list, PP_TouchPoint point);
[x] uint32_t GetTouchCount(PP_TouchListType list) const;
[x] TouchPoint GetTouchByIndex(PP_TouchListType list, uint32_t index) const;
[x] TouchPoint GetTouchById(PP_TouchListType list, uint32_t id) const;

IMEInputEvent
---

[x] explicit IMEInputEvent(const InputEvent& event);
[ ] IMEInputEvent(const InstanceHandle& instance,
                PP_InputEvent_Type type,
                PP_TimeTicks time_stamp,
                const Var& text,
                const std::vector<uint32_t>& segment_offsets,
                int32_t target_segment,
                const std::pair<uint32_t, uint32_t>& selection);
[x] Var GetText() const;
[x] uint32_t GetSegmentNumber() const;
[x] uint32_t GetSegmentOffset(uint32_t index) const;
[x] int32_t GetTargetSegment() const;
[x] void GetSelection(uint32_t* start, uint32_t* end) const;

WebSocket
---
[x] explicit WebSocket(const InstanceHandle& instance);
[x] int32_t Connect(const Var& url, const Var protocols[],
                  uint32_t protocol_count, const CompletionCallback& callback);
[x] int32_t Close(uint16_t code, const Var& reason,
                const CompletionCallback& callback);
[x] int32_t ReceiveMessage(Var* message,
                         const CompletionCallback& callback);

[x] int32_t SendMessage(const Var& message);
[x] uint64_t GetBufferedAmount();
[x] uint16_t GetCloseCode();
[x] Var GetCloseReason();
[x] bool GetCloseWasClean();
[x] Var GetExtensions();
[x] Var GetProtocol();
[x] PP_WebSocketReadyState GetReadyState();
[x] Var GetURL();


MessageLoop
---
[x] explicit MessageLoop(const InstanceHandle& instance);
[ ] MessageLoop(const MessageLoop& other);
[x] explicit MessageLoop(PP_Resource pp_message_loop);
[x] static MessageLoop GetForMainThread();
[x] static MessageLoop GetCurrent();
[x] int32_t AttachToCurrentThread();
[x] int32_t Run();
[x] int32_t PostWork(const CompletionCallback& callback,
                   int64_t delay_ms = 0);
[x] int32_t PostQuit(bool should_destroy);

FileSystem
---
[ ] FileSystem(const FileSystem& other);
[ ] explicit FileSystem(const Resource& resource);
[ ] FileSystem(PassRef, PP_Resource resource);
[x] FileSystem(const InstanceHandle& instance, PP_FileSystemType type);
[x] int32_t Open(int64_t expected_size, const CompletionCallback& cc);
[ ] static bool IsFileSystem(const Resource& resource);

FileRef
---
[ ] explicit FileRef(PP_Resource resource);
[x] FileRef(PassRef, PP_Resource resource);
[x] FileRef(const FileSystem& file_system, const char* path);
[ ] FileRef(const FileRef& other);
[x] PP_FileSystemType GetFileSystemType() const;
[x] Var GetName() const;
[x] FileRef GetParent() const;
[x] int32_t MakeDirectory(int32_t make_directory_flags,
                        const CompletionCallback& cc);
[x] int32_t Touch(PP_Time last_access_time,
                PP_Time last_modified_time,
                const CompletionCallback& cc);
[x] int32_t Delete(const CompletionCallback& cc);
[x] int32_t Rename(const FileRef& new_file_ref, const CompletionCallback& cc);
[x] int32_t Query(const CompletionCallbackWithOutput<PP_FileInfo>& callback);
[x] int32_t ReadDirectoryEntries(
      const CompletionCallbackWithOutput< std::vector<DirectoryEntry> >&
          callback);

FileIO
---
[x] explicit FileIO(const InstanceHandle& instance);
[ ] FileIO(const FileIO& other);
[x] int32_t Open(const FileRef& file_ref,
               int32_t open_flags,
               const CompletionCallback& cc);
[x] int32_t Query(PP_FileInfo* result_buf,
                const CompletionCallback& cc);
[x] int32_t Touch(PP_Time last_access_time,
                PP_Time last_modified_time,
                const CompletionCallback& cc);
[x] int32_t Read(int64_t offset,
               char* buffer,
               int32_t bytes_to_read,
               const CompletionCallback& cc);
[x] int32_t Read(int32_t offset,
               int32_t max_read_length,
               const CompletionCallbackWithOutput< std::vector<char> >& cc);
[x] int32_t Write(int64_t offset,
                const char* buffer,
                int32_t bytes_to_write,
                const CompletionCallback& cc);
[x] int32_t SetLength(int64_t length,
                    const CompletionCallback& cc);
[x] int32_t Flush(const CompletionCallback& cc);
[x] void Close();

URLLoader
---

[ ] explicit URLLoader(PP_Resource resource);
[x] explicit URLLoader(const InstanceHandle& instance);
[ ] URLLoader(const URLLoader& other);
[x] int32_t Open(const URLRequestInfo& request_info,
               const CompletionCallback& cc);
[x] int32_t FollowRedirect(const CompletionCallback& cc);
[x] bool GetUploadProgress(int64_t* bytes_sent,
                         int64_t* total_bytes_to_be_sent) const;
[x] bool GetDownloadProgress(int64_t* bytes_received,
                           int64_t* total_bytes_to_be_received) const;
[x] URLResponseInfo GetResponseInfo() const;
[x] int32_t ReadResponseBody(void* buffer,
                           int32_t bytes_to_read,
                           const CompletionCallback& cc);
[x] int32_t FinishStreamingToFile(const CompletionCallback& cc);

[x] void Close();

URLRequestInfo
---
[x] explicit URLRequestInfo(const InstanceHandle& instance);
[ ] URLRequestInfo(const URLRequestInfo& other);
[x] bool SetProperty(PP_URLRequestProperty property, const Var& value);
[x] bool AppendDataToBody(const void* data, uint32_t len);
[x] bool AppendFileToBody(const FileRef& file_ref,
                        PP_Time expected_last_modified_time = 0);
[x] bool AppendFileRangeToBody(const FileRef& file_ref,
                             int64_t start_offset,
                             int64_t length,
                             PP_Time expected_last_modified_time = 0);
[x] bool SetURL(const Var& url_string) {
[x] bool SetMethod(const Var& method_string) {
[x] bool SetHeaders(const Var& headers_string) {
[x] bool SetStreamToFile(bool enable) {
[x] bool SetFollowRedirects(bool enable) {
[x] bool SetRecordDownloadProgress(bool enable) {
[x] bool SetRecordUploadProgress(bool enable) {
[x] bool SetCustomReferrerURL(const Var& url) {
[x] bool SetAllowCrossOriginRequests(bool enable) {
[x] bool SetAllowCredentials(bool enable) {
[x] bool SetCustomContentTransferEncoding(const Var& content_transfer_encoding) {
[x] bool SetPrefetchBufferUpperThreshold(int32_t size) 
[x] bool SetPrefetchBufferLowerThreshold(int32_t size) 
[x] bool SetCustomUserAgent(const Var& user_agent)

URLResponseInfo
---
[x] URLResponseInfo(PassRef, PP_Resource resource);
[ ] URLResponseInfo(const URLResponseInfo& other);
[x] Var GetProperty(PP_URLResponseProperty property) const;
[x] FileRef GetBodyAsFileRef() const;
[x] Var GetURL() const {
[x] Var GetRedirectURL() const {
[x] Var GetRedirectMethod()
[x] int32_t GetStatusCode() 
[x] Var GetStatusLine()
[x] Var GetHeaders()

NetworkProxy
---
[ ] static bool IsAvailable();
[x] static int32_t GetProxyForURL(
      const InstanceHandle& instance,
      const Var& url,
      const pp::CompletionCallbackWithOutput<Var>& callback);

NetAddress
---
[x] NetAddress(PassRef, PP_Resource resource);
[x] NetAddress(const InstanceHandle& instance,
             const PP_NetAddress_IPv4& ipv4_addr);
[x] NetAddress(const InstanceHandle& instance,
             const PP_NetAddress_IPv6& ipv6_addr);
[ ] NetAddress(const NetAddress& other);
[x] virtual ~NetAddress();
[ ] NetAddress& operator=(const NetAddress& other);
[ ] static bool IsAvailable();
[x] PP_NetAddress_Family GetFamily() const;
[x] Var DescribeAsString(bool include_port) const;
[x] bool DescribeAsIPv4Address(PP_NetAddress_IPv4* ipv4_addr) const;
[x] bool DescribeAsIPv6Address(PP_NetAddress_IPv6* ipv6_addr) const;

NetworkList
---

[x] NetworkList(PassRef, PP_Resource resource);
[ ] static bool IsAvailable();
[x] uint32_t GetCount() const;
[x] std::string GetName(uint32_t index) const;
[x] PP_NetworkList_Type GetType(uint32_t index) const;
[x] PP_NetworkList_State GetState(uint32_t index) const;
[x] int32_t GetIpAddresses(uint32_t index,
                         std::vector<NetAddress>* addresses) const;
[x] std::string GetDisplayName(uint32_t index) const;
[x] uint32_t GetMTU(uint32_t index) const;

NetworkMonitor
---

[x] explicit NetworkMonitor(const InstanceHandle& instance);
[x] int32_t UpdateNetworkList(
      const CompletionCallbackWithOutput<NetworkList>& callback);
[ ] static bool IsAvailable();

NetworkProxy
---

[ ] static bool IsAvailable();
[x] static int32_t GetProxyForURL(
      const InstanceHandle& instance,
      const Var& url,
      const pp::CompletionCallbackWithOutput<Var>& callback);