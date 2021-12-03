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
    public partial class Form_modify_vcg_connection : Form
    {
        public Form_modify_vcg_connection()
        {
            InitializeComponent();
        }
        public Form_modify_vcg_connection(string _eth_ftp_name, string _sdh_ftp_name, string _sdh_protect_ftp_name, 
            string _vc_type,string _lcas, string _hold_off, string _wtr, string _tsd, string _tx_number, string _rx_number, string _so_handshake_state) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = _eth_ftp_name;
            if (true)
            {
                textBox_eth_ftp.Text = _eth_ftp_name;
                textBox_eth_ftp_name.Text = _eth_ftp_name;
                textBox_sdh_ftp.Text = _sdh_ftp_name;
                textBox_sdh_ftp_p.Text = _sdh_protect_ftp_name;
                if (string.IsNullOrEmpty(_sdh_protect_ftp_name)) {
                    textBox_mapping_path_p.Enabled = false;
                    VC_PATH_P.Enabled = false;
                }
                textBox_vc_type.Text = _vc_type;
                textBox_lcas.Text = _lcas;
                textBox_hold_off.Text = _hold_off;
                textBox_wtr.Text = _wtr;
                textBox_tsd.Text = _tsd;
                textBox_tx_num.Text = _tx_number;
                textBox_rx_num.Text = _rx_number;
                textBox_so_handshake_state.Text = _so_handshake_state;
            }
        }
        private void VC_PATH_Click(object sender, EventArgs e)
        {
            var Form_Info = new Form_Info(textBox_vc_type.Text);
            if (Form_Info.ShowDialog() == DialogResult.OK)
            {
                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                textBox_mapping_path.Text = Form_Info.Information;
            }
        }

        private void VC_PATH_P_Click(object sender, EventArgs e)
        {
            var Form_Info = new Form_Info(textBox_vc_type.Text);
            if (Form_Info.ShowDialog() == DialogResult.OK)
            {
                //如果点击了FromInfo的“确定”按钮，获取修改后的信息并显示
                textBox_mapping_path_p.Text = Form_Info.Information;
            }
        }

        public string _eth_ftp_name
        {
            get { return textBox_eth_ftp.Text; }
        }
        public string _sdh_ftp_name
        {
            get { return textBox_sdh_ftp.Text; }
        }
        public string _sdh_protect_ftp_name
        {
            get { return textBox_sdh_ftp_p.Text; }
        }
        public string _mapping_path
        {
            get { return textBox_mapping_path.Text; }
        }
        public string _mapping_path_protected
        {
            get { return textBox_mapping_path_p.Text; }
        }


    }
}
