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
    public partial class FormInfo : Form
    {
        public FormInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="info">待修改的信息</param>
        public FormInfo(string info):this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = info;
            if (true)
            {
                int count = 0;
                if (info.Contains("VC-4") || info.Contains("VC4")) { count = 17; }
                if (info.Contains("VC-3")|| info.Contains("VC3")) { count = 4; }
                if (info.Contains("VC-12") || info.Contains("VC12")) { count = 64; }
                if (!info.Contains("VC")) { return; }
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
                }
        }
        ArrayList list = new ArrayList();
        void SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox a = sender as CheckBox;
                if (a.CheckState == CheckState.Checked)
                {
                    if (this.Text.Contains("VC-12")|| this.Text.Contains("VC12")) {
                        //textBox.Text = textBox.Text + VC12(a.Text.Trim()) + ",";
                        list.Add(VC12(a.Text.Trim()));
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));
                    }
                    if (this.Text.Contains("VC-3") || this.Text.Contains("VC3"))
                    {
                        //textBox.Text = textBox.Text +comboBox.Text+"-"+ a.Text.Trim() + ",";
                        list.Add(comboBox.Text + "-" + a.Text.Trim());
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));
                    }
                    if (this.Text.Contains("VC-4") || this.Text.Contains("VC4"))
                    {
                        list.Add(a.Text.Trim());
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));
                        //  textBox.Text = textBox.Text + ","+a.Text.Trim() +",";
                    }

                }
                else if (a.CheckState == CheckState.Unchecked)
                {

                    if (this.Text.Contains("VC-12") || this.Text.Contains("VC12"))
                    {
                        //textBox.Text = textBox.Text.Replace(VC12(a.Text.Trim()) + ",", "");
                        list.Remove(VC12(a.Text.Trim()));
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));
                    }
                    if (this.Text.Contains("VC-3") || this.Text.Contains("VC3"))
                    {
                        //textBox.Text = textBox.Text.Replace(comboBox.Text + "-" + a.Text.Trim() + ",", "");
                        list.Remove(comboBox.Text + "-" + a.Text.Trim());
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));

                    }
                    if (this.Text.Contains("VC-4") || this.Text.Contains("VC4"))
                    {
                        list.Remove(a.Text.Trim());
                        textBox.Text = string.Join(",", (string[])list.ToArray(typeof(string)));
                        //textBox.Text = textBox.Text.Replace(","+a.Text.Trim()+"," , "");
                    }


                    //textBox.Text = a.Text.Trim() + "未选中";
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

                if (ctl is CheckBox)

                {

                    CheckBox chk = ctl as CheckBox;

                    chk.Checked = all.Checked;

                }

            }

        }

        public  string VC12(string a) {
            string hp = comboBox.Text;
            string lp="";
            switch (a) {
                case "1":
                    lp = "1-1-1";
                    break;
                case "2":
                    lp = "2-1-1";
                    break;
                case "3":
                    lp = "3-1-1";
                    break;
                case "4":
                    lp = "1-2-1";
                    break;
                case "5":
                    lp = "2-2-1";
                    break;
                case "6":
                    lp = "3-2-1";
                    break;
                case "7":
                    lp = "1-3-1";
                    break;
                case "8":
                    lp = "2-3-1";
                    break;
                case "9":
                    lp = "3-3-1";
                    break;
                case "10":
                    lp = "1-4-1";
                    break;
                case "11":
                    lp = "2-4-1";
                    break;
                case "12":
                    lp = "3-4-1";
                    break;
                case "13":
                    lp = "1-5-1";
                    break;
                case "14":
                    lp = "2-5-1";
                    break;
                case "15":
                    lp = "3-5-1";
                    break;
                case "16":
                    lp = "1-6-1";
                    break;
                case "17":
                    lp = "2-6-1";
                    break;
                case "18":
                    lp = "3-6-1";
                    break;
                case "19":
                    lp = "1-7-1";
                    break;
                case "20":
                    lp = "2-7-1";
                    break;
                case "21":
                    lp = "3-7-1";
                    break;
                case "22":
                    lp = "1-1-2";
                    break;
                case "23":
                    lp = "2-1-2";
                    break;
                case "24":
                    lp = "3-1-2";
                    break;
                case "25":
                    lp = "1-2-2";
                    break;
                case "26":
                    lp = "2-2-2";
                    break;
                case "27":
                    lp = "3-2-2";
                    break;
                case "28":
                    lp = "1-3-2";
                    break;
                case "29":
                    lp = "2-3-2";
                    break;
                case "30":
                    lp = "3-3-2";
                    break;
                case "31":
                    lp = "1-4-2";
                    break;
                case "32":
                    lp = "2-4-2";
                    break;
                case "33":
                    lp = "3-4-2";
                    break;
                case "34":
                    lp = "1-5-2";
                    break;
                case "35":
                    lp = "2-5-2";
                    break;
                case "36":
                    lp = "3-5-2";
                    break;
                case "37":
                    lp = "1-6-2";
                    break;
                case "38":
                    lp = "2-6-2";
                    break;
                case "39":
                    lp = "3-6-2";
                    break;
                case "40":
                    lp = "1-7-2";
                    break;
                case "41":
                    lp = "2-7-2";
                    break;
                case "42":
                    lp = "3-7-2";
                    break;
                case "43":
                    lp = "1-1-3";
                    break;
                case "44":
                    lp = "2-1-3";
                    break;
                case "45":
                    lp = "3-1-3";
                    break;
                case "46":
                    lp = "1-2-3";
                    break;
                case "47":
                    lp = "2-2-3";
                    break;
                case "48":
                    lp = "3-2-3";
                    break;
                case "49":
                    lp = "1-3-3";
                    break;
                case "50":
                    lp = "2-3-3";
                    break;
                case "51":
                    lp = "3-3-3";
                    break;
                case "52":
                    lp = "1-4-3";
                    break;
                case "53":
                    lp = "2-4-3";
                    break;
                case "54":
                    lp = "3-4-3";
                    break;
                case "55":
                    lp = "1-5-3";
                    break;
                case "56":
                    lp = "2-5-3";
                    break;
                case "57":
                    lp = "3-5-3";
                    break;
                case "58":
                    lp = "1-6-3";
                    break;
                case "59":
                    lp = "2-6-3";
                    break;
                case "60":
                    lp = "3-6-3";
                    break;
                case "61":
                    lp = "1-7-3";
                    break;
                case "62":
                    lp = "2-7-3";
                    break;
                case "63":
                    lp = "3-7-3";
                    break;
 
            }
            lp = hp + "-" + lp;
            return lp;
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
