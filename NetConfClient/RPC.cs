using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class RPC
    {
        public static string Send(XmlDocument rpc, int id, string ip)
        {
            string result = "";
            try
            {
                //TreeReP.Nodes.Clear();
                
                if (Form_Main.netConfClient[id] == null)
                {
                    result = "设备离线";
                    return result;
                }
                if (!Form_Main.netConfClient[id].IsConnected)
                {
                    // 断开连接ToolStripMenuItem.PerformClick();
                    result = "设备离线";
                    return result;
                }

                string rpcxml = "";
                if (!rpc.OuterXml.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + rpc.OuterXml + RpcEnd;
                    Form_Main.netConfClient[id].AutomaticMessageIdHandling = true;

                }
                else
                {
                    rpcxml = rpc.OuterXml;
                    Form_Main.netConfClient[id].AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                var rpcResponse = Form_Main.netConfClient[id].SendReceiveRpc(rpc);
                result = rpcResponse.OuterXml;

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
            return result;

        }
    }
}
