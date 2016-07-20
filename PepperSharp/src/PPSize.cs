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

        public static bool operator ==(PPSize s1, PPSize s2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(s1, s2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)s1 == null) || ((object)s2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return s1.Width == s2.Width && s1.Height == s2.Height;
        }

        public static bool operator !=(PPSize s1, PPSize s2)
        {
            return !(s1 == s2);
        }

    }
}
