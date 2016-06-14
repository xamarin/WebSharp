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

	public class PPResource : INativeObject, IDisposable {
		internal IntPtr handle;
        public IntPtr Handle => handle;
		
		public PPResource (IntPtr handle)
		{
			this.handle = handle;
            Console.WriteLine($"PPResource constructed {handle}");
        }
		
		protected PPResource() {}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			// TODO: do something here
		}

		~PPResource ()
		{
			Dispose (false);
		}

	}

    public partial struct PPSize
    {
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }

        public PPSize (int width, int height )
        {
            this.width = width;
            this.height = height;
        }
    }
}