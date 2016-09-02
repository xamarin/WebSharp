using System;
using System.Threading.Tasks;

namespace PepperSharp
{

    public class FileSystem : Resource
    {

        /// <summary>
        /// Event raised when the FileSystem issues Open on the FileSystem.
        /// </summary>
        public event EventHandler<PPError> HandleOpen;

        public FileSystem (Instance instance, FileSystemType type)
        {
            handle = PPBFileSystem.Create(instance, (PPFileSystemType)type);
        }

        /// <summary>
        /// Open() opens the file system. A file system must be opened before running
        /// any other operation on it.
        /// </summary>
        /// <param name="expected_size">
        /// The expected size of the file system.Note that
        /// this does not request quota; to do that, you must either invoke
        /// requestQuota from JavaScript:
        /// http://www.html5rocks.com/en/tutorials/file/filesystem/#toc-requesting-quota
        /// or set the unlimitedStorage permission for Chrome Web Store apps:
        /// http://code.google.com/chrome/extensions/manifest.html#permissions
        /// </param>
        /// <returns>A PPError code</returns>
        public PPError Open(long expected_size)
            => (PPError)PPBFileSystem.Open(this, expected_size, new CompletionCallback(OnOpen));

        protected void OnOpen(PPError result)
            => HandleOpen?.Invoke(this, result);

        /// <summary>
        /// Open() opens the file system asynchronously. A file system must be opened before running
        /// any other operation on it.
        /// </summary>
        /// <param name="expected_size">
        /// The expected size of the file system.Note that
        /// this does not request quota; to do that, you must either invoke
        /// requestQuota from JavaScript:
        /// http://www.html5rocks.com/en/tutorials/file/filesystem/#toc-requesting-quota
        /// or set the unlimitedStorage permission for Chrome Web Store apps:
        /// http://code.google.com/chrome/extensions/manifest.html#permissions
        /// </param>
        /// <param name="openLoop">Optional MessageLoop instance that can be used to post the command to</param>        
        /// <returns>A PPError code</returns>
        public Task<PPError> OpenAsync (long expected_size, MessageLoop openLoop = null)
            => OpenAsyncCore(expected_size, openLoop);

        private async Task<PPError> OpenAsyncCore(long expected_size, MessageLoop openLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleOpen += handler;

                if (MessageLoop == null && openLoop == null)
                {
                    Open(expected_size);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileSystem.Open(this, expected_size,
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

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleOpen = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

    }

    /**
    * The <code>FileType</code> enum contains file type constants.
    */
    public enum FileType
    {
        /** A regular file type */
        Regular = 0,
        /** A directory */
        Directory = 1,
        /** A catch-all for unidentified types */
        Other = 2
    }

    /**
     * The <code>FileSystemType</code> enum contains file system type constants.
     */
    public enum FileSystemType
    {
        /** For identified invalid return values */
        Invalid = 0,
        /** For external file system types */
        External = 1,
        /** For local persistent file system types */
        LocalPersistent = 2,
        /** For local temporary file system types */
        LocalTemporary = 3,
        /** For isolated file system types */
        Isolated = 4
    }

}
