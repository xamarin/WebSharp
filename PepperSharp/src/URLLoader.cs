using System;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class URLLoader : Resource
    {

        /// <summary>
        /// Event raised when the URLLoader issues Open request.
        /// </summary>
        public event EventHandler<PPError> HandleOpen;

        /// <summary>
        /// Event raised when the URLLoader issues a FollowRedirect.
        /// </summary>
        public event EventHandler<PPError> HandleRedirect;

        /// <summary>
        /// Event raised when the URLLoader issues a ReadResponseBody.
        /// </summary>
        public event EventHandler<PPError> HandleReadResonseBody;

        /// <summary>
        /// Event raised when the URLLoader issues a FishishStreamingToFile.
        /// </summary>
        public event EventHandler<PPError> HandleFinishStreamingToFile;

        /// <summary>
        /// Event raised when the FileIO issues Close on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleClose;

        /// <summary>
        /// Constructs a URLLoader object.
        /// </summary>
        /// <param name="instance">The instance with which this resource will be
        /// associated.</param>
        public URLLoader(Instance instance)
        {
            handle = PPBURLLoader.Create(instance);
        }

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {

                }
            }

            base.Dispose(disposing);
        }

        #endregion

        /// <summary>
        /// This function begins loading the <code>URLRequestInfo</code>.
        /// The operation completes when response headers are received or when an
        /// error occurs.  Use ResponseInfo to access the response
        /// headers.
        /// </summary>
        /// <param name="requestInfo">A <code>URLRequestInfo</code> corresponding to a
        /// URLRequestInfo.</param>
        /// <returns>Error code</returns>
        public PPError Open(PPResource requestInfo)
            => (PPError)PPBURLLoader.Open(this, requestInfo, new CompletionCallback(OnOpen));

        protected virtual void OnOpen(PPError result)
            => HandleOpen?.Invoke(this, result);

        /// <summary>
        /// This function begins loading the <code>URLRequestInfo</code> asynchronously.
        /// The operation completes when response headers are received or when an
        /// error occurs.  Use ResponseInfo to access the response
        /// headers.
        /// </summary>
        /// <param name="requestInfo">A <code>URLRequestInfo</code> corresponding to a
        /// URLRequestInfo.</param>
        /// <param name="openLoop">Optional MessageLoop instance that can be used to post the command to</param>   
        /// <returns>Error code</returns>
        public Task<PPError> OpenAsync(Resource requestInfo, MessageLoop openLoop = null)
            => OpenAsyncCore(requestInfo, openLoop);

        private async Task<PPError> OpenAsyncCore(Resource requestInfo, MessageLoop openLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleOpen += handler;

                if (MessageLoop == null && openLoop == null)
                {
                    Open(requestInfo);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBURLLoader.Open(this, requestInfo,
                            new BlockUntilComplete()
                        );
                        tcs.TrySetResult(result);
                    }
                    );
                    if (openLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        openLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleOpen -= handler;
            }
        }

        /// <summary>
        /// This function can be invoked to follow a redirect after Open() completed on receiving redirect headers. 
        /// </summary>
        /// <returns>Error code.</returns>
        public PPError FollowRedirect()
            => (PPError)PPBURLLoader.FollowRedirect(this, new CompletionCallback(OnRedirect));

        protected virtual void OnRedirect(PPError result)
            => HandleRedirect?.Invoke(this, result);

        /// <summary>
        /// This async function can be invoked to follow a redirect after Open() completed on receiving redirect headers.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> FollowRedirectAsync(MessageLoop messageLoop = null)
            => FollowRedirectAsyncCore(messageLoop);

        private async Task<PPError> FollowRedirectAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleRedirect += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    FollowRedirect();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBURLLoader.FollowRedirect(this, new BlockUntilComplete());
                        tcs.TrySetResult(result);
                    }
                    );
                    if (messageLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleRedirect -= handler;
            }
        }

        /// <summary>
        /// This function returns the current upload progress (which is only
        /// meaningful after Open() has been called). Progress only refers to the
        /// request body and does not include the headers.
        ///
        /// This data is only available if the <code>URLRequestInfo</code> passed to
        /// Open() had the <code>ReportUploadProgress</code> property set to <code>true</code>. 
        /// </summary>
        /// <param name="bytesSent">The number of bytes sent thus far.</param>
        /// <param name="totalBytesToBeSent">The total number of bytes to be sent.</param>
        /// <returns>true if the upload progress is available, false if it is not
        /// available.</returns>
        public bool GetUploadProgress(out long bytesSent,
                         out long totalBytesToBeSent)
            => PPBURLLoader.GetUploadProgress(this, out bytesSent, out totalBytesToBeSent) == PPBool.True;

        /// <summary>
        /// This function returns the current download progress, which is meaningful
        /// after Open() has been called. Progress only refers to the response body
        /// and does not include the headers.
        ///
        /// This data is only available if the <code>URLRequestInfo</code> passed to Open() had the
        /// <code>ReportDownloadProgress</code> property set to true.
        /// </summary>
        /// <param name="bytesReceived">The number of bytes received thus far.</param>
        /// <param name="totalBytesToBeReceived">The total number of bytes to be
        /// received. The total bytes to be received may be unknown, in which case
        /// <code>totalBytesToBeReceived</code> will be set to -1.</param>
        /// <returns>true if the download progress is available, false if it is
        /// not available.</returns>
        public bool GetDownloadProgress(out long bytesReceived,
                         out long totalBytesToBeReceived)
            => PPBURLLoader.GetDownloadProgress(this, out bytesReceived, out totalBytesToBeReceived) == PPBool.True;

        /// <summary>
        /// Gets the current <code>URLResponseInfo</code> object.
        /// 
        /// Will be an <code>IsEmpty</code> object if the loader is not a valid resource or if Open() has not been
        /// called.
        /// </summary>
        public PPResource ResponseInfo
            => PPBURLLoader.GetResponseInfo(this);


        /// <summary>
        /// This function is used to read the response body. The size of the buffer
        /// must be large enough to hold the specified number of bytes to read.
        /// This function might perform a partial read.
        /// </summary>
        /// <param name="buffer">A byte array for the response body.</param>
        /// <param name="bytesToRead">The number of bytes to read.</param>
        /// <returns></returns>
        public PPError ReadResponseBody(byte[] buffer,
                         int bytesToRead)
            => (PPError)PPBURLLoader.ReadResponseBody(this, buffer, bytesToRead, 
                new CompletionCallback(OnReadResponseBody));

        protected void OnReadResponseBody(PPError result)
            => HandleReadResonseBody?.Invoke(this, result);

        /// <summary>
        /// This function is used to read the response body asynchronously. The size of the buffer
        /// must be large enough to hold the specified number of bytes to read.
        /// This function might perform a partial read.
        /// </summary>
        /// <param name="buffer">A byte array for the response body.</param>
        /// <param name="bytesToRead">The number of bytes to read.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> ReadResponseBodyAsync(byte[] buffer,
                         int bytes_to_read, 
                         MessageLoop messageLoop = null)
            => ReadResponseBodyAsyncCore(buffer, bytes_to_read, messageLoop);

        private async Task<PPError> ReadResponseBodyAsyncCore(byte[] buffer,
                         int bytes_to_read, 
                         MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleReadResonseBody += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    ReadResponseBody(buffer, bytes_to_read);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBURLLoader.ReadResponseBody(this, 
                            buffer, bytes_to_read,
                            new BlockUntilComplete());
                        tcs.TrySetResult(result);
                    }
                    );
                    if (messageLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleReadResonseBody -= handler;
            }
        }

        /// <summary>
        /// This function is used to wait for the response body to be completely
        /// downloaded to the file provided by the GetBodyAsFileRef() in the current
        /// <code>URLResponseInfo</code>. This function is only used if
        /// <code>StreamToFile</code> was set on the <code>URLRequestInfo</code> passed to Open().
        /// </summary>
        /// <returns>Number of bytes read or an error code</returns>
        public PPError FinishStreamingToFile()
            => (PPError)PPBURLLoader.FinishStreamingToFile(this, new CompletionCallback(OnFinishStreamingToFile));

        protected virtual void OnFinishStreamingToFile(PPError result)
            => HandleFinishStreamingToFile?.Invoke(this, result);

        /// <summary>
        /// This async function is used to wait for the response body to be completely
        /// downloaded to the file provided by the GetBodyAsFileRef() in the current
        /// <code>URLResponseInfo</code>. This function is only used if
        /// <code>StreamToFile</code> was set on the <code>URLRequestInfo</code> passed to Open().
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> FinishStreamingToFileAsync(MessageLoop messageLoop = null)
            => FinishStreamingToFileAsyncCore(messageLoop);

        private async Task<PPError> FinishStreamingToFileAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleFinishStreamingToFile += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    FinishStreamingToFile();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBURLLoader.FinishStreamingToFile(this, new BlockUntilComplete());
                        tcs.TrySetResult(result);
                    }
                    );
                    if (messageLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleFinishStreamingToFile -= handler;
            }
        }

        /// <summary>
        /// This function is used to cancel any pending IO and close the URLLoader
        /// object. Any pending callbacks will still run, reporting
        /// <code>Aborted</code> if pending IO was interrupted.  It is NOT
        /// valid to call Open() again after a call to this function.
        ///
        /// <strong>Note:</strong> If the <code>URLLoader</code> object is destroyed
        /// while it is still open, then it will be implicitly closed so you are not
        /// required to call Close().
        /// </summary>
        public void Close()
        {
            PPBURLLoader.Close(this);
            OnClose(PPError.Ok);
        }

        protected virtual void OnClose(PPError result)
            => HandleClose?.Invoke(this, result);

        /// <summary>
        /// This async function is used to cancel any pending IO and close the URLLoader
        /// object. Any pending callbacks will still run, reporting
        /// <code>Aborted</code> if pending IO was interrupted.  It is NOT
        /// valid to call Open() again after a call to this function.
        ///
        /// <strong>Note:</strong> If the <code>URLLoader</code> object is destroyed
        /// while it is still open, then it will be implicitly closed so you are not
        /// required to call Close().
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> CloseAsync(MessageLoop messageLoop = null)
            => CloseAsyncCore(messageLoop);

        private async Task<PPError> CloseAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleClose += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Close();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        PPBURLLoader.Close(this);
                        tcs.TrySetResult(PPError.Ok);
                    }
                    );
                    if (messageLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleClose -= handler;
            }
        }

    }
}
