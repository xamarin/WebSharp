using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public partial struct PPPoint
    {

        public static readonly PPPoint Zero = new PPPoint(0, 0);

        public PPPoint (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PPPoint(PPPoint other)
        {
            x = other.X;
            y = other.Y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
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

        #region Equality

        public override bool Equals (object obj)
        {
            if (!(obj is PPPoint)) return false;
            PPPoint comp = (PPPoint)obj;
            // Note value types can't have derived classes, so we don't need 
            // to check the types of the objects here.  -- Microsoft, 2/21/2001
            return comp.X == this.X && comp.Y == this.Y;
        }

        public override int GetHashCode ()
        {
            return unchecked(x ^ y);
        }

        public static bool operator == (PPPoint left, PPPoint right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator != (PPPoint left, PPPoint right)
        {
            return !(left == right);
        }
        #endregion Equality

    }
}
