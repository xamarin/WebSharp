using System;

namespace PepperSharp
{
    public class URLResponseInfo : Resource
    {
        internal URLResponseInfo(PPResource resource) : base(PassRef.PassRef, resource)
        { }

        /// <summary>
        /// This function gets a response property.
        /// </summary>
        /// <param name="property">A <code>URLResponseProperty</code> identifying the
        /// type of property in the response.</param>
        /// <returns>An object representing the property being asked for</returns>
        public object GetProperty(URLResponseProperty property)
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)property)).AsObject();

        /// <summary>
        /// Gets a <code>FileRef</code> pointing to the file containing the response body.  This
        /// is only valid if <code>URLRequestProperty.StreamToFile</code> was set
        /// on the <code>URLRequestInfo</code> used to produce this response.  This
        /// file remains valid until the <code>URLLoader</code> associated with this
        /// <code>URLResponseInfo</code> is closed or destroyed.
        /// </summary>
        public FileRef GetBodyAsFileRef
            => new FileRef(PPBURLResponseInfo.GetBodyAsFileRef(this));

        /// <summary>
        /// This corresponds to a string; an absolute URL formed by
        /// resolving the relative request URL with the absolute document URL.Refer
        /// to the
        /// <a href = "http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html#sec5.1.2" >
        /// HTTP Request URI</a> and
        /// <a href = "http://www.w3.org/TR/html4/struct/links.html#h-12.4.1" >
        /// HTML Resolving Relative URIs</a> documentation for further information. 
        /// </summary>
        public string Url
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)URLResponseProperty.Url)).AsString();

        /// <summary>
        /// This corresponds to a string; the absolute URL returned
        /// in the response header's 'Location' field if this is a redirect response,
        /// an empty string otherwise. Refer to the
        /// <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3">
        /// HTTP Status Codes - Redirection</a> documentation for further information.
        /// </summary>
        public string RedirectUrl
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)URLResponseProperty.RedirectUrl)).AsString();

        /// <summary>
        /// This corresponds to a string; the HTTP method to be
        /// used in a new request if this is a redirect response, an empty string
        /// otherwise.Refer to the
        /// <a href = "http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3" >
        /// HTTP Status Codes - Redirection</a> documentation for further information.
        /// </summary>
        public string RedirectMethod
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)URLResponseProperty.RedirectMethod)).AsString();

        /// <summary>
        /// This corresponds to an int; the status code from the
        /// response, e.g., 200 if the request was successful.Refer to the
        /// <a href = "http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1.1" >
        /// HTTP Status Code and Reason Phrase</a> documentation for further
        /// information.
        /// </summary>
        public int StatusCode
            => (int)GetProperty(URLResponseProperty.StatusCode);

        /// <summary>
        /// This corresponds to a string; the status line
        /// from the response.Refer to the
        /// <a href = "http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1" >
        /// HTTP Response Status Line</a> documentation for further information.
        /// </summary>
        public string StatusLine
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)URLResponseProperty.StatusLine)).AsString();

        /// <summary>
        /// This corresponds to a string, a \n-delimited list of
        /// header field/value pairs of the form "field: value", returned by the
        /// server.Refer to the
        /// <a href = "http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14" >
        /// HTTP Header Field Definitions</a> documentation for further information.
        /// </summary>
        public string Headers
            => ((Var)PPBURLResponseInfo.GetProperty(this, (PPURLResponseProperty)URLResponseProperty.Headers)).AsString();

        public override string ToString()
        {
            return $"Url - {Url}\nRedirect Url - {RedirectUrl}\nRedirect Method - {RedirectMethod}\nStatus Code - {StatusCode}\nStatus Line - {StatusLine}\nHeaders - {Headers}";
        }

    }

    /**
    * This enumeration contains properties set on a URL response.
    */
    public enum URLResponseProperty
    {
        /**
         * This corresponds to a string; an absolute URL formed by
         * resolving the relative request URL with the absolute document URL. Refer
         * to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html#sec5.1.2">
         * HTTP Request URI</a> and
         * <a href="http://www.w3.org/TR/html4/struct/links.html#h-12.4.1">
         * HTML Resolving Relative URIs</a> documentation for further information.
         */
        Url = 0,
        /**
         * This corresponds to a string; the absolute URL returned
         * in the response header's 'Location' field if this is a redirect response,
         * an empty string otherwise. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3">
         * HTTP Status Codes - Redirection</a> documentation for further information.
         */
        RedirectUrl = 1,
        /**
         * This corresponds to a string; the HTTP method to be
         * used in a new request if this is a redirect response, an empty string
         * otherwise. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3">
         * HTTP Status Codes - Redirection</a> documentation for further information.
         */
        RedirectMethod = 2,
        /**
         * This corresponds to an int; the status code from the
         * response, e.g., 200 if the request was successful. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1.1">
         * HTTP Status Code and Reason Phrase</a> documentation for further
         * information.
         */
        StatusCode = 3,
        /**
         * This corresponds to a string; the status line
         * from the response. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html#sec6.1">
         * HTTP Response Status Line</a> documentation for further information.
         */
        StatusLine = 4,
        /**
         * This corresponds to a string, a \n-delimited list of
         * header field/value pairs of the form "field: value", returned by the
         * server. Refer to the
         * <a href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14">
         * HTTP Header Field Definitions</a> documentation for further information.
         */
        Headers = 5
    }
}
