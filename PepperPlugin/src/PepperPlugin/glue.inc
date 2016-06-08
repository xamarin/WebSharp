namespace pepper {

void mono_register_icalls(){
	::mono_add_internal_call("Pepper.PPInstance::_LogToConsole", (void*)MonoPepper::MonoPPInstance::LogToConsole);
}
}
