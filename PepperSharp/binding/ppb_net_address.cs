/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_net_address.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_NetAddress</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * Network address family types.
 */
public enum PPNetAddressFamily {
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
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * All members are expressed in network byte order.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPNetAddressIPv4 {
  /**
   * Port number.
   */
  public ushort port;
  /**
   * IPv4 address.
   */
  public unsafe fixed byte addr[4];
}

/**
 * All members are expressed in network byte order.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPNetAddressIPv6 {
  /**
   * Port number.
   */
  public ushort port;
  /**
   * IPv6 address.
   */
  public unsafe fixed byte addr[16];
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_NetAddress</code> interface provides operations on network
 * addresses.
 */
public static partial class PPBNetAddress {
  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetAddress_CreateFromIPv4Address")]
  extern static PPResource _CreateFromIPv4Address (
      PPInstance instance,
      PPNetAddressIPv4 ipv4_addr);

  /**
   * Creates a <code>PPB_NetAddress</code> resource with the specified IPv4
   * address.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   * @param[in] ipv4_addr An IPv4 address.
   *
   * @return A <code>PP_Resource</code> representing the same address as
   * <code>ipv4_addr</code> or 0 on failure.
   */
  public static PPResource CreateFromIPv4Address (
      PPInstance instance,
      PPNetAddressIPv4 ipv4_addr)
  {
  	return _CreateFromIPv4Address (instance, ipv4_addr);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetAddress_CreateFromIPv6Address")]
  extern static PPResource _CreateFromIPv6Address (
      PPInstance instance,
      PPNetAddressIPv6 ipv6_addr);

  /**
   * Creates a <code>PPB_NetAddress</code> resource with the specified IPv6
   * address.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   * @param[in] ipv6_addr An IPv6 address.
   *
   * @return A <code>PP_Resource</code> representing the same address as
   * <code>ipv6_addr</code> or 0 on failure.
   */
  public static PPResource CreateFromIPv6Address (
      PPInstance instance,
      PPNetAddressIPv6 ipv6_addr)
  {
  	return _CreateFromIPv6Address (instance, ipv6_addr);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetAddress_IsNetAddress")]
  extern static PPBool _IsNetAddress ( PPResource resource);

  /**
   * Determines if a given resource is a network address.
   *
   * @param[in] resource A <code>PP_Resource</code> to check.
   *
   * @return <code>PP_TRUE</code> if the input is a <code>PPB_NetAddress</code>
   * resource; <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsNetAddress ( PPResource resource)
  {
  	return _IsNetAddress (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetAddress_GetFamily")]
  extern static PPNetAddressFamily _GetFamily ( PPResource addr);

  /**
   * Gets the address family.
   *
   * @param[in] addr A <code>PP_Resource</code> corresponding to a network
   * address.
   *
   * @return The address family on success;
   * <code>PP_NETADDRESS_FAMILY_UNSPECIFIED</code> on failure.
   */
  public static PPNetAddressFamily GetFamily ( PPResource addr)
  {
  	return _GetFamily (addr);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetAddress_DescribeAsString")]
  extern static PPVar _DescribeAsString ( PPResource addr,
                                          PPBool include_port);

  /**
   * Returns a human-readable description of the network address. The
   * description is in the form of host [ ":" port ] and conforms to
   * http://tools.ietf.org/html/rfc3986#section-3.2 for IPv4 and IPv6 addresses
   * (e.g., "192.168.0.1", "192.168.0.1:99", or "[::1]:80").
   *
   * @param[in] addr A <code>PP_Resource</code> corresponding to a network
   * address.
   * @param[in] include_port Whether to include the port number in the
   * description.
   *
   * @return A string <code>PP_Var</code> on success; an undefined
   * <code>PP_Var</code> on failure.
   */
  public static PPVar DescribeAsString ( PPResource addr,  PPBool include_port)
  {
  	return _DescribeAsString (addr, include_port);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetAddress_DescribeAsIPv4Address")]
  extern static PPBool _DescribeAsIPv4Address (
      PPResource addr,
      out PPNetAddressIPv4 ipv4_addr);

  /**
   * Fills a <code>PP_NetAddress_IPv4</code> structure if the network address is
   * of <code>PP_NETADDRESS_FAMILY_IPV4</code> address family.
   * Note that passing a network address of
   * <code>PP_NETADDRESS_FAMILY_IPV6</code> address family will fail even if the
   * address is an IPv4-mapped IPv6 address.
   *
   * @param[in] addr A <code>PP_Resource</code> corresponding to a network
   * address.
   * @param[out] ipv4_addr A <code>PP_NetAddress_IPv4</code> structure to store
   * the result.
   *
   * @return A <code>PP_Bool</code> value indicating whether the operation
   * succeeded.
   */
  public static PPBool DescribeAsIPv4Address ( PPResource addr,
                                              out PPNetAddressIPv4 ipv4_addr)
  {
  	return _DescribeAsIPv4Address (addr, out ipv4_addr);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetAddress_DescribeAsIPv6Address")]
  extern static PPBool _DescribeAsIPv6Address (
      PPResource addr,
      out PPNetAddressIPv6 ipv6_addr);

  /**
   * Fills a <code>PP_NetAddress_IPv6</code> structure if the network address is
   * of <code>PP_NETADDRESS_FAMILY_IPV6</code> address family.
   * Note that passing a network address of
   * <code>PP_NETADDRESS_FAMILY_IPV4</code> address family will fail - this
   * method doesn't map it to an IPv6 address.
   *
   * @param[in] addr A <code>PP_Resource</code> corresponding to a network
   * address.
   * @param[out] ipv6_addr A <code>PP_NetAddress_IPv6</code> structure to store
   * the result.
   *
   * @return A <code>PP_Bool</code> value indicating whether the operation
   * succeeded.
   */
  public static PPBool DescribeAsIPv6Address ( PPResource addr,
                                              out PPNetAddressIPv6 ipv6_addr)
  {
  	return _DescribeAsIPv6Address (addr, out ipv6_addr);
  }


}
/**
 * @}
 */


}
