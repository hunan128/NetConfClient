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
    public partial class FormSaveXML : Form
    {
        public FormSaveXML()
        {
            InitializeComponent();
        }
        public FormSaveXML(string _name) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
           // this.textBoxXmlName.Text = _name;

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
        }
        public string _name
        {
            get {
                return comboBoxIPS.Text + comboBoxType.Text + textBoxXmlName.Text+label2.Text;
            }
        }
    }

}
