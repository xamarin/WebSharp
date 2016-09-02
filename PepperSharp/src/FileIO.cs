using System;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class FileIO : Resource
    {

        /// <summary>
        /// Event raised when the FileIO issues Open on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleOpen;

        /// <summary>
        /// Event raised when the FileIO issues Query on the FileRef.
        /// </summary>
        public event EventHandler<FileInfo> HandleQuery;

        /// <summary>
        /// Event raised when the FileIO issues Touch on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleTouch;

        /// <summary>
        /// Event raised when the FileIO issues Read on the FileRef.
        /// </summary>
        public event EventHandler<FileIOResult> HandleReadData;

        /// <summary>
        /// Event raised when the FileIO issues Write on the FileRef.
        /// </summary>
        public event EventHandler<FileIOResult> HandleWriteData;

        /// <summary>
        /// Event raised when the FileIO issues SetLength on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleSetLength;

        /// <summary>
        /// Event raised when the FileIO issues Flush on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleFlush;

        /// <summary>
        /// Event raised when the FileIO issues Close on the FileRef.
        /// </summary>
        public event EventHandler<PPError> HandleClose;



        public FileIO (Instance instance)
        {
            handle = PPBFileIO.Create(instance);
        }

        /// <summary>
        /// Open() opens the specified regular file for I/O according to the given
        /// open flags, which is a bit-mask of the FileOpenFlags values.  Upon
        /// success, the corresponding file is classified as "in use" by this FileIO
        /// object until such time as the FileIO object is closed or destroyed.
        /// </summary>
        /// <param name="fileRef">A FileRef instance</param>
        /// <param name="openFlags">A bit-mask of <code>FileOpenFlags</code> values.</param>
        /// <returns>Error code</returns>
        PPError Open(FileRef fileRef, FileOpenFlags openFlags)
            => (PPError)PPBFileIO.Open(this, fileRef, (int)openFlags, new CompletionCallback(OnOpen));

        protected virtual void OnOpen(PPError result)
            => HandleOpen?.Invoke(this, result);

        /// <summary>
        /// Open() opens the specified regular file for I/O according to the given
        /// open flags, which is a bit-mask of the FileOpenFlags values.  Upon
        /// success, the corresponding file is classified as "in use" by this FileIO
        /// object until such time as the FileIO object is closed or destroyed.
        /// </summary>
        /// <param name="fileRef">A FileRef instance</param>
        /// <param name="openFlags">A bit-mask of <code>FileOpenFlags</code> values.</param>
        /// <param name="openLoop">Optional MessageLoop instance that can be used to post the command to</param>   
        /// <returns>Error code</returns>
        public Task<PPError> OpenAsync(FileRef fileRef, FileOpenFlags openFlags, MessageLoop openLoop = null)
            => OpenAsyncCore(fileRef, openFlags, openLoop);
   
        private async Task<PPError> OpenAsyncCore(FileRef fileRef, FileOpenFlags openFlags, MessageLoop openLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleOpen += handler;

                if (MessageLoop == null && openLoop == null)
                {
                    Open(fileRef, openFlags);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileIO.Open(this, fileRef, (int)openFlags,
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
        /// Query() queries info about the file opened by this FileIO object. This
        /// function will fail if the FileIO object has not been opened.
        /// </summary>
        /// <returns>Error code</returns>
        PPError Query()
        {
            var ficb = new CompletionCallbackWithOutput<PPFileInfo>(OnQuery);
            return (PPError)PPBFileIO.Query(this, out ficb.OutputAdapter.output, ficb);
        }

        protected virtual void OnQuery(PPError result, PPFileInfo fileInfo)
            => HandleQuery?.Invoke(this, new FileInfo(result, fileInfo));

        /// <summary>
        /// Query() queries info about the file opened by this FileIO object asynchronously. This
        /// function will fail if the FileIO object has not been opened.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>A FileInfo instance see <see cref="FileInfo"/></returns>
        public Task<FileInfo> QueryAsync(MessageLoop messageLoop = null)
            => QueryAsyncCore(messageLoop);


        private async Task<FileInfo> QueryAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<FileInfo>();
            EventHandler<FileInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleQuery += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Query();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var fileInfo = new PPFileInfo();
                        var result = (PPError)PPBFileIO.Query(this, out fileInfo,
                            new BlockUntilComplete()
                        );
                        tcs.TrySetResult(new FileInfo(result, fileInfo));
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
                return new FileInfo(PPError.Aborted, new PPFileInfo());
            }
            finally
            {
                HandleQuery -= handler;
            }
        }

        /// <summary>
        /// Touch() Updates time stamps for a file.  You must have write access to the
        /// file if it exists in the external filesystem.
        /// </summary>
        /// <param name="lastAccessTime">The last time the file was accessed.</param>
        /// <param name="lastModifiedTime">The last time the file was modified.</param>
        /// <returns>Ok if all went well</returns>
        public PPError Touch(DateTime lastAccessTime, DateTime lastModifiedTime)
            => (PPError)PPBFileRef.Touch(this,
                PepperSharpUtils.ConvertToPepperTimestamp(lastAccessTime),
                PepperSharpUtils.ConvertToPepperTimestamp(lastModifiedTime),
                new CompletionCallback(OnTouch));

        protected virtual void OnTouch(PPError result)
            => HandleTouch?.Invoke(this, result);

        /// <summary>
        /// Touch() Updates time stamps for a file asynchronously.  You must have write access to the
        /// file if it exists in the external filesystem.
        /// </summary>
        /// <param name="lastAccessTime">The last time the file was accessed.</param>
        /// <param name="lastModifiedTime">The last time the file was modified.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Ok if all went well</returns>
        public Task<PPError> TouchAsync(DateTime lastAccessTime, DateTime lastModifiedTime, MessageLoop messageLoop = null)
            => TouchAsyncCore(lastAccessTime, lastModifiedTime, messageLoop);


        private async Task<PPError> TouchAsyncCore(DateTime lastAccessTime, DateTime lastModifiedTime, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleTouch += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Touch(lastAccessTime, lastModifiedTime);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileRef.Touch(this,
                            PepperSharpUtils.ConvertToPepperTimestamp(lastAccessTime),
                            PepperSharpUtils.ConvertToPepperTimestamp(lastModifiedTime),
                            new BlockUntilComplete()
                        );
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
                HandleTouch -= handler;
            }
        }


        /// <summary>
        /// Reads from an offset in the file.
        ///
        /// The size of the buffer must be large enough to hold the specified number
        /// of bytes to read.  This function might perform a partial read, meaning
        /// that all the requested bytes might not be returned, even if the end of the
        /// file has not been reached.
        ///
        /// This function reads into a buffer that the caller supplies. This buffer
        /// must remain valid as long as the FileIO resource is alive. If you use
        /// a completion callback factory and it goes out of scope, it will not issue
        /// the callback on your class, BUT the callback factory can NOT cancel
        /// the request from the browser's perspective. This means that the browser
        /// will still try to write to your buffer even if the callback factory is
        /// destroyed!
        ///
        /// So you must ensure that your buffer outlives the FileIO resource. If you
        /// have one class and use the FileIO resource exclusively from that class
        /// and never make any copies, this will be fine: the resource will be
        /// destroyed when your class is. But keep in mind that copying a pp::FileIO
        /// object just creates a second reference to the original resource. For
        /// example, if you have a function like this:
        ///   FileIO MyClass.GetFileIO();
        /// where a copy of your FileIO resource could outlive your class, the
        /// callback will still be pending when your class goes out of scope, creating
        /// the possibility of writing into invalid memory. So it's recommended to
        /// keep your FileIO resource and any output buffers tightly controlled in
        /// the same scope.
        ///
        /// <strong>Caveat:</strong> This Read() is potentially unsafe if you're using
        /// an EventHandler to scope callbacks to the lifetime of your
        /// class.  When your class goes out of scope, the native callback factory will not
        /// actually cancel the callback, but will rather just skip issuing the
        /// callback on your class.  This means that if the FileIO object outlives
        /// your class (if you made a copy saved somewhere else, for example), then
        /// the browser will still try to write into your buffer when the
        /// asynchronous read completes, potentially causing a crash.
        ///
        /// See the other version of Read() which avoids this problem by writing into
        /// ArraySegment, where the output buffer is automatically managed by the native callback.
        ///
        /// </summary>
        /// <param name="buffer">The buffer to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToRead">The number of bytes to read from <code>offset</code>.</param>
        /// <returns>Error code.  If the return value is 0, then end-of-file was
        /// reached.</returns>
        public PPError Read(byte[] buffer, int offset, int bytesToRead)
            => (PPError) PPBFileIO.Read(this, offset, buffer, bytesToRead, new CompletionCallback(OnReadData));

        protected virtual void OnReadData(PPError result)
        {
            HandleReadData?.Invoke(this, new FileIOResult((int)result, (int)result == 0));
        }

        /// <summary>
        /// Reads from an offset in the file asynchronously.
        ///
        /// The size of the buffer must be large enough to hold the specified number
        /// of bytes to read.  This function might perform a partial read, meaning
        /// that all the requested bytes might not be returned, even if the end of the
        /// file has not been reached.
        ///
        /// This function reads into a buffer that the caller supplies. This buffer
        /// must remain valid as long as the FileIO resource is alive. If you use
        /// a completion callback factory and it goes out of scope, it will not issue
        /// the callback on your class, BUT the callback factory can NOT cancel
        /// the request from the browser's perspective. This means that the browser
        /// will still try to write to your buffer even if the callback factory is
        /// destroyed!
        ///
        /// So you must ensure that your buffer outlives the FileIO resource. If you
        /// have one class and use the FileIO resource exclusively from that class
        /// and never make any copies, this will be fine: the resource will be
        /// destroyed when your class is. But keep in mind that copying a pp::FileIO
        /// object just creates a second reference to the original resource. For
        /// example, if you have a function like this:
        ///   FileIO MyClass.GetFileIO();
        /// where a copy of your FileIO resource could outlive your class, the
        /// callback will still be pending when your class goes out of scope, creating
        /// the possibility of writing into invalid memory. So it's recommended to
        /// keep your FileIO resource and any output buffers tightly controlled in
        /// the same scope.
        ///
        /// <strong>Caveat:</strong> This Read() is potentially unsafe if you're using
        /// an EventHandler to scope callbacks to the lifetime of your
        /// class.  When your class goes out of scope, the native callback factory will not
        /// actually cancel the callback, but will rather just skip issuing the
        /// callback on your class.  This means that if the FileIO object outlives
        /// your class (if you made a copy saved somewhere else, for example), then
        /// the browser will still try to write into your buffer when the
        /// asynchronous read completes, potentially causing a crash.
        ///
        /// See the other version of Read() which avoids this problem by writing into
        /// ArraySegment, where the output buffer is automatically managed by the native callback.
        ///
        /// </summary>
        /// <param name="buffer">The buffer to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToRead">The number of bytes to read from <code>offset</code>.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns><see cref="FileIOResult"/></returns>
        public Task<FileIOResult> ReadAsync(byte[] buffer, int offset, int bytesToRead, MessageLoop messageLoop = null)
        {
            return ReadAsyncCore(buffer, offset, bytesToRead, messageLoop );
        }

        private async Task<FileIOResult> ReadAsyncCore(byte[] buffer, int offset, int size, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<FileIOResult>();
            EventHandler<FileIOResult> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleReadData += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Read(buffer, offset, size);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileIO.Read(this, offset,
                            buffer,
                            size,
                            new BlockUntilComplete()
                        );
                        tcs.TrySetResult(new FileIOResult((int)result, (int)result == 0));
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
                return new FileIOResult((int)PPError.Aborted, true);
            }
            finally
            {
                HandleReadData -= handler;
            }
        }

        /// <summary>
        /// Read() reads from an offset in the file.  A PP_ArrayOutput must be
        /// provided so that output will be stored in its allocated buffer.  This
        /// function might perform a partial read.
        /// </summary>
        /// <param name="buffer">An ArraySegment<byte> to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToRead">The number of bytes to read from <code>offset</code>.</param>
        /// <returns>Error code.  If the return value is 0, then end-of-file was reached.</returns>
        public PPError Read(ArraySegment<byte> buffer, int offset, int bytesToRead)
        {

            var readToArrayAction = new Action<PPError, byte[]>(
                (result, bytes) =>
                {
                    Array.Copy(bytes, buffer.Array, Math.Min(bytes.Length, buffer.Count));
                    OnReadData(result);
                }

                );


            var arrayOutput = new CompletionCallbackWithOutput<byte[]>(new CompletionCallbackWithOutputFunc<byte[]>(readToArrayAction));
            return (PPError)PPBFileIO.ReadToArray(this, offset, bytesToRead, arrayOutput, arrayOutput);

        }

        /// <summary>
        /// Read() reads from an offset in the file.  A PP_ArrayOutput must be
        /// provided so that output will be stored in its allocated buffer.  This
        /// function might perform a partial read.
        /// </summary>
        /// <param name="buffer">An ArraySegment<byte> to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToRead">The number of bytes to read from <code>offset</code>.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns><see cref="FileIOResult"/></returns>
        public Task<FileIOResult> ReadAsync(ArraySegment<byte> buffer, int offset, int bytesToRead, MessageLoop messageLoop = null)
        {
            return ReadAsyncCore(buffer, offset, bytesToRead, messageLoop);
        }

        private async Task<FileIOResult> ReadAsyncCore(ArraySegment<byte> buffer, int offset, int size, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<FileIOResult>();
            EventHandler<FileIOResult> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleReadData += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Read(buffer, offset, size);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var arrayOutput = new ArrayOutputAdapterWithStorage<byte[]>();
                        var result = (PPError)PPBFileIO.ReadToArray(this, offset, size, arrayOutput.PPArrayOutput, 
                            new BlockUntilComplete());
                        var bytes = arrayOutput.Output;
                        Array.Copy(bytes, buffer.Array, Math.Min(bytes.Length, buffer.Count));

                        tcs.TrySetResult(new FileIOResult((int)result, (int)result == 0));
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
                return new FileIOResult((int)PPError.Aborted, true);
            }
            finally
            {
                HandleReadData -= handler;
            }
        }

        /// <summary>
        /// Write() writes to an offset in the file.  This function might perform a
        /// partial write. The FileIO object must have been opened with write access.
        /// </summary>
        /// <param name="buffer">The buffer to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToWrite">The number of bytes to write to
        /// <code>offset</code>.</param>
        /// <returns>Error code</returns>
        public PPError Write(byte[] buffer, int offset, int bytesToWrite)
            => (PPError)PPBFileIO.Write(this, offset, buffer, bytesToWrite, new CompletionCallback(OnWriteData));

        protected virtual void OnWriteData(PPError result)
        {
            HandleWriteData?.Invoke(this, new FileIOResult((int)result, (int)result == 0));
        }

        /// <summary>
        /// Write() writes to an offset in the file asynchronously.  This function might perform a
        /// partial write. The FileIO object must have been opened with write access.
        /// </summary>
        /// <param name="buffer">The buffer to hold the specified number of bytes read.</param>
        /// <param name="offset">The offset into the file.</param>
        /// <param name="bytesToWrite">The number of bytes to write to
        /// <code>offset</code>.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns><see cref="FileIOResult"/></returns>
        public Task<FileIOResult> WriteAsync(byte[] buffer, int offset, int bytesToWrite, MessageLoop messageLoop = null)
        {
            return WriteAsyncCore(buffer, offset, bytesToWrite, messageLoop);
        }

        private async Task<FileIOResult> WriteAsyncCore(byte[] buffer, int offset, int size, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<FileIOResult>();
            EventHandler<FileIOResult> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleWriteData += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Write(buffer, offset, size);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileIO.Write(this, offset,
                            buffer,
                            size,
                            new BlockUntilComplete()
                        );
                        tcs.TrySetResult(new FileIOResult((int)result, (int)result == 0));
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
                return new FileIOResult((int)PPError.Aborted, true);
            }
            finally
            {
                HandleWriteData -= handler;
            }
        }

        /// <summary>
        /// SetLength() sets the length of the file.  If the file size is extended,
        /// then the extended area of the file is zero-filled.  The FileIO object must
        /// have been opened with write access.
        /// </summary>
        /// <param name="length">The length of the file to be set.</param>
        /// <returns></returns>
        public PPError SetLength(long length)
            => (PPError)PPBFileIO.SetLength(this, length, new CompletionCallback(OnSetLength));

        protected void OnSetLength(PPError result)
            => HandleSetLength?.Invoke(this, result);

        /// <summary>
        /// SetLength() sets the length of the file asynchronously.  If the file size is extended,
        /// then the extended area of the file is zero-filled.  The FileIO object must
        /// have been opened with write access.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> SetLengthAsync(long length, MessageLoop messageLoop = null)
            => SetLengthAsyncCore(length, messageLoop);

        private async Task<PPError> SetLengthAsyncCore(long length, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleSetLength += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    SetLength(length);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileIO.SetLength(this, length,
                            new BlockUntilComplete()
                        );
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
                HandleSetLength -= handler;
            }
        }

        /// <summary>
        /// Flush() flushes changes to disk.  This call can be very expensive!
        /// </summary>
        /// <returns>Error code.</returns>
        public PPError Flush()
            => (PPError)PPBFileIO.Flush(this, new CompletionCallback(OnFlush));

        protected virtual void OnFlush(PPError result)
            => HandleFlush?.Invoke(this, result);

        /// <summary>
        /// Flush() flushes changes to disk asynchronously.  This call can be very expensive!
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> FlushAsync(MessageLoop messageLoop = null)
            => FlushAsyncCore(messageLoop);

        private async Task<PPError> FlushAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleFlush += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Flush();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileIO.Flush(this,
                            new BlockUntilComplete()
                        );
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
                HandleFlush -= handler;
            }
        }

        /// <summary>
        /// Close() cancels any IO that may be pending, and closes the FileIO object.
        /// Any pending callbacks will still run, reporting
        /// <code>Aborted</code> if pending IO was interrupted.  It is not
        /// valid to call Open() again after a call to this method.
        ///
        /// <strong>Note:</strong> If the FileIO object is destroyed, and it is still
        /// open, then it will be implicitly closed, so you are not required to call
        /// Close().
        /// </summary>
        public void Close()
        {
            PPBFileIO.Close(this);
            OnClose(PPError.Ok);
        }

        protected virtual void OnClose(PPError result)
            => HandleClose?.Invoke(this, result);

        /// <summary>
        /// Close() cancels any IO that may be pending, and closes the FileIO object asynchronously.
        /// Any pending callbacks will still run, reporting
        /// <code>Aborted</code> if pending IO was interrupted.  It is not
        /// valid to call Open() again after a call to this method.
        ///
        /// <strong>Note:</strong> If the FileIO object is destroyed, and it is still
        /// open, then it will be implicitly closed, so you are not required to call
        /// Close().
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
                        PPBFileIO.Close(this);
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

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleOpen = null;
                    HandleQuery = null;
                    HandleReadData = null;
                    HandleSetLength = null;
                    HandleFlush = null;
                    HandleClose = null;
                    HandleWriteData = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

    }


    public class FileIOResult
    {

        public FileIOResult(int count, bool endOfFile)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.Count = count;
            this.EndOfFile = endOfFile;
        }

        public int Count { get; private set; }
        public bool EndOfFile { get; private set; }

        internal FileIOResult Copy(int count)
        {
            System.Diagnostics.Debug.Assert(count >= 0, $"'Count' MUST NOT be negative.");
            System.Diagnostics.Debug.Assert(count <= this.Count, "'Count' MUST NOT be bigger than 'this.Count'.");
            this.Count -= count;
            return new FileIOResult(count, this.Count == 0 && this.EndOfFile);
        }
    }

    /**
    * The FileOpenFlags enum contains file open constants.
    */
    public enum FileOpenFlags
    {
        /** Requests read access to a file. */
        Read = 1 << 0,
        /**
         * Requests write access to a file.  May be combined with
         * <code>Read</code> to request read and write access.
         */
        Write = 1 << 1,
        /**
         * Requests that the file be created if it does not exist.  If the file
         * already exists, then this flag is ignored unless
         * <code>Exclusive</code> was also specified, in which case
         * FileIO.Open() will fail.
         */
        Create = 1 << 2,
        /**
         * Requests that the file be truncated to length 0 if it exists and is a
         * regular file. <code>PP_FILEOPENFLAG_WRITE</code> must also be specified.
         */
        Truncate = 1 << 3,
        /**
         * Requests that the file is created when this flag is combined with
         * <code>Create</code>.  If this flag is specified, and the
         * file already exists, then the FileIO.Open() call will fail.
         */
        Exclusive = 1 << 4,
        /**
         * Requests write access to a file, but writes will always occur at the end of
         * the file. Mututally exclusive with <code>PP_FILEOPENFLAG_WRITE</code>.
         *
         * This is only supported in version 1.2 (Chrome 29) and later.
         */
        Append = 1 << 5
    }
}
