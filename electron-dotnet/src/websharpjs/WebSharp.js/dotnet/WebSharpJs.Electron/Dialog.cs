using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{

    [Flags]
    public enum OpenDialogProperties
    {
        OpenFile = 1,
        OpenDirectory = 2,
        MultiSelections = 4,
        CreateDirectory = 8,
        ShowHiddenFiles = 16
    }

    [ScriptableType]
    public struct OpenDialogOptions
    {
        [ScriptableMember(ScriptAlias = "title")]
        public string Title { get; set; }
        [ScriptableMember(ScriptAlias = "defaultPath")]
        public string DefaultPath { get; set; }
        [ScriptableMember(ScriptAlias = "buttonLabel")]
        public string ButtonLabel { get; set; }
        [ScriptableMember(ScriptAlias = "filters")]
        public FileFilter[] Filters { get; set; }
        [ScriptableMember(ScriptAlias = "properties")]
        public string[] Properties
        {
            get
            {
                
                if (PropertyFlags == 0)
                    return null;
                
                var properties = new List<string>();

                if (PropertyFlags.HasFlag(OpenDialogProperties.OpenFile))
                    properties.Add("openFile");
                if (PropertyFlags.HasFlag(OpenDialogProperties.OpenDirectory))
                    properties.Add("openDirectory");
                if (PropertyFlags.HasFlag(OpenDialogProperties.MultiSelections))
                    properties.Add("multiSelections");
                if (PropertyFlags.HasFlag(OpenDialogProperties.CreateDirectory))
                    properties.Add("createDirectory");
                if (PropertyFlags.HasFlag(OpenDialogProperties.ShowHiddenFiles))
                    properties.Add("showHiddenFiles");
                return properties.ToArray();
            }

        }
        public OpenDialogProperties PropertyFlags { get; set; }
    }

    [ScriptableType]
    public struct SaveDialogOptions
    {
        [ScriptableMember(ScriptAlias = "title")]
        public string Title { get; set; }
        [ScriptableMember(ScriptAlias = "defaultPath")]
        public string DefaultPath { get; set; }
        [ScriptableMember(ScriptAlias = "buttonLabel")]
        public string ButtonLabel { get; set; }
        [ScriptableMember(ScriptAlias = "filters")]
        public FileFilter[] Filters { get; set; }
    }

    public enum MessageBoxType
    {
        None,
        Info,
        Error,
        Question,
        Warning
    }

    [ScriptableType]
    public struct MessageBoxOptions
    {

        public MessageBoxType MessageBoxType { get; set; }
        [ScriptableMember(ScriptAlias = "buttons")]
        public string[] Buttons { get; set; }
        [ScriptableMember(ScriptAlias = "defaultId")]
        public int DefaultId { get; set; }
        [ScriptableMember(ScriptAlias = "title")]
        public string Title { get; set; }
        [ScriptableMember(ScriptAlias = "title")]
        public string Message { get; set; }
        [ScriptableMember(ScriptAlias = "detail")]
        public string Detail { get; set; }
        [ScriptableMember(ScriptAlias = "cancelId")]
        public int CancelId { get; set; }
        [ScriptableMember(ScriptAlias = "noLink")]
        public bool NoLink { get; set; }
        //[ScriptableMember(ScriptAlias = "icon")]
        //public NativeImage Icon { get; set; }

        [ScriptableMember(ScriptAlias = "type")]
        public string Type
        {
            get
            {
                switch (MessageBoxType)
                {
                    case MessageBoxType.None:
                        return "none";
                    case MessageBoxType.Info:
                        return "info";
                    case MessageBoxType.Error:
                        return "error";
                    case MessageBoxType.Question:
                        return "question";
                    case MessageBoxType.Warning:
                        return "warning";
                }

                return string.Empty;
            }

        }
        public OpenDialogProperties PropertyFlags { get; set; }
    }


    public class Dialog : NodeJsObject
    {
        protected override string ScriptProxy => @"dialog;";
        protected override string Requires => require;

        string require = string.Empty;
        static readonly string mainRequire = @"const {dialog} = require('electron')";
        static readonly string remoteRequire = @"const {dialog} = require('electron').remote";


        static Dialog proxy;

        public static async Task<Dialog> Main()
        {
            if (proxy == null)
            {
                proxy = new Dialog();
                proxy.require = mainRequire;
                await proxy.Initialize();
            }
            return proxy;
        }

        public static async Task<Dialog> Remote()
        {
            if (proxy == null)
            {
                proxy = new Dialog();
                proxy.require = remoteRequire;
                await proxy.Initialize();
            }
            return proxy;
        }

        protected Dialog() : base() { }

        protected Dialog(object scriptObject) : base(scriptObject)
        { }

        public Dialog(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator Dialog(ScriptObjectProxy sop)
        {
            return new Dialog(sop);
        }

        public async Task<string[]> ShowOpenDialog(OpenDialogOptions options, Func<object, Task<object>> callback = null)
        {
            object[] result;
            if (callback == null)
            {
                result = await Invoke<object[]>("showOpenDialog", options);
            }
            else
                result = await Invoke<object[]>("showOpenDialog", options, callback);
            return (result == null ? new string[] { } : Array.ConvertAll(result, item => item.ToString()));
        }

        public async Task<string> ShowSaveDialog(SaveDialogOptions options, Func<object, Task<object>> callback = null)
        {
            if (callback == null)
            {
                return await Invoke<string>("showSaveDialog", options);
            }
            else
                return await Invoke<string>("showSaveDialog", options, callback);
        }

        public async Task<int> ShowMessageBox(MessageBoxOptions options, Func<object, Task<object>> callback = null)
        {
            if (callback == null)
            {
                return await Invoke<int>("showMessageBox", options);
            }
            else
                return await Invoke<int>("showMessageBox", options, callback);
        }

        public async Task<object> ShowErrorBox(string title, string content)
        {
            return await Invoke<int>("showErrorBox", title, content);
        }
    }
}
