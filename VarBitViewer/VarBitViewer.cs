using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Cache_Editor_API;
using Cache_Editor_API.Config;

namespace VarBitViewer
{
	public class VarBitViewer : IPlugin
	{
		//basic
		public string Name { get { return "VarBit Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.0"; } }
		public string Description { get { return "Views 317-format VarBit."; } }
		public EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		public bool Dominant { get { return true; } }

		//properties to be filled in by the shell
		public CacheItemNode Node { get; set; }
		public DataBuffer Data { get; set; }
		public Cache Cache { get; set; }

		//optional
		public Control[] Controls { get; set; }
		public Control[] ToolControls { get; set; }
		public string FileExtensions { get; set; }
		public string StaticFileExtensions { get; set; }
		public Form ConfigureForm { get; set; }

		private PropertyGrid properties;
		private bool initialized = false;
		private int last_var;

		public VarBitViewer()
		{
			Classifications = new EditorClassifications();
			Classifications.Filenames = new string[] { "varbit.dat" };
			FileExtensions = "";
			StaticFileExtensions = "";

			Controls = new Control[1];
			properties = new PropertyGrid();
			properties.Dock = DockStyle.Fill;
			Controls[0] = properties;

			ItemToolbox toolbox = new ItemToolbox();
			ToolControls = new Control[toolbox.Controls.Count];
			for (int i = 0; i < ToolControls.Length; i++)
				ToolControls[i] = toolbox.Controls[i];
			NumericUpDown n_item = (NumericUpDown)toolbox.Controls.Find("nItem", true)[0];

			n_item.ValueChanged += delegate(object sender, EventArgs e) { SelectVarBit((int)n_item.Value); };

			initialized = false;
			last_var = 0;
		}

		public void PluginSelected()
		{
			if (!initialized)
			{
				VarBit.UnpackVariables(Cache, Data);
				initialized = true;
			}

			SelectVarBit(last_var);
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

		public void SelectVarBit(int index)
		{
			properties.SelectedObject = VarBit.GetVarBit(index);
		}
	}
}