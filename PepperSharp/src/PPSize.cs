using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    public partial struct PPSize
    {
        public static readonly PPSize Zero = new PPSize(0, 0);

        public PPSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public PPSize(PPSize other)
        {
            width = other.Width;
            height = other.Height;
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public override string ToString()
        {
            return String.Format("PPSize : (width={0}, height={1})", Width, Height);
        }

    }
}
