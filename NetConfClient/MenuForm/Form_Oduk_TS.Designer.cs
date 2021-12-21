namespace NetConfClientSoftware
{
    partial class Form_Oduk_TS
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ButOk = new System.Windows.Forms.Button();
            this.ButCenter = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.CheckBox = new System.Windows.Forms.CheckBox();
            this.Butclear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 98);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel.Size = new System.Drawing.Size(370, 178);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // ButOk
            // 
            this.ButOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButOk.Location = new System.Drawing.Point(146, 282);
            this.ButOk.Name = "ButOk";
            this.ButOk.Size = new System.Drawing.Size(75, 23);
            this.ButOk.TabIndex = 2;
            this.ButOk.Text = "确定";
            this.ButOk.UseVisualStyleBackColor = true;
            // 
            // ButCenter
            // 
            this.ButCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButCenter.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButCenter.Location = new System.Drawing.Point(307, 282);
            this.ButCenter.Name = "ButCenter";
            this.ButCenter.Size = new System.Drawing.Size(75, 23);
            this.ButCenter.TabIndex = 3;
            this.ButCenter.Text = "取消";
            this.ButCenter.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(12, 4);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(370, 57);
            this.textBox.TabIndex = 4;
            // 
            // CheckBox
            // 
            this.CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckBox.AutoSize = true;
            this.CheckBox.Location = new System.Drawing.Point(25, 286);
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Size = new System.Drawing.Size(90, 16);
            this.CheckBox.TabIndex = 6;
            this.CheckBox.Text = "全选/全不选";
            this.CheckBox.UseVisualStyleBackColor = true;
            this.CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Butclear
            // 
            this.Butclear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Butclear.Location = new System.Drawing.Point(226, 282);
            this.Butclear.Name = "Butclear";
            this.Butclear.Size = new System.Drawing.Size(75, 23);
            this.Butclear.TabIndex = 8;
            this.Butclear.Text = "清空";
            this.Butclear.UseVisualStyleBackColor = true;
            this.Butclear.Click += new System.EventHandler(this.Butclear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "注意：不能“勾选”说明时隙已被占用。";
            // 
            // Form_Oduk_TS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 317);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Butclear);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.ButCenter);
            this.Controls.Add(this.ButOk);
            this.Controls.Add(this.flowLayoutPanel);
            this.Name = "Form_Oduk_TS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button ButOk;
        private System.Windows.Forms.Button ButCenter;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.CheckBox CheckBox;
        private System.Windows.Forms.Button Butclear;
        private System.Windows.Forms.Label label1;
    }
}