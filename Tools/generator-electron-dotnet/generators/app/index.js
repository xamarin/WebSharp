/*---------------------------------------------------------
 * Copyright (C) Microsoft Corporation. All rights reserved.
 *--------------------------------------------------------*/
'use strict';

var yeoman = require('yeoman-generator');
var yosay = require('yosay');
var chalk = require('chalk');

var path = require('path');
var fs = require('fs');
var request = require('request');
var validator = require('./validator');
var env = require('./env');

module.exports = yeoman.generators.Base.extend({

    constructor: function () {
        yeoman.generators.Base.apply(this, arguments);
        this.argument('applicationType', { type: String, required: false });
        this.argument('applicationName', { type: String, required: false });
        this.argument('applicationParam', { type: String, required: false });
        this.argument('applicationParam2', { type: String, required: false });

        this.applicationConfig = Object.create(null);
        this.applicationConfig.installDependencies = false;
        this.applicationConfig.electronprebuilt = env.electronVersion;
    },

    initializing: {

        // Welcome
        welcome: function () {
            this.log(yosay('Welcome to the Electron DotNet generator!'));
        }
    },

    prompting: {

        // Ask for application type
        askForType: function () {
            if (this.applicationType) {
                var applicationTypes = ['sharp', 'plugin', 'sharp-plugin', 'snippets', 'command-ts', 'command-js'];
                if (applicationTypes.indexOf(this.applicationType) !== -1) {
                    this.applicationConfig.type = 'dotnet-' + this.applicationType;
                } else {
                    this.env.error("Invalid extension type: " + this.applicationType + '. Possible types are :' + applicationTypes.join(', '));
                }
                return;
            }

            var done = this.async();
            this.prompt({
                type: 'list',
                name: 'type',
                message: 'What type of Electron DotNet app do you want to create?',
                choices: [
                    {
                        name: 'Node.js .NET Scripting',
                        value: 'dotnet-sharp'
                    },
                    {
                        name: 'PepperPlugin: Native Client',
                        value: 'dotnet-plugin'
                    },
                    {
                        name: 'Node.js .NET Scripting and PepperPlugin: Native Client',
                        value: 'dotnet-sharp-plugin'
                    },
                   {
                        name: 'WebSharp Electron Application',
                        value: 'dotnet-websharp'
                    }
                ]
            }, function (typeAnswer) {
                this.applicationConfig.type = typeAnswer.type;
                done();
            }.bind(this));
        },

        // Ask for application display name ("displayName" in package.json)
        askForApplicationDisplayName: function () {
            if (this.extensionDisplayName) {
                this.applicationConfig.displayName = this.extensionDisplayName;
                return;
            }

            var done = this.async();
            this.prompt({
                type: 'input',
                name: 'displayName',
                message: 'What\'s the name of your application?',
                default: this.applicationConfig.displayName
            }, function (displayNameAnswer) {
                this.applicationConfig.displayName = displayNameAnswer.displayName;
                done();
            }.bind(this));
        },

        // Ask for applicaton id ("name" in package.json)
        askForApplicationId: function () {
            if (this.applicationName) {
                this.applicationConfig.name = this.applicationName;
                return;
            }

            var done = this.async();
            this.prompt({
                type: 'input',
                name: 'name',
                message: 'What\'s the identifier of your application?',
                default: this.applicationConfig.name || this.applicationConfig.displayName.toLowerCase().replace(/[^a-z0-9]/g, '-'),
                validate: validator.validateAppId
            }, function (nameAnswer) {
                this.applicationConfig.name = nameAnswer.name;
                done();
            }.bind(this));
        },

        askForClassName: function () {
            var done = this.async();
            if (['dotnet-plugin', 'dotnet-sharp-plugin'].indexOf(this.applicationConfig.type) === -1) {
                done();
                return;
            }

            this.prompt({
                type: 'input',
                name: 'className',
                message: 'What\'s the name of your plugin class?',
                default: this.applicationConfig.displayName.replace(/[^a-zA-Z0-9]/g, '_'),
            }, function (classAnswer) {
                this.applicationConfig.className = classAnswer.className;
                done();
            }.bind(this));
        },
        askForWSClassName: function () {
            var done = this.async();
            if (['dotnet-websharp'].indexOf(this.applicationConfig.type) === -1) {
                done();
                return;
            }

            this.prompt({
                type: 'input',
                name: 'wsClassName',
                message: 'What\'s the name of your class?',
                default: this.applicationConfig.displayName.replace(/[^a-zA-Z0-9]/g, '_'),
            }, function (classAnswer) {
                this.applicationConfig.wsClassName = classAnswer.wsClassName;
                done();
            }.bind(this));
        },
        // Ask for application description
        askForApplicationDescription: function () {
            var done = this.async();
            this.prompt({
                type: 'input',
                name: 'description',
                message: 'What\'s the description of your application?'
            }, function (descriptionAnswer) {
                this.applicationConfig.description = descriptionAnswer.description;
                done();
            }.bind(this));
        },

        // Ask for publisher name
        askForPublisherName: function () {
            var done = this.async();
            this.prompt({
                type: 'input',
                name: 'publisher',
                message: 'What\'s your publisher name?',
                store: true,
                validate: validator.validatePublisher
            }, function (publisherAnswer) {
                this.applicationConfig.publisher = publisherAnswer.publisher;
                done();
            }.bind(this));
        },

        askForGit: function () {
            var done = this.async();
            if (['dotnet-sharp', 'dotnet-plugin', 'dotnet-sharp-plugin', 'dotnet-websharp'].indexOf(this.applicationConfig.type) === -1) {
                done();
                return;
            }

            this.prompt({
                type: 'confirm',
                name: 'gitInit',
                message: 'Initialize a git repository?',
                default: true
            }, function (gitAnswer) {
                this.applicationConfig.gitInit = gitAnswer.gitInit;
                done();
            }.bind(this));
        },
    },

    // Write files
    writing: function () {
        this.log('Writing ' + this.applicationConfig.name + ' has been created!');

        this.sourceRoot(path.join(__dirname, './templates/' + this.applicationConfig.type));

        switch (this.applicationConfig.type) {
            case 'dotnet-sharp':
                this._writingDotNetSharp();
                break;
            case 'dotnet-plugin':
                this._writingDotNetPlugin();
                break;
            case 'dotnet-sharp-plugin':
                this._writingDotNetSharpPlugin();
                break;
            case 'dotnet-websharp':
                this._writingDotNetWebSharp();
                break;
            default:
                //unknown project type
                break;
         }
    },

    // Write DotNet Sharp app
    _writingDotNetSharp: function () {
        var context = this.applicationConfig;

        this.directory(this.sourceRoot() + '/vscode', './.vscode');
        //this.directory(this.sourceRoot() + '/test', './test');

        this.copy(this.sourceRoot() + '/vscodeignore', './.vscodeignore');
        this.copy(this.sourceRoot() + '/gitignore', './.gitignore');
        this.template(this.sourceRoot() + '/README.md', './README.md', context);
        this.template(this.sourceRoot() + '/electron-dotnet-quickstart.md', './electron-dotnet-quickstart.md', context);
        this.copy(this.sourceRoot() + '/jsconfig.json', './jsconfig.json');

        this.template(this.sourceRoot() + '/src/dotnet.js', './src/' + context.name + '.js', context);
        this.template(this.sourceRoot() + '/index.html', './index.html', context);
        this.template(this.sourceRoot() + '/main.js', './main.js', context);
        this.template(this.sourceRoot() + '/package.json', './package.json', context);
        this.template(this.sourceRoot() + '/renderer.js', './renderer.js', context);
        this.template(this.sourceRoot() + '/.eslintrc.json', './.eslintrc.json', context);

        this.applicationConfig.installDependencies = true;
    },

    // Write DotNet Sharp app
    _writingDotNetPlugin: function () {
        var context = this.applicationConfig;

        this.directory(this.sourceRoot() + '/vscode', './.vscode');
        //this.directory(this.sourceRoot() + '/test', './test');

        this.copy(this.sourceRoot() + '/vscodeignore', './.vscodeignore');
        this.copy(this.sourceRoot() + '/gitignore', './.gitignore');
        this.template(this.sourceRoot() + '/README.md', './README.md', context);
        this.template(this.sourceRoot() + '/electron-dotnet-quickstart.md', './electron-dotnet-quickstart.md', context);
        this.copy(this.sourceRoot() + '/jsconfig.json', './jsconfig.json');

        this.template(this.sourceRoot() + '/src/dotnet-plugin.cs', './src/' + context.name + '.cs', context);
        this.template(this.sourceRoot() + '/src/project.json', './src/project.json', context);
        this.template(this.sourceRoot() + '/index.html', './index.html', context);
        this.template(this.sourceRoot() + '/main.js', './main.js', context);
        this.template(this.sourceRoot() + '/package.json', './package.json', context);
        this.template(this.sourceRoot() + '/renderer.js', './renderer.js', context);
        this.template(this.sourceRoot() + '/.eslintrc.json', './.eslintrc.json', context);

        this.applicationConfig.installDependencies = true;
    },

    // Write DotNet Sharp app with Pepper Plugin code module
    _writingDotNetSharpPlugin: function () {
        var context = this.applicationConfig;

        this.directory(this.sourceRoot() + '/vscode', './.vscode');
        //this.directory(this.sourceRoot() + '/test', './test');

        this.copy(this.sourceRoot() + '/vscodeignore', './.vscodeignore');
        this.copy(this.sourceRoot() + '/gitignore', './.gitignore');
        this.template(this.sourceRoot() + '/README.md', './README.md', context);
        this.template(this.sourceRoot() + '/electron-dotnet-quickstart.md', './electron-dotnet-quickstart.md', context);
        this.copy(this.sourceRoot() + '/jsconfig.json', './jsconfig.json');

        this.template(this.sourceRoot() + '/src/dotnet.js', './src/' + context.name + '.js', context);
        this.template(this.sourceRoot() + '/src/dotnet-plugin.cs', './src/' + context.name + '.cs', context);
        this.template(this.sourceRoot() + '/src/project.json', './src/project.json', context);
        this.template(this.sourceRoot() + '/index.html', './index.html', context);
        this.template(this.sourceRoot() + '/main.js', './main.js', context);
        this.template(this.sourceRoot() + '/package.json', './package.json', context);
        this.template(this.sourceRoot() + '/renderer.js', './renderer.js', context);
        this.template(this.sourceRoot() + '/.eslintrc.json', './.eslintrc.json', context);

        this.applicationConfig.installDependencies = true;
    },
    // Write DotNet WebSharp app with source code project file
    _writingDotNetWebSharp: function () {
        var context = this.applicationConfig;

        this.directory(this.sourceRoot() + '/vscode', './.vscode');
        //this.directory(this.sourceRoot() + '/test', './test');

        this.copy(this.sourceRoot() + '/vscodeignore', './.vscodeignore');
        this.copy(this.sourceRoot() + '/gitignore', './.gitignore');
        this.template(this.sourceRoot() + '/README.md', './README.md', context);
        this.template(this.sourceRoot() + '/websharp-quickstart.md', context.name + '-quickstart.md', context);
        this.copy(this.sourceRoot() + '/jsconfig.json', './jsconfig.json');

        this.template(this.sourceRoot() + '/src/websharp.js', './src/' + context.name + '.js', context);
        this.template(this.sourceRoot() + '/src/websharp.cs', './src/' + context.wsClassName + '/' + context.wsClassName + '.cs', context);
        this.template(this.sourceRoot() + '/src/websharp.csproj', './src/' + context.wsClassName + '/' + context.wsClassName + '.csproj', context);
        this.template(this.sourceRoot() + '/src/websharp.sln', './src/' + context.wsClassName + '/' + context.wsClassName + '.sln', context);
        this.template(this.sourceRoot() + '/src/packages.config', './src/' + context.wsClassName + '/' + 'packages.config', context);
        this.template(this.sourceRoot() + '/src/nuget.config', './src/' + context.wsClassName + '/' + 'nuget.config', context);
        this.template(this.sourceRoot() + '/src/websharp_main.cs', './src/Main/MainWindow.cs', context);
        this.template(this.sourceRoot() + '/src/websharp_main.csproj', './src/Main/MainWindow.csproj', context);
        this.template(this.sourceRoot() + '/src/websharp_main.sln', './src/Main/MainWindow.sln', context);
        this.template(this.sourceRoot() + '/src/build.sln', './src/Build.sln', context);
        this.template(this.sourceRoot() + '/src/packages.config', './src/Main/packages.config', context);
        this.template(this.sourceRoot() + '/src/nuget.config', './src/Main/nuget.config', context);
        this.template(this.sourceRoot() + '/index.html', './index.html', context);
        this.template(this.sourceRoot() + '/main.js', './main.js', context);
        this.template(this.sourceRoot() + '/package.json', './package.json', context);
        this.template(this.sourceRoot() + '/renderer.js', './renderer.js', context);
        this.template(this.sourceRoot() + '/.eslintrc.json', './.eslintrc.json', context);

        this.applicationConfig.installDependencies = true;
    },

    // Installation
    install: function () {
        //process.chdir(this.applicationConfig.name);

        if (this.applicationConfig.installDependencies) {
            this.installDependencies({
                npm: true,
                bower: false
            });
        }
    },

    // End
    end: function () {

        // Git init
        if (this.applicationConfig.gitInit) {
            this.spawnCommand('git', ['init', '--quiet']);
        }

        this.log('');
        this.log('Your Electron DotNet application ' + this.applicationConfig.name + ' has been created!');
        this.log('');
        this.log('To start editing with Visual Studio Code, use the following commands:');
        this.log('');
        this.log('     cd to '  + this.applicationConfig.name );
        this.log('     code .');
        this.log('');
        this.log('Open electron-dotnet-quickstart.md inside the new application for further instructions');
        this.log('on how to modify and run your application.');
        this.log('');
        this.log('For more information, also visit https://github.com/xamarin/WebSharp.');
        this.log('\r\n');
    }
});