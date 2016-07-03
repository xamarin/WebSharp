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

        public override string ToString()
        {
            return String.Format($"PPRect : ({point.x},{point.y}),({size.width}, {size.height})");
        }

    }
}
