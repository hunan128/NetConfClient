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
    public partial class Form_Tca_Parameter : Form
    {
        public Form_Tca_Parameter()
        {
            InitializeComponent();
        }
        public Form_Tca_Parameter(string _name, string _pm_parameter_name, string _granularity, string _threshold_type, string _object_type, string _threshold_value, string _input_power , string _output_power, string _input_power_upper_threshold, string _input_power_lower_threshold) : this()
        {
            // 在TextBox中显示信息初值  
            //textBoxInfo.Text = info;
            this.Text = this.Text + "= " + _name;
            if (true)
            {
                comboBoxName.Text = _name;
               
                comboBoxPm.Items.Add(_pm_parameter_name);
                comboBoxPm.SelectedIndex = 0;
                comboBoxGra.Text = _granularity;
                comboBoxThType.Text = _threshold_type;
                comboBoxObjType.Text = _object_type;
                textBoxThValue.Text = _threshold_value;
                textBoxinput.Text = _input_power;
                textBoxoutput.Text = _output_power;
                textBoxhigh.Text = _input_power_upper_threshold;
                textBoxlow.Text = _input_power_lower_threshold;

            }
        }
        public string _name
        {
            get { return comboBoxName.Text; }
        }
        public string _pm_parameter_name
        {
            get { return comboBoxPm.Text; }
        }
        public string _granularity
        {
            get { return comboBoxGra.Text; }
        }
        public string _threshold_type
        {
            get { return comboBoxThType.Text; }
        }
        public string _object_type
        {
            get { return comboBoxObjType.Text; }
        }
        public string _threshold_value
        {
            get { return textBoxThValue.Text; }
        }


    }
}
