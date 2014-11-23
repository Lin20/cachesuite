using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache_Editor_API;
using System.IO;

namespace HexEditor
{
	public class HexEditor : IPlugin
	{
		//basic
		public string Name { get { return "Hex Editor"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.2"; } }
		public string Description { get { return "A basic hex editor."; } }
		public EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		public bool Dominant { get { return false; } }

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

		public HexEditor()
		{
			Classifications = new EditorClassifications();
			FileExtensions = "Data file (*.dat)|*.dat";
			StaticFileExtensions = "Data file (*.dat)|*.dat";

			Controls = new Control[1];
			Be.Windows.Forms.HexBox box = new Be.Windows.Forms.HexBox();
			box.StringViewVisible = true;
			box.UseFixedBytesPerLine = true;
			box.LineInfoVisible = true;
			box.VScrollBarVisible = true;
			box.Dock = DockStyle.Fill;
			Controls[0] = box;

			ToolControls = new Control[0];
		}

		public void PluginSelected()
		{
			Be.Windows.Forms.HexBox box = (Be.Windows.Forms.HexBox)Controls[0];
			Be.Windows.Forms.ByteCollection c = new Be.Windows.Forms.ByteCollection(Data.Buffer);
			Be.Windows.Forms.DynamicByteProvider d = new Be.Windows.Forms.DynamicByteProvider(Data.Buffer);
			((Be.Windows.Forms.HexBox)Controls[0]).ByteProvider = d;
		}

		public bool OnImport(string filename)
		{
			try
			{
				BinaryReader br = new BinaryReader(File.OpenRead(filename));
				byte[] data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				if (Data != null)
				{
					Data.Buffer = data;
					Data.Location = 0;
					PluginSelected();
				}
				else
					Data = new DataBuffer(data);
				Cache.WriteFile(Node, data);
				return true;
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error importing file.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Importing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		public void OnExport(string filename)
		{
			try
			{
				if (File.Exists(filename))
					File.Delete(filename);
				BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate));
				bw.Write(Data.Buffer);
				bw.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error exporting file.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Exporting File", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void OnConfigure()
		{
		}
	}
}
