NACLSDK_TOOL=../nacl_sdk/naclsdk

all: 
	@echo Usage:
	@echo    make build 	- builds the bindings
	@echo    make setup 	- sets up the environment for you

build: check
	(cd electron-dotnet; npm install electron; npm install)
	(cd PepperPlugin/src; export NACL_SDK_ROOT=../../../nacl_sdk/pepper_canary/; make)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Debug /p:Platform=AnyCPU)

check:
	@if test ! -x $(NACLSDK_TOOL); then echo "You need to install the nacl_sdk on the parent directory, https://developer.chrome.com/native-client/sdk/download#installing-the-sdk"; exit 1; fi
	@if $(NACLSDK_TOOL) list | grep -q 'I *pepper_canary'; then echo $?; else echo "You should install the pepper_canary support using $(NACLSDK_TOOL) update pepper_canary --force"; exit 1; fi

setup:
	curl -O 'http://storage.googleapis.com/nativeclient-mirror/nacl/nacl_sdk/nacl_sdk.zip'

	unzip nacl_sdk.zip -d ../

	../nacl_sdk/naclsdk update pepper_canary # Downloads the real SDK. This takes a while