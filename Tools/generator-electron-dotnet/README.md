# Yo Code - Electron DotNet Application Generator

We have written a Yeoman generator to help get you started. We plan to add templates for most Electron DotNet application types into this.

## Install the Generator

Install Yeoman and the Electron DotNet generator:

```bash
npm install -g yo generator-electron-dotnet
```

You can also install with a symlink instead of installing globally.

* Mac

```bash
WebSharp$ cd Tools/generator-electron-dotnet
WebSharp/Tools/generator-electron-dotnet$ npm link  # link-install the `electron-dotnet` generator package
```

* Windows

```bash
WebSharp> cd Tools\generator-electron-dotnet
WebSharp\Tools\generator-electron-dotnet> npm link  # link-install the `electron-dotnet` generator package
```



The previous command will setup a symbolic link to the Yeoman generator so any changes that are pulled in from development will automatically be made available to the template generation.

## Run Yo Code
The Yeoman generator will walk you through the steps required to create your Electron DotNet application prompting for the required information.

To launch the generator simply type:

```bash
yo electron-dotnet
```

![The electron-dotnet generator](./../../docs/getting-started/screenshots/yogen-wsa-mac.png)

## Generator Output

See [Getting Started WebSharp Electron Application](./../../docs/getting-started/getting-started-websharp-electron-application.md) documentation.

## History

* 1.0.0: Generates an Electron DotNet application.

## License

[MIT](LICENSE)
