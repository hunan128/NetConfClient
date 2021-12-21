using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware
{
    public partial class Form_Oduk_TS : Form
    {
        public Form_Oduk_TS()
        {
            InitializeComponent();
        }
        public string SS = "";
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="info">待修改的信息</param>
        public Form_Oduk_TS(string info):this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = info;
            if (true)
            {
                int count = 0;
                if (info.Contains("8-") ) { count = 9; SS = "8-"; }
                if (info.Contains("2-")) { count = 3; ; SS = "2-"; }
                if (info.Contains("1-") ) { count = 2; ; SS = "1-"; }
                if (!info.Contains("-")) { return; }
                CheckBox[] n = new CheckBox[count];
                    for (int i = 1; i < count; i++)
                    {
                        n[i] = new CheckBox();
                        n[i].AutoSize = true;
                    if (i < 10)
                    {
                        n[i].Text = string.Format("{0}{1}", " ", i.ToString());

                    }
                    else {
                        n[i].Text = string.Format("{0}{1}", "", i.ToString());

                    }
                    n[i].CheckedChanged += new EventHandler(SelectedIndexChanged);
                  this.flowLayoutPanel.Controls.Add(n[i]);
                    }

                string [] tsbyte = info.Split('-');
                int num = Int32.Parse(tsbyte[1], System.Globalization.NumberStyles.HexNumber);
                ts = byte.Parse(tsbyte[1], System.Globalization.NumberStyles.HexNumber);
                for (int i = 1; i < 9; i++)
                {
                    if (GetbitValue(ts, 9-i) == 1)
                    {
                        n[i].Enabled = false;
                    }
                    else {
                        if(i<count)
                        n[i].Enabled = true;
                    }
                }
                ts = 0;
            }
        }

        byte ts = 0x00; //0b_1111_1111 或 255
        private static int GetbitValue(byte input, int index)
        {
            int value;
            value = index > 0 ? input >> index - 1 : input;
            return value &= 1;
        }
        /// <summary>
        /// 设置该字节的某一位的值(将该位设置成0或1)
        /// </summary>
        /// <param name="data">要设置的字节byte</param>
        /// <param name="index">要设置的位， 值从低到高为 1-8</param>
        /// <param name="flag">要设置的值 true(1) / false(0)</param>
        /// <returns></returns>
        public static byte SetbitValue(byte data, int index, bool flag)
        {
            if (index > 8 || index < 1)
                throw new ArgumentOutOfRangeException();
            int v = index < 2 ? index : (2 << (index - 2));
            return flag ? (byte)(data | v) : (byte)(data & ~v);
        }
        ArrayList list = new ArrayList();
        void SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox a = sender as CheckBox;
                if (a.CheckState == CheckState.Checked)
                {
                  
                    int i = 9- int.Parse(a.Text);
                    ts = SetbitValue(ts, i, true);
                    textBox.Text = SS + ts.ToString("x2");

                }
                else if (a.CheckState == CheckState.Unchecked)
                {
                    int i = 9 - int.Parse(a.Text);
                    ts = SetbitValue(ts, i, false);
                    if (ts==0)
                    {
                        textBox.Text = "";
                    }
                    else {
                        textBox.Text = SS + ts.ToString("x2");
                    }
                   

                }
            }
            catch 
            {

            }
        }

        /// <summary>
        ///     获取修改后的信息
        /// </summary>
        public string Information
        {
            get { return textBox.Text; }
        }


        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox all = sender as CheckBox;

            foreach (Control ctl in flowLayoutPanel.Controls)

            {

                if (ctl is CheckBox )

                {

                    CheckBox chk = ctl as CheckBox;
                    if(chk.Enabled == true)
                    chk.Checked = all.Checked;

                }

            }

        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBox.Checked = false;
           // CheckBox.CheckedChanged();
        }

        private void Butclear_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
           
        }
    }
}
