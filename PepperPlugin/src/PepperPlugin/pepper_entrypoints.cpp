/* Copyright (c) 2016 Xamarin. */
#include "stdafx.h"
/* NOTE: this is auto-generated from IDL */
#include "pepper_entrypoints.h"

#include "ppapi/c/ppb.h"
#include "ppapi/c/ppb_console.h"
#include "ppapi/c/ppb_fullscreen.h"
#include "ppapi/c/ppb_graphics_2d.h"
#include "ppapi/c/ppb_image_data.h"
#include "ppapi/c/ppb_input_event.h"
#include "ppapi/c/ppb_instance.h"
#include "ppapi/c/ppb_message_loop.h"
#include "ppapi/c/ppb_messaging.h"
#include "ppapi/c/ppb_mouse_cursor.h"
#include "ppapi/c/ppb_mouse_lock.h"
#include "ppapi/c/ppb_url_loader.h"
#include "ppapi/c/ppb_url_request_info.h"
#include "ppapi/c/ppb_url_response_info.h"
#include "ppapi/c/ppb_var.h"
#include "ppapi/c/ppb_view.h"
#include "ppapi/c/ppp_input_event.h"
#include "ppapi/c/ppp_messaging.h"
#include "ppapi/c/ppp_mouse_lock.h"

#ifndef PEPPER_EXPORT
#define PEPPER_EXPORT __declspec(dllexport)
#endif

using namespace pp;

namespace Pepper {

	namespace {
		// Specialize this function to return the interface string corresponding to the
		// PP?_XXX structure.
		template <typename T> const char* interface_name() {
			return NULL;
		}

		template <typename T> inline T const* get_interface() {
			static T const* funcs = reinterpret_cast<T const*>(
				pp::Module::Get()->GetBrowserInterface(interface_name<T>()));
			return funcs;
		}

		template <typename T> inline bool has_interface() {
			return get_interface<T>() != NULL;
		}

	}
}
/* BEGIN Declarations for all Interface Definitions. */

namespace Pepper {
	namespace {
		template <> const char*	interface_name<PPB_Console_1_0>() {
			return PPB_CONSOLE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Fullscreen_1_0>() {
			return PPB_FULLSCREEN_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_0>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_1>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_2>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_ImageData_1_0>() {
			return PPB_IMAGEDATA_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_InputEvent_1_0>() {
			return PPB_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseInputEvent_1_0>() {
			return PPB_MOUSE_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseInputEvent_1_1>() {
			return PPB_MOUSE_INPUT_EVENT_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_WheelInputEvent_1_0>() {
			return PPB_WHEEL_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_KeyboardInputEvent_1_0>() {
			return PPB_KEYBOARD_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_KeyboardInputEvent_1_2>() {
			return PPB_KEYBOARD_INPUT_EVENT_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_TouchInputEvent_1_0>() {
			return PPB_TOUCH_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_IMEInputEvent_1_0>() {
			return PPB_IME_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Instance_1_0>() {
			return PPB_INSTANCE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MessageLoop_1_0>() {
			return PPB_MESSAGELOOP_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Messaging_1_0>() {
			return PPB_MESSAGING_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Messaging_1_2>() {
			return PPB_MESSAGING_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_MouseCursor_1_0>() {
			return PPB_MOUSECURSOR_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseLock_1_0>() {
			return PPB_MOUSELOCK_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_URLLoader_1_0>() {
			return PPB_URLLOADER_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_URLRequestInfo_1_0>() {
			return PPB_URLREQUESTINFO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_URLResponseInfo_1_0>() {
			return PPB_URLRESPONSEINFO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Var_1_0>() {
			return PPB_VAR_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Var_1_1>() {
			return PPB_VAR_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_Var_1_2>() {
			return PPB_VAR_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_View_1_0>() {
			return PPB_VIEW_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_View_1_1>() {
			return PPB_VIEW_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_View_1_2>() {
			return PPB_VIEW_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPP_InputEvent_0_1>() {
			return PPP_INPUT_EVENT_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPP_Messaging_1_0>() {
			return PPP_MESSAGING_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPP_MouseLock_1_0>() {
			return PPP_MOUSELOCK_INTERFACE_1_0;
		}
	}
}
/* END Declarations for all Interface Definitions. */

namespace Pepper {


	/* We don't want name mangling for these external functions.  We only need
	* 'extern "C"' if we're compiling with a C++ compiler.
	*/
#ifdef __cplusplus
	extern "C" {
#endif
	namespace {

		#pragma region /* Begin entry point methods for PPB_Console */

		PEPPER_EXPORT void PPB_Console_Log(PP_Instance instance, PP_LogLevel level, struct PP_Var value) {
			if (has_interface<PPB_Console_1_0>()) {
				get_interface<PPB_Console_1_0>()->Log(instance, level, value);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Console_LogWithSource(PP_Instance instance, PP_LogLevel level, struct PP_Var source, struct PP_Var value) {
			if (has_interface<PPB_Console_1_0>()) {
				get_interface<PPB_Console_1_0>()->LogWithSource(instance, level, source, value);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_Console */

		#pragma region /* Begin entry point methods for PPB_Fullscreen */

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_IsFullscreen(PP_Instance instance) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->IsFullscreen(instance);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_SetFullscreen(PP_Instance instance, PP_Bool fullscreen) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->SetFullscreen(instance, fullscreen);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_GetScreenSize(PP_Instance instance, struct PP_Size* size) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->GetScreenSize(instance, size);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Fullscreen */

		#pragma region /* Begin entry point methods for PPB_Graphics2D */

		PEPPER_EXPORT PP_Resource PPB_Graphics2D_Create(PP_Instance instance, struct PP_Size size, PP_Bool is_always_opaque) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Create(instance, &size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Create(instance, &size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Create(instance, &size, is_always_opaque);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_IsGraphics2D(PP_Resource resource) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->IsGraphics2D(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->IsGraphics2D(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->IsGraphics2D(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_Describe(PP_Resource graphics_2d, struct PP_Size* size, PP_Bool* is_always_opaque) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Describe(graphics_2d, size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Describe(graphics_2d, size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Describe(graphics_2d, size, is_always_opaque);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void PPB_Graphics2D_PaintImageData(PP_Resource graphics_2d, PP_Resource image_data, struct PP_Point top_left, struct PP_Rect src_rect) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Graphics2D_Scroll(PP_Resource graphics_2d, struct PP_Rect clip_rect, struct PP_Point amount) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Graphics2D_ReplaceContents(PP_Resource graphics_2d, PP_Resource image_data) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->ReplaceContents(graphics_2d, image_data);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->ReplaceContents(graphics_2d, image_data);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->ReplaceContents(graphics_2d, image_data);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_Graphics2D_Flush(PP_Resource graphics_2d, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Flush(graphics_2d, callback);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Flush(graphics_2d, callback);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Flush(graphics_2d, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_SetScale(PP_Resource resource, float scale) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->SetScale(resource, scale);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->SetScale(resource, scale);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT float PPB_Graphics2D_GetScale(PP_Resource resource) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->GetScale(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->GetScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_SetLayerTransform(PP_Resource resource, float scale, struct PP_Point origin, struct PP_Point translate) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->SetLayerTransform(resource, scale, &origin, &translate);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Graphics2D */

		#pragma region /* Begin entry point methods for PPB_ImageData */

		PEPPER_EXPORT PP_ImageDataFormat PPB_ImageData_GetNativeImageDataFormat(void) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->GetNativeImageDataFormat();
			}
			return PP_IMAGEDATAFORMAT_BGRA_PREMUL;
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_IsImageDataFormatSupported(PP_ImageDataFormat format) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->IsImageDataFormatSupported(format);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Resource PPB_ImageData_Create(PP_Instance instance, PP_ImageDataFormat format, struct PP_Size size, PP_Bool init_to_zero) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Create(instance, format, &size, init_to_zero);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_IsImageData(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->IsImageData(image_data);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_Describe(PP_Resource image_data, struct PP_ImageDataDesc* desc) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Describe(image_data, desc);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void* PPB_ImageData_Map(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Map(image_data);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_ImageData_Unmap(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				get_interface<PPB_ImageData_1_0>()->Unmap(image_data);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_ImageData */

		#pragma region /* Begin entry point methods for PPB_InputEvent */

		PEPPER_EXPORT int32_t PPB_InputEvent_RequestInputEvents(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->RequestInputEvents(instance, event_classes);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_InputEvent_RequestFilteringInputEvents(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->RequestFilteringInputEvents(instance, event_classes);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_InputEvent_ClearInputEventRequest(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				get_interface<PPB_InputEvent_1_0>()->ClearInputEventRequest(instance, event_classes);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_InputEvent_IsInputEvent(PP_Resource resource) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->IsInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_InputEvent_Type PPB_InputEvent_GetType(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetType(event);
			}
			return PP_INPUTEVENT_TYPE_UNDEFINED;
		}

		PEPPER_EXPORT PP_TimeTicks PPB_InputEvent_GetTimeStamp(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetTimeStamp(event);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_InputEvent_GetModifiers(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetModifiers(event);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_InputEvent */

		#pragma region /* Begin entry point methods for PPB_MouseInputEvent */

		PEPPER_EXPORT PP_Resource PPB_MouseInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers, PP_InputEvent_MouseButton mouse_button, struct PP_Point mouse_position, int32_t click_count, struct PP_Point mouse_movement) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->Create(instance, type, time_stamp, modifiers, mouse_button, &mouse_position, click_count, &mouse_movement);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_MouseInputEvent_IsMouseInputEvent(PP_Resource resource) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->IsMouseInputEvent(resource);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->IsMouseInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_InputEvent_MouseButton PPB_MouseInputEvent_GetButton(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetButton(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetButton(mouse_event);
			}
			return PP_INPUTEVENT_MOUSEBUTTON_NONE;
		}

		PEPPER_EXPORT struct PP_Point PPB_MouseInputEvent_GetPosition(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetPosition(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetPosition(mouse_event);
			}
			return PP_MakePoint(0,0);
		}

		PEPPER_EXPORT int32_t PPB_MouseInputEvent_GetClickCount(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetClickCount(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetClickCount(mouse_event);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Point PPB_MouseInputEvent_GetMovement(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetMovement(mouse_event);
			}
			return PP_MakePoint(0,0);
		}

		#pragma endregion /* End entry point generation for PPB_MouseInputEvent */

		#pragma region /* Begin entry point methods for PPB_WheelInputEvent */

		PEPPER_EXPORT PP_Resource PPB_WheelInputEvent_Create(PP_Instance instance, PP_TimeTicks time_stamp, uint32_t modifiers, struct PP_FloatPoint wheel_delta, struct PP_FloatPoint wheel_ticks, PP_Bool scroll_by_page) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->Create(instance, time_stamp, modifiers, &wheel_delta, &wheel_ticks, scroll_by_page);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_WheelInputEvent_IsWheelInputEvent(PP_Resource resource) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->IsWheelInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_FloatPoint PPB_WheelInputEvent_GetDelta(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetDelta(wheel_event);
			}
			return PP_MakeFloatPoint(0,0);
		}

		PEPPER_EXPORT struct PP_FloatPoint PPB_WheelInputEvent_GetTicks(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetTicks(wheel_event);
			}
			return PP_MakeFloatPoint(0,0);
		}

		PEPPER_EXPORT PP_Bool PPB_WheelInputEvent_GetScrollByPage(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetScrollByPage(wheel_event);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_WheelInputEvent */

		#pragma region /* Begin entry point methods for PPB_KeyboardInputEvent */

		PEPPER_EXPORT PP_Resource PPB_KeyboardInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers, uint32_t key_code, struct PP_Var character_text, struct PP_Var code) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->Create(instance, type, time_stamp, modifiers, key_code, character_text, code);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_KeyboardInputEvent_IsKeyboardInputEvent(PP_Resource resource) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->IsKeyboardInputEvent(resource);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->IsKeyboardInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_KeyboardInputEvent_GetKeyCode(PP_Resource key_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetKeyCode(key_event);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->GetKeyCode(key_event);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_KeyboardInputEvent_GetCharacterText(PP_Resource character_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetCharacterText(character_event);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->GetCharacterText(character_event);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_KeyboardInputEvent_GetCode(PP_Resource key_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetCode(key_event);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_KeyboardInputEvent */

		#pragma region /* Begin entry point methods for PPB_TouchInputEvent */

		PEPPER_EXPORT PP_Resource PPB_TouchInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->Create(instance, type, time_stamp, modifiers);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_TouchInputEvent_AddTouchPoint(PP_Resource touch_event, PP_TouchListType list, struct PP_TouchPoint point) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				get_interface<PPB_TouchInputEvent_1_0>()->AddTouchPoint(touch_event, list, &point);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_TouchInputEvent_IsTouchInputEvent(PP_Resource resource) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->IsTouchInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_TouchInputEvent_GetTouchCount(PP_Resource resource, PP_TouchListType list) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchCount(resource, list);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_TouchPoint PPB_TouchInputEvent_GetTouchByIndex(PP_Resource resource, PP_TouchListType list, uint32_t index) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchByIndex(resource, list, index);
			}
			return PP_MakeTouchPoint();
		}

		PEPPER_EXPORT struct PP_TouchPoint PPB_TouchInputEvent_GetTouchById(PP_Resource resource, PP_TouchListType list, uint32_t touch_id) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchById(resource, list, touch_id);
			}
			return PP_MakeTouchPoint();
		}

		#pragma endregion /* End entry point generation for PPB_TouchInputEvent */

		#pragma region /* Begin entry point methods for PPB_IMEInputEvent */

		PEPPER_EXPORT PP_Resource PPB_IMEInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, struct PP_Var text, uint32_t segment_number, const uint32_t segment_offsets[], int32_t target_segment, uint32_t selection_start, uint32_t selection_end) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->Create(instance, type, time_stamp, text, segment_number, segment_offsets, target_segment, selection_start, selection_end);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_IMEInputEvent_IsIMEInputEvent(PP_Resource resource) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->IsIMEInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_IMEInputEvent_GetText(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetText(ime_event);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT uint32_t PPB_IMEInputEvent_GetSegmentNumber(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetSegmentNumber(ime_event);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_IMEInputEvent_GetSegmentOffset(PP_Resource ime_event, uint32_t index) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetSegmentOffset(ime_event, index);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_IMEInputEvent_GetTargetSegment(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetTargetSegment(ime_event);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_IMEInputEvent_GetSelection(PP_Resource ime_event, uint32_t* start, uint32_t* end) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				get_interface<PPB_IMEInputEvent_1_0>()->GetSelection(ime_event, start, end);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_IMEInputEvent */

		#pragma region /* Begin entry point methods for PPB_Instance */

		PEPPER_EXPORT PP_Bool PPB_Instance_BindGraphics(PP_Instance instance, PP_Resource device) {
			if (has_interface<PPB_Instance_1_0>()) {
				return get_interface<PPB_Instance_1_0>()->BindGraphics(instance, device);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Instance_IsFullFrame(PP_Instance instance) {
			if (has_interface<PPB_Instance_1_0>()) {
				return get_interface<PPB_Instance_1_0>()->IsFullFrame(instance);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Instance */

		#pragma region /* Begin entry point methods for PPB_MessageLoop */

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_Create(PP_Instance instance) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_GetForMainThread(void) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->GetForMainThread();
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_GetCurrent(void) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->GetCurrent();
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_AttachToCurrentThread(PP_Resource message_loop) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->AttachToCurrentThread(message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_Run(PP_Resource message_loop) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->Run(message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_PostWork(PP_Resource message_loop, struct PP_CompletionCallback callback, int64_t delay_ms) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->PostWork(message_loop, callback, delay_ms);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_PostQuit(PP_Resource message_loop, PP_Bool should_destroy) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->PostQuit(message_loop, should_destroy);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_MessageLoop */

		#pragma region /* Begin entry point methods for PPB_Messaging */

		PEPPER_EXPORT void PPB_Messaging_PostMessage(PP_Instance instance, struct PP_Var message) {
			if (has_interface<PPB_Messaging_1_2>()) {
				get_interface<PPB_Messaging_1_2>()->PostMessage(instance, message);
			}
			else if (has_interface<PPB_Messaging_1_0>()) {
				get_interface<PPB_Messaging_1_0>()->PostMessage(instance, message);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_Messaging_RegisterMessageHandler(PP_Instance instance, void* user_data, const struct PPP_MessageHandler_0_2* handler, PP_Resource message_loop) {
			if (has_interface<PPB_Messaging_1_2>()) {
				return get_interface<PPB_Messaging_1_2>()->RegisterMessageHandler(instance, user_data, handler, message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_Messaging_UnregisterMessageHandler(PP_Instance instance) {
			if (has_interface<PPB_Messaging_1_2>()) {
				get_interface<PPB_Messaging_1_2>()->UnregisterMessageHandler(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_Messaging */

		#pragma region /* Begin entry point methods for PPB_MouseCursor */

		PEPPER_EXPORT PP_Bool PPB_MouseCursor_SetCursor(PP_Instance instance, enum PP_MouseCursor_Type type, PP_Resource image, struct PP_Point hot_spot) {
			if (has_interface<PPB_MouseCursor_1_0>()) {
				return get_interface<PPB_MouseCursor_1_0>()->SetCursor(instance, type, image, &hot_spot);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_MouseCursor */

		#pragma region /* Begin entry point methods for PPB_MouseLock */

		PEPPER_EXPORT int32_t PPB_MouseLock_LockMouse(PP_Instance instance, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MouseLock_1_0>()) {
				return get_interface<PPB_MouseLock_1_0>()->LockMouse(instance, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_MouseLock_UnlockMouse(PP_Instance instance) {
			if (has_interface<PPB_MouseLock_1_0>()) {
				get_interface<PPB_MouseLock_1_0>()->UnlockMouse(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_MouseLock */

		#pragma region /* Begin entry point methods for PPB_URLLoader */

		PEPPER_EXPORT PP_Resource PPB_URLLoader_Create(PP_Instance instance) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_IsURLLoader(PP_Resource resource) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->IsURLLoader(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_Open(PP_Resource loader, PP_Resource request_info, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->Open(loader, request_info, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_FollowRedirect(PP_Resource loader, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->FollowRedirect(loader, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_GetUploadProgress(PP_Resource loader, int64_t* bytes_sent, int64_t* total_bytes_to_be_sent) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetUploadProgress(loader, bytes_sent, total_bytes_to_be_sent);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_GetDownloadProgress(PP_Resource loader, int64_t* bytes_received, int64_t* total_bytes_to_be_received) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetDownloadProgress(loader, bytes_received, total_bytes_to_be_received);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Resource PPB_URLLoader_GetResponseInfo(PP_Resource loader) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetResponseInfo(loader);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_ReadResponseBody(PP_Resource loader, void* buffer, int32_t bytes_to_read, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->ReadResponseBody(loader, buffer, bytes_to_read, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_FinishStreamingToFile(PP_Resource loader, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->FinishStreamingToFile(loader, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_URLLoader_Close(PP_Resource loader) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				get_interface<PPB_URLLoader_1_0>()->Close(loader);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_URLLoader */

		#pragma region /* Begin entry point methods for PPB_URLRequestInfo */

		PEPPER_EXPORT PP_Resource PPB_URLRequestInfo_Create(PP_Instance instance) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_IsURLRequestInfo(PP_Resource resource) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->IsURLRequestInfo(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_SetProperty(PP_Resource request, PP_URLRequestProperty property, struct PP_Var value) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->SetProperty(request, property, value);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_AppendDataToBody(PP_Resource request, const void* data, uint32_t len) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->AppendDataToBody(request, data, len);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_AppendFileToBody(PP_Resource request, PP_Resource file_ref, int64_t start_offset, int64_t number_of_bytes, PP_Time expected_last_modified_time) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->AppendFileToBody(request, file_ref, start_offset, number_of_bytes, expected_last_modified_time);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_URLRequestInfo */

		#pragma region /* Begin entry point methods for PPB_URLResponseInfo */

		PEPPER_EXPORT PP_Bool PPB_URLResponseInfo_IsURLResponseInfo(PP_Resource resource) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->IsURLResponseInfo(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_URLResponseInfo_GetProperty(PP_Resource response, PP_URLResponseProperty property) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->GetProperty(response, property);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Resource PPB_URLResponseInfo_GetBodyAsFileRef(PP_Resource response) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->GetBodyAsFileRef(response);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_URLResponseInfo */

		#pragma region /* Begin entry point methods for PPB_Var */

		PEPPER_EXPORT void PPB_Var_AddRef(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				get_interface<PPB_Var_1_2>()->AddRef(var);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				get_interface<PPB_Var_1_1>()->AddRef(var);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				get_interface<PPB_Var_1_0>()->AddRef(var);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Var_Release(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				get_interface<PPB_Var_1_2>()->Release(var);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				get_interface<PPB_Var_1_1>()->Release(var);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				get_interface<PPB_Var_1_0>()->Release(var);
			}
			return ;
		}

		PEPPER_EXPORT struct PP_Var PPB_Var_VarFromUtf8(const char* data, uint32_t len) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarFromUtf8(data, len);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				return get_interface<PPB_Var_1_1>()->VarFromUtf8(data, len);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT const char* PPB_Var_VarToUtf8(struct PP_Var var, uint32_t* len) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarToUtf8(var, len);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				return get_interface<PPB_Var_1_1>()->VarToUtf8(var, len);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				return get_interface<PPB_Var_1_0>()->VarToUtf8(var, len);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_Var_VarToResource(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarToResource(var);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_Var_VarFromResource(PP_Resource resource) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarFromResource(resource);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_Var */

		#pragma region /* Begin entry point methods for PPB_View */

		PEPPER_EXPORT PP_Bool PPB_View_IsView(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsView(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsView(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsView(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetRect(PP_Resource resource, struct PP_Rect* rect) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetRect(resource, rect);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetRect(resource, rect);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->GetRect(resource, rect);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsFullscreen(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsFullscreen(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsFullscreen(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsFullscreen(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsVisible(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsVisible(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsVisible(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsVisible(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsPageVisible(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsPageVisible(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsPageVisible(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsPageVisible(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetClipRect(PP_Resource resource, struct PP_Rect* clip) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetClipRect(resource, clip);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetClipRect(resource, clip);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->GetClipRect(resource, clip);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT float PPB_View_GetDeviceScale(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetDeviceScale(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetDeviceScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT float PPB_View_GetCSSScale(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetCSSScale(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetCSSScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetScrollOffset(PP_Resource resource, struct PP_Point* offset) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetScrollOffset(resource, offset);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_View */

		#pragma region /* Begin entry point methods for PPP_InputEvent */

		PEPPER_EXPORT PP_Bool PPP_InputEvent_HandleInputEvent(PP_Instance instance, PP_Resource input_event) {
			if (has_interface<PPP_InputEvent_0_1>()) {
				return get_interface<PPP_InputEvent_0_1>()->HandleInputEvent(instance, input_event);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPP_InputEvent */

/* Not generating entry point methods for PPP_MessageHandler_0_2 */

		#pragma region /* Begin entry point methods for PPP_Messaging */

		PEPPER_EXPORT void PPP_Messaging_HandleMessage(PP_Instance instance, struct PP_Var message) {
			if (has_interface<PPP_Messaging_1_0>()) {
				get_interface<PPP_Messaging_1_0>()->HandleMessage(instance, message);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPP_Messaging */

		#pragma region /* Begin entry point methods for PPP_MouseLock */

		PEPPER_EXPORT void PPP_MouseLock_MouseLockLost(PP_Instance instance) {
			if (has_interface<PPP_MouseLock_1_0>()) {
				get_interface<PPP_MouseLock_1_0>()->MouseLockLost(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPP_MouseLock */

	}
#ifdef __cplusplus
	}  /* extern "C" */
#endif
}
