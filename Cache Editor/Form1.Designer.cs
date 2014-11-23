namespace Cache_Editor
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openCacheFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newSubArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.batchExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.summaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.file_browser = new System.Windows.Forms.TreeView();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblSearch = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblSave = new System.Windows.Forms.ToolStripStatusLabel();
			this.pbSave = new System.Windows.Forms.ToolStripProgressBar();
			this.lblPlugins = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbPlugins = new System.Windows.Forms.TabControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupToolbox = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnImport = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblFile = new System.Windows.Forms.Label();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCacheFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveCurrentToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.newFileToolStripMenuItem,
            this.newSubArchiveToolStripMenuItem,
            this.toolStripMenuItem3,
            this.batchExportToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openCacheFolderToolStripMenuItem
			// 
			this.openCacheFolderToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openCacheFolderToolStripMenuItem.Image")));
			this.openCacheFolderToolStripMenuItem.Name = "openCacheFolderToolStripMenuItem";
			this.openCacheFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openCacheFolderToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.openCacheFolderToolStripMenuItem.Text = "Open Cache Folder";
			this.openCacheFolderToolStripMenuItem.Click += new System.EventHandler(this.openCacheFolderToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(257, 6);
			// 
			// saveCurrentToolStripMenuItem
			// 
			this.saveCurrentToolStripMenuItem.Enabled = false;
			this.saveCurrentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveCurrentToolStripMenuItem.Image")));
			this.saveCurrentToolStripMenuItem.Name = "saveCurrentToolStripMenuItem";
			this.saveCurrentToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveCurrentToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.saveCurrentToolStripMenuItem.Text = "Save Current Sub-Archive";
			this.saveCurrentToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.saveToolStripMenuItem.Text = "Save All Sub-Archives";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(257, 6);
			// 
			// newFileToolStripMenuItem
			// 
			this.newFileToolStripMenuItem.Enabled = false;
			this.newFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newFileToolStripMenuItem.Image")));
			this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
			this.newFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newFileToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.newFileToolStripMenuItem.Text = "New File";
			this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
			// 
			// newSubArchiveToolStripMenuItem
			// 
			this.newSubArchiveToolStripMenuItem.Enabled = false;
			this.newSubArchiveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newSubArchiveToolStripMenuItem.Image")));
			this.newSubArchiveToolStripMenuItem.Name = "newSubArchiveToolStripMenuItem";
			this.newSubArchiveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.newSubArchiveToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.newSubArchiveToolStripMenuItem.Text = "New Sub-Archive";
			this.newSubArchiveToolStripMenuItem.Visible = false;
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(257, 6);
			// 
			// batchExportToolStripMenuItem
			// 
			this.batchExportToolStripMenuItem.Enabled = false;
			this.batchExportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("batchExportToolStripMenuItem.Image")));
			this.batchExportToolStripMenuItem.Name = "batchExportToolStripMenuItem";
			this.batchExportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.batchExportToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
			this.batchExportToolStripMenuItem.Text = "Batch Export";
			this.batchExportToolStripMenuItem.Click += new System.EventHandler(this.batchExportToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchFileToolStripMenuItem,
            this.toolStripMenuItem2,
            this.refreshToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// searchFileToolStripMenuItem
			// 
			this.searchFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("searchFileToolStripMenuItem.Image")));
			this.searchFileToolStripMenuItem.Name = "searchFileToolStripMenuItem";
			this.searchFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.searchFileToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.searchFileToolStripMenuItem.Text = "Search File";
			this.searchFileToolStripMenuItem.Click += new System.EventHandler(this.searchFileToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(167, 6);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshToolStripMenuItem.Image")));
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// pluginsToolStripMenuItem
			// 
			this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.summaryToolStripMenuItem});
			this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
			this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.pluginsToolStripMenuItem.Text = "&Plugins";
			// 
			// summaryToolStripMenuItem
			// 
			this.summaryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("summaryToolStripMenuItem.Image")));
			this.summaryToolStripMenuItem.Name = "summaryToolStripMenuItem";
			this.summaryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.summaryToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.summaryToolStripMenuItem.Text = "Summary";
			this.summaryToolStripMenuItem.Click += new System.EventHandler(this.summaryToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.creditsToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// creditsToolStripMenuItem
			// 
			this.creditsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("creditsToolStripMenuItem.Image")));
			this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
			this.creditsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.creditsToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.creditsToolStripMenuItem.Text = "About";
			this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
			// 
			// file_browser
			// 
			this.file_browser.Dock = System.Windows.Forms.DockStyle.Left;
			this.file_browser.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.file_browser.HideSelection = false;
			this.file_browser.Location = new System.Drawing.Point(0, 24);
			this.file_browser.Name = "file_browser";
			this.file_browser.Size = new System.Drawing.Size(229, 516);
			this.file_browser.TabIndex = 1;
			this.file_browser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.file_browser_AfterSelect);
			this.file_browser.DragDrop += new System.Windows.Forms.DragEventHandler(this.file_browser_DragDrop);
			this.file_browser.DragEnter += new System.Windows.Forms.DragEventHandler(this.file_browser_DragEnter);
			this.file_browser.DragOver += new System.Windows.Forms.DragEventHandler(this.file_browser_DragOver);
			this.file_browser.DragLeave += new System.EventHandler(this.file_browser_DragLeave);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSearch,
            this.lblSave,
            this.pbSave,
            this.lblPlugins});
			this.statusStrip1.Location = new System.Drawing.Point(0, 540);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblSearch
			// 
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(1099, 17);
			this.lblSearch.Spring = true;
			this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSave
			// 
			this.lblSave.Name = "lblSave";
			this.lblSave.Size = new System.Drawing.Size(82, 17);
			this.lblSave.Text = "Save Progress:";
			this.lblSave.Visible = false;
			// 
			// pbSave
			// 
			this.pbSave.Name = "pbSave";
			this.pbSave.Size = new System.Drawing.Size(100, 16);
			this.pbSave.Step = 1;
			this.pbSave.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbSave.Visible = false;
			// 
			// lblPlugins
			// 
			this.lblPlugins.AutoSize = false;
			this.lblPlugins.Name = "lblPlugins";
			this.lblPlugins.Size = new System.Drawing.Size(150, 17);
			this.lblPlugins.Text = "0 Plugins Loaded";
			this.lblPlugins.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tbPlugins);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(237, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(819, 516);
			this.panel1.TabIndex = 3;
			// 
			// tbPlugins
			// 
			this.tbPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbPlugins.Location = new System.Drawing.Point(0, 0);
			this.tbPlugins.Name = "tbPlugins";
			this.tbPlugins.SelectedIndex = 0;
			this.tbPlugins.Size = new System.Drawing.Size(819, 516);
			this.tbPlugins.TabIndex = 0;
			this.tbPlugins.SelectedIndexChanged += new System.EventHandler(this.tbPlugins_SelectedIndexChanged);
			this.tbPlugins.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.tbPlugins.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(229, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 516);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.groupToolbox);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Controls.Add(this.groupBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(1064, 24);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(8);
			this.panel2.Size = new System.Drawing.Size(200, 516);
			this.panel2.TabIndex = 5;
			// 
			// groupToolbox
			// 
			this.groupToolbox.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupToolbox.Location = new System.Drawing.Point(8, 200);
			this.groupToolbox.Name = "groupToolbox";
			this.groupToolbox.Size = new System.Drawing.Size(184, 249);
			this.groupToolbox.TabIndex = 2;
			this.groupToolbox.TabStop = false;
			this.groupToolbox.Text = "Plugin Toolbox";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.flowLayoutPanel1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(8, 108);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(8);
			this.groupBox2.Size = new System.Drawing.Size(184, 92);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "File Actions";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnImport);
			this.flowLayoutPanel1.Controls.Add(this.btnExport);
			this.flowLayoutPanel1.Controls.Add(this.btnRename);
			this.flowLayoutPanel1.Controls.Add(this.btnDelete);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 21);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(168, 63);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// btnImport
			// 
			this.btnImport.Enabled = false;
			this.btnImport.Location = new System.Drawing.Point(3, 3);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 0;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// btnExport
			// 
			this.btnExport.Enabled = false;
			this.btnExport.Location = new System.Drawing.Point(84, 3);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 1;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnRename
			// 
			this.btnRename.Enabled = false;
			this.btnRename.Location = new System.Drawing.Point(3, 32);
			this.btnRename.Name = "btnRename";
			this.btnRename.Size = new System.Drawing.Size(75, 23);
			this.btnRename.TabIndex = 2;
			this.btnRename.Text = "Rename";
			this.btnRename.UseVisualStyleBackColor = true;
			this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(84, 32);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblFile);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(184, 100);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Information";
			// 
			// lblFile
			// 
			this.lblFile.AutoSize = true;
			this.lblFile.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFile.Location = new System.Drawing.Point(6, 16);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(35, 14);
			this.lblFile.TabIndex = 0;
			this.lblFile.Text = "Info";
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter2.Location = new System.Drawing.Point(1056, 24);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(8, 516);
			this.splitter2.TabIndex = 7;
			this.splitter2.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 562);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.file_browser);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Lin\'s Cache Suite";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openCacheFolderToolStripMenuItem;
		private System.Windows.Forms.TreeView file_browser;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchFileToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblSearch;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TabControl tbPlugins;
		private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem summaryToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel lblPlugins;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.GroupBox groupToolbox;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel lblSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem saveCurrentToolStripMenuItem;
		private System.Windows.Forms.ToolStripProgressBar pbSave;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newSubArchiveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem batchExportToolStripMenuItem;
	}
}

