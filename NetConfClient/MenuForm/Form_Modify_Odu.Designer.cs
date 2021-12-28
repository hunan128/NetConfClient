namespace NetConfClientSoftware
{
    partial class Form_Modify_Odu
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
            this.ComCTP = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ComPosition = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ComAction = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ComCurrentSlotsNum = new System.Windows.Forms.ComboBox();
            this.ComTsDetail = new System.Windows.Forms.ComboBox();
            this.ComTimeOut = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComCTP
            // 
            this.ComCTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComCTP.FormattingEnabled = true;
            this.ComCTP.Location = new System.Drawing.Point(111, 3);
            this.ComCTP.Name = "ComCTP";
            this.ComCTP.Size = new System.Drawing.Size(345, 20);
            this.ComCTP.TabIndex = 0;
            this.ComCTP.SelectedIndexChanged += new System.EventHandler(this.ComCTP_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "CTP端口";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComPosition
            // 
            this.ComPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComPosition.FormattingEnabled = true;
            this.ComPosition.Items.AddRange(new object[] {
            "client-odu",
            "line-odu"});
            this.ComPosition.Location = new System.Drawing.Point(111, 29);
            this.ComPosition.Name = "ComPosition";
            this.ComPosition.Size = new System.Drawing.Size(345, 20);
            this.ComPosition.TabIndex = 2;
            this.ComPosition.SelectedIndexChanged += new System.EventHandler(this.ComPosition_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口类型";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComAction
            // 
            this.ComAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComAction.FormattingEnabled = true;
            this.ComAction.Items.AddRange(new object[] {
            "delete-action",
            "add-action"});
            this.ComAction.Location = new System.Drawing.Point(111, 55);
            this.ComAction.Name = "ComAction";
            this.ComAction.Size = new System.Drawing.Size(345, 20);
            this.ComAction.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "动作";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 26);
            this.label4.TabIndex = 6;
            this.label4.Text = "支路调整到当前时隙数量";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 26);
            this.label5.TabIndex = 7;
            this.label5.Text = "线路时隙";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 31);
            this.label6.TabIndex = 8;
            this.label6.Text = "超时时间";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComCurrentSlotsNum
            // 
            this.ComCurrentSlotsNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComCurrentSlotsNum.Enabled = false;
            this.ComCurrentSlotsNum.FormattingEnabled = true;
            this.ComCurrentSlotsNum.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.ComCurrentSlotsNum.Location = new System.Drawing.Point(111, 81);
            this.ComCurrentSlotsNum.Name = "ComCurrentSlotsNum";
            this.ComCurrentSlotsNum.Size = new System.Drawing.Size(345, 20);
            this.ComCurrentSlotsNum.TabIndex = 9;
            // 
            // ComTsDetail
            // 
            this.ComTsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComTsDetail.FormattingEnabled = true;
            this.ComTsDetail.Items.AddRange(new object[] {
            "8-80",
            "8-C0",
            "8-E0",
            "8-F0",
            "8-F8",
            "8-FC",
            "8-FE",
            "8-FF"});
            this.ComTsDetail.Location = new System.Drawing.Point(111, 107);
            this.ComTsDetail.Name = "ComTsDetail";
            this.ComTsDetail.Size = new System.Drawing.Size(345, 20);
            this.ComTsDetail.TabIndex = 10;
            this.ComTsDetail.Text = "8-F0";
            // 
            // ComTimeOut
            // 
            this.ComTimeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComTimeOut.FormattingEnabled = true;
            this.ComTimeOut.Items.AddRange(new object[] {
            "0",
            "10",
            "15",
            "30",
            "60",
            "300"});
            this.ComTimeOut.Location = new System.Drawing.Point(111, 133);
            this.ComTimeOut.Name = "ComTimeOut";
            this.ComTimeOut.Size = new System.Drawing.Size(345, 20);
            this.ComTimeOut.TabIndex = 11;
            this.ComTimeOut.Text = "10";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(303, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(384, 201);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 247);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ODUFlex时隙调整";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.69231F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.30769F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ComCTP, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ComTimeOut, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.ComPosition, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.ComTsDetail, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ComCurrentSlotsNum, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ComAction, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(459, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Form_Modify_Odu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 266);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_Modify_Odu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Modify_Odu";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ComCTP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComAction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ComCurrentSlotsNum;
        private System.Windows.Forms.ComboBox ComTsDetail;
        private System.Windows.Forms.ComboBox ComTimeOut;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}