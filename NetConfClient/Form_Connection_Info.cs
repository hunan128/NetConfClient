using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace NetConfClientSoftware
{
    public partial class Form_Connection_Info : Form
    {
        public Form_Connection_Info()
        {
            InitializeComponent();
        }
        int id = 0;
        string ip = "";
        
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public Form_Connection_Info(int _id,string _ip ,string _PtpsFtpsCtps) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            id = _id;
            ip = _ip;
            string[] strArray = _PtpsFtpsCtps.Split(',');
            foreach (var item in strArray)
            {
                if (item != "")
                {
                    ComPtpCtpFtp.Items.Add(item);
                    int index = dataGridViewPtpFtpCtps.Rows.Add();
                    dataGridViewPtpFtpCtps.Rows[index].Cells["业务关联端口"].Value = item;
                    dataGridViewPtpFtpCtps.Rows[index].ReadOnly = true;
                }

            }
            if(strArray!=null)
            ComPtpCtpFtp.SelectedIndex = 0;
           

        }

        private void ComPtpCtpFtp_SelectedIndexChanged(object sender, EventArgs e)
        {
          
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

        private void buttonFind_Click(object sender, EventArgs e)
        {
            GetPtpFtpCtps(ComPtpCtpFtp.Text);
        }

        private void GetPtpFtpCtps(string port) {
            XmlDocument xmlDoc = new XmlDocument();
            if (port.Contains("PTP") && !port.Contains("CTP"))
            {
                xmlDoc.LoadXml(RPC.Send(GetXML.PTP(port), id, ip));
            }
            if (port.Contains("FTP") && !port.Contains("CTP"))
            {
                xmlDoc.LoadXml(RPC.Send(GetXML.FTP(port), id, ip));
            }
            if (port.Contains("CTP"))
            {
                xmlDoc.LoadXml(RPC.Send(GetXML.CTP(port), id, ip));
            }
            if (string.IsNullOrEmpty(port))
            {
                xmlDoc.LoadXml(RPC.Send(GetXML.PtpsFtpsCtps(true, true, true), id, ip));
            }
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
        }
        private void dataGridViewPtpFtpCtps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewPtpFtpCtps.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString();       //设备IP地址
                        GetPtpFtpCtps(PtpsFtpsCtps);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 内环ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewPtpFtpCtps.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString();       //设备IP地址
                        if (string.IsNullOrEmpty(dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString()))
                        {
                            return;
                        }
                        RPC.Send(ModifyXML.Loop_back(PtpsFtpsCtps, "terminal-loopback"), id, ip);
                        dataGridViewPtpFtpCtps.Rows[row.Index].Cells["环回"].Value = "内环(terminal-loopback)";
                        GetPtpFtpCtps(PtpsFtpsCtps);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void 外环ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewPtpFtpCtps.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString();       //设备IP地址
                        if (string.IsNullOrEmpty(dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString()))
                        {
                            return;
                        }
                        RPC.Send(ModifyXML.Loop_back(PtpsFtpsCtps, "facility-loopback"), id, ip);
                        dataGridViewPtpFtpCtps.Rows[row.Index].Cells["环回"].Value = "外环(facility-loopback)";
                        GetPtpFtpCtps(PtpsFtpsCtps);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 不环回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PtpsFtpsCtps = "";
                foreach (DataGridViewRow row in this.dataGridViewPtpFtpCtps.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        PtpsFtpsCtps = dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString();       //设备IP地址
                        if (string.IsNullOrEmpty(dataGridViewPtpFtpCtps.Rows[row.Index].Cells["业务关联端口"].Value.ToString()))
                        {
                            return;
                        }
                        RPC.Send(ModifyXML.Loop_back(PtpsFtpsCtps, "non-loopback"), id, ip);
                        dataGridViewPtpFtpCtps.Rows[row.Index].Cells["环回"].Value = "不环回(non-loopback)";
                        GetPtpFtpCtps(PtpsFtpsCtps);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
