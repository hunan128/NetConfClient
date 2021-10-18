using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class CreatETH
    {
        public static XmlDocument Common(string ips,string _Create_Connection,string _label, string _service_type, string _layer_protoco_name,string _cir, string _pir, string _cbs, string _pbs, string _nni_protection_type,string _service_mapping_mode,
    string _uni_ptp_name, string _vlan_id, string _vlan_priority, string _access_action, string _vlan_type, string _uni_vlan_id, string _uni_vlan_priority, string _uni_access_action, string _uni_vlan_type,
    string _primary_nni_name, string _primary_ts, string _primary_ada, string _primary_odu, string _primary_switch, string _primary_vlan_id, string _primary_vlan_priority, string _primary_access_action, string _primary_vlan_type,
    string _secondary_nni_name, string _secondary_ts, string _secondary_ada, string _secondary_odu, string _secondary_switch,
        string _sdh_signal_type, string _vc_type, string _mapping_path, string _sdh_signal_type_protect, string _mapping_path_protect,string _lcas,string _hold_off,string _wtr,string _tsd
)
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
            XmlElement create_eth_connection = null;
            if (_Create_Connection == "EOS") {
                create_eth_connection = commonXml.CreateElement("create-eos-connection");
                create_eth_connection.SetAttribute("xmlns", "urn:ccsa:yang:acc-eos");
                rpc.AppendChild(create_eth_connection);
            }
            if (_Create_Connection == "ETH")
            {
                create_eth_connection = commonXml.CreateElement("create-eth-connection");
                create_eth_connection.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
                rpc.AppendChild(create_eth_connection);
            }

            if (ips.Contains("移动"))
            {
                //lable
                XmlElement label = commonXml.CreateElement("label");
                label.InnerText = _label;
                create_eth_connection.AppendChild(label);
                //服务映射模式
                XmlElement service_mapping_mode = commonXml.CreateElement("service-mapping-mode");
                service_mapping_mode.InnerText = _service_mapping_mode;
                create_eth_connection.AppendChild(service_mapping_mode);
            }
            if (ips.Contains("联通"))
            {
                //lable
                XmlElement connection = commonXml.CreateElement("connection-name");
                connection.InnerText = "CONNECTION=" + _label;
                create_eth_connection.AppendChild(connection);
            }
            if (ips == "")
            {

                return commonXml;

            }
            //服务类型
            XmlElement service_type = commonXml.CreateElement("service-type");
            service_type.InnerText = _service_type;
            create_eth_connection.AppendChild(service_type);
            //层协议
            XmlElement layer_protocol_name = commonXml.CreateElement("layer-protocol-name");
            layer_protocol_name.InnerText = _layer_protoco_name;
            create_eth_connection.AppendChild(layer_protocol_name);



            //带宽大小
            XmlElement requested_capacity = commonXml.CreateElement("requested-capacity");
            create_eth_connection.AppendChild(requested_capacity);


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

            if (!_nni_protection_type.Contains("无"))
            {
                //保护类型
                XmlElement nni_protection_type = commonXml.CreateElement("nni-protection-type");
                nni_protection_type.InnerText = _nni_protection_type;
                create_eth_connection.AppendChild(nni_protection_type);
            }


            //客户侧配置
            XmlElement eth_uni = commonXml.CreateElement("eth-uni");
            create_eth_connection.AppendChild(eth_uni);

            //PTP接口配置
            XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
            uni_ptp_name.InnerText = _uni_ptp_name;
            eth_uni.AppendChild(uni_ptp_name);

            if (_vlan_id != "") {
                //客户VLan属性
                XmlElement client_vlan_spec = commonXml.CreateElement("client-vlan-spec");
                eth_uni.AppendChild(client_vlan_spec);

                //VLanID 
                XmlElement vlan_id = commonXml.CreateElement("vlan-id");
                vlan_id.InnerText = _vlan_id;
                client_vlan_spec.AppendChild(vlan_id);


                //VLan优先级 
                XmlElement vlan_priority = commonXml.CreateElement("vlan-priority");
                vlan_priority.InnerText = _vlan_priority;
                client_vlan_spec.AppendChild(vlan_priority);

                //VLan动作 
                XmlElement access_action = commonXml.CreateElement("access-action");
                access_action.InnerText = _access_action;
                client_vlan_spec.AppendChild(access_action);

                //VLan类型
                XmlElement vlan_type = commonXml.CreateElement("vlan-type");
                vlan_type.InnerText = _vlan_type;
                client_vlan_spec.AppendChild(vlan_type);
            }


            if (_uni_vlan_id != "") {
                //入口VLan属性
                XmlElement uni_vlan_spec = commonXml.CreateElement("uni-vlan-spec");
                eth_uni.AppendChild(uni_vlan_spec);

                //VLanID 
                XmlElement uni_vlan_id = commonXml.CreateElement("vlan-id");
                uni_vlan_id.InnerText = _uni_vlan_id;
                uni_vlan_spec.AppendChild(uni_vlan_id);


                //VLan优先级 
                XmlElement uni_vlan_priority = commonXml.CreateElement("vlan-priority");
                uni_vlan_priority.InnerText = _uni_vlan_priority;
                uni_vlan_spec.AppendChild(uni_vlan_priority);

                //VLan动作 
                XmlElement uni_access_action = commonXml.CreateElement("access-action");
                uni_access_action.InnerText = _uni_access_action;
                uni_vlan_spec.AppendChild(uni_access_action);

                //VLan类型
                XmlElement uni_vlan_type = commonXml.CreateElement("vlan-type");
                uni_vlan_type.InnerText = _uni_vlan_type;
                uni_vlan_spec.AppendChild(uni_vlan_type);

            }


            //线路侧配置
            XmlElement primary_nni = commonXml.CreateElement("primary-nni");
            create_eth_connection.AppendChild(primary_nni);
            //PTP接口配置
            XmlElement _nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
            _nni_ptp_name.InnerText = _primary_nni_name;
            primary_nni.AppendChild(_nni_ptp_name);

            //时隙配置
            XmlElement _nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
            _nni_ts_detail.InnerText = _primary_ts;
            primary_nni.AppendChild(_nni_ts_detail);

            //净荷类型
            XmlElement _adaptation_type = commonXml.CreateElement("adaptation-type");
            _adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            _adaptation_type.InnerText = _primary_ada;
            primary_nni.AppendChild(_adaptation_type);

            //ODU类型
            XmlElement _odu_signal_type = commonXml.CreateElement("odu-signal-type");
            _odu_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            _odu_signal_type.InnerText = _primary_odu;
            primary_nni.AppendChild(_odu_signal_type);

            //交换类型
            XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
            _switch_capability.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            _switch_capability.InnerText = _primary_switch;
            primary_nni.AppendChild(_switch_capability);

            if (_primary_vlan_id != "") {
                //VLan属性
                XmlElement ftp_vlan_spec = commonXml.CreateElement("ftp-vlan-spec");
                primary_nni.AppendChild(ftp_vlan_spec);


                //VLanID 
                XmlElement primary_vlan_id = commonXml.CreateElement("vlan-id");
                primary_vlan_id.InnerText = _primary_vlan_id;
                ftp_vlan_spec.AppendChild(primary_vlan_id);


                //VLan优先级 
                XmlElement primary_vlan_priority = commonXml.CreateElement("vlan-priority");
                primary_vlan_priority.InnerText = _primary_vlan_priority;
                ftp_vlan_spec.AppendChild(primary_vlan_priority);

                //VLan动作 
                XmlElement primary_access_action = commonXml.CreateElement("access-action");
                primary_access_action.InnerText = _primary_access_action;
                ftp_vlan_spec.AppendChild(primary_access_action);

                //VLan类型
                XmlElement primary_vlan_type = commonXml.CreateElement("vlan-type");
                primary_vlan_type.InnerText = _primary_vlan_type;
                ftp_vlan_spec.AppendChild(primary_vlan_type);

            }
            if (_Create_Connection == "EOS")
            {
                //EOS属性配置
                XmlElement eos_pac = commonXml.CreateElement("eos-pac");
                create_eth_connection.AppendChild(eos_pac);
                //SDH 信号类型
                XmlElement sdh_signal_type = commonXml.CreateElement("sdh-signal-type");
                sdh_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                sdh_signal_type.InnerText = _sdh_signal_type;
                eos_pac.AppendChild(sdh_signal_type);

                //VC类型
                XmlElement vc_type = commonXml.CreateElement("vc-type");
                vc_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                vc_type.InnerText = _vc_type;
                eos_pac.AppendChild(vc_type);

                string[] strArray = _mapping_path.Split(',');
                foreach (var item in strArray)
                {
                    if (item != "") {
                        XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                        mapping_path.InnerText = item;
                        eos_pac.AppendChild(mapping_path);
                    }

                }
                //时隙


                if (_secondary_nni_name != "无") {
                    //SDH 信号类型
                    XmlElement sdh_signal_type_protect = commonXml.CreateElement("sdh-signal-type-protect");
                    sdh_signal_type_protect.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_protect.InnerText = _sdh_signal_type_protect;
                    eos_pac.AppendChild(sdh_signal_type_protect);

                    //时隙
                    string[] strArrayP = _mapping_path_protect.Split(',');
                    foreach (var item in strArrayP)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path_protect = commonXml.CreateElement("mapping-path-protect");
                            mapping_path_protect.InnerText = item;
                            eos_pac.AppendChild(mapping_path_protect);

                        }

                    }

                }
                //LCAS
                XmlElement lcas = commonXml.CreateElement("lcas");
                lcas.InnerText = _lcas;
                eos_pac.AppendChild(lcas);
                //hold_off
                XmlElement hold_off = commonXml.CreateElement("hold-off");
                hold_off.InnerText = _hold_off;
                eos_pac.AppendChild(hold_off);
                //WTR
                XmlElement wtr = commonXml.CreateElement("wtr");
                wtr.InnerText = _wtr;
                eos_pac.AppendChild(wtr);
                //TSD
                XmlElement tsd = commonXml.CreateElement("tsd");
                tsd.InnerText = _tsd;
                eos_pac.AppendChild(tsd);

            }


            if (_secondary_nni_name != "无")
            {
                //线路侧配置 备接口
                XmlElement secondary_nni = commonXml.CreateElement("secondary-nni");
                create_eth_connection.AppendChild(secondary_nni);
                //PTP接口配置
                XmlElement _secondary_nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                _secondary_nni_ptp_name.InnerText = _secondary_nni_name;
                secondary_nni.AppendChild(_secondary_nni_ptp_name);

                //时隙配置
                XmlElement _secondary_nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                _secondary_nni_ts_detail.InnerText = _secondary_ts;
                secondary_nni.AppendChild(_secondary_nni_ts_detail);

                //净荷类型
                XmlElement _secondary_adaptation_type = commonXml.CreateElement("adaptation-type");
                _secondary_adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                _secondary_adaptation_type.InnerText = _secondary_ada;
                secondary_nni.AppendChild(_secondary_adaptation_type);

                //ODU类型
                XmlElement _secondary_odu_signal_type = commonXml.CreateElement("odu-signal-type");
                _secondary_odu_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                _secondary_odu_signal_type.InnerText = _secondary_odu;
                secondary_nni.AppendChild(_secondary_odu_signal_type);

                //交换类型
                XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                _secondary_switch_capability.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                _secondary_switch_capability.InnerText = _secondary_switch;
                secondary_nni.AppendChild(_secondary_switch_capability);
            }




            return commonXml;

        }
    }
}
