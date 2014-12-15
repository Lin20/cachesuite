using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cache_Editor_API.Controls
{
	public class SelectablePictureBox : PictureBox
	{
		public SelectablePictureBox()
		{
			this.SetStyle(ControlStyles.Selectable, true);
			this.TabStop = true;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Focus();
			base.OnMouseDown(e);
		}
		protected override void OnEnter(EventArgs e)
		{
			this.Invalidate();
			base.OnEnter(e);
		}
		protected override void OnLeave(EventArgs e)
		{
			this.Invalidate();
			base.OnLeave(e);
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			if (this.Focused)
			{
				var rc = this.ClientRectangle;
				rc.Inflate(-2, -2);
				ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
			}
		}
	}
}