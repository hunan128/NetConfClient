﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    public partial class LoginOn : Form
    {
        public LoginOn()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="ip">待修改的信息</param>
        public LoginOn(string ip,int port,string user,string passd,string version) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = ip;
            if (true)
            {
                Comip.Text=ip;
                TextPort.Text = port.ToString();
                TextUser.Text = user;
                Textpasd.Text = passd;
                ComVersion.Text = version;
            }
        }
        /// <summary>
        ///     获取修改后的信息
        /// </summary>
        public string IP
        {
            get { return Comip.Text; }
        }
        public int PORT
        {
            get { return int.Parse(TextPort.Text); }
        }
        public string USER
        {
            get { return TextUser.Text; }
        }
        public string PASSD
        {
            get { return Textpasd.Text; }
        }
        public string VER
        {
            get { return ComVersion.Text; }
        }
    }
}
