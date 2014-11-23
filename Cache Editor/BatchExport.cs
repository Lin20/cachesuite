using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache_Editor_API;
using System.Threading;

namespace Cache_Editor
{
	public struct PluginSelection
	{
		public IPlugin Plugin;
		public string Extension;

		public PluginSelection(IPlugin p = null, string ext = "")
		{
			Plugin = p;
			Extension = ext;
		}
	}

	public partial class BatchExport : Form
	{
		public Cache Cache { get; set; }
		List<PluginSelection> c_plugins;
		private FolderBrowserDialog folder;

		public BatchExport(Cache cache, PluginContainer container)
		{
			InitializeComponent();
			Cache = cache;
			folder = new FolderBrowserDialog();

			foreach (string s in Cache.ArchiveNames)
				cboArchive.Items.Add(s);
			foreach (string s in Cache.SubNames)
				cboSub.Items.Add(s);
			cboSub.SelectedIndex = 0;
			cboArchive.SelectedIndex = 0;

			c_plugins = new List<PluginSelection>();
			foreach (IPlugin p in container.Plugins)
			{
				if (p.Classifications != null && (p.Classifications.Filenames.Length != 0 || p.Classifications.FileExtensions.Length != 0))
					continue;
				string[] tes = p.StaticFileExtensions.Split('|');
				for (int i = 0; i < tes.Length; i += 2)
				{
					if (tes[i] != "")
					{
						cboType.Items.Add(tes[i] + " [" + p.Name + "]");
						c_plugins.Add(new PluginSelection(p, tes[i + 1]));
					}
				}
			}

			if (cboType.Items.Count == 0)
			{
				MessageBox.Show("No suitable plugins for exporting data found.", "Cannot Export Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			cboType.SelectedIndex = 0;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (txtExt.Text.Length == 0)
			{
				txtExt.Text = c_plugins[cboType.SelectedIndex].Extension;
			}

			IPlugin p = c_plugins[cboType.SelectedIndex].Plugin;
			p.Cache = Cache;
			int archive = (rbArchive.Checked ? cboArchive.SelectedIndex : 0);
			int sub = (rbSub.Checked ? cboSub.SelectedIndex : -1);

			if (p.Classifications != null)
			{
				if (p.Classifications.Archives.Length != 0 && !p.Classifications.Archives.Any(archive.Equals))
					goto Failed;
				if (p.Classifications.SubArchives.Length != 0 && !p.Classifications.SubArchives.Any(sub.Equals))
					goto Failed;
			}

			try
			{
				CacheItemNode n = new CacheItemNode("", "", archive, sub, 0, 0);

				if (rbArchive.Checked)
				{
					progressBar1.Maximum = Cache.Archives[cboArchive.SelectedIndex].GetFileCount();
					progressBar1.Value = 0;
					for (int i = 0; i < Cache.Archives[cboArchive.SelectedIndex].GetFileCount(); i++)
					{
						string filename = txtDest.Text + System.IO.Path.DirectorySeparatorChar + txtExt.Text;
						filename = filename.Replace("*", i.ToString());
						n.FileIndex = i;
						p.Node = n;
						p.Data = new DataBuffer(Cache.Archives[cboArchive.SelectedIndex].ExtractFile(i));
						if (p.Data.Buffer != null && p.Data.Buffer.Length > 0)
							p.OnExport(filename);
						else
						{
							try
							{
								System.IO.File.Create(filename);
							}
							catch (Exception)
							{
							}
						}

						progressBar1.Value++;
						Application.DoEvents();
					}
				}
				else
				{
					progressBar1.Maximum = Cache.SubArchives[cboSub.SelectedIndex].FileCount;
					progressBar1.Value = 0;
					for (int i = 0; i < Cache.SubArchives[cboSub.SelectedIndex].FileCount; i++)
					{
						string name;
						if (!FilesLoader.Files.TryGetValue(Cache.SubArchives[sub].FileNames[i], out name))
						{
							name = i.ToString();
						}
						string filename = txtDest.Text + System.IO.Path.DirectorySeparatorChar + txtExt.Text;
						filename = filename.Replace("*", name);
						n.FileIndex = i;
						p.Node = n;
						p.Data = Cache.SubArchives[sub].ExtractFile(i);
						if (p.Data.Buffer != null && p.Data.Buffer.Length > 0)
							p.OnExport(filename);

						progressBar1.Value++;
						Application.DoEvents();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error dumping data.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Dumping Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			progressBar1.Value = 0;
			return;

		Failed:
			MessageBox.Show("The export type is not supported for the selected files.", "Cannot Dump", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void cboType_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtExt.Text = c_plugins[cboType.SelectedIndex].Extension;
		}

		private void cboSub_SelectedIndexChanged(object sender, EventArgs e)
		{
			rbSub.Checked = true;
		}

		private void BatchExport_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void cboArchive_SelectedIndexChanged(object sender, EventArgs e)
		{
			rbArchive.Checked = true;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (folder.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;
			txtDest.Text = folder.SelectedPath;
			button1.Enabled = true;
		}
	}
}
