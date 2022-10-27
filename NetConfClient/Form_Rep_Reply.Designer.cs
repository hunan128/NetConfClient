namespace NetConfClientSoftware
{
    partial class Form_Rep_reply
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
            this.treeViewXML = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxXML = new System.Windows.Forms.TextBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox11);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1364, 681);
            this.splitContainer1.SplitterDistance = 682;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.treeViewXML);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(682, 681);
            this.groupBox11.TabIndex = 138;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "树状图";
            // 
            // treeViewXML
            // 
            this.treeViewXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewXML.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewXML.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewXML.ItemHeight = 14;
            this.treeViewXML.Location = new System.Drawing.Point(3, 17);
            this.treeViewXML.Name = "treeViewXML";
            this.treeViewXML.Size = new System.Drawing.Size(676, 661);
            this.treeViewXML.TabIndex = 136;
            this.treeViewXML.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewPtpCtpFtp_DrawNode);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxXML);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 681);
            this.groupBox1.TabIndex = 135;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "XML";
            // 
            // textBoxXML
            // 
            this.textBoxXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxXML.ForeColor = System.Drawing.Color.Blue;
            this.textBoxXML.Location = new System.Drawing.Point(3, 17);
            this.textBoxXML.MaxLength = 32767000;
            this.textBoxXML.Multiline = true;
            this.textBoxXML.Name = "textBoxXML";
            this.textBoxXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxXML.Size = new System.Drawing.Size(672, 661);
            this.textBoxXML.TabIndex = 22;
            // 
            // Form_Rep_reply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 681);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form_Rep_reply";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XML原始信息";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TreeView treeViewXML;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxXML;
    }
}