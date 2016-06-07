//
// Runtime.cs: Basic features for the Pepper C# runtime
//
// Authors:
//   Kenneth Pouncey (kenneth.pouncey@xamarin.com)
//
// Copyright 2016 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;

namespace Pepper {
	public interface INativeObject {
		IntPtr Handle { get; }
	}

	public class PepperBase : INativeObject, IDisposable {
		internal IntPtr handle;
		public IntPtr Handle { get { return handle; } }
		
		public PepperBase (IntPtr handle)
		{
			this.handle = handle;
		}
		
		protected PepperBase() {}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			// TODO: do something here
		}

		~PepperBase ()
		{
			Dispose (false);
		}

	}
}