using System;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class NetworkMonitor : Resource
    {

        /// <summary>
        /// Event raised when the NetworkMonitor issues UpdateNetworkList.
        /// </summary>
        public EventHandler<NetworkListInfo> HandleUpdateNetworkList;

        public NetworkMonitor(Instance instance)
        {
            handle = PPBNetworkMonitor.Create(instance);
        }

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleUpdateNetworkList = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        /// <summary>
        /// Returns objects that describe the network interfaces.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError UpdateNetworkList()
        {
            Action<PPError, PPResource> callback = new Action<PPError, PPResource>(
                (result, resource) =>
                {
                    OnUpdateNetworkList(result, new NetworkList(resource));
                }
                );

            var onUpdateNetworkListCallback = new CompletionCallbackWithOutput<PPResource>(new CompletionCallbackWithOutputFunc<PPResource>(callback));
            return (PPError)PPBNetworkMonitor.UpdateNetworkList(this, out onUpdateNetworkListCallback.OutputAdapter.output, onUpdateNetworkListCallback);
        }

        protected void OnUpdateNetworkList(PPError result, NetworkList networkList)
            => HandleUpdateNetworkList?.Invoke(this, new NetworkListInfo(result, networkList));

        /// <summary>
        /// Returns objects that describe the network interfaces asynchronously.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>A NetworkListInfo instance see <see cref="NetworkListInfo"/></returns>
        public Task<NetworkListInfo> UpdateNetworkListAsync(MessageLoop messageLoop = null)
            => UpdateNetworkListAsyncCore(messageLoop);

        private async Task<NetworkListInfo> UpdateNetworkListAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<NetworkListInfo>();
            EventHandler<NetworkListInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleUpdateNetworkList += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    UpdateNetworkList();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPResource>();
                        var result = (PPError)PPBNetworkMonitor.UpdateNetworkList(this, 
                            out output.output, 
                            new BlockUntilComplete());

                        tcs.TrySetResult(new NetworkListInfo(result, new NetworkList(output.Output)));
                    }
                    );
                    if (messageLoop == null)
                        MessageLoop.PostWork(action);
                    else
                        messageLoop.PostWork(action);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return new NetworkListInfo(PPError.Aborted, null);
            }
            finally
            {
                HandleUpdateNetworkList -= handler;
            }
        }
    }

    public class NetworkListInfo : EventArgs
    {
        public PPError Result { get; private set; }
        public NetworkList NetworkList { get; private set; }

        internal NetworkListInfo(PPError result, NetworkList networkList)
        {
            Result = result;
            NetworkList = networkList;
        }
    }
}
