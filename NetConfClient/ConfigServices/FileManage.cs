using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetConfClientSoftware.ConfigServices
{
    class FileManage
    {
        public static XmlDocument Upload_File(string _file_type,string _file_name,string _path,string _username,string _password)
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
            XmlElement upload_file = commonXml.CreateElement("upload-file");
            upload_file.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(upload_file);
            //创建文件类型
            XmlElement file_type  = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            upload_file.AppendChild(file_type);
            //创建文件名称
            XmlElement file_name  = commonXml.CreateElement("file-name");
            file_name.InnerText = _file_name;
            upload_file.AppendChild(file_name);
            //创建SFTP路径
            XmlElement path = commonXml.CreateElement("path");
            path.InnerText = _path;
            upload_file.AppendChild(path);
            //创建用户名
            XmlElement username = commonXml.CreateElement("username");
            username.InnerText = _username;
            upload_file.AppendChild(username);
            //创建密码
            XmlElement password = commonXml.CreateElement("password");
            password.InnerText = _password;
            upload_file.AppendChild(password);
            return commonXml;

        }
        public static XmlDocument Download_File(string _file_type, string _file_name, string _file_size,string _path, string _username, string _password)
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
            XmlElement download_file = commonXml.CreateElement("download-file");
            download_file.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(download_file);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            download_file.AppendChild(file_type);
            //创建文件名称
            XmlElement file_name = commonXml.CreateElement("file-name");
            file_name.InnerText = _file_name;
            download_file.AppendChild(file_name);
            //创建文件名称
            XmlElement file_size = commonXml.CreateElement("file-size");
            file_size.InnerText = _file_size;
            download_file.AppendChild(file_size);
            //创建SFTP路径
            XmlElement path = commonXml.CreateElement("path");
            path.InnerText = _path;
            download_file.AppendChild(path);
            //创建用户名
            XmlElement username = commonXml.CreateElement("username");
            username.InnerText = _username;
            download_file.AppendChild(username);
            //创建密码
            XmlElement password = commonXml.CreateElement("password");
            password.InnerText = _password;
            download_file.AppendChild(password);
            return commonXml;

        }

        public static XmlDocument Get_Download_Status(string _file_type, string _file_name)
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
            XmlElement get_download_status = commonXml.CreateElement("get-download-status");
            get_download_status.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(get_download_status);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            get_download_status.AppendChild(file_type);
            //创建文件名称
            XmlElement file_name = commonXml.CreateElement("file-name");
            file_name.InnerText = _file_name;
            get_download_status.AppendChild(file_name);
            return commonXml;

        }

        public static XmlDocument Active_File(string _file_type, string _file_name)
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
            XmlElement active_file = commonXml.CreateElement("active-file");
            active_file.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(active_file);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            active_file.AppendChild(file_type);
            //创建文件名称
            XmlElement file_name = commonXml.CreateElement("file-name");
            file_name.InnerText = _file_name;
            active_file.AppendChild(file_name);
            return commonXml;

        }

        public static XmlDocument Delete_file(string _file_type, string _file_name)
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
            XmlElement delete_file = commonXml.CreateElement("delete-file");
            delete_file.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(delete_file);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            delete_file.AppendChild(file_type);
            //创建文件名称
            XmlElement file_name = commonXml.CreateElement("file-name");
            file_name.InnerText = _file_name;
            delete_file.AppendChild(file_name);
            return commonXml;

        }

        public static XmlDocument Get_File_List(string _file_type)
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
            XmlElement get_file_list = commonXml.CreateElement("get-file-list");
            get_file_list.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(get_file_list);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            get_file_list.AppendChild(file_type);
            return commonXml;

        }

        public static XmlDocument Get_Active_Status(string _file_type)
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
            XmlElement get_active_status = commonXml.CreateElement("get-active-status");
            get_active_status.SetAttribute("xmlns", "urn:ccsa:yang:acc-file");
            rpc.AppendChild(get_active_status);
            //创建文件类型
            XmlElement file_type = commonXml.CreateElement("file-type");
            file_type.InnerText = _file_type;
            get_active_status.AppendChild(file_type);
            return commonXml;

        }
    }
}
