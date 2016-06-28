using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PPSize
    {
        internal PP_Size size;
        public static readonly PPSize Zero = new PPSize(0, 0);

        public PPSize(int width, int height)
        {
            size = new PP_Size();
            size.width = width;
            size.height = height;
        }

        public PPSize(PPSize other)
        {
            size = new PP_Size();
            size.width = other.Width;
            size.height = other.Height;
        }

        public PPSize(PP_Size other)
        {
            size = new PP_Size();
            size.width = other.width;
            size.height = other.height;

        }

        public int Width
        {
            get { return size.width; }
            set { size.width = value; }
        }

        public int Height
        {
            get { return size.height; }
            set { size.height = value; }
        }

        public override string ToString()
        {
            return String.Format("PPSize : (width={0}, height={1})", Width, Height);
        }

        public static implicit operator PP_Size(PPSize size)
        {
            return size.size;
        }

        public static implicit operator PPSize(PP_Size size)
        {
            return new PPSize(size);
        }
    }
}
