using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class ModifyXML
    {
        /// <summary>
        /// 层协议修改
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_layer_protocol_name">层协议</param>
        /// <returns></returns>
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
        /// <summary>
        /// 接口类型UNI或NNI
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_interface_type">接口类型</param>
        /// <returns></returns>
        public static XmlDocument interface_type(string _name, string _interface_type)
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
                XmlElement interface_type = commonXml.CreateElement("interface-type");
                interface_type.InnerText = _interface_type;
                ptp.AppendChild(interface_type);
            }

            return commonXml;

        }
        /// <summary>
        /// oduk时延测量
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_odu_ctp_delay">时延开关</param>
        /// <returns></returns>
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
        /// <summary>
        /// 显示修改
        /// </summary>
        /// <param name="_name">连接名称</param>
        /// <param name="_total_size">总带宽</param>
        /// <param name="_cir">CIR</param>
        /// <param name="_pir">PIR</param>
        /// <param name="_cbs">CBS</param>
        /// <param name="_pbs">PBS</param>
        /// <param name="ips">运营商</param>
        /// <returns></returns>
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

        /// <summary>
        /// TCA参数修改
        /// </summary>
        /// <param name="_name">接口</param>
        /// <param name="_pm_parameter_name"></param>
        /// <param name="_granularity"></param>
        /// <param name="_threshold_type"></param>
        /// <param name="_object_type">对象名称</param>
        /// <param name="_threshold_value">值</param>
        /// <param name="ips">运营商</param>
        /// <returns></returns>
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
        /// <summary>
        /// VCG时隙调整
        /// </summary>
        /// <param name="_eth_ftp_name">ETH-FTP</param>
        /// <param name="_sdh_ftp_name">SDH-FTP</param>
        /// <param name="_sdh_protect_ftp_name">SDH-FTP保护</param>
        /// <param name="_mapping_path">主用映射时隙</param>
        /// <param name="_mapping_path_protected">备用映射时隙</param>
        /// <param name="IPS">运营商</param>
        /// <returns></returns>
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

        /// <summary>
        /// NTP服务器
        /// </summary>
        /// <param name="_enable">开关</param>
        /// <param name="_name">NTP服务名称</param>
        /// <param name="_ip">IP地址</param>
        /// <param name="_port">端口</param>
        /// <param name="_ntp_version">版本</param>
        /// <returns></returns>
        public static XmlDocument Ntp(string _enable,string _name,string _ip,string _port,string _ntp_version)
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
            config.SetAttribute("xmlns", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //me
            XmlElement me = commonXml.CreateElement("me");
            me.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(me);

            //NTP服务使能
            XmlElement ntp_enable = commonXml.CreateElement("ntp-enable");
            ntp_enable.InnerText = _enable;
            me.AppendChild(ntp_enable);
            if (_enable == "true") {
                //ntp-servers
                XmlElement ntp_servers = commonXml.CreateElement("ntp-servers");
                ntp_servers.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                me.AppendChild(ntp_servers);
                //ntp-server
                XmlElement ntp_server = commonXml.CreateElement("ntp-server");
                ntp_server.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ntp_servers.AppendChild(ntp_server);
                //NTP名称
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                ntp_server.AppendChild(name);
                //NTP ip
                XmlElement ip_ipaddress = commonXml.CreateElement("ip-address");
                ip_ipaddress.InnerText = _ip;
                ntp_server.AppendChild(ip_ipaddress);
                //NTP 端口
                XmlElement port = commonXml.CreateElement("port");
                port.InnerText = _port;
                ntp_server.AppendChild(port);
                //NTP 版本
                XmlElement ntp_version = commonXml.CreateElement("ntp-version");
                ntp_version.InnerText = _ntp_version;
                ntp_server.AppendChild(ntp_version);
            }

            return commonXml;

        }


        /// <summary>
        /// VLAN修改
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_vlan_id"></param>
        /// <param name="_vlan_priority"></param>
        /// <param name="_access_action"></param> 
        /// <param name="_vlan_type"></param>
        /// <param name="_client_vlan_id"></param>
        /// <param name="_client_vlan_priority"></param>
        /// <param name="_client_access_action"></param>
        /// <param name="_client_vlan_type"></param>
        /// <returns></returns>
        public static XmlDocument Vlan_spec(string _name, string _vlan_id,string _vlan_priority, string _access_action,string _vlan_type,string _client_vlan_id, string _client_vlan_priority, string _client_access_action, string _client_vlan_type)
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
                XmlElement eth_ctp_pac = commonXml.CreateElement("eth-ctp-pac");
                eth_ctp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
                ptp.AppendChild(eth_ctp_pac);
                if (_vlan_id != "" || _vlan_priority != "" || _access_action != "" || _vlan_type != "") {
                    XmlElement vlan_spec = commonXml.CreateElement("vlan-spec");
                    eth_ctp_pac.AppendChild(vlan_spec);
                    if (_vlan_id != "")
                    {
                        XmlElement vlan_id = commonXml.CreateElement("vlan-id");
                        vlan_id.InnerText = _vlan_id;
                        vlan_spec.AppendChild(vlan_id);
                    }
                    if (_vlan_priority != "")
                    {
                        XmlElement vlan_priority = commonXml.CreateElement("vlan-priority");
                        vlan_priority.InnerText = _vlan_priority;
                        vlan_spec.AppendChild(vlan_priority);
                    }
                    if (_access_action != "")
                    {
                        XmlElement access_action = commonXml.CreateElement("access-action");
                        access_action.InnerText = _access_action;
                        vlan_spec.AppendChild(access_action);
                    }
                    if (_vlan_type != "")
                    {
                        XmlElement vlan_type = commonXml.CreateElement("vlan-type");
                        vlan_type.InnerText = _vlan_type;
                        vlan_spec.AppendChild(vlan_type);
                    }
                }
                if (_client_vlan_id != "" || _client_vlan_priority != "" || _client_access_action != "" || _client_vlan_type != "")
                {
                    XmlElement vlan_spec = commonXml.CreateElement("client-vlan-spec");
                    eth_ctp_pac.AppendChild(vlan_spec);
                    if (_client_vlan_id != "") {
                        XmlElement vlan_id = commonXml.CreateElement("vlan-id");
                        vlan_id.InnerText = _client_vlan_id;
                        vlan_spec.AppendChild(vlan_id);
                    }
                    if (_client_vlan_priority != "") {
                        XmlElement vlan_priority = commonXml.CreateElement("vlan-priority");
                        vlan_priority.InnerText = _client_vlan_priority;
                        vlan_spec.AppendChild(vlan_priority);
                    }
                    if (_client_access_action != "") {
                        XmlElement access_action = commonXml.CreateElement("access-action");
                        access_action.InnerText = _client_access_action;
                        vlan_spec.AppendChild(access_action);
                    }
                    if (_client_vlan_type != "") {
                        XmlElement vlan_type = commonXml.CreateElement("vlan-type");
                        vlan_type.InnerText = _client_vlan_type;
                        vlan_spec.AppendChild(vlan_type);
                    }

                }

            }

            return commonXml;

        }

        /// <summary>
        /// 环回接口
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_loopback_type">环回类型</param>
        /// <returns></returns>
        public static XmlDocument Loop_back(string _name, string _loopback_type)
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
            //PTP端口
            if (_name.Contains("PTP") && !_name.Contains("CTP")) {
                //PTP
                XmlElement ptps = commonXml.CreateElement("ptps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                config.AppendChild(ptps);

                //PTP
                XmlElement ptp = commonXml.CreateElement("ptp");
                ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ptps.AppendChild(ptp);
                if (!_name.Contains("无"))
                {
                    XmlElement name = commonXml.CreateElement("name");
                    name.InnerText = _name;
                    ptp.AppendChild(name);
                    XmlElement loop_back = commonXml.CreateElement("loop-back");
                    loop_back.InnerText = _loopback_type;
                    ptp.AppendChild(loop_back);
                }

            }
            //FTP端口
            if (_name.Contains("FTP") && !_name.Contains("CTP"))
            {
                //PTP
                XmlElement ptps = commonXml.CreateElement("ftps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                config.AppendChild(ptps);

                //PTP
                XmlElement ptp = commonXml.CreateElement("ftp");
                ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ptps.AppendChild(ptp);
                if (!_name.Contains("无"))
                {
                    XmlElement name = commonXml.CreateElement("name");
                    name.InnerText = _name;
                    ptp.AppendChild(name);
                    XmlElement loop_back = commonXml.CreateElement("loop-back");
                    loop_back.InnerText = _loopback_type;
                    ptp.AppendChild(loop_back);
                }

            }
            //CTP端口
            if (_name.Contains("CTP"))
            {
                //PTP
                XmlElement ptps = commonXml.CreateElement("ctps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                config.AppendChild(ptps);

                //PTP
                XmlElement ptp = commonXml.CreateElement("ctp");
                ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ptps.AppendChild(ptp);
                if (!_name.Contains("无"))
                {
                    XmlElement name = commonXml.CreateElement("name");
                    name.InnerText = _name;
                    ptp.AppendChild(name);
                    XmlElement loop_back = commonXml.CreateElement("loop-back");
                    loop_back.InnerText = _loopback_type;
                    ptp.AppendChild(loop_back);
                }

            }
            return commonXml;

        }
        /// <summary>
        /// 激光器开关配置
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_laser_status">激光器开关</param>
        /// <returns></returns>
        public static XmlDocument Laser_status(string _name, string _laser_status)
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
                XmlElement laser_status = commonXml.CreateElement("laser-status");
                laser_status.InnerText = _laser_status;
                ptp.AppendChild(laser_status);
            }

            return commonXml;

        }

        /// <summary>
        /// 端口shutdownl
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_admin_state">使能或关闭</param>
        /// <returns></returns>
        public static XmlDocument Admin_state(string _name, string _admin_state )
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

            //PTP
            if (_name.Contains("PTP") && !_name.Contains("CTP")) {
                //PTP
                XmlElement ptps = commonXml.CreateElement("ptps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                config.AppendChild(ptps);

                //PTP
                XmlElement ptp = commonXml.CreateElement("ptp");
                ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ptps.AppendChild(ptp);
                if (!_name.Contains("无"))
                {
                    XmlElement name = commonXml.CreateElement("name");
                    name.InnerText = _name;
                    ptp.AppendChild(name);
                    XmlElement state_pac = commonXml.CreateElement("state-pac");
                    ptp.AppendChild(state_pac);
                    XmlElement admin_state = commonXml.CreateElement("admin-state");
                    admin_state.InnerText = _admin_state;
                    state_pac.AppendChild(admin_state);
                }
            }
            //FTP
            if (_name.Contains("FTP") && !_name.Contains("CTP"))
            {
                //FTP
                XmlElement ptps = commonXml.CreateElement("ctps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                config.AppendChild(ptps);

                //FTP
                XmlElement ptp = commonXml.CreateElement("ctp");
                ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                ptps.AppendChild(ptp);
                if (!_name.Contains("无"))
                {
                    XmlElement name = commonXml.CreateElement("name");
                    name.InnerText = _name;
                    ptp.AppendChild(name);
                    XmlElement state_pac = commonXml.CreateElement("state-pac");
                    ptp.AppendChild(state_pac);
                    XmlElement admin_state = commonXml.CreateElement("admin-state");
                    admin_state.InnerText = _admin_state;
                    state_pac.AppendChild(admin_state);
                }
            }
            //CTP
            if (_name.Contains("CTP"))
            {
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
                    XmlElement state_pac = commonXml.CreateElement("state-pac");
                    ptp.AppendChild(state_pac);
                    XmlElement admin_state = commonXml.CreateElement("admin-state");
                    admin_state.InnerText = _admin_state;
                    state_pac.AppendChild(admin_state);
                }
            }

            return commonXml;

        }

        /// <summary>
        /// MTU修改
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_mtu">MTU值</param>
        /// <returns></returns>
        public static XmlDocument MTU(string _name, string _mtu)
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
                XmlElement eth_ptp_pac = commonXml.CreateElement("eth-ptp-pac");
                eth_ptp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
                ptp.AppendChild(eth_ptp_pac);
                XmlElement current_mtu = commonXml.CreateElement("current-mtu");
                current_mtu.InnerText = _mtu;
                eth_ptp_pac.AppendChild(current_mtu);
            }

            return commonXml;

        }

        /// <summary>
        /// 双工模式
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_mode">双工模式</param>
        /// <returns></returns>
        public static XmlDocument Working_Mode(string _name, string _working_mode)
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
                XmlElement eth_ptp_pac = commonXml.CreateElement("eth-ptp-pac");
                eth_ptp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
                ptp.AppendChild(eth_ptp_pac);
                XmlElement current_working_mode = commonXml.CreateElement("current-working-mode");
                current_working_mode.InnerText = _working_mode;
                eth_ptp_pac.AppendChild(current_working_mode);
            }

            return commonXml;

        }

        /// <summary>
        /// LLDP开关
        /// </summary>
        /// <param name="_name">接口名称</param>
        /// <param name="_enable">开关</param>
        /// <returns></returns>
        public static XmlDocument LLDP(string _name, string _lldp_enable)
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
                XmlElement eth_ptp_pac = commonXml.CreateElement("eth-ptp-pac");
                eth_ptp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
                ptp.AppendChild(eth_ptp_pac);
                XmlElement lldp_enable = commonXml.CreateElement("lldp-enable");
                lldp_enable.InnerText = _lldp_enable;
                eth_ptp_pac.AppendChild(lldp_enable);
            }

            return commonXml;

        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="_user_name">用户名</param>
        /// <param name="_old_password">旧密码</param>
        /// <param name="_new_password">新密码</param>
        /// <returns></returns>

        public static XmlDocument Password(string _user_name, string _old_password,string _new_password)
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

            ////创建信息节点
            //XmlElement edit_config = commonXml.CreateElement("edit-config");
            //rpc.AppendChild(edit_config);
            ////创建tatget
            //XmlElement target = commonXml.CreateElement("target");
            //edit_config.AppendChild(target);
            ////创建running
            //XmlElement running = commonXml.CreateElement("running");
            //target.AppendChild(running);

            ////连接删除
            //XmlElement config = commonXml.CreateElement("config");
            //config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            //edit_config.AppendChild(config);


            //CTP
            XmlElement modify_user_password = commonXml.CreateElement("modify-user-password");
            modify_user_password.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            rpc.AppendChild(modify_user_password);

            if (_user_name != "") {
                //用户名
                XmlElement user_name = commonXml.CreateElement("user-name");
                user_name.InnerText = _user_name;
                modify_user_password.AppendChild(user_name);
            }


            XmlElement old_password = commonXml.CreateElement("old-password");
            old_password.InnerText = _old_password;
            modify_user_password.AppendChild(old_password);
            XmlElement new_password = commonXml.CreateElement("new-password");
            new_password.InnerText = _new_password;
            modify_user_password.AppendChild(new_password);
            return commonXml;

        }


        public static XmlDocument Reset(string _eq_name, string _reset_type)
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

            ////创建信息节点
            //XmlElement edit_config = commonXml.CreateElement("edit-config");
            //rpc.AppendChild(edit_config);
            ////创建tatget
            //XmlElement target = commonXml.CreateElement("target");
            //edit_config.AppendChild(target);
            ////创建running
            //XmlElement running = commonXml.CreateElement("running");
            //target.AppendChild(running);

            ////连接删除
            //XmlElement config = commonXml.CreateElement("config");
            //config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            //edit_config.AppendChild(config);


            //CTP
            XmlElement reset = commonXml.CreateElement("reset");
            reset.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            rpc.AppendChild(reset);

            if (_eq_name != "")
            {
                //板卡名称
                XmlElement eq_name = commonXml.CreateElement("eq-name");
                eq_name.InnerText = _eq_name;
                reset.AppendChild(eq_name);
                //软硬复位
                XmlElement reset_type = commonXml.CreateElement("reset-type");
                reset_type.InnerText = _reset_type;
                reset.AppendChild(reset_type);
            }

            return commonXml;

        }

        public static XmlDocument Set_Managed_Element_Time(string _new_time)
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

            //CTP
            XmlElement set_managed_element_time = commonXml.CreateElement("set-managed-element-time");
            set_managed_element_time.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            rpc.AppendChild(set_managed_element_time);

            if (_new_time != "")
            {
                //板卡名称
                XmlElement new_time = commonXml.CreateElement("new-time");
                new_time.InnerText = _new_time;
                set_managed_element_time.AppendChild(new_time);
            }

            return commonXml;

        }


        public static XmlDocument Mc_Port(string _name,string _admin_state)
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
            XmlElement mc_ports = commonXml.CreateElement("mc-ports");
            mc_ports.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(mc_ports);

            XmlElement mc_port = commonXml.CreateElement("mc-port");
            mc_ports.AppendChild(mc_port);

            if (_name != "")
            {
                //板卡名称
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                mc_port.AppendChild(name);
            }
            if (_admin_state != "")
            {
                //板卡名称
                XmlElement admin_state = commonXml.CreateElement("admin-state");
                admin_state.InnerText = _admin_state;
                mc_port.AppendChild(admin_state);
            }
            return commonXml;

        }
    }
}
