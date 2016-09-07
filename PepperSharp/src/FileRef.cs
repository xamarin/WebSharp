using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class FileRef : Resource
    {

        /// <summary>
        /// Event raised when the FileRef issues MakeDirectory on the FileSystem.
        /// </summary>
        public event EventHandler<PPError> HandleMakeDirectory;

        /// <summary>
        /// Event raised when the FileRef issues Touch on the FileSystem.
        /// </summary>
        public event EventHandler<PPError> HandleTouch;

        /// <summary>
        /// Event raised when the FileRef issues Delete on the FileSystem.
        /// </summary>
        public event EventHandler<PPError> HandleDelete;

        /// <summary>
        /// Event raised when the FileRef issues Rename on the FileSystem.
        /// </summary>
        public event EventHandler<PPError> HandleRename;

        /// <summary>
        /// Event raised when the FileRef issues Query on the FileSystem.
        /// </summary>
        public event EventHandler<FileInfo> HandleQuery;

        /// <summary>
        /// Event raised when the FileRef issues ReadDirectoryEntries on the FileSystem.
        /// </summary>
        public event EventHandler<DirectoryEntries> HandleReadDirectoryEntries;

        public sealed class DirectoryEntries : EventArgs, IDisposable
        {
            public PPError Result { get; private set; }
            public ReadOnlyCollection<DirectoryEntry> Entries { get; private set; }

            internal DirectoryEntries(PPError result, ReadOnlyCollection<DirectoryEntry> entries)
            {
                Result = result;
                Entries = entries;
            }

            #region Implement IDisposable.

            public void Dispose()
            {
                foreach (var entry in Entries)
                    entry.Dispose();
                Entries = null;
                GC.SuppressFinalize(this);

            }

            ~DirectoryEntries()
            {
                Dispose();
            }

            #endregion
        }

        public FileRef (FileSystem fileSystem, string path)
        {
            handle = PPBFileRef.Create(fileSystem, Encoding.UTF8.GetBytes(path));
        }

        internal FileRef(PPResource fileRef) : base(PassRef.PassRef, fileRef)
        { }

        /// <summary>
        /// Gets the type of the file system.  If the file system type is invalid or
        /// the resource provided is not valid then returns Invalid. 
        /// </summary>
        FileSystemType FileSystemType => (FileSystemType)PPBFileRef.GetFileSystemType(this);

        /// <summary>
        /// Gets the name of the file.
        ///
        /// The value returned by this property does not include any path components (such as
        /// the name of the parent directory, for example). It is just the name of the
        /// file. Use Path to get the full file path.
        /// </summary>
        public string Name => ((Var)PPBFileRef.GetName(this)).AsString();

        /// <summary>
        /// Gets the absolute path of the file.  This will fail if the file system type is
        /// External.
        /// </summary>
        public string Path => ((Var)PPBFileRef.GetPath(this)).AsString();

        /// <summary>
        /// Gets the parent directory of this file.  If the FileRef points to the root of the 
        /// filesystem, then the root is returned.
        ///
        /// @return A <code>FileRef</code> containing the parent directory of the
        /// file. This function fails if the file system type is
        /// <code>PP_FileSystemType_External</code>.
        /// </summary>
        public FileRef Parent => new FileRef(PPBFileRef.GetParent(this));

        /// <summary>
        /// MakeDirectory() makes a new directory in the file system according to the
        /// given <code>makeDirectoryFlags</code>, which is a bit-mask of the
        /// <code>MakeDirectoryFlags</code> values.  It is not valid to make a
        /// directory in the external file system.
        /// </summary>
        /// <param name="makeDirectoryFlags">A bit-mask of the
        /// <code>MakeDirectoryFlags</code> values.</param>
        /// <returns>Error code</returns>
        PPError MakeDirectory(MakeDirectoryFlags makeDirectoryFlags)
            => (PPError)PPBFileRef.MakeDirectory(this,
                (int)makeDirectoryFlags,
                new CompletionCallback(OnMakeDirectory));

        protected virtual void OnMakeDirectory(PPError result)
            => HandleMakeDirectory?.Invoke(this, result);

        /// <summary>
        /// MakeDirectoryAsync() makes a new directory asynchronously in the file system according to the
        /// given <code>makeDirectoryFlags</code>, which is a bit-mask of the
        /// <code>MakeDirectoryFlags</code> values.  It is not valid to make a
        /// directory in the external file system.
        /// </summary>
        /// <param name="makeDirectoryFlags">A bit-mask of the
        /// <code>MakeDirectoryFlags</code> values.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> MakeDirectoryAsync(MakeDirectoryFlags makeDirectoryFlags, MessageLoop messageLoop = null)
            => MakeDirectoryAsyncCore(makeDirectoryFlags, messageLoop);

        private async Task<PPError> MakeDirectoryAsyncCore(MakeDirectoryFlags makeDirectoryFlags, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleMakeDirectory += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    MakeDirectory(makeDirectoryFlags);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileRef.MakeDirectory(this, (int)makeDirectoryFlags,
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
                HandleMakeDirectory -= handler;
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
        /// Delete() deletes a file or directory. If <code>FileRef</code> refers to
        /// a directory, then the directory must be empty. It is an error to delete a
        /// file or directory that is in use.  It is not valid to delete a file in
        /// the external file system.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError Delete()
            => (PPError)PPBFileRef.Delete(this, new CompletionCallback(OnDelete));

        protected virtual void OnDelete(PPError result)
            => HandleDelete?.Invoke(this, result);


        /// <summary>
        /// Delete() deletes a file or directory asynchronously. If <code>FileRef</code> refers to
        /// a directory, then the directory must be empty. It is an error to delete a
        /// file or directory that is in use.  It is not valid to delete a file in
        /// the external file system.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public Task<PPError> DeleteAsync(MessageLoop messageLoop = null)
            => DeleteAsyncCore(messageLoop);
        
        private async Task<PPError> DeleteAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleDelete += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                   Delete();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileRef.Delete(this,
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
                HandleDelete -= handler;
            }
        }

        /// <summary>
        /// Rename() renames a file or directory. Argument <code>newFileRef</code>
        /// must refer to files in the same file system as in this object. It is an
        /// error to rename a file or directory that is in use.  It is not valid to
        /// rename a file in the external file system.
        /// </summary>
        /// <param name="newFileRef">A <code>FileRef</code> corresponding to a new
        /// file reference.</param>
        /// <returns>Error code</returns>
        PPError Rename(FileRef newFileRef)
            => (PPError)PPBFileRef.Rename(this, newFileRef, new CompletionCallback(OnRename));

        protected virtual void OnRename(PPError result)
            => HandleRename?.Invoke(this, result);

        /// <summary>
        /// Rename() renames a file or directory asynchronously. Argument <code>newFileRef</code>
        /// must refer to files in the same file system as in this object. It is an
        /// error to rename a file or directory that is in use.  It is not valid to
        /// rename a file in the external file system.
        /// </summary>
        /// <param name="newFileRef">A <code>FileRef</code> corresponding to a new
        /// file reference.
        /// </param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns></returns>
        public Task<PPError> RenameAsync(FileRef newFileRef, MessageLoop messageLoop = null)
            => RenameAsyncCore(newFileRef, messageLoop);


        private async Task<PPError> RenameAsyncCore(FileRef newFileRef, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleRename += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Rename(newFileRef);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBFileRef.Rename(this, newFileRef,
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
                HandleRename -= handler;
            }
        }

        /// <summary>
        /// Query() queries info about a file or directory. You must have access to
        /// read this file or directory if it exists in the external filesystem.
        /// </summary>
        /// <returns>Error code</returns>
        PPError Query()
        {
            var ficb = new CompletionCallbackWithOutput<PPFileInfo>(OnQuery);
            return (PPError)PPBFileRef.Query(this, out ficb.OutputAdapter.output, ficb);
        }

        protected virtual void OnQuery(PPError result, PPFileInfo fileInfo)
            => HandleQuery?.Invoke(this, new FileInfo(result, fileInfo));

        /// <summary>
        /// Query() queries info about a file or directory asynchronously. You must have access to
        /// read this file or directory if it exists in the external filesystem.
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
                        var result = (PPError)PPBFileRef.Query(this, out fileInfo,
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
        /// ReadDirectoryEntries() Reads all entries in the directory.
        /// </summary>
        /// <returns>Error code.</returns>
        PPError ReadDirectoryEntries()
        {
            var listCallback = new CompletionCallbackWithOutput<PPDirectoryEntry[], PPResource>(ListCallback, this);
            return (PPError)PPBFileRef.ReadDirectoryEntries(this, listCallback, listCallback);
        }

        void ListCallback(PPError result,
                    PPDirectoryEntry[] entries,
                  PPResource unused_ref)
        {

            var sv = new List<DirectoryEntry>();
            if (entries != null)
            {
                for (int i = 0; i < entries.Length; ++i)
                {
                    sv.Add(new DirectoryEntry(entries[i]));
                }
            }

            OnReadDirectoryEntries(result, sv.AsReadOnly());
        }

        protected void OnReadDirectoryEntries(PPError result, ReadOnlyCollection<DirectoryEntry> entries)
            => HandleReadDirectoryEntries?.Invoke(this, new DirectoryEntries(result, entries));

        /// <summary>
        /// ReadDirectoryEntries() Reads all entries in the directory asynchronously.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>A DirectoryEntries object <see cref="DirectoryEntries"/></returns>
        public Task<DirectoryEntries> ReadDirectoryEntriesAsync(MessageLoop messageLoop = null)
            => ReadDirectoryEntriesAsyncCore(messageLoop);

        private async Task<DirectoryEntries> ReadDirectoryEntriesAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<DirectoryEntries>();
            EventHandler<DirectoryEntries> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleReadDirectoryEntries += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    ReadDirectoryEntries();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new ArrayOutputAdapterWithStorage<PPDirectoryEntry[]>();
                        var result = (PPError)PPBFileRef.ReadDirectoryEntries(this, output.PPArrayOutput,
                            new BlockUntilComplete()
                        );

                        var entries = new List<DirectoryEntry>();
                        foreach (var entry in output.Output)
                        {
                            entries.Add(new DirectoryEntry(entry));
                        }
                        tcs.TrySetResult(new DirectoryEntries(result, entries.AsReadOnly()));
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
                return new DirectoryEntries(PPError.Aborted, new List<DirectoryEntry> ().AsReadOnly());
            }
            finally
            {
                HandleReadDirectoryEntries -= handler;
            }
        }

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleMakeDirectory = null;
                    HandleTouch = null;
                    HandleDelete = null;
                    HandleRename = null;
                    HandleQuery = null;
                    HandleReadDirectoryEntries = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }


    /**
     * The <code>MakeDirectoryFlags</code> enum contains flags used to control
     * behavior of <code>FileRef.MakeDirectory()</code>.
     */
    public enum MakeDirectoryFlags
    {
        None = 0 << 0,
        /** Requests that ancestor directories are created if they do not exist. */
        WithAncestors = 1 << 0,
        /**
         * Requests that the PPB_FileRef.MakeDirectory() call fails if the directory
         * already exists.
         */
        Exclusive = 1 << 1
    }

    /**
    * The <code>FileInfo</code> struct represents all information about a file,
    * such as size, type, and creation time.
    */
    public class FileInfo
    {
        public PPError QueryResult { get; private set; }

        /** This value represents the size of the file measured in bytes */
        public long Size { get; private set; }
        /**
         * This value represents the type of file as defined by the
         * <code>FileType</code> enum
         */
        public FileType Type { get; private set; }
        /**
         * This value represents the file system type of the file as defined by the
         * <code>FileSystemType</code> enum.
         */
        public FileSystemType SystemType { get; private set; }
        /**
         * This value represents the creation time of the file.
         */
        public DateTime UTCCreationTime { get; private set; }
        /**
         * This value represents the last time the file was accessed.
         */
        public DateTime UTCLastAccessTime { get; private set; }
        /**
         * This value represents the last time the file was modified.
         */
        public DateTime UTCLastModifiedTime { get; private set; }

        internal FileInfo(PPError result, PPFileInfo ppFileInfo)
        {
            QueryResult = result;
            Size = ppFileInfo.size;
            Type = (FileType)ppFileInfo.type;
            SystemType = (FileSystemType)ppFileInfo.system_type;
            /* UTC "wall clock time" according to the browser */
            UTCCreationTime = PepperSharpUtils.ConvertFromPepperTimestamp(ppFileInfo.creation_time);
            UTCLastAccessTime = PepperSharpUtils.ConvertFromPepperTimestamp(ppFileInfo.last_access_time);
            UTCLastModifiedTime = PepperSharpUtils.ConvertFromPepperTimestamp(ppFileInfo.last_modified_time);
        }
    }

    public sealed class DirectoryEntry : IDisposable
    {
        public FileRef FileRef { get; private set; }
        public FileType FileType { get; private set; }

        internal DirectoryEntry (PPDirectoryEntry directoryEntry)
        {
            FileRef = new FileRef(directoryEntry.file_ref);
            FileType = (FileType)directoryEntry.file_type;
        }

        #region Implement IDisposable.

        public void Dispose()
        {
            if (FileRef != null)
                FileRef.Dispose();
            GC.SuppressFinalize(this);

        }

        ~DirectoryEntry()
        {
            Dispose();
        }

        #endregion
    }
}
