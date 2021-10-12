namespace NetConfClientSoftware
{
    partial class LoginOn
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButCenter = new System.Windows.Forms.Button();
            this.ButOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.Comip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Textpasd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ComVersion = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButCenter);
            this.groupBox1.Controls.Add(this.ButOk);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 227);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接设备";
            // 
            // ButCenter
            // 
            this.ButCenter.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButCenter.Location = new System.Drawing.Point(200, 187);
            this.ButCenter.Name = "ButCenter";
            this.ButCenter.Size = new System.Drawing.Size(75, 23);
            this.ButCenter.TabIndex = 2;
            this.ButCenter.Text = "取消";
            this.ButCenter.UseVisualStyleBackColor = true;
            // 
            // ButOk
            // 
            this.ButOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButOk.Location = new System.Drawing.Point(119, 187);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(75, 23);
            this.ButOk.TabIndex = 1;
            this.ButOk.Text = "连接";
            this.ButOk.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.35593F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.64407F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Comip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.TextUser, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Textpasd, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.TextPort, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ComVersion, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(29, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(246, 150);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备IP";
            // 
            // Comip
            // 
            this.Comip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Comip.FormattingEnabled = true;
            this.Comip.Location = new System.Drawing.Point(80, 3);
            this.Comip.Name = "Comip";
            this.Comip.Size = new System.Drawing.Size(163, 20);
            this.Comip.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户名";
            // 
            // TextUser
            // 
            this.TextUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextUser.Location = new System.Drawing.Point(80, 33);
            this.TextUser.Name = "TextUser";
            this.TextUser.Size = new System.Drawing.Size(163, 21);
            this.TextUser.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "密码";
            // 
            // Textpasd
            // 
            this.Textpasd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Textpasd.Location = new System.Drawing.Point(80, 63);
            this.Textpasd.Name = "Textpasd";
            this.Textpasd.PasswordChar = '*';
            this.Textpasd.Size = new System.Drawing.Size(163, 21);
            this.Textpasd.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 30);
            this.label4.TabIndex = 6;
            this.label4.Text = "端口";
            // 
            // TextPort
            // 
            this.TextPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextPort.Location = new System.Drawing.Point(80, 93);
            this.TextPort.Name = "TextPort";
            this.TextPort.Size = new System.Drawing.Size(163, 21);
            this.TextPort.TabIndex = 7;
            this.TextPort.Text = "830";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 30);
            this.label5.TabIndex = 8;
            this.label5.Text = "NetConf版本";
            // 
            // ComVersion
            // 
            this.ComVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComVersion.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComVersion.FormattingEnabled = true;
            this.ComVersion.Items.AddRange(new object[] {
            "Auto"});
            this.ComVersion.Location = new System.Drawing.Point(80, 123);
            this.ComVersion.Name = "ComVersion";
            this.ComVersion.Size = new System.Drawing.Size(163, 20);
            this.ComVersion.TabIndex = 9;
            // 
            // LoginOn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 251);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoginOn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginOn";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButCenter;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Comip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Textpasd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ComVersion;
    }
}