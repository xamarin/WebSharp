using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class DesktopCapturer : NodeJsObject
    {
        protected override string ScriptProxy => @"desktopCapturer; ";
        protected override string Requires => @"const {desktopCapturer} = require('electron');";

        static DesktopCapturer proxy;

        public static async Task<DesktopCapturer> Instance()
        {
            if (proxy == null)
            {
                proxy = new DesktopCapturer();
                await proxy.Initialize();
            }
            return proxy;
        }

        protected DesktopCapturer() : base() { }

        protected DesktopCapturer(object scriptObject) : base(scriptObject)
        { }

        public DesktopCapturer(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator DesktopCapturer(ScriptObjectProxy sop)
        {
            return new DesktopCapturer(sop);
        }

        public async Task<object> GetSources(DesktopCapturerOptions options, ScriptObjectCallback<Error, DesktopCapturerSource[]> callback)
        {
            return await Invoke<object>("getSources", options, callback);
        }

    }


    [Flags]
    public enum DesktopCapturerType
    {
        Screen,
        Window
    }

    [ScriptableType]
    public struct DesktopCapturerOptions
    {
        [ScriptableMember(ScriptAlias = "types")]
        public string[] TypeDescriptions
        {
            get
            {
                if (Types == null || !Types.HasValue)
                    return new string[0];

                var properties = new List<string>();

                if (Types.Value.HasFlag(DesktopCapturerType.Screen))
                    properties.Add("screen");
                if (Types.Value.HasFlag(DesktopCapturerType.Window))
                    properties.Add("window");
                return properties.ToArray();
            }
        
        }
        public DesktopCapturerType? Types { get; set; }
        public Size ThumbnailSize { get; set; }
    }

}
