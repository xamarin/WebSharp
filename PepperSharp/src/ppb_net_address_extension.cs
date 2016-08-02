using System;


namespace PepperSharp
{
    public partial struct PPNetAddressIPv4
    {
        internal const int IPv4AddressBytes = 4;
        public static readonly byte[] IPv4Any = new byte[] { 0, 0, 0, 0 };

        public PPNetAddressIPv4(ushort port, byte[] address = null) : this()
        {
            this.port = port;

            if (address == null)
                address = IPv4Any;

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (address.Length != IPv4AddressBytes)
            {
                throw new ArgumentException("address contains a bad IP address", "address");
            }

            if (address.Length == IPv4AddressBytes)
            {
                unsafe
                {
                    fixed (byte* fixedAddr = addr)
                    {
                        fixedAddr[0] = address[0];
                        fixedAddr[1] = address[1];
                        fixedAddr[2] = address[2];
                        fixedAddr[3] = address[3];
                    }
                }
                
            }
        }
    }

    public partial struct PPNetAddressIPv6
    {
        internal const int IPv6AddressBytes = 16;
        public static readonly byte[] IPv16Any = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public PPNetAddressIPv6(ushort port, byte[] address = null) : this()
        {
            this.port = port;

            if (address == null)
                address = IPv16Any;

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (address.Length != IPv6AddressBytes)
            {
                throw new ArgumentException("address contains a bad IP address", "address");
            }

            unsafe
            {
                fixed (byte* fixedAddr = addr)
                {
                    fixedAddr[0] = address[0];
                    fixedAddr[1] = address[1];
                    fixedAddr[2] = address[2];
                    fixedAddr[3] = address[3];
                    fixedAddr[4] = address[4];
                    fixedAddr[5] = address[5];
                    fixedAddr[6] = address[6];
                    fixedAddr[7] = address[7];
                    fixedAddr[8] = address[8];
                    fixedAddr[9] = address[9];
                    fixedAddr[10] = address[10];
                    fixedAddr[11] = address[11];
                    fixedAddr[12] = address[12];
                    fixedAddr[13] = address[13];
                    fixedAddr[14] = address[14];
                    fixedAddr[15] = address[15];
                }
            }

        }
    }
}
