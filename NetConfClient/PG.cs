using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class PG
    {
        public static XmlDocument Command(string _pg_id,string _protection_command,string _protection_direction)
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


            //配置connections
            XmlElement perform_protection_command = commonXml.CreateElement("perform-protection-command");
            perform_protection_command.SetAttribute("xmlns", "urn:ccsa:yang:acc-protection-group");
            rpc.AppendChild(perform_protection_command);

            //配置保护ID 
            XmlElement pg_id = commonXml.CreateElement("pg-id");
            pg_id.InnerText = _pg_id;
            perform_protection_command.AppendChild(pg_id);

            //配置外部命令 
            XmlElement protection_command = commonXml.CreateElement("protection-command");
            protection_command.InnerText = _protection_command;
            perform_protection_command.AppendChild(protection_command);

            //配置方向
            XmlElement protection_direction = commonXml.CreateElement("protection-direction");
            protection_direction.InnerText = _protection_direction;
            perform_protection_command.AppendChild(protection_direction);

            return commonXml;

        }
    }
}
