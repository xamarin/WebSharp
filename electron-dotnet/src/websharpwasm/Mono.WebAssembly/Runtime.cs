using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Mono.WebAssembly
{

	public sealed class Runtime {
		
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		static extern string ExecuteJavaScript (string str, out int exceptional_result);

		public static string ExecuteJavaScript (string str)
		{
			int exception = 0;
			var res = ExecuteJavaScript (str, out exception);
			if (exception != 0)
				throw new JSException (res);
			return res;
		}
	}

	public class JSException : Exception {
		public JSException (string msg) : base (msg) {}
	}
}