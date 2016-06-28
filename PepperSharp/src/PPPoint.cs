using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PPPoint
    {

        internal PP_Point point;

        public static readonly PPPoint Zero = new PPPoint(0, 0);

        public PPPoint (int x, int y)
        {
            point = new PP_Point();
            point.x = x;
            point.y = y;
        }

        public PPPoint(PPPoint other)
        {
            point = new PP_Point();
            point.x = other.X;
            point.y = other.Y;
        }

        public PPPoint(PP_Point other)
        {
            point = new PP_Point();
            point.x = other.x;
            point.y = other.y;

        }

        public int X
        {
            get { return point.x; }
            set { point.x = value; }
        }

        public int Y
        {
            get { return point.y; }
            set { point.y = value; }
        }

        public void Swap (ref PPPoint other)
        {
            int tempX = X;
            int tempY = Y;
            X = other.X;
            Y = other.Y;
            other.X = tempX;
            other.Y = tempY;
        }

        public override string ToString()
        {
            return String.Format("PPPoint : (x={0}, y={1})", X, Y);
        }

        public static implicit operator PP_Point (PPPoint point)
        {
            return point.point;
        }

        public static implicit operator PPPoint (PP_Point point)
        {
            return new PPPoint(point);
        }
    }
}
