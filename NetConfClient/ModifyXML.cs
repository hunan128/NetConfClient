using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class ModifyXML
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


        public static XmlDocument Tca_parameters(string _name, string _pm_parameter_name,string _granularity,string _threshold_type,string _object_type,string _threshold_value,string ips)
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
            XmlElement tca_parameters = commonXml.CreateElement("tca-parameters");
            tca_parameters.SetAttribute("xmlns", "urn:ccsa:yang:acc-alarms");
            config.AppendChild(tca_parameters);

            //CTP
            XmlElement tca_parameter = commonXml.CreateElement("tca-parameter");
            tca_parameter.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            tca_parameters.AppendChild(tca_parameter);
            if (!_name.Contains("无"))
            {
                XmlElement name = commonXml.CreateElement("object-name");
                name.InnerText = _name;
                tca_parameter.AppendChild(name);
                XmlElement pm_parameter_name = commonXml.CreateElement("pm-parameter-name");
                pm_parameter_name.InnerText = _pm_parameter_name;
                tca_parameter.AppendChild(pm_parameter_name);
                XmlElement granularity = commonXml.CreateElement("granularity");
                granularity.InnerText = _granularity;
                tca_parameter.AppendChild(granularity);
                XmlElement threshold_type = commonXml.CreateElement("threshold-type");
                threshold_type.InnerText = _threshold_type;
                tca_parameter.AppendChild(threshold_type);
                XmlElement object_type = commonXml.CreateElement("object-type");
                object_type.InnerText = _object_type;
                tca_parameter.AppendChild(object_type);
                XmlElement threshold_value = commonXml.CreateElement("threshold-value");
                threshold_value.InnerText = _threshold_value;
                tca_parameter.AppendChild(threshold_value);
            }

            return commonXml;

        }

        public static XmlDocument Modify_vcg_connection_capacity(string _eth_ftp_name, string _sdh_ftp_name, string _sdh_protect_ftp_name, string _mapping_path, string _mapping_path_protected, string IPS)
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
            XmlElement modify_vcg_connection_capacity = commonXml.CreateElement("modify-vcg-connection-capacity");
            modify_vcg_connection_capacity.SetAttribute("xmlns", "urn:ccsa:yang:acc-eos");
            rpc.AppendChild(modify_vcg_connection_capacity);
            if (IPS.Contains("移动"))
            {

                //ETH FTP
                XmlElement eth_ftp_name = commonXml.CreateElement("eth-ftp-name");
                eth_ftp_name.InnerText = _eth_ftp_name;
                modify_vcg_connection_capacity.AppendChild(eth_ftp_name);
                //SDH FTP
                XmlElement sdh_ftp_name = commonXml.CreateElement("sdh-ftp-name");
                sdh_ftp_name.InnerText = _sdh_ftp_name;
                modify_vcg_connection_capacity.AppendChild(sdh_ftp_name);
                //主用时隙
                XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                mapping_path.InnerText = _mapping_path;
                modify_vcg_connection_capacity.AppendChild(mapping_path);
                if (!string.IsNullOrEmpty(_sdh_protect_ftp_name))
                {
                    //SDH FTP P
                    XmlElement sdh_protect_ftp_name = commonXml.CreateElement("sdh-protect-ftp-name");
                    sdh_protect_ftp_name.InnerText = _sdh_protect_ftp_name;
                    modify_vcg_connection_capacity.AppendChild(sdh_protect_ftp_name);
                    //备用时隙
                    XmlElement mapping_path_protected = commonXml.CreateElement("mapping-path-protected");
                    mapping_path_protected.InnerText = _mapping_path_protected;
                    modify_vcg_connection_capacity.AppendChild(mapping_path_protected);
                }

            }
            if (IPS.Contains("联通"))
            {

                //ETH FTP
                XmlElement eth_ftp_name = commonXml.CreateElement("eth-ftp-name");
                eth_ftp_name.InnerText = _eth_ftp_name;
                modify_vcg_connection_capacity.AppendChild(eth_ftp_name);
                //SDH FTP
                XmlElement sdh_ftp_name = commonXml.CreateElement("sdh-ftp-name");
                sdh_ftp_name.InnerText = _sdh_ftp_name;
                modify_vcg_connection_capacity.AppendChild(sdh_ftp_name);
                //主用时隙
                string[] strArray = _mapping_path.Split(',');
                foreach (var item in strArray)
                {
                    if (item != "")
                    {
                        XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                        mapping_path.InnerText = item;
                        modify_vcg_connection_capacity.AppendChild(mapping_path);
                    }

                }
                if (!string.IsNullOrEmpty(_sdh_protect_ftp_name))
                {
                    //SDH FTP P
                    XmlElement sdh_protect_ftp_name = commonXml.CreateElement("sdh-protect-ftp-name");
                    sdh_protect_ftp_name.InnerText = _sdh_protect_ftp_name;
                    modify_vcg_connection_capacity.AppendChild(sdh_protect_ftp_name);
                    //备用时隙



                    string[] strArray1 = _mapping_path_protected.Split(',');
                    foreach (var item in strArray1)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path_protected = commonXml.CreateElement("mapping-path-protected");
                            mapping_path_protected.InnerText = item;
                            modify_vcg_connection_capacity.AppendChild(mapping_path_protected);
                        }

                    }
                }

            }

            return commonXml;

        }
    }
}
