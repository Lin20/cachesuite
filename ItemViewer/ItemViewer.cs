using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;
using Cache_Editor_API.Definitions;
using Cache_Editor_API.Graphics3D;
using Cache_Editor_API.Controls;

namespace ItemViewer
{
	public class ItemViewer : IPlugin
	{
		//basic
		public string Name { get { return "Item Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.0"; } }
		public string Description { get { return "Views 317-format items."; } }
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
		private Label name_label;
		private PictureBox pic_item_small;
		private PictureBox pic_item_large;
		private PropertyGrid properties;
		private ModelViewer model_viewer;

		private int last_item;
		private int last_amount;

		public ItemViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.Filenames = new string[] { "obj.dat" };
			FileExtensions = "";
			StaticFileExtensions = "";
			ConfigureForm = new Configure();

			Window window = new Window();
			this.Controls = new Control[window.Controls.Count];
			for (int i = 0; i < window.Controls.Count; i++)
				this.Controls[i] = window.Controls[i];
			name_label = (Label)window.Controls.Find("lblName", true)[0];
			pic_item_small = (PictureBox)window.Controls.Find("pItemSmall", true)[0];
			pic_item_large = (PictureBox)window.Controls.Find("pItemLarge", true)[0];
			properties = (PropertyGrid)window.Controls.Find("prop_item", true)[0];
			model_viewer = (ModelViewer)window.Controls.Find("modelViewer", true)[0];

			name_label.Text = "Select an item";

			/*Controls = new Control[2];
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
			Controls[0] = image;*/

			ItemToolbox toolbox = new ItemToolbox();
			ToolControls = new Control[toolbox.Controls.Count];
			for (int i = 0; i < ToolControls.Length; i++)
				ToolControls[i] = toolbox.Controls[i];
			NumericUpDown n_item = (NumericUpDown)toolbox.Controls.Find("nItem", true)[0];
			NumericUpDown n_amount = (NumericUpDown)toolbox.Controls.Find("nAmount", true)[0];
			n_item.ValueChanged += delegate(object sender, EventArgs e) { SelectItem((int)n_item.Value, (int)n_amount.Value, true); };
			n_amount.ValueChanged += delegate(object sender, EventArgs e) { SelectItem((int)n_item.Value, (int)n_amount.Value); };

			checkerboard = new Bitmap(32, 32);
			Graphics g = Graphics.FromImage(checkerboard);
			g.Clear(Color.White);
			g.FillRectangle(Brushes.LightGray, 0, 0, 16, 16);
			g.FillRectangle(Brushes.LightGray, 16, 16, 16, 16);

			pic_item_small.BackgroundImage = checkerboard;
			pic_item_large.BackgroundImage = checkerboard;
			last_item = 0;
			last_amount = 1;
		}

		public void PluginSelected()
		{
			Cache_Editor_API.Graphics3D.DrawingArea.initDrawingArea(512, 512, new Color[512 * 512]);
			Cache_Editor_API.Graphics3D.Rasterizer.LoadTextures(Cache);
			Cache_Editor_API.Graphics3D.Rasterizer.ApplyBrightness(0.59999999999999998D);
			Cache_Editor_API.Graphics3D.Rasterizer.ResetTextures();
			Cache_Editor_API.Definitions.ItemDefinition.UnpackItems(Cache, Cache.SubArchives[2].ExtractFile("obj.dat"), Cache.SubArchives[2].ExtractFile("obj.idx"));

			SelectItem(last_item, last_amount, true);
		}

		public bool OnImport(string filename)
		{
			return true;
		}

		public void OnExport(string filename)
		{
		}

		public void OnConfigure()
		{
			ModelViewer.SoftwareRendering = ((Configure)ConfigureForm).checkBox1.Checked;
			if (ModelViewer.SoftwareRendering)
			{
				Controls[1].Visible = false;
				Controls[2].Visible = true;
			}
			else
			{
				Controls[1].Visible = true;
				Controls[2].Visible = false;
			}
			try
			{
				SelectItem(last_item, last_amount);
			}
			catch (Exception)
			{
			}
		}

		public void SelectItem(int index, int amount, bool update_rotation = false)
		{
			ItemDefinition item = ItemDefinition.GetItem(index);
			properties.SelectedObject = item;
			name_label.Text = item.Name + " (" + item.ID + ")";
			pic_item_small.Image = pic_item_large.Image = Cache_Editor_API.Definitions.ItemDefinition.GetModelSprite(index, amount, 0).GenerateBitmap();

			if (model_viewer.SelectedModel == null || model_viewer.SelectedModel.ID != item.GetModelIndex(amount))
			{
				model_viewer.SelectedModel = item.GetModel(amount);
				if (last_item != index || update_rotation)
				{
					model_viewer.Yaw = item.RotationX;
					model_viewer.Pitch = item.RotationY;
					model_viewer.DrawModel();
				}
			}
			last_item = index;
		}
	}
}