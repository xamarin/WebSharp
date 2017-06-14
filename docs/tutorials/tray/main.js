// app is the Module to control application life.
// BrowserWindow is the Module to create native browser window.
const {app, BrowserWindow, ipcMain} = require('electron')

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
var mainWindow = null;
var mainWindowId = 0;
// Set our application icon on Mac
if (process.platform === 'darwin') {
  app.dock.setIcon(__dirname + '/assets/icons/appicon.png');
}

var dotnet = require('electron-dotnet');
//var main = dotnet.func("./src/Main/bin/Debug/MainWindow.dll");
var main = dotnet.func("./src/Main/MainWindow.cs");

function createWindow () {
    main(__dirname, function (error, result) {
        if (error) throw error;
        if (result) {
            mainWindow = BrowserWindow.fromId(result);
            
            // Emitted when the window is going to be closed. 
            // It's emitted before the beforeunload and unload event of the DOM. 
            mainWindow.on('close', function (e) {

              // Check if we should quit
              if (app.shouldQuit) {
                // Dereference the window object, usually you would store windows
                // in an array if your app supports multi windows, this is the time
                // when you should delete the corresponding element.
                mainWindow = null
              }
              else {
                // Calling event.preventDefault() will cancel the close.
                e.preventDefault();
                // Then we hide the main window.
                mainWindow.hide()
              }
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