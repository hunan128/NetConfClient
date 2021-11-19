using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class Modify
    {
        public static XmlDocument Layer_protocal_name(string _name,string _layer_protocol_name)
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


            //CTP
            XmlElement ptps = commonXml.CreateElement("ptps");
            ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(ptps);

            //CTP
            XmlElement ptp = commonXml.CreateElement("ptp");
            ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            ptps.AppendChild(ptp);
            if (!_name.Contains("无"))
            {
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                ptp.AppendChild(name);
                XmlElement layer_protocol_name = commonXml.CreateElement("layer-protocol-name");
                layer_protocol_name.InnerText = _layer_protocol_name;
                ptp.AppendChild(layer_protocol_name);
            }

            return commonXml;

        }
        public static XmlDocument Odu_ctp_delay(string _name, string _odu_ctp_delay)
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


            //CTP
            XmlElement ptps = commonXml.CreateElement("ctps");
            ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(ptps);

            //CTP
            XmlElement ptp = commonXml.CreateElement("ctp");
            ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            ptps.AppendChild(ptp);
            if (!_name.Contains("无"))
            {
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                ptp.AppendChild(name);
                XmlElement odu_ctp_pac = commonXml.CreateElement("odu-ctp-pac");
                odu_ctp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-otn");
                ptp.AppendChild(odu_ctp_pac);
                XmlElement odu_delay_enable = commonXml.CreateElement("odu-delay-enable");
                odu_ctp_pac.AppendChild(odu_delay_enable);
                XmlElement odu_ctp_delay_enable = commonXml.CreateElement("odu-ctp-delay-enable");
                odu_ctp_delay_enable.InnerText = _odu_ctp_delay;
                odu_delay_enable.AppendChild(odu_ctp_delay_enable);
            }

            return commonXml;

        }
        public static XmlDocument Connection_Rate(string _name, string _total_size,string _cir,string _pir ,string _cbs,string _pbs,string ips)
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


            //connections
            XmlElement connections = commonXml.CreateElement("connections");
            connections.SetAttribute("xmlns", "urn:ccsa:yang:acc-connection");
            config.AppendChild(connections);

            //connection
            XmlElement connection = commonXml.CreateElement("connection");
            connections.AppendChild(connection);

            if (!_name.Contains("无"))
            {
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                connection.AppendChild(name);
                XmlElement requested_capacity  = commonXml.CreateElement("requested-capacity");
                connection.AppendChild(requested_capacity);
                XmlElement total_size = commonXml.CreateElement("total-size");
                total_size.InnerText = _total_size;
                requested_capacity.AppendChild(total_size);
                XmlElement cir = commonXml.CreateElement("cir");
                cir.InnerText = _cir;
                requested_capacity.AppendChild(cir);
                XmlElement pir = commonXml.CreateElement("pir");
                pir.InnerText = _pir;
                requested_capacity.AppendChild(pir);
                XmlElement cbs = commonXml.CreateElement("cbs");
                cbs.InnerText = _cbs;
                requested_capacity.AppendChild(cbs);
                XmlElement pbs = commonXml.CreateElement("pbs");
                pbs.InnerText = _pbs;
                requested_capacity.AppendChild(pbs);

            }

            return commonXml;

        }
    }
}
