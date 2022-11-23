using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Runtime.InteropServices;
using NetConfClientSoftware;
using System.Xml.XPath;
using System.Net.NetworkInformation;
using System.Diagnostics;
using NetConfClientSoftware.MenuForm;
using NetConfClientSoftware.ConfigServices;

namespace NetConfClientSoftware
{
    public partial class Form_Main : Form
    {
        public static NetConfClient[] netConfClient = new NetConfClient[32];
        Thread[] ThSub = new Thread[32];
        // List<NetConfClient> netConfClient = new List<NetConfClient>();
        bool[] Sub = new bool[32];      //订阅开关默认禁止
        public static string defaultfilePath = "";       //打开文件夹默认路径
        public static string CUCC_YIN = @"C:\netconf\YANG\CUCC\YIN\";       //联通YIN文件
        public static string CTCC_YIN = @"C:\netconf\YANG\CTCC\YIN\";       //联通YIN文件
        public static string CMCC_YIN = @"C:\netconf\YANG\CMCC\YIN\";       //联通YIN文件
        public static string FenGeFu = "\r\n---------------------------------------------------------------------------------------------\r\n";//分隔符
        public string XML_URL = "http://hunan128.com:888/NetconfXML/";   //XML在线文件地址
        public string gpnip = "";//设备IP地址
        public int gpnport = 830;//设备端口
        public string gpnuser = "";//设备用户名
        public string gpnpassword = "";//设备密码
        public string gpnnetconfversion = "";//设备版本
        public string ips = "联通";
        public string gpnname = "GPN";
        public static string neinfopath = @"C:\netconf\neinfo.xml";
        public string CUCC_YIN_URL = "http://hunan128.com:888/YANG/CUCC/YIN/";   //XML在线文件地址
        public string CTCC_YIN_URL = "http://hunan128.com:888/YANG/CTCC/YIN/";   //XML在线文件地址
        public string CMCC_YIN_URL = "http://hunan128.com:888/YANG/CMCC/YIN/";   //XML在线文件地址
        public int AutoCount = 0;    //自动化循环次数
        public static string Rpc_Reply = "";
        #region 声明ini变量
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        // private string strFilePath = Application.StartupPath + "\\Config.ini";//获取INI文件路径
        private string strFilePath = @"C:\netconf\Config.ini";
        private string strSec = ""; //INI文件名
        #endregion
        public Form_Main()
        {
            InitializeComponent();
            this.treeViewNEID.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeViewNEID.DrawNode += new DrawTreeNodeEventHandler(treeViewNEID_DrawNode);
        }
        //在绘制节点事件中，按自已想的绘制
        private void treeViewNEID_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //e.DrawDefault = true; //我这里用默认颜色即可，只需要在TreeView失去焦点时选中节点仍然突显
            //return;
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                //演示为绿底白字
                e.Graphics.FillRectangle(Brushes.SkyBlue, e.Node.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, Rectangle.Inflate(e.Bounds, 2, 0));
            }
            else
            {
                e.DrawDefault = true;
            }
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = e.Node.Bounds;
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }
        }
        /// <summary>
        /// 发送XML脚本按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButSend_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            Thread thread = new Thread(() => SendNetconfRpc(id, ip));
            thread.Start();
        }
        /// <summary>
        /// 发送订阅消息
        /// </summary>
        private void SendNetconfRpc(int id, string ip)
        {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            try
            {
                BeginInvoke(new MethodInvoker(delegate () {
                    if (netConfClient[id] == null)
                    {
                        MessageBox.Show(ip + "：设备离线");
                        return;
                    }
                    if (!netConfClient[id].IsConnected)
                    {
                        //断开连接ToolStripMenuItem.PerformClick();
                        MessageBox.Show(ip + "：设备离线");
                        return;
                    }
                    TreeReP.Nodes.Clear();
                    Rpc_Reply = "";
                    string rpcxml = "";
                    XmlDocument rpc = new XmlDocument();
                    if (!fastColoredTextBoxReq.Text.Contains("rpc"))
                    {
                        string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                        string RpcEnd = "</rpc>";
                        rpcxml = RpcTop + fastColoredTextBoxReq.Text + RpcEnd;
                        if (netConfClient != null)
                            netConfClient[id].AutomaticMessageIdHandling = true;
                    }
                    else
                    {
                        rpcxml = fastColoredTextBoxReq.Text;
                        if (netConfClient != null)
                            netConfClient[id].AutomaticMessageIdHandling = false;
                    }
                    rpc.LoadXml(rpcxml);
                    DateTime dTimeEnd = System.DateTime.Now;
                    TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "请求：" + FenGeFu);
                    TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + FenGeFu);
                    fastColoredTextBoxReq.Text = XmlFormat.Xml(rpc.OuterXml);
                    DateTime dTimeServer = System.DateTime.Now;
                    var rpcResponse = netConfClient[id].SendReceiveRpc(rpc);
                    dTimeServer = System.DateTime.Now;
                    TimeSpan ts = dTimeServer - dTimeEnd;
                    LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                    TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                    Rpc_Reply = XmlFormat.Xml(rpcResponse.OuterXml);
                    TextLog.AppendText(Rpc_Reply + FenGeFu);
                    LoadTreeFromXmlDocument_TreeReP(rpcResponse);
                }));
            }
            catch (Exception ex)
            {
                TextLog.AppendText("网元："+ nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText(ex.Message + "\r\n");
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 使用netconf连接设备，从此处开始
        /// </summary>
        /// <param name="ip">设备IP</param>
        /// <param name="port">设备端口</param>
        /// <param name="user">用户名</param>
        /// <param name="passd">密码</param>
        private void LoginNetconfService(string ip, int port, string user, string passd, int id)
        {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            try
            {
                DateTime dTimeEnd = System.DateTime.Now;
                DateTime dTimeServer = System.DateTime.Now;
                netConfClient[id] = new NetConfClient(ip, port, user, passd);
                LabConncet.Text = "连接中";
                netConfClient[id].Connect();
                if (netConfClient[id].IsConnected)
                {
                    BeginInvoke(new MethodInvoker(delegate ()
                    {
                        LabConncet.Text = "已连接";
                        toolStripStatusLabelips.Text = ips;
                        dTimeServer = System.DateTime.Now;
                        TimeSpan ts = dTimeServer - dTimeEnd;
                        LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                        上载全部XMLToolStripMenuItem.Enabled = true;
                        TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                        TextLog.AppendText(XmlFormat.Xml(netConfClient[id].ServerCapabilities.OuterXml) + FenGeFu);
                        TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "请求：" + FenGeFu);
                        TextLog.AppendText(XmlFormat.Xml(netConfClient[id].ClientCapabilities.OuterXml) + FenGeFu);
                        netConfClient[id].OperationTimeout = TimeSpan.FromSeconds(15);
                        netConfClient[id].TimeOut = int.Parse(ComTimeOut.Text) * 1000;
                    }));
                }
                else
                {
                    LabConncet.Text = "连接失败";
                    MessageBox.Show(gpnip + "：连接失败！");
                    dTimeServer = System.DateTime.Now;
                    TimeSpan ts = dTimeServer - dTimeEnd;
                    LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                }
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
                LabConncet.Text = "连接失败";
                MessageBox.Show(gpnip + "：连接失败！");
            }
        }
        /// <summary>
        /// 导入XML文件脚本按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.SelectedPath = @"C:\";
            //清除之前打开的历史  
            //获取文件路径，不带文件名
            if (defaultfilePath != "")
            {
                //设置此次默认目录为上一次选中目录  
                path.SelectedPath = defaultfilePath;
            }
            if (path.ShowDialog() == DialogResult.OK)
            {
                defaultfilePath = path.SelectedPath;
                if (path.SelectedPath == @"C:\" || path.SelectedPath == @"D:\" || path.SelectedPath == @"E:\" || path.SelectedPath == @"F:\")
                {
                    defaultfilePath = path.SelectedPath;
                }
                else
                {
                    defaultfilePath = path.SelectedPath + @"\";
                }
            }
            Readfile(path.SelectedPath);
            Gpnsetini();
        }
        /// <summary>
        /// 导入XML文件显示到下拉框
        /// </summary>
        /// <param name="path">文件名全路径含文件名</param>
        private void Readfile(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir == null)
            {
                MessageBox.Show("文件空，请重新选择文件夹！");
                return;
            }
            FileInfo[] fileInfo = dir.GetFiles();
            List<string> fileNames = new List<string>();
            foreach (FileInfo item in fileInfo)
            {
                fileNames.Add(item.Name);
            }
            foreach (string s in fileNames)
            {
                if (s.Contains("xml") || s.Contains("XML"))
                {
                    ComXml.Items.Add(s);
                    if (ComXml.Items.Count > 0)
                    {
                        ComXml.SelectedIndex = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 下拉框选项变更更新脚本到文本框函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComXml_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 打开某文件(假设web.config在根目录中)
            string filename = defaultfilePath + ComXml.Text;
            XmlDocument xmldoc = new XmlDocument();
            if (!File.Exists(filename))
            {
                xmldoc.Load(XML_URL + ComXml.Text);
                fastColoredTextBoxReq.Text = XmlFormat.Xml(xmldoc.OuterXml);
                xmldoc.Save(filename);
            }
            else
            {
                xmldoc.Load(filename);
                fastColoredTextBoxReq.Text = XmlFormat.Xml(xmldoc.OuterXml);
            }
        }
        /// <summary>
        /// Tree树桩图 答复框的显示
        /// </summary>
        /// <param name="dom">XML</param>
        private void LoadTreeFromXmlDocument_TreeReP(XmlDocument dom)
        {
            try
            {
                // SECTION 2. Initialize the TreeView control.
                // SECTION 3. Populate the TreeView with the DOM nodes.
                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                        continue;
                    AddNode(TreeReP.Nodes, node);
                }
                TreeReP.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Tree树桩图 请求框的显示
        /// </summary>
        /// <param name="dom">XML文本</param>
        static string GetAttributeText(XmlNode inXmlNode, string name)
        {
            XmlAttribute attr = (inXmlNode.Attributes?[name]);
            return attr?.Value;
        }
        private void AddNode(TreeNodeCollection nodes, XmlNode inXmlNode)
        {
            if (inXmlNode.HasChildNodes)
            {
                string text = GetAttributeText(inXmlNode, "name");
                if (string.IsNullOrEmpty(text))
                    text = inXmlNode.Name;
                for (int i = 0; i < inXmlNode.Attributes.Count; i++)
                {
                    if (!string.IsNullOrEmpty(inXmlNode.Attributes[i].OuterXml))
                        text = text + " " + inXmlNode.Attributes[i].OuterXml;
                }
                TreeNode newNode = null;
                XmlNodeList nodeList = inXmlNode.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    XmlNode xNode = inXmlNode.ChildNodes[i];
                    if (!xNode.HasChildNodes)
                    {
                        // If the node has an attribute "name", use that.  Otherwise display the entire text of the node.
                        //string value = GetAttributeText(xNode, "name");
                        //if (string.IsNullOrEmpty(value)) {
                        //    value = (xNode.OuterXml).Trim();
                        //}
                        //nodes.Add(text + "(" + value + ")");
                        string value = GetAttributeText(xNode, "name");
                        if (string.IsNullOrEmpty(value))
                        {
                           
                            value = xNode.Value;

                            if (string.IsNullOrEmpty(value))
                            {
                                
                                // value = xNode.Name + " "+ xNode.Attributes[0].OuterXml;
                                value = xNode.Name;
                                if (i == 0)
                                {
                                    TreeNode textnode = new TreeNode();
                                    textnode.Text = text;
                                    textnode.Nodes.Add(value);
                                    nodes.Add(textnode);
                                }
                              

                            }
                            else
                            {
                                nodes.Add(text + "(" + value + ")");
                            }
                        }
                    }
                    else
                    {
                        if (newNode == null)
                        {
                            newNode = nodes.Add(text);
                        }

                    }
                    if (newNode != null)
                        AddNode(newNode.Nodes, xNode);
                }
            }
            else
            {
                //// If the node has an attribute "name", use that.  Otherwise display the entire text of the node.
                //string text = GetAttributeText(inXmlNode, "name");
                //if (string.IsNullOrEmpty(text))
                //    text = (inXmlNode.OuterXml).Trim();
                //TreeNode newNode = nodes.Add(text);
            }
        }
        /// <summary>
        /// 读取config文件函数
        /// </summary>
        /// <param name="Section">config节点</param>
        /// <param name="key">节点内的子节点</param>
        /// <returns></returns>
        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
        /// <summary>
        /// 加载ini的config文件
        /// </summary>
        private void Readini()
        {
            #region 读取ini文件
            try
            {
                if (!Directory.Exists(@"C:\netconf"))
                {
                    Directory.CreateDirectory(@"C:\netconf");
                }
                //   导入前俩列ToolStripMenuItem.PerformClick();
                if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
                {
                    strSec = "Config";
                    defaultfilePath = ContentValue(strSec, "NetconfXml");
                    gpnip = ContentValue(strSec, "ip");
                    gpnport = int.Parse(ContentValue(strSec, "port"));
                    gpnuser = ContentValue(strSec, "user");
                    gpnpassword = ContentValue(strSec, "password");
                    gpnnetconfversion = ContentValue(strSec, "ver");
                    ips = ContentValue(strSec, "ips");
                    Readfile(defaultfilePath);
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
            #endregion
        }
        #region 设置ini文件内容
        private void Gpnsetini()
        {
            try
            {
                //根据INI文件名设置要写入INI文件的节点名称
                //此处的节点名称完全可以根据实际需要进行配置
                strSec = "Config";
                WritePrivateProfileString(strSec, "NetconfXml", defaultfilePath, strFilePath);
                WritePrivateProfileString(strSec, "ip", gpnip, strFilePath);
                WritePrivateProfileString(strSec, "port", gpnport.ToString(), strFilePath);
                WritePrivateProfileString(strSec, "user", gpnuser, strFilePath);
                WritePrivateProfileString(strSec, "password", gpnpassword, strFilePath);
                WritePrivateProfileString(strSec, "ver", gpnnetconfversion, strFilePath);
                WritePrivateProfileString(strSec, "ips", ips, strFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Gpnsetini();
                //Sub = false;
                //for (int i = 0; i < dataGridViewNeInformation.RowCount - 1; i++)
                //{
                //    if (dataGridViewNeInformation.Rows[i].Cells["订阅"].Value != null)
                //    {
                //        if (dataGridViewNeInformation.Rows[i].Cells["订阅"].Value.ToString() == "已开启")
                //        {
                //            Sub = false;
                //            int id = int.Parse(dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString());
                //            netConfClient[id].SendReceiveRpcKeepLive();
                //            Thread.Sleep(3000);
                //        }
                //    }
                //}
                System.Environment.Exit(0);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 订阅在循环等待消息
        /// </summary>
        /// 
        private void Subscription(int id, string ip)
        {
            if (Sub[id])
            {
               // TextLog.AppendText("Notification服务器：" + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText("订阅监听已经开启，请关注\r\n");
            }
            string rpcResponse = "";
            while (Sub[id])
            {
                rpcResponse = netConfClient[id].SendReceiveRpcSub();
                try
                {
                    System.Diagnostics.Debug.WriteLine(FenGeFu+ rpcResponse + FenGeFu);
                    Thread notificatonthread = new Thread(() => Notification(rpcResponse, id,ip));
                    notificatonthread.Start();
                }
                catch (Exception ex)
                {
                    TextLog.AppendText("\r\n" + ex.ToString() + "\r\n");
                }
            }
        }
        private void Notification(string rpcResponse, int id,string ip )
        {
            Thread log = new Thread(() => Lognotification(rpcResponse, ip));
            log.Start();
            //Thread mes = new Thread(() => ShowXML(rpcResponse, id));
            //mes.Start();
            Pgnot(rpcResponse, ip);
            AlarmNotfication(rpcResponse, ip);
            //Thread attributeValueChange = new Thread(() => AttributeValueChange(rpcResponse, ip));
            //attributeValueChange.Start();
            AttributeValueChange(rpcResponse, ip);
            LLDP(rpcResponse, ip);
            Peer(rpcResponse, ip);
        }
            private void Lognotification(string rpcResponse , string ip) {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            TextLog.AppendText("网元：" + /*netConfClient[id].ConnectionInfo.Host*/ nename + " " + System.DateTime.Now.ToString() + "." + System.DateTime.Now.Millisecond.ToString() + "通知：" + FenGeFu + XmlFormat.Xml(rpcResponse) + FenGeFu);
        }
        private static readonly object LOCK = new object();
        private void Peer(string rpcResponse, string ip)
        {
            try
            {
                if (!rpcResponse.Contains("peer-change-notification"))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                string nename = "";
                string ipsname = "";
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                    {
                        nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                        ipsname = dataGridViewNeInformation.Rows[i].Cells["运营商"].Value.ToString();
                        break;
                    }
                }
                int index = 0;
                lock (LOCK)
                {
                    index = dataGridViewPeerChange.Rows.Add();
                    dataGridViewPeerChange.FirstDisplayedScrollingRowIndex = dataGridViewPeerChange.Rows.Count - 1;
                }

                //dataGridViewAttributeValueChange.CurrentCell = dataGridViewAttributeValueChange.Rows[index].Cells[0];
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:notification:1.0");
                root.AddNamespace("xmlns_name", "urn:ccsa:yang:acc-devm");
                XmlNode eventTime = xmlDoc.SelectSingleNode("/rpc:notification/rpc:eventTime", root);
                if (eventTime != null) { dataGridViewPeerChange.Rows[index].Cells["Peer事件时间"].Value = eventTime.InnerText; }
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//xmlns_name:peer-change-notification", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                    {

                        if (ipsname.Contains("移动"))
                        {
                            XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                            XmlNode peer_ip_address = itemNode.SelectSingleNode("//xmlns_name:peer-ip-address", root);
                            XmlNode peer_ptp_tcp_id = itemNode.SelectSingleNode("//xmlns_name:peer-ptp-tcp-id", root);
                            dataGridViewPeerChange.Rows[index].Cells["Peer网元"].Value = nename;
                            if (ptp_name != null) { dataGridViewPeerChange.Rows[index].Cells["Peer端口名称"].Value = ptp_name.InnerText; }
                            if (peer_ip_address != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端IP地址"].Value = peer_ip_address.InnerText; }
                            if (peer_ptp_tcp_id != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端端口TCPID"].Value = peer_ptp_tcp_id.InnerText; }

                        }
                        if (ipsname.Contains("电信"))
                        {
                            XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                            XmlNode peer_ip_address = itemNode.SelectSingleNode("//xmlns_name:peer-ip-address", root);
                            XmlNode peer_ptp_tcp_id = itemNode.SelectSingleNode("//xmlns_name:peer-ptp-tcp-id", root);
                            dataGridViewPeerChange.Rows[index].Cells["Peer网元"].Value = nename;
                            if (ptp_name != null) { dataGridViewPeerChange.Rows[index].Cells["Peer端口名称"].Value = ptp_name.InnerText; }
                            if (peer_ip_address != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端IP地址"].Value = peer_ip_address.InnerText; }
                            if (peer_ptp_tcp_id != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端端口TCPID"].Value = peer_ptp_tcp_id.InnerText; }

                        }
                        if (ipsname.Contains("联通"))
                        {
                            XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                            XmlNode peer_ip_address = itemNode.SelectSingleNode("//xmlns_name:peer-ip-address", root);
                            XmlNode peer_ptp_tcp_id = itemNode.SelectSingleNode("//xmlns_name:peer-ptp-tcp-id", root);
                            dataGridViewPeerChange.Rows[index].Cells["Peer网元"].Value = nename;
                            if (ptp_name != null) { dataGridViewPeerChange.Rows[index].Cells["Peer端口名称"].Value = ptp_name.InnerText; }
                            if (peer_ip_address != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端IP地址"].Value = peer_ip_address.InnerText; }
                            if (peer_ptp_tcp_id != null) { dataGridViewPeerChange.Rows[index].Cells["Peer对端端口TCPID"].Value = peer_ptp_tcp_id.InnerText; }

                        }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + rpcResponse);   //读取该节点的相关信息
            }
        }
        private void LLDP(string rpcResponse, string ip)
        {
            try
            {
                if (!rpcResponse.Contains("lldp-change-notification"))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                string nename = "";
                string ipsname = "";
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                    {
                        nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                        ipsname = dataGridViewNeInformation.Rows[i].Cells["运营商"].Value.ToString();
                        break;
                    }
                }
                int index = 0;
                lock (LOCK)
                {
                    index = dataGridViewLLDP.Rows.Add();
                    dataGridViewLLDP.FirstDisplayedScrollingRowIndex = dataGridViewLLDP.Rows.Count - 1;
                }

                //dataGridViewAttributeValueChange.CurrentCell = dataGridViewAttributeValueChange.Rows[index].Cells[0];
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:notification:1.0");
                root.AddNamespace("xmlns_name", "urn:ccsa:yang:acc-eth");
                XmlNode eventTime = xmlDoc.SelectSingleNode("/rpc:notification/rpc:eventTime", root);
                if (eventTime != null) { dataGridViewLLDP.Rows[index].Cells["LLDP事件时间"].Value = eventTime.InnerText; }
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//xmlns_name:lldp-change-notification", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                {

                    if (ipsname.Contains("移动"))
                    {
                        XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                        XmlNode lldp_peer_chassis_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-chassis-id", root);
                        XmlNode lldp_peer_port_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-port-id", root);
                        XmlNode lldp_peer_system_name = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-system-name", root);
                        dataGridViewLLDP.Rows[index].Cells["LLDP网元"].Value = nename;
                        if (ptp_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对象名称"].Value = ptp_name.InnerText; }
                        if (lldp_peer_chassis_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端MAC"].Value = lldp_peer_chassis_id.InnerText; }
                        if (lldp_peer_port_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端对口"].Value = lldp_peer_port_id.InnerText; }
                        if (lldp_peer_system_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端系统名称"].Value = lldp_peer_system_name.InnerText; }

                    }
                    if (ipsname.Contains("电信"))
                    {
                        XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                        XmlNode lldp_peer_chassis_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-chassis-id", root);
                        XmlNode lldp_peer_port_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-port-id", root);
                        XmlNode lldp_peer_system_name = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-system-name", root);
                        dataGridViewLLDP.Rows[index].Cells["LLDP网元"].Value = nename;
                        if (ptp_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对象名称"].Value = ptp_name.InnerText; }
                        if (lldp_peer_chassis_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端MAC"].Value = lldp_peer_chassis_id.InnerText; }
                        if (lldp_peer_port_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端端口"].Value = lldp_peer_port_id.InnerText; }
                        if (lldp_peer_system_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端系统名称"].Value = lldp_peer_system_name.InnerText; }

                    }
                    if (ipsname.Contains("联通"))
                    {
                        XmlNode ptp_name = itemNode.SelectSingleNode("//xmlns_name:ptp-name", root);
                        XmlNode lldp_peer_chassis_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-chassis-id", root);
                        XmlNode lldp_peer_port_id = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-port-id", root);
                        XmlNode lldp_peer_system_name = itemNode.SelectSingleNode("//xmlns_name:lldp-peer-system-name", root);
                        dataGridViewLLDP.Rows[index].Cells["LLDP网元"].Value = nename;
                        if (ptp_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对象名称"].Value = ptp_name.InnerText; }
                        if (lldp_peer_chassis_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端MAC"].Value = lldp_peer_chassis_id.InnerText; }
                        if (lldp_peer_port_id != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端对口"].Value = lldp_peer_port_id.InnerText; }
                        if (lldp_peer_system_name != null) { dataGridViewLLDP.Rows[index].Cells["LLDP对端系统名称"].Value = lldp_peer_system_name.InnerText; }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + rpcResponse);   //读取该节点的相关信息
            }
        }
        private void AttributeValueChange(string rpcResponse, string ip)
        {
            try
            {
                if ((!rpcResponse.Contains("attribute-value-change-notification"))&& (!rpcResponse.Contains("common-notification")))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                string nename = "";
                string ipsname = "";
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                    {
                        nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                        ipsname = dataGridViewNeInformation.Rows[i].Cells["运营商"].Value.ToString();
                        break;
                    }
                }
                int index =0;
                lock (LOCK) {
                    index = dataGridViewAttributeValueChange.Rows.Add();
                    dataGridViewAttributeValueChange.FirstDisplayedScrollingRowIndex = dataGridViewAttributeValueChange.Rows.Count - 1;
                }
                
                //dataGridViewAttributeValueChange.CurrentCell = dataGridViewAttributeValueChange.Rows[index].Cells[0];
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:notification:1.0");
                root.AddNamespace("xmlns_name", "urn:ccsa:yang:acc-notifications");
                XmlNode eventTime = xmlDoc.SelectSingleNode("/rpc:notification/rpc:eventTime", root);
                if (eventTime != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件时间"].Value = eventTime.InnerText; }
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//xmlns_name:event", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                {

                    if (ipsname.Contains("移动"))
                    {
                        XmlNode event_serial_no = itemNode.SelectSingleNode("//xmlns_name:event-serial-no", root);
                        XmlNode event_type = itemNode.SelectSingleNode("//xmlns_name:event-type", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode attribute_name = itemNode.SelectSingleNode("//xmlns_name:attribute-name", root);
                        XmlNode new_value = itemNode.SelectSingleNode("//xmlns_name:new-value", root);
                        XmlNode old_value = itemNode.SelectSingleNode("//xmlns_name:old-value", root);
                        dataGridViewAttributeValueChange.Rows[index].Cells["对象变更网元"].Value = nename;
                        if (event_serial_no != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件编号"].Value = event_serial_no.InnerText; }
                        if (event_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件类型"].Value = event_type.InnerText; }
                        if (object_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象名称"].Value = object_name.InnerText; }
                        if (object_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象类型"].Value = object_type.InnerText; }
                        if (attribute_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更属性名称"].Value = attribute_name.InnerText; }
                        if (new_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更新值"].Value = new_value.InnerText; }
                        if (old_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更旧值"].Value = old_value.InnerText; }

                    }
                    if (ipsname.Contains("电信"))
                    {
                        XmlNode event_serial_no = itemNode.SelectSingleNode("//xmlns_name:event-serial-no", root);
                        XmlNode event_type = itemNode.SelectSingleNode("//xmlns_name:event-type", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode attribute_name = itemNode.SelectSingleNode("//xmlns_name:attribute-name", root);
                        XmlNode new_value = itemNode.SelectSingleNode("//xmlns_name:new-value", root);
                        XmlNode old_value = itemNode.SelectSingleNode("//xmlns_name:old-value", root);
                        dataGridViewAttributeValueChange.Rows[index].Cells["对象变更网元"].Value = nename;
                        if (event_serial_no != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件编号"].Value = event_serial_no.InnerText; }
                        if (event_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件类型"].Value = event_type.InnerText; }
                        if (object_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象名称"].Value = object_name.InnerText; }
                        if (object_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象类型"].Value = object_type.InnerText; }
                        if (attribute_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更属性名称"].Value = attribute_name.InnerText; }
                        if (new_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更新值"].Value = new_value.InnerText; }
                        if (old_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更旧值"].Value = old_value.InnerText; }

                    }
                    if (ipsname.Contains("联通"))
                    {
                        XmlNode event_serial_no = itemNode.SelectSingleNode("//xmlns_name:event-serial-no", root);
                        XmlNode event_type = itemNode.SelectSingleNode("//xmlns_name:event-type", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode attribute_name = itemNode.SelectSingleNode("//xmlns_name:attribute-name", root);
                        XmlNode new_value = itemNode.SelectSingleNode("//xmlns_name:new-value", root);
                        XmlNode old_value = itemNode.SelectSingleNode("//xmlns_name:old-value", root);
                        dataGridViewAttributeValueChange.Rows[index].Cells["对象变更网元"].Value = nename;
                        if (event_serial_no != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件编号"].Value = event_serial_no.InnerText; }
                        if (event_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更事件类型"].Value = event_type.InnerText; }
                        if (object_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象名称"].Value = object_name.InnerText; }
                        if (object_type != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更对象类型"].Value = object_type.InnerText; }
                        if (attribute_name != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更属性名称"].Value = attribute_name.InnerText; }
                        if (new_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更新值"].Value = new_value.InnerText; }
                        if (old_value != null) { dataGridViewAttributeValueChange.Rows[index].Cells["对象变更旧值"].Value = old_value.InnerText; }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()+ rpcResponse);   //读取该节点的相关信息
            }
        }
        private void Pgnot(string rpcResponse, string ip)
        {
            try
            {
                if (!rpcResponse.Contains("pg-id"))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                string nename = "";
                string ipsname = "";
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                    {
                        nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                        ipsname = dataGridViewNeInformation.Rows[i].Cells["运营商"].Value.ToString();
                        break;
                    }
                }
                int index = 0;
                lock (LOCK)
                {
                    index = dataGridViewPGS_Not.Rows.Add();
                    dataGridViewPGS_Not.FirstDisplayedScrollingRowIndex = dataGridViewPGS_Not.Rows.Count - 1;
                }
               
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:notification:1.0");
                root.AddNamespace("pgsxmlns", "urn:ccsa:yang:acc-protection-group");
                XmlNode eventTime = xmlDoc.SelectSingleNode("/rpc:notification/rpc:eventTime", root);
                if (eventTime != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组事件时间"].Value = eventTime.InnerText; }
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//pgsxmlns:pg", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                {
                  
                    if (ipsname.Contains("移动"))
                    {
                        XmlNode protection_switch_serial_no = itemNode.SelectSingleNode("//pgsxmlns:protection-switch-serial-no", root);
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-direction", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (protection_switch_serial_no != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组事件编号"].Value = protection_switch_serial_no.InnerText; }
                        if (pg_id != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组IDN"].Value = pg_id.InnerText;
                            dataGridViewPGS_Not.Rows[index].Cells["保护组网元"].Value = nename;
                        }
                        if (protection_type != null) { dataGridViewPGS_Not.Rows[index].Cells["保护类型N"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS_Not.Rows[index].Cells["还原模式N"].Value = reversion_mode.InnerText; }
                        if (switch_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换类型N"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS_Not.Rows[index].Cells["SDN"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS_Not.Rows[index].Cells["WTRN"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS_Not.Rows[index].Cells["HoldOffN"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["主要端口N"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["次要端口N"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换原因N"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["保护方向N"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS_Not.Rows[index].Cells["选择端口N"].Value = selected_port.InnerText; }
                    }
                    if (ipsname.Contains("电信"))
                    {
                        XmlNode protection_switch_serial_no = itemNode.SelectSingleNode("//pgsxmlns:protection-switch-serial-no", root);
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-type", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (protection_switch_serial_no != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组事件编号"].Value = protection_switch_serial_no.InnerText; }
                        if (pg_id != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组IDN"].Value = pg_id.InnerText;
                            dataGridViewPGS_Not.Rows[index].Cells["保护组网元"].Value = nename;
                        }
                        if (protection_type != null) { dataGridViewPGS_Not.Rows[index].Cells["保护类型N"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS_Not.Rows[index].Cells["还原模式N"].Value = reversion_mode.InnerText; }
                        if (switch_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换类型N"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS_Not.Rows[index].Cells["SDN"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS_Not.Rows[index].Cells["WTRN"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS_Not.Rows[index].Cells["HoldOffN"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["主要端口N"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["次要端口N"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换原因N"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["保护方向N"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS_Not.Rows[index].Cells["选择端口N"].Value = selected_port.InnerText; }
                    }
                    if (ipsname.Contains("联通"))
                    {
                        XmlNode protection_switch_serial_no = itemNode.SelectSingleNode("//pgsxmlns:protection-switch-serial-no", root);
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-type", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (protection_switch_serial_no != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组事件编号"].Value = protection_switch_serial_no.InnerText; }
                        if (pg_id != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组IDN"].Value = pg_id.InnerText;
                            dataGridViewPGS_Not.Rows[index].Cells["保护组网元"].Value = nename;
                        }
                        if (protection_type != null) { dataGridViewPGS_Not.Rows[index].Cells["保护类型N"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS_Not.Rows[index].Cells["还原模式N"].Value = reversion_mode.InnerText; }
                        if (switch_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换类型N"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS_Not.Rows[index].Cells["SDN"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS_Not.Rows[index].Cells["WTRN"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS_Not.Rows[index].Cells["HoldOffN"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["主要端口N"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS_Not.Rows[index].Cells["次要端口N"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS_Not.Rows[index].Cells["倒换原因N"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["保护方向N"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS_Not.Rows[index].Cells["选择端口N"].Value = selected_port.InnerText; }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void AlarmNotfication(string rpcResponse, string ip)
        {
            try
            {
                if (!rpcResponse.Contains("alarm-notification") && !rpcResponse.Contains("tca-notification"))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                string nename = "";
                string ipsname = "";
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                    {
                        nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                        ipsname = dataGridViewNeInformation.Rows[i].Cells["运营商"].Value.ToString();
                        break;
                    }
                }
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("xmlns_name", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//xmlns_name:alarm", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                {

                    if (ipsname.Contains("电信"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:alarm-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("xmlns_name:alarm-code", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:alarm-state", root);
                        XmlNode perceived_severity = itemNode.SelectSingleNode("xmlns_name:perceived-severity", root);
                        XmlNode additional_text = itemNode.SelectSingleNode("xmlns_name:additional-text", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null) {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                {
                                    if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == alarm_serial_no.InnerText && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename) 
                                    {
                                        dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                        if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                        break;
                                    }
                                }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;

                                    if (perceived_severity != null)
                                    {
                                        dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                        switch (perceived_severity.InnerText)
                                        {
                                            case "critical":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                                break;
                                            case "major":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                                break;
                                            case "minor":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                                break;
                                            case "warning":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                                break;
                                        }
                                    }
                                    if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }
                                
                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (perceived_severity != null)
                                {
                                    dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                    switch (perceived_severity.InnerText)
                                    {
                                        case "critical":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                            break;
                                        case "major":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                            break;
                                        case "minor":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                            break;
                                        case "warning":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                            break;
                                    }
                                }
                                if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                    if (ipsname.Contains("移动"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:alarm-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("xmlns_name:alarm-code", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:alarm-state", root);
                        XmlNode perceived_severity = itemNode.SelectSingleNode("xmlns_name:perceived-severity", root);
                        XmlNode additional_text = itemNode.SelectSingleNode("xmlns_name:additional-text", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null)
                        {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                    for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                    {
                                        if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == (int.Parse(alarm_serial_no.InnerText)-1).ToString() && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename) //keyword要查的关键字
                                        {
                                            dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                            if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                            break;
                                        }
                                    }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;

                                    if (perceived_severity != null)
                                    {
                                        dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                        switch (perceived_severity.InnerText)
                                        {
                                            case "critical":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                                break;
                                            case "major":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                                break;
                                            case "minor":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                                break;
                                            case "warning":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                                break;
                                        }
                                    }
                                    if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }

                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (perceived_severity != null)
                                {
                                    dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                    switch (perceived_severity.InnerText)
                                    {
                                        case "critical":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                            break;
                                        case "major":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                            break;
                                        case "minor":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                            break;
                                        case "warning":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                            break;
                                    }
                                }
                                if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                    if (ipsname.Contains("联通"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:alarm-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("xmlns_name:alarm-code", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:alarm-state", root);
                        XmlNode perceived_severity = itemNode.SelectSingleNode("xmlns_name:perceived-severity", root);
                        XmlNode additional_text = itemNode.SelectSingleNode("xmlns_name:additional-text", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null)
                        {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                    for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                    {
                                        if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == (int.Parse(alarm_serial_no.InnerText) - 1).ToString() && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename) //keyword要查的关键字
                                        {
                                            dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                            if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                            break;
                                        }
                                    }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;

                                    if (perceived_severity != null)
                                    {
                                        dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                        switch (perceived_severity.InnerText)
                                        {
                                            case "critical":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                                break;
                                            case "major":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                                break;
                                            case "minor":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                                break;
                                            case "warning":
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                                break;
                                        }
                                    }
                                    if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }

                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (perceived_severity != null)
                                {
                                    dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                                    switch (perceived_severity.InnerText)
                                    {
                                        case "critical":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                            break;
                                        case "major":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                            break;
                                        case "minor":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                            break;
                                        case "warning":
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                            dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                            break;
                                    }
                                }
                                if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                }
                itemNodes = xmlDoc.SelectNodes("//xmlns_name:tca", root);
                lock (LOCK)
                    foreach (XmlNode itemNode in itemNodes)
                {

                    if (ipsname.Contains("电信"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:tca-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("//xmlns_name:pm-parameter-name", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:tca-state", root);
                        XmlNode granularity = itemNode.SelectSingleNode("//xmlns_name:granularity", root);
                        XmlNode threshold_type = itemNode.SelectSingleNode("//xmlns_name:threshold-type", root);
                        XmlNode threshold_value = itemNode.SelectSingleNode("//xmlns_name:threshold-value", root);
                        XmlNode current_value = itemNode.SelectSingleNode("xmlns_name:current-value", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null)
                        {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                    for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                    {
                                        if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == alarm_serial_no.InnerText && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename)
                                        {
                                            dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                            if (object_name != null) { dataGridViewAlarm.Rows[i].Cells["告警对象名称"].Value = object_name.InnerText; }
                                            if (object_type != null) { dataGridViewAlarm.Rows[i].Cells["告警对象类型"].Value = object_type.InnerText; }
                                            if (alarm_code != null) { dataGridViewAlarm.Rows[i].Cells["告警名称"].Value = alarm_code.InnerText; }
                                            if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            if (granularity != null) { dataGridViewAlarm.Rows[i].Cells["TCA周期"].Value = granularity.InnerText; }
                                            if (threshold_type != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                            if (threshold_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                            if (current_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA当前值"].Value = current_value.InnerText; }
                                            if (start_time != null) { dataGridViewAlarm.Rows[i].Cells["告警开始时间"].Value = start_time.InnerText; }
                                            if (end_time != null) { dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                            break;
                                        }
                                    }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                    if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                    if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                    if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                    if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }

                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                    if (ipsname.Contains("移动"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:tca-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("//xmlns_name:pm-parameter-name", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:tca-state", root);
                        XmlNode granularity = itemNode.SelectSingleNode("//xmlns_name:granularity", root);
                        XmlNode threshold_type = itemNode.SelectSingleNode("//xmlns_name:threshold-type", root);
                        XmlNode threshold_value = itemNode.SelectSingleNode("//xmlns_name:threshold-value", root);
                        XmlNode current_value = itemNode.SelectSingleNode("xmlns_name:current-value", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null)
                        {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                    for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                    {
                                        if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == (int.Parse(alarm_serial_no.InnerText) - 1).ToString() && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename) //keyword要查的关键字
                                        {
                                            dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                            if (object_name != null) { dataGridViewAlarm.Rows[i].Cells["告警对象名称"].Value = object_name.InnerText; }
                                            if (object_type != null) { dataGridViewAlarm.Rows[i].Cells["告警对象类型"].Value = object_type.InnerText; }
                                            if (alarm_code != null) { dataGridViewAlarm.Rows[i].Cells["告警名称"].Value = alarm_code.InnerText; }
                                            if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            if (granularity != null) { dataGridViewAlarm.Rows[i].Cells["TCA周期"].Value = granularity.InnerText; }
                                            if (threshold_type != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                            if (threshold_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                            if (current_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA当前值"].Value = current_value.InnerText; }
                                            if (start_time != null) { dataGridViewAlarm.Rows[i].Cells["告警开始时间"].Value = start_time.InnerText; }
                                            if (end_time != null) { dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                            break;
                                        }
                                    }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;

                                    if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                    if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                    if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                    if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }

                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                    if (ipsname.Contains("联通"))
                    {
                        XmlNode alarm_serial_no = itemNode.SelectSingleNode("xmlns_name:tca-serial-no", root);
                        XmlNode object_name = itemNode.SelectSingleNode("//xmlns_name:object-name", root);
                        XmlNode object_type = itemNode.SelectSingleNode("//xmlns_name:object-type", root);
                        XmlNode alarm_code = itemNode.SelectSingleNode("//xmlns_name:pm-parameter-name", root);
                        XmlNode alarm_state = itemNode.SelectSingleNode("xmlns_name:tca-state", root);
                        XmlNode granularity = itemNode.SelectSingleNode("//xmlns_name:granularity", root);
                        XmlNode threshold_type = itemNode.SelectSingleNode("//xmlns_name:threshold-type", root);
                        XmlNode threshold_value = itemNode.SelectSingleNode("//xmlns_name:threshold-value", root);
                        XmlNode current_value = itemNode.SelectSingleNode("xmlns_name:current-value", root);
                        XmlNode start_time = itemNode.SelectSingleNode("xmlns_name:start-time", root);
                        XmlNode end_time = itemNode.SelectSingleNode("xmlns_name:end-time", root);
                        bool alarmFind = false;
                        if (alarm_serial_no != null)
                        {
                            if (end_time != null)
                            {
                                if (dataGridViewAlarm.Rows.Count != 1)
                                    for (int i = 0; i < dataGridViewAlarm.Rows.Count; i++)
                                    {
                                        if (dataGridViewAlarm.Rows[i].Cells["告警编号"].Value.ToString() == (int.Parse(alarm_serial_no.InnerText) - 1).ToString() && dataGridViewAlarm.Rows[i].Cells["告警网元"].Value.ToString() == nename) //keyword要查的关键字
                                        {
                                            dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText;
                                            if (object_name != null) { dataGridViewAlarm.Rows[i].Cells["告警对象名称"].Value = object_name.InnerText; }
                                            if (object_type != null) { dataGridViewAlarm.Rows[i].Cells["告警对象类型"].Value = object_type.InnerText; }
                                            if (alarm_code != null) { dataGridViewAlarm.Rows[i].Cells["告警名称"].Value = alarm_code.InnerText; }
                                            if (alarm_state != null) { dataGridViewAlarm.Rows[i].Cells["告警状态"].Value = alarm_state.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                                            if (granularity != null) { dataGridViewAlarm.Rows[i].Cells["TCA周期"].Value = granularity.InnerText; }
                                            if (threshold_type != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                            if (threshold_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                            if (current_value != null) { dataGridViewAlarm.Rows[i].Cells["TCA当前值"].Value = current_value.InnerText; }
                                            if (start_time != null) { dataGridViewAlarm.Rows[i].Cells["告警开始时间"].Value = start_time.InnerText; }
                                            if (end_time != null) { dataGridViewAlarm.Rows[i].Cells["告警结束时间"].Value = end_time.InnerText; }
                                            dataGridViewAlarm.Rows[i].Cells["确认告警"].Value = "已清除";
                                            alarmFind = true;
                                            break;
                                        }
                                    }
                                if (alarmFind == false)
                                {
                                    int index = 0;
                                    lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                    dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                    dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;

                                    if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                    if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                    if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                    if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                                    dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                                }

                            }
                            else
                            {
                                int index = 0;
                                lock (LOCK) { index = dataGridViewAlarm.Rows.Add(); dataGridViewAlarm.FirstDisplayedScrollingRowIndex = dataGridViewAlarm.Rows.Count - 1; }
                                dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = nename;
                                dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText;
                                if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                                if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                                if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                                if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                                if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                                if (threshold_type != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText; }
                                if (threshold_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText; }
                                if (current_value != null) { dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText; }
                                if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                                dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        /// <summary>
        /// 订阅通知显示
        /// </summary>
        /// <param name="xmlDoc">传入进来的XML文件</param>
        private void ShowXML(string rpcResponse, int id) //显示xml数据
        {
            try
            {
                if (!rpcResponse.Contains("notification"))
                {
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rpcResponse);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true; //忽略xml文档中的注释
                XmlNode xn = xmlDoc.DocumentElement;
                XmlNodeList xnl = xn.ChildNodes;
                //循环遍历获取xml文档中的全部数据
                XmlElement xx = (XmlElement)xn;
                ListViewItem item = new ListViewItem(xx.Name);
                foreach (XmlNode Xnl in xnl)
                {
                    XmlNodeList xnl0 = Xnl.ChildNodes;
                    XmlElement xe1 = (XmlElement)Xnl;
                    item.SubItems.Add(xe1.Name);
                    int j = 0;
                    foreach (XmlNode Xn2 in xe1)
                    {
                        XmlNodeList xn11 = Xn2.ChildNodes;
                        int k = 0;
                        foreach (XmlNode Xn3 in xn11)
                        {
                            XmlNodeList xn12 = Xn3.ChildNodes;
                            for (int i = 0; i < xn12.Count; i++)
                            {
                                item.SubItems.Add(xn12.Item(i).InnerText);
                            }
                            k++;
                        }
                        if (k == 1)
                        {
                            for (int i = 0; i < xn11.Count; i++)
                            {
                                item.SubItems.Add(xn11.Item(i).InnerText);
                            }
                        }
                        j++;
                    }
                    if (j == 1 && xe1.Name == "eventTime")
                    {
                        for (int i = 0; i < xnl0.Count; i++)
                        {
                            item.SubItems.Add(xnl0.Item(i).InnerText);
                        }
                    }
                }
                //string tca = "tca-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(tca))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        if (alarmlog.SubItems[7].Text == "end")
                //        {
                //            int no = int.Parse(alarmlog.SubItems[0].Text) - 1;
                //            for (int i = 0; i < ListViewTcaAlarm.Items.Count; i++)
                //            {
                //                if (ListViewTcaAlarm.Items[i].SubItems[0].Text == no.ToString())
                //                {
                //                    ListViewTcaAlarm.Items.RemoveAt(i);
                //                    i--;
                //                }
                //            }
                //            alarmlog.SubItems.Add("已清除");
                //        }
                //        if (alarmlog.SubItems[7].Text == "start")
                //        {
                //            alarmlog.SubItems.Add("当前告警");
                //            alarmlog.SubItems.Add("未清除");
                //        }
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        ListViewTcaAlarm.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, tca));
                //        //mes.Start();
                //    }
                //}
                //string lldp = "lldp-change-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(lldp))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(1);
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        listViewLLDP.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, lldp));
                //        //mes.Start();
                //    }
                //}
                //string common = "common-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(common))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(1);
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        listViewCommon.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, common));
                //        //mes.Start();
                //    }
                //}
                //string peer = "peer-change-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(peer))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(1);
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        listViewPeer.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, peer));
                //        //mes.Start();
                //    }
                //}
                //string attribute = "attribute-value-change-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(attribute))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(1);
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        listViewAttribute.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, attribute));
                //        //mes.Start();
                //    }
                //}
                //string protection = "protection-switch-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(protection))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(0);
                //        alarmlog.SubItems.RemoveAt(1);
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        listViewProtection.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, protection));
                //        //mes.Start();
                //    }
                //}
                //string GHao = "g-hao-notification";
                //foreach (var type in item.SubItems)
                //{
                //    if (type.ToString().Contains(GHao))
                //    {
                //        ListViewItem alarmlog = (ListViewItem)item.Clone();
                //        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                //        {
                //            if (treeViewNEID.Nodes[i].Name == id.ToString())
                //            {
                //                alarmlog.SubItems.Add(treeViewNEID.Nodes[i].Text);
                //                break;
                //            }
                //        }
                //        //alarmlog.SubItems.RemoveAt(0);
                //        //alarmlog.SubItems.RemoveAt(0);
                //        //alarmlog.SubItems.RemoveAt(1);
                //        listViewGhao.Items.Add(alarmlog);
                //        //Thread mes = new Thread(() => CreatMesg(alarmlog, protection));
                //        //mes.Start();
                //    }
                //}

            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.ToString());
            }
        }
        private void CreatMesg(ListViewItem alarmlog, string title)
        {
            TextBox TextMesg = new TextBox();
            TextMesg.Name = "TextMesg";
            TextMesg.Size = new Size(100, 25);
            TextMesg.Multiline = true;
            TextMesg.Dock = System.Windows.Forms.DockStyle.Fill;
            Form Mesg = new Form();
            for (int i = 0; i < alarmlog.SubItems.Count; i++)
            {
                TextMesg.AppendText(alarmlog.SubItems[i].Text + "\r\n");
            }
            Mesg.Text = title;
            Mesg.Controls.Add(TextMesg);
            ////窗体飞入过程
            Mesg.ClientSize = new System.Drawing.Size(280, 180);
            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - Mesg.Width, Screen.PrimaryScreen.WorkingArea.Height);
            Mesg.PointToScreen(p);
            Mesg.Location = p;
            Mesg.ShowIcon = false;// 设置窗口的lcon是否显示
            Mesg.Show();
            Mesg.TopMost = true;
            Application.DoEvents();
            //Mesg.ShowDialog();
            for (int i = 0; i <= Mesg.Height; i++)
            {
                Mesg.Location = new Point(p.X, p.Y - i);
                Mesg.Refresh();//刷新窗体
                Thread.Sleep(2);
            }
            Thread.Sleep(10000);
            // Mesg.Close();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Form_Load(object sender, EventArgs e)
        {
            treeViewNEID.ImageList = imageListTree;
            ComTimeOut.SelectedIndex = 0;
            TextOduService_type.SelectedIndex = 0;
            Com_nni_protection_type.SelectedIndex = 0;
            Com_nni2_protection_type.SelectedIndex = 0;
            Com_uni_protection_type.SelectedIndex = 0;
            ComSdhPro.SelectedIndex = 0;
            Com_Eth_uni_protection_type.SelectedIndex = 0;
            //    ComOduNniTsDetailClient_UNI_A.SelectedIndex = 0;
            //   ComOduTsDetail_Primary_nni.SelectedIndex = 0;
            ComCreatConnection.SelectedIndex = 0;
            ComEthUniVlanAccessAction.SelectedIndex = 0;
            ComEthUniVlanPriority.SelectedIndex = 0;
            ComEthUniVlanType.SelectedIndex = 0;
            ComEthServiceType.SelectedIndex = 0;
            ComEthVlanAccessAction.SelectedIndex = 0;
            ComEthVlanPriority.SelectedIndex = 0;
            ComEthVlanType.SelectedIndex = 0;
            ComEthServiceMappingMode.SelectedIndex = 0;
            // ComEosSdhSignalType.SelectedIndex = 2;
            //  ComEosSdhSignalTypeProtect.SelectedIndex = 2;
            //   ComVCType.SelectedIndex = 2;
            ComTSD.SelectedIndex = 0;
            ComLCAS.SelectedIndex = 0;
            ComEthFtpVlanAccess_primary_nni.SelectedIndex = 2;
            ComEthFtpVlanPriority_primary_nni.SelectedIndex = 0;
            ComEthFtpVlanType_primary_nni.SelectedIndex = 1;
            ComSdhSer.SelectedIndex = 0;
            //   ComSdhUniSdhType.SelectedIndex = 2;
            ComSdhSerMap.SelectedIndex = 0;
            //   ComSdhNniSdhtype_A.SelectedIndex = 2;
            //   ComSdhNniVcType_A.SelectedIndex = 2;
            //    ComSdhNniTs_A.SelectedIndex = 0;
            //   ComSdhNniSdhtype_B.SelectedIndex = 2;
            //    ComSdhNniVcType_B.SelectedIndex = 2;
            //   ComSdhNniTs_B.SelectedIndex = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            Readini();
            XML_URL_COMBOX(XML_URL);

            if (File.Exists(neinfopath))
            {
                try
                {
                    dataGridViewNeInformation.Rows.Clear();
                    //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                    // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(neinfopath);
                    XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//ipaddress");
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        int index = dataGridViewNeInformation.Rows.Add();
                        XmlNode neip = itemNode.SelectSingleNode("ip", root);
                        XmlNode user = itemNode.SelectSingleNode("user", root);
                        XmlNode password = itemNode.SelectSingleNode("password", root);
                        XmlNode neid = itemNode.SelectSingleNode("id", root);
                        XmlNode nename = itemNode.SelectSingleNode("name", root);
                        XmlNode neips = itemNode.SelectSingleNode("ips", root);
                        if (neip != null) { dataGridViewNeInformation.Rows[index].Cells["网元ip"].Value = neip.InnerText; }
                        if (user != null) { dataGridViewNeInformation.Rows[index].Cells["用户名"].Value = user.InnerText; }
                        if (password != null) { dataGridViewNeInformation.Rows[index].Cells["密码"].Value = password.InnerText; }
                        if (neid != null) { dataGridViewNeInformation.Rows[index].Cells["SSH_ID"].Value = neid.InnerText; }
                        if (nename != null) { dataGridViewNeInformation.Rows[index].Cells["网元名称"].Value = nename.InnerText; }
                        if (neips != null) { dataGridViewNeInformation.Rows[index].Cells["运营商"].Value = neips.InnerText; }
                        TreeNode node = new TreeNode();
                        node.Tag = neid.InnerText;
                        node.Name = neid.InnerText;
                        node.Text = nename.InnerText;
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 4;
                        treeViewNEID.Nodes.Add(node);
                        for (int i = 0; i < this.dataGridViewNeInformation.Columns.Count; i++)
                        {
                            this.dataGridViewNeInformation.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridView_EQ.Columns.Count; i++)
                        {
                            this.dataGridView_EQ.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewAuto.Columns.Count; i++)
                        {
                            this.dataGridViewAuto.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewCurrentPerformance.Columns.Count; i++)
                        {
                            this.dataGridViewCurrentPerformance.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewPGS.Columns.Count; i++)
                        {
                            this.dataGridViewPGS.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewEth.Columns.Count; i++)
                        {
                            this.dataGridViewEth.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewAlarm.Columns.Count; i++)
                        {
                            this.dataGridViewAlarm.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewPGS_Not.Columns.Count; i++)
                        {
                            this.dataGridViewPGS_Not.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewAttributeValueChange.Columns.Count; i++)
                        {
                            this.dataGridViewAttributeValueChange.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewLLDP.Columns.Count; i++)
                        {
                            this.dataGridViewLLDP.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        for (int i = 0; i < this.dataGridViewPeerChange.Columns.Count; i++)
                        {
                            this.dataGridViewPeerChange.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    // Console.Read();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                }
                加载所有运营商YIN文件ToolStripMenuItem.PerformClick();
            }
        }
        /// <summary>
        /// 发送消息等待时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComTimeOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeViewNEID.SelectedNode != null)
            {
                ////
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                if (netConfClient[id] != null)
                {
                    if (netConfClient[id].IsConnected)
                    {
                        if (ComTimeOut.Text == "-1")
                        {
                            netConfClient[id].TimeOut = -1;
                        }
                        else
                        {
                            netConfClient[id].TimeOut = int.Parse(ComTimeOut.Text) * 1000;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 按照ctrl+S保存XML脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fastColoredTextBoxReq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                this.保存ToolStripMenuItem.PerformClick();
            }
        }
        /// <summary>
        /// 获取设备上所有的XML文件，最长只等待60秒
        /// </summary>
        private void GetXmlAll(int id, string ip)
        {
            try
            {
                上载全部XMLToolStripMenuItem.Enabled = false;
                XmlDocument doc = new XmlDocument();
                doc = Sendrpc(GetXML.FindGetAll(), id, ip);
                doc.Save(@"C:\netconf\" + ip + "_XmlAll.xml");
                MessageBox.Show("上载成功！");
                上载全部XMLToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                上载全部XMLToolStripMenuItem.Enabled = true;
            }
        }
        /// <summary>
        /// 查询网元信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButFindNeinfo_Click(object sender, EventArgs e)
        {
            try
            {
                textBox_me_name.Text = "";
                textBox_me_status.Text = "";
                textBox_me_product_name.Text = "";
                textBox_me_device_type.Text = "";
                textBox_me_uuid.Text = "";
                textBox_me_manufacturer.Text = "";
                textBox_me_hardware_version.Text = "";
                textBox_me_software_version.Text = "";
                textBox_me_protocol_name.Text = "";
                textBox_me_eq.Text = "";
                textBox_me_ip_address.Text = "";
                textBox_me_mask.Text = "";
                textBox_me_gate_way1.Text = "";
                textBox_me_gate_way2.Text = "";
                textBox_me_ntp_enable.Text = "";
                textBox_me_ntp_server_name.Text = "";
                textBox_me_ntp_server_ipaddress.Text = "";
                textBox_me_ntp_server_port.Text = "";
                textBox_me_ntp_server_version.Text = "";
                textBox_me_ntp_state.Text = "";
                textBoxallslot.Text = "";
                textBoxsn.Text = "";
                comboBoxMcPort.Items.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.ME(), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("me", "urn:ccsa:yang:acc-devm");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//me:me", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("me:name", root);
                    XmlNode status = itemNode.SelectSingleNode("me:status", root);
                    XmlNode ip_address = itemNode.SelectSingleNode("me:ip-address", root);
                    XmlNode mask = itemNode.SelectSingleNode("me:mask", root);
                    XmlNode ntp_enable = itemNode.SelectSingleNode("me:ntp-enable", root);
                    XmlNode gate_way1 = itemNode.SelectSingleNode("me:gate-way1", root);
                    XmlNode uuid = itemNode.SelectSingleNode("me:uuid", root);
                    XmlNode manufacturer = itemNode.SelectSingleNode("me:manufacturer", root);
                    XmlNode product_name = itemNode.SelectSingleNode("me:product-name", root);
                    XmlNode software_version = itemNode.SelectSingleNode("me:software-version", root);
                    XmlNode hardware_version = itemNode.SelectSingleNode("me:hardware-version", root);
                    XmlNode device_type = itemNode.SelectSingleNode("me:device-type", root);
                    XmlNode layer_protocol_nameeth = itemNode.SelectSingleNode("me:layer-protocol-name[1]/text()[1]", root);
                    XmlNode layer_protocol_namesdh = itemNode.SelectSingleNode("me:layer-protocol-name[2]/text()[1]", root);
                    XmlNode layer_protocol_nameotn = itemNode.SelectSingleNode("me:layer-protocol-name[3]/text()[1]", root);
                    XmlNode layer_protocol_nameosu = itemNode.SelectSingleNode("me:layer-protocol-name[4]/text()[1]", root);
                    XmlNodeList eq = itemNode.SelectNodes("me:eq", root);
                    XmlNode ntp_state = itemNode.SelectSingleNode("me:ntp-state", root);
                    XmlNode total_slot_number = itemNode.SelectSingleNode("me:total-slot-number", root);
                    XmlNode serial_number = itemNode.SelectSingleNode("me:serial-number", root);

                    string layer_protocol_name_eth = "";
                    string layer_protocol_name_sdh = "";
                    string layer_protocol_name_otn = "";
                    string layer_protocol_name_osu = "";
                    if (layer_protocol_nameeth != null) layer_protocol_name_eth = layer_protocol_nameeth.InnerText;
                    if (layer_protocol_namesdh != null) layer_protocol_name_sdh = layer_protocol_namesdh.InnerText;
                    if (layer_protocol_nameotn != null) layer_protocol_name_otn = layer_protocol_nameotn.InnerText;
                    if (layer_protocol_nameosu != null) layer_protocol_name_osu = layer_protocol_nameosu.InnerText;
                    string layer_protocol_name_all = layer_protocol_name_eth + layer_protocol_name_sdh + layer_protocol_name_otn + layer_protocol_name_osu;
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-eth:", "");
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-sdh", "");
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-otn", "");
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-osu", "");
                    if (name != null) textBox_me_name.Text = name.InnerText;
                    if (status != null) textBox_me_status.Text = status.InnerText;
                    if (ip_address != null) textBox_me_ip_address.Text = ip_address.InnerText;
                    if (mask != null) textBox_me_mask.Text = mask.InnerText;
                    if (ntp_state != null) textBox_me_ntp_state.Text = ntp_state.InnerText;
                    if (ntp_enable != null) textBox_me_ntp_enable.Text = ntp_enable.InnerText;
                    if (gate_way1 != null) textBox_me_gate_way1.Text = gate_way1.InnerText;
                    if (uuid != null) textBox_me_uuid.Text = uuid.InnerText;
                    if (manufacturer != null) textBox_me_manufacturer.Text = manufacturer.InnerText;
                    if (serial_number != null) textBoxsn.Text = serial_number.InnerText;

                    if (product_name != null) textBox_me_product_name.Text = product_name.InnerText;
                    if (software_version != null) textBox_me_software_version.Text = software_version.InnerText;
                    if (hardware_version != null) textBox_me_hardware_version.Text = hardware_version.InnerText;
                    if (device_type != null) textBox_me_device_type.Text = device_type.InnerText;
                    if (layer_protocol_name_all != null) textBox_me_protocol_name.Text = layer_protocol_name_all;
                    if (total_slot_number != null) textBoxallslot.Text = total_slot_number.InnerText;
                    if (eq != null) textBox_me_eq.Text = eq.Count.ToString();
                    XmlNodeList mc_port = itemNode.SelectNodes("me:mc-port", root);
                    foreach (XmlNode itemNode1 in mc_port)
                    {
                      
                        comboBoxMcPort.Items.Add(itemNode1.InnerText);
                        comboBoxMcPort.SelectedIndex = 0;
                    }
                    XmlNodeList net = itemNode.SelectNodes("//me:ntp-server", root);
                    foreach (XmlNode itemNode1 in net)
                    {
                        XmlNode ntpname = itemNode1.SelectSingleNode("me:name", root);
                        XmlNode ntpip = itemNode1.SelectSingleNode("me:ip-address", root);
                        XmlNode ntpport = itemNode1.SelectSingleNode("me:port", root);
                        XmlNode ntpversion = itemNode1.SelectSingleNode("me:ntp-version", root);
                        if (ntpname != null) textBox_me_ntp_server_name.Text = ntpname.InnerText;
                        if (ntpip != null) textBox_me_ntp_server_ipaddress.Text = ntpip.InnerText;
                        if (ntpport != null) textBox_me_ntp_server_port.Text = ntpport.InnerText;
                        if (ntpversion != null) textBox_me_ntp_server_version.Text = ntpversion.InnerText;
                    }
                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        /// <summary>
        /// 查询板卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButFindEq_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView_EQ.Rows.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.EQS(), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("eqsxmlns", "urn:ccsa:yang:acc-devm");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//eqsxmlns:eqs//eqsxmlns:eq", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    if (ips.Contains("移动")) {
                        XmlNode name = itemNode.SelectSingleNode("eqsxmlns:name", root);

                        XmlNode woking_date = itemNode.SelectSingleNode("eqsxmlns:working-state", root);
                        XmlNode plug_sate = itemNode.SelectSingleNode("eqsxmlns:plug-state", root);
                        XmlNode eq_type = itemNode.SelectSingleNode("eqsxmlns:eq-type[1]", root);
                        XmlNode eq_type2 = itemNode.SelectSingleNode("eqsxmlns:eq-type[2]", root);
                        XmlNode xc_capability = itemNode.SelectSingleNode("eqsxmlns:xc-capability", root);
                        XmlNode software_version = itemNode.SelectSingleNode("eqsxmlns:software-version", root);
                        XmlNode hardware_version = itemNode.SelectSingleNode("eqsxmlns:hardware-version", root);
                        XmlNode model = itemNode.SelectSingleNode("eqsxmlns:model", root);
                        XmlNode eq_type3 = itemNode.SelectSingleNode("eqsxmlns:eq-type[3]", root);

                        int index = dataGridView_EQ.Rows.Add();
                        if (model != null) { dataGridView_EQ.Rows[index].Cells["单板信息"].Value = model.InnerText; }
                        if (name != null) { dataGridView_EQ.Rows[index].Cells["单板名称"].Value = name.InnerText; }

                        if (woking_date != null) { dataGridView_EQ.Rows[index].Cells["状态"].Value = woking_date.InnerText; }
                        if (plug_sate != null) { dataGridView_EQ.Rows[index].Cells["是否在位"].Value = plug_sate.InnerText; }
                        if (eq_type != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText;
                        if (eq_type2 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText;
                        if (eq_type3 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText + "和" + eq_type3.InnerText;

                        if (xc_capability != null) { dataGridView_EQ.Rows[index].Cells["XC能力"].Value = xc_capability.InnerText; }
                        if (software_version != null) { dataGridView_EQ.Rows[index].Cells["软件版本"].Value = software_version.InnerText; }
                        if (hardware_version != null) { dataGridView_EQ.Rows[index].Cells["硬件版本"].Value = hardware_version.InnerText; }
                        XmlNodeList net = itemNode.SelectNodes("eqsxmlns:ptp", root);
                        if (net != null) { dataGridView_EQ.Rows[index].Cells["端口数量"].Value = net.Count.ToString(); }
                    }
                    if (ips.Contains("电信"))
                    {
                        XmlNode name = itemNode.SelectSingleNode("eqsxmlns:name", root);
                        XmlNode woking_date = itemNode.SelectSingleNode("eqsxmlns:working-state", root);
                        XmlNode plug_sate = itemNode.SelectSingleNode("eqsxmlns:plug-state", root);
                        XmlNode serial_number = itemNode.SelectSingleNode("eqsxmlns:serial-number", root);
                        XmlNode eq_type = itemNode.SelectSingleNode("eqsxmlns:eq-type[1]", root);
                        XmlNode eq_type2 = itemNode.SelectSingleNode("eqsxmlns:eq-type[2]", root);
                        XmlNode xc_capability = itemNode.SelectSingleNode("eqsxmlns:xc-capability", root);
                        XmlNode software_version = itemNode.SelectSingleNode("eqsxmlns:software-version", root);
                        XmlNode hardware_version = itemNode.SelectSingleNode("eqsxmlns:hardware-version", root);
                        XmlNode model = itemNode.SelectSingleNode("eqsxmlns:model", root);
                        XmlNode eq_type3 = itemNode.SelectSingleNode("eqsxmlns:eq-type[3]", root);

                        int index = dataGridView_EQ.Rows.Add();
                        if (model != null) { dataGridView_EQ.Rows[index].Cells["单板信息"].Value = model.InnerText; }
                        if (name != null) { dataGridView_EQ.Rows[index].Cells["单板名称"].Value = name.InnerText; }
                        if (woking_date != null) { dataGridView_EQ.Rows[index].Cells["状态"].Value = woking_date.InnerText; }
                        if (plug_sate != null) { dataGridView_EQ.Rows[index].Cells["是否在位"].Value = plug_sate.InnerText; }
                        if (eq_type != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText;
                        if (eq_type2 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText;
                        if (eq_type3 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText + "和" + eq_type3.InnerText;
                        if (xc_capability != null) { dataGridView_EQ.Rows[index].Cells["XC能力"].Value = xc_capability.InnerText; }
                        if (serial_number != null) { dataGridView_EQ.Rows[index].Cells["序列号"].Value = serial_number.InnerText; }

                        if (software_version != null) { dataGridView_EQ.Rows[index].Cells["软件版本"].Value = software_version.InnerText; }
                        if (hardware_version != null) { dataGridView_EQ.Rows[index].Cells["硬件版本"].Value = hardware_version.InnerText; }
                        XmlNodeList net = itemNode.SelectNodes("eqsxmlns:ptp", root);
                        if (net != null) { dataGridView_EQ.Rows[index].Cells["端口数量"].Value = net.Count.ToString(); }
                    }
                    if (ips.Contains("联通"))
                    {
                        XmlNode name = itemNode.SelectSingleNode("eqsxmlns:name", root);
                        XmlNode woking_date = itemNode.SelectSingleNode("eqsxmlns:eq-state", root);
                        XmlNode plug_sate = itemNode.SelectSingleNode("eqsxmlns:plug-state", root);
                        XmlNode eq_type = itemNode.SelectSingleNode("eqsxmlns:eq-type[1]", root);
                        XmlNode eq_type2 = itemNode.SelectSingleNode("eqsxmlns:eq-type[2]", root);
                        XmlNode eq_sn = itemNode.SelectSingleNode("eqsxmlns:eq-sn", root);
                        XmlNode software_version = itemNode.SelectSingleNode("eqsxmlns:software-version", root);
                        XmlNode hardware_version = itemNode.SelectSingleNode("eqsxmlns:hardware-version", root);
                        XmlNode model = itemNode.SelectSingleNode("eqsxmlns:model", root);
                        XmlNode eq_type3 = itemNode.SelectSingleNode("eqsxmlns:eq-type[3]", root);

                        int index = dataGridView_EQ.Rows.Add();
                        if (model != null) { dataGridView_EQ.Rows[index].Cells["单板信息"].Value = model.InnerText; }
                        if (name != null) { dataGridView_EQ.Rows[index].Cells["单板名称"].Value = name.InnerText; }
                        if (woking_date != null) { dataGridView_EQ.Rows[index].Cells["状态"].Value = woking_date.InnerText; }
                        if (plug_sate != null) { dataGridView_EQ.Rows[index].Cells["是否在位"].Value = plug_sate.InnerText; }
                        if (eq_type != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText;
                        if (eq_type2 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText;
                        if (eq_type3 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和" + eq_type2.InnerText + "和" + eq_type3.InnerText;

                        if (eq_sn != null) { dataGridView_EQ.Rows[index].Cells["序列号"].Value = eq_sn.InnerText; }
                        if (software_version != null) { dataGridView_EQ.Rows[index].Cells["软件版本"].Value = software_version.InnerText; }
                        if (hardware_version != null) { dataGridView_EQ.Rows[index].Cells["硬件版本"].Value = hardware_version.InnerText; }
                        XmlNodeList net = itemNode.SelectNodes("eqsxmlns:ptp", root);
                        if (net != null) { dataGridView_EQ.Rows[index].Cells["端口数量"].Value = net.Count.ToString(); }
                    }
                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        /// <summary>
        /// 查询ODU端口信息显示到创建业务页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButOduFind_Click(object sender, EventArgs e)
        {
            try
            {
                ComClientSideNni_UNI.Items.Clear();
                ComClientSideNni_UNI_second.Items.Clear();
                ComODUPtpNamesecondary_nni2.Items.Clear();
                comODUPtpNamePrimary_nni2.Items.Clear();
                comODUPtpNamePrimary_nni.Items.Clear();
                ComODUPtpNamesecondary_nni.Items.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                //// XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, false, false), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                ComClientSideNni_UNI.Items.Add("无");
                ComClientSideNni_UNI_second.Items.Add("无");

                ComODUPtpNamesecondary_nni2.Items.Add("无");
                comODUPtpNamePrimary_nni2.Items.Add("无");
                ComODUPtpNamesecondary_nni.Items.Add("无");
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText.Contains("ODU")||layer_protocol_name.InnerText.Contains("OSU"))
                        {
                            if (name != null)
                            {
                                if (interface_type.InnerText.Contains("NNI"))
                                {
                                    comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                                    comODUPtpNamePrimary_nni.SelectedIndex = 0;
                                    ComODUPtpNamesecondary_nni.Items.Add(name.InnerText);
                                    ComODUPtpNamesecondary_nni.SelectedIndex = 0;
                                    ComODUPtpNamesecondary_nni2.Items.Add(name.InnerText);
                                    ComODUPtpNamesecondary_nni2.SelectedIndex = 0;
                                    comODUPtpNamePrimary_nni2.Items.Add(name.InnerText);
                                    comODUPtpNamePrimary_nni2.SelectedIndex = 0;
                                }
                                if (interface_type.InnerText.Contains("UNI"))
                                {
                                    ComClientSideNni_UNI.Items.Add(name.InnerText);
                                    ComClientSideNni_UNI.SelectedIndex = 0;
                                    ComClientSideNni_UNI_second.Items.Add(name.InnerText);
                                    ComClientSideNni_UNI_second.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        /// <summary>
        /// 下拉框变更后自动更新内部支持能力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComClientSideNni_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComOduOduSignalType_UNI_A.Items.Clear();
                ComOduSwitchApability_UNI_A.Items.Clear();
                ComOduAdapataionType_UNI_A.Items.Clear();
                if (ComOduOduSignalType_UNI_A.Text == "无")
                    return;
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PTP(ComClientSideNni_UNI.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                root.AddNamespace("osuxmlns", "urn:ccsa:yang:acc-osu");

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText.Contains("ODU")|| layer_protocol_name.InnerText.Contains("OSU"))
                        {
                            if (name != null)
                            {
                                if (ComClientSideNni_UNI.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    if (layer_protocol_name.InnerText.Contains("OSU")){ itemNodesOduPtpPac = itemNode.SelectNodes("osuxmlns:osu-ptp-pac", root);}
                                        foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode ts_detail = itemNodeOdu.SelectSingleNode("oduxmlns:ts-detail", root);
                                        if (ts_detail != null) labelClientTs.Text = ts_detail.InnerText;
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);

                                        if (layer_protocol_name.InnerText.Contains("OSU")) { odu_signal_type = itemNodeOdu.SelectNodes("osuxmlns:osu-signal-type", root); }
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);
                                        if (odu_signal_type != null)
                                        {
                                            if (layer_protocol_name.InnerText.Contains("OSU")) {
                                                ComOduOduSignalType_UNI_A.Items.Clear();
                                                for (int i = 1; i <= odu_signal_type.Count; i++)
                                                {
                                                    XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("osuxmlns:osu-signal-type[" + i + "]", root);
                                                    string result_type = odu_signal_type1.InnerText;
                                                    if (result_type.Contains(":"))
                                                    {
                                                        result_type = result_type.Split(new char[] { ':' })[1];
                                                    }
                                                    ComOduOduSignalType_UNI_A.Items.Add(result_type);
                                                    ComOduOduSignalType_UNI_A.SelectedIndex = 0;
                                                }
                                            }
                                            if (layer_protocol_name.InnerText.Contains("ODU"))
                                            {
                                                ComOduOduSignalType_UNI_A.Items.Clear();
                                                for (int i = 1; i <= odu_signal_type.Count; i++)
                                                {
                                                    XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                    string result_type = odu_signal_type1.InnerText;
                                                    if (result_type.Contains(":"))
                                                    {
                                                        result_type = result_type.Split(new char[] { ':' })[1];
                                                    }
                                                    ComOduOduSignalType_UNI_A.Items.Add(result_type);
                                                    ComOduOduSignalType_UNI_A.SelectedIndex = 0;
                                                }
                                            }
                                           
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_UNI_A.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                string result_type = adaptation_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduAdapataionType_UNI_A.Items.Add(result_type);
                                                ComOduAdapataionType_UNI_A.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_UNI_A.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                string result_type = switch_capability1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduSwitchApability_UNI_A.Items.Add(result_type);
                                                ComOduSwitchApability_UNI_A.SelectedIndex = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ComOduNniPtpName_NNI_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComOduOduSignalType_Primary_nni.Text == "无")
                    return;
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PTP(comODUPtpNamePrimary_nni.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                if (comODUPtpNamePrimary_nni.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode ts_detail = itemNodeOdu.SelectSingleNode("oduxmlns:ts-detail", root);
                                        if (ts_detail != null) label_ts_primary_nni.Text = ts_detail.InnerText;
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);
                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_Primary_nni.Items.Clear();
                                            ComOduOduSignalType_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                string result_type = odu_signal_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduOduSignalType_Primary_nni.Items.Add(result_type);
                                                ComOduOduSignalType_Primary_nni.SelectedIndex = ComOduOduSignalType_Primary_nni.Items.Count - 1;
                                                ComOduOduSignalType_Secondary_nni.Items.Add(result_type);
                                                ComOduOduSignalType_Secondary_nni.SelectedIndex = ComOduOduSignalType_Secondary_nni.Items.Count - 1;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_Primary_nni.Items.Clear();
                                            ComOduAdapataionType_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                string result_type = adaptation_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduAdapataionType_Primary_nni.Items.Add(result_type);
                                                ComOduAdapataionType_Primary_nni.SelectedIndex = 0;
                                                ComOduAdapataionType_Secondary_nni.Items.Add(result_type);
                                                ComOduAdapataionType_Secondary_nni.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_Primary_nni.Items.Clear();
                                            ComOduSwitchApability_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                string result_type = switch_capability1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduSwitchApability_Primary_nni.Items.Add(result_type);
                                                ComOduSwitchApability_Primary_nni.SelectedIndex = 0;
                                                ComOduSwitchApability_Secondary_nni.Items.Add(result_type);
                                                ComOduSwitchApability_Secondary_nni.SelectedIndex = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ComOduNniPtpName_NNI_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComOduOduSignalType_Secondary_nni.Text == "无")
                    return;
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PTP(ComODUPtpNamesecondary_nni.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                if (ComODUPtpNamesecondary_nni.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode ts_detail = itemNodeOdu.SelectSingleNode("oduxmlns:ts-detail", root);
                                        if (ts_detail != null) label_ts_sec_nni.Text = ts_detail.InnerText;
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);
                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                string result_type = odu_signal_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduOduSignalType_Secondary_nni.Items.Add(result_type);
                                                ComOduOduSignalType_Secondary_nni.SelectedIndex = ComOduOduSignalType_Secondary_nni.Items.Count - 1;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                string result_type = adaptation_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduAdapataionType_Secondary_nni.Items.Add(result_type);
                                                ComOduAdapataionType_Secondary_nni.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_Secondary_nni.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                string result_type = switch_capability1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduSwitchApability_Secondary_nni.Items.Add(result_type);
                                                ComOduSwitchApability_Secondary_nni.SelectedIndex = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ComClientSideNni_UNI_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComOduOduSignalType_Secondary_nni2.Text == "无")
                    return;
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PTP(ComODUPtpNamesecondary_nni2.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                if (ComODUPtpNamesecondary_nni2.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode ts_detail = itemNodeOdu.SelectSingleNode("oduxmlns:ts-detail", root);
                                        if (ts_detail != null) label_ts_sec_nni2.Text = ts_detail.InnerText;
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);
                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_Secondary_nni2.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                string result_type = odu_signal_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduOduSignalType_Secondary_nni2.Items.Add(result_type);
                                                ComOduOduSignalType_Secondary_nni2.SelectedIndex = ComOduOduSignalType_Secondary_nni2.Items.Count - 1;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_Secondary_nni2.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                string result_type = adaptation_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduAdapataionType_Secondary_nni2.Items.Add(result_type);
                                                ComOduAdapataionType_Secondary_nni2.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_Secondary_nni2.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                string result_type = switch_capability1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduSwitchApability_Secondary_nni2.Items.Add(result_type);
                                                ComOduSwitchApability_Secondary_nni2.SelectedIndex = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        /// <summary>
        /// 透传业务创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButCreatODU_Click(object sender, EventArgs e)
        {
            
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            string ODUservicemode = comboBoxODUservicemode.Text;
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            string messg = Creat(CreateODU.Common(ODUservicemode,ips, TextOduLable.Text, TextOduService_type.Text, TextOduService_type.Text, TextOdusize.Text, Com_uni_protection_type.Text, Com_nni_protection_type.Text, Com_nni2_protection_type.Text,
                    ComClientSideNni_UNI.Text,ComClientSideNni_UNI_second.Text,ComOduNniTsDetailClient_UNI_A.Text, ComOduAdapataionType_UNI_A.Text, ComOduOduSignalType_UNI_A.Text, ComOduSwitchApability_UNI_A.Text,
                    comODUPtpNamePrimary_nni.Text, ComOduTsDetail_Primary_nni.Text, ComOduAdapataionType_Primary_nni.Text, ComOduOduSignalType_Primary_nni.Text, ComOduSwitchApability_Primary_nni.Text, ComOsuTpn_Primary_nni.Text,
                    ComODUPtpNamesecondary_nni.Text, ComOduTsDetail_Secondary_nni.Text, ComOduAdapataionType_Secondary_nni.Text, ComOduOduSignalType_Secondary_nni.Text, ComOduSwitchApability_Secondary_nni.Text, ComOsuTpn_secondary_nni.Text,
                    comODUPtpNamePrimary_nni2.Text, ComOduTsDetail_Primary_nni2.Text, ComOduAdapataionType_Primary_nni2.Text, ComOduOduSignalType_Primary_nni2.Text, ComOduSwitchApability_Primary_nni2.Text, ComOsuTpn_Primary_nni2.Text,
                    ComODUPtpNamesecondary_nni2.Text, ComOduTsDetail_Secondary_nni2.Text, ComOduAdapataionType_Secondary_nni2.Text, ComOduOduSignalType_Secondary_nni2.Text, ComOduSwitchApability_Secondary_nni2.Text, ComOsuTpn_secondary_nni2.Text


    ), id, ip);
            MessageBox.Show(messg);
        }
        private void ButFindOdu_local_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewEth.Rows.Clear();
                string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("connectionsxmlns", "urn:ccsa:yang:acc-connection");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//connectionsxmlns:connections//connectionsxmlns:connection", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewEth.Rows.Add();
                    XmlNode name = itemNode.SelectSingleNode("connectionsxmlns:name", root);
                    XmlNode label = itemNode.SelectSingleNode("connectionsxmlns:label", root);
                    XmlNode operational_state = itemNode.SelectSingleNode("connectionsxmlns:state-pac/connectionsxmlns:operational-state", root);
                    XmlNode admin_stste = itemNode.SelectSingleNode("connectionsxmlns:state-pac/connectionsxmlns:admin-state", root);
                    XmlNode total_size = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:total-size", root);
                    XmlNode cir = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:cir", root);
                    XmlNode pir = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:pir", root);
                    XmlNode cbs = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:cbs", root);
                    XmlNode pbs = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:pbs", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("connectionsxmlns:layer-protocol-name", root);
                    XmlNode service_type = itemNode.SelectSingleNode("connectionsxmlns:service-type", root);
                    if (name != null) { dataGridViewEth.Rows[index].Cells["连接名称"].Value = name.InnerText; }
                    if (label != null) { dataGridViewEth.Rows[index].Cells["标签别名"].Value = label.InnerText; }
                    if (operational_state != null) { dataGridViewEth.Rows[index].Cells["当前状态"].Value = operational_state.InnerText; }
                    if (admin_stste != null) { dataGridViewEth.Rows[index].Cells["管理状态"].Value = admin_stste.InnerText; }
                    if (total_size != null) { dataGridViewEth.Rows[index].Cells["业务总带宽"].Value = total_size.InnerText; }
                    if (cir != null) { dataGridViewEth.Rows[index].Cells["承诺带宽"].Value = cir.InnerText; }
                    if (pir != null) { dataGridViewEth.Rows[index].Cells["峰值带宽"].Value = pir.InnerText; }
                    if (cbs != null) { dataGridViewEth.Rows[index].Cells["承诺突发"].Value = cbs.InnerText; }
                    if (pbs != null) { dataGridViewEth.Rows[index].Cells["峰值突发"].Value = pbs.InnerText; }
                    if (layer_protocol_name != null) { dataGridViewEth.Rows[index].Cells["当前层协议"].Value = layer_protocol_name.InnerText; }
                    if (service_type != null) { dataGridViewEth.Rows[index].Cells["服务类型"].Value = service_type.InnerText; }
                    XmlNodeList ctp = itemNode.SelectNodes("connectionsxmlns:ctp", root);
                    string CTPAll = "";
                    if (ctp != null)
                    {
                        for (int i = 1; i <= ctp.Count; i++)
                        {
                            XmlNode _ctp = itemNode.SelectSingleNode("connectionsxmlns:ctp[" + i + "]", root);
                            CTPAll = CTPAll + "\r\n" + _ctp.InnerText;
                            dataGridViewEth.Rows[index].Cells["CTP端口2"].Value = _ctp.InnerText;
                            //((DataGridViewComboBoxCell)dataGridViewEth.Rows[index].Cells["CTP端口1"]).Items.Add(_ctp.InnerText);
                            //((DataGridViewComboBoxCell)dataGridViewEth.Rows[index].Cells["CTP端口1"]).Style.NullValue = _ctp.InnerText;
                        }
                        dataGridViewEth.Rows[index].Cells["CTP端口1"].Value = CTPAll;
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void 删除业务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + ", " + connection;
                    }
                }
                if (MessageBox.Show("正在删除当前业务:\r\n" + allconnection + "\r\n是否删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();
                            int id = int.Parse(treeViewNEID.SelectedNode.Name);
                            int line = -1;
                            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                            {
                                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                                {
                                    line = i;
                                    break;
                                }
                                if (line >= 0)
                                    break;
                            }
                            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                            var doc = Sendrpc(DeleteODU.Delete(connection), id, ip);//设备IP地址
                            if (doc.OuterXml.Contains("error"))
                            {
                                MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            }
                            else
                            {
                                this.dataGridViewEth.Rows.Remove(row);
                            }
                        }
                    }
                    MessageBox.Show(allconnection + "\r\n已成功删除，重新点击在线查询即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// RPC发送消息函数
        /// </summary>
        /// <param name="rpc">发送的脚本</param>
        /// <returns></returns>
        private XmlDocument Sendrpc(XmlDocument rpc, int id, string ip)
        {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            XmlDocument rpcc = new XmlDocument();
            try
            {
                //TreeReP.Nodes.Clear();
                if (netConfClient[id] == null)
                {
                    MessageBox.Show(ip + "：设备离线");
                    return rpcc;
                }
                if (!netConfClient[id].IsConnected)
                {
                    // 断开连接ToolStripMenuItem.PerformClick();
                    MessageBox.Show(ip + "：设备离线");
                    return rpcc;
                }
                string rpcxml = "";
                if (!rpc.OuterXml.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + rpc.OuterXml + RpcEnd;
                    netConfClient[id].AutomaticMessageIdHandling = true;
                }
                else
                {
                    rpcxml = rpc.OuterXml;
                    netConfClient[id].AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                DateTime dTimeEnd = System.DateTime.Now;
                TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "请求：" + FenGeFu);
                TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + FenGeFu);
                // fastColoredTextBoxReq.Text = sb.ToString();
                DateTime dTimeServer = System.DateTime.Now;
                var rpcResponse = netConfClient[id].SendReceiveRpc(rpc);
                dTimeServer = System.DateTime.Now;
                TimeSpan ts = dTimeServer - dTimeEnd;
                LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText(XmlFormat.Xml(rpcResponse.OuterXml) + FenGeFu);
                //BeginInvoke(new MethodInvoker(delegate () { LoadTreeFromXmlDocument_TreeReP(rpcResponse); }));
                if (rpcResponse.OuterXml.Contains("error-type"))
                {
                    //MessageBox.Show("运行失败：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                    rpcc.LoadXml(XmlFormat.Xml(rpcResponse.OuterXml));
                }
                else
                {
                    //MessageBox.Show("运行成功：\r\n" + sb.ToString());
                    rpcc.LoadXml(XmlFormat.Xml(rpcResponse.OuterXml));
                }
            }
            catch (Exception ex)
            {
                TextLog.AppendText("网元：" +nename+ " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText(ex.Message + "\r\n");
                MessageBox.Show("运行失败！原因如下：\r\n" + ex.ToString());
            }
            return rpcc;
        }
        /// <summary>
        /// 脚本业务的RPC函数，带返回提示
        /// </summary>
        /// <param name="rpc">发送的脚本XML</param>
        private string Creat(XmlDocument rpc, int id, string ip)
        {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            string Messg = "";
            try
            {
                if (netConfClient[id] == null)
                {
                    Messg = ip + "：设备离线";
                    return Messg;
                }
                if (!netConfClient[id].IsConnected)
                {
                    //断开连接ToolStripMenuItem.PerformClick();
                    Messg = ip + "：设备离线";
                    return Messg;
                }
                //TreeReP.Nodes.Clear();
                string rpcxml = "";
                if (!rpc.OuterXml.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + rpc.OuterXml + RpcEnd;
                    netConfClient[id].AutomaticMessageIdHandling = true;
                }
                else
                {
                    rpcxml = rpc.OuterXml;
                    netConfClient[id].AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                DateTime dTimeEnd = System.DateTime.Now;
                TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "请求：" + FenGeFu);
                TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + FenGeFu);
                // fastColoredTextBoxReq.Text = sb.ToString();
                DateTime dTimeServer = System.DateTime.Now;
                var rpcResponse = netConfClient[id].SendReceiveRpc(rpc);
                dTimeServer = System.DateTime.Now;
                TimeSpan ts = dTimeServer - dTimeEnd;
                LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText(XmlFormat.Xml(rpcResponse.OuterXml) + FenGeFu);
                // BeginInvoke(new MethodInvoker(delegate () { LoadTreeFromXmlDocument_TreeReP(rpcResponse); }));
                if (rpcResponse.OuterXml.Contains("error"))
                {
                    // MessageBox.Show("运行失败：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                    Messg = "运行失败：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml);
                }
                else
                {
                    // MessageBox.Show("运行成功：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                    Messg = "配置成功！";
                }
            }
            catch (Exception ex)
            {
                TextLog.AppendText("网元：" +nename+ " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                TextLog.AppendText(ex.Message + "\r\n");
                // MessageBox.Show("运行失败！原因如下：\r\n" + ex.ToString());
                Messg = "运行失败！原因如下：\r\n" + ex.ToString();
            }
            return Messg;
        }
        private void ButFindEoO_Click(object sender, EventArgs e)
        {
            try
            {
                groupBoxETH_UNI.Controls.Add(groupBoxunivlan);
                ComEthUniPtpName.Items.Clear();
                ComEthSecondUniPtpName.Items.Clear();
                comODUPtpNamePrimary_nni.Items.Clear();
                ComODUPtpNamesecondary_nni.Items.Clear();
                comODUPtpNamePrimary_nni2.Items.Clear();
                ComODUPtpNamesecondary_nni2.Items.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                //// XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, false, false), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                ComEthUniPtpName.Items.Add("无");
                ComEthSecondUniPtpName.Items.Add("无");
                ComODUPtpNamesecondary_nni2.Items.Add("无");
                comODUPtpNamePrimary_nni2.Items.Add("无");
                ComODUPtpNamesecondary_nni.Items.Add("无");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText.Contains("ETH"))
                        {
                            if (name != null)
                            {
                                ComEthUniPtpName.Items.Add(name.InnerText);
                                ComEthUniPtpName.SelectedIndex = 0;
                                ComEthSecondUniPtpName.Items.Add(name.InnerText);
                                ComEthSecondUniPtpName.SelectedIndex = 0;
                                if (ComCreatConnection.Text.Contains("ETH-to-ETH"))
                                {
                                    comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                                    comODUPtpNamePrimary_nni.SelectedIndex = 0;
                                }
                            }
                        }
                        if (layer_protocol_name.InnerText.Contains("ODU"))
                        {
                            if (name != null)
                            {
                                if (interface_type.InnerText.Contains("NNI"))
                                {
                                    comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                                    comODUPtpNamePrimary_nni.SelectedIndex = 0;
                                    ComODUPtpNamesecondary_nni.Items.Add(name.InnerText);
                                    ComODUPtpNamesecondary_nni.SelectedIndex = 0;
                                    ComODUPtpNamesecondary_nni2.Items.Add(name.InnerText);
                                    ComODUPtpNamesecondary_nni2.SelectedIndex = 0;
                                    comODUPtpNamePrimary_nni2.Items.Add(name.InnerText);
                                    comODUPtpNamePrimary_nni2.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ButCreatEoO_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            string messg = Creat(CreateETH.Common(ips, ComCreatConnection.Text, TextEthLabel.Text, ComEthServiceType.Text, "ETH", TextEthCir.Text, TextEthPir.Text, TextEthCbs.Text, TextEthPbs.Text, Com_Eth_uni_protection_type.Text, Com_nni_protection_type.Text, Com_nni2_protection_type.Text, ComEthServiceMappingMode.Text,
                    ComEthUniPtpName.Text, ComEthSecondUniPtpName.Text, ComEthClientVlanId.Text, ComEthVlanPriority.Text, ComEthVlanAccessAction.Text, ComEthVlanType.Text, ComEthUniVlanId.Text, ComEthUniVlanPriority.Text, ComEthUniVlanAccessAction.Text, ComEthUniVlanType.Text,
                    comODUPtpNamePrimary_nni.Text, ComOduTsDetail_Primary_nni.Text, ComOduAdapataionType_Primary_nni.Text, ComOduOduSignalType_Primary_nni.Text, ComOduSwitchApability_Primary_nni.Text, ComEthFtpVlanID_primary_nni.Text, ComEthFtpVlanPriority_primary_nni.Text, ComEthFtpVlanAccess_primary_nni.Text, ComEthFtpVlanType_primary_nni.Text, ComOsuTpn_Primary_nni.Text,
                    ComODUPtpNamesecondary_nni.Text, ComOduTsDetail_Secondary_nni.Text, ComOduAdapataionType_Secondary_nni.Text, ComOduOduSignalType_Secondary_nni.Text, ComOduSwitchApability_Secondary_nni.Text, ComOsuTpn_secondary_nni.Text,
                    ComEosSdhSignalType.Text, ComVCType.Text, TextMappingPath.Text, ComEosSdhSignalTypeProtect.Text, TextMappingPathProtect.Text, ComLCAS.Text, ComHoldOff.Text, ComWTR.Text, ComTSD.Text,
                    comODUPtpNamePrimary_nni2.Text, ComOduTsDetail_Primary_nni2.Text, ComOduAdapataionType_Primary_nni2.Text, ComOduOduSignalType_Primary_nni2.Text, ComOduSwitchApability_Primary_nni2.Text, ComEthFtpVlanID_primary_nni2.Text, ComEthFtpVlanPriority_primary_nni2.Text, ComEthFtpVlanAccess_primary_nni2.Text, ComEthFtpVlanType_primary_nni2.Text, ComOsuTpn_Primary_nni2.Text,
                    ComODUPtpNamesecondary_nni2.Text, ComOduTsDetail_Secondary_nni2.Text, ComOduAdapataionType_Secondary_nni2.Text, ComOduOduSignalType_Secondary_nni2.Text, ComOduSwitchApability_Secondary_nni2.Text, ComOsuTpn_secondary_nni2.Text
    ), id, ip);
            MessageBox.Show(messg);
        }
        private void ButFindEth_online_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewEth.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.Connections();
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("connectionsxmlns", "urn:ccsa:yang:acc-connection");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//connectionsxmlns:connections//connectionsxmlns:connection", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewEth.Rows.Add();
                    XmlNode name = itemNode.SelectSingleNode("connectionsxmlns:name", root);
                    XmlNode label = itemNode.SelectSingleNode("connectionsxmlns:label", root);
                    XmlNode operational_state = itemNode.SelectSingleNode("connectionsxmlns:state-pac/connectionsxmlns:operational-state", root);
                    XmlNode admin_stste = itemNode.SelectSingleNode("connectionsxmlns:state-pac/connectionsxmlns:admin-state", root);
                    XmlNode total_size = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:total-size", root);
                    XmlNode cir = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:cir", root);
                    XmlNode pir = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:pir", root);
                    XmlNode cbs = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:cbs", root);
                    XmlNode pbs = itemNode.SelectSingleNode("connectionsxmlns:requested-capacity/connectionsxmlns:pbs", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("connectionsxmlns:layer-protocol-name", root);
                    XmlNode service_type = itemNode.SelectSingleNode("connectionsxmlns:service-type", root);
                    if (name != null) { dataGridViewEth.Rows[index].Cells["连接名称"].Value = name.InnerText; }
                    if (label != null) { dataGridViewEth.Rows[index].Cells["标签别名"].Value = label.InnerText; }
                    if (operational_state != null) { dataGridViewEth.Rows[index].Cells["当前状态"].Value = operational_state.InnerText; }
                    if (admin_stste != null) { dataGridViewEth.Rows[index].Cells["管理状态"].Value = admin_stste.InnerText; }
                    if (total_size != null) { dataGridViewEth.Rows[index].Cells["业务总带宽"].Value = total_size.InnerText; }
                    if (cir != null) { dataGridViewEth.Rows[index].Cells["承诺带宽"].Value = cir.InnerText; }
                    if (pir != null) { dataGridViewEth.Rows[index].Cells["峰值带宽"].Value = pir.InnerText; }
                    if (cbs != null) { dataGridViewEth.Rows[index].Cells["承诺突发"].Value = cbs.InnerText; }
                    if (pbs != null) { dataGridViewEth.Rows[index].Cells["峰值突发"].Value = pbs.InnerText; }
                    if (layer_protocol_name != null) { dataGridViewEth.Rows[index].Cells["当前层协议"].Value = layer_protocol_name.InnerText; }
                    if (service_type != null) { dataGridViewEth.Rows[index].Cells["服务类型"].Value = service_type.InnerText; }
                    XmlNodeList ctp = itemNode.SelectNodes("connectionsxmlns:ctp", root);
                    string CTPAll = "";
                    ArrayList list = new ArrayList();
                    if (ctp != null)
                    {
                        for (int i = 1; i <= ctp.Count; i++)
                        {
                            XmlNode _ctp = itemNode.SelectSingleNode("connectionsxmlns:ctp[" + i + "]", root);
                            //  CTPAll = CTPAll+"," + _ctp.InnerText;
                            list.Add(_ctp.InnerText);
                            CTPAll = string.Join(",", (string[])list.ToArray(typeof(string)));
                            if (_ctp.InnerText.Contains("PTP"))
                            {
                                dataGridViewEth.Rows[index].Cells["CTP端口2"].Value = _ctp.InnerText;
                            }
                        }
                        dataGridViewEth.Rows[index].Cells["CTP端口1"].Value = CTPAll;
                    }
                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void XML_URL_COMBOX(string URL)
        {
            try
            {
                string strCode;
                ArrayList alLinks;
                if (URL == "")
                {
                    MessageBox.Show("请输入网址");
                    return;
                }
                string strURL = URL;
                if (strURL.Substring(0, 7) != @"http://")
                {
                    //strURL = @"http://" + strURL;
                }
                //判断请求是否超时
                //textDOS.AppendText("正在获取页面代码===========================================OK" + "\r\n");
                strCode = NetConfXml.GetPageSource(strURL);
                //textDOS.AppendText("正在提取超链接=============================================OK" + "\r\n");
                alLinks = NetConfXml.GetHyperLinks(strCode);
                //textDOS.AppendText("正在写入XML文件============================================OK" + "\r\n");
                NetConfXml.WriteToXml(strURL, alLinks, "HyperLinks");
                //读取设定档百
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"C:\netconf\HyperLinks.xml");
                //取得节点专
                XmlNodeList node = xmlDoc.GetElementsByTagName("other");
                for (int i = 0; i < node.Count; i++)
                {
                    if (!ComXml.Items.Contains(node[i].InnerText))
                    {
                        ComXml.Items.Add(node[i].InnerText);
                    }
                    // ComOnlineXml.Items.Add(node[i].InnerText);
                }
                ComXml.SelectedIndex = 0;
                //textDOS.AppendText("从网管服务器获取GPN76模块链接成功==========================OK" + "\r\n");
            }
            catch
            {
                //   textDOS.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "GPN模块获取失败，请链接格林威尔VPN后，再次尝试！" + "\r\n");
            }
        }
        private void YIN_XML_URL(string URL, string ips)
        {
            try
            {
                string strCode;
                ArrayList alLinks;
                if (URL == "")
                {
                    MessageBox.Show("请输入网址");
                    return;
                }
                string strURL = URL;
                if (strURL.Substring(0, 7) != @"http://")
                {
                    //strURL = @"http://" + strURL;
                }
                //判断请求是否超时
                //textDOS.AppendText("正在获取页面代码===========================================OK" + "\r\n");
                strCode = NetConfXml.GetPageSource(strURL);
                //textDOS.AppendText("正在提取超链接=============================================OK" + "\r\n");
                alLinks = NetConfXml.GetHyperLinks(strCode);
                //textDOS.AppendText("正在写入XML文件============================================OK" + "\r\n");
                NetConfXml.WriteToXml(strURL, alLinks, ips);
                //读取设定档百
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"C:\netconf\" + ips + ".xml");
                //取得节点专
                XmlNodeList node = xmlDoc.GetElementsByTagName("other");
                for (int i = 0; i < node.Count; i++)
                {
                    if (ips.Contains("联通"))
                    {
                        if (!Directory.Exists(CUCC_YIN))
                        {
                            Directory.CreateDirectory(CUCC_YIN);
                        }
                        if (Directory.Exists(CUCC_YIN))
                        {
                            if (node[i].InnerText != null)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(URL + node[i].InnerText);
                                Element dos = new Element();
                                dos.Element_Value_Find(doc, ips);
                                doc.Save(CUCC_YIN + node[i].InnerText);
                            }
                        }
                    }
                    if (ips.Contains("电信"))
                    {
                        if (!Directory.Exists(CTCC_YIN))
                        {
                            Directory.CreateDirectory(CTCC_YIN);
                        }
                        if (Directory.Exists(CTCC_YIN))
                        {
                            if (node[i].InnerText != null)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(URL + node[i].InnerText);
                                Element dos = new Element();
                                dos.Element_Value_Find(doc, ips);
                                doc.Save(CTCC_YIN + node[i].InnerText);
                            }
                        }
                    }
                    if (ips.Contains("移动"))
                    {
                        if (!Directory.Exists(CMCC_YIN))
                        {
                            Directory.CreateDirectory(CMCC_YIN);
                        }
                        if (Directory.Exists(CMCC_YIN))
                        {
                            if (node[i].InnerText != null)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(URL + node[i].InnerText);
                                Element dos = new Element();
                                dos.Element_Value_Find(doc, ips);
                                doc.Save(CMCC_YIN + node[i].InnerText);
                            }
                        }
                    }
                }
                //textDOS.AppendText("从网管服务器获取GPN76模块链接成功==========================OK" + "\r\n");
            }
            catch
            {
                //   textDOS.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "GPN模块获取失败，请链接格林威尔VPN后，再次尝试！" + "\r\n");
            }
        }
        private void ButCurPerFindPort_Click(object sender, EventArgs e)
        {
            try
            {
                ComCurPerObjectName.Items.Clear();
                ComCurPerObjectName.Items.Add("全部端口");
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, true, true), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                //  root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp|//ptpsxmlns:ftps//ptpsxmlns:ftp|//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    if (name != null)
                    {
                        ComCurPerObjectName.Items.Add(name.InnerText);
                        if (ComCurPerObjectName.Items.Count > 0)
                        {
                            ComCurPerObjectName.SelectedIndex = ComCurPerObjectName.Items.Count - 1;
                        }
                    }
                }
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ButCurPerSlect_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewCurrentPerformance.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.FindPerformance(ComCurPerObjectName.Text, ComCurPerGranularity.Text);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("performancesxmlns", "urn:ccsa:yang:acc-performance");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//performancesxmlns:performances//performancesxmlns:performance", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewCurrentPerformance.Rows.Add();
                    XmlNode pm_parameter_name = itemNode.SelectSingleNode("performancesxmlns:pm-parameter-name", root);
                    XmlNode start_time = itemNode.SelectSingleNode("performancesxmlns:start-time", root);
                    XmlNode object_name = itemNode.SelectSingleNode("performancesxmlns:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("performancesxmlns:object-type", root);
                    XmlNode granularity = itemNode.SelectSingleNode("performancesxmlns:granularity", root);
                    XmlNode digital_pm_value = itemNode.SelectSingleNode("performancesxmlns:digital-pm-value", root);
                    XmlNode max_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:max-value", root);
                    XmlNode min_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:min-value", root);
                    XmlNode average_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:average-value", root);
                    XmlNode current_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:current-value", root);
                    if (pm_parameter_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["参数名称"].Value = pm_parameter_name.InnerText; }
                    if (start_time != null) { dataGridViewCurrentPerformance.Rows[index].Cells["开始时间"].Value = start_time.InnerText; }
                    if (object_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象类型"].Value = object_type.InnerText; }
                    if (granularity != null) { dataGridViewCurrentPerformance.Rows[index].Cells["周期类型"].Value = granularity.InnerText; }
                    if (digital_pm_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["数字量性能值"].Value = digital_pm_value.InnerText; }
                    if (max_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["最大值"].Value = max_value.InnerText; }
                    if (min_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["最小值"].Value = min_value.InnerText; }
                    if (average_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["平均值"].Value = average_value.InnerText; }
                    if (current_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["当前值"].Value = current_value.InnerText; }
                    LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ButHisPerFind_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewCurrentPerformance.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.FindHisPerformance(dateTimePickerStartime.Text, dateTimePickerEndtime.Text, ComCurPerGranularity.Text, ComCurPerObjectName.Text, "",ips);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("performancesxmlns", "urn:ccsa:yang:acc-performance");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//performancesxmlns:performances//performancesxmlns:performance", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewCurrentPerformance.Rows.Add();
                    XmlNode pm_parameter_name = itemNode.SelectSingleNode("performancesxmlns:pm-parameter-name", root);
                    XmlNode start_time = itemNode.SelectSingleNode("performancesxmlns:start-time", root);
                    XmlNode object_name = itemNode.SelectSingleNode("performancesxmlns:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("performancesxmlns:object-type", root);
                    XmlNode granularity = itemNode.SelectSingleNode("performancesxmlns:granularity", root);
                    XmlNode digital_pm_value = itemNode.SelectSingleNode("performancesxmlns:digital-pm-value", root);
                    XmlNode max_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:max-value", root);
                    XmlNode min_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:min-value", root);
                    XmlNode average_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:average-value", root);
                    XmlNode current_value = itemNode.SelectSingleNode("performancesxmlns:analog-pm-value/performancesxmlns:current-value", root);
                    if (pm_parameter_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["参数名称"].Value = pm_parameter_name.InnerText; }
                    if (start_time != null) { dataGridViewCurrentPerformance.Rows[index].Cells["开始时间"].Value = start_time.InnerText; }
                    if (object_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象类型"].Value = object_type.InnerText; }
                    if (granularity != null) { dataGridViewCurrentPerformance.Rows[index].Cells["周期类型"].Value = granularity.InnerText; }
                    if (digital_pm_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["数字量性能值"].Value = digital_pm_value.InnerText; }
                    if (max_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["最大值"].Value = max_value.InnerText; }
                    if (min_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["最小值"].Value = min_value.InnerText; }
                    if (average_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["平均值"].Value = average_value.InnerText; }
                    if (current_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["当前值"].Value = current_value.InnerText; }
                    LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ButVc_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComVCType.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextMappingPath.Text = formInfo.Information;
            }
        }
        private void ButVcProtect_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComVCType.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextMappingPathProtect.Text = formInfo.Information;
            }
        }
        private void ButSdhFind_Click(object sender, EventArgs e)
        {
            try
            {
                ComSdhUniPtp.Items.Clear();
                ComSdhUniPtp_Secondary.Items.Clear();
                comODUPtpNamePrimary_nni.Items.Clear();
                ComODUPtpNamesecondary_nni.Items.Clear();
                ComODUPtpNamesecondary_nni2.Items.Clear();
                comODUPtpNamePrimary_nni2.Items.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, false, false), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                ComSdhUniPtp.Items.Add("无");
                ComSdhUniPtp_Secondary.Items.Add("无");
                ComODUPtpNamesecondary_nni.Items.Add("无");
                ComODUPtpNamesecondary_nni2.Items.Add("无");
                comODUPtpNamePrimary_nni2.Items.Add("无");
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-sdh:SDH")
                        {
                            if (name != null)
                            {
                                ComSdhUniPtp.Items.Add(name.InnerText);
                                ComSdhUniPtp.SelectedIndex = 0;
                                ComSdhUniPtp_Secondary.Items.Add(name.InnerText);
                                ComSdhUniPtp_Secondary.SelectedIndex = 0;
                            }
                        }
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                                comODUPtpNamePrimary_nni.SelectedIndex = 0;
                                ComODUPtpNamesecondary_nni.Items.Add(name.InnerText);
                                ComODUPtpNamesecondary_nni.SelectedIndex = 0;
                                comODUPtpNamePrimary_nni2.Items.Add(name.InnerText);
                                comODUPtpNamePrimary_nni2.SelectedIndex = 0;
                                ComODUPtpNamesecondary_nni2.Items.Add(name.InnerText);
                                ComODUPtpNamesecondary_nni2.SelectedIndex = 0;
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ButSdhUniTs_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComSdhUniSdhType.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhUniTs.Text = formInfo.Information;
            }
        }
        private void ButSdhNniTs_A_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComSdhNniVcType_primary_nni.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_primary_nni.Text = formInfo.Information;
            }
        }
        private void ButSdhNniTs_B_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComSdhNniVcType_secondary_nni.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_secondary_nni.Text = formInfo.Information;
            }
        }
        private void ButCreatSDH_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            string sdhunits = "";   //TextSdhUniTs
            string sdhunits_sec = "";   //TextSdhUniTs
            string sdhnnits_a = "";  //TextSdhNniTs_A
            string sdhnnits_b = "";    //TextSdhNniTs_B
            string sdhnnits_a2 = "";  //TextSdhNniTs_A
            string sdhnnits_b2 = "";    //TextSdhNniTs_B
            string[] sdhunitsbyte = TextSdhUniTs.Text.Split(',');
            string[] sdhunitsbyte_sec = TextSdhUniTs_Secondary.Text.Split(',');
            string[] sdhnnits_abyte = TextSdhNniTs_primary_nni.Text.Split(',');
            string[] sdhnnits_bbyte = TextSdhNniTs_secondary_nni.Text.Split(',');
            string[] sdhnnits_abyte2 = TextSdhNniTs_primary_nni2.Text.Split(',');
            string[] sdhnnits_bbyte2 = TextSdhNniTs_secondary_nni2.Text.Split(',');
            string messg0 = "";
            if (sdhunitsbyte.Length == sdhnnits_abyte.Length || sdhnnits_abyte2.Length == sdhnnits_abyte.Length)
            {
                string num = Regex.Replace(TextSdhlabel.Text, @"[^0-9]+", "");
                string label = TextSdhlabel.Text;
                label = label.Replace(num, "");
                if (string.IsNullOrEmpty(num))
                {
                    MessageBox.Show("标签名称必须包含开头数字");
                    return;
                }
                int result = int.Parse(num);
                if (sdhunitsbyte[0] !="") {
                    for (int i = 0; i < sdhunitsbyte.Length; i++)
                    {
                        sdhunits = sdhunitsbyte[i];
                        sdhnnits_a = sdhnnits_abyte[i];
                        if (sdhnnits_bbyte[0] != "")
                        {
                            sdhnnits_b = sdhnnits_bbyte[i];
                        }
                        if (sdhunitsbyte_sec[0] != "")
                        {
                            sdhunits_sec = sdhunitsbyte_sec[i];
                        }
                        string messg1 = Creat(CreateSDH.Common(ips, label + (i + result).ToString(), ComSdhSer.Text, "SDH", TextSdhTotal.Text, ComSdhPro.Text, Com_nni_protection_type.Text, Com_nni2_protection_type.Text,ComSdhSerMap.Text,
                                            ComSdhUniPtp.Text, ComSdhUniSdhType.Text, sdhunits,
                                            ComSdhUniPtp_Secondary.Text, ComSdhUniSdhType_Secondary.Text, sdhunits_sec,
                                            comODUPtpNamePrimary_nni.Text, ComOduTsDetail_Primary_nni.Text, ComOduAdapataionType_Primary_nni.Text, ComOduOduSignalType_Primary_nni.Text, ComOduSwitchApability_Primary_nni.Text, ComSdhNniSdhtype_primary_nni.Text, ComSdhNniVcType_primary_nni.Text, sdhnnits_a,
                                            ComODUPtpNamesecondary_nni.Text, ComOduTsDetail_Secondary_nni.Text, ComOduAdapataionType_Secondary_nni.Text, ComOduOduSignalType_Secondary_nni.Text, ComOduSwitchApability_Secondary_nni.Text, ComSdhNniSdhtype_secondary_nni.Text, ComSdhNniVcType_secondary_nni.Text, sdhnnits_b,
                                            comODUPtpNamePrimary_nni2.Text, ComOduTsDetail_Primary_nni2.Text, ComOduAdapataionType_Primary_nni2.Text, ComOduOduSignalType_Primary_nni2.Text, ComOduSwitchApability_Primary_nni2.Text, ComSdhNniSdhtype_primary_nni2.Text, ComSdhNniVcType_primary_nni2.Text, sdhnnits_a2,
                                            ComODUPtpNamesecondary_nni2.Text, ComOduTsDetail_Secondary_nni2.Text, ComOduAdapataionType_Secondary_nni2.Text, ComOduOduSignalType_Secondary_nni2.Text, ComOduSwitchApability_Secondary_nni2.Text, ComSdhNniSdhtype_secondary_nni2.Text, ComSdhNniVcType_secondary_nni2.Text, sdhnnits_b2
                                                ), id, ip);
                        messg0 = messg0 + messg1 + "=" + (i + result).ToString() + "  ";
                    }
                }
                if (sdhnnits_abyte2[0] != "")
                {
                    for (int i = 0; i < sdhnnits_abyte2.Length; i++)
                    {
                        sdhnnits_a = sdhnnits_abyte[i];
                        sdhnnits_a2 = sdhnnits_abyte2[i];
                        if (sdhnnits_bbyte[0] != "")
                        {
                            sdhnnits_b = sdhnnits_bbyte[i];
                        }
                        if (sdhnnits_bbyte2[0] != "")
                        {
                            sdhnnits_b2 = sdhnnits_bbyte2[i];
                        }
                        string messg1 = Creat(CreateSDH.Common(ips, label + (i + result).ToString(), ComSdhSer.Text, "SDH", TextSdhTotal.Text, ComSdhPro.Text, Com_nni_protection_type.Text, Com_nni2_protection_type.Text, ComSdhSerMap.Text,
                                            ComSdhUniPtp.Text, ComSdhUniSdhType.Text, sdhunits,
                                            ComSdhUniPtp_Secondary.Text, ComSdhUniSdhType_Secondary.Text, sdhunits_sec,
                                            comODUPtpNamePrimary_nni.Text, ComOduTsDetail_Primary_nni.Text, ComOduAdapataionType_Primary_nni.Text, ComOduOduSignalType_Primary_nni.Text, ComOduSwitchApability_Primary_nni.Text, ComSdhNniSdhtype_primary_nni.Text, ComSdhNniVcType_primary_nni.Text, sdhnnits_a,
                                            ComODUPtpNamesecondary_nni.Text, ComOduTsDetail_Secondary_nni.Text, ComOduAdapataionType_Secondary_nni.Text, ComOduOduSignalType_Secondary_nni.Text, ComOduSwitchApability_Secondary_nni.Text, ComSdhNniSdhtype_secondary_nni.Text, ComSdhNniVcType_secondary_nni.Text, sdhnnits_b,
                                            comODUPtpNamePrimary_nni2.Text, ComOduTsDetail_Primary_nni2.Text, ComOduAdapataionType_Primary_nni2.Text, ComOduOduSignalType_Primary_nni2.Text, ComOduSwitchApability_Primary_nni2.Text, ComSdhNniSdhtype_primary_nni2.Text, ComSdhNniVcType_primary_nni2.Text, sdhnnits_a2,
                                            ComODUPtpNamesecondary_nni2.Text, ComOduTsDetail_Secondary_nni2.Text, ComOduAdapataionType_Secondary_nni2.Text, ComOduOduSignalType_Secondary_nni2.Text, ComOduSwitchApability_Secondary_nni2.Text, ComSdhNniSdhtype_secondary_nni2.Text, ComSdhNniVcType_secondary_nni2.Text, sdhnnits_b2
                                                ), id, ip);
                        messg0 = messg0 + messg1 + "=" + (i + result).ToString() + "  ";
                    }
                }

                MessageBox.Show(messg0);
            }
            //        Creat(CreateSDH.Common(ips,TextSdhlabel.Text, ComSdhSer.Text, "SDH", TextSdhTotal.Text, ComSdhPro.Text, ComSdhSerMap.Text,
            //                ComSdhUniPtp.Text, ComSdhUniSdhType.Text, TextSdhUniTs.Text,
            //ComSdhNniPtp_A.Text, TSConversion.Ts(ComSdhNniOdu_A.Text, ComSdhNniSwitch_A.Text, ComSdhNniTs_A.Text), ComSdhNniAda_A.Text, ComSdhNniOdu_A.Text, ComSdhNniSwitch_A.Text, ComSdhNniSdhtype_A.Text, ComSdhNniVcType_A.Text, TextSdhNniTs_A.Text,
            //    ComSdhNniPtp_B.Text, TSConversion.Ts(ComSdhNniOdu_B.Text, ComSdhNniSwitch_B.Text, ComSdhNniTs_B.Text), ComSdhNniAda_B.Text, ComSdhNniOdu_B.Text, ComSdhNniSwitch_B.Text, ComSdhNniSdhtype_B.Text, ComSdhNniVcType_B.Text, TextSdhNniTs_B.Text
            //));
        }
        private void ButPGSFind_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewPGS.Rows.Clear();
                dataGridViewPGS_Not.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(GetXML.Pgs(), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("pgsxmlns", "urn:ccsa:yang:acc-protection-group");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//pgsxmlns:pgs//pgsxmlns:pg", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewPGS.Rows.Add();
                    if (ips.Contains("移动"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode tcm = itemNode.SelectSingleNode("pgsxmlns:tcm", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-direction", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (protection_type != null) { dataGridViewPGS.Rows[index].Cells["保护类型"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS.Rows[index].Cells["还原模式"].Value = reversion_mode.InnerText; }
                        if (tcm != null) { dataGridViewPGS.Rows[index].Cells["TCM"].Value = tcm.InnerText; }

                        if (switch_direction != null) { dataGridViewPGS.Rows[index].Cells["开关方向"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS.Rows[index].Cells["SD触发器"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS.Rows[index].Cells["恢复等待时间"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS.Rows[index].Cells["拖延时间"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                    if (ips.Contains("联通"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-type", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (protection_type != null) { dataGridViewPGS.Rows[index].Cells["保护类型"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS.Rows[index].Cells["还原模式"].Value = reversion_mode.InnerText; }
                        if (switch_direction != null) { dataGridViewPGS.Rows[index].Cells["开关方向"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS.Rows[index].Cells["SD触发器"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS.Rows[index].Cells["恢复等待时间"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS.Rows[index].Cells["拖延时间"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                    if (ips.Contains("电信"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode create_type = itemNode.SelectSingleNode("pgsxmlns:create-type", root);
                        XmlNode delete_cascade = itemNode.SelectSingleNode("pgsxmlns:delete-cascade", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode reversion_mode = itemNode.SelectSingleNode("pgsxmlns:reversion-mode", root);
                        XmlNode tcm = itemNode.SelectSingleNode("pgsxmlns:tcm", root);
                        XmlNode switch_direction = itemNode.SelectSingleNode("pgsxmlns:switch-type", root);
                        XmlNode sd_trigger = itemNode.SelectSingleNode("pgsxmlns:sd-trigger", root);
                        XmlNode WTR = itemNode.SelectSingleNode("pgsxmlns:wait-to-restore-time", root);
                        XmlNode hold_off = itemNode.SelectSingleNode("pgsxmlns:hold-off", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-port", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-port", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-port", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (create_type != null) { dataGridViewPGS.Rows[index].Cells["创建方式"].Value = create_type.InnerText; }
                        if (delete_cascade != null) { dataGridViewPGS.Rows[index].Cells["级联删除"].Value = delete_cascade.InnerText; }
                        if (protection_type != null) { dataGridViewPGS.Rows[index].Cells["保护类型"].Value = protection_type.InnerText; }
                        if (reversion_mode != null) { dataGridViewPGS.Rows[index].Cells["还原模式"].Value = reversion_mode.InnerText; }
                        if (tcm != null) { dataGridViewPGS.Rows[index].Cells["TCM"].Value = tcm.InnerText; }
                        if (switch_direction != null) { dataGridViewPGS.Rows[index].Cells["开关方向"].Value = switch_direction.InnerText; }
                        if (sd_trigger != null) { dataGridViewPGS.Rows[index].Cells["SD触发器"].Value = sd_trigger.InnerText; }
                        if (WTR != null) { dataGridViewPGS.Rows[index].Cells["恢复等待时间"].Value = WTR.InnerText; }
                        if (hold_off != null) { dataGridViewPGS.Rows[index].Cells["拖延时间"].Value = hold_off.InnerText; }
                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                }
                xmlDoc = Sendrpc(GetXML.Eq_Pgs(), id, ip);
                root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("pgsxmlns", "urn:ccsa:yang:acc-protection-group");
                itemNodes = xmlDoc.SelectNodes("//pgsxmlns:eq-pgs//pgsxmlns:eq-pg", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewPGS.Rows.Add();
                    if (ips.Contains("移动"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-eq", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-eq", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-eq", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                    if (ips.Contains("联通"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-eq", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-eq", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-eq", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                    if (ips.Contains("电信"))
                    {
                        XmlNode pg_id = itemNode.SelectSingleNode("pgsxmlns:pg-id", root);
                        XmlNode protection_type = itemNode.SelectSingleNode("pgsxmlns:protection-type", root);
                        XmlNode primary_port = itemNode.SelectSingleNode("pgsxmlns:primary-eq", root);
                        XmlNode secondary_port = itemNode.SelectSingleNode("pgsxmlns:secondary-eq", root);
                        XmlNode switch_reason = itemNode.SelectSingleNode("pgsxmlns:switch-reason", root);
                        XmlNode protection_direction = itemNode.SelectSingleNode("pgsxmlns:protection-direction", root);
                        XmlNode selected_port = itemNode.SelectSingleNode("pgsxmlns:selected-eq", root);
                        if (pg_id != null) { dataGridViewPGS.Rows[index].Cells["保护组ID"].Value = pg_id.InnerText; }
                        if (protection_type != null) { dataGridViewPGS.Rows[index].Cells["保护类型"].Value = protection_type.InnerText; }

                        if (primary_port != null) { dataGridViewPGS.Rows[index].Cells["主要端口"].Value = primary_port.InnerText; }
                        if (secondary_port != null) { dataGridViewPGS.Rows[index].Cells["次要端口"].Value = secondary_port.InnerText; }
                        if (switch_reason != null) { dataGridViewPGS.Rows[index].Cells["倒换原因"].Value = switch_reason.InnerText; }
                        if (protection_direction != null) { dataGridViewPGS.Rows[index].Cells["保护方向"].Value = protection_direction.InnerText; }
                        if (selected_port != null) { dataGridViewPGS.Rows[index].Cells["选择端口"].Value = selected_port.InnerText; }
                    }
                }
                MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void Pgcommand(string _command, string _ditection,string _type)
        {
            try
            {
                string All_pg_id = "";
                string pg_id = "";
                foreach (DataGridViewRow row in this.dataGridViewPGS.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        pg_id = dataGridViewPGS.Rows[row.Index].Cells["保护组ID"].Value.ToString();       //设备IP地址
                        All_pg_id = All_pg_id + "\r\n" + pg_id;
                    }
                }
                if (MessageBox.Show("正在保护操作当前保护组:\r\n" + All_pg_id + "\r\n是否执行？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewPGS.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            pg_id = dataGridViewPGS.Rows[row.Index].Cells["保护组ID"].Value.ToString();
                            int id = int.Parse(treeViewNEID.SelectedNode.Name);
                            int line = -1;
                            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                            {
                                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                                {
                                    line = i;
                                    break;
                                }
                                if (line >= 0)
                                    break;
                            }
                            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                            if (_command != "删除")
                            {
                                var doc = new XmlDocument();
                                if (_type == "port") {
                                    doc = Sendrpc(CreatePG.Command(pg_id, _command, _ditection), id, ip);//设备IP地址
                                }
                                if (_type == "eq")
                                {
                                    doc = Sendrpc(CreatePG.Command_Eq_Pgs(pg_id, _command, _ditection), id, ip);//设备IP地址
                                }

                                if (doc.OuterXml.Contains("error"))
                                {
                                    MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                                }
                                else
                                {
                                    // this.dataGridViewPGS.Rows.Remove(row);
                                }
                            }
                            if ((_command == "删除")&&(_type == "port"))
                            {
                                var doc = Sendrpc(DeleteODU.Pg(pg_id), id, ip);//设备IP地址
                                if (doc.OuterXml.Contains("error"))
                                {
                                    MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                                }
                                else
                                {
                                    // this.dataGridViewPGS.Rows.Remove(row);
                                }
                            }
                           
                           
                        }
                    }
                    //MessageBox.Show(All_pg_id + "\r\n已成功下发，重新点击在线查询即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 强制倒换到主用toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("force-switch", "to-primary","port");
        }
        private void 强制倒换到备用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("force-switch", "to-secondary", "port");
        }
        private void 人工倒换到主用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-primary", "port");
        }
        private void 人工倒换到备用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-secondary", "port");
        }
        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("cleared", "to-secondary", "port");
        }
        private void 锁定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("lockout", "to-secondary", "port");
        }
        private void 订阅ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            string nename = dataGridViewNeInformation.Rows[line].Cells["网元名称"].Value.ToString();
            string subscription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n" +
    "<rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"7\" >" + "\r\n" +
    "<create-subscription xmlns=\"urn:ietf:params:xml:ns:netconf:notification:1.0\" />" + "\r\n" +
    "</rpc > ";
            var sub = netConfClient[id].SendReceiveRpc(subscription);
            TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
            TextLog.AppendText(sub.OuterXml + FenGeFu);
            Sub[id] = true;
            Thread thread = new Thread(() => Subscription(id, ip));
            thread.Start();
            TextSub.Text = "已订阅";
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (defaultfilePath == "")
            {
                MessageBox.Show("请先点击文件---打开一个存放XML文件的目录，然后再次尝试！");
                return;
            }
            DialogResult dr1 = MessageBox.Show(defaultfilePath + ComXml.Text + "  保存到这里请点击 “是” !\r\n  如果不需要保存请点击   “取消”  !", "提示", MessageBoxButtons.YesNo);
            if (dr1 == DialogResult.Yes)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(fastColoredTextBoxReq.Text);
                doc.Save(defaultfilePath + ComXml.Text);
            }
        }
        private void 断开连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                toolStripStatusLabelips.Text = "未连接";
                LabConncet.Text = "连接断开";
                上载全部XMLToolStripMenuItem.Enabled = false;
                TextSub.Text = "未订阅";
                netConfClient[id].Disconnect();
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
            }
        }
        private void 上载全部XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            Thread thread = new Thread(() => GetXmlAll(id, ip));
            thread.Start();
            MessageBox.Show("请耐心等待15-60秒后，方可执行其他操作！\r\n如果60秒没有提示说明请求超时！");
        }
        private void TreeReP_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            int t = e.Node.Text.IndexOf('(');
            if (t > 0)
            {
                string s1 = e.Node.Text.Substring(0, t);
                string s2 = e.Node.Text.Substring(t);
                SizeF s = e.Graphics.MeasureString(s1, this.Font);
                e.Graphics.DrawString(s1, this.Font, Brushes.Magenta, e.Bounds.X, e.Bounds.Y);
                e.Graphics.DrawString(s2, this.Font, Brushes.Blue, e.Bounds.X + s.Width, e.Bounds.Y);
            }
            else
            {
                e.Graphics.DrawString(e.Node.Text, this.Font, Brushes.Magenta, e.Bounds.X, e.Bounds.Y);
            }
        }

        private void oAM创建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            try
            {
                string _server_tp = "", _vlan_id = "", _vlan_type = "", _dm_state = "", _tm_state = "", _lm_state = "", _cc_state = "", _cc_state1 = "", _mep_id = "", _remote_mep_id = "", _meg_id = "",
                    _md_name = "", _mel = "", _cc_interval = "", _lm_interval = "", _dm_interval = "",
    _delay = "", _near_loss = "", _far_loss = "", _tx_bytes = "", _rx_bytes = "";
                string allconnection = "";
                string _name = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        _name = dataGridViewEth.Rows[row.Index].Cells["CTP端口2"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + "\r\n" + _name;
                    }
                }
                if (MessageBox.Show("正在配置当前业务的OAM:\r\n" + allconnection + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            _name = dataGridViewEth.Rows[row.Index].Cells["CTP端口2"].Value.ToString();
                            try
                            {
                                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                XmlDocument xmlDoc = new XmlDocument();
                                //xmlDoc.Load(filename);
                                xmlDoc = Sendrpc(GetXML.CTP(_name), id, ip);
                                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                root.AddNamespace("ctpxmlns", "urn:ccsa:yang:acc-eth");
                                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                                foreach (XmlNode itemNode in itemNodes)
                                {
                                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                                    XmlNode server_tp = itemNode.SelectSingleNode("ptpsxmlns:server-tp", root);
                                    if (layer_protocol_name != null && server_tp != null)
                                    {
                                        _server_tp = server_tp.InnerText;
                                        if (layer_protocol_name.InnerText == "acc-eth:ETH")
                                        {
                                            if (name != null)
                                            {
                                                if (_name == name.InnerText)
                                                {
                                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("ctpxmlns:eth-ctp-pac", root);
                                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                                    {
                                                        if (ips.Contains("移动")|| ips.Contains("电信"))
                                                        {
                                                            XmlNode vlan_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:vlan-id", root);
                                                            XmlNode vlan_type = itemNodeOdu.SelectSingleNode("//ctpxmlns:vlan-type", root);
                                                            XmlNode dm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:dm-state", root);
                                                            XmlNode tm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:tm-state", root);
                                                            XmlNode lm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:lm-state", root);
                                                            XmlNode cc_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:cc-state", root);
                                                            XmlNode mep_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:mep-id", root);
                                                            XmlNode meg_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:meg-id", root);
                                                            XmlNode mel = itemNodeOdu.SelectSingleNode("//ctpxmlns:mel", root);
                                                            XmlNode remote_mep_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:remote-mep-id", root);
                                                            XmlNode md_name = itemNodeOdu.SelectSingleNode("//ctpxmlns:md-name", root);
                                                            XmlNode cc_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:cc-interval", root);
                                                            XmlNode lm_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:lm-interval", root);
                                                            XmlNode dm_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:dm-interval", root);
                                                            XmlNode delay = itemNodeOdu.SelectSingleNode("//ctpxmlns:delay", root);
                                                            XmlNode near_packet_loss_rate = itemNodeOdu.SelectSingleNode("//ctpxmlns:near-packet-loss-rate", root);
                                                            XmlNode far_packet_loss_rate = itemNodeOdu.SelectSingleNode("//ctpxmlns:far-packet-loss-rate", root);
                                                            XmlNode tx_bytes = itemNodeOdu.SelectSingleNode("//ctpxmlns:tx-bytes", root);
                                                            XmlNode rx_bytes = itemNodeOdu.SelectSingleNode("//ctpxmlns:rx-bytes", root);
                                                            if (vlan_id != null) { _vlan_id = vlan_id.InnerText; }
                                                            if (vlan_type != null) { _vlan_type = vlan_type.InnerText; }
                                                            if (dm_state != null) { _dm_state = dm_state.InnerText; }
                                                            if (tm_state != null) { _tm_state = tm_state.InnerText; }
                                                            if (lm_state != null) { _lm_state = lm_state.InnerText; }
                                                            if (cc_state != null) { _cc_state = cc_state.InnerText; }
                                                            if (mep_id != null) { _mep_id = mep_id.InnerText; }
                                                            if (meg_id != null) { _meg_id = meg_id.InnerText; }
                                                            if (mel != null) { _mel = mel.InnerText; }
                                                            if (remote_mep_id != null) { _remote_mep_id = remote_mep_id.InnerText; }
                                                            if (md_name != null) { _md_name = md_name.InnerText; }
                                                            if (cc_interval != null) { _cc_interval = cc_interval.InnerText; }
                                                            if (lm_interval != null) { _lm_interval = lm_interval.InnerText; }
                                                            if (dm_interval != null) { _dm_interval = dm_interval.InnerText; }
                                                            if (delay != null) { _delay = delay.InnerText; }
                                                            if (near_packet_loss_rate != null) { _near_loss = near_packet_loss_rate.InnerText; }
                                                            if (far_packet_loss_rate != null) { _far_loss = far_packet_loss_rate.InnerText; }
                                                            if (tx_bytes != null) { _tx_bytes = tx_bytes.InnerText; }
                                                            if (rx_bytes != null) { _rx_bytes = rx_bytes.InnerText; }
                                                        }
                                                        if (ips.Contains("联通"))
                                                        {
                                                            XmlNode vlan_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:vlan-id", root);
                                                            XmlNode vlan_type = itemNodeOdu.SelectSingleNode("//ctpxmlns:vlan-type", root);
                                                            XmlNode dm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:dm-enable", root);
                                                            XmlNode tm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:tm-enable", root);
                                                            XmlNode lm_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:lm-enable", root);
                                                            XmlNode cc_state = itemNodeOdu.SelectSingleNode("//ctpxmlns:cc-enable", root);
                                                            XmlNode cc_state1 = itemNodeOdu.SelectSingleNode("//ctpxmlns:cc-state", root);
                                                            XmlNode mep_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:mep-id", root);
                                                            XmlNode meg_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:meg-id", root);
                                                            XmlNode mel = itemNodeOdu.SelectSingleNode("//ctpxmlns:mel", root);
                                                            XmlNode remote_mep_id = itemNodeOdu.SelectSingleNode("//ctpxmlns:remote-mep-id", root);
                                                            XmlNode md_name = itemNodeOdu.SelectSingleNode("//ctpxmlns:md-name", root);
                                                            XmlNode cc_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:cc-interval", root);
                                                            XmlNode lm_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:lm-interval", root);
                                                            XmlNode dm_interval = itemNodeOdu.SelectSingleNode("//ctpxmlns:dm-interval", root);
                                                            XmlNode delay = itemNodeOdu.SelectSingleNode("//ctpxmlns:delay", root);
                                                            XmlNode near_packet_loss_rate = itemNodeOdu.SelectSingleNode("//ctpxmlns:near-packet-loss-rate", root);
                                                            XmlNode far_packet_loss_rate = itemNodeOdu.SelectSingleNode("//ctpxmlns:far-packet-loss-rate", root);
                                                            XmlNode tx_bytes = itemNodeOdu.SelectSingleNode("//ctpxmlns:tx-bytes", root);
                                                            XmlNode rx_bytes = itemNodeOdu.SelectSingleNode("//ctpxmlns:rx-bytes", root);
                                                            if (vlan_id != null) { _vlan_id = vlan_id.InnerText; }
                                                            if (vlan_type != null) { _vlan_type = vlan_type.InnerText; }
                                                            if (dm_state != null) { _dm_state = dm_state.InnerText; }
                                                            if (tm_state != null) { _tm_state = tm_state.InnerText; }
                                                            if (lm_state != null) { _lm_state = lm_state.InnerText; }
                                                            if (cc_state != null) { _cc_state = cc_state.InnerText; }
                                                            if (cc_state1 != null) { _cc_state1 = cc_state1.InnerText; }
                                                            if (mep_id != null) { _mep_id = mep_id.InnerText; }
                                                            if (meg_id != null) { _meg_id = meg_id.InnerText; }
                                                            if (mel != null) { _mel = mel.InnerText; }
                                                            if (remote_mep_id != null) { _remote_mep_id = remote_mep_id.InnerText; }
                                                            if (md_name != null) { _md_name = md_name.InnerText; }
                                                            if (cc_interval != null) { _cc_interval = cc_interval.InnerText; }
                                                            if (lm_interval != null) { _lm_interval = lm_interval.InnerText; }
                                                            if (dm_interval != null) { _dm_interval = dm_interval.InnerText; }
                                                            if (delay != null) { _delay = delay.InnerText; }
                                                            if (near_packet_loss_rate != null) { _near_loss = near_packet_loss_rate.InnerText; }
                                                            if (far_packet_loss_rate != null) { _far_loss = far_packet_loss_rate.InnerText; }
                                                            if (tx_bytes != null) { _tx_bytes = tx_bytes.InnerText; }
                                                            if (rx_bytes != null) { _rx_bytes = rx_bytes.InnerText; }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                // Console.Read();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                            }
                            // 实例化FormInfo，并传入待修改初值  
                            var Formoam = new Form_OAM(_name, _server_tp, _vlan_id, _vlan_type, _dm_state, _tm_state, _lm_state, _cc_state, _cc_state1, _mep_id, _remote_mep_id, _meg_id, _md_name, _mel, _cc_interval, _lm_interval, _dm_interval,
    _delay, _near_loss, _far_loss, _tx_bytes, _rx_bytes);
                            // 以对话框方式显示FormInfo  
                            if (Formoam.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _name = Formoam._name;
                                _mep_id = Formoam._mep_id;
                                _remote_mep_id = Formoam._remote_mep_id;
                                _meg_id = Formoam._meg_id;
                                _md_name = Formoam._md_name;
                                _mel = Formoam._mel;
                                _cc_interval = Formoam._cc_interval;
                                _lm_interval = Formoam._lm_interval;
                                _dm_interval = Formoam._dm_interval;
                                string messg = Creat(CreateOAM.Create(_name, _mep_id, _remote_mep_id, _meg_id, _md_name, _mel, _cc_interval, _lm_interval, _dm_interval, ips), id, ip);
                                MessageBox.Show(messg + "\n正在配置OAM状态，请稍等片刻！");
                                _name = Formoam._name;
                                _dm_state = Formoam._dm_state;
                                _tm_state = Formoam._tm_state;
                                _lm_state = Formoam._lm_state;
                                _cc_state = Formoam._cc_state;
                                messg = Creat(CreateOAM.State(_name, _dm_state, _tm_state, _lm_state, _cc_state, ips), id, ip);
                                MessageBox.Show(messg);
                            }
                            //var doc = Sendrpc(DeleteODU.Delete(_name));//设备IP地址
                            //if (doc.OuterXml.Contains("error"))
                            //{
                            //    MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            //}
                            //else
                            //{
                            //    //this.dataGridViewEth.Rows.Remove(row);
                            //}
                        }
                    }
                    // MessageBox.Show(allconnection + "\r\n已成功删除，重新点击在线查询即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ButAddTest_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var AutoXml = new Form_AutoRunningNetconf(gpnip);
            // 以对话框方式显示FormInfo  
            if (AutoXml.ShowDialog() == DialogResult.OK)
            {
                int index = dataGridViewAuto.Rows.Add();
                dataGridViewAuto.Rows[index].Cells["Auto编号"].Value = index;
                dataGridViewAuto.Rows[index].Cells["Autoip地址"].Value = AutoXml.Ip;
                dataGridViewAuto.Rows[index].Cells["Auto是否执行"].Value = AutoXml.Runnning;
                dataGridViewAuto.Rows[index].Cells["Auto等待时间"].Value = AutoXml.Time;
                dataGridViewAuto.Rows[index].Cells["Auto功能模块"].Value = AutoXml.Mode;
                dataGridViewAuto.Rows[index].Cells["Auto用例标题"].Value = AutoXml.Title;
                dataGridViewAuto.Rows[index].Cells["Auto运营商"].Value = AutoXml.Ips;
                dataGridViewAuto.Rows[index].Cells["Auto用例脚本"].Value = AutoXml.Xml;
                dataGridViewAuto.Rows[index].Cells["Auto匹配类型"].Value = AutoXml.Result;
                dataGridViewAuto.Rows[index].Cells["Auto预期"].Value = AutoXml.Result;
                dataGridViewAuto.Rows[index].Cells["Auto问题定位建议"].Value = AutoXml.Req;
            }
        }
        private void ButStartAutoRunningXML_Click(object sender, EventArgs e)
        {
            AutoCount = int.Parse(comboBoxAutoCount.Text);
            if (ButStartAutoRunningXML.Text == "开始")
            {
                if (Element.CTCC_Array.Count == 0 && Element.CUCC_Array.Count == 0 && Element.CMCC_Array.Count == 0)
                {
                    MessageBox.Show("未加载三大运营商yin文件，请点击菜单栏“工具”--“加载”运营商yin文件再试~");
                    return;
                }
                stop = false;
                CycleThread = new Thread(AutoRunningXml)
                {
                    IsBackground = true
                };
                CycleThread.Start();
                ButStartAutoRunningXML.Text = "停止";
            }
            else
            {
                stop = true;
                ButStartAutoRunningXML.Text = "开始";
            }
        }
        bool stop = false;
        bool on_off = false;
        ManualResetEvent ma;
        Thread CycleThread;
        private void AutoRunningXml()
        {
            for (int g = 0; g < AutoCount; g++)
            {
                labelAutoCount.Text = (g + 1).ToString();
                for (int i = 0; i < dataGridViewAuto.RowCount - 1; i++)
                {
                    dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = "";
                    dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = "";
                    dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "";
                }
                for (int i = 0; i < dataGridViewAuto.RowCount - 1; i++)
                {
                    // Thread.Sleep(50);
                    dataGridViewAuto.CurrentCell = dataGridViewAuto.Rows[i].Cells[0];
                    if (stop)
                    {
                        MessageBox.Show("\r\n已经停止！");
                        return;
                    }
                    if (on_off)
                    {
                        MessageBox.Show("暂停中！\r\n");
                        ma = new ManualResetEvent(false);
                        ma.WaitOne();
                    }
                    if (dataGridViewAuto.Rows[i].Cells["Auto是否执行"].Value.ToString() == "否")
                    {
                        continue;
                    }
                    int T = 50;
                    try
                    {
                        T = int.Parse(dataGridViewAuto.Rows[i].Cells["Auto等待时间"].Value.ToString());
                    }
                    catch
                    {
                    }
                    Thread.Sleep(T);
                    DateTime startTime = System.DateTime.Now;
                    dataGridViewAuto.Rows[i].Cells["Auto开始时间"].Value = DateTime.Now.ToString("HH:mm:ss");
                    if (dataGridViewAuto.Rows[i].Cells["Auto用例脚本"].Value != null && dataGridViewAuto.Rows[i].Cells["Auto预期"].Value != null)
                    {
                        if ((dataGridViewAuto.Rows[i].Cells["Auto用例脚本"].Value.ToString() != "") && (dataGridViewAuto.Rows[i].Cells["Auto预期"].Value.ToString() != ""))
                        {
                            try
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(dataGridViewAuto.Rows[i].Cells["Auto用例脚本"].Value.ToString());
                                string ip = dataGridViewAuto.Rows[i].Cells["Autoip地址"].Value.ToString();
                                int line = -1;
                                for (int l = 0; l < dataGridViewNeInformation.Rows.Count; l++)
                                {
                                    if (dataGridViewNeInformation.Rows[l].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                                    {
                                        line = l;
                                        break;
                                    }
                                    if (line >= 0)
                                        break;
                                }
                                int id = int.Parse(dataGridViewNeInformation.Rows[line].Cells["SSH_ID"].Value.ToString());
                                // string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                                string ips = dataGridViewAuto.Rows[i].Cells["Auto运营商"].Value.ToString();
                                var result = RPC.Send(xmlDoc, id, ip);
                                Thread log = new Thread(() => Lognotification(result, ip));
                                log.Start();
                                dataGridViewAuto.Rows[i].Cells["Auto结束时间"].Value = DateTime.Now.ToString("HH:mm:ss");
                                DateTime endTime = System.DateTime.Now;
                                TimeSpan ts = endTime - startTime;
                                dataGridViewAuto.Rows[i].Cells["Auto耗时"].Value = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                                //时隙
                                string[] strArrayP = dataGridViewAuto.Rows[i].Cells["Auto预期"].Value.ToString().Split(',');
                                string end = "";
                                bool key = false;
                                foreach (var item in strArrayP)
                                {
                                    if (item != "")
                                    {
                                        /* 匹配节点名称+value枚举判断 */
                                        if (dataGridViewAuto.Rows[i].Cells["Auto匹配类型"].Value.ToString().Contains("节点"))
                                        {
                                            if (Element.Element_Value(result, item, "", ips) == true)
                                            {
                                                if (ips.Contains("联通"))
                                                {
                                                    if (Element.CUCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CUCC_Array.Count; j++)
                                                        {
                                                            if (Element.CUCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是OK|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (ips.Contains("移动"))
                                                {
                                                    if (Element.CMCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CMCC_Array.Count; j++)
                                                        {
                                                            if (Element.CMCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是OK|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (ips.Contains("电信"))
                                                {
                                                    if (Element.CTCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CTCC_Array.Count; j++)
                                                        {
                                                            if (Element.CTCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是OK|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (!key)
                                                    end = end + item + "是OK|" + "\n";
                                                dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = end;
                                                dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                                                dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "通过";

                                            }
                                            else
                                            {
                                                if (ips.Contains("联通"))
                                                {
                                                    if (Element.CUCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CUCC_Array.Count; j++)
                                                        {
                                                            if (Element.CUCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是NOK参考:" + Element.CUCC_Array[j][2] + "|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (ips.Contains("移动"))
                                                {
                                                    if (Element.CMCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CMCC_Array.Count; j++)
                                                        {
                                                            if (Element.CMCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是NOK参考:" + Element.CMCC_Array[j][2] + "|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (ips.Contains("电信"))
                                                {
                                                    if (Element.CTCC_Array.Count != 0)
                                                        for (int j = 0; j < Element.CTCC_Array.Count; j++)
                                                        {
                                                            if (Element.CTCC_Array[j][0] == item)
                                                            {
                                                                end = end + item + "[枚举]是NOK参考:" + Element.CTCC_Array[j][2] + "|" + "\n";
                                                                key = true;
                                                                break;
                                                            }
                                                            key = false;
                                                        }
                                                }
                                                if (!key)
                                                    end = end + item + "是NOK|" + "\n";
                                                dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = end;
                                            }
                                        }
                                        /* 匹配节点内的值来进行包含判断 */
                                        if (dataGridViewAuto.Rows[i].Cells["Auto匹配类型"].Value.ToString().Contains("值"))
                                        {
                                            if (result.Contains(item))
                                            {
                                                end = end + item + "是OK|" + "\n";
                                                dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = end;
                                                dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                                                dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "通过";
                                            }
                                            else
                                            {
                                                end = end + item + "是NOK|" + "\n";
                                                dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = end;
                                            }
                                        }
                                    }
                                }
                                if (end.Contains("NOK"))
                                {
                                    dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "不通过";
                                }
                                dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = XmlFormat.Xml(result);
                            }
                            catch (Exception ex)
                            {
                                dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = "NOK";
                                dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = ex.ToString();
                                dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "不通过";
                                dataGridViewAuto.Rows[i].Cells["Auto结束时间"].Value = DateTime.Now.ToString("HH:mm:ss");
                                DateTime endTime = System.DateTime.Now;
                                TimeSpan ts = endTime - startTime;
                                dataGridViewAuto.Rows[i].Cells["Auto耗时"].Value = ts.Minutes.ToString() + "min:" + ts.Seconds.ToString() + "s:" + ts.Milliseconds.ToString() + "ms";
                            }
                        }
                        else
                        {
                            dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = "NOK";
                            dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = "空字符";
                            dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "不通过";
                            dataGridViewAuto.Rows[i].Cells["Auto结束时间"].Value = DateTime.Now.ToString("HH:mm:ss");
                            DateTime endTime = System.DateTime.Now;
                            TimeSpan ts = endTime - startTime;
                            dataGridViewAuto.Rows[i].Cells["Auto耗时"].Value = ts.Minutes.ToString() + "min:" + ts.Seconds.ToString() + "s:" + ts.Milliseconds.ToString() + "ms";
                        }
                    }
                    else
                    {
                        dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = "NOK";
                        dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = "Null值";
                        dataGridViewAuto.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        dataGridViewAuto.Rows[i].Cells["测试结论"].Value = "不通过";
                        dataGridViewAuto.Rows[i].Cells["Auto结束时间"].Value = DateTime.Now.ToString("HH:mm:ss");
                        DateTime endTime = System.DateTime.Now;
                        TimeSpan ts = endTime - startTime;
                        dataGridViewAuto.Rows[i].Cells["Auto耗时"].Value = ts.Minutes.ToString() + "min:" + ts.Seconds.ToString() + "s:" + ts.Milliseconds.ToString() + "ms";
                    }
                }

            }
            ButStartAutoRunningXML.Text = "开始";
            butCycleSuspend.Text = "暂停";
            MessageBox.Show("运行完成");
        }
        private void ToolStripMenuItemAUto_Click(object sender, EventArgs e)
        {
            MessageBox.Show("需要导入指定格式的excel格式文本");
            // dataGridViewAuto.DataSource = null;
            //dataGridViewAuto.Columns.Clear();
            dataGridViewAuto.Rows.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            string strPath;//完整的路径名
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strPath = ofd.FileName;
                    DataTable dataTable = null;
                    dataTable = ExcelUtility.ExcelToDataTable(strPath, true);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (dataTable.Rows[i]["ip地址"].ToString() != "")
                        {
                            int index = dataGridViewAuto.Rows.Add();
                            dataGridViewAuto.Rows[i].Cells["Auto编号"].Value = i;
                            dataGridViewAuto.Rows[i].Cells["Autoip地址"].Value = dataTable.Rows[i]["ip地址"].ToString();
                            ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[i].Cells["Auto是否执行"]).Items.Add("是");
                            ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[i].Cells["Auto是否执行"]).Items.Add("否");
                            ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[i].Cells["Auto是否执行"]).Value = dataTable.Rows[i]["是否执行"].ToString();
                            // dataGridViewAuto.Rows[i].Cells["Auto是否执行"].Value = dataTable.Rows[i]["是否执行"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto等待时间"].Value = dataTable.Rows[i]["等待时间"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto功能模块"].Value = dataTable.Rows[i]["功能模块"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto用例标题"].Value = dataTable.Rows[i]["用例标题"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto运营商"].Value = dataTable.Rows[i]["运营商"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto用例脚本"].Value = dataTable.Rows[i]["用例脚本"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto匹配类型"].Value = dataTable.Rows[i]["匹配类型"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto预期"].Value = dataTable.Rows[i]["预期"].ToString();
                            dataGridViewAuto.Rows[i].Cells["Auto问题定位建议"].Value = dataTable.Rows[i]["问题定位建议"].ToString();
                        }
                        //if (dataTable.Rows[i]["结果"] != null) {
                        //    dataGridViewAuto.Rows[i].Cells["Auto结果"].Value = dataTable.Rows[i]["结果"].ToString();
                        //}
                        //if (dataTable.Rows[i]["日志信息"] != null)
                        //{
                        //    dataGridViewAuto.Rows[i].Cells["Auto日志信息"].Value = dataTable.Rows[i]["日志信息"].ToString();
                        //}
                        //if (dataTable.Rows[i]["开始时间"] != null)
                        //{
                        //    dataGridViewAuto.Rows[i].Cells["Auto开始时间"].Value = dataTable.Rows[i]["开始时间"].ToString();
                        //}
                        //if (dataTable.Rows[i]["结束时间"] != null)
                        //{
                        //    dataGridViewAuto.Rows[i].Cells["Auto结束时间"].Value = dataTable.Rows[i]["结束时间"].ToString();
                        //}
                        //if (dataTable.Rows[i]["耗时"] != null)
                        //{
                        //    dataGridViewAuto.Rows[i].Cells["Auto耗时"].Value = dataTable.Rows[i]["耗时"].ToString();
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//捕捉异常
                    MessageBox.Show("请使用Office2003或者更新版本格式内容，如.xls或者.xlsx格式");
                }
            }
        }
        private void 导出用例报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelUtility ET = new ExcelUtility();
            ET.ExportExcel("sheet1", dataGridViewAuto, 0);
        }
        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in this.dataGridViewAuto.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        string ipadd = dataGridViewAuto.Rows[row.Index].Cells["Autoip地址"].Value.ToString();
                        string Model = dataGridViewAuto.Rows[row.Index].Cells["Auto功能模块"].Value.ToString();
                        string Title = dataGridViewAuto.Rows[row.Index].Cells["Auto用例标题"].Value.ToString();
                        string IPS = dataGridViewAuto.Rows[row.Index].Cells["Auto运营商"].Value.ToString();
                        string RPC = dataGridViewAuto.Rows[row.Index].Cells["Auto用例脚本"].Value.ToString();
                        string ExpType = dataGridViewAuto.Rows[row.Index].Cells["Auto匹配类型"].Value.ToString();
                        string Exp = dataGridViewAuto.Rows[row.Index].Cells["Auto预期"].Value.ToString();
                        string Rx = dataGridViewAuto.Rows[row.Index].Cells["Auto结果"].Value.ToString();
                        string Reply = dataGridViewAuto.Rows[row.Index].Cells["Auto日志信息"].Value.ToString();
                        string StartTime = dataGridViewAuto.Rows[row.Index].Cells["Auto开始时间"].Value.ToString();
                        string StopTime = dataGridViewAuto.Rows[row.Index].Cells["Auto结束时间"].Value.ToString();
                        string Dtime = dataGridViewAuto.Rows[row.Index].Cells["Auto耗时"].Value.ToString();
                        string Recommend = dataGridViewAuto.Rows[row.Index].Cells["Auto问题定位建议"].Value.ToString();
                        // 实例化FormInfo，并传入待修改初值  
                        var Info = new Form_AutoXmlInfo(ipadd, Model, Title, IPS, RPC, ExpType, Exp, Rx, Reply, StartTime, StopTime, Dtime, Recommend);
                        Info.ShowDialog();
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void butCycleSuspend_Click(object sender, EventArgs e)
        {
            if (butCycleSuspend.Text == "暂停")
            {
                on_off = true;
                butCycleSuspend.Text = "继续";
            }
            else
            {
                on_off = false;
                if (ma != null)
                {
                    ma.Set();
                }
                butCycleSuspend.Text = "暂停";
            }
        }
        private void ButUTC_Click(object sender, EventArgs e)
        {
            if (ButUTC.Text.Contains("UTC"))
            {
                dateTimePickerStartime.Value = DateTime.UtcNow;
                dateTimePickerEndtime.Value = DateTime.UtcNow;
                dateTimePickerStartime.CustomFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
                dateTimePickerEndtime.CustomFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
                ButUTC.Text = "北京时间";
            }
            else
            {
                dateTimePickerStartime.Value = DateTime.Now;
                dateTimePickerEndtime.Value = DateTime.Now;
                dateTimePickerStartime.CustomFormat = "yyyy-MM-ddTHH:mm:ss+08:00";
                dateTimePickerEndtime.CustomFormat = "yyyy-MM-ddTHH:mm:ss+08:00";
                ButUTC.Text = "UTC时间";
            }
        }
        private void ButQeqPort_Click(object sender, EventArgs e)
        {

            try
            {
                panel1.Controls.Add(groupBoxunivlan);
                ComPtpCtpFtp.Items.Clear();
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, true, true), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                //  root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp|//ptpsxmlns:ftps//ptpsxmlns:ftp|//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    if (name != null)
                    {
                        ComPtpCtpFtp.Items.Add(name.InnerText);
                        ComPtpCtpFtp.SelectedIndex = 0;
                    }
                }
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void ButFind_Click(object sender, EventArgs e)
        {
           
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            if (ComPtpCtpFtp.Text.Contains("PTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.PTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("FTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.FTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.CTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, true, true), id, ip);
            }
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
        }
        private void LoadTreeFromXmlDocument_TreePtpCtpFtp(XmlDocument dom)
        {
            try
            {
                // SECTION 2. Initialize the TreeView control.
                treeViewPtpCtpFtp.Nodes.Clear();
                // SECTION 3. Populate the TreeView with the DOM nodes.
                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                        continue;
                    AddNode(treeViewPtpCtpFtp.Nodes, node);
                }
                treeViewPtpCtpFtp.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void treeViewPtpCtpFtp_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            int t = e.Node.Text.IndexOf('(');
            if (t > 0)
            {
                string s1 = e.Node.Text.Substring(0, t);
                string s2 = e.Node.Text.Substring(t);
                SizeF s = e.Graphics.MeasureString(s1, this.Font);
                e.Graphics.DrawString(s1, this.Font, Brushes.Magenta, e.Bounds.X, e.Bounds.Y);
                e.Graphics.DrawString(s2, this.Font, Brushes.Blue, e.Bounds.X + s.Width, e.Bounds.Y);
            }
            else
            {
                e.Graphics.DrawString(e.Node.Text, this.Font, Brushes.Magenta, e.Bounds.X, e.Bounds.Y);
            }
        }
        private void ButModifyLayer_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            if (ComPtpCtpFtp.Text.Contains("PTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(ModifyXML.Layer_protocal_name(ComPtpCtpFtp.Text, ComLayer.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("FTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
            FindPort();
        }
        private void oDUFlex带宽调整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string _odu__ctp_name = "", _position = "", _action = "", _current_number_of_tributary_slots = "", _ts_detail = "", _timeout = "";
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + "\r\n" + connection;
                    }
                }
                if (MessageBox.Show("正在配置当前业务的OAM:\r\n" + allconnection + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            _odu__ctp_name = dataGridViewEth.Rows[row.Index].Cells["CTP端口1"].Value.ToString();
                            // 实例化FormInfo，并传入待修改初值  
                            var FormModifyOdu = new Form_Modify_Odu(_odu__ctp_name);
                            // 以对话框方式显示FormInfo  
                            if (FormModifyOdu.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _odu__ctp_name = FormModifyOdu._odu__ctp_name;
                                _position = FormModifyOdu._position;
                                _action = FormModifyOdu._action;
                                _current_number_of_tributary_slots = FormModifyOdu._current_number_of_tributary_slots;
                                _ts_detail = FormModifyOdu._ts_detail;
                                _timeout = FormModifyOdu._timeout;
                                string messg = Creat(CreateODU.Modify_Odu_Connection(_odu__ctp_name, _position, _action, _current_number_of_tributary_slots, _ts_detail, _timeout, ips), id, ip);
                                MessageBox.Show(messg);
                            }
                        }
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void oDUK在线时延测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string _name = "", _odu_delay_enable = "", _delay = "", _last_update_time = "", _server_tp = "", _odu_signal_type = "", _adaptation_type = "", _switch_capability = "", _pmtx = "", _pmexp = "", _pmrx = "";
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + "\r\n" + connection;
                    }
                }
                if (MessageBox.Show("正在配置当前的oduflex:\r\n" + allconnection + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            _name = dataGridViewEth.Rows[row.Index].Cells["CTP端口1"].Value.ToString();
                            string[] strArray = _name.Split(',');
                            foreach (var item in strArray)
                            {
                                if (item != "")
                                {
                                    if (!item.Contains("FTP") && !(item.Contains("port=10")))
                                        _name = item;
                                }
                            }
                            try
                            {
                                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                XmlDocument xmlDoc = new XmlDocument();
                                //xmlDoc.Load(filename);
                                xmlDoc = Sendrpc(GetXML.CTP(_name), id, ip);
                                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                root.AddNamespace("ctpxmlns", "urn:ccsa:yang:acc-otn");
                                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                                foreach (XmlNode itemNode in itemNodes)
                                {
                                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                                    XmlNode server_tp = itemNode.SelectSingleNode("ptpsxmlns:server-tp", root);
                                    if (layer_protocol_name != null && server_tp != null)
                                    {
                                        _server_tp = server_tp.InnerText;
                                        if (layer_protocol_name.InnerText.Contains("ODU"))
                                        {
                                            if (name != null)
                                            {
                                                if (_name == name.InnerText)
                                                {
                                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("ctpxmlns:odu-ctp-pac", root);
                                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                                    {
                                                        if (ips.Contains("移动"))
                                                        {
                                                            return;
                                                            //移动不支持
                                                        }
                                                        if (ips.Contains("联通"))
                                                        {
                                                            XmlNode odu_signal_type = itemNodeOdu.SelectSingleNode("//ctpxmlns:odu-signal-type", root);
                                                            XmlNode adaptation_type = itemNodeOdu.SelectSingleNode("//ctpxmlns:adaptation-type", root);
                                                            XmlNode switch_capability = itemNodeOdu.SelectSingleNode("//ctpxmlns:switch-capability", root);
                                                            XmlNode odu_ctp_delay_enable = itemNodeOdu.SelectSingleNode("//ctpxmlns:odu-ctp-delay-enable", root);
                                                            XmlNode delay = itemNodeOdu.SelectSingleNode("//ctpxmlns:delay", root);
                                                            XmlNode last_update_time = itemNodeOdu.SelectSingleNode("//ctpxmlns:last-update-time", root);
                                                            XmlNode pmtrail_trace_actual_tx = itemNodeOdu.SelectSingleNode("//ctpxmlns:pmtrail-trace-actual-tx", root);
                                                            XmlNode pmtrail_trace_actual_rx = itemNodeOdu.SelectSingleNode("//ctpxmlns:pmtrail-trace-actual-rx", root);
                                                            XmlNode pmtrail_trace_expected_rx = itemNodeOdu.SelectSingleNode("//ctpxmlns:pmtrail-trace-expected-rx", root);
                                                            if (odu_signal_type != null) { _odu_signal_type = odu_signal_type.InnerText; }
                                                            if (adaptation_type != null) { _adaptation_type = adaptation_type.InnerText; }
                                                            if (switch_capability != null) { _switch_capability = switch_capability.InnerText; }
                                                            if (odu_ctp_delay_enable != null) { _odu_delay_enable = odu_ctp_delay_enable.InnerText; }
                                                            if (delay != null) { _delay = delay.InnerText; }
                                                            if (last_update_time != null) { _last_update_time = last_update_time.InnerText; }
                                                            if (pmtrail_trace_actual_tx != null) { _pmtx = pmtrail_trace_actual_tx.InnerText; }
                                                            if (pmtrail_trace_actual_rx != null) { _pmrx = pmtrail_trace_actual_rx.InnerText; }
                                                            if (pmtrail_trace_expected_rx != null) { _pmexp = pmtrail_trace_expected_rx.InnerText; }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                // Console.Read();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                            }
                            // 实例化FormInfo，并传入待修改初值  
                            var FormODUkDelay = new Form_ODUk_Delay(_name, _odu_delay_enable, _delay, _last_update_time, _odu_signal_type, _adaptation_type, _switch_capability, _pmtx, _pmexp, _pmrx);
                            // 以对话框方式显示FormInfo  
                            if (FormODUkDelay.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _name = FormODUkDelay._name;
                                _odu_delay_enable = FormODUkDelay._odu_delay_enable;
                                string messg = Creat(ModifyXML.Odu_ctp_delay(_name, _odu_delay_enable), id, ip);
                                MessageBox.Show(messg);
                            }
                        }
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 新增网元ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalConnectionXml localxml = new LocalConnectionXml();
            // 实例化FormInfo，并传入待修改初值  
            int ipaddresscunt = 0;
            int ipaddresscunt1 = 1;
            bool idtf = false;
            var LoginOn = new From_Add_User(gpnip, 830, gpnuser, gpnpassword, gpnnetconfversion, ips, gpnname);
            // 以对话框方式显示FormInfo  
            if (LoginOn.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                gpnip = LoginOn.IP;
                gpnport = LoginOn.PORT;
                gpnuser = LoginOn.USER;
                gpnpassword = LoginOn.PASSD;
                gpnnetconfversion = LoginOn.VER;
                gpnname = LoginOn.NeName;
                ips = LoginOn.IPS;
                if (!File.Exists(neinfopath))
                {
                    localxml.CreatXmlTree(neinfopath, LoginOn.IP, LoginOn.PORT, LoginOn.USER, LoginOn.PASSD, ipaddresscunt, LoginOn.NeName, LoginOn.IPS);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(neinfopath);
                    if (doc == null)
                    {
                        MessageBox.Show("XML本地网元信息不存在");
                        return;
                    }
                    else
                    {
                        ipaddresscunt = doc.SelectNodes("/neinfo/ipaddress").Count + 1;
                        XmlElement root = doc.DocumentElement;
                        XmlNodeList nodes = root.ChildNodes;
                        if (nodes.Count == 0)
                        {
                            ipaddresscunt = 0;
                            localxml.Add(neinfopath, LoginOn.IP, LoginOn.PORT, LoginOn.USER, LoginOn.PASSD, ipaddresscunt, LoginOn.NeName, LoginOn.IPS);
                        }
                        else
                        {
                            int[] id = new int[nodes.Count];
                            for (int i = 0; i < nodes.Count; i++)
                            {
                                XmlNode el = nodes[i];
                                id[i] = int.Parse(el.Attributes["id"].Value);
                            }
                            for (int i = nodes.Count; i >= 1; i--)
                            {
                                ipaddresscunt1 = ipaddresscunt - i;
                                idtf = false;
                                if (ipaddresscunt1 == nodes.Count && ipaddresscunt1 == id.Max())
                                {
                                    localxml.Add(neinfopath, LoginOn.IP, LoginOn.PORT, LoginOn.USER, LoginOn.PASSD, ipaddresscunt, LoginOn.NeName, LoginOn.IPS);
                                }
                                for (int j = 0; j < nodes.Count; j++)
                                {
                                    if (ipaddresscunt1 == id[j])
                                    {
                                        idtf = true;
                                    }
                                }
                                if (!idtf)
                                {
                                    ipaddresscunt = ipaddresscunt1;
                                    localxml.Add(neinfopath, LoginOn.IP, LoginOn.PORT, LoginOn.USER, LoginOn.PASSD, ipaddresscunt1, LoginOn.NeName, LoginOn.IPS);
                                    break;
                                }
                            }
                            //foreach (XmlNode task in nodes)
                            //{
                            //    MessageBox.Show(task.Attributes["id"].Value);
                            //}
                        }
                    }
                }
                Gpnsetini();
                TextIP.Text = gpnip;
                if (File.Exists(neinfopath))
                {
                    int index = dataGridViewNeInformation.Rows.Add();
                    dataGridViewNeInformation.Rows[index].Cells["网元ip"].Value = LoginOn.IP;
                    dataGridViewNeInformation.Rows[index].Cells["用户名"].Value = LoginOn.USER;
                    dataGridViewNeInformation.Rows[index].Cells["密码"].Value = LoginOn.PASSD;
                    dataGridViewNeInformation.Rows[index].Cells["SSH_ID"].Value = ipaddresscunt;
                    dataGridViewNeInformation.Rows[index].Cells["运营商"].Value = LoginOn.IPS;
                    TreeNode node = new TreeNode();
                    node.Tag = ipaddresscunt.ToString();
                    node.Name = ipaddresscunt.ToString();
                    if (string.IsNullOrEmpty(LoginOn.NeName))
                    {
                        node.Text = LoginOn.IP;
                        dataGridViewNeInformation.Rows[index].Cells["网元名称"].Value = LoginOn.IP;
                    }
                    else
                    {
                        node.Text = LoginOn.NeName;
                        dataGridViewNeInformation.Rows[index].Cells["网元名称"].Value = LoginOn.NeName;
                    }
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 4;
                    treeViewNEID.Nodes.Add(node);
                }
            }
        }
        private void LoginNetconf(string ip, int port, string user, string passd, int id, int rowindex)
        {
            string nename = "";
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["网元ip"].Value.ToString() == ip) //keyword要查的关键字
                {
                    nename = dataGridViewNeInformation.Rows[i].Cells["网元名称"].Value.ToString();
                    break;
                }
            }
            try
            {
                DateTime dTimeEnd = System.DateTime.Now;
                DateTime dTimeServer = System.DateTime.Now;
                //netConfClient.Add(new NetConfClient(ip, port, user, passd));
                netConfClient[id] = new NetConfClient(ip, port, user, passd);
                netConfClient[id].Connect();
                if (netConfClient[id].IsConnected)
                {
                    BeginInvoke(new MethodInvoker(delegate ()
                    {
                        dTimeServer = System.DateTime.Now;
                        TimeSpan ts = dTimeServer - dTimeEnd;
                        LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                        TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                        TextLog.AppendText(XmlFormat.Xml(netConfClient[id].ServerCapabilities.OuterXml) + FenGeFu);
                        TextLog.AppendText("网元：" + nename + " " + System.DateTime.Now.ToString() + "请求：" + FenGeFu);
                        TextLog.AppendText(XmlFormat.Xml(netConfClient[id].ClientCapabilities.OuterXml) + FenGeFu);
                        netConfClient[id].OperationTimeout = TimeSpan.FromSeconds(15);
                        netConfClient[id].TimeOut = int.Parse(ComTimeOut.Text) * 1000;
                        dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Value = "连接成功";
                        dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Style.BackColor = Color.GreenYellow;

                        if (dataGridViewNeInformation.Rows[rowindex].Cells["运营商"].Value != null)
                        {
                            ips = dataGridViewNeInformation.Rows[rowindex].Cells["运营商"].Value.ToString();
                            toolStripStatusLabelips.Text = ips;
                            Thread sdhthread = new Thread(() => SDHchengetype(ips));
                            sdhthread.Start();
                        }
                        if (dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Value != null)
                        {
                            LabConncet.Text = dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Value.ToString();
                        }
                        if (dataGridViewNeInformation.Rows[rowindex].Cells["网元ip"].Value != null)
                        {
                            TextIP.Text = dataGridViewNeInformation.Rows[rowindex].Cells["网元ip"].Value.ToString();
                        }
                        for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                        {
                            if (treeViewNEID.Nodes[i].Name == id.ToString())
                            {
                                MessageBox.Show(treeViewNEID.Nodes[i].Text + "：连接成功！");
                                //treeViewNEID.Nodes[i].ForeColor = Color.Green;
                                treeViewNEID.Nodes[i].ImageIndex = 2;
                                treeViewNEID.Nodes[i].SelectedImageIndex = 2;
                                break;
                            }
                        }
                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc = Sendrpc(GetXML.ME(), id, ip);
                            XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                            root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                            root.AddNamespace("me", "urn:ccsa:yang:acc-devm");
                            XmlNodeList itemNodes = xmlDoc.SelectNodes("//me:me", root);
                            foreach (XmlNode itemNode in itemNodes)
                            {
                                XmlNode name = itemNode.SelectSingleNode("me:name", root);
                                XmlNode status = itemNode.SelectSingleNode("me:status", root);
                                XmlNode ip_address = itemNode.SelectSingleNode("me:ip-address", root);
                                XmlNode mask = itemNode.SelectSingleNode("me:mask", root);
                                XmlNode ntp_enable = itemNode.SelectSingleNode("me:ntp-enable", root);
                                XmlNode gate_way1 = itemNode.SelectSingleNode("me:gate-way1", root);
                                XmlNode uuid = itemNode.SelectSingleNode("me:uuid", root);
                                XmlNode manufacturer = itemNode.SelectSingleNode("me:manufacturer", root);
                                XmlNode product_name = itemNode.SelectSingleNode("me:product-name", root);
                                XmlNode software_version = itemNode.SelectSingleNode("me:software-version", root);
                                XmlNode hardware_version = itemNode.SelectSingleNode("me:hardware-version", root);
                                XmlNode device_type = itemNode.SelectSingleNode("me:device-type", root);
                                XmlNode layer_protocol_nameeth = itemNode.SelectSingleNode("me:layer-protocol-name[1]/text()[1]", root);
                                XmlNode layer_protocol_namesdh = itemNode.SelectSingleNode("me:layer-protocol-name[2]/text()[1]", root);
                                XmlNode layer_protocol_nameotn = itemNode.SelectSingleNode("me:layer-protocol-name[3]/text()[1]", root);
                                XmlNodeList eq = itemNode.SelectNodes("me:eq", root);
                                XmlNode ntp_state = itemNode.SelectSingleNode("me:ntp-state", root);
                                if (name != null)
                                {
                                    textBox_me_name.Text = name.InnerText;
                                }
                                if (status != null)
                                {
                                    textBox_me_status.Text = status.InnerText;
                                }
                                if (ip_address != null)
                                {
                                    textBox_me_ip_address.Text = ip_address.InnerText;
                                }
                                if (mask != null)
                                {
                                    textBox_me_mask.Text = mask.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["子网掩码"].Value = mask.InnerText;
                                }
                                if (ntp_state != null)
                                {
                                    textBox_me_ntp_state.Text = ntp_state.InnerText;
                                }
                                if (ntp_enable != null)
                                {
                                    textBox_me_ntp_enable.Text = ntp_enable.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["NTP"].Value = ntp_enable.InnerText;
                                }
                                if (gate_way1 != null)
                                {
                                    textBox_me_gate_way1.Text = gate_way1.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["网关1"].Value = gate_way1.InnerText;
                                }
                                if (uuid != null)
                                {
                                    textBox_me_uuid.Text = uuid.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["UUID"].Value = uuid.InnerText;
                                }
                                if (manufacturer != null) textBox_me_manufacturer.Text = manufacturer.InnerText;
                                if (product_name != null)
                                {
                                    textBox_me_product_name.Text = product_name.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["设备名称"].Value = product_name.InnerText;
                                }
                                if (software_version != null)
                                {
                                    textBox_me_software_version.Text = software_version.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["网元软件版本"].Value = software_version.InnerText;
                                }
                                if (hardware_version != null)
                                {
                                    textBox_me_hardware_version.Text = hardware_version.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["网元硬件版本"].Value = hardware_version.InnerText;
                                }
                                if (device_type != null)
                                {
                                    textBox_me_device_type.Text = device_type.InnerText;
                                    dataGridViewNeInformation.Rows[rowindex].Cells["设备类型"].Value = device_type.InnerText;
                                }
                                string layer_protocol_name_eth = "";
                                string layer_protocol_name_sdh = "";
                                string layer_protocol_name_otn = "";
                                string layer_protocol_name_all = "";
                                if (layer_protocol_nameeth != null)
                                {
                                    layer_protocol_name_eth = layer_protocol_nameeth.InnerText;
                                }
                                if (layer_protocol_namesdh != null)
                                {
                                    layer_protocol_name_sdh = layer_protocol_namesdh.InnerText;
                                }
                                if (layer_protocol_nameotn != null)
                                {
                                    layer_protocol_name_otn = layer_protocol_nameotn.InnerText;
                                }
                                layer_protocol_name_all = layer_protocol_name_eth + layer_protocol_name_sdh + layer_protocol_name_otn;
                                layer_protocol_name_all = layer_protocol_name_all.Replace("acc-eth:", "");
                                layer_protocol_name_all = layer_protocol_name_all.Replace("acc-sdh", "");
                                layer_protocol_name_all = layer_protocol_name_all.Replace("acc-otn", "");
                                if (layer_protocol_name_all != null)
                                {
                                    textBox_me_protocol_name.Text = layer_protocol_name_all;
                                }
                                if (eq != null) textBox_me_eq.Text = eq.Count.ToString();
                                XmlNode mc_port = itemNode.SelectSingleNode("me:mc-port", root);
                                XmlNodeList net = itemNode.SelectNodes("//me:ntp-server", root);
                                foreach (XmlNode itemNode1 in net)
                                {
                                    XmlNode ntpname = itemNode1.SelectSingleNode("me:name", root);
                                    XmlNode ntpip = itemNode1.SelectSingleNode("me:ip-address", root);
                                    XmlNode ntpport = itemNode1.SelectSingleNode("me:port", root);
                                    XmlNode ntpversion = itemNode1.SelectSingleNode("me:ntp-version", root);
                                    if (ntpname != null) textBox_me_ntp_server_name.Text = ntpname.InnerText;
                                    if (ntpip != null) textBox_me_ntp_server_ipaddress.Text = ntpip.InnerText;
                                    if (ntpport != null) textBox_me_ntp_server_port.Text = ntpport.InnerText;
                                    if (ntpversion != null) textBox_me_ntp_server_version.Text = ntpversion.InnerText;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                        }
                    }));
                }
                else
                {
                  
                    dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Value = "连接失败";
                    dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Style.BackColor = Color.Yellow;
                    for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                    {
                        if (treeViewNEID.Nodes[i].Name == id.ToString())
                        {

                            MessageBox.Show(treeViewNEID.Nodes[i].Text + "：连接失败！");
                            treeViewNEID.Nodes[i].ImageIndex = 3;
                            treeViewNEID.Nodes[i].SelectedImageIndex = 3;
                            break;
                        }
                    }
                    dTimeServer = System.DateTime.Now;
                    TimeSpan ts = dTimeServer - dTimeEnd;
                    LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                }
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
                dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Value = "连接失败";
                dataGridViewNeInformation.Rows[rowindex].Cells["连接状态"].Style.BackColor = Color.Yellow;
                for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                {
                    if (treeViewNEID.Nodes[i].Name == id.ToString())
                    {
                        MessageBox.Show(treeViewNEID.Nodes[i].Text + "：连接失败！");
                        treeViewNEID.Nodes[i].ImageIndex = 3;
                        treeViewNEID.Nodes[i].SelectedImageIndex = 3;
                        break;
                    }
                }
            }
        }
        private void 上线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => OnLinedevm());
            thread.Start();
        }
        private void OnLinedevm()
        {
            try
            {
                string neipall = "";
                string neip = "";
                int id = 1;
                string user = "";
                string password = "";
                int port = 0;
                foreach (DataGridViewRow row in this.dataGridViewNeInformation.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();       //设备IP地址
                        neipall = neipall + "\r\n" + neip;
                    }
                }
                if (MessageBox.Show("当前设备:" + neipall + "\r\n是否上线？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridViewNeInformation.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {


                            if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value != null)
                            {
                                if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value.ToString() == "已开启")
                                {
                                    ThSub[row.Index].Abort();
                                    Sub[id] = false;
                                    //netConfClient[id].SendReceiveRpcKeepLive();
                                    dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value = "已关闭";
                                }
                            }
                            //  Thread.Sleep(3000);
                            if (netConfClient[id] != null)
                            {
                                if (netConfClient[id].IsConnected)
                                {
                                    netConfClient[id].Disconnect();
                                }
                            }

                            dataGridViewNeInformation.Rows[row.Index].Cells["连接状态"].Style.BackColor = Color.White;
                            dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Style.BackColor = Color.White;
                            dataGridViewNeInformation.Rows[row.Index].Cells["连接状态"].Value = "连接中...";
                            dataGridViewNeInformation.Rows[row.Index].Cells["设备名称"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["设备类型"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["网元软件版本"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["网元硬件版本"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["UUID"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["NTP"].Value = "";
                            neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();
                            id = int.Parse(dataGridViewNeInformation.Rows[row.Index].Cells["SSH_ID"].Value.ToString());
                            port = 830;
                            user = dataGridViewNeInformation.Rows[row.Index].Cells["用户名"].Value.ToString();
                            password = dataGridViewNeInformation.Rows[row.Index].Cells["密码"].Value.ToString();

                            if (treeViewNEID.Nodes.Count > row.Index && row.Index >= 0)
                            {
                                treeViewNEID.SelectedNode = treeViewNEID.Nodes[row.Index];//选中
                            }
                                for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                            {
                                if (treeViewNEID.Nodes[i].Name == id.ToString())
                                {
                                    treeViewNEID.Nodes[i].ForeColor = Color.Black;
                                    treeViewNEID.Nodes[i].ImageIndex = 0;
                                    treeViewNEID.Nodes[i].SelectedImageIndex = 4;
                                    break;
                                }
                            }

                            Thread thread = new Thread(() => LoginNetconf(neip, port, user, password, id, row.Index));
                            thread.Start();
                            //LoginNetconf(neip, port, user, password, id, row.Index);
                        }
                        Thread.Sleep(2000);
                    }
                   // MessageBox.Show(neipall + "\r\n准备开始上线！");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dataGridViewNeInformation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 15)
            {
                if (e.Value != null && e.Value.ToString().Length > 0)
                {
                    e.Value = new string('*', e.Value.ToString().Length);
                }
            }
        }
        private void 离线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string neipall = "";
                string neip = "";
                int id = 1;
                string user = "";
                string password = "";
                foreach (DataGridViewRow row in this.dataGridViewNeInformation.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();       //设备IP地址
                        neipall = neipall + "\r\n" + neip;
                    }
                }
                if (MessageBox.Show("正在离线当前设备:" + neipall + "\r\n是否离线？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridViewNeInformation.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();
                            id = int.Parse(dataGridViewNeInformation.Rows[row.Index].Cells["SSH_ID"].Value.ToString());
                            user = dataGridViewNeInformation.Rows[row.Index].Cells["用户名"].Value.ToString();
                            password = dataGridViewNeInformation.Rows[row.Index].Cells["密码"].Value.ToString();
                            if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value != null)
                            {
                                if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value.ToString() == "已开启")
                                {
                                    ThSub[row.Index].Abort();
                                    Sub[id] = false;
                                    //netConfClient[id].SendReceiveRpcKeepLive();
                                    dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value = "已关闭";
                                }
                            }
                            //  Thread.Sleep(3000);
                            if (netConfClient[id] != null)
                            {
                                if (netConfClient[id].IsConnected)
                                {
                                    netConfClient[id].Disconnect();
                                }
                            }
                            dataGridViewNeInformation.Rows[row.Index].Cells["连接状态"].Style.BackColor = Color.White;
                            dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Style.BackColor = Color.White;
                            dataGridViewNeInformation.Rows[row.Index].Cells["连接状态"].Value = "已断开";
                            dataGridViewNeInformation.Rows[row.Index].Cells["设备名称"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["设备类型"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["网元软件版本"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["网元硬件版本"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["UUID"].Value = "";
                            dataGridViewNeInformation.Rows[row.Index].Cells["NTP"].Value = "";
                            for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                            {
                                if (treeViewNEID.Nodes[i].Name == id.ToString())
                                {
                                    treeViewNEID.Nodes[i].ForeColor = Color.Black;
                                    treeViewNEID.Nodes[i].ImageIndex = 0;
                                    treeViewNEID.Nodes[i].SelectedImageIndex = 4;
                                    break;
                                }
                            }
                        }
                    }
                    //  MessageBox.Show(neipall+"\r\n已离线！");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
            }
        }
        private void 删除网元ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string neipall = "";
                string neip = "";
                int id = 1;
                string user = "";
                string password = "";
                foreach (DataGridViewRow row in this.dataGridViewNeInformation.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();       //设备IP地址
                        neipall = neipall + "\r\n" + neip;
                    }
                }
                if (MessageBox.Show("正在删除当前设备:" + neipall + "\r\n是否删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridViewNeInformation.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();
                            id = int.Parse(dataGridViewNeInformation.Rows[row.Index].Cells["SSH_ID"].Value.ToString());
                            user = dataGridViewNeInformation.Rows[row.Index].Cells["用户名"].Value.ToString();
                            password = dataGridViewNeInformation.Rows[row.Index].Cells["密码"].Value.ToString();
                            if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value != null)
                            {
                                if (dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value.ToString() == "已开启")
                                {
                                    Sub[id] = false;
                                    netConfClient[id].SendReceiveRpcKeepLive();
                                }
                            }
                            LocalConnectionXml delete = new LocalConnectionXml();
                            delete.Delete(neinfopath, id);
                            treeViewNEID.Nodes.RemoveByKey(id.ToString());
                            dataGridViewNeInformation.Rows.Remove(row);
                            if (netConfClient[id] != null)
                            {
                                if (netConfClient[id].IsConnected)
                                {
                                    netConfClient[id].Disconnect();
                                }
                            }
                        }
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
            }
        }
        private void treeViewNEID_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)  //单击鼠标左键才响应
            {
                treeViewNEID.SelectedNode = e.Node;
                if (dataGridViewNeInformation.Rows[e.Node.Index].Cells["运营商"].Value != null)
                {
                    ips = dataGridViewNeInformation.Rows[e.Node.Index].Cells["运营商"].Value.ToString();
                    toolStripStatusLabelips.Text = ips;
                    Thread thread = new Thread(() => SDHchengetype(ips));
                    thread.Start();
                }
                if (dataGridViewNeInformation.Rows[e.Node.Index].Cells["连接状态"].Value != null)
                {
                    LabConncet.Text = dataGridViewNeInformation.Rows[e.Node.Index].Cells["连接状态"].Value.ToString();
                }
                if (dataGridViewNeInformation.Rows[e.Node.Index].Cells["订阅"].Value != null)
                {
                    TextSub.Text = dataGridViewNeInformation.Rows[e.Node.Index].Cells["订阅"].Value.ToString();
                }
                if (dataGridViewNeInformation.Rows[e.Node.Index].Cells["网元ip"].Value != null)
                {
                    TextIP.Text = dataGridViewNeInformation.Rows[e.Node.Index].Cells["网元ip"].Value.ToString();
                }
                // treeViewNEID.SelectedNode.ImageIndex = 2;
                //treeViewNEID.SelectedNode.SelectedImageIndex = 2;
                //  dataGridViewNeInformation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //   dataGridViewNeInformation.Rows[e.Node.Index].Selected = true;
                dataGridViewNeInformation.CurrentCell = dataGridViewNeInformation.Rows[e.Node.Index].Cells["网元ip"];
            }
        }
        private void SDHchengetype(string ips)
        {
            if (ips.Contains("联通"))
            {
                ComEosSdhSignalType.Items.Clear();
                ComEosSdhSignalTypeProtect.Items.Clear();
                ComVCType.Items.Clear();
                ComSdhUniSdhType.Items.Clear();
                ComSdhNniSdhtype_primary_nni.Items.Clear();
                ComSdhNniVcType_primary_nni.Items.Clear();
                ComSdhNniSdhtype_secondary_nni.Items.Clear();
                ComSdhNniVcType_secondary_nni.Items.Clear();
                ComSdhUniSdhType_Secondary.Items.Clear();
                ComSdhNniSdhtype_primary_nni2.Items.Clear();
                ComSdhNniVcType_primary_nni2.Items.Clear();
                ComSdhNniSdhtype_secondary_nni2.Items.Clear();
                ComSdhNniVcType_secondary_nni2.Items.Clear();
                ComEthServiceMappingMode.Enabled = false;
                ComSdhSerMap.Enabled = false;
                string[] collectionSDH = new string[4] { "STM1", "STM4", "STM16", "STM64" };
                string[] collectionVC = new string[3] { "VC12", "VC3", "VC4" };
                foreach (var item in collectionSDH)
                {
                    ComEosSdhSignalType.Items.Add(item);
                    ComEosSdhSignalTypeProtect.Items.Add(item);
                    ComSdhNniSdhtype_primary_nni.Items.Add(item);
                    ComSdhNniSdhtype_secondary_nni.Items.Add(item);
                    ComSdhNniSdhtype_primary_nni2.Items.Add(item);
                    ComSdhNniSdhtype_secondary_nni2.Items.Add(item);
                }
                foreach (var item in collectionVC)
                {
                    ComVCType.Items.Add(item);
                    ComSdhUniSdhType.Items.Add(item);
                    ComSdhUniSdhType_Secondary.Items.Add(item);
                    ComSdhNniVcType_primary_nni.Items.Add(item);
                    ComSdhNniVcType_secondary_nni.Items.Add(item);
                    ComSdhNniVcType_primary_nni2.Items.Add(item);
                    ComSdhNniVcType_secondary_nni2.Items.Add(item);
                }
                ComEosSdhSignalType.SelectedIndex = 2;
                ComEosSdhSignalTypeProtect.SelectedIndex = 2;
                ComVCType.SelectedIndex = 2;
                ComSdhUniSdhType.SelectedIndex = 2;
                ComSdhNniSdhtype_primary_nni.SelectedIndex = 2;
                ComSdhNniVcType_primary_nni.SelectedIndex = 2;
                ComSdhNniSdhtype_secondary_nni.SelectedIndex = 2;
                ComSdhNniVcType_secondary_nni.SelectedIndex = 2;
                ComSdhUniSdhType_Secondary.SelectedIndex = 2;
                ComSdhNniSdhtype_primary_nni2.SelectedIndex = 2;
                ComSdhNniVcType_primary_nni2.SelectedIndex = 2;
                ComSdhNniSdhtype_secondary_nni2.SelectedIndex = 2;
                ComSdhNniVcType_secondary_nni2.SelectedIndex = 2;
            }
            if (ips.Contains("移动"))
            {
                ComEosSdhSignalType.Items.Clear();
                ComEosSdhSignalTypeProtect.Items.Clear();
                ComVCType.Items.Clear();
                ComSdhUniSdhType.Items.Clear();
                ComSdhNniSdhtype_primary_nni.Items.Clear();
                ComSdhNniVcType_primary_nni.Items.Clear();
                ComSdhNniSdhtype_secondary_nni.Items.Clear();
                ComSdhNniVcType_secondary_nni.Items.Clear();
                ComSdhUniSdhType_Secondary.Items.Clear();
                ComSdhNniSdhtype_primary_nni2.Items.Clear();
                ComSdhNniVcType_primary_nni2.Items.Clear();
                ComSdhNniSdhtype_secondary_nni2.Items.Clear();
                ComSdhNniVcType_secondary_nni2.Items.Clear();
                ComEthServiceMappingMode.Enabled = true;
                ComSdhSerMap.Enabled = true;
                string[] collectionSDH = new string[4] { "STM-1", "STM-4", "STM-16", "STM-64" };
                string[] collectionVC = new string[3] { "VC-12", "VC-3", "VC-4" };
                foreach (var item in collectionSDH)
                {
                    ComEosSdhSignalType.Items.Add(item);
                    ComEosSdhSignalTypeProtect.Items.Add(item);
                    ComSdhNniSdhtype_primary_nni.Items.Add(item);
                    ComSdhNniSdhtype_secondary_nni.Items.Add(item);
                    ComSdhNniSdhtype_primary_nni2.Items.Add(item);
                    ComSdhNniSdhtype_secondary_nni2.Items.Add(item);
                }
                foreach (var item in collectionVC)
                {
                    ComVCType.Items.Add(item);
                    ComSdhUniSdhType.Items.Add(item);
                    ComSdhUniSdhType_Secondary.Items.Add(item);
                    ComSdhNniVcType_primary_nni.Items.Add(item);
                    ComSdhNniVcType_secondary_nni.Items.Add(item);
                    ComSdhNniVcType_primary_nni2.Items.Add(item);
                    ComSdhNniVcType_secondary_nni2.Items.Add(item);
                }
                ComEosSdhSignalType.SelectedIndex = 2;
                ComEosSdhSignalTypeProtect.SelectedIndex = 2;
                ComVCType.SelectedIndex = 2;
                ComSdhUniSdhType.SelectedIndex = 2;
                ComSdhNniSdhtype_primary_nni.SelectedIndex = 2;
                ComSdhNniVcType_primary_nni.SelectedIndex = 2;
                ComSdhNniSdhtype_secondary_nni.SelectedIndex = 2;
                ComSdhNniVcType_secondary_nni.SelectedIndex = 2;
                ComSdhUniSdhType_Secondary.SelectedIndex = 2;
                ComSdhNniSdhtype_primary_nni2.SelectedIndex = 2;
                ComSdhNniVcType_primary_nni2.SelectedIndex = 2;
                ComSdhNniSdhtype_secondary_nni2.SelectedIndex = 2;
                ComSdhNniVcType_secondary_nni2.SelectedIndex = 2;
                if (Element.CMCC_Array.Count != 0)
                {

                    for (int g = 0; g < Element.CMCC_Array.Count; g++)
                    {
                        if (Element.CMCC_Array[g][0] == "protection-type")
                        {
                            Com_Eth_uni_protection_type.Items.Clear();
                            Com_nni_protection_type.Items.Clear();
                            Com_nni2_protection_type.Items.Clear();
                            Com_uni_protection_type.Items.Clear();
                            ComSdhPro.Items.Clear();
                            Com_Eth_uni_protection_type.Items.Add("无");
                            Com_nni_protection_type.Items.Add("无");
                            Com_nni2_protection_type.Items.Add("无");
                            Com_uni_protection_type.Items.Add("无");
                            ComSdhPro.Items.Add("无");
                            string[] value_ = Element.CMCC_Array[g][2].Split(',');
                            for (int l = 0; l < value_.Length; l++)
                            {
                               
                                Com_Eth_uni_protection_type.Items.Add(value_[l]);
                                Com_nni_protection_type.Items.Add(value_[l]);
                                Com_nni2_protection_type.Items.Add(value_[l]);
                                Com_uni_protection_type.Items.Add(value_[l]);
                                ComSdhPro.Items.Add(value_[l]);

                            }
                            Com_Eth_uni_protection_type.SelectedIndex = 0;
                            Com_nni_protection_type.SelectedIndex = 0;
                            Com_nni2_protection_type.SelectedIndex = 0;
                            Com_uni_protection_type.SelectedIndex = 0;
                            ComSdhPro.SelectedIndex = 0;

                        }
                        //移动不是枚举的类型
                        //if (Element.CMCC_Array[g][0] == "adaptation-type")
                        //{
                        //    ComEthServiceMappingMode.Items.Clear();
                        //    ComSdhSerMap.Items.Clear();
                        //    ComEthServiceMappingMode.Items.Add("无");
                        //    ComSdhSerMap.Items.Add("无");
                        //    string[] value_ = Element.CMCC_Array[g][2].Split(',');
                        //    for (int l = 0; l < value_.Length; l++)
                        //    {
                        //        ComEthServiceMappingMode.Items.Add(value_[l]);
                        //        ComSdhSerMap.Items.Add(value_[l]);
                        //    }
                        //}
                        ComEthServiceMappingMode.Items.Clear();
                        ComSdhSerMap.Items.Clear();
                        string[] adaptation_type = { "CBR-AMP", "CBR-BMP", "GFP-T", "GFP-F", "NULL", "PRBS", "CBRx", "GMP", "ODUij", "ODUj21", "ODUk" };
                        foreach (var item in adaptation_type)
                        {
                             ComEthServiceMappingMode.Items.Add(item);
                             ComSdhSerMap.Items.Add(item);
                        }


                    }
                }

            }
            if (ips.Contains("电信")) {
                ComEthServiceMappingMode.Enabled = true;
                ComSdhSerMap.Enabled = true;
                if (Element.CTCC_Array.Count != 0)
                {

                    for (int g = 0; g < Element.CTCC_Array.Count; g++)
                    {
                        if (Element.CTCC_Array[g][0] == "protection-type")
                        {
                            Com_Eth_uni_protection_type.Items.Clear();
                            Com_nni_protection_type.Items.Clear();
                            Com_nni2_protection_type.Items.Clear();
                            Com_uni_protection_type.Items.Clear();
                            ComSdhPro.Items.Clear();
                            Com_Eth_uni_protection_type.Items.Add("无");
                            Com_nni_protection_type.Items.Add("无");
                            Com_nni2_protection_type.Items.Add("无");
                            Com_uni_protection_type.Items.Add("无");
                            ComSdhPro.Items.Add("无");
                            string[] value_ = Element.CTCC_Array[g][2].Split(',');
                            for (int l = 0; l < value_.Length; l++)
                            {

                                Com_Eth_uni_protection_type.Items.Add(value_[l]);
                                Com_nni_protection_type.Items.Add(value_[l]);
                                Com_nni2_protection_type.Items.Add(value_[l]);
                                Com_uni_protection_type.Items.Add(value_[l]);
                                ComSdhPro.Items.Add(value_[l]);

                            }
                            Com_Eth_uni_protection_type.SelectedIndex = 0;
                            Com_nni_protection_type.SelectedIndex = 0;
                            Com_nni2_protection_type.SelectedIndex = 0;
                            Com_uni_protection_type.SelectedIndex = 0;
                            ComSdhPro.SelectedIndex = 0;

                        }
                        if (Element.CTCC_Array[g][0] == "adaptation-type")
                        {
                            ComEthServiceMappingMode.Items.Clear();
                            ComSdhSerMap.Items.Clear();
                            string[] value_ = Element.CTCC_Array[g][2].Split(',');
                            for (int l = 0; l < value_.Length; l++)
                            {

                                ComEthServiceMappingMode.Items.Add(value_[l]);
                                ComSdhSerMap.Items.Add(value_[l]);

                            }

                        }
                        if (Element.CTCC_Array[g][0] == "signal-type")
                        {
                            ComSdhNniSdhtype_primary_nni.Items.Clear();
                            ComSdhNniSdhtype_secondary_nni.Items.Clear();
                            ComEosSdhSignalType.Items.Clear();
                            ComEosSdhSignalTypeProtect.Items.Clear();
                            ComSdhNniSdhtype_primary_nni2.Items.Clear();
                            ComSdhNniSdhtype_secondary_nni2.Items.Clear();
                            string[] value_ = Element.CTCC_Array[g][2].Split(',');
                            for (int l = 0; l < value_.Length; l++)
                            {

                                ComSdhNniSdhtype_primary_nni.Items.Add(value_[l]);
                                ComSdhNniSdhtype_secondary_nni.Items.Add(value_[l]);
                                ComSdhNniSdhtype_primary_nni2.Items.Add(value_[l]);
                                ComSdhNniSdhtype_secondary_nni2.Items.Add(value_[l]);
                                ComEosSdhSignalType.Items.Add(value_[l]);
                                ComEosSdhSignalTypeProtect.Items.Add(value_[l]);

                            }
                        }
                        if (Element.CTCC_Array[g][0] == "sdh-switch-type")
                        {
                            ComVCType.Items.Clear();
                            ComSdhUniSdhType.Items.Clear();
                            ComSdhNniVcType_primary_nni.Items.Clear();
                            ComSdhNniVcType_secondary_nni.Items.Clear();
                            ComSdhUniSdhType_Secondary.Items.Clear();
                            ComSdhNniVcType_primary_nni2.Items.Clear();
                            ComSdhNniVcType_secondary_nni2.Items.Clear();
                            string[] value_ = Element.CTCC_Array[g][2].Split(',');
                            for (int l = 0; l < value_.Length; l++)
                            {

                                ComVCType.Items.Add(value_[l]);
                                ComSdhUniSdhType.Items.Add(value_[l]);
                                ComSdhNniVcType_primary_nni.Items.Add(value_[l]);
                                ComSdhNniVcType_secondary_nni.Items.Add(value_[l]);
                                ComSdhUniSdhType_Secondary.Items.Add(value_[l]);
                                ComSdhNniVcType_primary_nni2.Items.Add(value_[l]);
                                ComSdhNniVcType_secondary_nni2.Items.Add(value_[l]);

                            }
                        }
                    }
                }
            }
        }
        private void 订阅ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string neipall = "";
                string neip = "";
                int id = 1;
                string user = "";
                string password = "";
                foreach (DataGridViewRow row in this.dataGridViewNeInformation.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();       //设备IP地址
                        neipall = neipall + "\r\n" + neip;
                    }
                }
                if (MessageBox.Show("正在订阅当前设备:" + neipall + "\r\n是否订阅？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridViewNeInformation.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            neip = dataGridViewNeInformation.Rows[row.Index].Cells["网元ip"].Value.ToString();
                            id = int.Parse(dataGridViewNeInformation.Rows[row.Index].Cells["SSH_ID"].Value.ToString());
                            user = dataGridViewNeInformation.Rows[row.Index].Cells["用户名"].Value.ToString();
                            password = dataGridViewNeInformation.Rows[row.Index].Cells["密码"].Value.ToString();
                            if (dataGridViewNeInformation.Rows[row.Index].Cells["连接状态"].Value.ToString() == "连接成功")
                            {
                                dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value = "订阅中";
                                string subscription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n" +
                                                        "<rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"7\" >" + "\r\n" +
                                                                    "<create-subscription xmlns=\"urn:ietf:params:xml:ns:netconf:notification:1.0\" />" + "\r\n" +
                                                         "</rpc > ";
                                var sub = netConfClient[id].SendReceiveRpc(subscription);
                                TextLog.AppendText("网元：" + dataGridViewNeInformation.Rows[row.Index].Cells["网元名称"].Value.ToString() + " " + System.DateTime.Now.ToString() + "应答：" + FenGeFu);
                                TextLog.AppendText(sub.OuterXml + FenGeFu);
                                Sub[id] = true;
                                ThSub[row.Index] = new Thread(() => Subscription(id, neip));
                                ThSub[row.Index].Start();
                                dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Value = "已开启";
                                dataGridViewNeInformation.Rows[row.Index].Cells["订阅"].Style.BackColor = Color.GreenYellow;
                                for (int i = 0; i < treeViewNEID.Nodes.Count; i++)
                                {
                                    if (treeViewNEID.Nodes[i].Name == id.ToString())
                                    {
                                        //treeViewNEID.Nodes[i].ForeColor = Color.Red;
                                        treeViewNEID.Nodes[i].ImageIndex = 1;
                                        treeViewNEID.Nodes[i].SelectedImageIndex = 1;
                                        break;
                                    }
                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
            }
        }
        private void cTP限速调整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            try
            {
                string _total_size = "", _cir = "", _pir = "", _cbs = "", _pbs = "";
                string allconnection = "";
                string _name = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        _name = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + "\r\n" + _name;
                    }
                }
                if (MessageBox.Show("正在配置当前业务的限速:\r\n" + allconnection + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            _name = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();
                            try
                            {
                                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                XmlDocument xmlDoc = new XmlDocument();
                                //xmlDoc.Load(filename);
                                xmlDoc = Sendrpc(GetXML.Connection(_name), id, ip);
                                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                root.AddNamespace("connectionxmlns", "urn:ccsa:yang:acc-connection");
                                XmlNode total_size = xmlDoc.SelectSingleNode("//connectionxmlns:total-size", root);
                                XmlNode cir = xmlDoc.SelectSingleNode("//connectionxmlns:cir", root);
                                XmlNode pir = xmlDoc.SelectSingleNode("//connectionxmlns:pir", root);
                                XmlNode cbs = xmlDoc.SelectSingleNode("//connectionxmlns:cbs", root);
                                XmlNode pbs = xmlDoc.SelectSingleNode("//connectionxmlns:pbs", root);
                                if (total_size != null) { _total_size = total_size.InnerText; }
                                if (cir != null) { _cir = cir.InnerText; }
                                if (pir != null) { _pir = pir.InnerText; }
                                if (cbs != null) { _cbs = cbs.InnerText; }
                                if (pbs != null) { _pbs = pbs.InnerText; }
                                // Console.Read();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                            }
                            // 实例化FormInfo，并传入待修改初值  
                            var Form_Connection_Rate = new Form_Connection_Rate(_name, _total_size, _cir, _pir, _cbs, _pbs);
                            // 以对话框方式显示FormInfo  
                            if (Form_Connection_Rate.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _name = Form_Connection_Rate._name;
                                _total_size = Form_Connection_Rate._total_size;
                                _cir = Form_Connection_Rate._cir;
                                _pir = Form_Connection_Rate._pir;
                                _cbs = Form_Connection_Rate._cbs;
                                _pbs = Form_Connection_Rate._pbs;
                                string messg = Creat(ModifyXML.Connection_Rate(_name, _total_size, _cir, _pir, _cbs, _pbs, ips), id, ip);
                                MessageBox.Show(messg);
                            }
                            //var doc = Sendrpc(DeleteODU.Delete(_name));//设备IP地址
                            //if (doc.OuterXml.Contains("error"))
                            //{
                            //    MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            //}
                            //else
                            //{
                            //    //this.dataGridViewEth.Rows.Remove(row);
                            //}
                        }
                    }
                    // MessageBox.Show(allconnection + "\r\n已成功删除，重新点击在线查询即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dataGridViewNeInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (treeViewNEID.Nodes.Count > e.RowIndex && e.RowIndex >= 0)
            {
                treeViewNEID.SelectedNode = treeViewNEID.Nodes[e.RowIndex];//选中
                if (dataGridViewNeInformation.Rows[e.RowIndex].Cells["运营商"].Value != null)
                {
                    ips = dataGridViewNeInformation.Rows[e.RowIndex].Cells["运营商"].Value.ToString();
                    toolStripStatusLabelips.Text = ips;
                    Thread thread = new Thread(() => SDHchengetype(ips));
                    thread.Start();
                }
                if (dataGridViewNeInformation.Rows[e.RowIndex].Cells["连接状态"].Value != null)
                {
                    LabConncet.Text = dataGridViewNeInformation.Rows[e.RowIndex].Cells["连接状态"].Value.ToString();
                }
                if (dataGridViewNeInformation.Rows[e.RowIndex].Cells["订阅"].Value != null)
                {
                    TextSub.Text = dataGridViewNeInformation.Rows[e.RowIndex].Cells["订阅"].Value.ToString();
                }
                if (dataGridViewNeInformation.Rows[e.RowIndex].Cells["网元ip"].Value != null)
                {
                    TextIP.Text = dataGridViewNeInformation.Rows[e.RowIndex].Cells["网元ip"].Value.ToString();
                }
            }
        }
        private void toolStripMenuItemPrameters_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            try
            {
                string _pm_parameter_name = "", _granularity = "", _threshold_type = "", _object_type = "", _threshold_value = "", _input_power = "", _output_power = "", _input_power_upper_threshold = "", _input_power_lower_threshold = "";
                string _name = "";
                foreach (DataGridViewRow row in this.dataGridViewCurrentPerformance.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        _name = dataGridViewCurrentPerformance.Rows[row.Index].Cells["对象名称"].Value.ToString();       //设备IP地址
                    }
                }
                if (MessageBox.Show("正在配置当接口:\r\n" + _name + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewCurrentPerformance.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            _name = dataGridViewCurrentPerformance.Rows[row.Index].Cells["对象名称"].Value.ToString();
                            _pm_parameter_name = dataGridViewCurrentPerformance.Rows[row.Index].Cells["参数名称"].Value.ToString();
                            _granularity = dataGridViewCurrentPerformance.Rows[row.Index].Cells["周期类型"].Value.ToString();
                            _object_type = dataGridViewCurrentPerformance.Rows[row.Index].Cells["对象类型"].Value.ToString();
                            _threshold_type = dataGridViewCurrentPerformance.Rows[row.Index].Cells["数字量性能值"].Value.ToString();
                            _threshold_value = dataGridViewCurrentPerformance.Rows[row.Index].Cells["最大值"].Value.ToString();
                            try
                            {
                                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                XmlDocument xmlDoc = new XmlDocument();
                                //xmlDoc.Load(filename);
                                xmlDoc = Sendrpc(GetXML.PTP(_name), id, ip);
                                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                root.AddNamespace("ptp", "urn:ccsa:yang:acc-devm");
                                XmlNode input_power = xmlDoc.SelectSingleNode("//ptp:input-power", root);
                                XmlNode output_power = xmlDoc.SelectSingleNode("//ptp:output-power", root);
                                XmlNode input_power_upper_threshold = xmlDoc.SelectSingleNode("//ptp:input-power-upper-threshold", root);
                                XmlNode input_power_lower_threshold = xmlDoc.SelectSingleNode("//ptp:input-power-lower-threshold", root);
                                if (input_power != null) { _input_power = input_power.InnerText; }
                                if (output_power != null) { _output_power = output_power.InnerText; }
                                if (input_power_upper_threshold != null) { _input_power_upper_threshold = input_power_upper_threshold.InnerText; }
                                if (input_power_lower_threshold != null) { _input_power_lower_threshold = input_power_lower_threshold.InnerText; }
                                // Console.Read();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                            }
                            // 实例化FormInfo，并传入待修改初值  
                            var Form_Tca_Parameter = new Form_Tca_Parameter(_name, _pm_parameter_name, _granularity, _threshold_type, _object_type, _threshold_value, _input_power, _output_power, _input_power_upper_threshold, _input_power_lower_threshold);
                            // 以对话框方式显示FormInfo  
                            if (Form_Tca_Parameter.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _name = Form_Tca_Parameter._name;
                                _pm_parameter_name = Form_Tca_Parameter._pm_parameter_name;
                                _granularity = Form_Tca_Parameter._granularity;
                                _threshold_type = Form_Tca_Parameter._threshold_type;
                                _object_type = Form_Tca_Parameter._object_type;
                                _threshold_value = Form_Tca_Parameter._threshold_value;
                                string messg = Creat(ModifyXML.Tca_parameters(_name, _pm_parameter_name, _granularity, _threshold_type, _object_type, _threshold_value, ips), id, ip);
                                MessageBox.Show(messg);
                            }
                            //var doc = Sendrpc(DeleteODU.Delete(_name));//设备IP地址
                            //if (doc.OuterXml.Contains("error"))
                            //{
                            //    MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            //}
                            //else
                            //{
                            //    //this.dataGridViewEth.Rows.Remove(row);
                            //}
                        }
                    }
                    // MessageBox.Show(allconnection + "\r\n已成功删除，重新点击在线查询即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttontcafind_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewCurrentPerformance.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.TCA(ComCurPerObjectName.Text, ComCurPerGranularity.Text);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("performancesxmlns", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//performancesxmlns:tca-parameters//performancesxmlns:tca-parameter", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewCurrentPerformance.Rows.Add();
                    XmlNode pm_parameter_name = itemNode.SelectSingleNode("performancesxmlns:pm-parameter-name", root);
                    XmlNode object_name = itemNode.SelectSingleNode("performancesxmlns:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("performancesxmlns:object-type", root);
                    XmlNode granularity = itemNode.SelectSingleNode("performancesxmlns:granularity", root);
                    XmlNode threshold_type = itemNode.SelectSingleNode("performancesxmlns:threshold-type", root);
                    XmlNode threshold_value = itemNode.SelectSingleNode("performancesxmlns:threshold-value", root);
                    if (pm_parameter_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["参数名称"].Value = pm_parameter_name.InnerText; }
                    if (object_name != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewCurrentPerformance.Rows[index].Cells["对象类型"].Value = object_type.InnerText; }
                    if (granularity != null) { dataGridViewCurrentPerformance.Rows[index].Cells["周期类型"].Value = granularity.InnerText; }
                    if (threshold_type != null) { dataGridViewCurrentPerformance.Rows[index].Cells["数字量性能值"].Value = threshold_type.InnerText; }
                    if (threshold_value != null) { dataGridViewCurrentPerformance.Rows[index].Cells["最大值"].Value = threshold_value.InnerText; }
                    LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        List<string[]> array = new List<string[]>();
        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(fastColoredTextBoxReq.Text);
            TreeReP.Nodes.Clear();
            BeginInvoke(new MethodInvoker(delegate () { LoadTreeFromXmlDocument_TreeReP(xml); }));
        }
        private void 加载联通YIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr1 = MessageBox.Show("是否从hunan128.com下载，“否”则从hunan128.com加载？", "提示", MessageBoxButtons.YesNo);
            if (dr1 == DialogResult.No)
            {
                if (Element.CUCC_Array.Count != 0)
                {
                    Element.CUCC_Array.Clear();
                }
                YIN_XML_URL(CUCC_YIN_URL, "联通");
                if (Element.CUCC_Array.Count != 0)
                {
                    MessageBox.Show("加载联通YIN文件成功！");
                }
                else
                {
                    MessageBox.Show("加载失败！");
                }
            }
            if (dr1 == DialogResult.Yes)
            {
                if (Element.CUCC_Array.Count != 0)
                {
                    Element.CUCC_Array.Clear();
                }
                if (Directory.Exists(CUCC_YIN))
                {
                    LoadYIN(CUCC_YIN, "联通");
                }
                else
                {
                    YIN_XML_URL(CUCC_YIN_URL, "联通");
                }
                if (Element.CUCC_Array.Count != 0)
                {
                    MessageBox.Show("加载联通YIN文件成功！");
                }
                else
                {
                    MessageBox.Show("加载失败！");
                }
            }
        }
        private void 加载移动YIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr1 = MessageBox.Show("是否从hunan128.com下载，“否”则从hunan128.com加载？", "提示", MessageBoxButtons.YesNo);
            if (dr1 == DialogResult.No)
            {
                if (Element.CMCC_Array.Count != 0)
                {
                    Element.CMCC_Array.Clear();
                }
                YIN_XML_URL(CMCC_YIN_URL, "移动");
                if (Element.CMCC_Array.Count != 0)
                {
                    MessageBox.Show("加载移动YIN文件成功");
                }
                else
                {
                    MessageBox.Show("加载失败");
                }
            }
            if (dr1 == DialogResult.Yes)
            {
                if (Element.CMCC_Array.Count != 0)
                {
                    Element.CMCC_Array.Clear();
                }
                if (Directory.Exists(CMCC_YIN))
                {
                    LoadYIN(CMCC_YIN, "移动");
                }
                else
                {
                    YIN_XML_URL(CMCC_YIN_URL, "移动");
                }
                if (Element.CMCC_Array.Count != 0)
                {
                    MessageBox.Show("加载移动YIN文件成功");
                }
                else
                {
                    MessageBox.Show("加载失败");
                }
            }
        }
        private void 加载电信YIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr1 = MessageBox.Show("是否从hunan128.com下载，“否”则从hunan128.com加载？", "提示", MessageBoxButtons.YesNo);
            if (dr1 == DialogResult.No)
            {
                if (Element.CTCC_Array.Count != 0)
                {
                    Element.CTCC_Array.Clear();
                }
                YIN_XML_URL(CTCC_YIN_URL, "电信");
                if (Element.CTCC_Array.Count != 0)
                {
                    MessageBox.Show("加载电信YIN文件成功");
                }
                else
                {
                    MessageBox.Show("加载失败");
                }
            }
            if (dr1 == DialogResult.Yes)
            {
                if (Element.CTCC_Array.Count != 0)
                {
                    Element.CTCC_Array.Clear();
                }
                if (Directory.Exists(CTCC_YIN))
                {
                    LoadYIN(CTCC_YIN, "电信");
                }
                else
                {
                    YIN_XML_URL(CTCC_YIN_URL, "电信");
                }
                if (Element.CTCC_Array.Count != 0)
                {
                    MessageBox.Show("加载电信YIN文件成功");
                }
                else
                {
                    MessageBox.Show("加载失败");
                }
            }
        }
        private void LoadYIN(string path, string ips)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir == null)
            {
                MessageBox.Show("文件空，请重新选择文件夹！");
                return;
            }
            FileInfo[] fileInfo = dir.GetFiles();
            List<string> fileNames = new List<string>();
            foreach (FileInfo item in fileInfo)
            {
                fileNames.Add(item.Name);
            }
            foreach (string yin in fileNames)
            {
                if (yin.Contains("yin"))
                {
                    if (ips.Contains("联通"))
                    {
                        if (Directory.Exists(CUCC_YIN))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(path + yin);
                            Element dos = new Element();
                            dos.Element_Value_Find(doc, ips);
                        }
                    }
                    if (ips.Contains("电信"))
                    {
                        if (Directory.Exists(CTCC_YIN))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(path + yin);
                            Element dos = new Element();
                            dos.Element_Value_Find(doc, ips);
                        }
                    }
                    if (ips.Contains("移动"))
                    {
                        if (Directory.Exists(CMCC_YIN))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(path + yin);
                            Element dos = new Element();
                            dos.Element_Value_Find(doc, ips);
                        }
                    }
                }
            }
        }
        private void vCG时隙调整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string _eth_ftp_name = "", _sdh_ftp_name = "", _sdh_protect_ftp_name = "", _mapping_path = "", _mapping_path_protected = "", _vc_type = "", _lcas = "", _hold_off = "", _wtr = "", _tsd = "", _tx_number = "", _rx_number = "", _so_handshake_state = "";
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + "\r\n" + connection;
                    }
                }
                if (MessageBox.Show("正在配置当前业务的VCG:\r\n" + allconnection + "\r\n是否查询或配置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            string _odu_ctp_name = dataGridViewEth.Rows[row.Index].Cells["CTP端口1"].Value.ToString();
                            string[] strArray = _odu_ctp_name.Split(',');
                            foreach (var item in strArray)
                            {
                                if (item != "")
                                {
                                    if (item.Contains("FTP"))
                                    {
                                        try
                                        {
                                            //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                            // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                            XmlDocument xmlDoc = new XmlDocument();
                                            //xmlDoc.Load(filename);
                                            xmlDoc = Sendrpc(GetXML.CTP(item), id, ip);
                                            XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                            root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                            root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                            XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                                            foreach (XmlNode itemNode in itemNodes)
                                            {
                                                XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                                                XmlNode protect_role = itemNode.SelectSingleNode("ptpsxmlns:protect-role", root);
                                                XmlNode server_tp = itemNode.SelectSingleNode("ptpsxmlns:server-tp", root);
                                                if (protect_role != null && server_tp != null)
                                                {
                                                    if (protect_role.InnerText == "secondary")
                                                    {
                                                        _sdh_protect_ftp_name = server_tp.InnerText;
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                                            // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                                            XmlDocument xmlDoc0 = new XmlDocument();
                                                            //xmlDoc.Load(filename);
                                                            xmlDoc0 = Sendrpc(GetXML.FTP(server_tp.InnerText), id, ip);
                                                            XmlNamespaceManager root0 = new XmlNamespaceManager(xmlDoc0.NameTable);
                                                            root0.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                                            root0.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                                            root0.AddNamespace("eth", "urn:ccsa:yang:acc-eth");
                                                            root0.AddNamespace("eos", "urn:ccsa:yang:acc-eos");
                                                            XmlNodeList itemNodes0 = xmlDoc0.SelectNodes("//ptpsxmlns:ftps//ptpsxmlns:ftp", root0);
                                                            foreach (XmlNode itemNode0 in itemNodes0)
                                                            {
                                                                XmlNode layer_protocol_name = itemNode0.SelectSingleNode("ptpsxmlns:layer-protocol-name", root0);
                                                                if (layer_protocol_name != null)
                                                                {
                                                                    if (layer_protocol_name.InnerText.Contains("ETH"))
                                                                    {
                                                                        _eth_ftp_name = server_tp.InnerText;
                                                                        XmlNode vc_type = itemNode0.SelectSingleNode("//eos:vc-type", root0);
                                                                        XmlNode lcas = itemNode0.SelectSingleNode("//eos:lcas", root0);
                                                                        XmlNode hold_off = itemNode0.SelectSingleNode("//eos:hold-off", root0);
                                                                        XmlNode wtr = itemNode0.SelectSingleNode("//eos:wtr", root0);
                                                                        XmlNode tsd = itemNode0.SelectSingleNode("//eos:tsd", root0);
                                                                        XmlNode tx_number = itemNode0.SelectSingleNode("//eos:tx-number", root0);
                                                                        XmlNode rx_number = itemNode0.SelectSingleNode("//eos:rx-number", root0);
                                                                        XmlNode so_handshake_state = itemNode0.SelectSingleNode("//eos:so-handshake-state", root0);
                                                                        if (vc_type != null) { _vc_type = vc_type.InnerText; }
                                                                        if (lcas != null) { _lcas = lcas.InnerText; }
                                                                        if (hold_off != null) { _hold_off = hold_off.InnerText; }
                                                                        if (wtr != null) { _wtr = wtr.InnerText; }
                                                                        if (tsd != null) { _tsd = tsd.InnerText; }
                                                                        if (tx_number != null) { _tx_number = tx_number.InnerText; }
                                                                        if (rx_number != null) { _rx_number = rx_number.InnerText; }
                                                                        if (so_handshake_state != null) { _so_handshake_state = so_handshake_state.InnerText; }
                                                                    }
                                                                    else
                                                                    {
                                                                        _sdh_ftp_name = server_tp.InnerText;
                                                                    }
                                                                }
                                                            }
                                                            // Console.Read();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                                        }
                                                    }
                                                }
                                            }
                                            // Console.Read();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                        }
                                    }
                                }
                            }
                            // 实例化FormInfo，并传入待修改初值  
                            var Form_modify_vcg_connection = new Form_modify_vcg_connection(_eth_ftp_name, _sdh_ftp_name, _sdh_protect_ftp_name, _vc_type, _lcas, _hold_off, _wtr, _tsd, _tx_number, _rx_number, _so_handshake_state);
                            // 以对话框方式显示FormInfo  
                            if (Form_modify_vcg_connection.ShowDialog() == DialogResult.OK)
                            {
                                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                                _eth_ftp_name = Form_modify_vcg_connection._eth_ftp_name;
                                _sdh_ftp_name = Form_modify_vcg_connection._sdh_ftp_name;
                                _sdh_protect_ftp_name = Form_modify_vcg_connection._sdh_protect_ftp_name;
                                _mapping_path = Form_modify_vcg_connection._mapping_path;
                                _mapping_path_protected = Form_modify_vcg_connection._mapping_path_protected;
                                string messg = Creat(ModifyXML.Modify_vcg_connection_capacity(_eth_ftp_name, _sdh_ftp_name, _sdh_protect_ftp_name, _mapping_path, _mapping_path_protected, ips), id, ip);
                                MessageBox.Show(messg);
                            }
                        }
                    }
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ComCreatConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComCreatConnection.Text.Contains("ETH"))
            {
                TextEthLabel.Text = "ETH1";
                groupBoxCreateEOS.Enabled = false;
                panel_osu_primary_nni.Visible = false;
                panel_osu_secondary_nni.Visible = false;
                panel_osu_primary_nni2.Visible = false;
                panel_osu_secondary_nni2.Visible = false;
            }
            if (ComCreatConnection.Text.Contains("EOS"))
            {
                TextEthLabel.Text = "ETH1";
                groupBoxCreateEOS.Enabled = true;
                panel_osu_primary_nni.Visible = false;
                panel_osu_secondary_nni.Visible = false;
                panel_osu_primary_nni2.Visible = false;
                panel_osu_secondary_nni2.Visible = false;
            }
            if (ComCreatConnection.Text.Contains("EO-OSU"))
            {
                TextEthLabel.Text = "EoOSU1";
                groupBoxCreateEOS.Enabled = false;
                panel_osu_primary_nni.Visible = true;
                panel_osu_secondary_nni.Visible = true;
                panel_osu_primary_nni2.Visible = true;
                panel_osu_secondary_nni2.Visible = true;

                ComEthServiceMappingMode.Enabled = false;

            }
            else {
                ComEthServiceMappingMode.Enabled = true;
            }
        }
        private void ComEthServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComEthServiceType.Text.Contains("EPL"))
            {
                groupBoxunivlan.Enabled = false;
                ComEthClientVlanId.Text = "";
                ComEthUniVlanId.Text = "";
            }
            if (ComEthServiceType.Text.Contains("EVPL"))
            {
                groupBoxunivlan.Enabled = true;
            }
        }
        private void 分享SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//清空剪切板内容
            Clipboard.SetData(DataFormats.Text, "http://hunan128.com/index.php/2021/09/02/netconfclient工具发布/");//复制内容到剪切板
            MessageBox.Show("链接已复制，请粘贴使用");
            //Process.Start("http://hunan128.com/index.php/2021/09/02/netconfclient工具发布/");
        }
        private void 取消执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewAuto.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    //dataGridViewAuto.Rows[row.Index].Cells["Auto是否执行"].Value = "否";
                    ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[row.Index].Cells["Auto是否执行"]).Items.Add("否");
                    ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[row.Index].Cells["Auto是否执行"]).Value = "否";
                }
            }
            MessageBox.Show("修改完成");
        }
        private void 确认执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewAuto.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[row.Index].Cells["Auto是否执行"]).Items.Add("是");
                    ((DataGridViewComboBoxCell)dataGridViewAuto.Rows[row.Index].Cells["Auto是否执行"]).Value = "是";
                }
            }
            MessageBox.Show("修改完成");
        }

        private void buttonEGG_Click(object sender, EventArgs e)
        {
            Process.Start("http://hunan128.com:888/AutoRunningExcel/");
        }
        private void comPrimary_nni2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComOduAdapataionType_Primary_nni2.Text == "无")
                    return;
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PTP(ComODUPtpNamesecondary_nni2.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                if (comODUPtpNamePrimary_nni2.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode ts_detail = itemNodeOdu.SelectSingleNode("oduxmlns:ts-detail", root);
                                        if (ts_detail != null) label_ts_primary_nni2.Text = ts_detail.InnerText;
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);
                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_Primary_nni2.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                string result_type = odu_signal_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduOduSignalType_Primary_nni2.Items.Add(result_type);
                                                ComOduOduSignalType_Primary_nni2.SelectedIndex = ComOduOduSignalType_Primary_nni2.Items.Count - 1;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_Primary_nni2.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                string result_type = adaptation_type1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduAdapataionType_Primary_nni2.Items.Add(result_type);
                                                ComOduAdapataionType_Primary_nni2.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_Primary_nni2.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                string result_type = switch_capability1.InnerText;
                                                if (result_type.Contains(":"))
                                                {
                                                    result_type = result_type.Split(new char[] { ':' })[1];
                                                }
                                                ComOduSwitchApability_Primary_nni2.Items.Add(result_type);
                                                ComOduSwitchApability_Primary_nni2.SelectedIndex = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Console.Read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void comboBoxODUservicemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxODUservicemode.Text.Contains("NNI-To-NNI业务"))
            {
                ComClientSideNni_UNI.Text = "无";
                ComClientSideNni_UNI_second.Text = "无";

                groupBoxODUClient.Visible = false;
                groupBoxODUprimary_nni2.Visible = true;
                groupBoxsecondary_nni2.Visible = true;
                panel_primary_nni.Visible = false;
                panel_secondary_nni.Visible = false;
                panel_primary_nni2.Visible = false;
                panel_secondary_nni2.Visible = false;
                panel_sdh_primary_nni.Visible = false;
                panel_sdh_secondary_nni.Visible = false;
                panel_sdh_primary_nni2.Visible = false;
                panel_sdh_secondary_nni2.Visible = false;

            }
            else
            {
                comODUPtpNamePrimary_nni2.Text = "无";
                ComODUPtpNamesecondary_nni2.Text = "无";
                groupBoxODUClient.Visible = true;
                groupBoxODUprimary_nni2.Visible = false;
                groupBoxsecondary_nni2.Visible = false;
                panel_primary_nni.Visible = false;
                panel_secondary_nni.Visible = false;
                panel_primary_nni2.Visible = false;
                panel_secondary_nni2.Visible = false;
                panel_sdh_primary_nni.Visible = false;
                panel_sdh_secondary_nni.Visible = false;
                panel_sdh_primary_nni2.Visible = false;
                panel_sdh_secondary_nni2.Visible = false;
            }
        }
        private void comboBoxETHservicemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxETHservicemode.Text.Contains("NNI-To-NNI业务"))
            {
                ComEthUniPtpName.Text = "无";
                ComEthSecondUniPtpName.Text = "无";
                ComEthClientVlanId.Text = "";
                ComEthUniVlanId.Text = "";
                groupBoxETH_UNI.Visible = false;
                groupBoxODUprimary_nni2.Visible = true;
                ComEthFtpVlanID_primary_nni2.Text = "";
                ComEthFtpVlanID_secondary_nni2.Text = "";
                groupBoxsecondary_nni2.Visible = true;
                panel_primary_nni.Visible = true;
                panel_secondary_nni.Visible = true;
                panel_primary_nni2.Visible = true;
                panel_secondary_nni2.Visible = true;
                panel_sdh_primary_nni.Visible = false;
                panel_sdh_secondary_nni.Visible = false;
                panel_sdh_primary_nni2.Visible = false;
                panel_sdh_secondary_nni2.Visible = false;
            }
            else
            {
                comODUPtpNamePrimary_nni2.Text = "无";
                ComODUPtpNamesecondary_nni2.Text = "无";
                ComEthFtpVlanID_primary_nni2.Text = "";
                ComEthFtpVlanID_secondary_nni2.Text = "";
                groupBoxETH_UNI.Visible = true;
                groupBoxODUprimary_nni2.Visible = false;
                groupBoxsecondary_nni2.Visible = false;
                panel_primary_nni.Visible = true;
                panel_secondary_nni.Visible = true;
                panel_primary_nni2.Visible = false;
                panel_secondary_nni2.Visible = false;
                panel_sdh_primary_nni.Visible = false;
                panel_sdh_secondary_nni.Visible = false;
                panel_sdh_primary_nni2.Visible = false;
                panel_sdh_secondary_nni2.Visible = false;
            }
        }
        private void comboBoxSDHservicemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSDHservicemode.Text.Contains("NNI-To-NNI业务"))
            {
                ComSdhUniPtp.Text = "无";
                ComSdhUniPtp_Secondary.Text = "无";
                ComSdhPro.Text = "无";
                ComSdhUniPtp.Enabled = false;
                ComSdhUniPtp_Secondary.Enabled = false;
                //ComSdhPro.Text = "";
                //TextSdhUniTs.Visible = false;
                groupBoxODUprimary_nni2.Visible = true;
                groupBoxsecondary_nni2.Visible = true;
                panel_primary_nni.Visible = false;
                panel_secondary_nni.Visible = false;
                panel_primary_nni2.Visible = false;
                panel_secondary_nni2.Visible = false;
                panel_sdh_primary_nni.Visible = true;
                panel_sdh_secondary_nni.Visible = true;
                panel_sdh_primary_nni2.Visible = true;
                panel_sdh_secondary_nni2.Visible = true;
            }
            else
            {

                //groupBoxETH_UNI.Visible = true;
                comODUPtpNamePrimary_nni2.Text = "无";
                ComODUPtpNamesecondary_nni2.Text = "无";
                ComSdhUniPtp.Enabled = true;
                ComSdhUniPtp_Secondary.Enabled = true;
                groupBoxODUprimary_nni2.Visible = false;
                groupBoxsecondary_nni2.Visible = false;
                panel_primary_nni.Visible = false;
                panel_secondary_nni.Visible = false;
                panel_primary_nni2.Visible = false;
                panel_secondary_nni2.Visible = false;
                panel_sdh_primary_nni.Visible = true;
                panel_sdh_secondary_nni.Visible = true;
                panel_sdh_primary_nni2.Visible = false;
                panel_sdh_secondary_nni2.Visible = false;
            }
        }
        private void 清空所有通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewAlarm.Rows.Clear();
            //ListViewTcaAlarm.Items.Clear();
            dataGridViewPGS_Not.Rows.Clear();
            dataGridViewAttributeValueChange.Rows.Clear();
            dataGridViewLLDP.Rows.Clear();
            dataGridViewPeerChange.Rows.Clear();
        }
        private void 清空告警通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewAlarm.Rows.Clear();
        }
        private void 清空保护倒换通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewPGS_Not.Rows.Clear();
        }
        private void 清空对象变更通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewAttributeValueChange.Rows.Clear();
        }
        private void 清空LLDP通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewLLDP.Rows.Clear();
        }
        private void 清空Peer通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewPeerChange.Rows.Clear();
        }
        private void 清空GHao通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void butOduuni_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Oduk_TS(labelClientTs.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                ComOduNniTsDetailClient_UNI_A.Text = formInfo.Information;
            }
        }
        private void but_ts_primary_nni_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Oduk_TS(label_ts_primary_nni.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                ComOduTsDetail_Primary_nni.Text = formInfo.Information;
            }
        }
        private void but_ts_sec_nni_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Oduk_TS(label_ts_sec_nni.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                ComOduTsDetail_Secondary_nni.Text = formInfo.Information;
            }
        }
        private void but_ts_primary_nni2_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Oduk_TS(label_ts_primary_nni2.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                ComOduTsDetail_Primary_nni2.Text = formInfo.Information;
            }
        }
        private void but_ts_sec_nni2_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Oduk_TS(label_ts_sec_nni2.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                ComOduTsDetail_Secondary_nni2.Text = formInfo.Information;
            }
        }

        private void dataGridViewAuto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            详细信息ToolStripMenuItem.PerformClick();
        }

        private void 加载所有运营商YIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Element.CUCC_Array.Count != 0)
            {
                Element.CUCC_Array.Clear();
            }
            if (Directory.Exists(CUCC_YIN))
            {
                LoadYIN(CUCC_YIN, "联通");
            }
            else
            {
                YIN_XML_URL(CUCC_YIN_URL, "联通");
            }
            if (Element.CUCC_Array.Count != 0)
            {
               // MessageBox.Show("加载联通YIN文件成功！");
                TextLog.AppendText("加载联通YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("加载联通YIN文件失败！" + FenGeFu);
            }

            if (Element.CMCC_Array.Count != 0)
            {
                Element.CMCC_Array.Clear();
            }
            if (Directory.Exists(CMCC_YIN))
            {
                LoadYIN(CMCC_YIN, "移动");
            }
            else
            {
                YIN_XML_URL(CMCC_YIN_URL, "移动");
            }
            if (Element.CMCC_Array.Count != 0)
            {
                TextLog.AppendText("加载移动YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("加载移动YIN文件失败！" + FenGeFu);
            }
            if (Element.CTCC_Array.Count != 0)
            {
                Element.CTCC_Array.Clear();
            }
            if (Directory.Exists(CTCC_YIN))
            {
                LoadYIN(CTCC_YIN, "电信");
            }
            else
            {
                YIN_XML_URL(CTCC_YIN_URL, "电信");
            }
            if (Element.CTCC_Array.Count != 0)
            {
                TextLog.AppendText("加载电信YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("加载电信YIN文件失败！" + FenGeFu);
            }
            //MessageBox.Show("加载所有运营商YIN文件已完成！");
        }

        private void dataGridViewNeInformation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Thread thread = new Thread(() => OnLinedevm());
            thread.Start();
        }

        private void treeViewNEID_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Thread thread = new Thread(() => OnLinedevm());
            thread.Start();
        }

        private void ButModifyInterface_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            if (ComPtpCtpFtp.Text.Contains("PTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(ModifyXML.interface_type(ComPtpCtpFtp.Text, cominterfacetype.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("FTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
            FindPort();
        }

        private void ButSdhUniTs_Secondary_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new Form_Info(ComSdhUniSdhType_Secondary.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhUniTs_Secondary.Text = formInfo.Information;
            }
        }

        private void ButSdhNniTs_primary_nni2_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Info(ComSdhNniVcType_primary_nni2.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_primary_nni2.Text = formInfo.Information;
            }
        }

        private void ButSdhNniTs__secondary_nni2_Click(object sender, EventArgs e)
        {
            var formInfo = new Form_Info(ComSdhNniVcType_secondary_nni2.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_secondary_nni2.Text = formInfo.Information;
            }
        }

        private void TextOduService_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TextOduService_type.Text == "OSU") {
                TextOduLable.Text = "OSU1";
                ComOduSwitchApability_UNI_A.Enabled = false;
                ComOduAdapataionType_UNI_A.Enabled = false;
                ComOduNniTsDetailClient_UNI_A.Enabled = false;
                butOduuni.Enabled = false;
                panel_osu_primary_nni.Visible = true;
                panel_osu_primary_nni2.Visible = true;
                panel_osu_secondary_nni.Visible = true;
                panel_osu_secondary_nni2.Visible = true;
            }
            if (TextOduService_type.Text == "ODU")
            {
                TextOduLable.Text = "ODU1";
                ComOduSwitchApability_UNI_A.Enabled = true;
                ComOduAdapataionType_UNI_A.Enabled = true;
                ComOduNniTsDetailClient_UNI_A.Enabled = true;
                butOduuni.Enabled = true;
                panel_osu_primary_nni.Visible = false;
                panel_osu_primary_nni2.Visible = false;
                panel_osu_secondary_nni.Visible = false;
                panel_osu_secondary_nni2.Visible = false;
            }
        }

        private void ButEnableNTP_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            Creat(ModifyXML.Ntp(textBox_me_ntp_enable.Text, textBox_me_ntp_server_name.Text, textBox_me_ntp_server_ipaddress.Text, textBox_me_ntp_server_port.Text, textBox_me_ntp_server_version.Text),id,ip);
            ButFindNeinfo.PerformClick();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }


            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();


            if (ComPtpCtpFtp.Text.Contains("PTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (ComPtpCtpFtp.Text.Contains("FTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                return;
            }
            if (ComPtpCtpFtp.Text.Contains("CTP"))
            {
                Creat(ModifyXML.Vlan_spec(ComPtpCtpFtp.Text, ComEthUniVlanId.Text, ComEthUniVlanPriority.Text, ComEthUniVlanAccessAction.Text, ComEthUniVlanType.Text, ComEthClientVlanId.Text, ComEthVlanPriority.Text, ComEthVlanAccessAction.Text, ComEthVlanType.Text), id, ip);
                FindPort();
            }
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
           
        }

        private void ButtonLoopBack_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.Loop_back(ComPtpCtpFtp.Text, comboBoxLoopBack.Text), id, ip);
            FindPort();
        }

        private void buttonLaserStatus_Click(object sender, EventArgs e)
        {
            
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.Laser_status(ComPtpCtpFtp.Text, comboBoxLaserStatus.Text), id, ip);
            FindPort();
        }

        private void buttonAdminState_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.Admin_state(ComPtpCtpFtp.Text, comboBoxAdminState.Text), id, ip);
            FindPort();
        }

        private void 从服务器更新YIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Element.CUCC_Array.Count != 0)
            {
                Element.CUCC_Array.Clear();
            }
            YIN_XML_URL(CUCC_YIN_URL, "联通");
            if (Element.CUCC_Array.Count != 0)
            {
                // MessageBox.Show("加载联通YIN文件成功！");
                TextLog.AppendText("服务器更新联通YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("服务器更新联通YIN文件失败！" + FenGeFu);
            }

            if (Element.CMCC_Array.Count != 0)
            {
                Element.CMCC_Array.Clear();
            }
            YIN_XML_URL(CMCC_YIN_URL, "移动");
            if (Element.CMCC_Array.Count != 0)
            {
                TextLog.AppendText("服务器更新移动YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("服务器更新移动YIN文件失败！" + FenGeFu);
            }
            if (Element.CTCC_Array.Count != 0)
            {
                Element.CTCC_Array.Clear();
            }
            YIN_XML_URL(CTCC_YIN_URL, "电信");
            if (Element.CTCC_Array.Count != 0)
            {
                TextLog.AppendText("服务器更新电信YIN文件成功！" + FenGeFu);
            }
            else
            {
                TextLog.AppendText("服务器更新电信YIN文件失败！" + FenGeFu);
            }
            MessageBox.Show("服务器更新所有运营商YIN文件已完成！");
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("删除", "to-secondary","port");
        }

        private void 创建保护组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string _pg_id = "", _create_type = "", _delete_cascade = "", _protection_type = "", _reversion_mode = "", _switch_type = "", _hold_off = "", _wait_to_restore_time = "", _sd_trigger = "", _primary_port = "", _secondary_port = "";
                try
                {
                    if (ips.Contains("联通"))
                    {
                        
                    }
                    if (ips.Contains("移动"))
                    {
                        if (Element.CMCC_Array.Count != 0)
                        {
                            ArrayList cmccpro = new ArrayList();
                            for (int g = 0; g < Element.CMCC_Array.Count; g++)
                            {
                                if (Element.CMCC_Array[g][0] == "protection-type")
                                {
                                    string[] value_ = Element.CMCC_Array[g][2].Split(',');
                                    for (int l = 0; l < value_.Length; l++)
                                    {
                                        cmccpro.Add(value_[l]);
                                        _protection_type = string.Join(",", (string[])cmccpro.ToArray(typeof(string)));

                                    }

                                }
                            }
                        }

                    }
                    if (ips.Contains("电信"))
                    {
                        ComEthServiceMappingMode.Enabled = true;
                        ComSdhSerMap.Enabled = true;
                        if (Element.CTCC_Array.Count != 0)
                        {
                            ArrayList ctccpro = new ArrayList();
                            for (int g = 0; g < Element.CTCC_Array.Count; g++)
                            {
                                if (Element.CTCC_Array[g][0] == "protection-type")
                                {
                                    string[] value_ = Element.CTCC_Array[g][2].Split(',');
                                    for (int l = 0; l < value_.Length; l++)
                                    {

                                        ctccpro.Add(value_[l]);
                                        _protection_type = string.Join(",", (string[])ctccpro.ToArray(typeof(string)));

                                    }

                                }
                            }
                        }
                    }

                    XmlDocument xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, false, false), id, ip);
                    XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                    root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                    root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                    root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                    ArrayList list = new ArrayList();
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                        XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                        XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                        if (layer_protocol_name != null && interface_type != null)
                        {
                            if (layer_protocol_name.InnerText == "acc-sdh:SDH")
                            {
                                if (name != null)
                                {
                                   
                                    list.Add(name.InnerText);
                                    _primary_port = string.Join(",", (string[])list.ToArray(typeof(string)));
                                    _secondary_port = string.Join(",", (string[])list.ToArray(typeof(string)));
                                    //ComSdhUniPtp.Items.Add(name.InnerText);
                                    //ComSdhUniPtp.SelectedIndex = 0;
                                    //ComSdhUniPtp_Secondary.Items.Add(name.InnerText);
                                    //ComSdhUniPtp_Secondary.SelectedIndex = 0;
                                }
                            }
                            //if (layer_protocol_name.InnerText == "acc-otn:ODU")
                            //{
                            //    if (name != null)
                            //    {
                            //        ArrayList list = new ArrayList();
                            //        list.Add(name.InnerText);
                            //        _primary_port = string.Join(",", (string[])list.ToArray(typeof(string)));
                            //        _secondary_port = string.Join(",", (string[])list.ToArray(typeof(string)));

                            //        //comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                            //        //comODUPtpNamePrimary_nni.SelectedIndex = 0;
                            //        //ComODUPtpNamesecondary_nni.Items.Add(name.InnerText);
                            //        //ComODUPtpNamesecondary_nni.SelectedIndex = 0;
                            //        //comODUPtpNamePrimary_nni2.Items.Add(name.InnerText);
                            //        //comODUPtpNamePrimary_nni2.SelectedIndex = 0;
                            //        //ComODUPtpNamesecondary_nni2.Items.Add(name.InnerText);
                            //        //ComODUPtpNamesecondary_nni2.SelectedIndex = 0;
                            //    }
                            //}
                        }
                    }
                    // Console.Read();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                }
                // 实例化FormInfo，并传入待修改初值  
                var FormCreatePg = new Form_Create_Pg(_pg_id, _create_type , _delete_cascade , _protection_type , _reversion_mode, _switch_type , _hold_off, _wait_to_restore_time , _sd_trigger , _primary_port , _secondary_port,"msp");
                // 以对话框方式显示FormInfo  
                if (FormCreatePg.ShowDialog() == DialogResult.OK)
                {
                    //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                    _pg_id = FormCreatePg._pg_id;
                    _create_type = FormCreatePg._create_type;
                    _delete_cascade = FormCreatePg._delete_cascade;
                    _protection_type = FormCreatePg._protection_type;
                    _reversion_mode = FormCreatePg._reversion_mode;
                    _switch_type = FormCreatePg._switch_type;
                    _hold_off = FormCreatePg._hold_off;
                    _wait_to_restore_time = FormCreatePg._wait_to_restore_time;
                    _sd_trigger = FormCreatePg._sd_trigger;
                    _primary_port = FormCreatePg._primary_port;
                    _secondary_port = FormCreatePg._secondary_port;
                    string messg = Creat(CreatePG.Pg(_pg_id, _create_type, _delete_cascade, _protection_type, _reversion_mode, _switch_type, _hold_off, _wait_to_restore_time, _sd_trigger, _primary_port, _secondary_port), id, ip);
                    MessageBox.Show(messg);
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 创建OCH保护组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string _pg_id = "", _create_type = "", _delete_cascade = "", _protection_type = "", _reversion_mode = "", _switch_type = "", _hold_off = "", _wait_to_restore_time = "", _sd_trigger = "", _primary_port = "", _secondary_port = "";
                try
                {
                    if (ips.Contains("联通"))
                    {

                    }
                    if (ips.Contains("移动"))
                    {
                        if (Element.CMCC_Array.Count != 0)
                        {
                            ArrayList cmccpro = new ArrayList();
                            for (int g = 0; g < Element.CMCC_Array.Count; g++)
                            {
                                if (Element.CMCC_Array[g][0] == "protection-type")
                                {
                                    string[] value_ = Element.CMCC_Array[g][2].Split(',');
                                    for (int l = 0; l < value_.Length; l++)
                                    {
                                        cmccpro.Add(value_[l]);
                                        _protection_type = string.Join(",", (string[])cmccpro.ToArray(typeof(string)));

                                    }

                                }
                            }
                        }

                    }
                    if (ips.Contains("电信"))
                    {
                        ComEthServiceMappingMode.Enabled = true;
                        ComSdhSerMap.Enabled = true;
                        if (Element.CTCC_Array.Count != 0)
                        {
                            ArrayList ctccpro = new ArrayList();
                            for (int g = 0; g < Element.CTCC_Array.Count; g++)
                            {
                                if (Element.CTCC_Array[g][0] == "protection-type")
                                {
                                    string[] value_ = Element.CTCC_Array[g][2].Split(',');
                                    for (int l = 0; l < value_.Length; l++)
                                    {

                                        ctccpro.Add(value_[l]);
                                        _protection_type = string.Join(",", (string[])ctccpro.ToArray(typeof(string)));

                                    }

                                }
                            }
                        }
                    }

                    XmlDocument xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, false, false), id, ip);
                    XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                    root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                    root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                    root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                    ArrayList list = new ArrayList();
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                        XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                        XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);
                        if (layer_protocol_name != null && interface_type != null)
                        {
                            if (layer_protocol_name.InnerText == "acc-otn:ODU" && interface_type.InnerText.Contains("NNI"))
                            {
                                if (name != null)
                                {
                                    list.Add(name.InnerText);
                                    _primary_port = string.Join(",", (string[])list.ToArray(typeof(string)));
                                    _secondary_port = string.Join(",", (string[])list.ToArray(typeof(string)));

                                    //comODUPtpNamePrimary_nni.Items.Add(name.InnerText);
                                    //comODUPtpNamePrimary_nni.SelectedIndex = 0;
                                    //ComODUPtpNamesecondary_nni.Items.Add(name.InnerText);
                                    //ComODUPtpNamesecondary_nni.SelectedIndex = 0;
                                    //comODUPtpNamePrimary_nni2.Items.Add(name.InnerText);
                                    //comODUPtpNamePrimary_nni2.SelectedIndex = 0;
                                    //ComODUPtpNamesecondary_nni2.Items.Add(name.InnerText);
                                    //ComODUPtpNamesecondary_nni2.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                    // Console.Read();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                }
                // 实例化FormInfo，并传入待修改初值  
                var FormCreatePg = new Form_Create_Pg(_pg_id, _create_type, _delete_cascade, _protection_type, _reversion_mode, _switch_type, _hold_off, _wait_to_restore_time, _sd_trigger, _primary_port, _secondary_port,"och");
                // 以对话框方式显示FormInfo  
                if (FormCreatePg.ShowDialog() == DialogResult.OK)
                {
                    //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                    _pg_id = FormCreatePg._pg_id;
                    _create_type = FormCreatePg._create_type;
                    _delete_cascade = FormCreatePg._delete_cascade;
                    _protection_type = FormCreatePg._protection_type;
                    _reversion_mode = FormCreatePg._reversion_mode;
                    _switch_type = FormCreatePg._switch_type;
                    _hold_off = FormCreatePg._hold_off;
                    _wait_to_restore_time = FormCreatePg._wait_to_restore_time;
                    _sd_trigger = FormCreatePg._sd_trigger;
                    _primary_port = FormCreatePg._primary_port;
                    _secondary_port = FormCreatePg._secondary_port;
                    string messg = Creat(CreatePG.Pg(_pg_id, _create_type, _delete_cascade, _protection_type, _reversion_mode, _switch_type, _hold_off, _wait_to_restore_time, _sd_trigger, _primary_port, _secondary_port), id, ip);
                    MessageBox.Show(messg);
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonMTU_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.MTU(ComPtpCtpFtp.Text, comboBoxMTU.Text), id, ip);
            FindPort();
        }

        private void buttonWorkingMode_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.Working_Mode(ComPtpCtpFtp.Text, comboBoxWorkingMode.Text), id, ip);
            FindPort();
        }

        private void buttonLLDP_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                return;
            }
            Creat(ModifyXML.LLDP(ComPtpCtpFtp.Text, comboBoxLLDP.Text), id, ip);
            FindPort();
        }

        private void FindPort() {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            if (ComPtpCtpFtp.Text.Contains("PTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.PTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("FTP") && !ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.FTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (ComPtpCtpFtp.Text.Contains("CTP"))
            {
                xmlDoc = Sendrpc(GetXML.CTP(ComPtpCtpFtp.Text), id, ip);
            }
            if (string.IsNullOrEmpty(ComPtpCtpFtp.Text))
            {
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, true, true), id, ip);
            }
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
        }
        private void ComPtpCtpFtp_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindPort();
        }

        private void buttonModifyPassword_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();

           MessageBox.Show( Creat(ModifyXML.Password(textBoxuser.Text, textBoxOldPassword.Text,textBoxNewPassword.Text), id, ip));
        }

        private void 软复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetSlot("soft-reset");
        }

        private void 硬复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetSlot("hard-reset");
        }

        private void ResetSlot(string _reset_type) {
            try
            {
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridView_EQ.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridView_EQ.Rows[row.Index].Cells["单板名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + ", " + connection;
                    }
                }
                if (MessageBox.Show("正在"+ _reset_type + "复位当前板卡:\r\n" + allconnection + "\r\n是否操作？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridView_EQ.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            connection = dataGridView_EQ.Rows[row.Index].Cells["单板名称"].Value.ToString();
                            int id = int.Parse(treeViewNEID.SelectedNode.Name);
                            int line = -1;
                            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                            {
                                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                                {
                                    line = i;
                                    break;
                                }
                                if (line >= 0)
                                    break;
                            }
                            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                            var doc = Sendrpc(ModifyXML.Reset(connection, _reset_type), id, ip);//设备IP地址
                            if (doc.OuterXml.Contains("error"))
                            {
                                MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            }
                        }
                    }
                    MessageBox.Show(allconnection + "\r\n已成功复位，重新点击刷新即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除板卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string allconnection = "";
                string connection = "";
                foreach (DataGridViewRow row in this.dataGridView_EQ.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        connection = dataGridView_EQ.Rows[row.Index].Cells["单板名称"].Value.ToString();       //设备IP地址
                        allconnection = allconnection + ", " + connection;
                    }
                }
                if (MessageBox.Show("正在删除卸载当前板卡:\r\n" + allconnection + "\r\n是否操作？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in this.dataGridView_EQ.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            connection = dataGridView_EQ.Rows[row.Index].Cells["单板名称"].Value.ToString();
                            int id = int.Parse(treeViewNEID.SelectedNode.Name);
                            int line = -1;
                            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                            {
                                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                                {
                                    line = i;
                                    break;
                                }
                                if (line >= 0)
                                    break;
                            }
                            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                            var doc = Sendrpc(DeleteODU.EQ(connection), id, ip);//设备IP地址
                            if (doc.OuterXml.Contains("error"))
                            {
                                MessageBox.Show("运行失败：\r\n" + doc.OuterXml);
                            }
                            else {
                                dataGridView_EQ.Rows.Remove(row);
                            }
                        }
                    }
                    MessageBox.Show(allconnection + "\r\n已成功复位，重新点击刷新即可更新。");
                }
                // 保存在实体类属性中
                //保存密码选中状态
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxFileList.Clear();
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                XmlDocument xmlDoc = Sendrpc(FileManage.Upload_File(comboBoxFileType.Text, comboBoxFileName.Text, comboBoxSftpPath.Text, comboBoxUsername.Text, comboBoxPassword.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("file", "urn:ccsa:yang:acc-file");
                XmlNode file_name = xmlDoc.SelectSingleNode("//file:file-name", root);
                XmlNode md5 = xmlDoc.SelectSingleNode("//file:md5", root);
                XmlNode file_size = xmlDoc.SelectSingleNode("//file:file-size", root);
                string a = "",b="",c="";
                if (file_name != null) a = file_name.InnerText;
                if (md5 != null) b = md5.InnerText;
                if (file_size != null) c = file_size.InnerText;
                // Console.Read();
                textBoxFileList.AppendText("文件名称：" + a +"\r\n" + "md5：" + b + "\r\n" + "文件大小：" + c + "\r\n");
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));
                    }
                    else
                    {
                        MessageBox.Show("运行完成");
                    }
                }

            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string Me = Creat(FileManage.Download_File(comboBoxFileType.Text, comboBoxFileName.Text, textBoxFileSize.Text,comboBoxSftpPath.Text, comboBoxUsername.Text, comboBoxPassword.Text), id, ip);
                MessageBox.Show(Me);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonActiveFile_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string Me = Creat(FileManage.Active_File(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
                MessageBox.Show(Me);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDownloadStatus_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxDownload_Bytes.Text = "";
                textBoxGetDownloadStatus.Text = "";
                textBoxFail_Reason.Text = "";
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                //  string Me = Creat(FileManage.Active_File(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = Sendrpc(FileManage.Get_Download_Status(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("file", "urn:ccsa:yang:acc-file");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//file:download-bytes", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode download_bytes = xmlDoc.SelectSingleNode("//file:download-bytes", root);
                    XmlNode status = xmlDoc.SelectSingleNode("//file:status", root);
                    XmlNode fail_reason = xmlDoc.SelectSingleNode("//file:fail-reason", root);
                    if (download_bytes != null) textBoxDownload_Bytes.Text = download_bytes.InnerText;
                    if (status != null) textBoxGetDownloadStatus.Text = status.InnerText;
                    if (fail_reason != null) textBoxFail_Reason.Text = fail_reason.InnerText;
                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));
                    }
                    else {
                        MessageBox.Show("运行完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                string Me = Creat(FileManage.Delete_file(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
                MessageBox.Show(Me);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonGetActiveStatus_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFileName.Text = "";
                textBoxGetDownloadStatus.Text = "";
                textBoxFail_Reason.Text = "";
                textBoxActiveTime.Text = "";
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                //  string Me = Creat(FileManage.Active_File(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = Sendrpc(FileManage.Get_Active_Status(comboBoxFileType.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("file", "urn:ccsa:yang:acc-file");
                XmlNode file_name = xmlDoc.SelectSingleNode("//file:file-name", root);
                XmlNode active_time = xmlDoc.SelectSingleNode("//file:active-time", root);
                XmlNode status = xmlDoc.SelectSingleNode("//file:status", root);
                XmlNode fail_reason = xmlDoc.SelectSingleNode("//file:fail-reason", root);
                if (file_name != null) comboBoxFileName.Text = file_name.InnerText;
                if (active_time != null) textBoxActiveTime.Text = active_time.InnerText;
                if (status != null) textBoxGetDownloadStatus.Text = status.InnerText;
                if (fail_reason != null) textBoxFail_Reason.Text = fail_reason.InnerText;
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));
                    }
                    else
                    {
                        MessageBox.Show("运行完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonGetFileList_Click(object sender, EventArgs e)
        {
            try {

                textBoxFileList.Clear();
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            //  string Me = Creat(FileManage.Active_File(comboBoxFileType.Text, comboBoxFileName.Text), id, ip);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc = Sendrpc(FileManage.Get_File_List(comboBoxFileType.Text), id, ip);
            XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
            root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            root.AddNamespace("file", "urn:ccsa:yang:acc-file");
            XmlNodeList itemNodes = xmlDoc.SelectNodes("//file:file", root);
            string a="" ,b="";
            foreach (XmlNode itemNode in itemNodes)
            {
                XmlNode file_name = itemNode.SelectSingleNode("file:file-name", root);
                XmlNode file_type = itemNode.SelectSingleNode("file:file-type", root);
                if (file_name != null) a = file_name.InnerText;
                if (file_type != null) b = file_type.InnerText;
                textBoxFileList.AppendText( b + "\t" + a +"\r\n");
            }
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));

                    }
                    else
                    {
                        MessageBox.Show("运行完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
}

        private void buttonModifytime_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();

            MessageBox.Show(Creat(ModifyXML.Set_Managed_Element_Time(dateTimePickerCurrentTime.Text), id, ip));
        }

        private void buttonGetTime_Click(object sender, EventArgs e)
        {
            try
            {
               
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.Get_Managed_Element_Time(), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("me", "urn:ccsa:yang:acc-devm");
                XmlNode date_time = xmlDoc.SelectSingleNode("//me:date-time", root);
                if (date_time != null) dateTimePickerCurrentTime.Text = date_time.InnerText;
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));

                    }
                    else
                    {
                        MessageBox.Show("运行完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void buttonModifyPCTime_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            dateTimePickerCurrentTime.Value = DateTime.Now;
            dateTimePickerCurrentTime.CustomFormat = "yyyy-MM-ddTHH:mm:ss+08:00";
            MessageBox.Show(Creat(ModifyXML.Set_Managed_Element_Time(dateTimePickerCurrentTime.Text), id, ip));
        }

        private void buttonMcPortModify_Click(object sender, EventArgs e)
        {
            int id = int.Parse(treeViewNEID.SelectedNode.Name);
            int line = -1;
            for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
            {
                if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                {
                    line = i;
                    break;
                }
                if (line >= 0)
                    break;
            }
            string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
            Creat(ModifyXML.Mc_Port(comboBoxMcPort.Text, comboBoxMcPortModify.Text), id, ip);
            buttonMcPortGet.PerformClick();
        }

        private void buttonMcPortGet_Click(object sender, EventArgs e)
        {
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.Mc_Ports(comboBoxMcPort.Text), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("me", "urn:ccsa:yang:acc-devm");
                XmlNode admin_state = xmlDoc.SelectSingleNode("//me:admin-state", root); 
                if (admin_state != null) textBoxMcPortAdminState.Text = admin_state.InnerText;
                XmlNode operational_state = xmlDoc.SelectSingleNode("//me:operational-state", root); 
                if (operational_state != null) textBoxMcPortOperState.Text = operational_state.InnerText;
                // Console.Read();
                if (xmlDoc.DocumentElement != null)
                {

                    if (xmlDoc.InnerText.Contains("erro"))
                    {
                        MessageBox.Show(XmlFormat.Xml(xmlDoc.OuterXml));

                    }
                    else
                    {
                        MessageBox.Show("运行完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void 板卡人工倒换到主用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-primary", "eq");

        }

        private void 板卡人工倒换到备用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-secondary", "eq");

        }

        private void 板卡清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("cleared", "to-secondary", "eq");

        }

        private void 查看交互日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", @"C:\netconf\Log\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + "GWTT-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
        }

        private void 更改为当前ip地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewAuto.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    //((DataGridViewComboBoxCell)dataGridViewAuto.Rows[row.Index].Cells["Autoip地址"]).Items.Add("是");
                    dataGridViewAuto.Rows[row.Index].Cells["Autoip地址"].Value = TextIP.Text;
                }
            }
            MessageBox.Show("修改完成");
        }

        private void DeleteConnection() {
            textBoxConnections.Clear();
            textBoxTelnetLogs.Clear();
            string[,] dxc = null;
            int dxcrow = 0;
            int dxccol = 0;
            bool findts = false;
            string dxctype = "client";
            string dxcstr = "";
            string ID = "", Rate = "", Dir = "", Source = "", sink = "", P_Source = "", P_Sink = "", LPG = "";
            string ID_Stream = "", Rate_Stream = "", Dir_Stream = "", Source_Stream = "", sink_Stream = "", P_Source_Stream = "", P_Sink_Stream = "", LPG_Stream = "";
            string connectstr = Connection_Arrange.Connect(TextIP.Text, "admin", "greenway");
            //查询交叉ID和保护组
            textBoxTelnetLogs.AppendText(connectstr);
            textBoxTelnetLogs.AppendText("\r\n=========================================查询所在业务=========================================\r\n");
            dxcstr = Connection_Arrange.Get_DXC();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {
                            if (dxc[i, 4] == dxctype + ":" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxTs.Text)
                            {
                                ID = dxc[i, 1];
                                Rate = dxc[i, 2];
                                Dir = dxc[i, 3];
                                Source = dxc[i, 4];
                                sink = dxc[i, 5];
                                P_Source = dxc[i, 6];
                                P_Sink = dxc[i, 7];
                                LPG = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件oduk交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");

                                findts = true;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(dxc[i, 5]))
                        {
                            if (dxc[i, 5] == "logic" + ":" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxTs.Text)
                            {
                                ID = dxc[i, 1];
                                Rate = dxc[i, 2];
                                Dir = dxc[i, 3];
                                Source = dxc[i, 4];
                                sink = dxc[i, 5];
                                P_Source = dxc[i, 6];
                                P_Sink = dxc[i, 7];
                                LPG = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件oduk交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                findts = true;
                                break;
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件oduk交叉：不存在或错误 NOK" + "\r\n");
                    textBoxTelnetLogs.AppendText("您勾选源时隙：\t" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxTs.Text + "\r\n");
                    ID = "空"; Rate = "空"; Dir = "空"; Source = "空"; sink = "空"; P_Source = "空"; P_Sink = "空"; LPG = "空";

                }
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {
                            if (dxc[i, 4] == sink)
                            {
                                ID_Stream = dxc[i, 1];
                                Rate_Stream = dxc[i, 2];
                                Dir_Stream = dxc[i, 3];
                                Source_Stream = dxc[i, 4];
                                sink_Stream = dxc[i, 5];
                                P_Source_Stream = dxc[i, 6];
                                P_Sink_Stream = dxc[i, 7];
                                LPG_Stream = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件Stream交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");

                                findts = true;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(dxc[i, 5]))
                        {
                            if (dxc[i, 5] == dxctype + ":" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxTs.Text)
                            {
                                ID_Stream = dxc[i, 1];
                                Rate_Stream = dxc[i, 2];
                                Dir_Stream = dxc[i, 3];
                                Source_Stream = dxc[i, 4];
                                sink_Stream = dxc[i, 5];
                                P_Source_Stream = dxc[i, 6];
                                P_Sink_Stream = dxc[i, 7];
                                LPG_Stream = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件Stream交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                findts = true;
                                break;
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件Stream交叉：不存在或错误 NOK" + "\r\n");
                    ID_Stream = "空"; Rate_Stream = "空"; Dir_Stream = "空"; Source_Stream = "空"; sink_Stream = "空"; P_Source_Stream = "空"; P_Sink_Stream = "空"; LPG_Stream = "空";
                }


            }
            textBoxTelnetLogs.AppendText("\r\n=========================================删除UMS配置=========================================\r\n");
            //删除UMS配置
            dxcstr = Connection_Arrange.Get_UMS();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol >= 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 5]))
                        {
                            if (dxc[i, 5] == ID && dxc[i, 2] == ("ums-cross"))
                            {

                                string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4]);
                                textBoxTelnetLogs.AppendText(dxcstrID);
                                findts = true;
                                break;
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件UMS交叉：不存在或错误 NOK" + "\r\n");

                }

                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 8)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (Source.Contains("/"))
                            {
                                string [] _Source = Source.Split(new char[] { ':' });
                                string[] portts = _Source[1].Split(new char[] { '/' });
                                string Slot = portts[0];
                                string Port = portts[1];
                                string TS = portts[2];
                                if (dxc[i, 5] == (Slot + "/" + Port) && dxc[i, 8] == (TS) && dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ctp"))
                                {
                                    string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4] + " " + dxc[i, 5] + " " + dxc[i, 6] + " " + dxc[i, 7] + " " + dxc[i, 8]);
                                    textBoxTelnetLogs.AppendText(dxcstrID);
                                    findts = true;
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (sink.Contains("/"))
                            {
                                string[] _Source = sink.Split(new char[] { ':' });
                                string[] portts = _Source[1].Split(new char[] { '/' });
                                string Slot = portts[0];
                                string Port = portts[1];
                                string TS = portts[2];
                                if (dxc[i, 5] == (Slot + "/" + Port) && dxc[i, 8] == (TS) && dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ctp"))
                                {
                                    string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4] + " " + dxc[i, 5] + " " + dxc[i, 6] + " " + dxc[i, 7] + " " + dxc[i, 8]);
                                    textBoxTelnetLogs.AppendText(dxcstrID);
                                    findts = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (P_Source.Contains("/"))
                            {
                                string[] _Source = P_Source.Split(new char[] { ':' });
                                string[] portts = _Source[1].Split(new char[] { '/' });
                                string Slot = portts[0];
                                string Port = portts[1];
                                string TS = portts[2];
                                if (dxc[i, 5] == (Slot + "/" + Port) && dxc[i, 8] == (TS) && dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ctp"))
                                {
                                    string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4] + " " + dxc[i, 5] + " " + dxc[i, 6] + " " + dxc[i, 7] + " " + dxc[i, 8]);
                                    textBoxTelnetLogs.AppendText(dxcstrID);
                                    findts = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (P_Sink.Contains("/"))
                            {
                                string[] _Source = P_Sink.Split(new char[] { ':' });
                                string[] portts = _Source[1].Split(new char[] { '/' });
                                string Slot = portts[0];
                                string Port = portts[1];
                                string TS = portts[2];
                                if (dxc[i, 5] == (Slot + "/" + Port) && dxc[i, 8] == (TS) && dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ctp"))
                                {
                                    string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4] + " " + dxc[i, 5] + " " + dxc[i, 6] + " " + dxc[i, 7] + " " + dxc[i, 8]);
                                    textBoxTelnetLogs.AppendText(dxcstrID);
                                    findts = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (sink.Contains("/"))
                            {
                                string[] _Source = sink.Split(new char[] { ':' });
                                string[] portts = _Source[1].Split(new char[] { '/' });
                                string Slot = portts[0];
                                string Port = portts[1];
                                string stream1 = portts[2];
                                if (dxc[i, 4] == (Slot + "/" + Port) && dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ftp"))
                                {
                                    string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3]);
                                    textBoxTelnetLogs.AppendText(dxcstrID);
                                    findts = true;
                                }
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件UMS的CTP配置：不存在或错误 NOK" + "\r\n");

                }


            }




            textBoxTelnetLogs.AppendText("\r\n=========================================删除DXC交叉=========================================\r\n");
            //删除交叉
            if (!string.IsNullOrEmpty(ID) && ID != "空")
            {
                dxcstr = Connection_Arrange.Delete_DXC(ID);
                textBoxTelnetLogs.AppendText(dxcstr);
            }
            if (!string.IsNullOrEmpty(ID_Stream) && ID_Stream != "空")
            {
                dxcstr = Connection_Arrange.Delete_DXC(ID_Stream);
                textBoxTelnetLogs.AppendText(dxcstr);
            }
            if (!string.IsNullOrEmpty(LPG) && LPG != "空")
            {
                dxcstr = Connection_Arrange.Delete_UMS_LPG(LPG);
                textBoxTelnetLogs.AppendText(dxcstr);
            }
            textBoxTelnetLogs.AppendText("\r\n=========================================删除CTP接口=========================================\r\n");
            //删除CTP的物理时隙
            dxcstr = Connection_Arrange.Get_CTP();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 4)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]) && !dxc[i, 4].Contains("-"))
                        {
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == Source && dxc[i, 3].Contains("ODU")) || Source == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n工作源ODU的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                //Source = dxc[i, 4];
                                findts = true;
                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == sink && dxc[i, 3].Contains("ODU")) || sink == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n工作宿ODU的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                //sink = dxc[i, 4];
                                findts = true;
                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == P_Source && dxc[i, 3].Contains("ODU")) || P_Source == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n保护源ODU的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                //P_Source = dxc[i, 4];
                                findts = true;
                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == P_Sink && dxc[i, 3].Contains("ODU")) || P_Sink == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n保护宿ODU的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                               // P_Sink = dxc[i, 4];
                                findts = true;

                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == Source_Stream && dxc[i, 3].Contains("stream")) || Source_Stream == "初始化" || (dxc[i, 2] + dxc[i, 3] + dxc[i, 4]) == "client" + "stream" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxStream.Text)
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n工作源流的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                //  Source_Stream = dxc[i, 4];
                                findts = true;
                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == sink_Stream && dxc[i, 3].Contains("stream")) || sink_Stream == "初始化" || (dxc[i, 2] + dxc[i, 3] + dxc[i, 4]) == "logic" + "stream" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxStream.Text)
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n工作宿流的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                // sink_Stream = dxc[i, 4];
                                findts = true;

                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == P_Source_Stream && dxc[i, 3].Contains("stream")) || P_Source_Stream == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n保护源流的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                findts = true;

                            }
                            if (((dxc[i, 2] + ":" + dxc[i, 4]) == P_Sink_Stream && dxc[i, 3].Contains("stream")) || P_Sink_Stream == "初始化")
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n保护宿流的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                findts = true;

                            }
                        }
                    }

                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n业务所在CTP接口：不存在或错误 NOK" + "\r\n");

                }

            }

            textBoxTelnetLogs.AppendText("\r\n=========================================检查配置=========================================\r\n");
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_ETH_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_PTN_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_UMS());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_DXC());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_CTP());
            MessageBox.Show("运行完成");
        }
        private void buttonGetConnection_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(DeleteConnection);
            t.Start();
        }

        private void buttonDeleteConnection_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(DeleteConnectionAll);
            t.Start();
        }

        private void 查看Telnet交互日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", @"C:\netconf\Logs\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + TextIP.Text+"-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
        }
        private void DeleteConnectionAll()
        {
            textBoxConnections.Clear();
            textBoxTelnetLogs.Clear();
            string[,] dxc = null;
            int dxcrow = 0;
            int dxccol = 0;
            bool findts = false;
            string dxctype = "client";
            string dxcstr = "";
            string ID = "", Rate = "", Dir = "", Source = "", sink = "", P_Source = "", P_Sink = "", LPG = "";
            string ID_Stream = "", Rate_Stream = "", Dir_Stream = "", Source_Stream = "", sink_Stream = "", P_Source_Stream = "", P_Sink_Stream = "", LPG_Stream = "";
            string connectstr = Connection_Arrange.Connect(TextIP.Text, "admin", "greenway");
            //查询交叉ID和保护组
            textBoxTelnetLogs.AppendText(connectstr);
            textBoxTelnetLogs.AppendText("\r\n=========================================删除UMS配置=========================================\r\n");
            //删除UMS配置
            dxcstr = Connection_Arrange.Get_UMS();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol >= 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 5]))
                        {
                            if (dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-cross"))
                            {

                                string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4]);
                                textBoxTelnetLogs.AppendText(dxcstrID);
                                findts = true;
                            }
                        }
                    }
                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件UMS交叉：不存在或错误 NOK" + "\r\n");
                }
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 8)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 8]))
                        {
                            if (dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-lpg"))
                            {
                                string dxcstrID = Connection_Arrange.Delete_UMS_LPG(dxc[i, 3]);
                                textBoxTelnetLogs.AppendText(dxcstrID);
                                findts = true;
                            }
                            if (dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ctp"))
                            {
                                string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3] + " " + dxc[i, 4] + " " + dxc[i, 5] + " " + dxc[i, 6] + " " + dxc[i, 7] + " " + dxc[i, 8]);
                                textBoxTelnetLogs.AppendText(dxcstrID);
                                findts = true;
                            }
                            if (dxc[i, 1] == ("create") && dxc[i, 2] == ("ums-ftp"))
                            {
                                string dxcstrID = Connection_Arrange.Delete_UMS(dxc[i, 2] + " " + dxc[i, 3]);
                                textBoxTelnetLogs.AppendText(dxcstrID);
                                findts = true;
                            }
                        }

                    }
                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件UMS的CTP配置：不存在或错误 NOK" + "\r\n");

                }
            }
            textBoxTelnetLogs.AppendText("\r\n=========================================查询所在业务=========================================\r\n");
            dxcstr = Connection_Arrange.Get_DXC();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                textBoxTelnetLogs.AppendText("\r\n=========================================删除DXC交叉=========================================\r\n");
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {


                            if (dxc[i, 4].Contains("/")&& dxc[i, 2].Contains("ODU"))
                            {
                                ID = dxc[i, 1];
                                Rate = dxc[i, 2];
                                Dir = dxc[i, 3];
                                Source = dxc[i, 4];
                                sink = dxc[i, 5];
                                P_Source = dxc[i, 6];
                                P_Sink = dxc[i, 7];
                                LPG = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件oduk交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                //删除交叉
                                if (!string.IsNullOrEmpty(ID) && ID != "空")
                                {
                                    dxcstr = Connection_Arrange.Delete_DXC(ID);
                                    textBoxTelnetLogs.AppendText(dxcstr);
                                }
                                findts = true;
                                
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件oduk交叉：不存在或错误 NOK" + "\r\n");
                    textBoxTelnetLogs.AppendText("您勾选源时隙：\t" + comboBoxSlot.Text + "/" + comboBoxPort.Text + "/" + comboBoxTs.Text + "\r\n");
                    ID = "空"; Rate = "空"; Dir = "空"; Source = "空"; sink = "空"; P_Source = "空"; P_Sink = "空"; LPG = "空";

                }
                textBoxTelnetLogs.AppendText("\r\n=========================================删除Stream交叉=========================================\r\n");

                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {
                            if (dxc[i, 4].Contains("/") && dxc[i, 2].Contains("stream"))
                            {
                                ID_Stream = dxc[i, 1];
                                Rate_Stream = dxc[i, 2];
                                Dir_Stream = dxc[i, 3];
                                Source_Stream = dxc[i, 4];
                                sink_Stream = dxc[i, 5];
                                P_Source_Stream = dxc[i, 6];
                                P_Sink_Stream = dxc[i, 7];
                                LPG_Stream = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件Stream交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                if (!string.IsNullOrEmpty(ID_Stream) && ID_Stream != "空")
                                {
                                    dxcstr = Connection_Arrange.Delete_DXC(ID_Stream);
                                    textBoxTelnetLogs.AppendText(dxcstr);
                                }
                                findts = true;
                                
                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件Stream交叉：不存在或错误 NOK" + "\r\n");
                    ID_Stream = "空"; Rate_Stream = "空"; Dir_Stream = "空"; Source_Stream = "空"; sink_Stream = "空"; P_Source_Stream = "空"; P_Sink_Stream = "空"; LPG_Stream = "空";
                }

                textBoxTelnetLogs.AppendText("\r\n=========================================删除OSU交叉=========================================\r\n");

                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {
                            if (dxc[i, 4].Contains("/") && dxc[i, 2].Contains("OSU"))
                            {
                                ID_Stream = dxc[i, 1];
                                Rate_Stream = dxc[i, 2];
                                Dir_Stream = dxc[i, 3];
                                Source_Stream = dxc[i, 4];
                                sink_Stream = dxc[i, 5];
                                P_Source_Stream = dxc[i, 6];
                                P_Sink_Stream = dxc[i, 7];
                                LPG_Stream = dxc[i, 9];
                                textBoxConnections.AppendText("\r\n软件OSU交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                if (!string.IsNullOrEmpty(ID_Stream) && ID_Stream != "空")
                                {
                                    dxcstr = Connection_Arrange.Delete_DXC(ID_Stream);
                                    textBoxTelnetLogs.AppendText(dxcstr);
                                }
                                findts = true;

                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\n软件OSU交叉：不存在或错误 NOK" + "\r\n");
                    ID_Stream = "空"; Rate_Stream = "空"; Dir_Stream = "空"; Source_Stream = "空"; sink_Stream = "空"; P_Source_Stream = "空"; P_Sink_Stream = "空"; LPG_Stream = "空";
                }
                textBoxTelnetLogs.AppendText("\r\n=========================================删除VC交叉=========================================\r\n");

                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 5)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]))
                        {
                            if (dxc[i, 4].Contains("/") && dxc[i, 2].Contains("VC"))
                            {
                                ID_Stream = dxc[i, 1];
                                Rate_Stream = dxc[i, 2];
                                Dir_Stream = dxc[i, 3];
                                Source_Stream = dxc[i, 4];
                                sink_Stream = dxc[i, 5];
                                P_Source_Stream = dxc[i, 6];
                                P_Sink_Stream = dxc[i, 7];
                                LPG_Stream = dxc[i, 9];
                                textBoxConnections.AppendText("\r\nVC交叉：\t" + "\r\n");
                                textBoxConnections.AppendText("交叉ID：\t" + dxc[i, 1] + "\r\n");
                                textBoxConnections.AppendText("速率：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("工作源时隙：\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("工作宿时隙：\t" + dxc[i, 5] + "\r\n");
                                textBoxConnections.AppendText("保护源时隙：\t" + dxc[i, 6] + "\r\n");
                                textBoxConnections.AppendText("保护宿时隙：\t" + dxc[i, 7] + "\r\n");
                                textBoxConnections.AppendText("保护组ID：\t" + dxc[i, 9] + "\r\n");
                                if (!string.IsNullOrEmpty(ID_Stream) && ID_Stream != "空")
                                {
                                    dxcstr = Connection_Arrange.Delete_DXC(ID_Stream);
                                    textBoxTelnetLogs.AppendText(dxcstr);
                                }
                                findts = true;

                            }
                        }
                    }


                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\nVC交叉：不存在或错误 NOK" + "\r\n");
                    ID_Stream = "空"; Rate_Stream = "空"; Dir_Stream = "空"; Source_Stream = "空"; sink_Stream = "空"; P_Source_Stream = "空"; P_Sink_Stream = "空"; LPG_Stream = "空";
                }
            }
            textBoxTelnetLogs.AppendText("\r\n=========================================删除CTP接口=========================================\r\n");
            //删除CTP的物理时隙
            dxcstr = Connection_Arrange.Get_CTP();
            textBoxTelnetLogs.AppendText(dxcstr);
            if (!string.IsNullOrEmpty(dxcstr))
            {
                dxc = Connection_Arrange.StringToArray(dxcstr);
                dxcrow = dxc.GetLength(0);
                dxccol = dxc.GetLength(1);
                for (int i = 0; i < dxcrow; i++)
                {
                    if (dxccol > 4)
                    {
                        if (!string.IsNullOrEmpty(dxc[i, 4]) && !dxc[i, 4].Contains("-"))
                        {
                            if (dxc[i, 4].Contains("/"))
                            {
                                dxcstr = Connection_Arrange.Delete_CTP(dxc[i, 2], dxc[i, 3], dxc[i, 4], dxc[i, 7]);
                                textBoxConnections.AppendText("\r\n的CTP接口：\t" + "\r\n");
                                textBoxConnections.AppendText("dir：\t\t" + dxc[i, 2] + "\r\n");
                                textBoxConnections.AppendText("rate：\t\t" + dxc[i, 3] + "\r\n");
                                textBoxConnections.AppendText("port：\t\t" + dxc[i, 4] + "\r\n");
                                textBoxConnections.AppendText("crtinfo：\t" + dxc[i, 7] + "\r\n");
                                textBoxTelnetLogs.AppendText(dxcstr);
                                findts = true;
                            }
                        }
                    }

                }
                if (findts == false)
                {
                    textBoxTelnetLogs.AppendText("\r\nCTP接口：不存在或错误 NOK" + "\r\n");

                }

            }

            textBoxTelnetLogs.AppendText("\r\n=========================================检查配置=========================================\r\n");
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_ETH_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_PTN_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_UMS());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_DXC());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_CTP());
            MessageBox.Show("运行完成");
        }

        private void buttonConnectionGet_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(OTNConfig);
            t.Start();
        }
        private void OTNConfig() {

            bool isAll = false;

            DialogResult dr1 = MessageBox.Show("是否查询所有OTN配置？\r\n选择否则进行指定端口查询（请选择槽位和接口后进行）", "提示", MessageBoxButtons.YesNoCancel);
            if (dr1 == DialogResult.Yes)
            {
                isAll = true; 
            }
            if (dr1 == DialogResult.No)
            {
                isAll = false;
            }
            if (dr1 == DialogResult.Cancel)
            {
                return;
            }
            textBoxConnections.Clear();
            textBoxTelnetLogs.Clear();
            string dxcstr = "";
            string connectstr = Connection_Arrange.Connect(TextIP.Text, "admin", "greenway");
            //查询交叉ID和保护组
            textBoxTelnetLogs.AppendText(connectstr);
            dxcstr = Connection_Arrange.Get_OTN();
            dxcstr += Connection_Arrange.Get_PTN_CTP();
            textBoxTelnetLogs.AppendText(dxcstr);
            string[] PortConfig = dxcstr.Split(new char[] { '\n' });
            string FindPortAll = "";
            for (int i = 0; i < PortConfig.Length; i++)
            {
                if (!FindPortAll.Contains(PortConfig[i])) {

                    if (PortConfig[i].Contains("interface ") && isAll)
                    {
                        FindPortAll += PortConfig[i] + ",";
                        textBoxConnections.AppendText(PortConfig[i] + "\r\n");

                        for (int j = 0; j < PortConfig.Length; j++)
                        {
                            if (PortConfig[j] == PortConfig[i])
                            {
                                for (int n = 0; n < PortConfig.Length; n++)
                                {
                                    if (!PortConfig[j + n].Contains("interface") && !PortConfig[j + n].Contains("exit"))
                                    {
                                        textBoxConnections.AppendText("   " + PortConfig[j + n] + "\r\n");
                                    }
                                    if (PortConfig[j + n].Contains("exit"))
                                    {

                                        break;
                                    }
                                }

                            }
                        }
                        textBoxConnections.AppendText("     exit\r\n");
                    }
                    if (PortConfig[i].Contains("interface ") && isAll==false && PortConfig[i].Contains(" "+comboBoxSlot.Text+"/"+ comboBoxPort.Text+"\r"))
                    {
                        FindPortAll += PortConfig[i] + ",";
                        textBoxConnections.AppendText(PortConfig[i] + "\r\n");

                        for (int j = 0; j < PortConfig.Length; j++)
                        {
                            if (PortConfig[j] == PortConfig[i])
                            {
                                for (int n = 0; n < PortConfig.Length; n++)
                                {
                                    if (!PortConfig[j + n].Contains("interface") && !PortConfig[j + n].Contains("exit"))
                                    {
                                        textBoxConnections.AppendText("   " + PortConfig[j + n] + "\r\n");
                                    }
                                    if (PortConfig[j + n].Contains("exit"))
                                    {

                                        break;
                                    }
                                }

                            }
                        }
                        textBoxConnections.AppendText("     exit\r\n");
                    }
                }
            }
            textBoxTelnetLogs.AppendText("\r\n=========================================检查配置=========================================\r\n");
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_ETH_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_PTN_CTP());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_UMS());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_DXC());
            textBoxTelnetLogs.AppendText(Connection_Arrange.Get_CTP());
            
        }

        private void buttonGetCurrentAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewAlarm.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.Alarms(comboBoxAlarmPort.Text);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("alarms", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//alarms:alarms//alarms:alarm", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewAlarm.Rows.Add();
                    XmlNode alarm_serial_no = itemNode.SelectSingleNode("alarms:alarm-serial-no", root);
                    XmlNode object_name = itemNode.SelectSingleNode("alarms:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("alarms:object-type", root);
                    //XmlNode granularity = itemNode.SelectSingleNode("alarms:granularity", root);
                    XmlNode alarm_code = itemNode.SelectSingleNode("alarms:alarm-code", root);
                    XmlNode alarm_state = itemNode.SelectSingleNode("alarms:alarm-state", root);
                    XmlNode perceived_severity = itemNode.SelectSingleNode("alarms:perceived-severity", root);
                    XmlNode additional_text = itemNode.SelectSingleNode("alarms:additional-text", root);
                    XmlNode start_time = itemNode.SelectSingleNode("alarms:start-time", root);
                    XmlNode end_time = itemNode.SelectSingleNode("alarms:end-time", root);
                    if (alarm_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = alarm_serial_no.InnerText; }
                    if (alarm_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = treeViewNEID.SelectedNode.Text; }
                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                    if (perceived_severity != null) {
                        switch (perceived_severity.InnerText)
                        {
                            case "critical":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                //alarmlog.SubItems[5].BackColor = Color.Red;
                                break;
                            case "major":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                break;
                            case "minor":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                break;
                            case "warning":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                break;
                        }
                      //  dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                    }
                    if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                    if (alarm_state != null) {
                        if (alarm_state.InnerText == "start") {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                        }
                    }

                    // LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonGetPortList_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxAlarmPort.Items.Clear();
                comboBoxAlarmPort.Items.Add("全部端口");
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                xmlDoc = Sendrpc(GetXML.PtpsFtpsCtps(true, true, true), id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                //  root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp|//ptpsxmlns:ftps//ptpsxmlns:ftp|//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    if (name != null)
                    {
                        comboBoxAlarmPort.Items.Add(name.InnerText);
                        if (comboBoxAlarmPort.Items.Count > 0)
                        {
                            comboBoxAlarmPort.SelectedIndex = comboBoxAlarmPort.Items.Count - 1;
                        }
                    }
                }
                if (xmlDoc.DocumentElement != null)
                    MessageBox.Show("运行完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void buttonUTC_Click(object sender, EventArgs e)
        {
            if (ButUTC.Text.Contains("UTC"))
            {
                dateTimePickerStartTime.Value = DateTime.UtcNow;
                dateTimePickerStopTime.Value = DateTime.UtcNow;
                dateTimePickerStartTime.CustomFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
                dateTimePickerStopTime.CustomFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
                ButUTC.Text = "北京时间";
            }
            else
            {
                dateTimePickerStartTime.Value = DateTime.Now;
                dateTimePickerStopTime.Value = DateTime.Now;
                dateTimePickerStartTime.CustomFormat = "yyyy-MM-ddTHH:mm:ss+08:00";
                dateTimePickerStopTime.CustomFormat = "yyyy-MM-ddTHH:mm:ss+08:00";
                ButUTC.Text = "UTC时间";
            }
        }

        private void buttonGetHistoryAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewAlarm.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.Get_History_Alarms(dateTimePickerStartTime.Text, dateTimePickerStopTime.Text);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("alarms", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//alarms:alarms//alarms:alarm", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewAlarm.Rows.Add();
                    XmlNode alarm_serial_no = itemNode.SelectSingleNode("alarms:alarm-serial-no", root);
                    XmlNode object_name = itemNode.SelectSingleNode("alarms:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("alarms:object-type", root);
                    //XmlNode granularity = itemNode.SelectSingleNode("alarms:granularity", root);
                    XmlNode alarm_code = itemNode.SelectSingleNode("alarms:alarm-code", root);
                    XmlNode alarm_state = itemNode.SelectSingleNode("alarms:alarm-state", root);
                    XmlNode perceived_severity = itemNode.SelectSingleNode("alarms:perceived-severity", root);
                    XmlNode additional_text = itemNode.SelectSingleNode("alarms:additional-text", root);
                    XmlNode start_time = itemNode.SelectSingleNode("alarms:start-time", root);
                    XmlNode end_time = itemNode.SelectSingleNode("alarms:end-time", root);
                    if (alarm_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = "alarm：" +  alarm_serial_no.InnerText; }
                    if (alarm_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = treeViewNEID.SelectedNode.Text; }
                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                    if (alarm_code != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = alarm_code.InnerText; }
                    if (alarm_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = alarm_state.InnerText; }
                    dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                    if (perceived_severity != null)
                    {
                        switch (perceived_severity.InnerText)
                        {
                            case "critical":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "紧急" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.OrangeRed;
                                //alarmlog.SubItems[5].BackColor = Color.Red;
                                break;
                            case "major":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "重要" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Orange;
                                break;
                            case "minor":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "次要" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.Yellow;
                                break;
                            case "warning":
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = "提示" + perceived_severity.InnerText;
                                dataGridViewAlarm.Rows[index].Cells["告警级别"].Style.BackColor = Color.DeepSkyBlue;
                                break;
                        }
                        //  dataGridViewAlarm.Rows[index].Cells["告警级别"].Value = perceived_severity.InnerText;
                    }
                    if (additional_text != null) { dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = additional_text.InnerText; }
                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                    if (alarm_state != null)
                    {
                        if (alarm_state.InnerText == "start")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                        }
                        if (alarm_state.InnerText == "end")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                        }
                    }

                    // LabPerCount.Text = (index + 1).ToString();
                }
                 itemNodes = xmlDoc.SelectNodes("//alarms:tcas//alarms:tca", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewAlarm.Rows.Add();
                    XmlNode tca_serial_no = itemNode.SelectSingleNode("alarms:tca-serial-no", root);
                    XmlNode object_name = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:object-type", root);
                    XmlNode granularity = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:granularity", root);
                    XmlNode pm_parameter_name = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:pm-parameter-name", root);
                    XmlNode tca_state = itemNode.SelectSingleNode("alarms:tca-state", root);
                    XmlNode threshold_value = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:threshold-value", root);
                    XmlNode threshold_type = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:threshold-type", root);
                    XmlNode current_value = itemNode.SelectSingleNode("alarms:current-value", root);
                    XmlNode start_time = itemNode.SelectSingleNode("alarms:start-time", root);
                    XmlNode end_time = itemNode.SelectSingleNode("alarms:end-time", root);
                    if (tca_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = "tca:"+ tca_serial_no.InnerText; }
                    if (tca_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = treeViewNEID.SelectedNode.Text; }
                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                    if (pm_parameter_name != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = pm_parameter_name.InnerText; }
                    if (tca_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = tca_state.InnerText; }
                    if (threshold_value != null && threshold_type != null && current_value!=null) {
                        dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText;
                        dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText;
                        dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText;
                    }
                    if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                    if (tca_state != null)
                    {
                        if (tca_state.InnerText == "start")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                        }
                        if (tca_state.InnerText == "end")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                            dataGridViewAlarm.Rows[index].Cells["告警状态"].Style.BackColor = Color.GreenYellow;
                        }
                    }

                    // LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonTcaAlarms_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewAlarm.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = GetXML.TcasAlarms(comboBoxAlarmPort.Text);
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                var xmlDoc = Sendrpc(xmlDoc1, id, ip);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("alarms", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//alarms:tcas//alarms:tca", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewAlarm.Rows.Add();
                    XmlNode tca_serial_no = itemNode.SelectSingleNode("alarms:tca-serial-no", root);
                    XmlNode object_name = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:object-type", root);
                    XmlNode granularity = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:granularity", root);
                    XmlNode pm_parameter_name = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:pm-parameter-name", root);
                    XmlNode tca_state = itemNode.SelectSingleNode("alarms:tca-state", root);
                    XmlNode threshold_value = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:threshold-value", root);
                    XmlNode threshold_type = itemNode.SelectSingleNode("alarms:tca-parameter//alarms:threshold-type", root);
                    XmlNode current_value = itemNode.SelectSingleNode("alarms:current-value", root);
                    XmlNode start_time = itemNode.SelectSingleNode("alarms:start-time", root);
                    XmlNode end_time = itemNode.SelectSingleNode("alarms:end-time", root);
                    if (tca_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警编号"].Value = tca_serial_no.InnerText; }
                    if (tca_serial_no != null) { dataGridViewAlarm.Rows[index].Cells["告警网元"].Value = treeViewNEID.SelectedNode.Text; }
                    if (object_name != null) { dataGridViewAlarm.Rows[index].Cells["告警对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewAlarm.Rows[index].Cells["告警对象类型"].Value = object_type.InnerText; }
                    if (pm_parameter_name != null) { dataGridViewAlarm.Rows[index].Cells["告警名称"].Value = pm_parameter_name.InnerText; }
                    if (tca_state != null) { dataGridViewAlarm.Rows[index].Cells["告警状态"].Value = tca_state.InnerText; }
                    if (threshold_value != null && threshold_type != null && current_value != null)
                    {
                       // dataGridViewAlarm.Rows[index].Cells["告警描述"].Value = "类型： " + threshold_type.InnerText + " 阈值： " + threshold_value.InnerText + " 当前值： " + current_value.InnerText;
                        dataGridViewAlarm.Rows[index].Cells["TCA阈值类型"].Value = threshold_type.InnerText;
                        dataGridViewAlarm.Rows[index].Cells["TCA阈值"].Value = threshold_value.InnerText;
                        dataGridViewAlarm.Rows[index].Cells["TCA当前值"].Value = current_value.InnerText;
                    }
                    if (granularity != null) { dataGridViewAlarm.Rows[index].Cells["TCA周期"].Value = granularity.InnerText; }
                    if (end_time != null) { dataGridViewAlarm.Rows[index].Cells["告警结束时间"].Value = end_time.InnerText; }
                    if (start_time != null) { dataGridViewAlarm.Rows[index].Cells["告警开始时间"].Value = start_time.InnerText; }
                    if (tca_state != null)
                    {
                        if (tca_state.InnerText == "start")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "未清除";
                        }
                        if (tca_state.InnerText == "end")
                        {
                            dataGridViewAlarm.Rows[index].Cells["确认告警"].Value = "已清除";
                        }
                    }
                    // LabPerCount.Text = (index + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 业务详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewEth.Rows[row.Index].Cells["CTP端口1"].Value.ToString();       //设备IP地址
                        int id = int.Parse(treeViewNEID.SelectedNode.Name);
                        int line = -1;
                        for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                        {
                            if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                            {
                                line = i;
                                break;
                            }
                            if (line >= 0)
                                break;
                        }
                        string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                        string[] strArray = PtpsFtpsCtps.Split(',');
                        foreach (var item in strArray)
                        {
                            if (item != "" && item.Contains("CTP"))
                            {
                                try
                                {
                                    //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                    // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                    XmlDocument xmlDoc = new XmlDocument();
                                    //xmlDoc.Load(filename);
                                    xmlDoc = Sendrpc(GetXML.CTP(item), id, ip);
                                    XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                    root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                    root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                                    foreach (XmlNode itemNode in itemNodes)
                                    {
                                        XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                                        XmlNode server_tp = itemNode.SelectSingleNode("ptpsxmlns:server-tp", root);
                                        if (server_tp != null)
                                        {
                                            if (server_tp.InnerText.Contains("PTP"))
                                            {
                                                PtpsFtpsCtps = PtpsFtpsCtps + "," + server_tp.InnerText;
                                            }
                                            if (server_tp.InnerText.Contains("FTP"))
                                            {
                                                PtpsFtpsCtps = PtpsFtpsCtps + "," + server_tp.InnerText;
                                                try
                                                {
                                                    XmlDocument xmlDoc0 = new XmlDocument();
                                                    xmlDoc0 = Sendrpc(GetXML.FTP(server_tp.InnerText), id, ip);
                                                    XmlNamespaceManager root0 = new XmlNamespaceManager(xmlDoc0.NameTable);
                                                    root0.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                                    root0.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                                    root0.AddNamespace("eth", "urn:ccsa:yang:acc-eth");
                                                    root0.AddNamespace("eos", "urn:ccsa:yang:acc-eos");
                                                    XmlNodeList itemNodes0 = xmlDoc0.SelectNodes("//ptpsxmlns:ftps//ptpsxmlns:ftp//ptpsxmlns:server-ctp", root0);
                                                    foreach (XmlNode itemNode0 in itemNodes0)
                                                    {
                                                        if (itemNode0 != null)
                                                        {
                                                            string ctp = itemNode0.InnerText;
                                                            PtpsFtpsCtps = PtpsFtpsCtps + "," + ctp;
                                                        }

                                                    }
                                                    // Console.Read();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                                }
                                            }
                                        }
                                    }
                                    // Console.Read();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                }
                            }
                        }
                        var Formoam = new Form_Connection_Info(id, ip, PtpsFtpsCtps);
                        Formoam.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridViewEth_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewEth.Rows[row.Index].Cells["CTP端口1"].Value.ToString();       //设备IP地址
                        int id = int.Parse(treeViewNEID.SelectedNode.Name);
                        int line = -1;
                        for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                        {
                            if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                            {
                                line = i;
                                break;
                            }
                            if (line >= 0)
                                break;
                        }
                        string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                        string[] strArray = PtpsFtpsCtps.Split(',');
                        foreach (var item in strArray)
                        {
                            if (item != "" && item.Contains("CTP"))
                            {
                                try
                                {
                                    //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                                    // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                                    XmlDocument xmlDoc = new XmlDocument();
                                    //xmlDoc.Load(filename);
                                    xmlDoc = Sendrpc(GetXML.CTP(item), id, ip);
                                    XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                                    root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                    root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                                    foreach (XmlNode itemNode in itemNodes)
                                    {
                                        XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                                        XmlNode server_tp = itemNode.SelectSingleNode("ptpsxmlns:server-tp", root);
                                        if (server_tp != null)
                                        {
                                            if (server_tp.InnerText.Contains("PTP"))
                                            {
                                                PtpsFtpsCtps = PtpsFtpsCtps + "," + server_tp.InnerText;
                                            }
                                            if (server_tp.InnerText.Contains("FTP"))
                                            {
                                                PtpsFtpsCtps = PtpsFtpsCtps + "," + server_tp.InnerText;
                                                try
                                                {
                                                    XmlDocument xmlDoc0 = new XmlDocument();
                                                    xmlDoc0 = Sendrpc(GetXML.FTP(server_tp.InnerText), id, ip);
                                                    XmlNamespaceManager root0 = new XmlNamespaceManager(xmlDoc0.NameTable);
                                                    root0.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                                                    root0.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                                                    root0.AddNamespace("eth", "urn:ccsa:yang:acc-eth");
                                                    root0.AddNamespace("eos", "urn:ccsa:yang:acc-eos");
                                                    XmlNodeList itemNodes0 = xmlDoc0.SelectNodes("//ptpsxmlns:ftps//ptpsxmlns:ftp//ptpsxmlns:server-ctp", root0);
                                                    foreach (XmlNode itemNode0 in itemNodes0)
                                                    {
                                                        if (itemNode0 != null) {
                                                            string ctp = itemNode0.InnerText;
                                                            PtpsFtpsCtps = PtpsFtpsCtps + "," + ctp;
                                                        }
                                                          
                                                    }
                                                    // Console.Read();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                                }
                                            }
                                        }
                                    }
                                    // Console.Read();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());   //读取该节点的相关信息
                                }
                            }
                        }
                        var Formoam = new Form_Connection_Info(id, ip, PtpsFtpsCtps);
                        Formoam.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        int logindex = 0;
        private void buttonLogFind_Click(object sender, EventArgs e)
        {
            if (textBoxLogFind.Text == "")
            {
                MessageBox.Show("查询字符串为空！");
                return;
            }
            logindex = TextLog.Text.IndexOf(textBoxLogFind.Text, logindex);
            if (logindex < 0)
            {
                logindex = 0;
                TextLog.SelectionStart = 0;
                TextLog.SelectionLength = 0;
                MessageBox.Show("已到结尾");
                return;
            }
            TextLog.SelectionStart = logindex;
            TextLog.SelectionLength = textBoxLogFind.Text.Length;
            logindex++;
            TextLog.Focus();
            TextLog.ScrollToCaret();

        }

        private void buttonLogClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("正在清空所有LOG日志，确认删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                TextLog.Text = "";
        }

        private void 帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox About = new AboutBox();//实例化窗体
            About.ShowDialog();// 将窗体显示出来
        }

        private void 查看原始XML脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rpc = new Form_Rep_reply(Rpc_Reply);
            rpc.Show();
        }

        private void TreeReP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var rpc = new Form_Rep_reply(Rpc_Reply);
            rpc.Show();
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("正在清空所有LOG日志，确认删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                TextLog.Text = "";
        }

        private void 告警抑制屏蔽ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                string object_type = "";
                int id = int.Parse(treeViewNEID.SelectedNode.Name);
                int line = -1;
                for (int i = 0; i < dataGridViewNeInformation.Rows.Count; i++)
                {
                    if (dataGridViewNeInformation.Rows[i].Cells["SSH_ID"].Value.ToString() == id.ToString()) //keyword要查的关键字
                    {
                        line = i;
                        break;
                    }
                    if (line >= 0)
                        break;
                }
                string ip = dataGridViewNeInformation.Rows[line].Cells["网元ip"].Value.ToString();
                foreach (DataGridViewRow row in this.dataGridViewAlarm.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewAlarm.Rows[row.Index].Cells["告警对象名称"].Value.ToString();       //设备IP地址
                        object_type = dataGridViewAlarm.Rows[row.Index].Cells["告警对象类型"].Value.ToString();
                    }
                }
                var Formoam = new Form_alarm_mask_states(id, ip, PtpsFtpsCtps,object_type);
                Formoam.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
