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