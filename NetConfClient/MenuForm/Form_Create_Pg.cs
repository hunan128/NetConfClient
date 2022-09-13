using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetConfClientSoftware.MenuForm
{
    public partial class Form_Create_Pg : Form
    {
        public Form_Create_Pg()
        {
            InitializeComponent();
        }
        public Form_Create_Pg(string _pg_id, string _create_type, string _delete_cascade, string _protection_type,
            string _reversion_mode, string _switch_type, string _hold_off, string _wait_to_restore_time, 
            string _sd_trigger, string _primary_port, string _secondary_port, string pro ) : this()
        {
            if (true)
            {
                string[] strArray = _primary_port.Split(',');
                foreach (var item in strArray)
                {
                    if (item != "")
                    {
                        comboBox_primary_port.Items.Add(item);
                        comboBox_secondary_port.Items.Add(item);
                    }

                }
                comboBox_primary_port.SelectedIndex = 0;
                comboBox_secondary_port.SelectedIndex = 0;
                string[] protection_type = _protection_type.Split(',');
                foreach (var item in protection_type)
                {
                    if (item != "")
                    {
                        comboBox_protection_type.Items.Add(item);
                        if (item.Contains(pro) || item.Contains(pro)) {
                            comboBox_protection_type.Text = item;
                        }
                        if (item.Contains(pro) || item.Contains(pro))
                        {
                            comboBox_protection_type.Text = item;
                        }
                    }

                }
                comboBox_create_type.SelectedIndex = 1;
                comboBox_delete_cascade.SelectedIndex = 1;
                comboBox_reversion_mode.SelectedIndex = 0;
                comboBox_switch_type.SelectedIndex = 0;
                comboBox_sd_trigger.SelectedIndex = 0;

            }
        }
        public string _pg_id
        {
            get { return comboBox_pg_id.Text; }
        }
        public string _create_type
        {
            get { return comboBox_create_type.Text; }
        }
        public string _delete_cascade
        {
            get { return comboBox_delete_cascade.Text; }
        }
        public string _protection_type
        {
            get { return comboBox_protection_type.Text; }
        }
        public string _reversion_mode
        {
            get { return comboBox_reversion_mode.Text; }
        }
        public string _switch_type
        {
            get { return comboBox_switch_type.Text; }
        }
        public string _hold_off
        {
            get { return comboBox_hold_off.Text; }
        }
        public string _wait_to_restore_time
        {
            get { return comboBox_WTR.Text; }
        }
        public string _sd_trigger
        {
            get { return comboBox_sd_trigger.Text; }
        }
        public string _primary_port
        {
            get { return comboBox_primary_port.Text; }
        }
        public string _secondary_port
        {
            get { return comboBox_secondary_port.Text; }
        }

    }
}
