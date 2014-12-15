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

namespace NPCViewer
{
	public class NPCViewer : IPlugin
	{
		//basic
		public string Name { get { return "NPC Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.0"; } }
		public string Description { get { return "Views 317-format NPCs."; } }
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
		private PropertyGrid properties;
		private ModelViewer model_viewer;

		private int last_npc = -1;
		private bool initialized;

		public NPCViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.Filenames = new string[] { "npc.dat" };
			FileExtensions = "";
			StaticFileExtensions = "";
			ConfigureForm = new Configure();

			Window window = new Window();
			this.Controls = new Control[window.Controls.Count];
			for (int i = 0; i < window.Controls.Count; i++)
				this.Controls[i] = window.Controls[i];
			name_label = (Label)window.Controls.Find("lblName", true)[0];
			properties = (PropertyGrid)window.Controls.Find("prop_item", true)[0];
			model_viewer = (ModelViewer)window.Controls.Find("modelViewer", true)[0];
			properties.PropertyValueChanged += properties_PropertyValueChanged;

			name_label.Text = "Select an NPC";

			ItemToolbox toolbox = new ItemToolbox();
			ToolControls = new Control[toolbox.Controls.Count];
			for (int i = 0; i < ToolControls.Length; i++)
				ToolControls[i] = toolbox.Controls[i];
			NumericUpDown n_item = (NumericUpDown)toolbox.Controls.Find("nItem", true)[0];

			n_item.ValueChanged += delegate(object sender, EventArgs e) { SelectNPC((int)n_item.Value, true); };

			checkerboard = new Bitmap(32, 32);
			Graphics g = Graphics.FromImage(checkerboard);
			g.Clear(Color.White);
			g.FillRectangle(Brushes.LightGray, 0, 0, 16, 16);
			g.FillRectangle(Brushes.LightGray, 16, 16, 16, 16);

			last_npc = 0;
			initialized = false;
		}

		public void PluginSelected()
		{
			if (!initialized)
			{
				Cache_Editor_API.Graphics3D.Rasterizer.LoadTextures(Cache);
				Cache_Editor_API.Graphics3D.Rasterizer.ApplyBrightness(0.59999999999999998D);
				Cache_Editor_API.Graphics3D.Rasterizer.ResetTextures();
				Cache_Editor_API.Config.NPCDefinition.UnpackNPCs(Cache, Data, Cache.SubArchives[2].ExtractFile("npc.idx"));
				initialized = true;
			}
			model_viewer.UpdateBuffer();

			SelectNPC(last_npc, true, true);
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
			model_viewer.DrawModel();
		}

		public void SelectNPC(int index, bool update_rotation = false, bool force = false)
		{
			NPCDefinition item = NPCDefinition.GetNPC(index);
			properties.SelectedObject = item;
			name_label.Text = item.Name + " (" + item.ID + ")";

			if (force || last_npc != index)
			{
				model_viewer.SelectedModel = item.GetModel(0, 0, null);

			}
			else
			{
				model_viewer.SetModel(item.GetModel(0, 0, null), false);
			}
			last_npc = index;
		}

		private void properties_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			ChangesModel changes_model = (ChangesModel)e.ChangedItem.PropertyDescriptor.Attributes[typeof(ChangesModel)];
			if (changes_model != null)
			{
				if (changes_model.Changes)
				{
					SelectNPC(last_npc);
				}
			}
		}
	}
}