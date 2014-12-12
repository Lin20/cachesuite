namespace ItemViewer
{
	partial class Window
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.lblName = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pnlModelViewer = new System.Windows.Forms.Panel();
			this.splitContainer4 = new System.Windows.Forms.SplitContainer();
			this.pItemSmall = new System.Windows.Forms.PictureBox();
			this.pItemLarge = new System.Windows.Forms.PictureBox();
			this.prop_item = new System.Windows.Forms.PropertyGrid();
			this.modelViewer = new Cache_Editor_API.Controls.ModelViewer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlModelViewer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.Panel2.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pItemSmall)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pItemLarge)).BeginInit();
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
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(751, 491);
			this.splitContainer1.SplitterDistance = 250;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer3.Panel1.Controls.Add(this.panel3);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer3.Panel2.Controls.Add(this.panel4);
			this.splitContainer3.Size = new System.Drawing.Size(250, 491);
			this.splitContainer3.SplitterDistance = 53;
			this.splitContainer3.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.tableLayoutPanel1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(250, 53);
			this.panel3.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.86179F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.13821F));
			this.tableLayoutPanel1.Controls.Add(this.button1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 53);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
			this.button1.Location = new System.Drawing.Point(200, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(47, 47);
			this.button1.TabIndex = 0;
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// lblName
			// 
			this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(3, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(191, 53);
			this.lblName.TabIndex = 1;
			this.lblName.Text = "Name (ID)";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel4
			// 
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel4.Controls.Add(this.prop_item);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(250, 434);
			this.panel4.TabIndex = 1;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer2.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer2.Panel2.Controls.Add(this.pnlModelViewer);
			this.splitContainer2.Size = new System.Drawing.Size(497, 491);
			this.splitContainer2.SplitterDistance = 104;
			this.splitContainer2.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.splitContainer4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(497, 104);
			this.panel1.TabIndex = 0;
			// 
			// pnlModelViewer
			// 
			this.pnlModelViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlModelViewer.Controls.Add(this.modelViewer);
			this.pnlModelViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlModelViewer.Location = new System.Drawing.Point(0, 0);
			this.pnlModelViewer.Name = "pnlModelViewer";
			this.pnlModelViewer.Size = new System.Drawing.Size(497, 383);
			this.pnlModelViewer.TabIndex = 1;
			// 
			// splitContainer4
			// 
			this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer4.Location = new System.Drawing.Point(0, 0);
			this.splitContainer4.Name = "splitContainer4";
			// 
			// splitContainer4.Panel1
			// 
			this.splitContainer4.Panel1.Controls.Add(this.pItemSmall);
			// 
			// splitContainer4.Panel2
			// 
			this.splitContainer4.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.splitContainer4.Panel2.Controls.Add(this.pItemLarge);
			this.splitContainer4.Size = new System.Drawing.Size(493, 100);
			this.splitContainer4.SplitterDistance = 103;
			this.splitContainer4.TabIndex = 0;
			// 
			// pItemSmall
			// 
			this.pItemSmall.BackColor = System.Drawing.Color.Black;
			this.pItemSmall.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pItemSmall.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pItemSmall.Location = new System.Drawing.Point(0, 0);
			this.pItemSmall.Name = "pItemSmall";
			this.pItemSmall.Size = new System.Drawing.Size(103, 100);
			this.pItemSmall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pItemSmall.TabIndex = 0;
			this.pItemSmall.TabStop = false;
			// 
			// pItemLarge
			// 
			this.pItemLarge.BackColor = System.Drawing.Color.Black;
			this.pItemLarge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pItemLarge.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pItemLarge.Location = new System.Drawing.Point(0, 0);
			this.pItemLarge.Name = "pItemLarge";
			this.pItemLarge.Size = new System.Drawing.Size(386, 100);
			this.pItemLarge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pItemLarge.TabIndex = 1;
			this.pItemLarge.TabStop = false;
			// 
			// prop_item
			// 
			this.prop_item.Dock = System.Windows.Forms.DockStyle.Fill;
			this.prop_item.Location = new System.Drawing.Point(0, 0);
			this.prop_item.Name = "prop_item";
			this.prop_item.Size = new System.Drawing.Size(246, 430);
			this.prop_item.TabIndex = 0;
			// 
			// modelViewer
			// 
			this.modelViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelViewer.Location = new System.Drawing.Point(0, 0);
			this.modelViewer.Name = "modelViewer";
			this.modelViewer.Size = new System.Drawing.Size(493, 379);
			this.modelViewer.TabIndex = 0;
			// 
			// Window
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(751, 491);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Window";
			this.Text = "Window";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.pnlModelViewer.ResumeLayout(false);
			this.splitContainer4.Panel1.ResumeLayout(false);
			this.splitContainer4.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
			this.splitContainer4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pItemSmall)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pItemLarge)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlModelViewer;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.SplitContainer splitContainer4;
		private System.Windows.Forms.PictureBox pItemSmall;
		private System.Windows.Forms.PictureBox pItemLarge;
		private System.Windows.Forms.PropertyGrid prop_item;
		private Cache_Editor_API.Controls.ModelViewer modelViewer;
	}
}