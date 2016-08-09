NACLSDK_TOOL=../nacl_sdk/naclsdk

all: 
	@echo Usage:
	@echo    make build 	- builds the bindings
	@echo    make setup 	- sets up the environment for you

build: check
	(cd PepperPlugin/src; export NACL_SDK_ROOT=../../../nacl_sdk/pepper_canary/; make)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Debug /p:Platform=AnyCPU)

check:
	@if test ! -x $(NACLSDK_TOOL); then echo "You need to install the nacl_sdk on the parent directory, https://developer.chrome.com/native-client/sdk/download#installing-the-sdk"; exit 1; fi
	@if $(NACLSDK_TOOL) list | grep -q 'I *pepper_canary'; then echo $?; else echo "You should install the pepper_canary support using $(NACLSDK_TOOL) update pepper_canary"; exit 1; fi

