namespace WebSharpJs.DOM
{
    using System.Net;

    public static class HttpUtility
    {
        #region HtmlEncode and HtmlDecode methods

        public static string HtmlEncode(string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        public static string HtmlDecode(string value)
        {
            return WebUtility.HtmlDecode(value);
        }

        #endregion

        #region UrlEncode and UrlDecode public methods

        public static string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(value);
        }

        public static string UrlDecode(string encodedValue)
        {
            return WebUtility.UrlDecode(encodedValue);
        }

        #endregion


    }
}
