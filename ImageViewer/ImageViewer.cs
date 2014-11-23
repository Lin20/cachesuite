using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;

namespace ImageViewer
{
	public class ImageViewer : IPlugin
	{
		//basic
		public string Name { get { return "Image Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.2"; } }
		public string Description { get { return "Views and allows the basic importing and exporting of 317-format images."; } }
		public EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		public bool Dominant { get { return true; } }

		//properties to be filled in by the shell
		public CacheItemNode Node { get; set; }
		public DataBuffer Data { get; set; }
		public Cache Cache { get; set; }

		//optional
		public Control[] Controls { get; set; }
		public Control[] ToolControls { get; set; }
		public string StaticFileExtensions { get; set; }
		public string FileExtensions { get; set; }
		public Form ConfigureForm { get; set; }

		private Bitmap checkerboard;

		public ImageViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.SubArchives = new int[] { 1, 4, 6 };
			FileExtensions = "";
			StaticFileExtensions = "PNG Image|*.png";

			Controls = new Control[2];
			Label not = new Label();
			not.AutoSize = false;
			not.Text = "Not an image";
			not.Dock = DockStyle.Fill;
			not.TextAlign = ContentAlignment.MiddleCenter;
			not.Visible = false;
			Controls[1] = not;

			PictureBox image = new PictureBox();
			image.SizeMode = PictureBoxSizeMode.CenterImage;
			image.Dock = DockStyle.Fill;
			Controls[0] = image;

			ToolControls = new Control[2];
			Label l = new Label();
			l.BackColor = Color.Transparent;
			l.AutoSize = false;
			l.Text = "Sub-index:";
			l.Top = 20;
			l.Left = 4;
			l.Width = 80;
			ToolControls[0] = l;

			NumericUpDown n = new NumericUpDown();
			n.Maximum = 256;
			n.Width = 80;
			n.Left = 88;
			n.Top = l.Top - 3;
			n.ValueChanged += delegate(object sender, EventArgs e) { PluginSelected(); };
			ToolControls[1] = n;

			checkerboard = new Bitmap(32, 32);
			Graphics g = Graphics.FromImage(checkerboard);
			g.Clear(Color.White);
			g.FillRectangle(Brushes.LightGray, 0, 0, 16, 16);
			g.FillRectangle(Brushes.LightGray, 16, 16, 16, 16);
			image.BackgroundImage = checkerboard;
			image.BackgroundImageLayout = ImageLayout.Tile;
		}

		public void PluginSelected()
		{
			RSImage image = new RSImage(Cache, Node, Data, (int)((NumericUpDown)ToolControls[1]).Value);
			((PictureBox)Controls[0]).Image = image.GenerateBitmap();
			if (((PictureBox)Controls[0]).Image == null)
			{
				Controls[0].Visible = false;
				Controls[1].Visible = true;
				FileExtensions = "";
			}
			else
			{
				Controls[0].Visible = true;
				Controls[1].Visible = false;
				FileExtensions = "PNG Image (*.png)|*.png";
			}
		}

		public bool OnImport(string filename)
		{
			try
			{
				Exception e = null;
				RSImage new_image = new RSImage(filename, out e);
				if (e != null)
					throw e;
				new_image.Write(Cache, Node, Data);
				Data = Cache.SubArchives[Node.SubArchive].ExtractFile(Node.FileIndex);
				((PictureBox)Controls[0]).Image = new_image.GenerateBitmap();
				return true;

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error importing image.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Importing Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		public void OnExport(string filename)
		{
			try
			{
				PictureBox p = (PictureBox)Controls[0];
				PluginSelected();
				if (p.Image != null)
					p.Image.Save(filename);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error exporting image.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Exporting Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void OnConfigure()
		{
		}
	}
}
