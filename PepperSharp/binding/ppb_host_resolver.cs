/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_host_resolver.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_HostResolver</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * <code>PP_HostResolver_Flag</code> is an enumeration of flags which can be
 * OR-ed and passed to the host resolver. Currently there is only one flag
 * defined.
 */
public enum PPHostResolverFlag {
  /**
   * Hint to request the canonical name of the host, which can be retrieved by
   * <code>GetCanonicalName()</code>.
   */
  Canonname = 1<<0
}
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * <code>PP_HostResolver_Hint</code> represents hints for host resolution.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPHostResolverHint {
  /**
   * Network address family.
   */
  public PPNetAddressFamily family;
  /**
   * Combination of flags from <code>PP_HostResolver_Flag</code>.
   */
  public int flags;
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_HostResolver</code> interface supports host name
 * resolution.
 *
 * Permissions: In order to run <code>Resolve()</code>, apps permission
 * <code>socket</code> with subrule <code>resolve-host</code> is required.
 * For more details about network communication permissions, please see:
 * http://developer.chrome.com/apps/app_network.html
 */
internal static partial class PPBHostResolver {
  [DllImport("PepperPlugin", EntryPoint = "PPB_HostResolver_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a host resolver resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance of
   * a module.
   *
   * @return A <code>PP_Resource</code> corresponding to a host reslover or 0
   * on failure.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_HostResolver_IsHostResolver")]
  extern static PPBool _IsHostResolver ( PPResource resource);

  /**
   * Determines if a given resource is a host resolver.
   *
   * @param[in] resource A <code>PP_Resource</code> to check.
   *
   * @return <code>PP_TRUE</code> if the input is a
   * <code>PPB_HostResolver</code> resource; <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsHostResolver ( PPResource resource)
  {
  	return _IsHostResolver (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_HostResolver_Resolve")]
  extern static int _Resolve ( PPResource host_resolver,
                              IntPtr host,
                               ushort port,
                               PPHostResolverHint hint,
                               PPCompletionCallback callback);

  /**
   * Requests resolution of a host name. If the call completes successfully, the
   * results can be retrieved by <code>GetCanonicalName()</code>,
   * <code>GetNetAddressCount()</code> and <code>GetNetAddress()</code>.
   *
   * @param[in] host_resolver A <code>PP_Resource</code> corresponding to a host
   * resolver.
   * @param[in] host The host name (or IP address literal) to resolve.
   * @param[in] port The port number to be set in the resulting network
   * addresses.
   * @param[in] hint A <code>PP_HostResolver_Hint</code> structure providing
   * hints for host resolution.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * <code>PP_ERROR_NOACCESS</code> will be returned if the caller doesn't have
   * required permissions. <code>PP_ERROR_NAME_NOT_RESOLVED</code> will be
   * returned if the host name couldn't be resolved.
   */
  public static int Resolve ( PPResource host_resolver,
                             byte[] host,
                              ushort port,
                              PPHostResolverHint hint,
                              PPCompletionCallback callback)
  {
  	if (host == null)
  		throw new ArgumentNullException ("host");

  	unsafe
  	{
  		fixed (byte* host_ = &host[0])
  		{
  			return _Resolve (host_resolver, (IntPtr) host_, port, hint, callback);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_HostResolver_GetCanonicalName")]
  extern static PPVar _GetCanonicalName ( PPResource host_resolver);

  /**
   * Gets the canonical name of the host.
   *
   * @param[in] host_resolver A <code>PP_Resource</code> corresponding to a host
   * resolver.
   *
   * @return A string <code>PP_Var</code> on success, which is an empty string
   * if <code>PP_HOSTRESOLVER_FLAG_CANONNAME</code> is not set in the hint flags
   * when calling <code>Resolve()</code>; an undefined <code>PP_Var</code> if
   * there is a pending <code>Resolve()</code> call or the previous
   * <code>Resolve()</code> call failed.
   */
  public static PPVar GetCanonicalName ( PPResource host_resolver)
  {
  	return _GetCanonicalName (host_resolver);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_HostResolver_GetNetAddressCount")]
  extern static uint _GetNetAddressCount ( PPResource host_resolver);

  /**
   * Gets the number of network addresses.
   *
   * @param[in] host_resolver A <code>PP_Resource</code> corresponding to a host
   * resolver.
   *
   * @return The number of available network addresses on success; 0 if there is
   * a pending <code>Resolve()</code> call or the previous
   * <code>Resolve()</code> call failed.
   */
  public static uint GetNetAddressCount ( PPResource host_resolver)
  {
  	return _GetNetAddressCount (host_resolver);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_HostResolver_GetNetAddress")]
  extern static PPResource _GetNetAddress ( PPResource host_resolver,
                                            uint index);

  /**
   * Gets a network address.
   *
   * @param[in] host_resolver A <code>PP_Resource</code> corresponding to a host
   * resolver.
   * @param[in] index An index indicating which address to return.
   *
   * @return A <code>PPB_NetAddress</code> resource on success; 0 if there is a
   * pending <code>Resolve()</code> call or the previous <code>Resolve()</code>
   * call failed, or the specified index is out of range.
   */
  public static PPResource GetNetAddress ( PPResource host_resolver,
                                           uint index)
  {
  	return _GetNetAddress (host_resolver, index);
  }


}
/**
 * @}
 */


}
