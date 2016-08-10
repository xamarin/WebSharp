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

        #region Equality

        public override bool Equals (object obj)
        {
            if (!(obj is PPRect))
                return false;

            PPRect comp = (PPRect)obj;

            return (comp.X == this.X) &&
            (comp.Y == this.Y) &&
            (comp.Width == this.Width) &&
            (comp.Height == this.Height);
        }

        public override int GetHashCode ()
        {
            return unchecked((int)((UInt32)X ^
                        (((UInt32)Y << 13) | ((UInt32)Y >> 19)) ^
                        (((UInt32)Width << 26) | ((UInt32)Width >> 6)) ^
                        (((UInt32)Height << 7) | ((UInt32)Height >> 25))));
        }

        public static bool operator == (PPRect left, PPRect right)
        {
            return (left.X == right.X
                    && left.Y == right.Y
                    && left.Width == right.Width
                    && left.Height == right.Height);
        }

        public static bool operator != (PPRect left, PPRect right)
        {
            return !(left == right);
        }
        #endregion Equality

    }
}
