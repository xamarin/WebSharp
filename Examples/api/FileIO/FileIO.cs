using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using PepperSharp;
using System.Linq;

namespace FileIO
{
    public class FileIO : Instance
    {

        FileSystem fileSystem;
        MessageLoop messageLoop;

        // Indicates whether file_system_ was opened successfully. We only read/write
        // this on the file_thread_.
        bool IsFileSystemReady { get; set; } = false;

        public FileIO (IntPtr handle) : base(handle)
        {
            fileSystem = new FileSystem(this, FileSystemType.LocalPersistent);

            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {

            // Open the file system on the file_thread_. Since this is the first
            // operation we perform there, and because we do everything on the
            // file_thread_ synchronously, this ensures that the FileSystem is open
            // before any FileIO operations execute.
            messageLoop = new MessageLoop(this);
            var startTask = messageLoop.Start();

            // Set the MessageLoop that the filesystem will run asynchronously with
            // This is not necassary though as the async will run regardless if a 
            // MessageLoop is set or not.
            fileSystem.MessageLoop = messageLoop;

            OpenFileSystem();
        }

        /// <summary>
        /// Handler for messages coming in from the browser via postMessage().  The
        /// @a var_message can contain anything: a JSON string; a string that encodes
        /// method names and arguments; etc.
        ///
        /// Here we use messages to communicate with the user interface
        ///
        /// </summary>
        /// <param name="vmessage">The message posted by the browser.</param>
        private void OnHandleMessage(object sender, Var varMessage)
        {

            if (!varMessage.IsArray)
                return;

            // Message should be an array with the following elements:
            // [command, path, extra args]
            var message = new VarArray(varMessage);
            
            var command = message[0].AsString();
            var fileName = message[1].AsString();

            if (fileName.Length == 0 || fileName[0] != '/')
            {
                ShowStatusMessage("File name must begin with /");
                return;
            }

            
            Console.WriteLine($"command: {command} File Name: {fileName}");
            if (command == "load")
            {
                Load(fileName);
                
            }
            else if (command == "save")
            {
                var fileText = message[2].AsString();
                Save(fileName, fileText);
            }
            else if (command == "delete")
            {
                Delete(fileName);
            }
            else if (command == "list")
            {
                var dirName = fileName;
                List(dirName);
            }
            else if (command == "makedir")
            {
                var dirName = fileName;
                MakeDir(dirName);
            }
            else if (command == "rename")
            {
                var newName = message[2].AsString();
                Rename(fileName, newName);
            }
            else if (command == "query")
            {
                var dirName = fileName;
                Query(fileName);
            }
        }

        private async Task Delete(string fileName)
        {
            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var fileref = new FileRef(fileSystem, fileName))
            {
                var result = await fileref.DeleteAsync();
                if (result == PPError.Filenotfound)
                {
                    ShowErrorMessage("File/Directory not found", result);
                    return;
                }
                else if (result != PPError.Ok)
                {
                    ShowErrorMessage("Deletion failed", result);
                    return;
                }
                ShowStatusMessage("Delete success");
            }
        }

        private async Task MakeDir(string dirName)
        {
            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var refDir = new FileRef(fileSystem, dirName))
            {
                var makeResult = await refDir.MakeDirectoryAsync(MakeDirectoryFlags.None);
                if (makeResult != PPError.Ok)
                {
                    ShowErrorMessage("Make directory failed", makeResult);
                    return;
                }
                ShowStatusMessage("Make directory success");
            }
        }

        private async Task Rename(string oldName, string newName)
        {
            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var refOld = new FileRef(fileSystem, oldName))
            using (var refNew = new FileRef(fileSystem, newName))
            {

                var fileInfo = await refOld.QueryAsync();
                var strInfo = new StringBuilder();
                strInfo.Append($"QueryResult {fileInfo.QueryResult}\n");
                strInfo.Append($"Size {fileInfo.Size}\n");
                strInfo.Append($"Type {fileInfo.Type}\n");
                strInfo.Append($"SystemType {fileInfo.SystemType}\n");
                strInfo.Append($"CreationTime {fileInfo.UTCCreationTime}\n");
                strInfo.Append($"LastAccessTime {fileInfo.UTCLastAccessTime}\n");
                strInfo.Append($"LastModifiedTime {fileInfo.UTCLastModifiedTime}\n");
                LogToConsole(PPLogLevel.Log, strInfo.ToString());

                var result = await refOld.RenameAsync(refNew);
                if (result != PPError.Ok)
                {
                    ShowErrorMessage("Rename failed", result);
                    return;
                }
                ShowStatusMessage("Rename success");
            }
        }

        async Task OpenFileSystem()
        {
            
            var rv = await fileSystem.OpenAsync(1024 * 1024);
            if (rv == PPError.Ok)
            {
                IsFileSystemReady = true;
                // Notify the user interface that we're ready
                PostArrayMessage("READY");
            }
            else
            {
                ShowErrorMessage("Failed to open file system", rv);
            }
        }

        async Task Load(string fileName)
        {
            
            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var fileref = new FileRef(fileSystem, fileName))
            using (var file = new PepperSharp.FileIO(this))
            {

                var openResult = await file.OpenAsync(fileref, FileOpenFlags.Read);
                if (openResult == PPError.Filenotfound)
                {
                    ShowErrorMessage("File not found", openResult);
                    return;
                }
                else if (openResult != PPError.Ok)
                {
                    ShowErrorMessage("File open for read failed", openResult);
                    return;
                }

                var queryResult = await file.QueryAsync();
                if (queryResult.QueryResult != PPError.Ok)
                {
                    ShowErrorMessage("File query failed", queryResult.QueryResult);
                    return;
                }

                // FileIO.Read() can only handle int32 sizes
                if (queryResult.Size > Int32.MaxValue)
                {
                    ShowErrorMessage("File too big", PPError.Filetoobig);
                    return;
                }

                int numBytesRead = 0;
                int numBytesToRead = (int)queryResult.Size;
                var bytes = new byte[numBytesToRead];
                var dataBufferString = new StringBuilder();

                var readBuffer = new ArraySegment<byte>(bytes);

                while (numBytesToRead > 0)
                {
                    var readResult = await file.ReadAsync(readBuffer, numBytesRead,
                                readBuffer.Count);

                    if (readResult.EndOfFile)
                        break;

                    if (readResult.Count > 0)
                    {
                        byte[] readBytes = readBuffer.Skip(readBuffer.Offset).Take((int)readResult.Count).ToArray();
                        dataBufferString.Append(Encoding.UTF8.GetString(readBytes));
                    }
                    else
                    {
                        ShowErrorMessage("File read failed", numBytesRead);
                        return;
                    }

                    numBytesRead += readResult.Count;
                    numBytesToRead -= readResult.Count;
                }
                PostArrayMessage("DISP", dataBufferString.ToString());
                ShowStatusMessage("Load success");
            }
        }

        async Task Save(string fileName,
            string fileContents)
        {
            
            if (!IsFileSystemReady) {
              ShowErrorMessage("File system is not open", PPError.Failed);
              return;
            }

            using (var fileref = new FileRef(fileSystem, fileName))
            using (var file = new PepperSharp.FileIO(this))
            {
                var openResult = await file.OpenAsync(fileref,
                    FileOpenFlags.Write | FileOpenFlags.Create | FileOpenFlags.Truncate);
                if (openResult != PPError.Ok)
                {
                    ShowErrorMessage("File open for write failed", openResult);
                    return;
                }

                // We have truncated the file to 0 bytes. So we need only write if
                // file_contents is non-empty.
                if (!string.IsNullOrEmpty(fileContents.ToString()))
                {
                    if (fileContents.Length > Int32.MaxValue)
                    {
                        ShowErrorMessage("File too big", PPError.Filetoobig);
                        return;
                    }
                    int offset = 0;
                    int bytesWritten = 0;
                    var byteContents = Encoding.UTF8.GetBytes(fileContents);
                    do
                    {
                        var writeResult = await file.WriteAsync(byteContents,
                            offset, byteContents.Length);

                        if (writeResult.EndOfFile)
                            return;

                        bytesWritten = writeResult.Count;

                        if (bytesWritten > 0)
                        {
                            offset += bytesWritten;
                        }
                        else
                        {
                            ShowErrorMessage("File write failed", bytesWritten);
                            return;
                        }
                    } while (bytesWritten < fileContents.Length);

                }
                // All bytes have been written, flush the write buffer to complete
                var flush_result = await file.FlushAsync();
                if (flush_result != PPError.Ok)
                {
                    ShowErrorMessage("File fail to flush", flush_result);
                    return;
                }


                ShowStatusMessage("Save success");
            }
        }

        private async Task List(string dir_name)
        {

            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var fileRef = new FileRef(fileSystem, dir_name))
            {

                using (var listResult = await fileRef.ReadDirectoryEntriesAsync(messageLoop))
                {
                    if (listResult.Result != PPError.Ok)
                    {
                        ShowErrorMessage("List failed", listResult.Result);
                        return;
                    }

                    var entries = new List<string>();
                    foreach (var entry in listResult.Entries)
                    {
                        entries.Add(entry.FileRef.Name);
                    }

                    PostArrayMessage("LIST", entries.ToArray());
                    ShowStatusMessage("List success");

                }
            }
        }

        private async Task Query(string fileName)
        {
            if (!IsFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            using (var fileRef = new FileRef(fileSystem, fileName))
            {
                var fileInfo = await fileRef.QueryAsync();
                var strInfo = new List<string>();
                strInfo.Add($"QueryResult:      {fileInfo.QueryResult}");
                strInfo.Add($"Size:             {fileInfo.Size}");
                strInfo.Add($"Type:             {fileInfo.Type}");
                strInfo.Add($"SystemType:       {fileInfo.SystemType}");
                strInfo.Add($"CreationTime:     {fileInfo.UTCCreationTime.ToLocalTime()}");
                strInfo.Add($"LastAccessTime:   {fileInfo.UTCLastAccessTime.ToLocalTime()}");
                strInfo.Add($"LastModifiedTime: {fileInfo.UTCLastModifiedTime.ToLocalTime()}");

                if (fileInfo.QueryResult != PPError.Ok)
                {
                    ShowErrorMessage("Query failed", fileInfo.QueryResult);
                    return;
                }
                PostArrayMessage("QUERY", strInfo.ToArray());
                ShowStatusMessage("Query success");
            }
        }

        void PostArrayMessage(string command, params string[] strings)
        {
            using (var message = new VarArray())
            {
                message[0] = new Var(command);
                for (uint i = 0; i < strings.Length; ++i)
                {
                    message[i + 1] = new Var(strings[i]);
                }

                PostMessage(message);
            }
        }

        /// Encapsulates our simple javascript communication protocol
        void ShowErrorMessage(string message, int result)
        {
            ShowErrorMessage(message, (PPError)result);
        }

        /// Encapsulates our simple javascript communication protocol
        void ShowErrorMessage(string message, PPError result)
        {
            PostArrayMessage("ERR", $"{message} -- Error #: {result}");
        }

        void ShowStatusMessage(string message)
        {
            PostArrayMessage("STAT", message);
        }
    }
}
