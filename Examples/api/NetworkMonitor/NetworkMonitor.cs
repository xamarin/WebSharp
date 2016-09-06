using System;
using System.Threading.Tasks;

using PepperSharp;

namespace NetworkMonitor
{
    public class NetworkMonitor : Instance
    {

        public NetworkMonitor(IntPtr handle) : base(handle)
        {
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "NetworkMonitor", "There be dragons.");

            UpdateNetworkList();
        }

        async Task UpdateNetworkList()
        {
            using (var networkMonitor = new PepperSharp.NetworkMonitor(this))
            {
                var networkListInfo = await networkMonitor.UpdateNetworkListAsync();

                if (networkListInfo.Result != PPError.Ok)
                {
                    PostMessage($"UpdateNetworkList failed: {networkListInfo.Result}");
                    return;
                }

                using (var networkList = networkListInfo.NetworkList)
                {
                    // Send the new network list to JavaScript.
                    using (var varNetworkList = new VarArray())
                    {
                        uint infoIndex = 0;
                        foreach (var nic in networkList.NetworkInterfaces)
                        {
                            using (VarDictionary varNetwork = new VarDictionary())
                            {
                                varNetwork.Set("displayName", nic.DisplayName);
                                varNetwork.Set("name", nic.Name);
                                varNetwork.Set("state", GetEnumAsString(typeof(NetworkInterfaceState), nic.State));
                                varNetwork.Set("type", GetEnumAsString(typeof(NetworkInterfaceType), nic.NetworkType));
                                varNetwork.Set("MTU", nic.MTU);

                                using (var varIPAddresses = new VarArray())
                                {
                                    uint j = 0;
                                    foreach (var address in nic.NetAddresses)
                                    {
                                        varIPAddresses.Set(j++, address.DescriptionWithPort);
                                    }
                                    varNetwork.Set("ipAddresses", varIPAddresses);
                                    varNetworkList.Set(infoIndex++, varNetwork);
                                }
                            }
                        }

                        PostMessage(varNetworkList);
                    }
                }
            }

        }

        // static
        static string GetEnumAsString(Type type, object value)
        {
            if (Enum.IsDefined(type, value))
                return Enum.GetName(type, value);
            else
                return "Invalid";
        }
 
    }
}
