/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_url_response_info.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_URLResponseInfo</code> API for examining URL
 * responses.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains properties set on a URL response.
 */
public enum PPURLResponseProperty {
  /**
   * This corresponds to a string (PP_VARTYPE_STRING); an absolute URL formed by
   * resolving the relative request URL with the absolute document URL. Refer
   * to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html#sec5.1.2">
   * HTTP Request URI</a> and
   * <a href="http://www.w3.org/TR/html4/struct/links.html#h-12.4.1">
   * HTML Resolving Relative URIs</a> documentation for further information.
   */
  Url = 0,
  /**
   * This corresponds to a string (PP_VARTYPE_STRING); the absolute URL returned
   * in the response header's 'Location' field if this is a redirect response,
   * an empty string otherwise. Refer to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3">
   * HTTP Status Codes - Redirection</a> documentation for further information.
   */
  Redirecturl = 1,
  /**
   * This corresponds to a string (PP_VARTYPE_STRING); the HTTP method to be
   * used in a new request if this is a redirect response, an empty string
   * otherwise. Refer to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3">
   * HTTP Status Codes - Redirection</a> documentation for further information.
   */
  Redirectmethod = 2,
  /**
   * This corresponds to an int32 (PP_VARETYPE_INT32); the status code from the
   * response, e.g., 200 if the request was successful. Refer to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1.1">
   * HTTP Status Code and Reason Phrase</a> documentation for further
   * information.
   */
  Statuscode = 3,
  /**
   * This corresponds to a string (PP_VARTYPE_STRING); the status line
   * from the response. Refer to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1">
   * HTTP Response Status Line</a> documentation for further information.
   */
  Statusline = 4,
  /**
   * This corresponds to a string(PP_VARTYPE_STRING), a \n-delimited list of
   * header field/value pairs of the form "field: value", returned by the
   * server. Refer to the
   * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14">
   * HTTP Header Field Definitions</a> documentation for further information.
   */
  Headers = 5
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The PPB_URLResponseInfo interface contains APIs for
 * examining URL responses. Refer to <code>PPB_URLLoader</code> for further
 * information.
 */
public static partial class PPBURLResponseInfo {
  [DllImport("PepperPlugin",
             EntryPoint = "PPB_URLResponseInfo_IsURLResponseInfo")]
  extern static PPBool _IsURLResponseInfo ( PPResource resource);

  /**
   * IsURLResponseInfo() determines if a response is a
   * <code>URLResponseInfo</code>.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a
   * <code>URLResponseInfo</code>.
   *
   * @return <code>PP_TRUE</code> if the resource is a
   * <code>URLResponseInfo</code>, <code>PP_FALSE</code> if the resource is
   * invalid or some type other than <code>URLResponseInfo</code>.
   */
  public static PPBool IsURLResponseInfo ( PPResource resource)
  {
  	return _IsURLResponseInfo (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_URLResponseInfo_GetProperty")]
  extern static PPVar _GetProperty ( PPResource response,
                                     PPURLResponseProperty property);

  /**
   * GetProperty() gets a response property.
   *
   * @param[in] request A <code>PP_Resource</code> corresponding to a
   * <code>URLResponseInfo</code>.
   * @param[in] property A <code>PP_URLResponseProperty</code> identifying
   * the type of property in the response.
   *
   * @return A <code>PP_Var</code> containing the response property value if
   * successful, <code>PP_VARTYPE_VOID</code> if an input parameter is invalid.
   */
  public static PPVar GetProperty ( PPResource response,
                                    PPURLResponseProperty property)
  {
  	return _GetProperty (response, property);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_URLResponseInfo_GetBodyAsFileRef")]
  extern static PPResource _GetBodyAsFileRef ( PPResource response);

  /**
   * GetBodyAsFileRef() returns a FileRef pointing to the file containing the
   * response body.  This is only valid if
   * <code>PP_URLREQUESTPROPERTY_STREAMTOFILE</code> was set on the
   * <code>URLRequestInfo</code> used to produce this response.  This file
   * remains valid until the <code>URLLoader</code> associated with this
   * <code>URLResponseInfo</code> is closed or destroyed.
   *
   * @param[in] request A <code>PP_Resource</code> corresponding to a
   * <code>URLResponseInfo</code>.
   *
   * @return A <code>PP_Resource</code> corresponding to a <code>FileRef</code>
   * if successful, 0 if <code>PP_URLREQUESTPROPERTY_STREAMTOFILE</code> was
   * not requested or if the <code>URLLoader</code> has not been opened yet.
   */
  public static PPResource GetBodyAsFileRef ( PPResource response)
  {
  	return _GetBodyAsFileRef (response);
  }


}
/**
 * @}
 */


}
