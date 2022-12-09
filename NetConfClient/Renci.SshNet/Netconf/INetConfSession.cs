using System.Xml;

namespace Renci.SshNet.NetConf
{
    internal interface INetConfSession : ISubsystemSession
    {
        /// <summary>
        /// Gets the NetConf server capabilities.
        /// </summary>
        /// <value>
        /// The NetConf server capabilities.
        /// </value>
        XmlDocument ServerCapabilities { get; }

        /// <summary>
        /// Gets the NetConf client capabilities.
        /// </summary>
        /// <value>
        /// The NetConf client capabilities.
        /// </value>
        XmlDocument ClientCapabilities { get; }

        XmlDocument SendReceiveRpc(XmlDocument rpc, bool automaticMessageIdHandling,int Timeout);
        string SendReceiveRpcSub(int Timeout);
        bool Netconf_version(bool _netconf_version);

        void SendReceiveRpcKeepLive();
        void HostInformation( string hostip, int port, string username, string password);
    }
}
