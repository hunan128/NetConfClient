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
    public partial class Form_Rep_reply : Form
    {
        public Form_Rep_reply()
        {
            InitializeComponent();
        }

        
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public Form_Rep_reply(string Rpc_Reply) : this()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Rpc_Reply);
            LoadTreeFromXmlDocument_TreePtpCtpFtp(xmlDoc);
            //textBoxXML.Text = Rpc_Reply;
            
            fastColoredTextBox1.Text = Rpc_Reply;
        }

        private void LoadTreeFromXmlDocument_TreePtpCtpFtp(XmlDocument dom)
        {
            try
            {
                // SECTION 2. Initialize the TreeView control.
                treeViewXML.Nodes.Clear();
                // SECTION 3. Populate the TreeView with the DOM nodes.
                foreach (XmlNode node in dom.ChildNodes)
                {
                    if (node.Name == "namespace" && node.ChildNodes.Count == 0 && string.IsNullOrEmpty(GetAttributeText(node, "name")))
                        continue;
                    AddNode(treeViewXML.Nodes, node);
                }
                treeViewXML.ExpandAll();
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

    }
}
