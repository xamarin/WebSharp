using System;

using PepperSharp;

namespace URL_Loader
{
    public class URL_Loader : Instance
    {
        const string LOAD_URL_METHOD_ID = "getUrl";
        static char messageArgumentSeparator = ':';



        // Called by the browser to handle the postMessage() call in Javascript.
        // The message in this case is expected to contain the string 'getUrl'
        // followed by a ':' separator, then the URL to fetch.  If a valid message
        // of the form 'getUrl:URL' is received, then start up an asynchronous
        // download of URL.  In the event that errors occur, this method posts an
        // error string back to the browser.
        public override void HandleMessage(PPVar messageToHandle)
        {
            var varMsg = new Var(messageToHandle);
            PPBConsole.Log(this, PPLogLevel.Log, messageToHandle);
            if (!varMsg.IsString)
            {
                return;
            }
            var message = varMsg.AsString();

            if (message.StartsWith(LOAD_URL_METHOD_ID))
            {
                // The argument to getUrl is everything after the first ':'.
                var sepPos = message.IndexOf(messageArgumentSeparator);
                if (sepPos >= 0) {
                    var url = message.Substring(sepPos + 1);
                    Console.WriteLine($"URL_LoaderInstance HandleMessage ({message}, {url})");
                    try
                    {
                        var handler = new URL_LoaderHandler(this, url);
                        // Starts asynchronous download. When download is finished or when an
                        // error occurs, |handler| posts the results back to the browser
                        // vis PostMessage and self-destroys.
                        handler.Start();
                    }
                    catch (Exception exc)
                    {
                        PPBConsole.LogWithSource(this, PPLogLevel.Error, new Var(exc.Source), new Var(exc.Message));
                    }
                }
            }
        }
    }
}
