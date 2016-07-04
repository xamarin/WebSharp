#pragma once

#ifndef PEPPER_ENTRYPOINTS_H_
#define PEPPER_ENTRYPOINTS_H_

// Windows headers will redefine SendMessage.
#ifdef SendMessage
#undef SendMessage
#endif

#include "all_ppapi_c_includes.h"
#include "all_ppapi_cpp_includes.h"

#ifndef PEPPER_EXPORT
#define PEPPER_EXPORT __declspec(dllexport)
#endif


#endif  /* PEPPER_ENTRYPOINTS_H_ */

