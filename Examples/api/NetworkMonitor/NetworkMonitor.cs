using System;

using PepperSharp;

namespace NetworkMonitor
{
    public class NetworkMonitor : Instance
    {
        PPResource networkMonitor;

        public NetworkMonitor(IntPtr handle) : base(handle)
        {
            networkMonitor = PPBNetworkMonitor.Create(this);
        }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "NetworkMonitor", "There be dragons.");
            // Start listing for network updates.
            var onUpdateNetworkListCallback = new CompletionCallbackWithOutput<PPResource>(OnUpdateNetworkList);
            var result = (PPError)PPBNetworkMonitor.UpdateNetworkList(networkMonitor, out onUpdateNetworkListCallback.OutputAdapter.output, onUpdateNetworkListCallback);
            if (result != PPError.OkCompletionpending)
            {
                PostMessage($"UpdateNetworkList failed: {result}");
            }
            return base.Init(argc, argn, argv);
        }

        private void OnUpdateNetworkList(PPError result, PPResource networkList)
        {
            
            // Send the new network list to JavaScript.
            if (result < 0)
            {
                PostMessage($"UpdateNetworkList failed: {result}");
                return;
            }

            var varNetworkList = new VarArray();
            uint count = PPBNetworkList.GetCount(networkList);

            for (uint i = 0; i < count; ++i)
            {
                VarDictionary varNetwork = new VarDictionary();
                
                varNetwork.Set("displayName", ((Var)PPBNetworkList.GetDisplayName(networkList, i)).AsString());
                varNetwork.Set("name", ((Var)PPBNetworkList.GetName(networkList, i)).AsString());
                varNetwork.Set("state", GetNetworkStateAsString(PPBNetworkList.GetState(networkList, i)));
                varNetwork.Set("type", GetNetworkTypeAsString(PPBNetworkList.GetType(networkList, i)));
                varNetwork.Set("MTU", (int)PPBNetworkList.GetMTU(networkList, i));

                var varIPAddresses = new VarArray();
                var IPAddresses = new ArrayOutputAdapterWithStorage<PPResource[]>();
                
                result = (PPError)PPBNetworkList.GetIpAddresses(networkList, i, (PPArrayOutput)IPAddresses.Adapter);
                if (result == PPError.Ok)
                {
                    var length = IPAddresses.Output.Length;
                    
                    for (uint j = 0; j < IPAddresses.Output.Length; ++j)
                    {
                        varIPAddresses.Set(j, GetNetAddressAsString(IPAddresses.Output[j]));
                    }
                }
                else
                {
                    // Call to GetIpAddresses failed, just give an empty list.
                }
                varNetwork.Set("ipAddresses", varIPAddresses);
                varNetworkList.Set(i, varNetwork);
            }

            PostMessage(varNetworkList);
        }

        // static
        static string GetNetworkStateAsString(
            PPNetworkListState state)
        {
            switch (state)
            {
                case PPNetworkListState.Up:
                    return "up";

                case PPNetworkListState.Down:
                    return "down";

                default:
                    return "invalid";
            }
        }

        // static
        static string GetNetworkTypeAsString(
            PPNetworkListType type)
        {
            switch (type)
            {
                case PPNetworkListType.Ethernet:
                    return "ethernet";

                case PPNetworkListType.Wifi:
                    return "wifi";

                case PPNetworkListType.Cellular:
                    return "cellular";

                case PPNetworkListType.Unknown:
                    return "unknown";

                default:
                    return "invalid";
            }
        }

        // static
        static string GetNetAddressAsString(
            PPResource address)
        {
            bool include_port = true;
            return ((Var)PPBNetAddress.DescribeAsString(address, include_port?PPBool.True:PPBool.False)).AsString();
        }
    }
}
