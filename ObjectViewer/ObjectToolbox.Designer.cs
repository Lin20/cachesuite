namespace ObjectViewer
{
	partial class ItemToolbox
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
			this.label1 = new System.Windows.Forms.Label();
			this.nItem = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.nType = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.nItem)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nType)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Item:";
			// 
			// nItem
			// 
			this.nItem.Location = new System.Drawing.Point(43, 19);
			this.nItem.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nItem.Name = "nItem";
			this.nItem.Size = new System.Drawing.Size(121, 20);
			this.nItem.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Type:";
			// 
			// nType
			// 
			this.nType.Location = new System.Drawing.Point(43, 45);
			this.nType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nType.Name = "nType";
			this.nType.Size = new System.Drawing.Size(121, 20);
			this.nType.TabIndex = 3;
			this.nType.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// ItemToolbox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nItem);
			this.Controls.Add(this.label1);
			this.Name = "ItemToolbox";
			this.Size = new System.Drawing.Size(167, 150);
			((System.ComponentModel.ISupportInitialize)(this.nItem)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nType)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nType;
	}
}
