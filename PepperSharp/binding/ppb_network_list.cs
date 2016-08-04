/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_network_list.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_NetworkList</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * Type of a network interface.
 */
public enum PPNetworkListType {
  /**
   * Type of the network interface is not known.
   */
  Unknown = 0,
  /**
   * Wired Ethernet network.
   */
  Ethernet = 1,
  /**
   * Wireless Wi-Fi network.
   */
  Wifi = 2,
  /**
   * Cellular network (e.g. LTE).
   */
  Cellular = 3
}

/**
 * State of a network interface.
 */
public enum PPNetworkListState {
  /**
   * Network interface is down.
   */
  Down = 0,
  /**
   * Network interface is up.
   */
  Up = 1
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_NetworkList</code> is used to represent a list of
 * network interfaces and their configuration. The content of the list
 * is immutable.  The current networks configuration can be received
 * using the <code>PPB_NetworkMonitor</code> interface.
 */
public static partial class PPBNetworkList {
  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_IsNetworkList")]
  extern static PPBool _IsNetworkList ( PPResource resource);

  /**
   * Determines if the specified <code>resource</code> is a
   * <code>NetworkList</code> object.
   *
   * @param[in] resource A <code>PP_Resource</code> resource.
   *
   * @return Returns <code>PP_TRUE</code> if <code>resource</code> is
   * a <code>PPB_NetworkList</code>, <code>PP_FALSE</code>
   * otherwise.
   */
  public static PPBool IsNetworkList ( PPResource resource)
  {
  	return _IsNetworkList (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetCount")]
  extern static uint _GetCount ( PPResource resource);

  /**
   * Gets number of interfaces in the list.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   *
   * @return Returns number of available network interfaces or 0 if
   * the list has never been updated.
   */
  public static uint GetCount ( PPResource resource)
  {
  	return _GetCount (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetName")]
  extern static PPVar _GetName ( PPResource resource,  uint index);

  /**
   * Gets name of a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   *
   * @return Returns name for the network interface with the specified
   * <code>index</code>.
   */
  public static PPVar GetName ( PPResource resource,  uint index)
  {
  	return _GetName (resource, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetType")]
  extern static PPNetworkListType _GetType ( PPResource resource,  uint index);

  /**
   * Gets type of a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   *
   * @return Returns type of the network interface with the specified
   * <code>index</code>.
   */
  public static PPNetworkListType GetType ( PPResource resource,  uint index)
  {
  	return _GetType (resource, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetState")]
  extern static PPNetworkListState _GetState ( PPResource resource,
                                               uint index);

  /**
   * Gets state of a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   *
   * @return Returns current state of the network interface with the
   * specified <code>index</code>.
   */
  public static PPNetworkListState GetState ( PPResource resource,  uint index)
  {
  	return _GetState (resource, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetIpAddresses")]
  extern static int _GetIpAddresses ( PPResource resource,
                                      uint index,
                                      PPArrayOutput output);

  /**
   * Gets list of IP addresses for a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   * @param[in] output An output array which will receive
   * <code>PPB_NetAddress</code> resources on success. Please note that the
   * ref count of those resources has already been increased by 1 for the
   * caller.
   *
   * @return An error code from <code>pp_errors.h</code>.
   */
  public static int GetIpAddresses ( PPResource resource,
                                     uint index,
                                     PPArrayOutput output)
  {
  	return _GetIpAddresses (resource, index, output);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetDisplayName")]
  extern static PPVar _GetDisplayName ( PPResource resource,  uint index);

  /**
   * Gets display name of a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   *
   * @return Returns display name for the network interface with the
   * specified <code>index</code>.
   */
  public static PPVar GetDisplayName ( PPResource resource,  uint index)
  {
  	return _GetDisplayName (resource, index);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkList_GetMTU")]
  extern static uint _GetMTU ( PPResource resource,  uint index);

  /**
   * Gets MTU (Maximum Transmission Unit) of a network interface.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * network list.
   * @param[in] index Index of the network interface.
   *
   * @return Returns MTU for the network interface with the specified
   * <code>index</code> or 0 if MTU is unknown.
   */
  public static uint GetMTU ( PPResource resource,  uint index)
  {
  	return _GetMTU (resource, index);
  }


}
/**
 * @}
 */


}
