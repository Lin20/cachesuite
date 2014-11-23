using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;

namespace PaletteViewer
{
	public class PaletteViewer : IPlugin
	{
		//basic
		public string Name { get { return "Palette Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.2"; } }
		public string Description { get { return "Views the palette an image archive uses."; } }
		public EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		public bool Dominant { get { return false; } }

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

		public PaletteViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.SubArchives = new int[] { 1, 4, 6 };
			StaticFileExtensions = "";
			FileExtensions = "";

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

			ToolControls = new Control[0];

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
			RSImage image = new RSImage(Cache, Node, Data, 0);
			((PictureBox)Controls[0]).Image = image.GeneratePaletteBitmap();
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
				FileExtensions = "";
			}
		}

		public bool OnImport(string filename)
		{
			return false;
		}

		public void OnExport(string filename)
		{
		}

		public void OnConfigure()
		{
		}
	}
}