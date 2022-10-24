namespace NetConfClientSoftware
{
    partial class Form_Connection_Info
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.treeViewPtpCtpFtp = new System.Windows.Forms.TreeView();
            this.ComPtpCtpFtp = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonFind = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox11);
            this.splitContainer1.Size = new System.Drawing.Size(1142, 624);
            this.splitContainer1.SplitterDistance = 435;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.treeViewPtpCtpFtp);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(703, 624);
            this.groupBox11.TabIndex = 138;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "端口信息";
            // 
            // treeViewPtpCtpFtp
            // 
            this.treeViewPtpCtpFtp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPtpCtpFtp.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewPtpCtpFtp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewPtpCtpFtp.ItemHeight = 14;
            this.treeViewPtpCtpFtp.Location = new System.Drawing.Point(3, 17);
            this.treeViewPtpCtpFtp.Name = "treeViewPtpCtpFtp";
            this.treeViewPtpCtpFtp.Size = new System.Drawing.Size(697, 604);
            this.treeViewPtpCtpFtp.TabIndex = 136;
            this.treeViewPtpCtpFtp.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewPtpCtpFtp_DrawNode);
            // 
            // ComPtpCtpFtp
            // 
            this.ComPtpCtpFtp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComPtpCtpFtp.Font = new System.Drawing.Font("宋体", 14F);
            this.ComPtpCtpFtp.FormattingEnabled = true;
            this.ComPtpCtpFtp.Location = new System.Drawing.Point(6, 20);
            this.ComPtpCtpFtp.Name = "ComPtpCtpFtp";
            this.ComPtpCtpFtp.Size = new System.Drawing.Size(428, 27);
            this.ComPtpCtpFtp.TabIndex = 134;
            this.ComPtpCtpFtp.SelectedIndexChanged += new System.EventHandler(this.ComPtpCtpFtp_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonFind);
            this.groupBox1.Controls.Add(this.ComPtpCtpFtp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 624);
            this.groupBox1.TabIndex = 135;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "业务关联端口";
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(316, 53);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(116, 41);
            this.buttonFind.TabIndex = 135;
            this.buttonFind.Text = "查询";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // Form_Connection_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 624);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form_Connection_Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Connection_Info";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TreeView treeViewPtpCtpFtp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ComPtpCtpFtp;
        private System.Windows.Forms.Button buttonFind;
    }
}