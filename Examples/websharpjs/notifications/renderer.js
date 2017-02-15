// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.
var notifications = require("./src/notifications.js");

document.addEventListener('DOMContentLoaded', function() {
    notifications.addNotifications();
})
