NACLSDK_TOOL=../nacl_sdk/naclsdk
NUGET_TOOL=./electron-dotnet/tools/build/nuget.exe

all: 
	@echo Usage:
	@echo    make build 	- builds the bindings
	@echo    make setup 	- sets up the environment for you

build: check
	(cd electron-dotnet; npm install electron; npm install)
	(cd Tools/generator-electron-dotnet; npm install)
	(cd PepperPlugin/src; export NACL_SDK_ROOT=../../../nacl_sdk/pepper_canary/; make)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU)
	(cd PepperSharp; xbuild PepperSharp.csproj /t:Rebuild /p:Configuration=Debug /p:Platform=AnyCPU)
	(cd electron-dotnet/tools; mono ./build/nuget.exe pack ./nuget/Xamarin.PepperSharp.nuspec -outputdirectory ./build/nuget -properties Configuration=Release -basepath ../../PepperSharp)

check:
	@if test ! -x $(NACLSDK_TOOL); then echo "You need to install the nacl_sdk on the parent directory, https://developer.chrome.com/native-client/sdk/download#installing-the-sdk"; exit 1; fi
	@if $(NACLSDK_TOOL) list | egrep -q 'I\*?.*pepper_canary'; then echo $?; else echo "You should install the pepper_canary support using $(NACLSDK_TOOL) update pepper_canary --force"; exit 1; fi
	@if test ! -f $(NUGET_TOOL); then echo "You need to install NuGet.exe.  Run 'make setup'"; exit 1; fi


setup: setup-nacl setup-nuget

setup-nacl:
	curl -O 'http://storage.googleapis.com/nativeclient-mirror/nacl/nacl_sdk/nacl_sdk.zip'

	unzip nacl_sdk.zip -d ../

	../nacl_sdk/naclsdk update pepper_canary # Downloads the real SDK. This takes a while

setup-nuget:
	(mkdir -p electron-dotnet/tools/build)
	(mkdir -p electron-dotnet/tools/build/nuget)

	(cd electron-dotnet/tools/ && { mcs download.cs ; mono download.exe 'http://nuget.org/nuget.exe' ./build/nuget.exe;  mono ./build/nuget.exe update -self; cd -; })
