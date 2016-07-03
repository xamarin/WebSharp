using System;
using System.Text;

using PepperSharp;

namespace URL_Loader
{
    // URLLoaderHandler is used to download data from |url|. When download is
    // finished or when an error occurs, it posts a message back to the browser
    // with the results encoded in the message as a string and self-destroys.
    //
    // pp::URLLoader.GetDownloadProgress() is used to to allocate the memory
    // required for url_response_body_ before the download starts.  (This is not so
    // much of a performance improvement, but it saves some memory since
    // std::string.insert() typically grows the string's capacity by somewhere
    // between 50% to 100% when it needs more memory, depending on the
    // implementation.)  Other performance improvements made as outlined in this
    // bug: http://code.google.com/p/chromium/issues/detail?id=103947
    //
    // EXAMPLE USAGE:
    // URLLoaderHandler* handler* = URLLoaderHandler::Create(instance,url);
    // handler->Start();
    //
    public class URL_LoaderHandler : IDisposable
    {
        Instance instance;
        string url;         // URL to be downloaded.
        PPResource urlRequest;
        PPResource urlLoader; // URLLoader provides an API to download URLs.
        byte[] buffer = new byte[READ_BUFFER_SIZE];              // Temporary buffer for reads.
        StringBuilder urlResponseBody;  // Contains accumulated downloaded data.

        bool disposed;

        const int READ_BUFFER_SIZE = 32768;

        public URL_LoaderHandler(Instance instance, string url)
        {
            this.instance = instance;
            this.url = url;

            urlRequest = PPBURLRequestInfo.Create(instance);
            if (PPBURLRequestInfo.IsURLRequestInfo(urlRequest) == PPBool.False)
                throw new Exception("Error creating a PPB_URLRequestInfo.");

            urlLoader = PPBURLLoader.Create(instance);
            if (PPBURLLoader.IsURLLoader(urlLoader) == PPBool.False)
                throw new Exception("Error creating a PPB_URLLoader.");

            PPBURLRequestInfo.SetProperty(urlRequest, PPURLRequestProperty.Url, new Var(url));
            PPBURLRequestInfo.SetProperty(urlRequest, PPURLRequestProperty.Method, new Var("GET"));
            PPBURLRequestInfo.SetProperty(urlRequest, PPURLRequestProperty.Recorddownloadprogress, new Var(true));

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

        public void Start()
        {
            var openCallback = new PPCompletionCallback();
            openCallback.func = OnOpen;
            openCallback.flags = (int)PPCompletionCallbackFlag.None;
            PPBURLLoader.Open(urlLoader, urlRequest, openCallback);

        }

        void OnOpen(IntPtr userData, int result)
        {
            if ((PPError)result != PPError.Ok)
            {
                ReportResultAndDie(url, "URLLoader.Open() failed", false);
                return;
            }
            // Here you would process the headers. A real program would want to at least
            // check the HTTP code and potentially cancel the request.
            // pp::URLResponseInfo response = loader_.GetResponseInfo();

            // Try to figure out how many bytes of data are going to be downloaded in
            // order to allocate memory for the response body in advance (this will
            // reduce heap traffic and also the amount of memory allocated).
            // It is not a problem if this fails, it just means that the
            // url_response_body_.insert() call in URLLoaderHandler::AppendDataBytes()
            // will allocate the memory later on.
            long bytes_received = 0;
            long total_bytes_to_be_received = 0;
            var gdlpr = PPBURLLoader.GetDownloadProgress(urlLoader, out bytes_received, out total_bytes_to_be_received);
            if (gdlpr == PPBool.True)
            {
                if (total_bytes_to_be_received > 0)
                {
                    urlResponseBody = new StringBuilder((int)total_bytes_to_be_received);
                }
            }

            // We will not use the download progress anymore, so just disable it.
            PPBURLRequestInfo.SetProperty(urlRequest, PPURLRequestProperty.Recorddownloadprogress, new Var(false));
            // Start streaming.
            ReadBody();
        }

        void ReadBody()
        {
            // Note that you specifically want an "optional" callback here. This will
            // allow ReadBody() to return synchronously, ignoring your completion
            // callback, if data is available. For fast connections and large files,
            // reading as fast as we can will make a large performance difference
            // However, in the case of a synchronous return, we need to be sure to run
            // the callback we created since the loader won't do anything with it.
            var onReadCallback = new PPCompletionCallback();
            onReadCallback.func = OnRead;
            onReadCallback.flags = (int)PPCompletionCallbackFlag.Optional;

            int result = (int)PPError.Ok;
            do
            {
                result = PPBURLLoader.ReadResponseBody(urlLoader, buffer, READ_BUFFER_SIZE, onReadCallback);

                // Handle streaming data directly. Note that we *don't* want to call
                // OnRead here, since in the case of result > 0 it will schedule
                // another call to this function. If the network is very fast, we could
                // end up with a deeply recursive stack.
                if (result > 0)
                {
                    AppendDataBytes(buffer, result);
                }
            } while (result > 0);

            if ((PPError)result != PPError.OkCompletionpending)
            {
                // Either we reached the end of the stream (result == PP_OK) or there was
                // an error. We want OnRead to get called no matter what to handle
                // that case, whether the error is synchronous or asynchronous. If the
                // result code *is* COMPLETIONPENDING, our callback will be called
                // asynchronously.
                onReadCallback.func(IntPtr.Zero, result);
            }
        }

        void OnRead(IntPtr userData, int result)
        {
            if ((PPError)result == PPError.Ok)
            {

                // Streaming the file is complete, delete the read buffer since it is
                // no longer needed.
                buffer = null;
                ReportResultAndDie(url, urlResponseBody.ToString(), true);
            }
            else if (result > 0)
            {
                var text = Encoding.UTF8.GetString(buffer);
                // The URLLoader just filled "result" number of bytes into our buffer.
                // Save them and perform another read.
                AppendDataBytes(buffer, result);
                ReadBody();
            }
            else
            {
                // A read error occurred.
                ReportResultAndDie(
                    url, "URL_Loader.ReadResponseBody() result<0", false);
            }
        }

        void AppendDataBytes(byte[] buffer, int num_bytes)
        {
            if (num_bytes <= 0)
                return;
            // Make sure we don't get a buffer overrun.
            num_bytes = Math.Min(READ_BUFFER_SIZE, num_bytes);

            urlResponseBody.Append(Encoding.UTF8.GetString(this.buffer).Substring(0, num_bytes));

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
