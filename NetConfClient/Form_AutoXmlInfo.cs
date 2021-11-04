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
    public partial class Form_AutoXmlInfo : Form
    {
        public Form_AutoXmlInfo()
        {
            InitializeComponent();
        }

        public Form_AutoXmlInfo(string Model, string Title, string IPS, string RPC, string Exp, string Rx, string Reply, string StartTime, string StopTime, string Dtime, string Recommend) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = Title;
            if (true)
            {
                textBoxModel.Text = Model;
                textBoxtitle.Text = Title;
                textBoxips.Text = IPS;
                textBoxstarttime.Text = StartTime;
                textBoxstoptime.Text = StopTime;
                textBoxtime.Text = Dtime;
                textBoxRecommod.Text = Recommend;
                richTextBoxRpc.Text = RPC;
                richTextBoxReply.Text = Reply;

                string[] ExpByte = Exp.Split(',');
                string[] RxByte = Rx.Split(',');

                for (int i = 0; i < ExpByte.Length; i++)
                {
                    int index = dataGridViewExpRx.Rows.Add();
                    dataGridViewExpRx.Rows[index].Cells["预期"].Value = ExpByte[i];
                    string[] RxByte0 = RxByte[i].Split('=');

                    dataGridViewExpRx.Rows[index].Cells["结果"].Value = RxByte0[1];
                    if (RxByte[i].Contains("NOK")) {
                        dataGridViewExpRx.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                   else
                    {
                        dataGridViewExpRx.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                    }

                }

            }
        }


        private void Form_AutoXmlInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
