using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class CreatODU
    {
        public static XmlDocument Common(string _label,string _service_type,string _layer_protoco_name,string _total_size,string _nni_protection_type,
            string _client_nni_name,string _client_ts,string _client_ada,string _client_odu,string _client_switch,
            string _primary_nni_name,string _primary_ts,string _primary_ada,string _primary_odu,string _primary_switch,
            string _secondary_nni_name, string _secondary_ts, string _secondary_ada, string _secondary_odu, string _secondary_switch) {
            XmlDocument commonXml = new XmlDocument();
            //  创建XML文档，存在就删除再生成
            XmlDeclaration dec = commonXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            commonXml.AppendChild(dec);
            //  创建根结点
            XmlElement rpc = commonXml.CreateElement("rpc");

            rpc.SetAttribute("message-id","1");
            rpc.SetAttribute("xmlns","urn:ietf:params:xml:ns:netconf:base:1.0");
            commonXml.AppendChild(rpc);

            //创建信息节点
            XmlElement create_odu_connection = commonXml.CreateElement("create-odu-connection");
            create_odu_connection.SetAttribute("xmlns", "urn:ccsa:yang:acc-otn");
            rpc.AppendChild(create_odu_connection);

            //lable
            XmlElement label = commonXml.CreateElement("label");
            label.InnerText = _label;
            create_odu_connection.AppendChild(label);
            //服务类型
            XmlElement service_type = commonXml.CreateElement("service-type");
            service_type.InnerText = _service_type;
            create_odu_connection.AppendChild(service_type);
            //层协议
            XmlElement layer_protocol_name  = commonXml.CreateElement("layer-protocol-name");
            layer_protocol_name.InnerText = _layer_protoco_name;
            create_odu_connection.AppendChild(layer_protocol_name);



            //带宽大小
            XmlElement requested_capacity = commonXml.CreateElement("requested-capacity");
            create_odu_connection.AppendChild(requested_capacity);

            XmlElement total_size = commonXml.CreateElement("total-size");
            total_size.InnerText = _total_size;
            requested_capacity.AppendChild(total_size);

            if (!_nni_protection_type.Contains("无")) {
                //保护类型
                XmlElement nni_protection_type = commonXml.CreateElement("nni-protection-type");
                nni_protection_type.InnerText = _nni_protection_type;
                create_odu_connection.AppendChild(nni_protection_type);
            }


            //客户侧配置
            XmlElement client_side_nni = commonXml.CreateElement("client-side-nni");
            create_odu_connection.AppendChild(client_side_nni);

            //PTP接口配置
            XmlElement nni_ptp_name = commonXml.CreateElement("nni-ptp-name");
            nni_ptp_name.InnerText = _client_nni_name;
            client_side_nni.AppendChild(nni_ptp_name);

            //时隙配置
            XmlElement nni_ts_detail = commonXml.CreateElement("nni-ts-detail");
            nni_ts_detail.InnerText = _client_ts;
            client_side_nni.AppendChild(nni_ts_detail);

            //净荷类型
            XmlElement adaptation_type = commonXml.CreateElement("adaptation-type");
            adaptation_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            adaptation_type.InnerText = _client_ada;
            client_side_nni.AppendChild(adaptation_type);

            //ODU类型
            XmlElement odu_signal_type = commonXml.CreateElement("odu-signal-type");
            odu_signal_type.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            odu_signal_type.InnerText = _client_odu;
            client_side_nni.AppendChild(odu_signal_type);

            //交换类型
            XmlElement switch_capability = commonXml.CreateElement("switch-capability");
            switch_capability.SetAttribute("xmlns:acc-otn-types", "urn:ccsa:yang:acc-otn-types");
            switch_capability.InnerText = _client_switch;
            client_side_nni.AppendChild(switch_capability);





            //线路侧配置
            XmlElement primary_nni = commonXml.CreateElement("primary-nni");
            create_odu_connection.AppendChild(primary_nni);
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




            if (_secondary_nni_name != "无") {
                //线路侧配置 备接口
                XmlElement secondary_nni = commonXml.CreateElement("secondary-nni");
                create_odu_connection.AppendChild(secondary_nni);
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
