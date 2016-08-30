using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using PepperSharp;

namespace FileIO
{
    public class FileIO : Instance
    {

        PPResource fileSystem;
        MessageLoop messageLoop;

        // Indicates whether file_system_ was opened successfully. We only read/write
        // this on the file_thread_.
        bool isFileSystemReady = false;

        public FileIO (IntPtr handle) : base(handle)
        {
            fileSystem = PPBFileSystem.Create(this, PPFileSystemType.Localpersistent);

            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {

            // Open the file system on the file_thread_. Since this is the first
            // operation we perform there, and because we do everything on the
            // file_thread_ synchronously, this ensures that the FileSystem is open
            // before any FileIO operations execute.
            messageLoop = CreateMessageLoop();
            var startTask = messageLoop.Start();

            messageLoop.PostWork(OpenFileSystem);
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

            
            Console.WriteLine($"command: {command} file_name: {fileName}");
            if (command == "load")
            {
                messageLoop.PostWork(Load, fileName);
                
            }
            else if (command == "save")
            {
                var fileText = message[2].AsString();
                messageLoop.PostWork(Save, fileName, fileText);
            }
            else if (command == "delete")
            {
                messageLoop.PostWork(Delete, fileName);
            }
            else if (command == "list")
            {
                var dirName = fileName;
                messageLoop.PostWork(List, dirName);
            }
            else if (command == "makedir")
            {
                var dirName = fileName;
                messageLoop.PostWork(MakeDir, dirName);
            }
            else if (command == "rename")
            {
                var neName = message[2].AsString();
                messageLoop.PostWork(Rename, fileName, neName);
            }
        }

        private void Delete(PPError result, string file_name)
        {
            if (!isFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            var fileref = PPBFileRef.Create(fileSystem, file_name);

            result = (PPError)PPBFileRef.Delete(fileref, new BlockUntilComplete());
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

        private void MakeDir(PPError result, string dirName)
        {
            if (!isFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }
            var refDir = PPBFileRef.Create(fileSystem, dirName);

            result = (PPError)PPBFileRef.MakeDirectory(refDir, (int)PPMakeDirectoryFlags.MakedirectoryflagNone, new BlockUntilComplete());

            if (result != PPError.Ok)
            {
                ShowErrorMessage("Make directory failed", result);
                return;
            }
            ShowStatusMessage("Make directory success");
        }

        private void Rename(PPError result, string oldName, string newName)
        {
            if (!isFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            var refOld = PPBFileRef.Create(fileSystem, oldName);
            var refNew = PPBFileRef.Create(fileSystem, newName);

            result = (PPError)PPBFileRef.Rename(refOld, refNew, new BlockUntilComplete());
            if (result != PPError.Ok)
            {
                ShowErrorMessage("Rename failed", result);
                return;
            }
            ShowStatusMessage("Rename success");
        }

        void OpenFileSystem(PPError result)
        {
            
            var rv = (PPError)PPBFileSystem.Open(fileSystem, 1024 * 1024, new BlockUntilComplete());
            if (rv == PPError.Ok)
            {
                isFileSystemReady = true;
                // Notify the user interface that we're ready
                PostArrayMessage("READY");
            }
            else
            {
                ShowErrorMessage("Failed to open file system", rv);
            }
        }

        void Load(PPError result, string fileName)
        {
            
            if (!isFileSystemReady)
            {
                ShowErrorMessage("File system is not open", PPError.Failed);
                return;
            }

            var fileref = PPBFileRef.Create(fileSystem, fileName);
            var file = PPBFileIO.Create(this);

            var openResult = (PPError)PPBFileIO.Open(file, fileref, (int)PPFileOpenFlags.FileopenflagRead, new BlockUntilComplete());
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

            var info = new PPFileInfo();
            var queryResult = (PPError)PPBFileIO.Query(file, out info, new BlockUntilComplete());
            if (queryResult != PPError.Ok)
            {
                ShowErrorMessage("File query failed", queryResult);
                return;
            }
            // FileIO.Read() can only handle int32 sizes
            if (info.size > Int32.MaxValue)
            {
                ShowErrorMessage("File too big", PPError.Filetoobig);
                return;
            }

            int offset = 0;
            int bytesRead = 0;
            int bytesToRead = (int)info.size;
            var dataBuffer = new byte[bytesToRead];
            var dataBufferString = new StringBuilder();
            while (bytesToRead > 0)
            {
                Array.Clear(dataBuffer, 0, bytesToRead);
                bytesRead = PPBFileIO.Read(file, offset,
                            dataBuffer, (int)info.size - offset,
                           new BlockUntilComplete());

                if (bytesRead > 0)
                {
                    offset += bytesRead;
                    bytesToRead -= bytesRead;
                    dataBufferString.Append(Encoding.UTF8.GetString(dataBuffer));
                }
                else if (bytesRead < 0)
                {
                    // If bytes_read < PP_OK then it indicates the error code.
                    ShowErrorMessage("File read failed", bytesRead);
                    return;
                }
            }
            PostArrayMessage("DISP", dataBufferString.ToString());
            ShowStatusMessage("Load success");
        }

        void Save(PPError result,
            string fileName,
            string fileContents)
        {
            
            if (!isFileSystemReady) {
              ShowErrorMessage("File system is not open", PPError.Failed);
              return;
            }

            var fileref = PPBFileRef.Create(fileSystem, fileName);
            var file = PPBFileIO.Create(this);

            var openResult = (PPError)PPBFileIO.Open(file, fileref,
                (int)(PPFileOpenFlags.FileopenflagWrite | PPFileOpenFlags.FileopenflagCreate |
                                  PPFileOpenFlags.FileopenflagTruncate),
                new BlockUntilComplete());
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
                    bytesWritten = PPBFileIO.Write(file, offset,
                                               byteContents,
                                               byteContents.Length,
                                               new BlockUntilComplete());
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
            var flush_result = (PPError)PPBFileIO.Flush(file, new BlockUntilComplete());
            if (flush_result != PPError.Ok)
            {
                  ShowErrorMessage("File fail to flush", flush_result);
                  return;
            }


            ShowStatusMessage("Save success");
        }

        private void List(PPError result, string dir_name)
        {

            if (!isFileSystemReady) {
              ShowErrorMessage("File system is not open", PPError.Failed);
              return;
            }

            var fileRef = PPBFileRef.Create(fileSystem, dir_name);

            // Pass ref along to keep it alive.
            var listCallback = new CompletionCallbackWithOutput<PPDirectoryEntry[], PPResource>(ListCallback, fileRef);
            PPBFileRef.ReadDirectoryEntries(fileRef, listCallback, listCallback);
        }

        void ListCallback(PPError result,
                            PPDirectoryEntry[] entries,
                          PPResource unused_ref )
        {
            
            if (result != PPError.Ok)
            {
                ShowErrorMessage("List failed", result);
                return;
            }

            var sv = new List<string>();
            if (entries != null)
            {
                for (int i = 0; i < entries.Length; ++i)
                {
                    var name = (Var)PPBFileRef.GetName(entries[i].file_ref);
                    if (name.IsString)
                    {
                        sv.Add(name.AsString());
                    }
                }
            }
            PostArrayMessage("LIST", sv.ToArray());
            ShowStatusMessage("List success");
        }

        void PostArrayMessage(string command, string[] strings)
        {
            var message = new VarArray();
            message[0] = new Var(command);
            for (uint i = 0; i<strings.Length; ++i) {
              message[i + 1] = new Var( strings[i]);
            }

            PostMessage(message);
        }

        void PostArrayMessage(string command)
        {
            PostArrayMessage(command, new string[0]);
        }

        void PostArrayMessage(string command, string s)
        {
            string[] sv = new string[1] { s };
            PostArrayMessage(command, sv);
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
