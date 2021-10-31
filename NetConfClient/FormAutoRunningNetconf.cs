using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    public partial class FormAutoRunningNetconf : Form
    {
        public FormAutoRunningNetconf()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="ip">待修改的信息</param>
        public FormAutoRunningNetconf(string ip) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = ip;
            if (true)
            {
                ipaddress.Text = ip;


            }
        }
        /// <summary>
        ///     获取修改后的信息
        /// </summary>
        public string Ip {
            get { return ipaddress.Text; }
        } 
        public string Mode
        {
            get { return ComMode.Text; }
        }
        public string Title
        {
            get { return ComTitle.Text; }
        }
        public string Ips
        {
            get { return comips.Text; }
        }
        public string Xml
        {
            get { return TextXml.Text; }
        }
        public string Result
        {
            get { return TextResult.Text; }
        }
        public string Req
        {
            get { return TextReq.Text; }
        }
    }
}
