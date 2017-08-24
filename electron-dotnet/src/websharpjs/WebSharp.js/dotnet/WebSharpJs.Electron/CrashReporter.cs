using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class CrashReporter : EventEmitter
    {
        protected override string ScriptProxy => @"crashReporter;";
        protected override string Requires => @"const {crashReporter} = require('electron');";

        static CrashReporter proxy;

        public static new async Task<CrashReporter> Create()
        {
            throw new NotSupportedException("Create() is not valid.  Use Instance() to obtain a reference to CrashReporter.");
        }

        public static async Task<CrashReporter> Instance()
        {
            if (proxy == null)
            {
                proxy = new CrashReporter();
                await proxy.Initialize();
            }
            return proxy;

        }

        protected CrashReporter() : base() { }

        protected CrashReporter(object scriptObject) : base(scriptObject)
        { }

        public CrashReporter(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator CrashReporter(ScriptObjectProxy sop)
        {
            return new CrashReporter(sop);
        }

        public async Task Start(CrashReporterStartOptions options)
        {
            await Invoke<object>("start", options);
        }

        public async Task<CrashReport> GetLastCrashReport()
        {
            return await Invoke<CrashReport>("getLastCrashReport");
        }

        public async Task<CrashReport[]> GetUploadedReports()
        {
            return await Invoke<CrashReport[]>("getUploadedReports");
        }

        public async Task<bool> GetUploadToServer()
        {
            return await Invoke<bool>("getUploadToServer");
        }

        public async Task SetUploadToServer(bool uploadToServer)
        {
            await Invoke<object>("setUploadToServer", uploadToServer);
        }

        public async Task SetExtraParameter(string key, string value)
        {
            await Invoke<object>("setExtraParameter", key, value);
        }

    }

    [ScriptableType]
    public class CrashReporterStartOptions
    {
        [ScriptableMember(ScriptAlias = "companyName")]
        public string CompanyName { get; set; } = string.Empty;
        [ScriptableMember(ScriptAlias = "submitURL")]
        public string SubmitURL { get; set; } = string.Empty;
        [ScriptableMember(ScriptAlias = "productName")]
        public string ProductName { get; set; } = string.Empty;
        [ScriptableMember(ScriptAlias = "uploadToServer")]
        public bool? UploadToServer { get; set; }
        [ScriptableMember(ScriptAlias = "ignoreSystemCrashHandler")]
        public bool? IgnoreSystemCrashHandler { get; set; }
        [ScriptableMember(ScriptAlias = "extra")]
        public object Extra { get; set; }
    }
}
