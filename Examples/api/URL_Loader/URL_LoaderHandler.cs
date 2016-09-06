using System;
using System.Text;

using PepperSharp;
using System.Threading.Tasks;

namespace URL_Loader
{
    // URLLoaderHandler is used to download data from |url|. When download is
    // finished or when an error occurs, it posts a message back to the browser
    // with the results encoded in the message as a string and self-destroys.
    //
    // URLLoader.GetDownloadProgress() is used to to allocate the StringBuilder memory
    // required for urlResponseBody before the download starts.  Other performance improvements 
    // made as outlined in this bug: http://code.google.com/p/chromium/issues/detail?id=103947
    //
    // EXAMPLE USAGE:
    // var handler = new URL_LoaderHandler(instance,url);
    // handler.Start();
    //
    public class URL_LoaderHandler : IDisposable
    {
        Instance instance;
        string url;         // URL to be downloaded.
        URLRequestInfo urlRequest;
        URLLoader urlLoader; // URLLoader provides an API to download URLs.
        byte[] buffer = new byte[READ_BUFFER_SIZE];              // Temporary buffer for reads.
        StringBuilder urlResponseBody;  // Contains accumulated downloaded data.

        bool disposed;

        const int READ_BUFFER_SIZE = 32768;

        MessageLoop bodyMessageLoop;

        public URL_LoaderHandler(Instance instance, string url)
        {
            this.instance = instance;
            this.url = url;

            urlRequest = new URLRequestInfo(instance);
            urlLoader = new URLLoader(instance);
            urlRequest.SetURL(url);
            urlRequest.SetMethod("GET");
            urlRequest.SetRecordDownloadProgress(true);
            urlRequest.SetFollowRedirects(true);
        }

        #region Implement IDisposable.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    buffer = null;
                    if (bodyMessageLoop != null)
                        bodyMessageLoop.Dispose();
                }

                disposed = true;
            }
        }

        ~URL_LoaderHandler()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
       
        public async Task Start()
        {
            bodyMessageLoop = new MessageLoop(instance);
            bodyMessageLoop.Start();

            var openresult = await urlLoader.OpenAsync(urlRequest, bodyMessageLoop);
            if (openresult != PPError.Ok)
            {
                ReportResultAndDie(url, "URLLoader.Open() failed", false);
                return;
            }
            // Here you would process the headers. A real program would want to at least
            // check the HTTP code and potentially cancel the request.
            // var response = loader.ResponseInfo;
            var response = urlLoader.ResponseInfo;
            instance.LogToConsole(PPLogLevel.Warning, response.ToString());

            // Try to figure out how many bytes of data are going to be downloaded in
            // order to allocate memory for the response body in advance (this will
            // reduce heap traffic and also the amount of memory allocated).
            // It is not a problem if this fails, it just means that the
            // urlResponseBody.Append call in URLLoaderHandler:AppendDataBytes()
            // will allocate the memory later on.
            long bytesReceived = 0;
            long totalBytesToBeReceived = 0;
            if (urlLoader.GetDownloadProgress(out bytesReceived, out totalBytesToBeReceived))
            {
                if (totalBytesToBeReceived > 0)
                {
                    urlResponseBody = new StringBuilder((int)totalBytesToBeReceived);
                }
            }

            // We will not use the download progress anymore, so just disable it.
            urlRequest.SetRecordDownloadProgress(false);
            // Start streaming.
            await ReadBody();

        }

        async Task ReadBody()
        {
            // We will run this on a separate MessageLoop so we can read as fast as we can
            var result = PPError.Ok;
            do
            {
                result = await urlLoader.ReadResponseBodyAsync(buffer, READ_BUFFER_SIZE, bodyMessageLoop);
                if (result > 0)
                {
                    AppendDataBytes(buffer, (int)result);
                }
                else
                {
                    if (result == PPError.Ok)
                    {
                        // Streaming the file is complete
                        ReportResultAndDie(url, urlResponseBody.ToString(), true);
                    }
                    else
                    {
                        // A read error occurred.
                        ReportResultAndDie(
                            url, $"URL_Loader.ReadResponseBody() result {result}", false);
                    }
                }
            } while (result > 0);

        }

        void AppendDataBytes(byte[] buffer, int num_bytes)
        {
            if (num_bytes <= 0)
                return;
            // Make sure we don't get a buffer overrun.
            num_bytes = Math.Min(READ_BUFFER_SIZE, num_bytes);
            urlResponseBody.Append(Encoding.UTF8.GetString(this.buffer), 0, num_bytes);

        }

        void ReportResultAndDie(string fname,
                                          string text,
                                          bool success)
        {
            ReportResult(fname, text, success);
            Dispose(true);
        }

        void ReportResult(string fname,
                                    string text,
                                    bool success)
        {
            if (success)
                Console.WriteLine("URL_LoaderHandler::ReportResult(Ok).");
            else
                Console.WriteLine($"URL_LoaderHandler::ReportResult(Error). {text}");

            if (instance != null)
            {
                instance.PostMessage(fname + "\n" + text + "\n");
            }
        }
    }
}
