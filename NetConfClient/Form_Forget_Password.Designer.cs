namespace NetConfClientSoftware
{
    partial class Form_Forget_Password
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
            this.ButtonForget = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Account = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxViewMail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.labelp = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.textBoxPass2 = new System.Windows.Forms.TextBox();
            this.labelp2 = new System.Windows.Forms.Label();
            this.buttoncheckemail = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonForget
            // 
            this.ButtonForget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ButtonForget.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButtonForget.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ButtonForget.ForeColor = System.Drawing.Color.White;
            this.ButtonForget.Location = new System.Drawing.Point(238, 543);
            this.ButtonForget.Name = "ButtonForget";
            this.ButtonForget.Size = new System.Drawing.Size(150, 40);
            this.ButtonForget.TabIndex = 0;
            this.ButtonForget.Text = "重新设置密码";
            this.ButtonForget.UseVisualStyleBackColor = false;
            this.ButtonForget.Visible = false;
            this.ButtonForget.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 40);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = global::NetConfClientSoftware.Properties.Resources.关闭;
            this.pictureBox1.Location = new System.Drawing.Point(582, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PicClose_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(622, 60);
            this.label1.TabIndex = 3;
            this.label1.Text = "Welcom To Login Netconf Client";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Account
            // 
            this.Account.AutoSize = true;
            this.Account.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Account.Location = new System.Drawing.Point(158, 114);
            this.Account.Name = "Account";
            this.Account.Size = new System.Drawing.Size(58, 22);
            this.Account.TabIndex = 4;
            this.Account.Text = "用户名";
            // 
            // textBoxUser
            // 
            this.textBoxUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxUser.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.textBoxUser.Location = new System.Drawing.Point(162, 149);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(300, 22);
            this.textBoxUser.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(162, 175);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 1);
            this.panel2.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(162, 239);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 1);
            this.panel3.TabIndex = 9;
            // 
            // textBoxViewMail
            // 
            this.textBoxViewMail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxViewMail.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.textBoxViewMail.Location = new System.Drawing.Point(162, 214);
            this.textBoxViewMail.Name = "textBoxViewMail";
            this.textBoxViewMail.Size = new System.Drawing.Size(300, 22);
            this.textBoxViewMail.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(158, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 22);
            this.label2.TabIndex = 7;
            this.label2.Text = "注册邮箱提示：";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(162, 303);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(300, 1);
            this.panel4.TabIndex = 12;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEmail.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.textBoxEmail.Location = new System.Drawing.Point(162, 278);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(300, 22);
            this.textBoxEmail.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(158, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 22);
            this.label3.TabIndex = 10;
            this.label3.Text = "输入注册邮箱进行验证";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Black;
            this.panel5.Location = new System.Drawing.Point(162, 445);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(300, 1);
            this.panel5.TabIndex = 15;
            this.panel5.Visible = false;
            // 
            // textBoxPass
            // 
            this.textBoxPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPass.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.textBoxPass.Location = new System.Drawing.Point(162, 420);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.Size = new System.Drawing.Size(300, 22);
            this.textBoxPass.TabIndex = 14;
            this.textBoxPass.UseSystemPasswordChar = true;
            this.textBoxPass.Visible = false;
            // 
            // labelp
            // 
            this.labelp.AutoSize = true;
            this.labelp.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelp.Location = new System.Drawing.Point(158, 385);
            this.labelp.Name = "labelp";
            this.labelp.Size = new System.Drawing.Size(90, 22);
            this.labelp.TabIndex = 13;
            this.labelp.Text = "设置新密码";
            this.labelp.Visible = false;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Black;
            this.panel6.Location = new System.Drawing.Point(162, 509);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(300, 1);
            this.panel6.TabIndex = 18;
            this.panel6.Visible = false;
            // 
            // textBoxPass2
            // 
            this.textBoxPass2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPass2.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.textBoxPass2.Location = new System.Drawing.Point(162, 484);
            this.textBoxPass2.Name = "textBoxPass2";
            this.textBoxPass2.Size = new System.Drawing.Size(300, 22);
            this.textBoxPass2.TabIndex = 17;
            this.textBoxPass2.UseSystemPasswordChar = true;
            this.textBoxPass2.Visible = false;
            // 
            // labelp2
            // 
            this.labelp2.AutoSize = true;
            this.labelp2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelp2.Location = new System.Drawing.Point(158, 449);
            this.labelp2.Name = "labelp2";
            this.labelp2.Size = new System.Drawing.Size(106, 22);
            this.labelp2.TabIndex = 16;
            this.labelp2.Text = "再次输入密码";
            this.labelp2.Visible = false;
            // 
            // buttoncheckemail
            // 
            this.buttoncheckemail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttoncheckemail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttoncheckemail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.buttoncheckemail.ForeColor = System.Drawing.Color.White;
            this.buttoncheckemail.Location = new System.Drawing.Point(238, 330);
            this.buttoncheckemail.Name = "buttoncheckemail";
            this.buttoncheckemail.Size = new System.Drawing.Size(150, 40);
            this.buttoncheckemail.TabIndex = 19;
            this.buttoncheckemail.Text = "验证邮箱";
            this.buttoncheckemail.UseVisualStyleBackColor = false;
            this.buttoncheckemail.Click += new System.EventHandler(this.buttoncheckemail_Click);
            // 
            // Form_Forget_Password
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(622, 601);
            this.Controls.Add(this.buttoncheckemail);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.textBoxPass2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.labelp2);
            this.Controls.Add(this.textBoxPass);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.labelp);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxViewMail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.Account);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ButtonForget);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Forget_Password";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Login";
            this.Load += new System.EventHandler(this.Form_Login_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonForget;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Account;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBoxViewMail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox textBoxPass;
        private System.Windows.Forms.Label labelp;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox textBoxPass2;
        private System.Windows.Forms.Label labelp2;
        private System.Windows.Forms.Button buttoncheckemail;
    }
}