namespace NetConfClientSoftware
{
    partial class Form_alarm_mask_states
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxObjectName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxObjectType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxMaskStates = new System.Windows.Forms.ComboBox();
            this.comboBoxAlarmCode = new System.Windows.Forms.ComboBox();
            this.buttonFindMaskStates = new System.Windows.Forms.Button();
            this.buttonSetMaskStates = new System.Windows.Forms.Button();
            this.dataGridViewAlarmMaskStates = new System.Windows.Forms.DataGridView();
            this.对象名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.对象类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.告警编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.抑制状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlarmMaskStates)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "object-name(对象名称)";
            // 
            // comboBoxObjectName
            // 
            this.comboBoxObjectName.FormattingEnabled = true;
            this.comboBoxObjectName.Location = new System.Drawing.Point(158, 12);
            this.comboBoxObjectName.Name = "comboBoxObjectName";
            this.comboBoxObjectName.Size = new System.Drawing.Size(284, 20);
            this.comboBoxObjectName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "object-type(对象类型)";
            // 
            // comboBoxObjectType
            // 
            this.comboBoxObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObjectType.FormattingEnabled = true;
            this.comboBoxObjectType.Items.AddRange(new object[] {
            "PTP",
            "FTP",
            "CTP",
            "EQ",
            "ME"});
            this.comboBoxObjectType.Location = new System.Drawing.Point(158, 44);
            this.comboBoxObjectType.Name = "comboBoxObjectType";
            this.comboBoxObjectType.Size = new System.Drawing.Size(284, 20);
            this.comboBoxObjectType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "alarm-code(告警编码)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "mask-state(抑制状态)";
            // 
            // comboBoxMaskStates
            // 
            this.comboBoxMaskStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaskStates.FormattingEnabled = true;
            this.comboBoxMaskStates.Items.AddRange(new object[] {
            "true",
            "false"});
            this.comboBoxMaskStates.Location = new System.Drawing.Point(158, 101);
            this.comboBoxMaskStates.Name = "comboBoxMaskStates";
            this.comboBoxMaskStates.Size = new System.Drawing.Size(284, 20);
            this.comboBoxMaskStates.TabIndex = 7;
            // 
            // comboBoxAlarmCode
            // 
            this.comboBoxAlarmCode.FormattingEnabled = true;
            this.comboBoxAlarmCode.Location = new System.Drawing.Point(158, 74);
            this.comboBoxAlarmCode.Name = "comboBoxAlarmCode";
            this.comboBoxAlarmCode.Size = new System.Drawing.Size(284, 20);
            this.comboBoxAlarmCode.TabIndex = 6;
            // 
            // buttonFindMaskStates
            // 
            this.buttonFindMaskStates.Location = new System.Drawing.Point(504, 44);
            this.buttonFindMaskStates.Name = "buttonFindMaskStates";
            this.buttonFindMaskStates.Size = new System.Drawing.Size(108, 35);
            this.buttonFindMaskStates.TabIndex = 8;
            this.buttonFindMaskStates.Text = "查询";
            this.buttonFindMaskStates.UseVisualStyleBackColor = true;
            this.buttonFindMaskStates.Click += new System.EventHandler(this.ButtonFindMaskStates_Click);
            // 
            // buttonSetMaskStates
            // 
            this.buttonSetMaskStates.Location = new System.Drawing.Point(504, 86);
            this.buttonSetMaskStates.Name = "buttonSetMaskStates";
            this.buttonSetMaskStates.Size = new System.Drawing.Size(108, 35);
            this.buttonSetMaskStates.TabIndex = 9;
            this.buttonSetMaskStates.Text = "配置";
            this.buttonSetMaskStates.UseVisualStyleBackColor = true;
            this.buttonSetMaskStates.Click += new System.EventHandler(this.buttonSetMaskStates_Click);
            // 
            // dataGridViewAlarmMaskStates
            // 
            this.dataGridViewAlarmMaskStates.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewAlarmMaskStates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewAlarmMaskStates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAlarmMaskStates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewAlarmMaskStates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewAlarmMaskStates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.对象名称,
            this.对象类型,
            this.告警编码,
            this.抑制状态});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAlarmMaskStates.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewAlarmMaskStates.Location = new System.Drawing.Point(3, 127);
            this.dataGridViewAlarmMaskStates.Name = "dataGridViewAlarmMaskStates";
            this.dataGridViewAlarmMaskStates.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAlarmMaskStates.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewAlarmMaskStates.RowHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewAlarmMaskStates.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewAlarmMaskStates.RowTemplate.Height = 23;
            this.dataGridViewAlarmMaskStates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAlarmMaskStates.Size = new System.Drawing.Size(643, 538);
            this.dataGridViewAlarmMaskStates.TabIndex = 10;
            this.dataGridViewAlarmMaskStates.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAlarmMaskStates_CellClick);
            // 
            // 对象名称
            // 
            this.对象名称.HeaderText = "对象名称";
            this.对象名称.Name = "对象名称";
            this.对象名称.ReadOnly = true;
            this.对象名称.Width = 280;
            // 
            // 对象类型
            // 
            this.对象类型.HeaderText = "对象类型";
            this.对象类型.Name = "对象类型";
            this.对象类型.ReadOnly = true;
            // 
            // 告警编码
            // 
            this.告警编码.HeaderText = "告警编码";
            this.告警编码.Name = "告警编码";
            this.告警编码.ReadOnly = true;
            this.告警编码.Width = 140;
            // 
            // 抑制状态
            // 
            this.抑制状态.HeaderText = "抑制状态";
            this.抑制状态.Name = "抑制状态";
            this.抑制状态.ReadOnly = true;
            // 
            // Form_alarm_mask_states
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 668);
            this.Controls.Add(this.dataGridViewAlarmMaskStates);
            this.Controls.Add(this.buttonSetMaskStates);
            this.Controls.Add(this.buttonFindMaskStates);
            this.Controls.Add(this.comboBoxMaskStates);
            this.Controls.Add(this.comboBoxAlarmCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxObjectType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxObjectName);
            this.Controls.Add(this.label1);
            this.Name = "Form_alarm_mask_states";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_alarm_mask_states";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlarmMaskStates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxObjectName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxObjectType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxMaskStates;
        private System.Windows.Forms.ComboBox comboBoxAlarmCode;
        private System.Windows.Forms.Button buttonFindMaskStates;
        private System.Windows.Forms.Button buttonSetMaskStates;
        private System.Windows.Forms.DataGridView dataGridViewAlarmMaskStates;
        private System.Windows.Forms.DataGridViewTextBoxColumn 对象名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 对象类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 告警编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 抑制状态;
    }
}