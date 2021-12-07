using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace NetConfClientSoftware
{
    public partial class Form_AutoXmlInfo : Form
    {
        public Form_AutoXmlInfo()
        {
            InitializeComponent();
        }
        public static string RxXml = "";
        public Form_AutoXmlInfo(string ip,string Model, string Title, string IPS, string RPC,string ExpType, string Exp, string Rx, string Reply, string StartTime, string StopTime, string Dtime, string Recommend) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = ip;
            if (true)
            {
                textBoxModel.Text = Model;
                textBoxtitle.Text = Title;
                textBoxips.Text = IPS;
                textBoxstarttime.Text = StartTime;
                textBoxstoptime.Text = StopTime;
                textBoxtime.Text = Dtime;
                textBoxRecommod.Text = Recommend;
                textBoxtype.Text = ExpType;
                richTextBoxRpc.Text = RPC;
                richTextBoxReply.Text = Reply;
                RxXml = Reply;
                string[] ExpByte = Exp.Split(',');
                string[] RxByte = Rx.Split('|');

                for (int i = 0; i < ExpByte.Length; i++)
                {
                    int index = dataGridViewExpRx.Rows.Add();
                    //dataGridViewExpRx.Rows[index].Cells["预期"].Value = ExpByte[i];
                   
                   // string[] RxByte0 = RxByte[i].Split('=');
                    string[] RxByte0 = Regex.Split(RxByte[i], "是", RegexOptions.IgnoreCase);
                    dataGridViewExpRx.Rows[index].Cells["预期"].Value = RxByte0[0];
                    dataGridViewExpRx.Rows[index].Cells["结果"].Value = RxByte0[1];
                    if (RxByte0[0].Contains("枚举")) {
                        dataGridViewExpRx.Rows[i].Cells["预期"].Style.ForeColor = Color.Red;
                        dataGridViewExpRx.Rows[i].Cells["结果"].Style.ForeColor = Color.Red;
                    }
                        
                    if (RxByte[i].Contains("NOK")) {
                        dataGridViewExpRx.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        dataGridViewExpRx.Rows[i].Cells["结果"].Value = "匹配节点名称:"+ RxByte0[1];

                    }
                    else
                    {
                        dataGridViewExpRx.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                    }

                }


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
                treeView.Nodes.Clear();
                // SECTION 3. Populate the TreeView with the DOM nodes.
                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                        continue;
                    AddNode(treeView.Nodes, node);

                }

                treeView.ExpandAll();
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
                for (int i = 0; i < inXmlNode.Attributes.Count; i++)
                {
                    text = text + " " + inXmlNode.Attributes[i].OuterXml;

                }
                TreeNode newNode = null;
                XmlNodeList nodeList = inXmlNode.ChildNodes;
                for (int i = 0; i < nodeList.Count; i++)
                {
                   
                    XmlNode xNode = inXmlNode.ChildNodes[i];
                    if (!xNode.HasChildNodes)
                    {
                        // If the node has an attribute "name", use that.  Otherwise display the entire text of the node.
                       
                        string value = GetAttributeText(xNode, "name");
                        if (string.IsNullOrEmpty(value)) {
                            value = xNode.Value;
                            if (string.IsNullOrEmpty(value))
                            {
                                value = xNode.Name;
                                if (i == 0)
                                {
                                    TreeNode textnode = new TreeNode();
                                    textnode.Text = text;
                                    textnode.Nodes.Add(value);
                                    nodes.Add(textnode);
                                }
                            }
                            else {
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
        }

        private void Form_AutoXmlInfo_Load(object sender, EventArgs e)
        {
            try {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(RxXml);
                LoadTreeFromXmlDocument_TreeReQ(dom);
            }catch(Exception ex){
                MessageBox.Show(ex.ToString());
            }


        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
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
