# Yo Code - Electron DotNet Application Generator

We have written a Yeoman generator to help get you started. We plan to add templates for most Electron DotNet application types into this.

## Install the Generator

Install Yeoman and the Electron DotNet generator:

```bash
npm install -g yo generator-electron-dotnet
```

## Run Yo Code
The Yeoman generator will walk you through the steps required to create your Electron DotNet application prompting for the required information.

To launch the generator simply type:

```bash
yo electron-dotnet
```

![The command generator](yocode.png)

## Generator Output

These templates will
* Create a base folder structure
* Template out a rough `package.json`
* Import any assets required for your extension e.g. tmBundles or the VS Code Library
* Set-up `launch.json` for running your Electron DotNet application

## History

* 1.0.0: Generates an Electron DotNet application.

## License

[MIT](LICENSE)
