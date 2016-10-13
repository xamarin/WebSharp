#pragma once

namespace pepper {

	bool debugMode;
	#define DBG(...) if (debugMode) { printf(__VA_ARGS__); printf("\n"); }
}
