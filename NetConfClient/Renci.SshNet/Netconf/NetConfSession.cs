using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Renci.SshNet.Common;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace Renci.SshNet.NetConf
{
    internal class NetConfSession : SubsystemSession, INetConfSession
    {
        private const string Prompt = "]]>]]>";

        private readonly StringBuilder _data = new StringBuilder();
        private readonly StringBuilder _datanotification = new StringBuilder();
        public  bool _usingFramingProtocol =true;
        private EventWaitHandle _serverCapabilitiesConfirmed = new AutoResetEvent(false);
        private EventWaitHandle _rpcReplyReceived = new AutoResetEvent(false);
        private EventWaitHandle _rpcReplyReceivedNotificaton = new AutoResetEvent(false);
        private StringBuilder _rpcReply = new StringBuilder();
        private StringBuilder _rpcReplyNotification = new StringBuilder();
        private int _messageId;

        /// <summary>
        /// Gets NetConf server capabilities.
        /// </summary>
        public XmlDocument ServerCapabilities { get; private set; }

        /// <summary>
        /// Gets NetConf client capabilities.
        /// </summary>
        public XmlDocument ClientCapabilities { get; private set; }


        
        /// <summary>
        /// Initializes a new instance of the <see cref="NetConfSession"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="operationTimeout">The number of milliseconds to wait for an operation to complete, or -1 to wait indefinitely.</param>
        public NetConfSession(ISession session, int operationTimeout): base(session, "netconf", operationTimeout)
        {
            ClientCapabilities = new XmlDocument();
            string xml1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                                    "<hello xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\">" +
                                                        "<capabilities>" +
                                                            "<capability>" +
                                                                "urn:ietf:params:netconf:base:1.0" +
                                                            "</capability>" +
                                                        "</capabilities>" +
                                                    "</hello>";
            string xml2 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                        "<hello xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\">" +
                                            "<capabilities>" +
                                                "<capability>" +
                                                    "urn:ietf:params:netconf:base:1.0" +
                                                "</capability>" +
                                              "<capability>" +
                                                "urn:ietf:params:netconf:base:1.1" +
                                            "</capability>" +
                                            "</capabilities>" +
                                        "</hello>";

            if (_usingFramingProtocol)
            {
                ClientCapabilities.LoadXml(xml2);

            }
            else {
                ClientCapabilities.LoadXml(xml1);
            }
        }
        static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                NewLineChars = "\n",
                //Encoding = Encoding.UTF8,
                NewLineHandling = System.Xml.NewLineHandling.Replace,
                OmitXmlDeclaration = true,
                ConformanceLevel = System.Xml.ConformanceLevel.Document
            };
            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }
            return stringBuilder.ToString();

        }
        public XmlDocument SendReceiveRpc(XmlDocument rpc, bool automaticMessageIdHandling ,int Timeout)
        {
            _data.Clear();
            XmlNamespaceManager nsMgr = null;
            if (automaticMessageIdHandling)
            {
                _messageId++;
                nsMgr = new XmlNamespaceManager(rpc.NameTable);
                nsMgr.AddNamespace("nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                rpc.SelectSingleNode("/nc:rpc/@message-id", nsMgr).Value = _messageId.ToString(CultureInfo.InvariantCulture);
            }
            _rpcReply = new StringBuilder();
            _rpcReplyReceived.Reset();
            var reply = new XmlDocument();
            //_usingFramingProtocol = false;
            if (_usingFramingProtocol)
            {
                //var command = PrettyXml(rpc.OuterXml);


                //var command = new StringBuilder();
                //command.AppendFormat("\n#{0}\n", PrettyXml(rpc.OuterXml).Length);
                //string top = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                //command.Append(PrettyXml(rpc.OuterXml));
                //command.Append("\n##\n");
                //SendData(Encoding.UTF8.GetBytes(command.ToString()));
                //System.Diagnostics.Debug.WriteLine("1.1版本+RPC请求打印日志：\r\n" + command);

                string rpcsend1 = rpc.OuterXml;
                //int size = PrettyXml(rpcsend1).Length;
                int size = Encoding.UTF8.GetByteCount(PrettyXml(rpcsend1));
                string rpctop = "\n#" + size.ToString() + "\n";
                string end = "\n##\n";
                string rpcsend = rpctop + PrettyXml(rpcsend1) + end;
                SendData(Encoding.UTF8.GetBytes(rpcsend));
                System.Diagnostics.Debug.WriteLine("1.1版本+RPC请求打印日志：\r\n" + rpcsend);
                WaitOnHandle(_rpcReplyReceived, Timeout);
                for (int i = 0; i < 5; i++) {
                    if (!_rpcReply.ToString().Contains("rpc"))
                    {
                        _data.Clear();
                        _rpcReply = new StringBuilder();
                        _rpcReplyReceived.Reset();
                        WaitOnHandle(_rpcReplyReceived, Timeout);
                    }
                    if (_rpcReply.ToString().Contains("rpc"))
                    {
                        break;
                    }
                }
                reply.LoadXml(_rpcReply.ToString());

            }
            else
            {
                SendData(Encoding.UTF8.GetBytes(PrettyXml(rpc.OuterXml) + Prompt));
                System.Diagnostics.Debug.WriteLine("1.0版本+RPC请求打印日志：\r\n" + PrettyXml(rpc.OuterXml) + Prompt);

                WaitOnHandle(_rpcReplyReceived, Timeout);
                for (int i = 0; i < 5; i++)
                {
                    if (!_rpcReply.ToString().Contains("rpc"))
                    {
                        _data.Clear();
                        _rpcReply = new StringBuilder();
                        _rpcReplyReceived.Reset();
                        WaitOnHandle(_rpcReplyReceived, Timeout);
                    }
                    if (_rpcReply.ToString().Contains("rpc"))
                    {
                        break;
                    }

                }
                reply.LoadXml(_rpcReply.ToString());
            }
            if (automaticMessageIdHandling)
            {
                var replyId = rpc.SelectSingleNode("/nc:rpc/@message-id", nsMgr).Value;
                if (replyId != _messageId.ToString(CultureInfo.InvariantCulture))
                {
                    throw new NetConfServerException("The rpc message id does not match the rpc-reply message id.");
                }
            }
            return reply;
        }


        public string  SendReceiveRpcSub(int Timeout)
        {
            _datanotification.Clear();
            _rpcReplyNotification = new StringBuilder();
            _rpcReplyReceivedNotificaton.Reset();
            WaitOnHandleNotification(_rpcReplyReceivedNotificaton, -1);
            return _rpcReplyNotification.ToString();

        }
        public void SendReceiveRpcKeepLive()
        {
            var rpc = new XmlDocument();
            rpc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"6\"><get ><filter/></get ></rpc>");
            SendData(Encoding.UTF8.GetBytes(rpc.InnerXml + Prompt));
        }


        protected override void OnChannelOpen()
        {
            _data.Clear();
            _datanotification.Clear();
           // var message = string.Format("{0}{1}", ClientCapabilities.InnerXml, Prompt);
            var command = PrettyXml(ClientCapabilities.InnerXml);
            SendData(Encoding.UTF8.GetBytes(command.ToString()+ Prompt));
           // SendData(Encoding.UTF8.GetBytes(message));


            WaitOnHandle(_serverCapabilitiesConfirmed, OperationTimeout);
        }

        protected override void OnDataReceived(byte[] data)
        {
            var chunk = Encoding.UTF8.GetString(data);
            var chunknotfication = Encoding.UTF8.GetString(data);
            if (ServerCapabilities == null)   // This must be server capabilities, old protocol
            {
                _data.Append(chunk);  

                if (!chunk.Contains(Prompt))
                {
                    return;
                }
                try
                {
                    chunk = _data.ToString(); 
                    _data.Clear();

                    ServerCapabilities = new XmlDocument();
                    ServerCapabilities.LoadXml(chunk.Replace(Prompt, ""));
                }
                catch (XmlException e)
                {
                    throw new NetConfServerException("Server capabilities received are not well formed XML", e);
                }

                var nsMgr = new XmlNamespaceManager(ServerCapabilities.NameTable);
                nsMgr.AddNamespace("nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                _usingFramingProtocol = (ServerCapabilities.SelectSingleNode("/nc:hello/nc:capabilities/nc:capability[text()='urn:ietf:params:netconf:base:1.1']", nsMgr) != null);

                _serverCapabilitiesConfirmed.Set();
            }
            else 
            if (_usingFramingProtocol)
            {
                _data.Append(chunk);
                _datanotification.Append(chunknotfication);

                if (!chunk.Contains("##"))
                {
                    System.Diagnostics.Debug.WriteLine(chunk);
                    return;
                    //throw new NetConfServerException("Server XML message does not end with the prompt " + _prompt);
                }

                if (!chunknotfication.Contains("##"))
                {
                    return;
                    //throw new NetConfServerException("Server XML message does not end with the prompt " + _prompt);
                }
                System.Diagnostics.Debug.WriteLine(chunk);
                System.Diagnostics.Debug.WriteLine("\r\n 1.1版本服务器回复打印完毕");

                chunk = _data.ToString();
                chunknotfication = _datanotification.ToString();
                _data.Clear();
                _datanotification.Clear();
                _rpcReply.Append(Regex.Replace(chunk, @"\n#([\d\n\#]*)", ""));
                chunknotfication =Regex.Replace(chunknotfication, @"\n#([\d\n\#]*)", "");
                _rpcReplyNotification.Append(chunknotfication);
                _rpcReplyReceived.Set();
                _rpcReplyReceivedNotificaton.Set();
            }
            else  // Old protocol
            {
                _data.Append(chunk);
                _datanotification.Append(chunknotfication);

                if (!chunk.Contains(Prompt))
                {
                    System.Diagnostics.Debug.WriteLine(chunk);
                    return;
                    //throw new NetConfServerException("Server XML message does not end with the prompt " + _prompt);
                }

                if (!chunknotfication.Contains(Prompt))
                {
                    return;
                    //throw new NetConfServerException("Server XML message does not end with the prompt " + _prompt);
                }
                System.Diagnostics.Debug.WriteLine(chunk);
                System.Diagnostics.Debug.WriteLine("\r\n 1.0版本服务器回复打印完毕");

                chunk = _data.ToString();
                chunknotfication = _datanotification.ToString();
                _data.Clear();
                _datanotification.Clear();
                _rpcReply.Append(chunk.Replace(Prompt, ""));
                _rpcReplyNotification.Append(chunknotfication.Replace(Prompt, ""));
                _rpcReplyReceived.Set();
                _rpcReplyReceivedNotificaton.Set();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_serverCapabilitiesConfirmed != null)
                {
                    _serverCapabilitiesConfirmed.Dispose();
                    _serverCapabilitiesConfirmed = null;
                }

                if (_rpcReplyReceived != null)
                {
                    _rpcReplyReceived.Dispose();
                    _rpcReplyReceived = null;
                }
            }
        }

        public bool Netconf_version(bool _netconf_version)
        {
            _usingFramingProtocol = _netconf_version;
            throw new NotImplementedException();
        }
    }
}
