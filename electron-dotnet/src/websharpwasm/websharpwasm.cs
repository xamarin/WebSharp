using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace WebAssembly {

	public sealed class Runtime {
		
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		static extern string InvokeJS (string str, out int exceptional_result);

		public static string InvokeJS (string str)
		{
			int exception = 0;
			var res = InvokeJS (str, out exception);
			if (exception != 0)
				throw new JSException (res);
			return res;
		}
	}

	public class JSException : Exception {
		public JSException (string msg) : base (msg) {}
	}
}

public class Driver {
	static void Main () {
		Console.WriteLine ("hello");
		//Send ("run", "mini");
	}

	public static string Send (string key, string val) {

		if (key == "sayHello")
		{

			Console.WriteLine($"Hello from {val}");
			WebAssembly.Runtime.InvokeJS("sayHello2()");
			return null;
		}


		if (key == "eval")
		{
			var aa = WebAssembly.Runtime.InvokeJS(val);
			return aa;
		}

		return "INVALID-KEY is returned";


	}


}
