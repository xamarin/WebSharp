/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_network_proxy.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_NetworkProxy</code> interface.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * This interface provides a way to determine the appropriate proxy settings
 * for a given URL.
 *
 * Permissions: Apps permission <code>socket</code> with subrule
 * <code>resolve-proxy</code> is required for using this API.
 * For more details about network communication permissions, please see:
 * http://developer.chrome.com/apps/app_network.html
 */
public static partial class PPBNetworkProxy {
  [DllImport("PepperPlugin", EntryPoint = "PPB_NetworkProxy_GetProxyForURL")]
  extern static int _GetProxyForURL ( PPInstance instance,
                                      PPVar url,
                                     out PPVar proxy_string,
                                      PPCompletionCallback callback);

  /**
   * Retrieves the proxy that will be used for the given URL. The result will
   * be a string in PAC format. For more details about PAC format, please see
   * http://en.wikipedia.org/wiki/Proxy_auto-config
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   *
   * @param[in] url A string <code>PP_Var</code> containing a URL.
   *
   * @param[out] proxy_string A <code>PP_Var</code> that GetProxyForURL will
   * set upon successful completion. If the call fails, <code>proxy_string
   * </code> will be unchanged. Otherwise, it will be set to a string <code>
   * PP_Var</code> containing the appropriate PAC string for <code>url</code>.
   * If set, <code>proxy_string</code> will have a reference count of 1 which
   * the plugin must manage.
   *
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int GetProxyForURL ( PPInstance instance,
                                     PPVar url,
                                    out PPVar proxy_string,
                                     PPCompletionCallback callback)
  {
  	return _GetProxyForURL (instance, url, out proxy_string, callback);
  }


}
/**
 * @}
 */


}
