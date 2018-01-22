#include <emscripten.h>
//#include <emscripten/val.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


typedef enum {
	/* Disables AOT mode */
	MONO_AOT_MODE_NONE,
	/* Enables normal AOT mode, equivalent to mono_jit_set_aot_only (false) */
	MONO_AOT_MODE_NORMAL,
	/* Enables hybrid AOT mode, JIT can still be used for wrappers */
	MONO_AOT_MODE_HYBRID,
	/* Enables full AOT mode, JIT is disabled and not allowed,
	 * equivalent to mono_jit_set_aot_only (true) */
	MONO_AOT_MODE_FULL,
	/* Same as full, but use only llvm compiled code */
	MONO_AOT_MODE_LLVMONLY,
	/* Uses Interpreter, JIT is disabled and not allowed,
	 * equivalent to "--full-aot --interpreter" */
	MONO_AOT_MODE_INTERP,
	/* Same as INTERP, but use only llvm compiled code */
	MONO_AOT_MODE_INTERP_LLVMONLY,
} MonoAotMode;

typedef enum {
	MONO_IMAGE_OK,
	MONO_IMAGE_ERROR_ERRNO,
	MONO_IMAGE_MISSING_ASSEMBLYREF,
	MONO_IMAGE_IMAGE_INVALID
} MonoImageOpenStatus;

typedef struct MonoDomain_ MonoDomain;
typedef struct MonoAssembly_ MonoAssembly;
typedef struct MonoMethod_ MonoMethod;
typedef struct MonoException_ MonoException;
typedef struct MonoString_ MonoString;
typedef struct MonoClass_ MonoClass;
typedef struct MonoImage_ MonoImage;
typedef struct MonoObject_ MonoObject;
typedef struct MonoThread_ MonoThread;
typedef struct _MonoAssemblyName MonoAssemblyName;
typedef struct MonoArray_ MonoArray;

#ifdef FALSE
#undef FALSE
#endif
#define FALSE 0
#ifdef TRUE
#undef TRUE
#endif
#define TRUE  1
typedef int BOOL;

BOOL debugMode = TRUE;

#define DBG(...) if (debugMode) { printf(__VA_ARGS__); printf("\n"); }

extern "C" {

void mono_jit_set_aot_mode (MonoAotMode mode);
MonoDomain*  mono_jit_init_version (const char *root_domain_name, const char *runtime_version);
MonoAssembly* mono_assembly_open (const char *filename, MonoImageOpenStatus *status);
int mono_jit_exec (MonoDomain *domain, MonoAssembly *assembly, int argc, char *argv[]);
void mono_set_assemblies_path (const char* path);
int monoeg_g_setenv(const char *variable, const char *value, int overwrite);
void mono_free (void*);
MonoString* mono_string_new (MonoDomain *domain, const char *text);
MonoDomain* mono_domain_get (void);
MonoClass* mono_class_from_name (MonoImage *image, const char* name_space, const char *name);
MonoMethod* mono_class_get_method_from_name (MonoClass *klass, const char *name, int param_count);
MonoClass* mono_object_get_class (MonoObject *obj);

MonoString* mono_object_to_string (MonoObject *obj, MonoObject **exc);//FIXME Use MonoError variant
char* mono_string_to_utf8 (MonoString *string_obj);
MonoObject* mono_runtime_invoke (MonoMethod *method, void *obj, void **params, MonoObject **exc);
MonoImage* mono_assembly_get_image (MonoAssembly *assembly);
MonoAssembly* mono_assembly_load (MonoAssemblyName *aname, const char *basedir, MonoImageOpenStatus *status);

MonoAssemblyName* mono_assembly_name_new (const char *name);
void mono_assembly_name_free (MonoAssemblyName *aname);
const char* mono_image_get_name (MonoImage *image);
const char* mono_class_get_name (MonoClass *klass);
void mono_add_internal_call (const char *name, const void* method);
MonoString * mono_string_from_utf16 (char *data);
MonoArray* mono_array_new (MonoDomain *domain, MonoClass *eclass, int n);
MonoClass* mono_get_string_class (void);
int mono_runtime_exec_main (MonoMethod *method, MonoArray *args, MonoObject **exc);
MonoAssembly* mono_domain_assembly_open  (MonoDomain *domain, const char *name);

MonoImage* GetImage();
MonoClass* GetClass();

static MonoDomain *root_domain;
static MonoAssembly *assembly = NULL;

// static char*
// m_strdup (const char *str)
// {
// 	if (!str)
// 		return NULL;

// 	int len = strlen (str) + 1;
// 	char *res = malloc (len);
// 	memcpy (res, str, len);
// 	return res;
// }



 static MonoString*
 mono_wasm_invoke_js (MonoString *str, int *is_exception)
 {
	DBG("mono_wasm_invoke_js");

 	if (str == NULL)
 		return NULL;

 	char *native_val = mono_string_to_utf8 (str);
 	char *native_res = (char*)EM_ASM_INT ({
		var str = UTF8ToString ($0);
		try {
			var res = eval (str);
			if (res === null)
				return 0;
			res = res.toString ();
			setValue ($1, 0, "i32");
		} catch (e) {
			res = e.toString ();
			setValue ($1, 1, "i32");
			if (res === null)
				res = "unknown exception";
		}
		var buff = Module._malloc((res.length + 1) * 2);
		stringToUTF16 (res, buff, (res.length + 1) * 2);
		return buff;
 	}, (int)native_val, is_exception);

 	mono_free (native_val);

	if (native_res == NULL)
		return NULL;

	MonoString *res = mono_string_from_utf16 (native_res);
	free (native_res);
 	return res;
 }

bool HasEnvironmentVariable(const char* variableName)
{
    return getenv(variableName) != NULL;
}


void add_internal_calls()
{
	mono_add_internal_call ("Mono.WebAssembly.Runtime::ExecuteJavaScript", (const void*)&mono_wasm_invoke_js);

}

 EMSCRIPTEN_KEEPALIVE void
 mono_wasm_load_runtime (const char *managed_path)
 {
	if (HasEnvironmentVariable("WEBSHARP_WASM_DEBUG"))
	{
		debugMode = TRUE;
	}

	DBG("mono_wasm_load_runtime");

	monoeg_g_setenv ("MONO_LOG_LEVEL", "debug", 1);
	monoeg_g_setenv ("MONO_LOG_MASK", "gc", 1);
	mono_jit_set_aot_mode (MONO_AOT_MODE_INTERP_LLVMONLY);
	mono_set_assemblies_path (managed_path);
	root_domain = mono_jit_init_version ("mono", "v4.0.30319");

	MonoImageOpenStatus status;
	MonoAssemblyName* aname = mono_assembly_name_new ("monoembedding");
	assembly = mono_assembly_load (aname, NULL, &status);
	mono_assembly_name_free (aname);
	MonoClass* klass = mono_class_from_name(mono_assembly_get_image(assembly), "", "MonoEmbedding");
	MonoMethod* main = mono_class_get_method_from_name(klass, "Main", -1);
	MonoException* exc;
	MonoArray* args = mono_array_new(mono_domain_get(), mono_get_string_class(), 0);
	mono_runtime_exec_main(main, args, (MonoObject**)&exc);	 

	add_internal_calls();
}

EMSCRIPTEN_KEEPALIVE void
 mono_wasm_mount_runtime (const char *assemblies_path, const char *mount_point )
 {
 	if (HasEnvironmentVariable("WEBSHARP_WASM_DEBUG"))
	{
		debugMode = TRUE;
	}

	DBG("mono_wasm_mount_runtime");
	
	DBG("Trying to mount MONO Assemblies path");
	// mount the mono_path folder as a NODEFS instance
	// inside of emscripten
	EM_ASM({
		var mapToPath = UTF8ToString ($0);
		var mountPoint = UTF8ToString ($1);
		FS.mkdir(mountPoint);
		FS.mount(NODEFS, { root: mapToPath }, mountPoint);
	},(int)assemblies_path, (int)mount_point);

	DBG("Done mounting MONO Assemblies path");

 	monoeg_g_setenv ("MONO_LOG_LEVEL", "debug", 1);
 	monoeg_g_setenv ("MONO_LOG_MASK", "gc", 1);
 	mono_jit_set_aot_mode (MONO_AOT_MODE_INTERP_LLVMONLY);
	mono_set_assemblies_path (mount_point);
 	root_domain = mono_jit_init_version ("mono", "v4.0.30319");

   	MonoImageOpenStatus status;
	MonoAssemblyName* aname = mono_assembly_name_new ("monoembedding");
	assembly = mono_assembly_load (aname, NULL, &status);
	mono_assembly_name_free (aname);
    MonoClass* klass = mono_class_from_name(mono_assembly_get_image(assembly), "", "MonoEmbedding");
    MonoMethod* main = mono_class_get_method_from_name(klass, "Main", -1);
    MonoException* exc;
    MonoArray* args = mono_array_new(mono_domain_get(), mono_get_string_class(), 0);
    mono_runtime_exec_main(main, args, (MonoObject**)&exc);	 

 	add_internal_calls();
}

EMSCRIPTEN_KEEPALIVE MonoAssembly*
mono_wasm_assembly_load (const char *name)
{
	DBG("mono_wasm_assembly_load");

	MonoImageOpenStatus status;
	MonoAssemblyName* aname = mono_assembly_name_new (name);
	if (!name)
		return NULL;

	MonoAssembly *res = mono_assembly_load (aname, NULL, &status);
	mono_assembly_name_free (aname);

	return res;
}

EMSCRIPTEN_KEEPALIVE MonoClass*
mono_wasm_assembly_find_class (MonoAssembly *assembly, const char *name_space, const char *name)
{
	DBG("mono_wasm_assembly_find_class");
	return mono_class_from_name (mono_assembly_get_image (assembly), name_space, name);
}

EMSCRIPTEN_KEEPALIVE MonoMethod*
mono_wasm_assembly_find_method (MonoClass *klass, const char *name, int arguments)
{
	DBG("mono_wasm_assembly_find_method");
	return mono_class_get_method_from_name (klass, name, arguments);
}

EMSCRIPTEN_KEEPALIVE MonoObject*
mono_wasm_invoke_method (MonoMethod *method, MonoObject *this_arg, void *params[], int* got_exception)
{
	DBG("mono_wasm_invoke_method");
	MonoObject *exc = NULL;
	MonoObject *res = mono_runtime_invoke (method, this_arg, params, &exc);
	*got_exception = 0;

	if (exc) {
		*got_exception = 1;

		MonoObject *exc2 = NULL;
		res = (MonoObject*)mono_object_to_string (exc, &exc2); 
		if (exc2)
			res = (MonoObject*) mono_string_new (root_domain, "Exception Double Fault");
		return res;
	}

	return res;
}

MonoImage* GetImage()
{
	DBG("GetImage");
    return mono_assembly_get_image(assembly);
}

MonoClass* GetClass()
{
	DBG("GetClass");
    static MonoClass* klass;

    if (!klass)
        klass = mono_class_from_name(GetImage(), "", "MonoEmbedding");

    return klass;
}

EMSCRIPTEN_KEEPALIVE MonoObject*
mono_wasm_get_clr_func_reflection_wrap_func(const char* assemblyFile, const char* typeName, const char* methodName, MonoException ** exc)
{
	DBG("GetClrFuncReflectionWrapFunc");
    static MonoMethod* method;
    void* params[3];

    if (!method)
    {
        method = mono_class_get_method_from_name(GetClass(), "GetFunc", 3);
    }

    params[0] = mono_string_new(mono_domain_get(), assemblyFile);
    params[1] = mono_string_new(mono_domain_get(), typeName);
    params[2] = mono_string_new(mono_domain_get(), methodName);
    MonoObject* action = mono_runtime_invoke(method, NULL, params, (MonoObject**)exc);

    return action;
}

EMSCRIPTEN_KEEPALIVE MonoObject*
mono_wasm_invoke_clr_wrapped_func(MonoObject *func, void *params[], int* got_exception)
{
	DBG("mono_wasm_invoke_clr_wrapped_func");

	MonoClass* klass = mono_object_get_class(func);
	if (!klass)
		DBG("mono_wasm_invoke_clr_wrapped_func::Error - no class for func");
    
	MonoMethod* method;
	method = mono_class_get_method_from_name(klass, "Invoke", -1);
	if (!method)
		DBG("mono_wasm_invoke_clr_wrapped_func::Error - Invoke method not found.");

	MonoObject *exc = NULL;

	MonoObject* invocationResult = mono_runtime_invoke(method, func, params, (MonoObject**)exc);

	return invocationResult;
}

EMSCRIPTEN_KEEPALIVE char *
mono_wasm_string_get_utf8 (MonoString *str)
{
	return mono_string_to_utf8 (str); //XXX JS is responsible for freeing this
}

EMSCRIPTEN_KEEPALIVE MonoString *
mono_wasm_string_from_js (const char *str)
{
	return mono_string_new (root_domain, str);
}

}