using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PPRect
    {
        internal PP_Rect rect;

        public static readonly PPRect Zero = new PPRect(0, 0, 0, 0);

        public PPRect(int x, int y, int width, int height)
        {
            rect = new PP_Rect();
            rect.point.x = x;
            rect.point.y = y;
            rect.size.width = width;
            rect.size.height = height;
        }

        public PPRect (PPPoint origin, PPSize size) 
            : this(origin.X, origin.Y, size.Width, size.Height)
        {  }

        public PPRect(PP_Rect rect)
            : this(rect.point.x, rect.point.y, rect.size.width, rect.size.height)
        { }

        public PPPoint Origin
        {
            get { return rect.point;  }
            set { rect.point = value;  }
        }

        public PPSize Size
        {
            get { return rect.size;  }
            set { rect.size = value;  }
        }

        public int X
        {
            get { return rect.point.x;  }
            set { rect.point.x = value;  }
        }

        public int Y
        {
            get { return rect.point.y; }
            set { rect.point.y = value; }
        }

        public int Width
        {
            get { return rect.size.width; }
            set { rect.size.width = value; }
        }

        public int Height
        {
            get { return rect.size.height; }
            set { rect.size.height = value; }
        }

        public override string ToString()
        {
            return String.Format($"PPRect : ({rect.point.x},{rect.point.y}),({rect.size.width}, {rect.size.height})");
        }

        public static implicit operator PP_Rect(PPRect rect)
        {
            return rect.rect;
        }

        public static implicit operator PPRect(PP_Rect rect)
        {
            return new PPRect(rect);
        }
    }
}
