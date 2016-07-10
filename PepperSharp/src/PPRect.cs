using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{

    public partial struct PPRect
    {

        public static readonly PPRect Zero = new PPRect(0, 0, 0, 0);

        public PPRect(int x, int y, int width, int height)
        {
            point.x = x;
            point.y = y;
            size.width = width;
            size.height = height;
        }

        public PPRect (PPPoint origin, PPSize size) 
            : this(origin.X, origin.Y, size.Width, size.Height)
        {  }

        public PPRect(int width, int height)
            :this(0,0,width,height)
        { }

        public PPRect(PPSize size)
            : this(size.Width, size.Height)
        { }

        public PPPoint Origin
        {
            get { return point;  }
            set { point = value;  }
        }

        public PPSize Size
        {
            get { return size;  }
            set { size = value;  }
        }

        public int X
        {
            get { return point.x;  }
            set { point.x = value;  }
        }

        public int Y
        {
            get { return point.y; }
            set { point.y = value; }
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

        public int Right
        {
            get { return X + Width; }
        }

        public int Bottom
        {
            get { return Y + Height; }
        }

        public override string ToString()
        {
            return String.Format($"PPRect : ({point.x},{point.y}),({size.width}, {size.height})");
        }

        public static bool operator ==(PPRect r1, PPRect r2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(r1, r2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)r1 == null) || ((object)r2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return r1.X == r2.X && r1.Y == r2.Y && r1.Width == r2.Height;
        }

        public static bool operator !=(PPRect r1, PPRect r2)
        {
            return !(r1 == r2);
        }
    }
}
