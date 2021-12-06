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
    public partial class Form_Modify_Odu : Form
    {
        public Form_Modify_Odu()
        {
            InitializeComponent();
        }
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public Form_Modify_Odu(string _odu__ctp_name ) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = _odu__ctp_name;
            if (true)
            {
                string[] strArray = _odu__ctp_name.Split(',');
                foreach (var item in strArray)
                {
                    if (item != "")
                    {
                        if (!item.Contains("FTP")) {
                            ComCTP.Items.Add(item);
                            ComCTP.SelectedIndex = 0;
                        }

                    }

                }
            }
        }

        private void ComCTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComCTP.Text.Contains("101"))
            {
                ComPosition.Text = "line-odu";
            }
            else {
                ComPosition.Text = "client-odu";
            }
        }
        public string _odu__ctp_name
        {
            get { return ComCTP.Text; }
        }
        public string _position
        {
            get { return ComPosition.Text; }
        }
        public string _action
        {
            get { return ComAction.Text; }
        }
        public string _current_number_of_tributary_slots
        {
            get { return ComCurrentSlotsNum.Text; }
        }
        public string _ts_detail
        {
            get { return ComTsDetail.Text; }
        }
        public string _timeout
        {
            get { return ComTimeOut.Text; }
        }
        private void ComPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComPosition.Text == "line-odu")
            {
                ComCurrentSlotsNum.Enabled = false;
                ComCurrentSlotsNum.Text = "";
                ComTsDetail.Text = "8-F0";
                ComTsDetail.Enabled = true;
            }
            else {
                ComCurrentSlotsNum.Enabled = true;

                ComTsDetail.Enabled = false;
                ComTsDetail.Text = "";
                ComCurrentSlotsNum.Text = "4";
            }
        }

  
    }
}
