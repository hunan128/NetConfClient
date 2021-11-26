using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NetConfClientSoftware
{
    class LocalConnectionXml
    {
        public void CreatXmlTree(string xmlPath,string ip,int port, string user, string password,int id,string name,string ips)
        {
            if (string.IsNullOrEmpty(name)) {
                name = ip;
            }
            XElement xElement = new XElement(
                  new XElement("neinfo",
                    new XElement("ipaddress", new XAttribute("id",id),
                        new XElement("ip", ip),
                        new XElement("port", port),
                        new XElement("id", id),
                        new XElement("user", user),
                        new XElement("password", password),
                        new XElement("name", name),
                        new XElement("ips", ips) ) 
                        )
                  );

            //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xw = XmlWriter.Create(xmlPath, settings);
            xElement.Save(xw);
            //写入文件
            xw.Flush();
            xw.Close();
        }
        public void Add(string xmlPath, string ip, int port, string user, string password, int id, string name, string ips)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            var root = xmlDoc.DocumentElement;//取到根结点
            XmlNode newNode = xmlDoc.CreateNode("element", "ipaddress","");
            XmlAttribute attr = null;
            attr = xmlDoc.CreateAttribute("id");
            attr.Value = id.ToString();
            newNode.Attributes.SetNamedItem(attr);
            root.AppendChild(newNode);
            XmlNode newNodeip = xmlDoc.CreateNode("element", "ip", "");
            newNodeip.InnerText = ip;
            newNode.AppendChild(newNodeip);
            XmlNode newNodeport = xmlDoc.CreateNode("element", "port", "");
            newNodeport.InnerText = port.ToString() ;
            newNode.AppendChild(newNodeport);
            XmlNode newNodeuser = xmlDoc.CreateNode("element", "user", "");
            newNodeuser.InnerText = user;
            newNode.AppendChild(newNodeuser);
            XmlNode newNodepassword = xmlDoc.CreateNode("element", "password", "");
            newNodepassword.InnerText = password;
            newNode.AppendChild(newNodepassword);
            XmlNode newNodeid = xmlDoc.CreateNode("element", "id", "");
            newNodeid.InnerText = id.ToString();
            newNode.AppendChild(newNodeid);
            XmlNode newNodename = xmlDoc.CreateNode("element", "name", "");
            if (string.IsNullOrEmpty(name))
            {
                newNodename.InnerText = ip;
            }
            else {
                newNodename.InnerText = name;
            }
            
            newNode.AppendChild(newNodename);
            XmlNode newNodeips = xmlDoc.CreateNode("element", "ips", "");
            newNodeips.InnerText = ips;
            newNode.AppendChild(newNodeips);
            //添加为根元素的第一层子结点
            xmlDoc.Save(xmlPath);
        }
        public void Delete(string xmlPath,int id)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            var root = xmlDoc.DocumentElement;//取到根结点

            //var element = xmlDoc.SelectSingleNode("neinfo/ipaddress");
            //root.RemoveChild(element);
            //xmlDoc.Save(xmlPath);


            XmlNodeList xnl = xmlDoc.SelectSingleNode("neinfo").ChildNodes;


            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("id") == id.ToString())
                {
                    xe.RemoveAll();//删除该节点的全部内容
                    root.RemoveChild(xe);
                }
  
            }
            xmlDoc.Save(xmlPath);
        }
        public void Modify(string xmlPath, string ip, int port, string user, string password, int id, string name, string ips)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlElement element = (XmlElement)xmlDoc.SelectSingleNode("neinfo/ipaddress/name");
            element.InnerText = name;
            xmlDoc.Save(xmlPath);
        }

    }
}
