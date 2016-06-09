
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
//using namespace pepper;

NAMESPACE(Pepper)

//MonoDomain*		domain()		{ return mono_environment->domain(); }

CLASS(PPInstance)

FUNC void LogToConsole(intptr_t handle, PP_LogLevel level, MonoString* value)
{
	((pp::Instance)handle).LogToConsole(level, MonoToUtf8(value).to_pp_var());
	
}

FUNC void LogToConsoleWithSource(intptr_t handle, PP_LogLevel level, MonoString* source, MonoString* value)
{
	((pp::Instance)handle).LogToConsoleWithSource(level, MonoToUtf8(source).to_pp_var(), MonoToUtf8(value).to_pp_var());
}

FUNC void RequestInputEvents(intptr_t handle, PP_InputEvent_Class event_classes)
{
	printf("events: %d\n", event_classes);
	((pp::Instance)handle).RequestInputEvents(event_classes);
}

END_CLASS

END_NAMESPACE

#include "glue.inc"
