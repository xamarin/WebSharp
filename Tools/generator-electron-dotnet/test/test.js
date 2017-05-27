var path = require('path');
var assert = require('yeoman-generator').assert;
var helpers = require('yeoman-generator').test;

var env = require('../generators/app/env');

var fs = require('fs');

describe('test electron-dotnet generator', function () {
    this.timeout(10000);

    it('dotnet-sharp', function (done) {
        this.timeout(10000);

        helpers.run(path.join(__dirname, '../generators/app'))
            .withPrompts({
                type: 'dotnet-sharp',
                name: 'testCom',
                displayName: 'Test Com',
                description: 'My TestCom',
                publisher: 'Xamarin',
                gitInit: false
            }) // Mock the prompt answers
            .on('end', function () {
                var expected = {
                    "name": "testCom",
                    "displayName": 'Test Com',
                    "description": "My TestCom",
                    "version": "0.0.1",
                    "publisher": 'Xamarin',
                    "main": "main.js",
                    "scripts": {
                        "start": "electron ."
                    },
                    "devDependencies": {
                        "electron": env.electronVersion
                    }
                };
                assert.file(['package.json', 'README.md', '.vscodeignore', 'index.html', 'main.js', 'renderer.js' , './src/testCom.js', '.gitignore' ]);

                var body = fs.readFileSync('package.json', 'utf8');

                var actual = JSON.parse(body);
                assert.deepEqual(expected, actual);

                done();
            });
    });

    it('dotnet-plugin', function (done) {
        this.timeout(10000);

        helpers.run(path.join(__dirname, '../generators/app'))
            .withPrompts({
                type: 'dotnet-plugin',
                name: 'testCom',
                displayName: 'Test Com',
                description: 'My TestCom',
                publisher: 'Xamarin',
                className: 'Plugin',
                gitInit: false
            }) // Mock the prompt answers
            .on('end', function () {
                var expected = {
                    "name": "testCom",
                    "displayName": 'Test Com',
                    "description": "My TestCom",
                    "version": "0.0.1",
                    "publisher": 'Xamarin',
                    "main": "main.js",
                    "scripts": {
                        "start": "electron ."
                    },
                    "devDependencies": {
                        "electron": env.electronVersion
                    }
                };
                assert.file(['package.json', 'README.md', '.vscodeignore', 'index.html', 'main.js', 'renderer.js' , './src/testCom.cs', '.gitignore' ]);

                var body = fs.readFileSync('package.json', 'utf8');

                var actual = JSON.parse(body);
                assert.deepEqual(expected, actual);

                done();
            });
    });
    it('dotnet-sharp-plugin', function (done) {
        this.timeout(10000);

        helpers.run(path.join(__dirname, '../generators/app'))
            .withPrompts({
                type: 'dotnet-sharp-plugin',
                name: 'testCom',
                displayName: 'Test Com',
                description: 'My TestCom',
                publisher: 'Xamarin',
                className: 'Plugin',
                gitInit: false
            }) // Mock the prompt answers
            .on('end', function () {
                var expected = {
                    "name": "testCom",
                    "displayName": 'Test Com',
                    "description": "My TestCom",
                    "version": "0.0.1",
                    "publisher": 'Xamarin',
                    "main": "main.js",
                    "scripts": {
                        "start": "electron ."
                    },
                    "devDependencies": {
                        "electron": env.electronVersion
                    }
                };
                assert.file(['package.json', 'README.md', '.vscodeignore', 'index.html', 'main.js', 'renderer.js' , './src/testCom.cs', './src/testCom.js', '.gitignore' ]);

                var body = fs.readFileSync('package.json', 'utf8');

                var actual = JSON.parse(body);
                assert.deepEqual(expected, actual);

                done();
            });
    });
    it('dotnet-websharp', function (done) {
        this.timeout(10000);

        helpers.run(path.join(__dirname, '../generators/app'))
            .withPrompts({
                type: 'dotnet-websharp',
                name: 'testCom',
                displayName: 'Test Com',
                description: 'My TestCom',
                publisher: 'Xamarin',
                wsClassName: 'Bridge',
                gitInit: false
            }) // Mock the prompt answers
            .on('end', function () {
                var expected = {
                    "name": "testCom",
                    "displayName": 'Test Com',
                    "description": "My TestCom",
                    "version": "0.0.1",
                    "publisher": 'Xamarin',
                    "main": "main.js",
                    "scripts": {
                        "start": "electron ."
                    },
                    "devDependencies": {
                        "electron": env.electronVersion
                    }
                };
                assert.file(['package.json', 'README.md', '.vscodeignore', 'index.html', 'main.js', 'renderer.js' , './src/Main/MainWindow.cs', './src/Main/MainWindow.csproj', './src/Bridge/Bridge.cs', './src/Bridge/Bridge.csproj', './src/testCom.js', '.gitignore' ]);

                var body = fs.readFileSync('package.json', 'utf8');

                var actual = JSON.parse(body);
                assert.deepEqual(expected, actual);

                done();
            });
    });
});