
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

namespace NetConfClientSoftware
{
    public partial class Main_Form : Form
    {

        NetConfClient netConfClient;
        bool Sub = false;      //订阅开关默认禁止
        public static string defaultfilePath = "";       //打开文件夹默认路径
        public static string FenGeFu = "----------------------------------------------------------------------------";//分隔符
        public string XML_URL = "http://hunan128.com:888/NetconfXML/";   //XML在线文件地址
        public string gpnip = "";//设备IP地址
        public int gpnport = 830;//设备端口
        public string gpnuser = "";//设备用户名
        public string gpnpassword = "";//设备密码
        public string gpnnetconfversion = "";//设备版本

        Thread t ;   //订阅功能线程
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


        DoubleBufferListView listViewAll = new DoubleBufferListView();
        public Main_Form()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 发送XML脚本按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButSend_Click(object sender, EventArgs e)
        {
            Thread send = new Thread(SendNetconfRpc);
            send.Start();

        }


        /// <summary>
        /// 发送订阅消息
        /// </summary>
        private void SendNetconfRpc() {
            try
            {
                if (netConfClient == null) {
                    连接设备ToolStripMenuItem.PerformClick();
                    return;
                }
                if (!netConfClient.IsConnected)
                {
                    断开连接ToolStripMenuItem.PerformClick();
                    连接设备ToolStripMenuItem.PerformClick();
                    return;
                }


                TreeReP.Nodes.Clear();
                string rpcxml = "";
                XmlDocument rpc = new XmlDocument();
                if (!RichTextReq.Text.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + RichTextReq.Text + RpcEnd;
                    if (netConfClient != null)
                    netConfClient.AutomaticMessageIdHandling = true;

                }
                else
                {
                    rpcxml = RichTextReq.Text;
                    if (netConfClient != null)
                        netConfClient.AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                BeginInvoke(new MethodInvoker(delegate (){ LoadTreeFromXmlDocument_TreeReQ(rpc);}));
                //netConfClient.AutomaticMessageIdHandling = false;

                DateTime dTimeEnd = System.DateTime.Now;

                TextLog.AppendText("Rpc本机：0.0.0.0" + " " + System.DateTime.Now.ToString() + "请求：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                RichTextReq.Text = XmlFormat.Xml(rpc.OuterXml);
                DateTime dTimeServer = System.DateTime.Now;
                var rpcResponse = netConfClient.SendReceiveRpc(rpc);

                dTimeServer = System.DateTime.Now;
                TimeSpan ts = dTimeServer - dTimeEnd;
                LabResponsTime.Text =ts.Minutes.ToString() +"min："+ ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                TextLog.AppendText("Rpc服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpcResponse.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                BeginInvoke(new MethodInvoker(delegate () {LoadTreeFromXmlDocument_TreeReP(rpcResponse); }));


            }
            catch (Exception ex)
            {
                TextLog.AppendText("Rpc服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
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
        private void LoginNetconfService(string ip ,int port,string user,string passd) {
            try
            {
                DateTime dTimeEnd = System.DateTime.Now;
                DateTime dTimeServer = System.DateTime.Now;

                netConfClient = new NetConfClient(ip, port, user, passd);
                LabConncet.Text = "连接中";
                连接设备ToolStripMenuItem.Enabled = false;
                netConfClient.Connect();

                if (netConfClient.IsConnected)
                {
                    BeginInvoke(new MethodInvoker(delegate () {
                        LabConncet.Text = "已连接";
                        dTimeServer = System.DateTime.Now;
                        TimeSpan ts = dTimeServer - dTimeEnd;
                        LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                        上载全部XMLToolStripMenuItem.Enabled = true;
                        TextLog.AppendText("Rpc服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                        TextLog.AppendText(XmlFormat.Xml(netConfClient.ServerCapabilities.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                        TextLog.AppendText("Rpc本机：0.0.0.0" + " " + System.DateTime.Now.ToString() + "请求：\r\n" + FenGeFu + "\r\n");
                        TextLog.AppendText(XmlFormat.Xml(netConfClient.ClientCapabilities.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                        netConfClient.OperationTimeout = TimeSpan.FromSeconds(15);
                        netConfClient.TimeOut = int.Parse(ComTimeOut.Text) * 1000;
                        订阅ToolStripMenuItem.Enabled = true;
                        MessageBox.Show(gpnip + "：连接成功！");


                    }));

                }
                else
                {
                    LabConncet.Text = "连接失败";
                    连接设备ToolStripMenuItem.Enabled = true;
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
                连接设备ToolStripMenuItem.Enabled = true;
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

                RichTextReq.Text = XmlFormat.Xml(xmldoc.OuterXml);
                xmldoc.Save(filename);

            }
            else {
                xmldoc.Load(filename);
                RichTextReq.Text = XmlFormat.Xml(xmldoc.OuterXml);
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
                foreach (XmlNode node in dom.DocumentElement.ChildNodes)
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
        private void LoadTreeFromXmlDocument_TreeReQ(XmlDocument dom)
        {
            try
            {
                // SECTION 2. Initialize the TreeView control.
                TreeReq.Nodes.Clear();
                // SECTION 3. Populate the TreeView with the DOM nodes.
                foreach (XmlNode node in dom.DocumentElement.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                        continue;
                    AddNode(TreeReq.Nodes, node);

                }

                TreeReq.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                TreeNode newNode = null;
                XmlNodeList nodeList = inXmlNode.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    XmlNode xNode = inXmlNode.ChildNodes[i];
                    if (!xNode.HasChildNodes)
                    {
                        // If the node has an attribute "name", use that.  Otherwise display the entire text of the node.
                        string value = GetAttributeText(xNode, "name");
                        if (string.IsNullOrEmpty(value))
                            value = (xNode.OuterXml).Trim();
                        //nodes.Remove(newNode);
                        nodes.Add(text + "(" + value + ")");
                    }
                    else {
                        if (newNode == null) {
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
                Sub = false;

                t.Abort();
            }
            catch {

            }
      
        }

        /// <summary>
        /// 订阅在循环等待消息
        /// </summary>
        private void Subscription() {
            if (Sub)
            {
                TextLog.AppendText("Notification服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText("订阅监听已经开启，请关注\r\n");
            }
            string rpcResponse = "" ;
            while (Sub) {
                try
                {
                    XmlDocument notfication = new XmlDocument();
                    rpcResponse = netConfClient.SendReceiveRpcSub();
                    if (rpcResponse != "") {
                        notfication.LoadXml(rpcResponse);
                        TextLog.AppendText("Notification服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                        if (!rpcResponse.Contains("rpc"))
                        {
                            TextLog.AppendText(XmlFormat.Xml(rpcResponse) + "\r\n" + FenGeFu + "\r\n");
                        }
                        BeginInvoke(new MethodInvoker(delegate (){
                            ShowXML(notfication);
                            Pgnot(notfication);
                        }));

                    }

                    Thread.Sleep(100);
                }
                catch {}
            }
            TextLog.AppendText("Notification服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
            TextLog.AppendText("订阅监听已经停止，请关注\r\n");
        }
        /// <summary>
        /// 订阅通知显示
        /// </summary>
        /// <param name="xmlDoc">传入进来的XML文件</param>
        private void ShowXML(XmlDocument xmlDoc) //显示xml数据
        {
            try
            {
                if (!xmlDoc.OuterXml.Contains("notification")) {
                    return;
                }
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
                        int  k= 0;
                        foreach (XmlNode Xn3 in xn11)
                        {
                            XmlNodeList xn12 = Xn3.ChildNodes;

                                for (int i = 0; i < xn12.Count; i++)
                                {
                                    item.SubItems.Add(xn12.Item(i).InnerText);
                                }
                            k++;
                        }
                        if (k == 1 )
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
                listViewAll.BeginUpdate();//数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                listViewAll.Items.Add(item);
                listViewAll.EndUpdate();  //结束数据处理，UI界面一次性绘制。
                string Alarm = "alarm-notification";
                foreach (var type in item.SubItems)
                {
                   if (type.ToString().Contains(Alarm)) {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        if (alarmlog.SubItems[4].Text == "end") {
                            int no = int.Parse(alarmlog.SubItems[0].Text) - 1;
                            for (int i = 0; i < ListViewAlarm.Items.Count; i++)
                            {
                                if (ListViewAlarm.Items[i].SubItems[0].Text == no.ToString())
                                {
                                    ListViewAlarm.Items.RemoveAt(i);
                                    i--;
                                }
                            }
                            alarmlog.SubItems.Add("已清除");
                        }
                        if (alarmlog.SubItems[4].Text == "start")
                        {
                            alarmlog.SubItems.Add("");
                            alarmlog.SubItems.Add("未清除");
                        }
                        alarmlog.UseItemStyleForSubItems = false;
                        switch (alarmlog.SubItems[5].Text)
                        {
                            case "critical":
                                alarmlog.SubItems[5].Text = "紧急"+ alarmlog.SubItems[5].Text;
                                alarmlog.BackColor = Color.OrangeRed;
                                //alarmlog.SubItems[5].BackColor = Color.Red;
                                break;
                            case "major":
                                alarmlog.SubItems[5].Text = "重要"+ alarmlog.SubItems[5].Text;
                                alarmlog.BackColor = Color.Orange;
                                break;
                            case "minor":
                                alarmlog.SubItems[5].Text = "次要"+ alarmlog.SubItems[5].Text;
                                alarmlog.BackColor = Color.Yellow;
                                break;
                            case "warning":
                                alarmlog.SubItems[5].Text = "提示"+ alarmlog.SubItems[5].Text;
                                alarmlog.BackColor = Color.DeepSkyBlue;
                                break;

                        }
                        ListViewAlarm.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, Alarm));
                        mes.Start();
                    }

                }
                string tca = "tca-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(tca))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        if (alarmlog.SubItems[7].Text == "end")
                        {
                            int no = int.Parse(alarmlog.SubItems[0].Text) - 1;
                            for (int i = 0; i < ListViewTcaAlarm.Items.Count; i++)
                            {
                                if (ListViewTcaAlarm.Items[i].SubItems[0].Text == no.ToString())
                                {
                                    ListViewTcaAlarm.Items.RemoveAt(i);
                                    i--;
                                }
                            }
                            alarmlog.SubItems.Add("已清除");
                        }
                        if (alarmlog.SubItems[7].Text == "start")
                        {
                            alarmlog.SubItems.Add("当前告警");
                            alarmlog.SubItems.Add("未清除");
                        }
                        ListViewTcaAlarm.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, tca));
                        mes.Start();
                    }

                }
                string lldp = "lldp-change-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(lldp))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(1);
                        listViewLLDP.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, lldp));
                        mes.Start();
                    }

                }
                string common = "common-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(common))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(1);
                        listViewCommon.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, common));
                        mes.Start();
                    }

                }
                string peer = "peer-change-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(peer))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(1);
                        listViewPeer.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, peer));
                        mes.Start();
                    }

                }
                string attribute = "attribute-value-change-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(attribute))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(1);
                        listViewAttribute.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, attribute));
                        mes.Start();
                    }

                }
                string protection = "protection-switch-notification";
                foreach (var type in item.SubItems)
                {
                    if (type.ToString().Contains(protection))
                    {
                        ListViewItem alarmlog = (ListViewItem)item.Clone();
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(0);
                        alarmlog.SubItems.RemoveAt(1);
                        listViewProtection.Items.Add(alarmlog);
                        Thread mes = new Thread(() => CreatMesg(alarmlog, protection));
                        mes.Start();

                    }


                }
                foreach (ColumnHeader ch in listViewAll.Columns)
                {
                     ch.Width = -1;//根据内容
                    //ch.Width = -2;//根据标题
                }
            }
            catch(Exception ex) {
                TextLog.AppendText(ex.ToString());

            }

        }


        private void CreatMesg(ListViewItem alarmlog,string title) {

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
             Application.DoEvents();

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
        /// 自定义listview控件，防止更新闪烁
        /// </summary>
        public class DoubleBufferListView : ListView
        {
            public DoubleBufferListView()
            {
                SetStyle(ControlStyles.DoubleBuffer |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint, true);
                UpdateStyles();
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Form_Load(object sender, EventArgs e)
        {
            ComTimeOut.SelectedIndex = 0;
            TextOduService_type.SelectedIndex = 0;
            Com_nni_protection_type.SelectedIndex = 0;
            ComOduNniTsDetailClient_UNI_A.SelectedIndex = 0;
            ComOduNniTsDetail_NNI_A.SelectedIndex = 0;
            ComCreatConnection.SelectedIndex = 0;


            ComEthServiceType.SelectedIndex = 0;
            Com_Eth_nni_protection_type.SelectedIndex = 0;
            ComEthVlanAccessAction.SelectedIndex = 0;
            ComEthVlanPriority.SelectedIndex = 0;
            ComEthVlanType.SelectedIndex = 0;
            ComEthServiceMappingMode.SelectedIndex = 0;
            ComEthPrimayTs.SelectedIndex = 0;
            ComEosSdhSignalType.SelectedIndex = 2;
            ComEosSdhSignalTypeProtect.SelectedIndex = 2;
            ComVCType.SelectedIndex = 2;
            ComTSD.SelectedIndex = 0;
            ComLCAS.SelectedIndex = 0;
            ComEthFtpVlanAccess.SelectedIndex = 2;
            ComEthFtpVlanPriority.SelectedIndex = 0;
            ComEthFtpVlanType.SelectedIndex = 1;

            ComSdhSer.SelectedIndex = 0;
            ComSdhPro.SelectedIndex = 7;
            ComSdhUniSdhType.SelectedIndex = 2;
            ComSdhSerMap.SelectedIndex = 0;
            ComSdhNniSdhtype_A.SelectedIndex = 2;
            ComSdhNniVcType_A.SelectedIndex = 2;
            ComSdhNniTs_A.SelectedIndex = 0;
            ComSdhNniSdhtype_B.SelectedIndex = 2;
            ComSdhNniVcType_B.SelectedIndex = 2;
            ComSdhNniTs_B.SelectedIndex = 0;

            Control.CheckForIllegalCrossThreadCalls = false;
            Readini();
            Gpnurlupdate();
            this.listViewAll.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right);
            for (int i = 0; i < 20; i++)
            {
                listViewAll.Columns.Add("类型");
            }

            this.listViewAll.FullRowSelect = true;
            this.listViewAll.GridLines = true;
            this.listViewAll.HideSelection = false;
            this.listViewAll.Location = new System.Drawing.Point(6, 6);
            this.listViewAll.Name = "listViewAll";
             this.listViewAll.Size = new System.Drawing.Size(1251, 499);
            this.listViewAll.TabIndex = 2;
            this.listViewAll.UseCompatibleStateImageBehavior = false;
            this.listViewAll.View = System.Windows.Forms.View.Details;

            tabPageAllNotificontion.Controls.Add(listViewAll);
        }
        /// <summary>
        /// 发送消息等待时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComTimeOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (netConfClient != null) {
                if (netConfClient.IsConnected)
                {
                    if (ComTimeOut.Text == "-1")
                    {
                        netConfClient.TimeOut = -1;

                    }
                    else
                    {
                        netConfClient.TimeOut = int.Parse(ComTimeOut.Text) * 1000;

                    }
                }
            }

        }
        /// <summary>
        /// 按照ctrl+S保存XML脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextReq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                this.保存ToolStripMenuItem.PerformClick();
            }
        }

        /// <summary>
        /// 获取设备上所有的XML文件，最长只等待60秒
        /// </summary>
        private void GetXmlAll() {
            try
            {
                上载全部XMLToolStripMenuItem.Enabled = false;
                XmlDocument doc = new XmlDocument();
                doc = Sendrpc(FindPath.FindGetAll());
                doc.Save(@"C:\netconf\" + gpnip + "_XmlAll.xml");
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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
               // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.ME());
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

                    string layer_protocol_name_eth = layer_protocol_nameeth.InnerText;
                    string layer_protocol_name_sdh = layer_protocol_namesdh.InnerText;
                    string layer_protocol_name_otn = layer_protocol_nameotn.InnerText;
                    string layer_protocol_name_all = layer_protocol_name_eth + layer_protocol_name_sdh + layer_protocol_name_otn;
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-eth:", "");
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-sdh", "");
                    layer_protocol_name_all = layer_protocol_name_all.Replace("acc-otn", "");

                    if (name != null) textBox_me_name.Text = name.InnerText;
                    if (status != null) textBox_me_status.Text = status.InnerText;
                    if (ip_address!= null) textBox_me_ip_address.Text = ip_address.InnerText;
                    if (mask!= null) textBox_me_mask.Text = mask.InnerText;
                    if (ntp_state != null) textBox_me_ntp_state.Text = ntp_state.InnerText;
                    if (ntp_enable != null) textBox_me_ntp_enable.Text = ntp_enable.InnerText;

                    if (gate_way1 != null) textBox_me_gate_way1.Text = gate_way1.InnerText;
                    if (uuid != null) textBox_me_uuid.Text = uuid.InnerText;
                    if (manufacturer != null) textBox_me_manufacturer.Text = manufacturer.InnerText;
                    if (product_name != null) textBox_me_product_name.Text = product_name.InnerText;
                    if (software_version != null) textBox_me_software_version.Text = software_version.InnerText;
                    if (hardware_version != null) textBox_me_hardware_version.Text = hardware_version.InnerText;
                    if (device_type != null) textBox_me_device_type.Text = device_type.InnerText;
                    if (layer_protocol_name_all != null) textBox_me_protocol_name.Text = layer_protocol_name_all;
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



                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
               // Console.Read();

            }
            catch( Exception ex)
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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.EQS());
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("eqsxmlns", "urn:ccsa:yang:acc-devm");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//eqsxmlns:eqs//eqsxmlns:eq", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("eqsxmlns:name", root);
                    XmlNode woking_date = itemNode.SelectSingleNode("eqsxmlns:working-state", root);
                    XmlNode plug_sate = itemNode.SelectSingleNode("eqsxmlns:plug-state", root);
                    XmlNode eq_type = itemNode.SelectSingleNode("eqsxmlns:eq-type[1]", root);
                    XmlNode eq_type2 = itemNode.SelectSingleNode("eqsxmlns:eq-type[2]", root);
                    XmlNode xc_capability = itemNode.SelectSingleNode("eqsxmlns:xc-capability", root);
                    XmlNode software_version = itemNode.SelectSingleNode("eqsxmlns:software-version", root);
                    XmlNode hardware_version = itemNode.SelectSingleNode("eqsxmlns:hardware-version", root);
                    int index = dataGridView_EQ.Rows.Add();
                    dataGridView_EQ.Rows[index].Cells["单板名称"].Value = name.InnerText;
                    dataGridView_EQ.Rows[index].Cells["是否在位"].Value = plug_sate.InnerText;
                    dataGridView_EQ.Rows[index].Cells["状态"].Value = woking_date.InnerText;
                    if (eq_type != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText;
                    if (eq_type2 != null) dataGridView_EQ.Rows[index].Cells["板卡类型"].Value = eq_type.InnerText + "和"  +eq_type2.InnerText;
                    dataGridView_EQ.Rows[index].Cells["XC能力"].Value = xc_capability.InnerText;
                    dataGridView_EQ.Rows[index].Cells["软件版本"].Value = software_version.InnerText;
                    dataGridView_EQ.Rows[index].Cells["硬件版本"].Value = hardware_version.InnerText;
                    dataGridView_EQ.Rows[index].Cells["操作"].Value ="硬复位";


                    XmlNodeList net = itemNode.SelectNodes("eqsxmlns:ptp", root);

                    dataGridView_EQ.Rows[index].Cells["端口数量"].Value = net.Count.ToString();

                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                // Console.Read();

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
                dataGridView_EQ.Rows.Clear();
                ComClientSideNni_UNI_A.Items.Clear();
                ComClientSideNni_UNI_B.Items.Clear();
                ComOduNniPtpName_NNI_A.Items.Clear();
                ComOduNniPtpName_NNI_B.Items.Clear();

                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                //// XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PtpsFtpsCtps(true,false,false));
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                ComClientSideNni_UNI_B.Items.Add("无");
                ComOduNniPtpName_NNI_B.Items.Add("无");

                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);

                    if (layer_protocol_name != null && interface_type!=null) {
                        if (layer_protocol_name.InnerText == "acc-otn:ODU" ) {
                            if (name != null)
                            {
                                ComClientSideNni_UNI_A.Items.Add(name.InnerText);
                                ComClientSideNni_UNI_A.SelectedIndex = 0;
                                ComClientSideNni_UNI_B.Items.Add(name.InnerText);
                                ComClientSideNni_UNI_B.SelectedIndex = 0;
                                ComOduNniPtpName_NNI_A.Items.Add(name.InnerText);
                                ComOduNniPtpName_NNI_A.SelectedIndex = 0;
                                ComOduNniPtpName_NNI_B.Items.Add(name.InnerText);
                                ComOduNniPtpName_NNI_B.SelectedIndex = 0;

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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComClientSideNni_UNI_A.Text));
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
                                if (ComClientSideNni_UNI_A.Text == name.InnerText) {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_UNI_A.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);


                                                ComOduOduSignalType_UNI_A.Items.Add(odu_signal_type1.InnerText);
                                                ComOduOduSignalType_UNI_A.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_UNI_A.Items.Clear();

                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);

                                                ComOduAdapataionType_UNI_A.Items.Add(adaptation_type1.InnerText);
                                                ComOduAdapataionType_UNI_A.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_UNI_A.Items.Clear();

                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComOduSwitchApability_UNI_A.Items.Add(switch_capability1.InnerText);
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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComOduNniPtpName_NNI_A.Text));

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
                                if (ComOduNniPtpName_NNI_A.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComOduOdusignalType_NNI_A.Items.Clear();
                                            ComOduOdusignalType_NNI_B.Items.Clear();

                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                ComOduOdusignalType_NNI_A.Items.Add(odu_signal_type1.InnerText);
                                                ComOduOdusignalType_NNI_A.SelectedIndex = 0;
                                                ComOduOdusignalType_NNI_B.Items.Add(odu_signal_type1.InnerText);
                                                ComOduOdusignalType_NNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapatationType_NNI_A.Items.Clear();
                                            ComOduAdapatationType_NNI_B.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                ComOduAdapatationType_NNI_A.Items.Add(adaptation_type1.InnerText);
                                                ComOduAdapatationType_NNI_A.SelectedIndex = 0;
                                                ComOduAdapatationType_NNI_B.Items.Add(adaptation_type1.InnerText);
                                                ComOduAdapatationType_NNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchCapability_NNI_A.Items.Clear();
                                            ComOduSwitchCapability_NNI_B.Items.Clear();

                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComOduSwitchCapability_NNI_A.Items.Add(switch_capability1.InnerText);
                                                ComOduSwitchCapability_NNI_A.SelectedIndex = 0;
                                                ComOduSwitchCapability_NNI_B.Items.Add(switch_capability1.InnerText);
                                                ComOduSwitchCapability_NNI_B.SelectedIndex = 0;
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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComOduNniPtpName_NNI_B.Text));

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
                                if (ComOduNniPtpName_NNI_B.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComOduOdusignalType_NNI_B.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                ComOduOdusignalType_NNI_B.Items.Add(odu_signal_type1.InnerText);
                                                ComOduOdusignalType_NNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapatationType_NNI_B.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                ComOduAdapatationType_NNI_B.Items.Add(adaptation_type1.InnerText);
                                                ComOduAdapatationType_NNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchCapability_NNI_B.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComOduSwitchCapability_NNI_B.Items.Add(switch_capability1.InnerText);
                                                ComOduSwitchCapability_NNI_B.SelectedIndex = 0;
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
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComClientSideNni_UNI_B.Text));

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

                                if (ComClientSideNni_UNI_B.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComOduOduSignalType_UNI_B.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);


                                                ComOduOduSignalType_UNI_B.Items.Add(odu_signal_type1.InnerText);
                                                ComOduOduSignalType_UNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComOduAdapataionType_UNI_B.Items.Clear();

                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);

                                                ComOduAdapataionType_UNI_B.Items.Add(adaptation_type1.InnerText);
                                                ComOduAdapataionType_UNI_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComOduSwitchApability_UNI_B.Items.Clear();

                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComOduSwitchApability_UNI_B.Items.Add(switch_capability1.InnerText);
                                                ComOduSwitchApability_UNI_B.SelectedIndex = 0;
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
            
            Creat(CreatODU.Common(TextOduLable.Text, TextOduService_type.Text, "ODU", TextOdusize.Text, Com_nni_protection_type.Text,
                    ComClientSideNni_UNI_A.Text, TSConversion.Ts(ComOduOduSignalType_UNI_A.Text, ComOduSwitchApability_UNI_A.Text,ComOduNniTsDetailClient_UNI_A.Text),ComOduAdapataionType_UNI_A.Text, ComOduOduSignalType_UNI_A.Text, ComOduSwitchApability_UNI_A.Text,
    ComOduNniPtpName_NNI_A.Text, TSConversion.Ts(ComOduOdusignalType_NNI_A.Text, ComOduSwitchCapability_NNI_A.Text, ComOduNniTsDetail_NNI_A.Text), ComOduAdapatationType_NNI_A.Text, ComOduOdusignalType_NNI_A.Text, ComOduSwitchCapability_NNI_A.Text,
        ComOduNniPtpName_NNI_B.Text, TSConversion.Ts(ComOduOdusignalType_NNI_B.Text, ComOduSwitchCapability_NNI_B.Text, ComOduNniTsDetail_NNI_B.Text) , ComOduAdapatationType_NNI_B.Text, ComOduOdusignalType_NNI_B.Text, ComOduSwitchCapability_NNI_B.Text


    ));
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
                        allconnection = allconnection + "\r\n" + connection;
                    }
                }
                if (MessageBox.Show("正在删除当前业务:\r\n" + allconnection + "\r\n是否删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    foreach (DataGridViewRow row in this.dataGridViewEth.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            connection = dataGridViewEth.Rows[row.Index].Cells["连接名称"].Value.ToString();
                            var doc = Sendrpc(DeleteODU.Delete(connection));//设备IP地址
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
                    MessageBox.Show(allconnection+"\r\n已成功删除，重新点击在线查询即可更新。");

                }
                // 保存在实体类属性中
                //保存密码选中状态


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// RPC发送消息函数
        /// </summary>
        /// <param name="rpc">发送的脚本</param>
        /// <returns></returns>
        private  XmlDocument Sendrpc(XmlDocument rpc) {
            XmlDocument rpcc = new XmlDocument();
            try
            {
                //TreeReP.Nodes.Clear();
                if (netConfClient == null)
                {
                    连接设备ToolStripMenuItem.PerformClick();
                    return rpcc;
                }
                if (!netConfClient.IsConnected)
                {
                    断开连接ToolStripMenuItem.PerformClick();
                    连接设备ToolStripMenuItem.PerformClick();
                    return rpcc;
                }

                string rpcxml = "";
                if (!rpc.OuterXml.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + rpc.OuterXml + RpcEnd;
                    netConfClient.AutomaticMessageIdHandling = true;

                }
                else
                {
                    rpcxml = rpc.OuterXml;
                    netConfClient.AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                DateTime dTimeEnd = System.DateTime.Now;

                TextLog.AppendText("Rpc本机：0.0.0.0" + " " + System.DateTime.Now.ToString() + "请求：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + "\r\n" + FenGeFu + "\r\n");
               // RichTextReq.Text = sb.ToString();
                DateTime dTimeServer = System.DateTime.Now;
                
                var rpcResponse = netConfClient.SendReceiveRpc(rpc);

                dTimeServer = System.DateTime.Now;
                TimeSpan ts = dTimeServer - dTimeEnd;
                LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                TextLog.AppendText("Rpc服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpcResponse.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                //BeginInvoke(new MethodInvoker(delegate () { LoadTreeFromXmlDocument_TreeReP(rpcResponse); }));
                if (rpcResponse.OuterXml.Contains("error-type"))
                {
                   MessageBox.Show("运行失败：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                }
                else
                {
                    //MessageBox.Show("运行成功：\r\n" + sb.ToString());
                    rpcc.LoadXml(XmlFormat.Xml(rpcResponse.OuterXml));
                }

            }
            catch (Exception ex)
            {
                TextLog.AppendText("Rpc服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(ex.Message + "\r\n");
                MessageBox.Show("运行失败！原因如下：\r\n" + ex.ToString());
            }
            return rpcc;

        }

        /// <summary>
        /// 脚本业务的RPC函数，带返回提示
        /// </summary>
        /// <param name="rpc">发送的脚本XML</param>
        private void Creat(XmlDocument rpc)
        {
            try
            {
                if (netConfClient == null)
                {
                    连接设备ToolStripMenuItem.PerformClick();
                    return;
                }
                if (!netConfClient.IsConnected)
                {
                    断开连接ToolStripMenuItem.PerformClick();
                    连接设备ToolStripMenuItem.PerformClick();
                    return;
                }

                //TreeReP.Nodes.Clear();
                string rpcxml = "";
                if (!rpc.OuterXml.Contains("rpc"))
                {
                    string RpcTop = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"4\">";
                    string RpcEnd = "</rpc>";
                    rpcxml = RpcTop + rpc.OuterXml + RpcEnd;
                    netConfClient.AutomaticMessageIdHandling = true;

                }
                else
                {
                    rpcxml = rpc.OuterXml;
                    netConfClient.AutomaticMessageIdHandling = false;
                }
                rpc.LoadXml(rpcxml);
                DateTime dTimeEnd = System.DateTime.Now;

                TextLog.AppendText("Rpc本机：0.0.0.0" + " " + System.DateTime.Now.ToString() + "请求：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpc.OuterXml) + "\r\n" + FenGeFu + "\r\n");
                // RichTextReq.Text = sb.ToString();
                DateTime dTimeServer = System.DateTime.Now;
                var rpcResponse = netConfClient.SendReceiveRpc(rpc);

                dTimeServer = System.DateTime.Now;
                TimeSpan ts = dTimeServer - dTimeEnd;
                LabResponsTime.Text = ts.Minutes.ToString() + "min：" + ts.Seconds.ToString() + "s：" + ts.Milliseconds.ToString() + "ms";
                TextLog.AppendText("Rpc服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(XmlFormat.Xml(rpcResponse.OuterXml) + "\r\n" + FenGeFu + "\r\n");
               // BeginInvoke(new MethodInvoker(delegate () { LoadTreeFromXmlDocument_TreeReP(rpcResponse); }));
                if (rpcResponse.OuterXml.Contains("error"))
                {
                    MessageBox.Show("运行失败：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                }
                else
                {
                    MessageBox.Show("运行成功：\r\n" + XmlFormat.Xml(rpcResponse.OuterXml));
                }

            }
            catch (Exception ex)
            {
                TextLog.AppendText("Rpc服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText(ex.Message + "\r\n");
                MessageBox.Show("运行失败！原因如下：\r\n" + ex.ToString());
            }

        }

        private void ButFindEoO_Click(object sender, EventArgs e)
        {
            try
            {
                ComEthUniPtpName.Items.Clear();
                ComEthPrimayNniPtpName.Items.Clear();
                ComEthSecNniPtpName.Items.Clear();

                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                //// XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PtpsFtpsCtps(true, false, false));
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                ComEthSecNniPtpName.Items.Add("无");

                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);
                    XmlNode layer_protocol_name = itemNode.SelectSingleNode("ptpsxmlns:layer-protocol-name", root);
                    XmlNode interface_type = itemNode.SelectSingleNode("ptpsxmlns:interface-type", root);

                    if (layer_protocol_name != null && interface_type != null)
                    {
                        if (layer_protocol_name.InnerText == "acc-eth:ETH")
                        {
                            if (name != null)
                            {
                                ComEthUniPtpName.Items.Add(name.InnerText);
                                ComEthUniPtpName.SelectedIndex = 0;

                            }

                        }
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                ComEthPrimayNniPtpName.Items.Add(name.InnerText);
                                ComEthPrimayNniPtpName.SelectedIndex = 0;
                                ComEthSecNniPtpName.Items.Add(name.InnerText);
                                ComEthSecNniPtpName.SelectedIndex = 0;
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

        private void ComEthPrimayNniPtpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComEthPrimayNniPtpName.Text));
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
                                if (ComEthPrimayNniPtpName.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComEthPrimayOduType.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);


                                                ComEthPrimayOduType.Items.Add(odu_signal_type1.InnerText);
                                                ComEthPrimayOduType.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComEthPrimayAdaType.Items.Clear();

                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);

                                                ComEthPrimayAdaType.Items.Add(adaptation_type1.InnerText);
                                                ComEthPrimayAdaType.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComEthPrimarySwitch.Items.Clear();

                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComEthPrimarySwitch.Items.Add(switch_capability1.InnerText);
                                                ComEthPrimarySwitch.SelectedIndex = 0;
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

        private void ComEthSecNniPtpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComEthSecNniPtpName.Text));

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
                                if (ComEthSecNniPtpName.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComEthSecOduType.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);


                                                ComEthSecOduType.Items.Add(odu_signal_type1.InnerText);
                                                ComEthSecOduType.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComEthSecAdaType.Items.Clear();

                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);

                                                ComEthSecAdaType.Items.Add(adaptation_type1.InnerText);
                                                ComEthSecAdaType.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComEthSecSwitch.Items.Clear();

                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComEthSecSwitch.Items.Add(switch_capability1.InnerText);
                                                ComEthSecSwitch.SelectedIndex = 0;
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

        private void ButCreatEoO_Click(object sender, EventArgs e)
        {
            Creat(CreatETH.Common(ComCreatConnection.Text, TextEthLabel.Text, ComEthServiceType.Text, "ETH", TextEthCir.Text, TextEthPir.Text, TextEthCbs.Text, TextEthPbs.Text, Com_Eth_nni_protection_type.Text, ComEthServiceMappingMode.Text,
                    ComEthUniPtpName.Text, ComEthClientVlanId.Text, ComEthVlanPriority.Text, ComEthVlanAccessAction.Text, ComEthVlanType.Text, ComEthUniVlanId.Text, ComEthUniVlanPriority.Text, ComEthUniVlanAccessAction.Text, ComEthUniVlanType.Text,
    ComEthPrimayNniPtpName.Text, TSConversion.Ts(ComEthPrimayOduType.Text, ComEthPrimarySwitch.Text, ComEthPrimayTs.Text), ComEthPrimayAdaType.Text, ComEthPrimayOduType.Text, ComEthPrimarySwitch.Text, ComEthFtpVlanID.Text, ComEthFtpVlanPriority.Text, ComEthFtpVlanAccess.Text, ComEthFtpVlanType.Text,
        ComEthSecNniPtpName.Text, TSConversion.Ts(ComEthSecOduType.Text, ComEthSecSwitch.Text, ComEthSecNniTs.Text), ComEthSecAdaType.Text, ComEthSecOduType.Text, ComEthSecSwitch.Text,
        ComEosSdhSignalType.Text, ComVCType.Text, TextMappingPath.Text, ComEosSdhSignalTypeProtect.Text, TextMappingPathProtect.Text, ComLCAS.Text, ComHoldOff.Text, ComWTR.Text, ComTSD.Text

    ));
        }

        private void ButFindEth_online_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridViewEth.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = FindPath.Find();
                var xmlDoc = Sendrpc(xmlDoc1);
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
                            CTPAll = CTPAll+"\r\n" + _ctp.InnerText;
                            dataGridViewEth.Rows[index].Cells["CTP端口2"].Value = _ctp.InnerText;

                        }
                        dataGridViewEth.Rows[index].Cells["CTP端口1"].Value = CTPAll;

                    }





                    //XmlNode product-name = itemNode.SelectSingleNode("//me:name", root);
                    //XmlNode software-version = itemNode.SelectSingleNode("//me:status", root);
                    //if ((titleNode != null) && (dateNode != null))
                    //    System.Diagnostics.Debug.WriteLine(dateNode.InnerText + ": " + titleNode.InnerText);
                }
                // Console.Read();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void Gpnurlupdate()
        {
            try
            {
                string strCode;
                ArrayList alLinks;
                if (XML_URL == "")
                {
                    MessageBox.Show("请输入网址");
                    return;
                }
                string strURL = XML_URL;
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
                NetConfXml.WriteToXml(strURL, alLinks);
                //读取设定档百
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"C:\netconf\HyperLinks.xml");
                //取得节点专
                XmlNodeList node = xmlDoc.GetElementsByTagName("other");
                for (int i = 0; i < node.Count; i++)
                {
                    if (!ComXml.Items.Contains(node[i].InnerText)) {
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

        private void ButPerformanceFind_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridViewCurrentPerformance.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = FindPath.FindPerformances();
                var xmlDoc = Sendrpc(xmlDoc1);
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
                    LabPerCount.Text = (index+1).ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   
            }
        }

        private void ButCurPerFindPort_Click(object sender, EventArgs e)
        {
            try
            {

                ComCurPerObjectName.Items.Clear();

                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PtpsFtpsCtps(true, true, true));
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
              //  root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp|//ptpsxmlns:ftps//ptpsxmlns:ftp|//ptpsxmlns:ctps//ptpsxmlns:ctp", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    XmlNode name = itemNode.SelectSingleNode("ptpsxmlns:name", root);

                    if (name != null )
                    {
                        ComCurPerObjectName.Items.Add(name.InnerText);
                        ComCurPerObjectName.SelectedIndex = 0;
                    }
                }
                // Console.Read();

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
                xmlDoc1 = FindPath.FindPerformance(ComCurPerObjectName.Text,ComCurPerGranularity.Text);
                var xmlDoc = Sendrpc(xmlDoc1);
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
                    LabPerCount.Text = (index+1).ToString();

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
                xmlDoc1 = FindPath.FindHisPerformance(dateTimePickerStartime.Text, dateTimePickerEndtime.Text,ComCurPerGranularity.Text, ComCurPerObjectName.Text,"");
                var xmlDoc = Sendrpc(xmlDoc1);
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
                    LabPerCount.Text = (index+1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox About = new AboutBox();//实例化窗体
            About.ShowDialog();// 将窗体显示出来
        }


        private void ButVc_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new FormInfo(ComVCType.Text);
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
            var formInfo = new FormInfo(ComVCType.Text);
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
                ComSdhNniPtp_A.Items.Clear();
                ComSdhNniPtp_B.Items.Clear();

                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PtpsFtpsCtps(true, false, false));
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("ptpsxmlns", "urn:ccsa:yang:acc-devm");
                root.AddNamespace("oduxmlns", "urn:ccsa:yang:acc-otn");

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//ptpsxmlns:ptps//ptpsxmlns:ptp", root);
                ComSdhNniPtp_B.Items.Add("无");

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
                            }

                        }
                        if (layer_protocol_name.InnerText == "acc-otn:ODU")
                        {
                            if (name != null)
                            {
                                ComSdhNniPtp_A.Items.Add(name.InnerText);
                                ComSdhNniPtp_A.SelectedIndex = 0;
                                ComSdhNniPtp_B.Items.Add(name.InnerText);
                                ComSdhNniPtp_B.SelectedIndex = 0;
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

        private void ComSdhNniPtp_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComSdhNniPtp_A.Text));
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
                                if (ComSdhNniPtp_A.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComSdhNniOdu_A.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                ComSdhNniOdu_A.Items.Add(odu_signal_type1.InnerText);
                                                ComSdhNniOdu_A.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComSdhNniAda_A.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                ComSdhNniAda_A.Items.Add(adaptation_type1.InnerText);
                                                ComSdhNniAda_A.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComSdhNniSwitch_A.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComSdhNniSwitch_A.Items.Add(switch_capability1.InnerText);
                                                ComSdhNniSwitch_A.SelectedIndex = 0;
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

        private void ComSdhNniPtp_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filename);
                xmlDoc = Sendrpc(FindPath.PTP(ComSdhNniPtp_B.Text));
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
                                if (ComSdhNniPtp_B.Text == name.InnerText)
                                {
                                    XmlNodeList itemNodesOduPtpPac = itemNode.SelectNodes("oduxmlns:odu-ptp-pac", root);
                                    foreach (XmlNode itemNodeOdu in itemNodesOduPtpPac)
                                    {
                                        XmlNode odu_capacity = itemNodeOdu.SelectSingleNode("oduxmlns:odu-capacity", root);
                                        XmlNodeList odu_signal_type = itemNodeOdu.SelectNodes("oduxmlns:odu-signal-type", root);
                                        XmlNodeList adaptation_type = itemNodeOdu.SelectNodes("oduxmlns:adaptation-type", root);
                                        XmlNodeList switch_capability = itemNodeOdu.SelectNodes("oduxmlns:switch-capability", root);

                                        if (odu_signal_type != null)
                                        {
                                            ComSdhNniOdu_B.Items.Clear();
                                            for (int i = 1; i <= odu_signal_type.Count; i++)
                                            {
                                                XmlNode odu_signal_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:odu-signal-type[" + i + "]", root);
                                                ComSdhNniOdu_B.Items.Add(odu_signal_type1.InnerText);
                                                ComSdhNniOdu_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (adaptation_type != null)
                                        {
                                            ComSdhNniAda_B.Items.Clear();
                                            for (int i = 1; i <= adaptation_type.Count; i++)
                                            {
                                                XmlNode adaptation_type1 = itemNodeOdu.SelectSingleNode("oduxmlns:adaptation-type[" + i + "]", root);
                                                ComSdhNniAda_B.Items.Add(adaptation_type1.InnerText);
                                                ComSdhNniAda_B.SelectedIndex = 0;
                                            }
                                        }
                                        if (switch_capability != null)
                                        {
                                            ComSdhNniSwitch_B.Items.Clear();
                                            for (int i = 1; i <= switch_capability.Count; i++)
                                            {
                                                XmlNode switch_capability1 = itemNodeOdu.SelectSingleNode("oduxmlns:switch-capability[" + i + "]", root);
                                                ComSdhNniSwitch_B.Items.Add(switch_capability1.InnerText);
                                                ComSdhNniSwitch_B.SelectedIndex = 0;
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

        private void ButSdhUniTs_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new FormInfo(ComSdhUniSdhType.Text);
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
            var formInfo = new FormInfo(ComSdhNniVcType_A.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_A.Text = formInfo.Information;
            }
        }

        private void ButSdhNniTs_B_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var formInfo = new FormInfo(ComSdhNniVcType_B.Text);
            // 以对话框方式显示FormInfo  
            if (formInfo.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                TextSdhNniTs_B.Text = formInfo.Information;
            }
        }

        private void ButCreatSDH_Click(object sender, EventArgs e)
        {

            Creat(CreateSDH.Common(TextSdhlabel.Text, ComSdhSer.Text, "SDH", TextSdhTotal.Text, ComSdhPro.Text, ComSdhSerMap.Text,
                    ComSdhUniPtp.Text, ComSdhUniSdhType.Text, TextSdhUniTs.Text,
    ComSdhNniPtp_A.Text, TSConversion.Ts(ComSdhNniOdu_A.Text, ComSdhNniSwitch_A.Text, ComSdhNniTs_A.Text), ComSdhNniAda_A.Text, ComSdhNniOdu_A.Text, ComSdhNniSwitch_A.Text, ComSdhNniSdhtype_A.Text, ComSdhNniVcType_A.Text, TextSdhNniTs_A.Text,
        ComSdhNniPtp_B.Text, TSConversion.Ts(ComSdhNniOdu_B.Text, ComSdhNniSwitch_B.Text, ComSdhNniTs_B.Text), ComSdhNniAda_B.Text, ComSdhNniOdu_B.Text, ComSdhNniSwitch_B.Text, ComSdhNniSdhtype_B.Text, ComSdhNniVcType_B.Text, TextSdhNniTs_B.Text

    ));

               
        }

        private void ButPGSFind_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridViewPGS.Rows.Clear();
                // string filename = @"C:\netconf\" + gpnip + "_XmlAll.xml";
                // XPathDocument doc = new XPathDocument(@"C:\netconf\" + gpnip + "_XmlAll.xml");
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1 = FindPath.FindPgs();
                var xmlDoc = Sendrpc(xmlDoc1);
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("pgsxmlns", "urn:ccsa:yang:acc-protection-group");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//pgsxmlns:pgs//pgsxmlns:pg", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewPGS.Rows.Add();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }
        private void Pgnot(XmlDocument xmlDoc) {
            try
            {
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("pgsxmlns", "urn:ccsa:yang:acc-protection-group");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//pgsxmlns:pg", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewPGS_Not.Rows.Add();

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

                    if (pg_id != null) { dataGridViewPGS_Not.Rows[index].Cells["保护组IDN"].Value = pg_id.InnerText; }
                    if (protection_type != null) { dataGridViewPGS_Not.Rows[index].Cells["保护类型N"].Value = protection_type.InnerText; }
                    if (reversion_mode != null) { dataGridViewPGS_Not.Rows[index].Cells["还原模式N"].Value = reversion_mode.InnerText; }
                    if (switch_direction != null) { dataGridViewPGS_Not.Rows[index].Cells["开关方向N"].Value = switch_direction.InnerText; }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }

        }


        private void Pgcommand(string _command, string _ditection) {

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
                            var doc = Sendrpc(PG.Command(pg_id, _command, _ditection));//设备IP地址
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
            Pgcommand("force-switch", "to-primary");
        }

        private void 强制倒换到备用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("force-switch", "to-secondary");
        }

        private void 人工倒换到主用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-primary");

        }

        private void 人工倒换到备用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("manual-switch", "to-secondary");
        }

        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pgcommand("cleared","to-secondary");
        }

        private void 锁定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
         Pgcommand("lockout", "to-secondary");

        }

        private void 订阅ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string subscription = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n" +
    "<rpc xmlns=\"urn:ietf:params:xml:ns:netconf:base:1.0\" message-id=\"7\" >" + "\r\n" +
    "<create-subscription xmlns=\"urn:ietf:params:xml:ns:netconf:notification:1.0\" />" + "\r\n" +
    "</rpc > ";
            var sub = netConfClient.SendReceiveRpc(subscription);
            TextLog.AppendText("Rpc服务器：" + netConfClient.ConnectionInfo.Host + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
            TextLog.AppendText(sub.OuterXml + "\r\n" + FenGeFu + "\r\n");
            Sub = true;
            if (t != null) {
                t.Abort();
            }
            t = new Thread(Subscription);
            t.Start();
            订阅ToolStripMenuItem.Enabled = false;
            订阅监听禁止ToolStripMenuItem.Text = "订阅监听禁止";
            订阅监听禁止ToolStripMenuItem.Enabled = true;
            TextSub.Text = "已订阅";
        }

        private void 连接设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 实例化FormInfo，并传入待修改初值  
            var LoginOn = new LoginOn(gpnip,830,gpnuser,gpnpassword,gpnnetconfversion);
            // 以对话框方式显示FormInfo  
            if (LoginOn.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示  
                gpnip = LoginOn.IP;
                gpnport = LoginOn.PORT;
                gpnuser = LoginOn.USER;
                gpnpassword = LoginOn.PASSD;
                gpnnetconfversion = LoginOn.VER;
                Gpnsetini();
                TextIP.Text = gpnip;
                Thread thread = new Thread(() => LoginNetconfService(LoginOn.IP, LoginOn.PORT, LoginOn.USER, LoginOn.PASSD));
                thread.Start();


            }



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
                doc.LoadXml(RichTextReq.Text);
                doc.Save(defaultfilePath + ComXml.Text);

            }
        }

        private void 断开连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                订阅ToolStripMenuItem.Enabled = false;
                订阅监听禁止ToolStripMenuItem.Enabled = false;
                LabConncet.Text = "连接断开";
                上载全部XMLToolStripMenuItem.Enabled = false;
                TextSub.Text = "未订阅";

                if (订阅监听禁止ToolStripMenuItem.Text == "订阅监听禁止")
                {
                    Sub = false;
                    netConfClient.SendReceiveRpcKeepLive();

                }

                netConfClient.Disconnect();

            }

            catch (Exception ex)
            {
                TextLog.AppendText(ex.Message + "\r\n");
            }

            连接设备ToolStripMenuItem.Enabled = true;
        }

        private void ButLogin_Click(object sender, EventArgs e)
        {

        }

        private void 订阅监听禁止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (订阅监听禁止ToolStripMenuItem.Text == "订阅监听禁止")
            {
                Sub = false;
                netConfClient.SendReceiveRpcKeepLive();
                订阅监听禁止ToolStripMenuItem.Text = "订阅监听使能";
                TextLog.AppendText("服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText("订阅监听已经停止，请关注\r\n");
                TextSub.Text = "未订阅";


            }
            else
            {
                Sub = true;
                if (t != null) {
                    t.Abort();
                }
                t = new Thread(Subscription);
                t.Start();
                订阅ToolStripMenuItem.Enabled = false;
                订阅监听禁止ToolStripMenuItem.Text = "订阅监听禁止";
                TextLog.AppendText("服务器：" + " " + System.DateTime.Now.ToString() + "应答：\r\n" + FenGeFu + "\r\n");
                TextLog.AppendText("订阅监听已经开启，请关注\r\n");
                TextSub.Text = "已订阅";


            }
        }

        private void 上载全部XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread getxmlall = new Thread(GetXmlAll);
            MessageBox.Show("请耐心等待15-60秒后，方可执行其他操作！\r\n如果60秒没有提示说明请求超时！");
            getxmlall.Start();
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
            else {
                e.Graphics.DrawString(e.Node.Text, this.Font, Brushes.Magenta, e.Bounds.X, e.Bounds.Y);
            }

        }

        private void TreeReq_DrawNode(object sender, DrawTreeNodeEventArgs e)
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
    }
}
