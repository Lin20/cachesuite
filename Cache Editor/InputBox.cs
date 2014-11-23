using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cache_Editor
{
	public class InputBox : Form
	{
		public InputBox(string title, int textboxes, params string[] labels)
		{
			this.KeyPreview = true;
			this.KeyPress += InputBox_KeyPress;

			this.Text = title;
			this.Width = 300;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MinimizeBox = false;
			this.MaximizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Height = textboxes * 28 + 100;
			for (int i = 0; i < textboxes; i++)
			{
				TextBox t = new TextBox();
				t.Width = 118;
				t.Location = new System.Drawing.Point(150, 16 + i * 28);
				t.KeyPress += Pressed_Enter;
				this.Controls.Add(t);
			}

			for (int i = 0; i < labels.Length; i++)
			{
				Label l = new Label();
				l.Text = labels[i];
				l.Location = new System.Drawing.Point(16, 20 + i * 28);
				l.AutoSize = true;
				this.Controls.Add(l);
			}

			Button ok = new Button();
			ok.Text = "OK";
			ok.Location = new System.Drawing.Point(this.Width / 2 - ok.Width - 16, this.ClientSize.Height - ok.Height - 16);
			ok.Click += OK_Clicked;
			this.Controls.Add(ok);

			Button cancel = new Button();
			cancel.Text = "Cancel";
			cancel.Location = new System.Drawing.Point(this.Width / 2, this.ClientSize.Height - cancel.Height - 16);
			cancel.Click += delegate(object sender, EventArgs e) { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; this.Close(); };
			this.Controls.Add(cancel);
		}

		private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape)
			{
				e.Handled = true;
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.Close();
			}
		}

		private void Pressed_Enter(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				OK_Clicked(this, null);
			}
		}

		private void OK_Clicked(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		public string GetText(int index)
		{
			return this.Controls[index].Text;
		}
	}
}
