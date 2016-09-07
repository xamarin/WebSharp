using System;

namespace PepperSharp
{
    public class NetAddress : Resource
    {
        internal NetAddress(PPResource resource) : base(PassRef.PassRef, resource)
        { }

        public NetAddress(Instance instance, ushort port, byte[] address = null)
        {

            if (address == null || address.Length <= PPNetAddressIPv4.IPv4AddressBytes)
            {
                var ipAddress = new PPNetAddressIPv4(port, address);
                handle = PPBNetAddress.CreateFromIPv4Address(instance, ipAddress);
            }
            else
            {
                var ipAddress = new PPNetAddressIPv6(port, address);
                handle = PPBNetAddress.CreateFromIPv6Address(instance, ipAddress);
            }
        }

        /// <summary>
        /// Gets the address family
        /// </summary>
        public NetAddressFamily Family
            => (NetAddressFamily)PPBNetAddress.GetFamily(this);

        /// <summary>
        /// Gets the string description
        /// </summary>
        public string Description
            => ((Var)PPBNetAddress.DescribeAsString(this, PPBool.False)).AsString();

        /// <summary>
        /// Gets the string description appended with the prot
        /// </summary>
        public string DescriptionWithPort
            => ((Var)PPBNetAddress.DescribeAsString(this, PPBool.True)).AsString();

        /// <summary>
        /// Returns an IPv4 NetAddress
        /// </summary>
        public PPNetAddressIPv4 IPv4
        {
            get
            {
                var ipv4 = new PPNetAddressIPv4();
                PPBNetAddress.DescribeAsIPv4Address(this, out ipv4);
                return ipv4;
            }
        }

        /// <summary>
        /// Returns an IPv6 NetAddress
        /// </summary>
        public PPNetAddressIPv6 IPv6
        {
            get
            {
                var ipv6 = new PPNetAddressIPv6();
                PPBNetAddress.DescribeAsIPv6Address(this, out ipv6);
                return ipv6;
            }
        }

    }

    /**
    * Network address family types.
    */
    public enum NetAddressFamily
    {
        /**
         * The address family is unspecified.
         */
        Unspecified = 0,
        /**
         * The Internet Protocol version 4 (IPv4) address family.
         */
        Ipv4 = 1,
        /**
         * The Internet Protocol version 6 (IPv6) address family.
         */
        Ipv6 = 2
    }
}
