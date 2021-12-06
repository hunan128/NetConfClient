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
    public partial class Form_ODUk_Delay : Form
    {
        public Form_ODUk_Delay()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="name">待修改的信息</param>
        public Form_ODUk_Delay(string _name,string _odu_delay_enable,string _delay,string _last_update_time,string _odu,string _ada,string _swi, string _pmtx, string _pmexp, string _pmrx) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = _name;
            if (true)
            {
                string[] strArray = _name.Split(',');
                foreach (var item in strArray)
                {
                    if (item != "")
                    {
                        if (!item.Contains("FTP"))
                        {
                            ComCTP.Items.Add(item);
                            ComCTP.SelectedIndex = 0;
                        }

                    }

                }
                ComDelay_Enable.Text = _odu_delay_enable;
                ComDelay.Text = _delay;
                ComLastUpdateTime.Text = _last_update_time;
                Textada.Text = _ada;
                Textswitch.Text = _swi;
                Textodu.Text = _odu;
                Textpmtx.Text = _pmtx;
                Textpmexp.Text = _pmexp;
                Textpmrx.Text = _pmrx;
            }
        }
        public string _name
        {
            get { return ComCTP.Text; }
        }
        public string _odu_delay_enable
        {
            get { return ComDelay_Enable.Text; }
        }


    }
}
