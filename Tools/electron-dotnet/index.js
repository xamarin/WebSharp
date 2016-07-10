// Attach all the components

var electron_dotnet = module.exports = {};
electron_dotnet.Register = require("./lib/register.js")
electron_dotnet.Embed = require("./lib/embed.js")

//Expose the electron_dotnet object and provide the entrypoint
module.exports = electron_dotnet;
