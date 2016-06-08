
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

#include "ppapi/c/ppb_image_data.h"
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
FUNC void Info(MonoString *system, MonoString *message) //Flags:STATIC
{
}

FUNC void LogToConsole(intptr_t handle, int level, MonoString* string)
{
	auto instance = pp::Instance(handle);
	auto msg = mono_string_to_utf8(string);
	instance.LogToConsole(PP_LOGLEVEL_LOG, pp::Var(msg));
	mono_free(msg);
}

END_CLASS

END_NAMESPACE

#include "glue.inc"
