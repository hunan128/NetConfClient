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
    public partial class Form_Connection_Rate : Form
    {
        public Form_Connection_Rate()
        {
            InitializeComponent();
        }
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public Form_Connection_Rate(string _name, string _total_size, string _cir, string _pir,string _cbs,string _pbs) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = this.Text+"= "+ _name;
            if (true)
            {
                comboBoxCon.Text = _name;
                textBoxTot.Text = _total_size;

                textBoxCir.Text = _cir;
                textBoxPir.Text = _pir;
                textBoxCbs.Text = _cbs;
                textBoxPbs.Text = _pbs;

            }
        }
        public string _name
        {
            get { return comboBoxCon.Text; }
        }
        public string _total_size
        {
            get { return textBoxTot.Text; }
        }
        public string _cir
        {
            get { return textBoxCir.Text; }
        }
        public string _pir
        {
            get { return textBoxPir.Text; }
        }
        public string _cbs
        {
            get { return textBoxCbs.Text; }
        }
        public string _pbs
        {
            get { return textBoxPbs.Text; }
        }
    }
}
