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

        /// GetArea() determines the area (width * height).
        ///
        /// @return The area.
        public int Area {
            get { return Width * Height;  }
        }

        public override string ToString()
        {
            return String.Format("PPSize : (width={0}, height={1})", Width, Height);
        }

        #region Equality

        public override bool Equals (object obj)
        {
            if (!(obj is PPSize))
                return false;

            PPSize comp = (PPSize)obj;
            return (comp.width == this.width) &&
                   (comp.height == this.height);
        }

        public override int GetHashCode ()
        {
            return width ^ height;
        }

        public static bool operator == (PPSize sz1, PPSize sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        public static bool operator != (PPSize sz1, PPSize sz2)
        {
            return !(sz1 == sz2);
        }

        #endregion

    }
}
