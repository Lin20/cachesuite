using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;
using Cache_Editor_API.Graphics3D;

namespace ModelViewer
{
	public class ModelViewer : IPlugin
	{
		//basic
		public string Name { get { return "Model Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.1"; } }
		public string Description { get { return "Views and allows the basic importing and exporting of 317-format models."; } }
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

		private static bool initialized;
		private Cache_Editor_API.Controls.ModelViewer model_viewer;

		public ModelViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.Archives = new int[] { 1 };
			StaticFileExtensions = "";
			FileExtensions = "";
			ConfigureForm = new Configure();

			Controls = new Control[1];
			model_viewer = new Cache_Editor_API.Controls.ModelViewer();
			model_viewer.Dock = DockStyle.Fill;
			Controls[0] = model_viewer;
			initialized = false;

			ToolControls = new Control[0];
			/*ToolboxControls t = new ToolboxControls();
			ToolControls = new Control[t.Controls[0].Controls.Count];
			for (int i = 0; i < t.Controls[0].Controls.Count; i++)
				ToolControls[i] = t.Controls[0].Controls[i];*/

		}

		public void PluginSelected()
		{
			if (!initialized)
			{
				initialized = true;
				Rasterizer.LoadTextures(Cache);
				Rasterizer.ApplyBrightness(0.59999999999999998D);
				Rasterizer.ResetTextures();
			}

			model_viewer.UpdateBuffer();

			try
			{
				Model.LoadModel(Data.Buffer, Node.FileIndex);
				Model model = new Model(Node.FileIndex);
				model.Light(64, 768, -50, -10, -50, true);
				model_viewer.SelectedModel = model;
			}
			catch (Exception)
			{
				model_viewer.SelectedModel = null;
			}
		}

		public bool OnImport(string filename)
		{
			try
			{
				Model.LoadModel(Data.Buffer, Node.FileIndex);
				Model model = new Model(Node.FileIndex);
				model.Light(64, 768, -50, -10, -50, true);
				model_viewer.SelectedModel = model;
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void OnExport(string filename)
		{
		}

		public void OnConfigure()
		{
			Cache_Editor_API.Controls.ModelViewer.SoftwareRendering = ((Configure)ConfigureForm).checkBox1.Checked;
			if (Cache_Editor_API.Controls.ModelViewer.SoftwareRendering)
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
	}
}