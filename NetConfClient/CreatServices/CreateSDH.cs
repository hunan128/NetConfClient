using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class CreateSDH
    {
        public static XmlDocument Common(string ips,string _label, string _service_type, string _layer_protoco_name, string _total_size, string _uni_protection_type, string _nni_protection_type, string _nni2_protection_type, string _service_mapping_mode,
                string _uni_ptp_name,string _uni_vc_type,string _uni_mapping_path,
                string _uni_ptp_name2, string _uni_vc_type2, string _uni_mapping_path2,
                string _primary_nni_name, string _primary_ts, string _primary_ada, string _primary_odu, string _primary_switch,string _sdh_signal_type_A,string _vc_type_A,string _mapping_path_A,
                string _secondary_nni_name, string _secondary_ts, string _secondary_ada, string _secondary_odu, string _secondary_switch, string _sdh_signal_type_B, string _vc_type_B, string _mapping_path_B,
                string _primary_nni_name2, string _primary_ts2, string _primary_ada2, string _primary_odu2, string _primary_switch2, string _sdh_signal_type_A2, string _vc_type_A2, string _mapping_path_A2,
                string _secondary_nni_name2, string _secondary_ts2, string _secondary_ada2, string _secondary_odu2, string _secondary_switch2, string _sdh_signal_type_B2, string _vc_type_B2, string _mapping_path_B2
)
        {
            _layer_protoco_name = "acc-sdh:" + _layer_protoco_name;
            string xmlns = "";
            if (ips.Contains("联通"))
            {
                xmlns = "otn-types:";
                _primary_odu = xmlns + _primary_odu;
                _primary_switch = xmlns + _primary_switch;
                _secondary_odu = xmlns + _secondary_odu;
                _secondary_switch = xmlns + _secondary_switch;
                _primary_odu2 = xmlns + _primary_odu2;
                _primary_switch2 = xmlns + _primary_switch2;

            }
            if (ips.Contains("移动"))
            {
                xmlns = "acc-otn-types:";
                _primary_ada = xmlns + _primary_ada;
                _primary_odu = xmlns + _primary_odu;
                _primary_switch = xmlns + _primary_switch;
                _secondary_ada = xmlns + _secondary_ada;
                _secondary_odu = xmlns + _secondary_odu;
                _secondary_switch = xmlns + _secondary_switch;
                _uni_vc_type = xmlns + _uni_vc_type;
                _sdh_signal_type_A = xmlns + _sdh_signal_type_A;
                _vc_type_A = xmlns + _vc_type_A;
                _sdh_signal_type_B = xmlns + _sdh_signal_type_B;
                _vc_type_B = xmlns + _vc_type_B;
                _primary_ada2 = xmlns + _primary_ada2;
                _primary_odu2 = xmlns + _primary_odu2;
                _primary_switch2 = xmlns + _primary_switch2;
                _secondary_ada2 = xmlns + _secondary_ada2;
                _secondary_odu2 = xmlns + _secondary_odu2;
                _secondary_switch2 = xmlns + _secondary_switch2;
                _uni_vc_type2 = xmlns + _uni_vc_type2;
                _sdh_signal_type_A2 = xmlns + _sdh_signal_type_A2;
                _vc_type_A2 = xmlns + _vc_type_A2;
                _sdh_signal_type_B2 = xmlns + _sdh_signal_type_B2;
                _vc_type_B2 = xmlns + _vc_type_B2;
            }
            if (ips.Contains("电信"))
            {
                xmlns = "acc-enum:";
                //_uni_protection_type = "acc-pg:" + _uni_protection_type;
                //_nni_protection_type = "acc-pg:" + _nni_protection_type;
                //_nni2_protection_type = "acc-pg:" + _nni2_protection_type;
                _service_mapping_mode = xmlns + _service_mapping_mode;
                _primary_ada = xmlns + _primary_ada;
                _primary_odu = xmlns + _primary_odu;
                _primary_switch = xmlns + _primary_switch;
                _secondary_ada = xmlns + _secondary_ada;
                _secondary_odu = xmlns + _secondary_odu;
                _secondary_switch = xmlns + _secondary_switch;


                _uni_vc_type = xmlns + _uni_vc_type;
                _sdh_signal_type_A = xmlns + _sdh_signal_type_A;
                _vc_type_A = xmlns + _vc_type_A;
                _sdh_signal_type_B = xmlns + _sdh_signal_type_B;
                _vc_type_B = xmlns + _vc_type_B;

                _primary_ada2 = xmlns + _primary_ada2;
                _primary_odu2 = xmlns + _primary_odu2;
                _primary_switch2 = xmlns + _primary_switch2;
                _secondary_ada2 = xmlns + _secondary_ada2;
                _secondary_odu2 = xmlns + _secondary_odu2;
                _secondary_switch2 = xmlns + _secondary_switch2;


                _uni_vc_type2 = xmlns + _uni_vc_type2;
                _sdh_signal_type_A2 = xmlns + _sdh_signal_type_A2;
                _vc_type_A2 = xmlns + _vc_type_A2;
                _sdh_signal_type_B2 = xmlns + _sdh_signal_type_B2;
                _vc_type_B2 = xmlns + _vc_type_B2;

                // _uni_protection_type = "acc-pg:" + _uni_protection_type;
            }
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
            XmlElement create_sdh_connection = null;
            create_sdh_connection = commonXml.CreateElement("create-sdh-connection");
            create_sdh_connection.SetAttribute("xmlns", "urn:ccsa:yang:acc-sdh");
            rpc.AppendChild(create_sdh_connection);
            if (ips.Contains("电信"))
            {
                //lable
                XmlElement label = commonXml.CreateElement("label");
                label.InnerText = _label;
                create_sdh_connection.AppendChild(label);
                //服务映射模式
                XmlElement service_mapping_mode = commonXml.CreateElement("service-mapping-mode");
                service_mapping_mode.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-enum");
                service_mapping_mode.InnerText = _service_mapping_mode;
                create_sdh_connection.AppendChild(service_mapping_mode);
            }
            if (ips.Contains("移动"))
            {
                //lable
                XmlElement label = commonXml.CreateElement("label");
                label.InnerText = _label;
                create_sdh_connection.AppendChild(label);
                //服务映射模式
                XmlElement service_mapping_mode = commonXml.CreateElement("service-mapping-mode");
                service_mapping_mode.InnerText = _service_mapping_mode;
                create_sdh_connection.AppendChild(service_mapping_mode);
            }
            if (ips.Contains("联通"))
            {
                //lable
                XmlElement connection = commonXml.CreateElement("connection-name");
                connection.InnerText = "CONNECTION=" + _label;
                create_sdh_connection.AppendChild(connection);
            }
            if (ips == "")
            {

                return commonXml;

            }
            if (ips.Contains("移动")) {
                //服务类型
                XmlElement service_type = commonXml.CreateElement("service-type");
                service_type.InnerText = _service_type;
                create_sdh_connection.AppendChild(service_type);
                //层协议
                XmlElement layer_protocol_name = commonXml.CreateElement("layer-protocol-name");
                layer_protocol_name.SetAttribute("xmlns:acc-sdh", "urn:ccsa:yang:acc-sdh");
                layer_protocol_name.InnerText = _layer_protoco_name;
                create_sdh_connection.AppendChild(layer_protocol_name);



                //带宽大小
                XmlElement requested_capacity = commonXml.CreateElement("requested-capacity");
                create_sdh_connection.AppendChild(requested_capacity);



                XmlElement total_size = commonXml.CreateElement("total-size");
                total_size.InnerText = _total_size;
                requested_capacity.AppendChild(total_size);
                if (!_uni_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement uni_protection_type = commonXml.CreateElement("uni-protection-type");
                    uni_protection_type.InnerText = _uni_protection_type;
                    create_sdh_connection.AppendChild(uni_protection_type);
                }

                if (!_nni_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement nni_protection_type = commonXml.CreateElement("nni-protection-type");
                    nni_protection_type.InnerText = _nni_protection_type;
                    create_sdh_connection.AppendChild(nni_protection_type);
                }

                if (!_nni2_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement nni2_protection_type = commonXml.CreateElement("nni2-protection-type");
                    nni2_protection_type.InnerText = _nni2_protection_type;
                    create_sdh_connection.AppendChild(nni2_protection_type);
                }

                if (_uni_ptp_name != "无") {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    vc_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }

                }
                if (_uni_ptp_name2 != "无")
                {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("second-sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name2;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    vc_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type2;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path2.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }

                }

                if (_primary_nni_name != "无") {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni");
                    create_sdh_connection.AppendChild(primary_nni);
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
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-ftp");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    sdh_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A;
                    sdh_ftp.AppendChild(sdh_signal_type);

                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    vc_type_A.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A;
                    sdh_ftp.AppendChild(vc_type_A);

                    string[] strArray_A = _mapping_path_A.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



                if (_secondary_nni_name != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni");
                    create_sdh_connection.AppendChild(secondary_nni);
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

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-ftp");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                    sdh_signal_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);

                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B;
                    sdh_ftp_B.AppendChild(vc_type_B);

                    string[] strArray_B = _mapping_path_B.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }
                if (_primary_nni_name2 != "无")
                {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni2");
                    create_sdh_connection.AppendChild(primary_nni);
                    //PTP接口配置
                    XmlElement _nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _nni_ptp_name.InnerText = _primary_nni_name2;
                    primary_nni.AppendChild(_nni_ptp_name);

                    //时隙配置
                    XmlElement _nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _nni_ts_detail.InnerText = _primary_ts2;
                    primary_nni.AppendChild(_nni_ts_detail);

                    //净荷类型
                    XmlElement _adaptation_type = commonXml.CreateElement("adaptation-type");
                    _adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _adaptation_type.InnerText = _primary_ada2;
                    primary_nni.AppendChild(_adaptation_type);

                    //ODU类型
                    XmlElement _odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _odu_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _odu_signal_type.InnerText = _primary_odu2;
                    primary_nni.AppendChild(_odu_signal_type);

                    //交换类型
                    XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
                    _switch_capability.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _switch_capability.InnerText = _primary_switch2;
                    primary_nni.AppendChild(_switch_capability);
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-ftp");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    sdh_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A2;
                    sdh_ftp.AppendChild(sdh_signal_type);

                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    vc_type_A.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A2;
                    sdh_ftp.AppendChild(vc_type_A);

                    string[] strArray_A = _mapping_path_A2.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



                if (_secondary_nni_name2 != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni2");
                    create_sdh_connection.AppendChild(secondary_nni);
                    //PTP接口配置
                    XmlElement _secondary_nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _secondary_nni_ptp_name.InnerText = _secondary_nni_name2;
                    secondary_nni.AppendChild(_secondary_nni_ptp_name);

                    //时隙配置
                    XmlElement _secondary_nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _secondary_nni_ts_detail.InnerText = _secondary_ts2;
                    secondary_nni.AppendChild(_secondary_nni_ts_detail);

                    //净荷类型
                    XmlElement _secondary_adaptation_type = commonXml.CreateElement("adaptation-type");
                    _secondary_adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _secondary_adaptation_type.InnerText = _secondary_ada2;
                    secondary_nni.AppendChild(_secondary_adaptation_type);

                    //ODU类型
                    XmlElement _secondary_odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _secondary_odu_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _secondary_odu_signal_type.InnerText = _secondary_odu2;
                    secondary_nni.AppendChild(_secondary_odu_signal_type);

                    //交换类型
                    XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                    _secondary_switch_capability.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _secondary_switch_capability.InnerText = _secondary_switch2;
                    secondary_nni.AppendChild(_secondary_switch_capability);

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-ftp");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                    sdh_signal_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B2;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);

                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B2;
                    sdh_ftp_B.AppendChild(vc_type_B);

                    string[] strArray_B = _mapping_path_B2.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }





            }
            if (ips.Contains("联通"))
            {
                //服务类型
                XmlElement service_type = commonXml.CreateElement("service-type");
                service_type.InnerText = _service_type;
                create_sdh_connection.AppendChild(service_type);
                //层协议
                XmlElement layer_protocol_name = commonXml.CreateElement("layer-protocol-name");
                layer_protocol_name.SetAttribute("xmlns:acc-sdh", "urn:ccsa:yang:acc-sdh");
                layer_protocol_name.InnerText = _layer_protoco_name;
                create_sdh_connection.AppendChild(layer_protocol_name);


                //带宽大小
                XmlElement requested_capacity = commonXml.CreateElement("requested-capacity");
                create_sdh_connection.AppendChild(requested_capacity);

                XmlElement total_size = commonXml.CreateElement("total-size");
                total_size.InnerText = _total_size;
                requested_capacity.AppendChild(total_size);

                if (!_nni_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement nni_protection_type = commonXml.CreateElement("nni-protection-type");
                    nni_protection_type.InnerText = _nni_protection_type;
                    create_sdh_connection.AppendChild(nni_protection_type);
                }

                if (_uni_ptp_name != "无") {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    // vc_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }
                }
                if (_uni_ptp_name2 != "无")
                {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("second-sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name2;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    // vc_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type2;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path2.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }
                }


                if (_primary_nni_name != "无") {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni-1");
                    create_sdh_connection.AppendChild(primary_nni);
                    //VC 类型
                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    // vc_type_A.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A;
                    primary_nni.AppendChild(vc_type_A);

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
                    // _adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _adaptation_type.InnerText = _primary_ada;
                    primary_nni.AppendChild(_adaptation_type);

                    //ODU类型
                    XmlElement _odu_signal_type = commonXml.CreateElement("client-signal-type");
                    _odu_signal_type.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _odu_signal_type.InnerText = _primary_odu;
                    primary_nni.AppendChild(_odu_signal_type);

                    //交换类型
                    XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
                    _switch_capability.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _switch_capability.InnerText = _primary_switch;
                    primary_nni.AppendChild(_switch_capability);
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-spec");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    //sdh_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A;
                    sdh_ftp.AppendChild(sdh_signal_type);



                    string[] strArray_A = _mapping_path_A.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



                if (_secondary_nni_name != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni-1");
                    create_sdh_connection.AppendChild(secondary_nni);

                    //PTP接口配置
                    XmlElement _secondary_nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _secondary_nni_ptp_name.InnerText = _secondary_nni_name;
                    secondary_nni.AppendChild(_secondary_nni_ptp_name);
                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B;
                    secondary_nni.AppendChild(vc_type_B);
                    //时隙配置
                    XmlElement _secondary_nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _secondary_nni_ts_detail.InnerText = _secondary_ts;
                    secondary_nni.AppendChild(_secondary_nni_ts_detail);

                    //净荷类型
                    XmlElement _secondary_adaptation_type = commonXml.CreateElement("adaptation-type");
                    //_secondary_adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _secondary_adaptation_type.InnerText = _secondary_ada;
                    secondary_nni.AppendChild(_secondary_adaptation_type);

                    //ODU类型
                    XmlElement _secondary_odu_signal_type = commonXml.CreateElement("client-signal-type");
                    _secondary_odu_signal_type.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _secondary_odu_signal_type.InnerText = _secondary_odu;
                    secondary_nni.AppendChild(_secondary_odu_signal_type);

                    //交换类型
                    XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                    _secondary_switch_capability.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _secondary_switch_capability.InnerText = _secondary_switch;
                    secondary_nni.AppendChild(_secondary_switch_capability);

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-spec");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                   // sdh_signal_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);



                    string[] strArray_B = _mapping_path_B.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }


                if (_primary_nni_name2 != "无")
                {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni-2");
                    create_sdh_connection.AppendChild(primary_nni);
                    //VC 类型
                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    // vc_type_A.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A2;
                    primary_nni.AppendChild(vc_type_A);

                    //PTP接口配置
                    XmlElement _nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _nni_ptp_name.InnerText = _primary_nni_name2;
                    primary_nni.AppendChild(_nni_ptp_name);

                    //时隙配置
                    XmlElement _nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _nni_ts_detail.InnerText = _primary_ts2;
                    primary_nni.AppendChild(_nni_ts_detail);

                    //净荷类型
                    XmlElement _adaptation_type = commonXml.CreateElement("adaptation-type");
                    // _adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _adaptation_type.InnerText = _primary_ada2;
                    primary_nni.AppendChild(_adaptation_type);

                    //ODU类型
                    XmlElement _odu_signal_type = commonXml.CreateElement("client-signal-type");
                    _odu_signal_type.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _odu_signal_type.InnerText = _primary_odu2;
                    primary_nni.AppendChild(_odu_signal_type);

                    //交换类型
                    XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
                    _switch_capability.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _switch_capability.InnerText = _primary_switch2;
                    primary_nni.AppendChild(_switch_capability);
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-spec");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    //sdh_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A2;
                    sdh_ftp.AppendChild(sdh_signal_type);



                    string[] strArray_A = _mapping_path_A2.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



                if (_secondary_nni_name2 != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni-2");
                    create_sdh_connection.AppendChild(secondary_nni);

                    //PTP接口配置
                    XmlElement _secondary_nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _secondary_nni_ptp_name.InnerText = _secondary_nni_name2;
                    secondary_nni.AppendChild(_secondary_nni_ptp_name);
                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B2;
                    secondary_nni.AppendChild(vc_type_B);
                    //时隙配置
                    XmlElement _secondary_nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _secondary_nni_ts_detail.InnerText = _secondary_ts2;
                    secondary_nni.AppendChild(_secondary_nni_ts_detail);

                    //净荷类型
                    XmlElement _secondary_adaptation_type = commonXml.CreateElement("adaptation-type");
                    //_secondary_adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    _secondary_adaptation_type.InnerText = _secondary_ada2;
                    secondary_nni.AppendChild(_secondary_adaptation_type);

                    //ODU类型
                    XmlElement _secondary_odu_signal_type = commonXml.CreateElement("client-signal-type");
                    _secondary_odu_signal_type.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _secondary_odu_signal_type.InnerText = _secondary_odu2;
                    secondary_nni.AppendChild(_secondary_odu_signal_type);

                    //交换类型
                    XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                    _secondary_switch_capability.SetAttribute("xmlns:otn-types", "urn:ietf:params:xml:ns:yang:ietf-otn-types");
                    _secondary_switch_capability.InnerText = _secondary_switch2;
                    secondary_nni.AppendChild(_secondary_switch_capability);

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-spec");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                    // sdh_signal_type_B.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B2;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);



                    string[] strArray_B = _mapping_path_B2.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("ctp-mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



            }
            if (ips.Contains("电信"))
            {
                //服务类型
                XmlElement service_type = commonXml.CreateElement("service-type");
                service_type.InnerText = _service_type;
                create_sdh_connection.AppendChild(service_type);
                //层协议
                XmlElement layer_protocol_name = commonXml.CreateElement("layer-protocol-name");
                layer_protocol_name.SetAttribute("xmlns:acc-sdh", "urn:ccsa:yang:acc-sdh");
                layer_protocol_name.InnerText = _layer_protoco_name;
                create_sdh_connection.AppendChild(layer_protocol_name);



                //带宽大小
                XmlElement requested_capacity = commonXml.CreateElement("requested-capacity");
                create_sdh_connection.AppendChild(requested_capacity);



                XmlElement total_size = commonXml.CreateElement("total-size");
                total_size.InnerText = _total_size;
                requested_capacity.AppendChild(total_size);
                if (!_uni_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement uni_protection_type = commonXml.CreateElement("uni-protection-type");
                    uni_protection_type.SetAttribute("xmlns:acc-pg", "urn:ccsa:yang:acc-protection-group");
                    uni_protection_type.InnerText = _uni_protection_type;
                    create_sdh_connection.AppendChild(uni_protection_type);
                }

                if (!_nni_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement nni_protection_type = commonXml.CreateElement("nni-protection-type");
                    nni_protection_type.SetAttribute("xmlns:acc-pg", "urn:ccsa:yang:acc-protection-group");
                    nni_protection_type.InnerText = _nni_protection_type;
                    create_sdh_connection.AppendChild(nni_protection_type);
                }

                if (!_nni2_protection_type.Contains("无"))
                {
                    //保护类型
                    XmlElement nni2_protection_type = commonXml.CreateElement("nni2-protection-type");
                    nni2_protection_type.SetAttribute("xmlns:acc-pg", "urn:ccsa:yang:acc-protection-group");
                    nni2_protection_type.InnerText = _nni2_protection_type;
                    create_sdh_connection.AppendChild(nni2_protection_type);
                }

                if (_uni_ptp_name != "无") {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    vc_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }
                }
                if (_uni_ptp_name2 != "无")
                {
                    //客户侧配置
                    XmlElement sdh_uni = commonXml.CreateElement("second-sdh-uni");
                    create_sdh_connection.AppendChild(sdh_uni);

                    //PTP接口配置
                    XmlElement uni_ptp_name = commonXml.CreateElement("uni-ptp-name");
                    uni_ptp_name.InnerText = _uni_ptp_name2;
                    sdh_uni.AppendChild(uni_ptp_name);

                    //VC类型
                    XmlElement vc_type = commonXml.CreateElement("vc-type");
                    vc_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type.InnerText = _uni_vc_type2;
                    sdh_uni.AppendChild(vc_type);

                    string[] strArray = _uni_mapping_path2.Split(',');
                    foreach (var item in strArray)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_uni.AppendChild(mapping_path);
                        }

                    }
                }


                if (_primary_nni_name != "无") {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni");
                    create_sdh_connection.AppendChild(primary_nni);
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
                    _adaptation_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _adaptation_type.InnerText = _primary_ada;
                    primary_nni.AppendChild(_adaptation_type);

                    //ODU类型
                    XmlElement _odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _odu_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _odu_signal_type.InnerText = _primary_odu;
                    primary_nni.AppendChild(_odu_signal_type);

                    //交换类型
                    XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
                    _switch_capability.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _switch_capability.InnerText = _primary_switch;
                    primary_nni.AppendChild(_switch_capability);
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-ftp");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    sdh_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A;
                    sdh_ftp.AppendChild(sdh_signal_type);

                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    vc_type_A.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A;
                    sdh_ftp.AppendChild(vc_type_A);

                    string[] strArray_A = _mapping_path_A.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }



                if (_secondary_nni_name != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni");
                    create_sdh_connection.AppendChild(secondary_nni);
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
                    _secondary_adaptation_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_adaptation_type.InnerText = _secondary_ada;
                    secondary_nni.AppendChild(_secondary_adaptation_type);

                    //ODU类型
                    XmlElement _secondary_odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _secondary_odu_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_odu_signal_type.InnerText = _secondary_odu;
                    secondary_nni.AppendChild(_secondary_odu_signal_type);

                    //交换类型
                    XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                    _secondary_switch_capability.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_switch_capability.InnerText = _secondary_switch;
                    secondary_nni.AppendChild(_secondary_switch_capability);

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-ftp");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                    sdh_signal_type_B.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);

                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B;
                    sdh_ftp_B.AppendChild(vc_type_B);

                    string[] strArray_B = _mapping_path_B.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }


                if (_primary_nni_name2 != "无")
                {
                    //线路侧配置
                    XmlElement primary_nni = commonXml.CreateElement("primary-nni2");
                    create_sdh_connection.AppendChild(primary_nni);
                    //PTP接口配置
                    XmlElement _nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _nni_ptp_name.InnerText = _primary_nni_name2;
                    primary_nni.AppendChild(_nni_ptp_name);

                    //时隙配置
                    XmlElement _nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _nni_ts_detail.InnerText = _primary_ts2;
                    primary_nni.AppendChild(_nni_ts_detail);

                    //净荷类型
                    XmlElement _adaptation_type = commonXml.CreateElement("adaptation-type");
                    _adaptation_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _adaptation_type.InnerText = _primary_ada2;
                    primary_nni.AppendChild(_adaptation_type);

                    //ODU类型
                    XmlElement _odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _odu_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _odu_signal_type.InnerText = _primary_odu2;
                    primary_nni.AppendChild(_odu_signal_type);

                    //交换类型
                    XmlElement _switch_capability = commonXml.CreateElement("switch-capability");
                    _switch_capability.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _switch_capability.InnerText = _primary_switch2;
                    primary_nni.AppendChild(_switch_capability);
                    //SDH属性配置
                    XmlElement sdh_ftp = commonXml.CreateElement("sdh-ftp");
                    primary_nni.AppendChild(sdh_ftp);
                    //SDH 信号类型
                    XmlElement sdh_signal_type = commonXml.CreateElement("sdh-type");
                    sdh_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type.InnerText = _sdh_signal_type_A2;
                    sdh_ftp.AppendChild(sdh_signal_type);

                    //VC类型
                    XmlElement vc_type_A = commonXml.CreateElement("vc-type");
                    vc_type_A.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type_A.InnerText = _vc_type_A2;
                    sdh_ftp.AppendChild(vc_type_A);

                    string[] strArray_A = _mapping_path_A2.Split(',');
                    foreach (var item in strArray_A)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }
                if (_secondary_nni_name2 != "无")
                {
                    //线路侧配置 备接口
                    XmlElement secondary_nni = commonXml.CreateElement("secondary-nni2");
                    create_sdh_connection.AppendChild(secondary_nni);
                    //PTP接口配置
                    XmlElement _secondary_nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
                    _secondary_nni_ptp_name.InnerText = _secondary_nni_name2;
                    secondary_nni.AppendChild(_secondary_nni_ptp_name);

                    //时隙配置
                    XmlElement _secondary_nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
                    _secondary_nni_ts_detail.InnerText = _secondary_ts2;
                    secondary_nni.AppendChild(_secondary_nni_ts_detail);

                    //净荷类型
                    XmlElement _secondary_adaptation_type = commonXml.CreateElement("adaptation-type");
                    _secondary_adaptation_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_adaptation_type.InnerText = _secondary_ada2;
                    secondary_nni.AppendChild(_secondary_adaptation_type);

                    //ODU类型
                    XmlElement _secondary_odu_signal_type = commonXml.CreateElement("odu-signal-type");
                    _secondary_odu_signal_type.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_odu_signal_type.InnerText = _secondary_odu2;
                    secondary_nni.AppendChild(_secondary_odu_signal_type);

                    //交换类型
                    XmlElement _secondary_switch_capability = commonXml.CreateElement("switch-capability");
                    _secondary_switch_capability.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    _secondary_switch_capability.InnerText = _secondary_switch2;
                    secondary_nni.AppendChild(_secondary_switch_capability);

                    //SDH属性配置
                    XmlElement sdh_ftp_B = commonXml.CreateElement("sdh-ftp");
                    secondary_nni.AppendChild(sdh_ftp_B);
                    //SDH 信号类型
                    XmlElement sdh_signal_type_B = commonXml.CreateElement("sdh-type");
                    sdh_signal_type_B.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    sdh_signal_type_B.InnerText = _sdh_signal_type_B2;
                    sdh_ftp_B.AppendChild(sdh_signal_type_B);

                    //VC类型
                    XmlElement vc_type_B = commonXml.CreateElement("vc-type");
                    vc_type_B.SetAttribute("xmlns:acc-enum", "urn:ccsa:yang:acc-otn-types");
                    vc_type_B.InnerText = _vc_type_B2;
                    sdh_ftp_B.AppendChild(vc_type_B);

                    string[] strArray_B = _mapping_path_B2.Split(',');
                    foreach (var item in strArray_B)
                    {
                        if (item != "")
                        {
                            XmlElement mapping_path = commonXml.CreateElement("mapping-path");
                            mapping_path.InnerText = item;
                            sdh_ftp_B.AppendChild(mapping_path);
                        }

                    }
                    //时隙
                }


            }
            return commonXml;
        }


    }
}
