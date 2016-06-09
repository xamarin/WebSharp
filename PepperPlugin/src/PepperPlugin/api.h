#include "stdafx.h"
#include <stdlib.h>
#include <mono/jit/jit.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/mono-config.h>
#include <mono/metadata/loader.h>
#include <mono/metadata/exception.h>
#include <mono/metadata/mono-gc.h>
#include <stdint.h>

#include "ppapi/cpp/var.h"

#define GETTER   /* nothing, just a marker */
#define SETTER void
#define FUNC
#define FUNCI
#define FUNCP    /* define only the external binding */
#define NAMESPACE(x) namespace Mono##x {
#define PROPERTY(type,name)
#define PROPERTY_GETTER(type,name)
#define END_SIMPLE_PROPERTY
#define SIMPLE_PROPERTY(owner,type,name,apiMethod) \
FUNCI type Get##name## (owner* obj) \
{\
	return obj->apiMethod();\
}\
\
FUNCI void Set##name## (owner* obj, type value)\
{\
	obj->set_##apiMethod##(value);\
}\
\
PROPERTY(type, name)\
END_SIMPLE_PROPERTY

#define MONO_ASSERT(condition, exception) if (!(condition)) mono_raise_exception (exception); 
#define MONO_ASSERT_ARGUMENT(condition, argument, message) MONO_ASSERT(condition, mono_get_exception_argument (argument, message))
#define MONO_ASSERT_INVALIDOP(condition, message) MONO_ASSERT(condition, mono_get_exception_invalid_operation (message))
#define MONO_ASSERT_NOT_SUPPORTED(condition, message) MONO_ASSERT(condition, mono_get_exception_not_supported(message))

#define END_NAMESPACE }
#define CLASS(x) namespace Mono##x {
#define STATIC_CLASS(x) namespace Mono##x {
#define END_CLASS } 
#define NULLALLOWED /* nothing, just a marker */

//
// This is a convenience class that can be used to easily convert an incoming
// MonoString* parameter (what is used for Internal Calls) into a C string
//
// You can use this to ensure that the returned string is properly deallocated
// after it was used
//
class MonoToUtf8 {
public:
	MonoToUtf8(MonoString *monostring) { strptr = mono_string_to_utf8(monostring); }
	~MonoToUtf8() { mono_free(strptr); }
	const char *str() { return strptr; }
	pp::Var to_pp_var() { return pp::Var(strptr); }
private:
	char *strptr;
};

