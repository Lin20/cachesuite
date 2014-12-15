using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;
using Cache_Editor_API.Config;
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
		private CheckBox lock_sprite;

		private int last_item;
		private int last_amount;
		private bool initialized;

		public ItemViewer()
		{
			try
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
				model_viewer.Controls[1].MouseUp += model_viewer_MouseUp;
				model_viewer.software_control.MouseUp += model_viewer_MouseUp;
				properties.PropertyValueChanged += properties_PropertyValueChanged;

				name_label.Text = "Select an item";

				ItemToolbox toolbox = new ItemToolbox();
				ToolControls = new Control[toolbox.Controls.Count];
				for (int i = 0; i < ToolControls.Length; i++)
					ToolControls[i] = toolbox.Controls[i];
				NumericUpDown n_item = (NumericUpDown)toolbox.Controls.Find("nItem", true)[0];
				NumericUpDown n_amount = (NumericUpDown)toolbox.Controls.Find("nAmount", true)[0];
				lock_sprite = (CheckBox)toolbox.Controls.Find("chkModelSprite", true)[0];

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

				initialized = false;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}

		public void PluginSelected()
		{
			if (!initialized)
			{
				Cache_Editor_API.Graphics3D.Rasterizer.LoadTextures(Cache);
				Cache_Editor_API.Graphics3D.Rasterizer.ApplyBrightness(0.59999999999999998D);
				Cache_Editor_API.Graphics3D.Rasterizer.ResetTextures();
				Cache_Editor_API.Config.ItemDefinition.UnpackItems(Cache, Data, Cache.SubArchives[2].ExtractFile("obj.idx"));
				initialized = true;
			}

			model_viewer.UpdateBuffer();

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
				model_viewer.Controls[1].Visible = false;
				model_viewer.Controls[2].Visible = true;
			}
			else
			{
				model_viewer.Controls[1].Visible = true;
				model_viewer.Controls[2].Visible = false;
			}
			//SelectItem(last_item, last_amount);
			model_viewer.DrawModel();
		}

		public void SelectItem(int index, int amount, bool update_rotation = false)
		{
			ItemDefinition item = ItemDefinition.GetItem(index);
			properties.SelectedObject = item;
			name_label.Text = item.Name + " (" + item.ID + ")";
			pic_item_small.Image = pic_item_large.Image = Cache_Editor_API.Config.ItemDefinition.GetModelSprite(index, amount, 0).GenerateBitmap();

			if (last_item != index || model_viewer.SelectedModel == null || model_viewer.SelectedModel.ID != item.GetModelIndex(amount))
			{
				model_viewer.SelectedModel = item.GetModel(amount);
				if (last_item != index || update_rotation)
				{
					model_viewer.Yaw = item.RotationX;
					model_viewer.Pitch = item.RotationY;
					model_viewer.Roll = item.RotationZ;
					model_viewer.DrawModel();
				}
			}
			last_item = index;
		}

		private void properties_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			ChangesItemSprite changes_sprite = (ChangesItemSprite)e.ChangedItem.PropertyDescriptor.Attributes[typeof(ChangesItemSprite)];
			if (changes_sprite != null)
			{
				if (changes_sprite.Changes)
				{
					pic_item_small.Image = pic_item_large.Image = Cache_Editor_API.Config.ItemDefinition.GetModelSprite(last_item, last_amount, 0).GenerateBitmap();
				}
			}

			ChangesModel changes_model = (ChangesModel)e.ChangedItem.PropertyDescriptor.Attributes[typeof(ChangesModel)];
			if (changes_model != null)
			{
				if (changes_model.Changes)
				{
					model_viewer.SetModel(((ItemDefinition)properties.SelectedObject).GetModel(last_amount), false);
				}
			}
		}

		private void model_viewer_MouseUp(object sender, MouseEventArgs e)
		{
			if (lock_sprite.Checked)
			{
				ItemDefinition def = ((ItemDefinition)properties.SelectedObject);
				def.RotationX = model_viewer.Yaw;
				def.RotationY = model_viewer.Pitch;
				def.RotationZ = model_viewer.Roll;
				properties.SelectedObject = def;
				pic_item_small.Image = pic_item_large.Image = Cache_Editor_API.Config.ItemDefinition.GetModelSprite(last_item, last_amount, 0).GenerateBitmap();
			}
		}
	}
}