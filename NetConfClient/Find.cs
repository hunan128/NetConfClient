using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware
{
    class Find
    {
        public static XmlDocument Connections()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //配置connections
            XmlElement connections = commonXml.CreateElement("connections");
            connections.SetAttribute("xmlns", "urn:ccsa:yang:acc-connection");
            connections.SetAttribute("xmlns:acc-con", "urn:ccsa:yang:acc-connection");

            filter.AppendChild(connections);

          

            return commonXml;

        }


        public static XmlDocument ME()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);
            //配置connections
            XmlElement me = commonXml.CreateElement("me");
            me.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            filter.AppendChild(me);
            return commonXml;

        }
        public static XmlDocument EQS()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);
            //配置connections
            XmlElement eqs = commonXml.CreateElement("eqs");
            eqs.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            filter.AppendChild(eqs);
            return commonXml;

        }
        public static XmlDocument FindGetAll()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            



            return commonXml;

        }
        public static XmlDocument FindPerformances()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //performances
            XmlElement performances = commonXml.CreateElement("performances");
            performances.SetAttribute("xmlns", "urn:ccsa:yang:acc-performance");
            filter.AppendChild(performances);

            return commonXml;

        }

        public static XmlDocument FindPerformance(string _object_name, string _granularity)
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //performances
            XmlElement performances = commonXml.CreateElement("performances");
            performances.SetAttribute("xmlns", "urn:ccsa:yang:acc-performance");
            filter.AppendChild(performances);

            //performance
            XmlElement performance = commonXml.CreateElement("performance");
            performances.AppendChild(performance);

            //对象名称
            if (_object_name != "") {
                XmlElement object_name = commonXml.CreateElement("object-name");
                object_name.InnerText = _object_name;
                performance.AppendChild(object_name);
            }

            //周期
            if (_granularity != "") {
                XmlElement granularity = commonXml.CreateElement("granularity");
                granularity.InnerText = _granularity;
                performance.AppendChild(granularity);
            }


            return commonXml;

        }

        public static XmlDocument FindHisPerformance(string _start_time, string _end_time,string _granularity ,string _object_name,string _pm_parameter_name)
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
            XmlElement get = commonXml.CreateElement("get-history-performance-monitoring-data");
            get.SetAttribute("xmlns", "urn:ccsa:yang:acc-performance");
            rpc.AppendChild(get);
            //开始键
            XmlElement start_time = commonXml.CreateElement("start-time");
            start_time.InnerText = _start_time;
            get.AppendChild(start_time);

            //结束时间
            XmlElement end_time = commonXml.CreateElement("end-time");
            end_time.InnerText = _end_time;
            get.AppendChild(end_time);
            //周期
            XmlElement granularity = commonXml.CreateElement("granularity");
            granularity.InnerText = _granularity;
            get.AppendChild(granularity);
            //对象名称
            if (_object_name != "") {
                XmlElement object_name = commonXml.CreateElement("object-name");
                object_name.InnerText = _object_name;
                get.AppendChild(object_name);
            }

            //性能名称
            if (_pm_parameter_name != "") {
                XmlElement pm_parameter_name = commonXml.CreateElement("pm-parameter-name");
                pm_parameter_name.InnerText = _pm_parameter_name;
                get.AppendChild(pm_parameter_name);
            }



            return commonXml;

        }



        public static XmlDocument FindPgs()
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //performances
            XmlElement performances = commonXml.CreateElement("pgs");
            performances.SetAttribute("xmlns", "urn:ccsa:yang:acc-protection-group");
            filter.AppendChild(performances);

            return commonXml;

        }


        public static XmlDocument PtpsFtpsCtps(bool _ptps,bool _ftps,bool _ctps)
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //CTP
            if (_ctps) {
                XmlElement ctps = commonXml.CreateElement("ctps");
                ctps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                filter.AppendChild(ctps);
            }

            //ftp
            if (_ftps) {
                XmlElement ftps = commonXml.CreateElement("ftps");
                ftps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                filter.AppendChild(ftps);
            }

            //CTP
            if (_ptps) {
                XmlElement ptps = commonXml.CreateElement("ptps");
                ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
                filter.AppendChild(ptps);
            }


            return commonXml;

        }

        public static XmlDocument PTPS(string _name)
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //CTP
            XmlElement ptps = commonXml.CreateElement("ptps");
            ptps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            filter.AppendChild(ptps);

            //CTP
            XmlElement ptp = commonXml.CreateElement("ptp");
            ptp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            ptps.AppendChild(ptp);
            if (!_name.Contains("无")) {
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                ptp.AppendChild(name);
            }


            return commonXml;

        }

        public static XmlDocument CTP(string _name)
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
            XmlElement get = commonXml.CreateElement("get");
            rpc.AppendChild(get);
            //创建条件
            XmlElement filter = commonXml.CreateElement("filter");
            filter.SetAttribute("type", "subtree");
            get.AppendChild(filter);


            //CTP
            XmlElement ctps = commonXml.CreateElement("ctps");
            ctps.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            filter.AppendChild(ctps);

            //CTP
            XmlElement ctp = commonXml.CreateElement("ctp");
            ctp.SetAttribute("xmlns", "urn:ccsa:yang:acc-devm");
            ctps.AppendChild(ctp);
            if (!_name.Contains("无"))
            {
                XmlElement name = commonXml.CreateElement("name");
                name.InnerText = _name;
                ctp.AppendChild(name);
            }


            return commonXml;

        }

    }
}
