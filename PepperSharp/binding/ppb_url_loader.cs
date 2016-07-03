/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_url_loader.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <strong>PPB_URLLoader</strong> interface for loading
 * URLs.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <strong>PPB_URLLoader</strong> interface contains pointers to functions
 * for loading URLs. The typical steps for loading a URL are:
 *
 * -# Call Create() to create a URLLoader object.
 * -# Create a <code>URLRequestInfo</code> object and set properties on it.
 * Refer to <code>PPB_URLRequestInfo</code> for further information.
 * -# Call Open() with the <code>URLRequestInfo</code> as an argument.
 * -# When Open() completes, call GetResponseInfo() to examine the response
 * headers. Refer to <code>PPB_URLResponseInfo</code> for further information.
 * -# Call ReadResponseBody() to stream the data for the response.
 *
 * Alternatively, if <code>PP_URLREQUESTPROPERTY_STREAMTOFILE</code> was set on
 * the <code>URLRequestInfo</code> in step #2:
 * - Call FinishStreamingToFile(), after examining the response headers
 * (step #4),  to wait for the downloaded file to be complete.
 * - Then, access the downloaded file using the GetBodyAsFileRef() function of
 * the <code>URLResponseInfo</code> returned in step #4.
 */
public static partial class PPBURLLoader {
  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Create() creates a new <code>URLLoader</code> object. The
   * <code>URLLoader</code> is associated with a particular instance, so that
   * any UI dialogs that need to be shown to the user can be positioned
   * relative to the window containing the instance.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   *
   * @return A <code>PP_Resource</code> corresponding to a URLLoader if
   * successful, 0 if the instance is invalid.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_IsURLLoader")]
  extern static PPBool _IsURLLoader ( PPResource resource);

  /**
   * IsURLLoader() determines if a resource is an <code>URLLoader</code>.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   *
   * @return <code>PP_TRUE</code> if the resource is a <code>URLLoader</code>,
   * <code>PP_FALSE</code> if the resource is invalid or some type other
   * than <code>URLLoader</code>.
   */
  public static PPBool IsURLLoader ( PPResource resource)
  {
  	return _IsURLLoader (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_Open")]
  extern static int _Open ( PPResource loader,
                            PPResource request_info,
                            PPCompletionCallback callback);

  /**
   * Open() begins loading the <code>URLRequestInfo</code>. The operation
   * completes when response headers are received or when an error occurs.  Use
   * GetResponseInfo() to access the response headers.
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * <code>URLRequestInfo</code>.
   * @param[in] callback A <code>PP_CompletionCallback</code> to run on
   * asynchronous completion of Open(). This callback will run when response
   * headers for the url are received or error occurred. This callback
   * will only run if Open() returns <code>PP_OK_COMPLETIONPENDING</code>.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Open ( PPResource loader,
                           PPResource request_info,
                           PPCompletionCallback callback)
  {
  	return _Open (loader, request_info, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_FollowRedirect")]
  extern static int _FollowRedirect ( PPResource loader,
                                      PPCompletionCallback callback);

  /**
   * FollowRedirect() can be invoked to follow a redirect after Open()
   * completed on receiving redirect headers.
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in] callback A <code>PP_CompletionCallback</code> to run on
   * asynchronous completion of FollowRedirect(). This callback will run when
   * response headers for the redirect url are received or error occurred. This
   * callback will only run if FollowRedirect() returns
   * <code>PP_OK_COMPLETIONPENDING</code>.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int FollowRedirect ( PPResource loader,
                                     PPCompletionCallback callback)
  {
  	return _FollowRedirect (loader, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_GetUploadProgress")]
  extern static PPBool _GetUploadProgress ( PPResource loader,
                                           out long bytes_sent,
                                           out long total_bytes_to_be_sent);

  /**
   * GetUploadProgress() returns the current upload progress (which is
   * meaningful after Open() has been called). Progress only refers to the
   * request body and does not include the headers.
   *
   * This data is only available if the <code>URLRequestInfo</code> passed
   * to Open() had the <code>PP_URLREQUESTPROPERTY_REPORTUPLOADPROGRESS</code>
   * property set to PP_TRUE.
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in] bytes_sent The number of bytes sent thus far.
   * @param[in] total_bytes_to_be_sent The total number of bytes to be sent.
   *
   * @return <code>PP_TRUE</code> if the upload progress is available,
   * <code>PP_FALSE</code> if it is not available.
   */
  public static PPBool GetUploadProgress ( PPResource loader,
                                          out long bytes_sent,
                                          out long total_bytes_to_be_sent)
  {
  	return _GetUploadProgress (loader,
                              out bytes_sent,
                              out total_bytes_to_be_sent);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_GetDownloadProgress")]
  extern static PPBool _GetDownloadProgress (
      PPResource loader,
      out long bytes_received,
      out long total_bytes_to_be_received);

  /**
   * GetDownloadProgress() returns the current download progress, which is
   * meaningful after Open() has been called. Progress only refers to the
   * response body and does not include the headers.
   *
   * This data is only available if the <code>URLRequestInfo</code> passed to
   * Open() had the <code>PP_URLREQUESTPROPERTY_REPORTDOWNLOADPROGRESS</code>
   * property set to <code>PP_TRUE</code>.
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in] bytes_received The number of bytes received thus far.
   * @param[in] total_bytes_to_be_received The total number of bytes to be
   * received. The total bytes to be received may be unknown, in which case
   * <code>total_bytes_to_be_received</code> will be set to -1.
   *
   * @return <code>PP_TRUE</code> if the download progress is available,
   * <code>PP_FALSE</code> if it is not available.
   */
  public static PPBool GetDownloadProgress (
      PPResource loader,
      out long bytes_received,
      out long total_bytes_to_be_received)
  {
  	return _GetDownloadProgress (loader,
                                out bytes_received,
                                out total_bytes_to_be_received);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_GetResponseInfo")]
  extern static PPResource _GetResponseInfo ( PPResource loader);

  /**
   * GetResponseInfo() returns the current <code>URLResponseInfo</code> object.
   *
   * @param[in] instance A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   *
   * @return A <code>PP_Resource</code> corresponding to the
   * <code>URLResponseInfo</code> if successful, 0 if the loader is not a valid
   * resource or if Open() has not been called.
   */
  public static PPResource GetResponseInfo ( PPResource loader)
  {
  	return _GetResponseInfo (loader);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_ReadResponseBody")]
  extern static int _ReadResponseBody ( PPResource loader,
                                       IntPtr buffer,
                                        int bytes_to_read,
                                        PPCompletionCallback callback);

  /**
   * ReadResponseBody() is used to read the response body. The size of the
   * buffer must be large enough to hold the specified number of bytes to read.
   * This function might perform a partial read.
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in,out] buffer A pointer to the buffer for the response body.
   * @param[in] bytes_to_read The number of bytes to read.
   * @param[in] callback A <code>PP_CompletionCallback</code> to run on
   * asynchronous completion. The callback will run if the bytes (full or
   * partial) are read or an error occurs asynchronously. This callback will
   * run only if this function returns <code>PP_OK_COMPLETIONPENDING</code>.
   *
   * @return An int32_t containing the number of bytes read or an error code
   * from <code>pp_errors.h</code>.
   */
  public static int ReadResponseBody ( PPResource loader,
                                      byte[] buffer,
                                       int bytes_to_read,
                                       PPCompletionCallback callback)
  {
  	unsafe
  	{
  		fixed (byte* buffer_ = &buffer[0])
  		{
  			return _ReadResponseBody (loader, (IntPtr) buffer_,
                                        bytes_to_read,
                                        callback);
  		}
  	}
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_URLLoader_FinishStreamingToFile")]
  extern static int _FinishStreamingToFile ( PPResource loader,
                                             PPCompletionCallback callback);

  /**
   * FinishStreamingToFile() is used to wait for the response body to be
   * completely downloaded to the file provided by the GetBodyAsFileRef()
   * in the current <code>URLResponseInfo</code>. This function is only used if
   * <code>PP_URLREQUESTPROPERTY_STREAMTOFILE</code> was set on the
   * <code>URLRequestInfo</code> passed to Open().
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   * @param[in] callback A <code>PP_CompletionCallback</code> to run on
   * asynchronous completion. This callback will run when body is downloaded
   * or an error occurs after FinishStreamingToFile() returns
   * <code>PP_OK_COMPLETIONPENDING</code>.
   *
   * @return An int32_t containing the number of bytes read or an error code
   * from <code>pp_errors.h</code>.
   */
  public static int FinishStreamingToFile ( PPResource loader,
                                            PPCompletionCallback callback)
  {
  	return _FinishStreamingToFile (loader, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLLoader_Close")]
  extern static void _Close ( PPResource loader);

  /**
   * Close is a pointer to a function used to cancel any pending IO and close
   * the <code>URLLoader</code> object. Any pending callbacks will still run,
   * reporting <code>PP_ERROR_ABORTED</code> if pending IO was interrupted.
   * It is NOT valid to call Open() again after a call to this function.
   *
   * <strong>Note:</strong> If the <code>URLLoader</code> object is destroyed
   * while it is still open, then it will be implicitly closed so you are not
   * required to call Close().
   *
   * @param[in] loader A <code>PP_Resource</code> corresponding to a
   * <code>URLLoader</code>.
   */
  public static void Close ( PPResource loader)
  {
  	 _Close (loader);
  }


}
/**
 * @}
 */


}
