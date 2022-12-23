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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewPtpFtpCtps = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.内环ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.外环ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.不环回ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonFind = new System.Windows.Forms.Button();
            this.ComPtpCtpFtp = new System.Windows.Forms.ComboBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.treeViewPtpCtpFtp = new System.Windows.Forms.TreeView();
            this.业务关联端口 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.环回 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPtpFtpCtps)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox11.SuspendLayout();
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
            this.splitContainer1.SplitterDistance = 434;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewPtpFtpCtps);
            this.groupBox1.Controls.Add(this.buttonFind);
            this.groupBox1.Controls.Add(this.ComPtpCtpFtp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 624);
            this.groupBox1.TabIndex = 135;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "业务关联端口";
            // 
            // dataGridViewPtpFtpCtps
            // 
            this.dataGridViewPtpFtpCtps.AllowUserToResizeColumns = false;
            this.dataGridViewPtpFtpCtps.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewPtpFtpCtps.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPtpFtpCtps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPtpFtpCtps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewPtpFtpCtps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewPtpFtpCtps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.业务关联端口,
            this.环回});
            this.dataGridViewPtpFtpCtps.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPtpFtpCtps.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewPtpFtpCtps.Location = new System.Drawing.Point(6, 112);
            this.dataGridViewPtpFtpCtps.Name = "dataGridViewPtpFtpCtps";
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPtpFtpCtps.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewPtpFtpCtps.RowHeadersVisible = false;
            this.dataGridViewPtpFtpCtps.RowTemplate.Height = 23;
            this.dataGridViewPtpFtpCtps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPtpFtpCtps.Size = new System.Drawing.Size(422, 500);
            this.dataGridViewPtpFtpCtps.TabIndex = 136;
            this.dataGridViewPtpFtpCtps.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPtpFtpCtps_CellClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.内环ToolStripMenuItem,
            this.外环ToolStripMenuItem,
            this.不环回ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(113, 70);
            // 
            // 内环ToolStripMenuItem
            // 
            this.内环ToolStripMenuItem.Name = "内环ToolStripMenuItem";
            this.内环ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.内环ToolStripMenuItem.Text = "内环";
            this.内环ToolStripMenuItem.Click += new System.EventHandler(this.内环ToolStripMenuItem_Click);
            // 
            // 外环ToolStripMenuItem
            // 
            this.外环ToolStripMenuItem.Name = "外环ToolStripMenuItem";
            this.外环ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.外环ToolStripMenuItem.Text = "外环";
            this.外环ToolStripMenuItem.Click += new System.EventHandler(this.外环ToolStripMenuItem_Click);
            // 
            // 不环回ToolStripMenuItem
            // 
            this.不环回ToolStripMenuItem.Name = "不环回ToolStripMenuItem";
            this.不环回ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.不环回ToolStripMenuItem.Text = "不环回";
            this.不环回ToolStripMenuItem.Click += new System.EventHandler(this.不环回ToolStripMenuItem_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(315, 53);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(116, 41);
            this.buttonFind.TabIndex = 135;
            this.buttonFind.Text = "查询";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
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
            this.ComPtpCtpFtp.Size = new System.Drawing.Size(427, 27);
            this.ComPtpCtpFtp.TabIndex = 134;
            this.ComPtpCtpFtp.SelectedIndexChanged += new System.EventHandler(this.ComPtpCtpFtp_SelectedIndexChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.treeViewPtpCtpFtp);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(704, 624);
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
            this.treeViewPtpCtpFtp.Size = new System.Drawing.Size(698, 604);
            this.treeViewPtpCtpFtp.TabIndex = 136;
            this.treeViewPtpCtpFtp.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewPtpCtpFtp_DrawNode);
            // 
            // 业务关联端口
            // 
            this.业务关联端口.HeaderText = "业务关联端口";
            this.业务关联端口.Name = "业务关联端口";
            this.业务关联端口.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.业务关联端口.Width = 280;
            // 
            // 环回
            // 
            this.环回.HeaderText = "环回";
            this.环回.Name = "环回";
            this.环回.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.环回.Width = 140;
            // 
            // Form_Connection_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 624);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form_Connection_Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "业务详细信息";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPtpFtpCtps)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TreeView treeViewPtpCtpFtp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ComPtpCtpFtp;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.DataGridView dataGridViewPtpFtpCtps;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 内环ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 外环ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 不环回ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn 业务关联端口;
        private System.Windows.Forms.DataGridViewTextBoxColumn 环回;
    }
}