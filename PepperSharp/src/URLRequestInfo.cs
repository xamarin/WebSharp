using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class URLRequestInfo : Resource
    {
        /// <summary>
        /// Constructs a URLRequestInfo object.
        /// </summary>
        /// <param name="instance">The instance with which this resource will be
        /// associated.</param>
        public URLRequestInfo(Instance instance)
        {
            handle = PPBURLRequestInfo.Create(instance);
        }

        /// <summary>
        /// SetProperty() sets a request property. The value of the property must be
        /// the correct type according to the property being set.
        /// </summary>
        /// <param name="property">A <code>URLRequestProperty</code> identifying the
        /// property to set.</param>
        /// <param name="value">An <code>Object</code> containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetProperty(URLRequestProperty property, object value)
            => PPBURLRequestInfo.SetProperty(this, (PPURLRequestProperty)property, new Var(value)) == PPBool.True;

        /// <summary>
        /// AppendDataToBody() appends data to the request body. A content-length
        /// request header will be automatically generated.
        /// </summary>
        /// <param name="data">A byte array buffer holding the data.</param>
        /// <param name="len">The length, in bytes, of the data.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool AppendDataToBody(byte[] data, uint len)
            => PPBURLRequestInfo.AppendDataToBody(this, data, len) == PPBool.True;

        /// <summary>
        /// AppendFileToBody() is used to append an entire file, to be uploaded, to
        /// the request body. A content-length request header will be automatically
        /// generated. 
        /// </summary>
        /// <param name="fileRef">A <code>FileRef</code> containing the file
        /// reference.</param>
        /// <param name="utcExpectedLastModifiedTime">An optional last
        /// modified time stamp used to validate that the file was not modified since
        /// the given time before it was uploaded. The upload will fail with an error
        /// code of <code>ErrorFilechanged</code> if the file has been modified
        /// since the given time. If utcExpectedLastModifiedTime is null, then no
        /// validation is performed.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool AppendFileToBody(FileRef fileRef,
                        DateTime? utcExpectedLastModifiedTime = null)
            => AppendFileRangeToBody(fileRef, 0, -1, utcExpectedLastModifiedTime);

        /// <summary>
        /// AppendFileRangeToBody() is used to append part or
        /// all of a file, to be uploaded, to the request body. A content-length
        /// request header will be automatically generated.
        /// </summary>
        /// <param name="fileRef">A <code>FileRef</code> containing the file
        /// reference.</param>
        /// <param name="startOffset">An optional starting point offset within the
        /// file.</param>
        /// <param name="length">An optional number of bytes of the file to
        /// be included. If the value is -1, then the sub-range to upload extends
        /// to the end of the file.</param>
        /// <param name="utcExpectedLastModifiedTime">An optional last
        /// modified time stamp used to validate that the file was not modified since
        /// the given time before it was uploaded. The upload will fail with an error
        /// code of <code>ErrorFilechanged</code> if the file has been modified
        /// since the given time. If utcExpectedLastModifiedTime is null, then no
        /// validation is performed.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool AppendFileRangeToBody(FileRef fileRef,
                             long startOffset,
                             long length,
                             DateTime? utcExpectedLastModifiedTime = null)
            => PPBURLRequestInfo.AppendFileToBody(this,
                fileRef,
                startOffset,
                length,
                (utcExpectedLastModifiedTime.HasValue) ? PepperSharpUtils.ConvertToPepperTimestamp(utcExpectedLastModifiedTime.Value) : 0) == PPBool.True;

        /// <summary>
        /// SetURL() sets the <code>URLRequestProperty.Url</code>
        /// property for the request.
        /// </summary>
        /// <param name="urlString">A string containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetURL(string urlString)
            => SetProperty(URLRequestProperty.Url, urlString);

        /// <summary>
        /// SetMethod() sets the <code>URLRequestProperty.Headers</code>
        /// property for the request. This string is either a POST or GET. Refer to
        /// the <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html">HTTP
        /// Methods</a> documentation for further information.
        /// </summary>
        /// <param name="methodString">A string containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetMethod(string methodString)
            => SetProperty(URLRequestProperty.Method, methodString);

        /// <summary>
        /// SetHeaders() sets the <code>PP_URLREQUESTPROPERTY_HEADERS</code>
        /// (corresponding to a <code>\n</code> delimited string of type
        /// <code>PP_VARTYPE_STRING</code>) property for the request.
        /// Refer to the
        /// <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html"Header
        /// Field Definitions</a> documentation for further information.
        /// </summary>
        /// <param name="headersString">A string containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetHeaders(string headersString)
            => SetProperty(URLRequestProperty.Headers, headersString);

        /// <summary>
        /// SetStreamToFile() sets the
        /// <code>URLRequestProperty.StreamToFile</code>. The default of the
        /// property is false. Set this value to true if you want to download the
        /// data to a file. Use URLLoader.FinishStreamingToFile() to complete
        /// the download.
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetStreamToFile(bool enable)
            => SetProperty(URLRequestProperty.StreamToFile, enable);

        /// <summary>
        /// SetFollowRedirects() sets the
        /// <code>URLRequestProperty.FollowRedirects</code>. The default of the
        /// property is true. Set this value to false if you want to use
        /// URLLoader.FollowRedirects() to follow the redirects only after examining
        /// redirect headers.
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetFollowRedirects(bool enable)
            => SetProperty(URLRequestProperty.FollowRedirects, enable);

        /// <summary>
        /// SetRecordDownloadProgress() sets the
        /// <code>URLRequestProperty.RecordDownloadProgress</code>. The
        /// default of the property is false. Set this value to true if you want to
        /// be able to poll the download progress using
        /// URLLoader.GetDownloadProgress().
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetRecordDownloadProgress(bool enable)
            => SetProperty(URLRequestProperty.RecordDownloadProgress, enable);

        /// <summary>
        /// SetRecordUploadProgress() sets the
        /// <code>URLRequestProperty.RecordUploadProgress</code>. The
        /// default of the property is false. Set this value to true if you want to
        /// be able to poll the upload progress using URLLoader.GetUploadProgress().
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetRecordUploadProgress(bool enable)
            => SetProperty(URLRequestProperty.RecordUploadProgress, enable);

        /// <summary>
        /// SetCustomReferrerURL() sets the
        /// <code>URLRequestProperty.CustomReferrerUrl</code>
        /// (corresponding to a string or might be set to null). Set it
        /// to a string to set a custom referrer (if empty, the referrer header will
        /// be omitted), or to null to use the default referrer. Only loaders
        /// with universal access (only available on trusted implementations) will
        /// accept <code>URLRequestInfo</code> objects that try to set a custom
        /// referrer; if given to a loader without universal access,
        /// <code>BadArgument</code> will result.
        /// </summary>
        /// <param name="url">A string or null containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetCustomReferrerURL(string url)
            => SetProperty(URLRequestProperty.CustomReferrerUrl, url);

        /// <summary>
        /// SetAllowCrossOriginRequests() sets the
        /// <code>URLRequestProperty.AllowCrossOriginRequests</code>. The
        /// default of the property is false. Whether cross-origin requests are
        /// allowed. Cross-origin requests are made using the CORS (Cross-Origin
        /// Resource Sharing) algorithm to check whether the request should be
        /// allowed. For the complete CORS algorithm, refer to the
        /// <a href="http://www.w3.org/TR/access-control">Cross-Origin Resource
        /// Sharing</a> documentation. 
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetAllowCrossOriginRequests(bool enable)
            => SetProperty(URLRequestProperty.AllowCrossOriginRequests, enable);

        /// <summary>
        /// SetAllowCredentials() sets the
        /// <code>URLRequestProperty.AllowCredentials</code>. The
        /// default of the property is false. Whether HTTP credentials are sent with
        /// cross-origin requests. If false, no credentials are sent with the request
        /// and cookies are ignored in the response. If the request is not
        /// cross-origin, this property is ignored.
        /// </summary>
        /// <param name="enable">A boolean containing the property value</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetAllowCredentials(bool enable)
            => SetProperty(URLRequestProperty.AllowCredentials, enable);

        /// <summary>
        /// SetContentTransferEncoding() sets the
        /// <code>URLRequestProperty.CustomContentTransferEncoding</code>
        /// (corresponding to a string or might be set to null). Set it
        /// to a string to set a custom content transfer encoding (if empty, the header will
        /// be omitted), or to null to use the default if any. Only loaders
        /// with universal access (only available on trusted implementations) will
        /// accept <code>URLRequestInfo</code> objects that try to set a custom
        /// content transfer encoding; if given to a loader without universal access,
        /// <code>BadArgument</code> will result.
        /// </summary>
        /// <param name="contentTransferEncoding">A string or null containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetContentTransferEncoding(string contentTransferEncoding)
            => SetProperty(URLRequestProperty.CustomContentTransferEncoding, contentTransferEncoding);

        /// <summary>
        /// SetPrefetchBufferUpperThreshold() sets the
        /// <code>URLRequestProperty.PrefetchBufferUpperThreshold</code>. The
        /// default is not defined and is set by the browser possibly depending on
        /// system capabilities. Set it to an integer to set an upper threshold for
        /// the prefetched buffer of an asynchronous load. When exceeded, the browser
        /// will defer loading until
        /// <code>URLRequestProperty.PrefetchBufferLowerThreshold</code> is hit,
        /// at which time it will begin prefetching again. When setting this
        /// property,
        /// <code>URLRequestProperty.PrefetchBufferLowerThreshold</code> must
        /// also be set. Behavior is undefined if the former is less than or equal to the latter.
        /// </summary>
        /// <param name="size">An int containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetPrefetchBufferUpperThreshold(int size)
            => SetProperty(URLRequestProperty.PrefetchBufferUpperThreshold, size);

        /// <summary>
        /// SetPrefetchBufferLowerThreshold() sets the
        /// <code>URLRequestProperty.PrefetchBufferLowerThreshold</code>. The
        /// default is not defined and is set by the browser to a value appropriate
        /// for the default
        /// <code>URLRequestProperty.PrefetchBufferUpperThreshold</code>.
        /// Set it to an integer to set a lower threshold for the prefetched buffer
        /// of an asynchronous load. When reached, the browser will resume loading if
        /// If <code>URLRequestProperty.PrefetchBufferLowerThreshold</code> had
        /// previously been reached.
        /// When setting this property,
        /// <code>URLRequestProperty.PrefetchBufferUpperThreshold</code> must also
        /// be set. Behavior is undefined if the former is greater than or equal to the latter.
        /// </summary>
        /// <param name="size">An int containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetPrefetchBufferLowerThreshold(int size)
            => SetProperty(URLRequestProperty.PrefetchBufferLowerThreshold, size);

        /// <summary>
        /// SetContentTransferEncoding() sets the
        /// <code>URLRequestProperty.CustomUserAgent</code>
        /// (corresponding to a string or might be set to null). Set it
        /// to a string to set a custom user agent header (if empty, the header will
        /// be omitted), or to null to use the default if any. Only loaders
        /// with universal access (only available on trusted implementations) will
        /// accept <code>URLRequestInfo</code> objects that try to set a custom
        /// user agent; if given to a loader without universal access,
        /// <code>BadArgument</code> will result.
        /// </summary>
        /// <param name="userAgent">A string or null containing the property value.</param>
        /// <returns>true if successful, false if any of the parameters are invalid.</returns>
        public bool SetCustomUserAgent(string userAgent)
            => SetProperty(URLRequestProperty.CustomUserAgent, userAgent);

    }


    public enum URLRequestProperty
    {
        /** This corresponds to a string (<code>PP_VARTYPE_STRING</code>). */
        Url = 0,
        /**
         * This corresponds to a string (<code>PP_VARTYPE_STRING</code>); either
         * POST or GET. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html">HTTP
         * Methods</a> documentation for further information.
         *
         */
        Method = 1,
        /**
         * This corresponds to a string (<code>PP_VARTYPE_STRING</code>); \n
         * delimited. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html"Header
         * Field Definitions</a> documentation for further information.
         */
        Headers = 2,
        /**
         * This corresponds to a <code>PP_Bool</code> (<code>PP_VARTYPE_BOOL</code>;
         * default=<code>PP_FALSE</code>).
         * Set this value to <code>PP_TRUE</code> if you want to download the data
         * to a file. Use PPB_URLLoader.FinishStreamingToFile() to complete the
         * download.
         */
        StreamToFile = 3,
        /**
         * This corresponds to a <code>PP_Bool</code> (<code>PP_VARTYPE_BOOL</code>;
         * default=<code>PP_TRUE</code>).
         * Set this value to <code>PP_FALSE</code> if you want to use
         * PPB_URLLoader.FollowRedirects() to follow the redirects only after
         * examining redirect headers.
         */
        FollowRedirects = 4,
        /**
         * This corresponds to a <code>PP_Bool</code> (<code>PP_VARTYPE_BOOL</code>;
         * default=<code>PP_FALSE</code>).
         * Set this value to <code>PP_TRUE</code> if you want to be able to poll the
         * download progress using PPB_URLLoader.GetDownloadProgress().
         */
        RecordDownloadProgress = 5,
        /**
         * This corresponds to a <code>PP_Bool</code>
         * (default=<code>PP_FALSE</code>). Set this value to <code>PP_TRUE</code> if
         * you want to be able to poll the upload progress using
         * PPB_URLLoader.GetUploadProgress().
         */
        RecordUploadProgress = 6,
        /**
         * This corresponds to a string (<code>PP_VARTYPE_STRING)</code> or may be
         * undefined (<code>PP_VARTYPE_UNDEFINED</code>; default).
         * Set it to a string to set a custom referrer (if empty, the referrer header
         * will be omitted), or to undefined to use the default referrer. Only loaders
         * with universal access (only available on trusted implementations) will
         * accept <code>URLRequestInfo</code> objects that try to set a custom
         * referrer; if given to a loader without universal access,
         * <code>PP_ERROR_NOACCESS</code> will result.
         */
        CustomReferrerUrl = 7,
        /**
         * This corresponds to a <code>PP_Bool</code> (<code>PP_VARTYPE_BOOL</code>;
         * default=<code>PP_FALSE</code>). Whether cross-origin requests are allowed.
         * Cross-origin requests are made using the CORS (Cross-Origin Resource
         * Sharing) algorithm to check whether the request should be allowed. For the
         * complete CORS algorithm, refer to
         * the <a href="http://www.w3.org/TR/access-control">Cross-Origin Resource
         * Sharing</a> documentation.
         */
        AllowCrossOriginRequests = 8,
        /**
         * This corresponds to a <code>PP_Bool</code> (<code>PP_VARTYPE_BOOL</code>;
         * default=<code>PP_FALSE</code>).
         * Whether HTTP credentials are sent with cross-origin requests. If false,
         * no credentials are sent with the request and cookies are ignored in the
         * response. If the request is not cross-origin, this property is ignored.
         */
        AllowCredentials = 9,
        /**
         * This corresponds to a string (<code>PP_VARTYPE_STRING</code>) or may be
         * undefined (<code>PP_VARTYPE_UNDEFINED</code>; default).
         * Set it to a string to set a custom content-transfer-encoding header (if
         * empty, that header will be omitted), or to undefined to use the default
         * (if any). Only loaders with universal access (only available on trusted
         * implementations) will accept <code>URLRequestInfo</code> objects that try
         * to set a custom content transfer encoding; if given to a loader without
         * universal access, <code>PP_ERROR_NOACCESS</code> will result.
         */
        CustomContentTransferEncoding = 10,
        /**
         * This corresponds to an integer (<code>PP_VARTYPE_INT32</code>); default
         * is not defined and is set by the browser, possibly depending on system
         * capabilities. Set it to an integer to set an upper threshold for the
         * prefetched buffer of an asynchronous load. When exceeded, the browser will
         * defer loading until
         * <code>PP_URLREQUESTPROPERTY_PREFETCHBUFFERLOWERERTHRESHOLD</code> is hit,
         * at which time it will begin prefetching again. When setting this property,
         * <code>PP_URLREQUESTPROPERTY_PREFETCHBUFFERLOWERERTHRESHOLD</code> must also
         * be set. Behavior is undefined if the former is <= the latter.
         */
        PrefetchBufferUpperThreshold = 11,
        /**
         * This corresponds to an integer (<code>PP_VARTYPE_INT32</code>); default is
         * not defined and is set by the browser to a value appropriate for the
         * default <code>PP_URLREQUESTPROPERTY_PREFETCHBUFFERUPPERTHRESHOLD</code>.
         * Set it to an integer to set a lower threshold for the prefetched buffer
         * of an asynchronous load. When reached, the browser will resume loading if
         * If <code>PP_URLREQUESTPROPERTY_PREFETCHBUFFERLOWERERTHRESHOLD</code> had
         * previously been reached.
         * When setting this property,
         * <code>PP_URLREQUESTPROPERTY_PREFETCHBUFFERUPPERTHRESHOLD</code> must also
         * be set. Behavior is undefined if the former is >= the latter.
         */
        PrefetchBufferLowerThreshold = 12,
        /**
         * This corresponds to a string (<code>PP_VARTYPE_STRING</code>) or may be
         * undefined (<code>PP_VARTYPE_UNDEFINED</code>; default). Set it to a string
         * to set a custom user-agent header (if empty, that header will be omitted),
         * or to undefined to use the default. Only loaders with universal access
         * (only available on trusted implementations) will accept
         * <code>URLRequestInfo</code> objects that try to set a custom user agent; if
         * given to a loader without universal access, <code>PP_ERROR_NOACCESS</code>
         * will result.
         */
        CustomUserAgent = 13
    }
}
