/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_network_monitor.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_NetworkMonitor</code> interface.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_NetworkMonitor</code> allows to get network interfaces
 * configuration and monitor network configuration changes.
 *
 * Permissions: Apps permission <code>socket</code> with subrule
 * <code>network-state</code> is required for <code>UpdateNetworkList()</code>.
 * For more details about network communication permissions, please see:
 * http://developer.chrome.com/apps/app_network.html
 */
public static partial class PPBNetworkMonitor {
  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkMonitor_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a Network Monitor resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   *
   * @return A <code>PP_Resource</code> corresponding to a network monitor or 0
   * on failure.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetworkMonitor_UpdateNetworkList")]
  extern static int _UpdateNetworkList ( PPResource network_monitor,
                                        out PPResource network_list,
                                         PPCompletionCallback callback);

  /**
   * Gets current network configuration. When called for the first time,
   * completes as soon as the current network configuration is received from
   * the browser. Each consequent call will wait for network list changes,
   * returning a new <code>PPB_NetworkList</code> resource every time.
   *
   * @param[in] network_monitor A <code>PP_Resource</code> corresponding to a
   * network monitor.
   * @param[out] network_list The <code>PPB_NetworkList<code> resource with the
   * current state of network interfaces.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * <code>PP_ERROR_NOACCESS</code> will be returned if the caller doesn't have
   * required permissions.
   */
  public static int UpdateNetworkList ( PPResource network_monitor,
                                       out PPResource network_list,
                                        PPCompletionCallback callback)
  {
  	return _UpdateNetworkList (network_monitor, out network_list, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_NetworkMonitor_IsNetworkMonitor")]
  extern static PPBool _IsNetworkMonitor ( PPResource resource);

  /**
   * Determines if the specified <code>resource</code> is a
   * <code>NetworkMonitor</code> object.
   *
   * @param[in] resource A <code>PP_Resource</code> resource.
   *
   * @return Returns <code>PP_TRUE</code> if <code>resource</code> is a
   * <code>PPB_NetworkMonitor</code>, <code>PP_FALSE</code>  otherwise.
   */
  public static PPBool IsNetworkMonitor ( PPResource resource)
  {
  	return _IsNetworkMonitor (resource);
  }


}
/**
 * @}
 */


}
