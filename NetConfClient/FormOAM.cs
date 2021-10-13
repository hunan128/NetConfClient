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
    public partial class FormOAM : Form
    {
        public FormOAM()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public FormOAM(string _name, string _server_tp, string _vlan_id, string _vlan_type, string _dm_state, string _tm_state, string _lm_state, string _cc_state,
            string _mep_id, string _remote_mep_id, string _meg_id, string _md_name, string _mel, string _cc_interval, string _lm_interval, string _dm_interval,
    string _delay, string _near_loss, string _far_loss, string _tx_bytes, string _rx_bytes) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = _name;
            if (true)
            {
                ComName.Text = _name;
                comPtp.Text = _server_tp;

                comVlan.Text = _vlan_id;
                comVlantype.Text = _vlan_type;
                comMepid.Text = _mep_id;
                comRemoteMepid.Text = _remote_mep_id;
                comMegid.Text = _meg_id;
                comMdname.Text = _md_name;
                comMel.Text = _mel;
                comCcinterval.Text = _cc_interval;
                comLminterval.Text = _lm_interval;
                comDminterval.Text = _dm_interval;
                comDmstate.Text = _dm_state;
                comTmstate.Text = _tm_state;
                comLmstate.Text = _lm_state;
                comCcstate.Text = _cc_state;
                comtxbtye.Text = _tx_bytes;
                comRxbyte.Text = _rx_bytes;
                comfarloss.Text = _far_loss;
                comnearloss.Text = _near_loss;
                comdelay.Text = _delay;
            }
        }
        public string _name
        {
            get { return ComName.Text; }
        }
        public string _mep_id
        {
            get { return comMepid.Text; }
        }
        public string _remote_mep_id
        {
            get { return comRemoteMepid.Text; }
        }
        public string _meg_id
        {
            get { return comMegid.Text; }
        }
        public string _md_name
        {
            get { return comMdname.Text; }
        }
        public string _mel
        {
            get { return comMel.Text; }
        }
        public string _cc_interval
        {
            get { return comCcinterval.Text; }
        }
        public string _lm_interval
        {
            get { return comLminterval.Text; }
        }
        public string _dm_interval
        {
            get { return comDminterval.Text; }
        }
        public string _dm_state
        {
            get { return comDmstate.Text; }
        }
        public string _tm_state
        {
            get { return comTmstate.Text; }
        }
        public string _lm_state
        {
            get { return comLmstate.Text; }
        }
        public string _cc_state
        {
            get { return comCcstate.Text; }
        }
    }
}
