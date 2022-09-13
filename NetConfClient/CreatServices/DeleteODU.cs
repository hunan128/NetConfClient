using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class DeleteODU
    {
        public static XmlDocument Delete(string connection_name) {

            XmlDocument commonXml = new XmlDocument();
            //  创建XML文档，存在就删除再生成
            XmlDeclaration dec = commonXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            commonXml.AppendChild(dec);
            //  创建根结点
            XmlElement rpc = commonXml.CreateElement("rpc");

            rpc.SetAttribute("message-id", "1");
            rpc.SetAttribute("xmlns", "urn:ietf:params:xml:ns:netconf:base:1.0");
            commonXml.AppendChild(rpc);

            //创建信息节点
            XmlElement edit_config = commonXml.CreateElement("edit-config");
            rpc.AppendChild(edit_config);
            //创建tatget
            XmlElement target = commonXml.CreateElement("target");
            edit_config.AppendChild(target);
            //创建running
            XmlElement running = commonXml.CreateElement("running");
            target.AppendChild(running);

            //连接删除
            XmlElement config = commonXml.CreateElement("config");
            config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //配置connections
            XmlElement connections = commonXml.CreateElement("connections");
            connections.SetAttribute("xmlns", "urn:ccsa:yang:acc-connection");
            config.AppendChild(connections);

            //配置connection
            XmlElement connection = commonXml.CreateElement("connection");
            XmlAttribute a = commonXml.CreateAttribute("nc", "operation", "urn:ietf:params:xml:ns:netconf:base:1.0");
            a.Value = "remove";
            connection.Attributes.Append(a);
            //connection.SetAttribute("nc:operation", "remove");
            connections.AppendChild(connection);


            //删除名称
            XmlElement name = commonXml.CreateElement("name");
            name.InnerText = connection_name;
            connection.AppendChild(name);

            return commonXml;

        }


        public static XmlDocument Pg(string _pg_id)
        {

            XmlDocument commonXml = new XmlDocument();
            //  创建XML文档，存在就删除再生成
            XmlDeclaration dec = commonXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            commonXml.AppendChild(dec);
            //  创建根结点
            XmlElement rpc = commonXml.CreateElement("rpc");

            rpc.SetAttribute("message-id", "1");
            rpc.SetAttribute("xmlns", "urn:ietf:params:xml:ns:netconf:base:1.0");
            commonXml.AppendChild(rpc);

            //创建信息节点
            XmlElement edit_config = commonXml.CreateElement("edit-config");
            rpc.AppendChild(edit_config);
            //创建tatget
            XmlElement target = commonXml.CreateElement("target");
            edit_config.AppendChild(target);
            //创建running
            XmlElement running = commonXml.CreateElement("running");
            target.AppendChild(running);

            //连接删除
            XmlElement config = commonXml.CreateElement("config");
            config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //配置connections
            XmlElement pgs = commonXml.CreateElement("pgs");
            pgs.SetAttribute("xmlns", "urn:ccsa:yang:acc-protection-group");
            config.AppendChild(pgs);

            //配置connection
            XmlElement pg = commonXml.CreateElement("pg");
            XmlAttribute a = commonXml.CreateAttribute("nc", "operation", "urn:ietf:params:xml:ns:netconf:base:1.0");
            a.Value = "remove";
            pg.Attributes.Append(a);
            //connection.SetAttribute("nc:operation", "remove");
            pgs.AppendChild(pg);


            //删除名称
            XmlElement pg_id = commonXml.CreateElement("pg-id");
            pg_id.InnerText = _pg_id;
            pg.AppendChild(pg_id);

            return commonXml;

        }

        public static XmlDocument EQ(string _name)
        {

            XmlDocument commonXml = new XmlDocument();
            //  创建XML文档，存在就删除再生成
            XmlDeclaration dec = commonXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            commonXml.AppendChild(dec);
            //  创建根结点
            XmlElement rpc = commonXml.CreateElement("rpc");

            rpc.SetAttribute("message-id", "1");
            rpc.SetAttribute("xmlns", "urn:ietf:params:xml:ns:netconf:base:1.0");
            commonXml.AppendChild(rpc);

            //创建信息节点
            XmlElement edit_config = commonXml.CreateElement("edit-config");
            rpc.AppendChild(edit_config);
            //创建tatget
            XmlElement target = commonXml.CreateElement("target");
            edit_config.AppendChild(target);
            //创建running
            XmlElement running = commonXml.CreateElement("running");
            target.AppendChild(running);

            //连接删除
            XmlElement config = commonXml.CreateElement("config");
            config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //配置connections
            XmlElement pgs = commonXml.CreateElement("eqs");
            pgs.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(pgs);

            //配置connection
            XmlElement pg = commonXml.CreateElement("eq");
            XmlAttribute a = commonXml.CreateAttribute("nc", "operation", "urn:ietf:params:xml:ns:netconf:base:1.0");
            a.Value = "remove";
            pg.Attributes.Append(a);
            //connection.SetAttribute("nc:operation", "remove");
            pgs.AppendChild(pg);


            //删除名称
            XmlElement name = commonXml.CreateElement("name");
            name.InnerText = _name;
            pg.AppendChild(name);

            return commonXml;

        }
    }
}
