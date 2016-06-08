using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices;
namespace Pepper {

	public partial class PPInstance : PepperBase {
		public PPInstance (IntPtr handle) : base (handle) {}

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern static void _LogToConsole(IntPtr handle, int logLevel, string msg);
        public void LogToConsole(int logLevel, string msg)
        {
            _LogToConsole(handle, logLevel, msg);
        }

	} /* end class PPInstance*/
}
