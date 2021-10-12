using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class OAM
    {
        public static XmlDocument Create(string _ctp_name,string _mep_id,string _remote_mep_id,string _meg_id,string _md_name,string _mel,string _cc_interval,string _lm_interval,string _dm_interval)
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
            //config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //配置connections
            XmlElement ctps = commonXml.CreateElement("ctps");
            ctps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(ctps);

            //配置connection
            XmlElement ctp = commonXml.CreateElement("ctp");
            ctps.AppendChild(ctp);


            //CTP端口
            XmlElement name = commonXml.CreateElement("name");
            name.InnerText = _ctp_name;
            ctp.AppendChild(name);

            //CTP属性
            XmlElement eth_ctp_pac = commonXml.CreateElement("eth-ctp-pac");
            eth_ctp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
            ctp.AppendChild(eth_ctp_pac);
            //OAMconfig
            XmlElement oam_config = commonXml.CreateElement("oam-config");
            eth_ctp_pac.AppendChild(oam_config);
            //MEP-ID
            XmlElement mep_id = commonXml.CreateElement("mep-id");
            mep_id.InnerText = _mep_id;
            oam_config.AppendChild(mep_id);
            //远端MEP-ID
            XmlElement remote_mep_id = commonXml.CreateElement("remote-mep-id");
            remote_mep_id.InnerText = _remote_mep_id;
            oam_config.AppendChild(remote_mep_id);
            //MGE-ID
            XmlElement meg_id = commonXml.CreateElement("meg-id");
            meg_id.InnerText = _meg_id;
            oam_config.AppendChild(meg_id);
            //md-name
            XmlElement md_name = commonXml.CreateElement("ma-name");
            md_name.InnerText = _md_name;
            oam_config.AppendChild(md_name);
            //mel
            XmlElement mel = commonXml.CreateElement("mel");
            mel.InnerText = _mel;
            oam_config.AppendChild(mel);
            //cc_interval
            XmlElement cc_interval = commonXml.CreateElement("cc-interval");
            cc_interval.InnerText = _cc_interval;
            oam_config.AppendChild(cc_interval);


            //cc_interval
            XmlElement lm_interval = commonXml.CreateElement("lm-interval");
            lm_interval.InnerText = _lm_interval;
            oam_config.AppendChild(lm_interval);

            //cc_interval
            XmlElement dm_interval = commonXml.CreateElement("dm-interval");
            dm_interval.InnerText = _dm_interval;
            oam_config.AppendChild(cc_interval);

            return commonXml;

        }




        public static XmlDocument State(string _ctp_name, string _dm_state, string _tm_state, string _lm_state, string _cc_state)
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
            //config.SetAttribute("xmlns:nc", "urn:ietf:params:xml:ns:netconf:base:1.0");
            edit_config.AppendChild(config);


            //配置connections
            XmlElement ctps = commonXml.CreateElement("ctps");
            ctps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            config.AppendChild(ctps);

            //配置connection
            XmlElement ctp = commonXml.CreateElement("ctp");
            ctps.AppendChild(ctp);


            //CTP端口
            XmlElement name = commonXml.CreateElement("name");
            name.InnerText = _ctp_name;
            ctp.AppendChild(name);

            //CTP属性
            XmlElement eth_ctp_pac = commonXml.CreateElement("eth-ctp-pac");
            eth_ctp_pac.SetAttribute("xmlns", "urn:ccsa:yang:acc-eth");
            ctp.AppendChild(eth_ctp_pac);
            //OAMconfig
            XmlElement oam_state_pac = commonXml.CreateElement("oam-state-pac");
            eth_ctp_pac.AppendChild(oam_state_pac);
            //MEP-ID
            XmlElement dm_state = commonXml.CreateElement("dm-state");
            dm_state.InnerText = _dm_state;
            oam_state_pac.AppendChild(dm_state);
            //远端MEP-ID
            XmlElement tm_state = commonXml.CreateElement("tm-state");
            tm_state.InnerText = _tm_state;
            oam_state_pac.AppendChild(tm_state);
            //MGE-ID
            XmlElement lm_state = commonXml.CreateElement("lm-state");
            lm_state.InnerText = _lm_state;
            oam_state_pac.AppendChild(lm_state);
            //md-name
            XmlElement cc_state = commonXml.CreateElement("cc-state");
            cc_state.InnerText = _cc_state;
            oam_state_pac.AppendChild(cc_state);

            return commonXml;

        }

    }
}
