using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PepperSharp
{
    public class NetworkList : Resource
    {

        List<NetworkInterface> interfaces = new List<NetworkInterface>();

        internal NetworkList(PPResource resource) : base(PassRef.PassRef, resource)
        {
            Count = PPBNetworkList.GetCount(this);
            for (uint x = 0; x < Count; x++)
            {
                interfaces.Add(new NetworkInterface(this, x));
            }
        }

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                foreach (var item in interfaces)
                    item.Dispose();
                    
                interfaces.Clear();
                interfaces = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        /// <summary>
        /// Gets the number of information objects contained in the list
        /// </summary>
        public uint Count { get; private set; }

        /// <summary>
        /// Get a readonly collection of the network information classes.
        /// </summary>
        public ReadOnlyCollection<NetworkInterface> NetworkInterfaces
            => interfaces.AsReadOnly();
    }

    /// <summary>
    /// This class encapsulates data for network interface info.  
    /// You do not create instances of this class; the NetworkMonitor.UpdateNetworkList method 
    /// creates a collection that contains one instance of this class for each network interface in the list.
    /// </summary>
    public sealed class NetworkInterface : IDisposable
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public NetworkInterfaceState State { get; private set; }
        public NetworkInterfaceType NetworkType { get; private set; }
        public uint MTU { get; private set; }
        List<NetAddress> ipAddress = new List<NetAddress>();

        internal NetworkInterface(PPResource networkList, uint index)
        {
            Name = ((Var)PPBNetworkList.GetName(networkList, index)).AsString();
            DisplayName = ((Var)PPBNetworkList.GetDisplayName(networkList, index)).AsString();
            State = (NetworkInterfaceState)PPBNetworkList.GetState(networkList, index);
            NetworkType = (NetworkInterfaceType)PPBNetworkList.GetType(networkList, index);
            MTU = PPBNetworkList.GetMTU(networkList, index);

            using (var varIPAddresses = new VarArray ())
            {
                var IPAddresses = new ArrayOutputAdapterWithStorage<PPResource []> ();
                var result = (PPError)PPBNetworkList.GetIpAddresses (networkList, index, (PPArrayOutput)IPAddresses.Adapter);
                if (result == PPError.Ok) {
                    var length = IPAddresses.Output.Length;

                    for (uint j = 0; j < IPAddresses.Output.Length; ++j) {
                        ipAddress.Add (new NetAddress (IPAddresses.Output [j]));
                    }
                }
            }
        }

        public ReadOnlyCollection<NetAddress> NetAddresses
            => ipAddress.AsReadOnly();

        #region Implement IDisposable.

        public void Dispose()
        {
            foreach (var item in ipAddress)
                item.Dispose();

            ipAddress.Clear();
            ipAddress = null;

            GC.SuppressFinalize(this);

        }

        ~NetworkInterface()
        {
            Dispose();
        }

        #endregion
    }

    /**
    * Type of a network interface.
    */
    public enum NetworkInterfaceType
    {
        /**
         * Type of the network interface is not known.
         */
        Unknown = 0,
        /**
         * Wired Ethernet network.
         */
        Ethernet = 1,
        /**
         * Wireless Wi-Fi network.
         */
        Wifi = 2,
        /**
         * Cellular network (e.g. LTE).
         */
        Cellular = 3
    }

    /**
     * State of a network interface.
     */
    public enum NetworkInterfaceState
    {
        /**
         * Network interface is down.
         */
        Down = 0,
        /**
         * Network interface is up.
         */
        Up = 1
    }
}
