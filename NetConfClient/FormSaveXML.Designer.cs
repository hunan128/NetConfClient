namespace NetConfClientSoftware
{
    partial class FormSaveXML
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
            this.textBoxXmlName = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.comboBoxIPS = new System.Windows.Forms.ComboBox();
            this.labelsaveXML = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxXmlName
            // 
            this.textBoxXmlName.Location = new System.Drawing.Point(249, 36);
            this.textBoxXmlName.Multiline = true;
            this.textBoxXmlName.Name = "textBoxXmlName";
            this.textBoxXmlName.Size = new System.Drawing.Size(182, 20);
            this.textBoxXmlName.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(249, 73);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = ".xml";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "请输入";
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "01-网元-",
            "02-板卡-",
            "03-端口-",
            "04-业务-",
            "05-带保护业务-",
            "06-保护组-",
            "07-告警-",
            "08-性能-",
            "09-时钟-",
            "10-文件上下载-"});
            this.comboBoxType.Location = new System.Drawing.Point(143, 36);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(100, 20);
            this.comboBoxType.TabIndex = 6;
            this.comboBoxType.Text = "01-网元-";
            // 
            // comboBoxIPS
            // 
            this.comboBoxIPS.FormattingEnabled = true;
            this.comboBoxIPS.Items.AddRange(new object[] {
            "01-电信-",
            "02-联通-",
            "03-移动-"});
            this.comboBoxIPS.Location = new System.Drawing.Point(59, 36);
            this.comboBoxIPS.Name = "comboBoxIPS";
            this.comboBoxIPS.Size = new System.Drawing.Size(78, 20);
            this.comboBoxIPS.TabIndex = 7;
            this.comboBoxIPS.Text = "01-电信-";
            // 
            // labelsaveXML
            // 
            this.labelsaveXML.AutoSize = true;
            this.labelsaveXML.Location = new System.Drawing.Point(13, 13);
            this.labelsaveXML.Name = "labelsaveXML";
            this.labelsaveXML.Size = new System.Drawing.Size(71, 12);
            this.labelsaveXML.TabIndex = 8;
            this.labelsaveXML.Text = "XML存放路径";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(356, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormSaveXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 125);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelsaveXML);
            this.Controls.Add(this.comboBoxIPS);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxXmlName);
            this.Name = "FormSaveXML";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormSaveXML";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxXmlName;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboBoxIPS;
        private System.Windows.Forms.Label labelsaveXML;
        private System.Windows.Forms.Button button1;
    }
}