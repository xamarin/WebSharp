// Overview of electron-dotnet.js: https://github.com/xamarin/WebSharp/tree/master/electron-dotnet


var dotnet = require('electron-dotnet');

var createCounter = dotnet.func(function () {/*
    async (start) => 
    {
        var k = (int)start;
        return (Func<object,Task<object>>)(
            async (i) => 
            { 
                return k++;
            }
        );
    }
*/});

function create_Counter()
{
    var nextNumber = createCounter(12, true);
    console.log(nextNumber(null, true));
    console.log(nextNumber(null, true));
    console.log(nextNumber(null, true));
}
