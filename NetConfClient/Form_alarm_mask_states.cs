using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace NetConfClientSoftware
{
    public partial class Form_alarm_mask_states : Form
    {
        public Form_alarm_mask_states()
        {
            InitializeComponent();
        }
        int id = 0;
        string ip = "";
        public Form_alarm_mask_states(int _id,string _ip, string _name, string _type) : this()
        {
            if (true)
            {
                comboBoxObjectName.Text = _name;
                comboBoxObjectType.Text = _type;
                id = _id;
                ip = _ip;
            }
        }
        private void ButtonFindMaskStates_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewAlarmMaskStates.Rows.Clear();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = GetXML.AlarmsMaskStates(comboBoxObjectName.Text, comboBoxObjectType.Text);
                xmlDoc.LoadXml(RPC.Send(xmlDoc, id, ip));
                XmlNamespaceManager root = new XmlNamespaceManager(xmlDoc.NameTable);
                root.AddNamespace("rpc", "urn:ietf:params:xml:ns:netconf:base:1.0");
                root.AddNamespace("alarms", "urn:ccsa:yang:acc-alarms");
                XmlNodeList itemNodes = xmlDoc.SelectNodes("//alarms:alarm-mask-states//alarms:alarm-mask-state", root);
                foreach (XmlNode itemNode in itemNodes)
                {
                    int index = dataGridViewAlarmMaskStates.Rows.Add();
                    XmlNode object_name = itemNode.SelectSingleNode("alarms:object-name", root);
                    XmlNode object_type = itemNode.SelectSingleNode("alarms:object-type", root);
                    XmlNode mask_state = itemNode.SelectSingleNode("alarms:mask-state", root);
                    XmlNode alarm_code = itemNode.SelectSingleNode("alarms:alarm-code", root);
                    if (object_name != null) { dataGridViewAlarmMaskStates.Rows[index].Cells["对象名称"].Value = object_name.InnerText; }
                    if (object_type != null) { dataGridViewAlarmMaskStates.Rows[index].Cells["对象类型"].Value = object_type.InnerText; }
                    if (mask_state != null) { dataGridViewAlarmMaskStates.Rows[index].Cells["抑制状态"].Value = mask_state.InnerText; }
                    if (alarm_code != null) { dataGridViewAlarmMaskStates.Rows[index].Cells["告警编码"].Value = alarm_code.InnerText; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());   //读取该节点的相关信息
            }
        }

        private void buttonSetMaskStates_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc = ModifyXML.AlarmsMaskState(comboBoxObjectName.Text, comboBoxObjectType.Text, comboBoxAlarmCode.Text, comboBoxMaskStates.Text);
            var str = RPC.Send(xmlDoc, id, ip);
            if ((!str.Contains("erro"))&&(str!=""))
            {
                MessageBox.Show("下发成功，请查询确认");
            }
            else {
                MessageBox.Show("下发失败，请检查");
            }
            buttonFindMaskStates.PerformClick();
        }

        private void dataGridViewAlarmMaskStates_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                foreach (DataGridViewRow row in this.dataGridViewAlarmMaskStates.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        comboBoxMaskStates.Text = dataGridViewAlarmMaskStates.Rows[row.Index].Cells["抑制状态"].Value.ToString();
                        if (comboBoxMaskStates.Text == "false") {
                            comboBoxMaskStates.Text = "true";
                        }
                        else {
                            comboBoxMaskStates.Text = "false";
                        }
                        comboBoxObjectName.Text = dataGridViewAlarmMaskStates.Rows[row.Index].Cells["对象名称"].Value.ToString();
                        comboBoxObjectType.Text = dataGridViewAlarmMaskStates.Rows[row.Index].Cells["对象类型"].Value.ToString();
                        comboBoxAlarmCode.Text = dataGridViewAlarmMaskStates.Rows[row.Index].Cells["告警编码"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
