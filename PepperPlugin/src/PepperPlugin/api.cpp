
#include "stdafx.h"
/*
 * IDEA: use C++ as the "contract API", this file is coded in a way
 * that would be parsable by a trivial tool that would generate the
 * glue between the two universes
 *
 * IDEA: Keep the style in such a way that it is trivial to parse,
 * with something like Perl.
 *
 * Use well known idioms for all public functions
 */
#include "api.h"

#include "ppapi/cpp/graphics_2d.h"
#include "ppapi/cpp/image_data.h"
#include "ppapi/cpp/input_event.h"
#include "ppapi/cpp/instance.h"
#include "ppapi/cpp/module.h"
#include "ppapi/cpp/point.h"
#include "ppapi/utility/completion_callback_factory.h"
#include "ppapi/c/ppb_view.h"

#ifdef __cplusplus
extern "C" {
#endif

	namespace Pepper
	{

		namespace PPInstance
		{

			PEPPER_EXPORT void PPInstance_LogToConsole(intptr_t handle, PP_LogLevel level, char* value)
			{
				((pp::Instance)handle).LogToConsole(level, pp::Var(value));

			}

			PEPPER_EXPORT void PPInstance_LogToConsoleWithSource(intptr_t handle, PP_LogLevel level, char* source, char* value)
			{
				((pp::Instance)handle).LogToConsoleWithSource(level, pp::Var(source), pp::Var(value));
			}

			PEPPER_EXPORT void PPInstance_RequestInputEvents(intptr_t handle, PP_InputEvent_Class event_classes)
			{
				printf("events: %d\n", event_classes);
				((pp::Instance)handle).RequestInputEvents(event_classes);
			}

		}


		namespace PPBView {


			PEPPER_EXPORT bool PPBView_IsVisible(intptr_t handle)
			{
				return ((pp::View)handle).IsVisible();

			}

			PEPPER_EXPORT bool PPBView_IsPageVisible(intptr_t handle)
			{
				return ((pp::View)handle).IsPageVisible();

			}

			PEPPER_EXPORT bool PPBView_IsFullscreen(intptr_t handle)
			{
				return ((pp::View)handle).IsFullscreen();

			}


			PEPPER_EXPORT pp::Rect PPBView_GetRect(intptr_t handle)
			{
				return ((pp::View)handle).GetRect();

			}

			PEPPER_EXPORT float PPBView_GetDeviceScale(intptr_t handle)
			{
				return ((pp::View)handle).GetDeviceScale();

			}

		}

	}

#ifdef __cplusplus
}
#endif
