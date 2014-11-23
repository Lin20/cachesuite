namespace Cache_Editor
{
	partial class BatchExport
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
			this.rbArchive = new System.Windows.Forms.RadioButton();
			this.rbSub = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cboType = new System.Windows.Forms.ComboBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.cboArchive = new System.Windows.Forms.ComboBox();
			this.cboSub = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtExt = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDest = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// rbArchive
			// 
			this.rbArchive.AutoSize = true;
			this.rbArchive.Checked = true;
			this.rbArchive.Location = new System.Drawing.Point(12, 12);
			this.rbArchive.Name = "rbArchive";
			this.rbArchive.Size = new System.Drawing.Size(64, 17);
			this.rbArchive.TabIndex = 0;
			this.rbArchive.TabStop = true;
			this.rbArchive.Text = "Archive:";
			this.rbArchive.UseVisualStyleBackColor = true;
			// 
			// rbSub
			// 
			this.rbSub.AutoSize = true;
			this.rbSub.Location = new System.Drawing.Point(12, 38);
			this.rbSub.Name = "rbSub";
			this.rbSub.Size = new System.Drawing.Size(83, 17);
			this.rbSub.TabIndex = 2;
			this.rbSub.Text = "Sub-Archive";
			this.rbSub.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "As Type:";
			// 
			// cboType
			// 
			this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboType.FormattingEnabled = true;
			this.cboType.Location = new System.Drawing.Point(67, 64);
			this.cboType.Name = "cboType";
			this.cboType.Size = new System.Drawing.Size(186, 21);
			this.cboType.TabIndex = 5;
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 195);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(241, 23);
			this.progressBar1.TabIndex = 6;
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(37, 166);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(102, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "Dump";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(145, 166);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(83, 23);
			this.button2.TabIndex = 8;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cboArchive
			// 
			this.cboArchive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArchive.FormattingEnabled = true;
			this.cboArchive.Location = new System.Drawing.Point(101, 11);
			this.cboArchive.Name = "cboArchive";
			this.cboArchive.Size = new System.Drawing.Size(152, 21);
			this.cboArchive.TabIndex = 1;
			this.cboArchive.SelectedIndexChanged += new System.EventHandler(this.cboArchive_SelectedIndexChanged);
			// 
			// cboSub
			// 
			this.cboSub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSub.FormattingEnabled = true;
			this.cboSub.Location = new System.Drawing.Point(101, 37);
			this.cboSub.Name = "cboSub";
			this.cboSub.Size = new System.Drawing.Size(152, 21);
			this.cboSub.TabIndex = 9;
			this.cboSub.SelectedIndexChanged += new System.EventHandler(this.cboSub_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "File Name (* = #):";
			// 
			// txtExt
			// 
			this.txtExt.Location = new System.Drawing.Point(113, 91);
			this.txtExt.Name = "txtExt";
			this.txtExt.Size = new System.Drawing.Size(140, 20);
			this.txtExt.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Destination Folder:";
			// 
			// txtDest
			// 
			this.txtDest.Location = new System.Drawing.Point(113, 117);
			this.txtDest.Name = "txtDest";
			this.txtDest.ReadOnly = true;
			this.txtDest.Size = new System.Drawing.Size(100, 20);
			this.txtDest.TabIndex = 13;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(219, 115);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(34, 23);
			this.button3.TabIndex = 14;
			this.button3.Text = "...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// BatchExport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(265, 230);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.txtDest);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtExt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cboSub);
			this.Controls.Add(this.cboArchive);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.cboType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rbSub);
			this.Controls.Add(this.rbArchive);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BatchExport";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Batch Export";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BatchExport_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbArchive;
		private System.Windows.Forms.RadioButton rbSub;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboType;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox cboArchive;
		private System.Windows.Forms.ComboBox cboSub;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtExt;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDest;
		private System.Windows.Forms.Button button3;
	}
}