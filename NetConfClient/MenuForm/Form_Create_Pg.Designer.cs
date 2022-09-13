namespace NetConfClientSoftware.MenuForm
{
    partial class Form_Create_Pg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_pg_id = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_create_type = new System.Windows.Forms.ComboBox();
            this.comboBox_protection_type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_delete_cascade = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_WTR = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_hold_off = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_switch_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_reversion_mode = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_secondary_port = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_primary_port = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_sd_trigger = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.确认 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.创建保护组 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.创建保护组);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 51);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.确认);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 385);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(537, 60);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.comboBox_secondary_port);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.comboBox_primary_port);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.comboBox_sd_trigger);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.comboBox_WTR);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.comboBox_hold_off);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.comboBox_switch_type);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.comboBox_reversion_mode);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.comboBox_protection_type);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.comboBox_delete_cascade);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.comboBox_create_type);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.comboBox_pg_id);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 51);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(537, 334);
            this.panel3.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "保护组ID";
            // 
            // comboBox_pg_id
            // 
            this.comboBox_pg_id.FormattingEnabled = true;
            this.comboBox_pg_id.Location = new System.Drawing.Point(134, 15);
            this.comboBox_pg_id.Name = "comboBox_pg_id";
            this.comboBox_pg_id.Size = new System.Drawing.Size(121, 20);
            this.comboBox_pg_id.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "创建方式";
            // 
            // comboBox_create_type
            // 
            this.comboBox_create_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_create_type.FormattingEnabled = true;
            this.comboBox_create_type.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox_create_type.Location = new System.Drawing.Point(134, 41);
            this.comboBox_create_type.Name = "comboBox_create_type";
            this.comboBox_create_type.Size = new System.Drawing.Size(121, 20);
            this.comboBox_create_type.TabIndex = 3;
            // 
            // comboBox_protection_type
            // 
            this.comboBox_protection_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_protection_type.FormattingEnabled = true;
            this.comboBox_protection_type.Location = new System.Drawing.Point(134, 93);
            this.comboBox_protection_type.Name = "comboBox_protection_type";
            this.comboBox_protection_type.Size = new System.Drawing.Size(121, 20);
            this.comboBox_protection_type.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "保护类型";
            // 
            // comboBox_delete_cascade
            // 
            this.comboBox_delete_cascade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_delete_cascade.FormattingEnabled = true;
            this.comboBox_delete_cascade.Items.AddRange(new object[] {
            "true",
            "false"});
            this.comboBox_delete_cascade.Location = new System.Drawing.Point(134, 67);
            this.comboBox_delete_cascade.Name = "comboBox_delete_cascade";
            this.comboBox_delete_cascade.Size = new System.Drawing.Size(121, 20);
            this.comboBox_delete_cascade.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "级联删除";
            // 
            // comboBox_WTR
            // 
            this.comboBox_WTR.FormattingEnabled = true;
            this.comboBox_WTR.Location = new System.Drawing.Point(134, 197);
            this.comboBox_WTR.Name = "comboBox_WTR";
            this.comboBox_WTR.Size = new System.Drawing.Size(121, 20);
            this.comboBox_WTR.TabIndex = 15;
            this.comboBox_WTR.Text = "300";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "WTR";
            // 
            // comboBox_hold_off
            // 
            this.comboBox_hold_off.FormattingEnabled = true;
            this.comboBox_hold_off.Location = new System.Drawing.Point(134, 171);
            this.comboBox_hold_off.Name = "comboBox_hold_off";
            this.comboBox_hold_off.Size = new System.Drawing.Size(121, 20);
            this.comboBox_hold_off.TabIndex = 13;
            this.comboBox_hold_off.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "hold-off";
            // 
            // comboBox_switch_type
            // 
            this.comboBox_switch_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_switch_type.FormattingEnabled = true;
            this.comboBox_switch_type.Items.AddRange(new object[] {
            "uni-switch",
            "bi-switch"});
            this.comboBox_switch_type.Location = new System.Drawing.Point(134, 145);
            this.comboBox_switch_type.Name = "comboBox_switch_type";
            this.comboBox_switch_type.Size = new System.Drawing.Size(121, 20);
            this.comboBox_switch_type.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "倒换类型";
            // 
            // comboBox_reversion_mode
            // 
            this.comboBox_reversion_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_reversion_mode.FormattingEnabled = true;
            this.comboBox_reversion_mode.Items.AddRange(new object[] {
            "revertive",
            "non-revertive"});
            this.comboBox_reversion_mode.Location = new System.Drawing.Point(134, 119);
            this.comboBox_reversion_mode.Name = "comboBox_reversion_mode";
            this.comboBox_reversion_mode.Size = new System.Drawing.Size(121, 20);
            this.comboBox_reversion_mode.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "返回模式";
            // 
            // comboBox_secondary_port
            // 
            this.comboBox_secondary_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_secondary_port.FormattingEnabled = true;
            this.comboBox_secondary_port.Location = new System.Drawing.Point(134, 275);
            this.comboBox_secondary_port.Name = "comboBox_secondary_port";
            this.comboBox_secondary_port.Size = new System.Drawing.Size(248, 20);
            this.comboBox_secondary_port.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(45, 278);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "备用接口";
            // 
            // comboBox_primary_port
            // 
            this.comboBox_primary_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_primary_port.FormattingEnabled = true;
            this.comboBox_primary_port.Location = new System.Drawing.Point(134, 249);
            this.comboBox_primary_port.Name = "comboBox_primary_port";
            this.comboBox_primary_port.Size = new System.Drawing.Size(248, 20);
            this.comboBox_primary_port.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(46, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "主用端口";
            // 
            // comboBox_sd_trigger
            // 
            this.comboBox_sd_trigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sd_trigger.FormattingEnabled = true;
            this.comboBox_sd_trigger.Items.AddRange(new object[] {
            "enabled",
            "disabled"});
            this.comboBox_sd_trigger.Location = new System.Drawing.Point(134, 223);
            this.comboBox_sd_trigger.Name = "comboBox_sd_trigger";
            this.comboBox_sd_trigger.Size = new System.Drawing.Size(121, 20);
            this.comboBox_sd_trigger.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(46, 226);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 16;
            this.label11.Text = "SD出发倒换";
            // 
            // 确认
            // 
            this.确认.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.确认.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.确认.Location = new System.Drawing.Point(328, 25);
            this.确认.Name = "确认";
            this.确认.Size = new System.Drawing.Size(75, 23);
            this.确认.TabIndex = 0;
            this.确认.Text = "确认";
            this.确认.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(410, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // 创建保护组
            // 
            this.创建保护组.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.创建保护组.AutoSize = true;
            this.创建保护组.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.创建保护组.Location = new System.Drawing.Point(178, 9);
            this.创建保护组.Name = "创建保护组";
            this.创建保护组.Size = new System.Drawing.Size(158, 29);
            this.创建保护组.TabIndex = 0;
            this.创建保护组.Text = "创建保护组";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(274, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "* 数字";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(274, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(143, 12);
            this.label13.TabIndex = 23;
            this.label13.Text = "* 0=创建业务,1=预先创建";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(274, 70);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(221, 12);
            this.label14.TabIndex = 24;
            this.label14.Text = "* true=随业务删除,false=不随业务删除";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(274, 96);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(155, 12);
            this.label15.TabIndex = 25;
            this.label15.Text = "* 只支持PTP端口的OCH与MSP";
            // 
            // Form_Create_Pg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 445);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form_Create_Pg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Create_Pg";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox_WTR;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_hold_off;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_switch_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_reversion_mode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_protection_type;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_delete_cascade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_create_type;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_pg_id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label 创建保护组;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button 确认;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_secondary_port;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox_primary_port;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_sd_trigger;
        private System.Windows.Forms.Label label11;
    }
}