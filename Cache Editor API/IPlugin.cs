using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cache_Editor_API
{
	public interface IPlugin
	{
		//basic
		string Name { get; }
		string Author { get; }
		string Version { get; }
		string Description { get; }
		EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		bool Dominant { get; } //determines where the plugin gets added on the tabs

		//properties to be filled in by the shell
		CacheItemNode Node { get; set; }
		DataBuffer Data { get; set; }
		Cache Cache { get; set; }

		//optional
		Control[] Controls { get; set; }
		Control[] ToolControls { get; set; }
		string StaticFileExtensions { get; set; }
		string FileExtensions { get; set; }
		Form ConfigureForm { get; set; }

		void PluginSelected();
		bool OnImport(string filename);
		void OnExport(string filename);
		void OnConfigure();
	}
}
