namespace ModelViewer
{
	partial class ToolboxControls
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupToolbox = new System.Windows.Forms.GroupBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupToolbox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupToolbox
			// 
			this.groupToolbox.Controls.Add(this.numericUpDown1);
			this.groupToolbox.Controls.Add(this.checkBox1);
			this.groupToolbox.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupToolbox.Location = new System.Drawing.Point(0, 0);
			this.groupToolbox.Name = "groupToolbox";
			this.groupToolbox.Size = new System.Drawing.Size(184, 249);
			this.groupToolbox.TabIndex = 3;
			this.groupToolbox.TabStop = false;
			this.groupToolbox.Text = "Plugin Toolbox";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(93, 18);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(85, 20);
			this.numericUpDown1.TabIndex = 5;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 19);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(75, 17);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Animation:";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// ToolboxControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupToolbox);
			this.Name = "ToolboxControls";
			this.Size = new System.Drawing.Size(184, 249);
			this.groupToolbox.ResumeLayout(false);
			this.groupToolbox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupToolbox;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.CheckBox checkBox1;


	}
}
