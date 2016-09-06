using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PepperSharp
{
    public static class NetworkProxy
    {

        /// <summary>
        /// Event raised when the NetworkProxy issues GetProxyForURL.
        /// </summary>
        public static EventHandler<ProxyInfo> HandleProxyForUrl;

        /// <summary>
        /// Event args class that contains the information returned from GetProxyForUrl
        /// </summary>
        public class ProxyInfo : EventArgs
        {
            public PPError Result { get; private set; }
            public string Proxy { get; private set; }

            public ProxyInfo(PPError result, string proxy)
            {
                Result = result;
                Proxy = proxy;
            }
        }

        /// <summary>
        /// Retrieves the proxy that will be used for the given URL. The result will
        /// be a string in PAC format. For more details about PAC format, please see
        /// http://en.wikipedia.org/wiki/Proxy_auto-config
        /// </summary>
        /// <param name="instance">An <code>InstanceHandle</code> identifying one
        /// instance of a module.</param>
        /// <param name="url">A string containing a URL.</param>
        /// <returns>Error code</returns>
        public static PPError GetProxyForURL(Instance instance, string url)
        {

            var cbwoAction = new Action<PPError, PPVar>(
                (result, proxy) =>
                {
                    HandleProxyForUrl?.Invoke(instance, new ProxyInfo(result, ((Var)proxy).AsString()));
                }

                );

            var cbwo = new CompletionCallbackWithOutput<PPVar>(new CompletionCallbackWithOutputFunc<PPVar>(cbwoAction));
            return (PPError)PPBNetworkProxy.GetProxyForURL(instance, new Var(url), out cbwo.OutputAdapter.output, cbwo);
        }

        /// <summary>
        /// Retrieves the proxy that will be used for the given URL asynchronously. The result will
        /// be a string in PAC format. For more details about PAC format, please see
        /// http://en.wikipedia.org/wiki/Proxy_auto-config
        /// </summary>
        /// <param name="instance">An <code>InstanceHandle</code> identifying one
        /// instance of a module.</param>
        /// <param name="url">A string containing a URL.</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code</returns>
        public static Task<ProxyInfo> GetProxyForURLAsync(Instance instance, string url, MessageLoop messageLoop = null)
            => GetProxyForURLAsyncCore(instance, url, messageLoop);

        private static async Task<ProxyInfo> GetProxyForURLAsyncCore(Instance instance, string url, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<ProxyInfo>();
            EventHandler<ProxyInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleProxyForUrl += handler;

                if (messageLoop == null)
                {
                    GetProxyForURL(instance, url);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new PPVar();
                        var presult = (PPError)PPBNetworkProxy.GetProxyForURL(instance, new Var(url), 
                            out output, new BlockUntilComplete() );

                        tcs.TrySetResult(new ProxyInfo(presult, ((Var)output).AsString()));
                    }
                    );
                    messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return new ProxyInfo(PPError.Aborted, string.Empty);
            }
            finally
            {
                HandleProxyForUrl -= handler;
            }
        }

    }
}
