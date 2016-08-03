using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepperSharp
{
    public partial struct PPGamepadSampleData
    {

        public uint AxesCount
        {
            get { return axes_length; }
        }

        public float Axes(int index)
        {
            if (index > AxesCount)
                throw new ArgumentOutOfRangeException("index out of range");

            unsafe
            {
                fixed (float* fixedAxes = axes)
                {
                    return fixedAxes[index];
                }
            }

        }

        public uint ButtonCount
        {
            get { return buttons_length; }
        }

        public float Button(int index)
        {
            if (index > ButtonCount)
                throw new ArgumentOutOfRangeException("index out of range");

            unsafe
            {
                fixed (float* fixedButtons = buttons)
                {
                    return fixedButtons[index];
                }
            }

        }

        public double TimeStamp
        {
            get { return timestamp; }
        }

        public bool IsConnected
        {
            get { return connected == PPBool.True; }
        }

        public ushort[] Id
        {
            
            get
            {
                var safeId = new ushort[128];
                unsafe
                {
                    fixed (ushort* fixedId = id)
                    {
                        for (int x = 0; x < safeId.Length; x++)
                            safeId[x] = fixedId[x];
                    }
                }
                return safeId;
            }

        }
        
    }

    public partial struct PPGamepadsSampleData
    {

        public uint Length
        {
            get { return length;  }
        }

        public PPGamepadSampleData this[int index]
        {
            get
            {
                if (index > length)
                    throw new ArgumentOutOfRangeException("index out of range");

                if (index == 0)
                    return items_1;
                if (index == 1)
                    return items_2;
                if (index == 2)
                    return items_3;
                if (index == 3)
                    return items_4;

                return items_1;
            }
        }
    }

}
