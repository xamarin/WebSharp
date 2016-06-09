namespace pepper {

void mono_register_icalls(){
	::mono_add_internal_call("Pepper.PPInstance::_LogToConsole", (void*)MonoPepper::MonoPPInstance::LogToConsole);
	::mono_add_internal_call("Pepper.PPInstance::_LogToConsoleWithSource", (void*)MonoPepper::MonoPPInstance::LogToConsoleWithSource);
	::mono_add_internal_call("Pepper.PPInstance::_RequestInputEvents", (void*)MonoPepper::MonoPPInstance::RequestInputEvents);
}
}
