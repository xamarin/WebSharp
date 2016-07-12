using System;


namespace PepperSharp
{
    public partial struct PPResource
    {
        public static readonly PPResource Empty = new PPResource(0);

        internal PPResource(int resourceId)
        {
            ppresource = resourceId;
        }

    }
}
