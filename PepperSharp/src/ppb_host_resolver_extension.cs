using System.Text;

namespace PepperSharp
{
    internal static partial class PPBHostResolver
    {
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
        public static int Resolve(PPResource host_resolver,
                                   string host,
                                    ushort port,
                                    PPHostResolverHint hint,
                                    PPCompletionCallback callback)
        {
            return Resolve(host_resolver, Encoding.UTF8.GetBytes(host), port, hint, callback);
        }
    }
}
