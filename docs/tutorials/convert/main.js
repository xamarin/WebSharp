// app is the Module to control application life.
// BrowserWindow is the Module to create native browser window.
const {app, BrowserWindow} = require('electron')

app.dock.setIcon(__dirname + "/assets/icons/images/appicon_256x256.png");

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
var mainWindow = null;

if (process.platform === 'win32') {
  if (process.arch === 'x64')
      process.env.PATH = "c:\\Program Files\\Mono\\bin;" + process.env.PATH;
  else
      process.env.PATH = "c:\\\Program Files (x86)\\\Mono\\bin;" + process.env.PATH;
}

var dotnet = require('electron-dotnet');
if (!require('electron-is-dev'))
  var main = dotnet.func(__dirname + "/src/Main/bin/Debug/MainWindow.dll");
else

  var main = dotnet.func({source : __dirname + "/src/Main/MainWindow.cs", 
      references: [__dirname + "/src/lib/MediaToolkit.dll"],
      itemgroup: [__dirname + "/src/Main/Convert.cs"],
                          symbols: ["DEV"]});

function createWindow () {
    main(__dirname, function (error, result) {
        if (error) throw error;
        if (result) {
            mainWindow = BrowserWindow.fromId(result);
            // Emitted when the window is closed.
            mainWindow.on('closed', function () {
              // Dereference the window object, usually you would store windows
              // in an array if your app supports multi windows, this is the time
              // when you should delete the corresponding element.
              mainWindow = null
            })
            
        }
      });
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', createWindow)

// Quit when all windows are closed.
app.on('window-all-closed', function () {
  // On OS X it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

app.on('activate', function () {
  // On OS X it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (mainWindow === null) {
    createWindow()
  }
})

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.