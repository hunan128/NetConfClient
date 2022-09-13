using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class CreatePG
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

        public static XmlDocument Pg(string _pg_id, string _create_type, string _delete_cascade, string _protection_type, string _reversion_mode, string _switch_type, string _hold_off,string _wait_to_restore_time,string _sd_trigger, string _primary_port,string _secondary_port)
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
            a.Value = "create";
            pg.Attributes.Append(a);
            //connection.SetAttribute("nc:operation", "remove");
            pgs.AppendChild(pg);



            //保护ID
            XmlElement pg_id = commonXml.CreateElement("pg-id");
            pg_id.InnerText = _pg_id;
            pg.AppendChild(pg_id);
            //创建方式
            XmlElement create_type = commonXml.CreateElement("create-type");
            create_type.InnerText = _create_type;
            pg.AppendChild(create_type);
            //级联删除
            XmlElement delete_cascade = commonXml.CreateElement("delete-cascade");
            delete_cascade.InnerText = _delete_cascade;
            pg.AppendChild(delete_cascade);
            //保护类型
            XmlElement protection_type = commonXml.CreateElement("protection-type");
            protection_type.InnerText = _protection_type;
            pg.AppendChild(protection_type);
            //返回模式
            XmlElement reversion_mode = commonXml.CreateElement("reversion-mode");
            reversion_mode.InnerText = _reversion_mode;
            pg.AppendChild(reversion_mode);
            //倒换类型
            XmlElement switch_type = commonXml.CreateElement("switch-type");
            switch_type.InnerText = _switch_type;
            pg.AppendChild(switch_type);
            //hold-off
            XmlElement hold_off = commonXml.CreateElement("hold-off");
            hold_off.InnerText = _hold_off;
            pg.AppendChild(hold_off);
            //WTR
            XmlElement wait_to_restore_time = commonXml.CreateElement("wait-to-restore-time");
            wait_to_restore_time.InnerText = _wait_to_restore_time;
            pg.AppendChild(wait_to_restore_time);
            //SD
            XmlElement sd_trigger = commonXml.CreateElement("sd-trigger");
            sd_trigger.InnerText = _sd_trigger;
            pg.AppendChild(sd_trigger);


            //主用端口
            XmlElement primary_port = commonXml.CreateElement("primary-port");
            primary_port.InnerText = _primary_port;
            pg.AppendChild(primary_port);
            //备份端口
            XmlElement secondary_port = commonXml.CreateElement("secondary-port");
            secondary_port.InnerText = _secondary_port;
            pg.AppendChild(secondary_port);

            return commonXml;

        }
        public static XmlDocument Command_Eq_Pgs(string _pg_id, string _protection_command, string _protection_direction)
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
            XmlElement perform_protection_command = commonXml.CreateElement("perform-eq-protection-command");
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
