// PepperPlugin.cpp : Defines the exported functions for the DLL application.
//
#include "PepperPlugin.h"

#include "stdafx.h"

// Copyright (c) 2013 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

#include <stdio.h>
#include <stdlib.h>

#include "ppapi/c/ppb_image_data.h"
#include "ppapi/cpp/graphics_2d.h"
#include "ppapi/cpp/image_data.h"
#include "ppapi/cpp/input_event.h"
#include "ppapi/cpp/instance.h"
#include "ppapi/cpp/module.h"
#include "ppapi/cpp/point.h"
#include "ppapi/utility/completion_callback_factory.h"

#include <mono/metadata/mono-config.h>
#include <mono/metadata/threads.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/mono-gc.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/class.h>
#include <mono/metadata/metadata.h>
#include <mono/jit/jit.h>
#include <mono/metadata/debug-helpers.h>

#ifdef WIN32
#undef PostMessage
// Allow 'this' in initializer list
#pragma warning(disable : 4355)
#endif

namespace {
	bool initialised = false;
	MonoDomain* monoDomain = NULL;

	const char* getEnvVar(const char *key)
	{

		char * val = getenv(key);
		return val == NULL ? "" : val;
	}

	MonoDomain* load_domain()
	{
		MonoDomain* newDomain = mono_domain_create_appdomain("PepperPlugin App Domain", NULL);
		if (!newDomain) {
			fprintf(stderr, "load_domain: Error creating domain\n");
			return nullptr;
		}

		if (!mono_domain_set(newDomain, false)) {
			fprintf(stderr, "load_domain: Error setting domain\n");
			return nullptr;
		}

		return mono_domain_get();
	}

	void InitMono() {

		if (initialised)
			return;

		printf("Initialising Mono!\n");
		// point to the relevant directories of the Mono installation
		auto mono_root = std::string(getEnvVar("MONO_ROOT"));
		auto mono_lib = mono_root + "/lib";
		auto mono_etc = mono_root + "/etc";
		mono_set_dirs(mono_lib.c_str(), mono_etc.c_str());

		//mono_set_dirs("C:\\Program Files (x86)\\Mono\\lib",
		//	"C:\\Program Files (x86)\\Mono\\etc");

		////// load the default Mono configuration file in 'etc/mono/config'
		mono_config_parse(nullptr);

		MonoDomain* domain = mono_jit_init_version("PepperPlugin Domain", "v4.0.30319");

		initialised = true;

		if (!monoDomain)
		{
			printf("load_domain\n");
			monoDomain = load_domain();
		}
	}

}  // namespace

class PluginInstance : public pp::Instance {
public:
	explicit PluginInstance(PP_Instance instance)
		: pp::Instance(instance) {}

	~PluginInstance() {  }

	virtual bool Init(uint32_t argc, const char* argn[], const char* argv[]) {
		
		InitMono();

		for (unsigned int x = 0; x < argc; x++)
		{
			//printf("argn = %s argv = %s\n", argn[x], argv[x]);
			auto property = std::string(argn[x]);
			if (property == "assembly")
			{
				printf("argn = %s argv = %s\n", argn[x], argv[x]);
				//// open our Example.dll assembly
				MonoAssembly* assembly = mono_domain_assembly_open(monoDomain,
					argv[x]);
				if (assembly )
					MonoImage* monoImage = mono_assembly_get_image(assembly);
				else
					fprintf(stderr, "Error loading assembly: %s\n", argv[x]);
			}
		}

		return true;
	}
};

class PluginModule : public pp::Module {
public:
	PluginModule() : pp::Module() {}
	virtual ~PluginModule() {}

	virtual pp::Instance* CreateInstance(PP_Instance instance) {
		return new PluginInstance(instance);
	}
};



namespace pp {

	Module* CreateModule() { return new PluginModule(); }
}  // namespace pp
