
all: 
	@echo Usage:
	@echo    make build 	- builds the example project
	@echo    make run 		- installs the environment and runs the example

SRCS = Graphics_2D.cs

build: $(SRCS)
	(xbuild Graphics_2D.csproj /t:Rebuild /p:Configuration=Debug /p:Platform=AnyCPU)

run: build
	npm install && npm start

