using System;
using System.Runtime.CompilerServices;

namespace Mono.WebAssembly
{

	public sealed class Runtime {
		
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		static extern string ExecuteJavaScript (string str, out int exceptional_result);

		public static string ExecuteJavaScript (string str)
		{
			Console.WriteLine($"CS::Mono.WebAssembly.Runtime::ExecuteJavaScript {str}");
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