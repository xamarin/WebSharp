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
[] Init(uint32_t argc, const char *argn[], const char *argv[])
[] Instance(PP_Instance instance)
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
[] ~Instance()

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




