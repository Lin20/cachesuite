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
using Cache_Editor_API.Graphics3D;
using System.IO;
using System.Reflection;

namespace Cache_Editor
{
	public partial class Form1 : Form
	{
		public Cache Cache { get; set; }
		public PluginContainer Plugins { get; set; }

		private int search_count;
		private InputBox search_box;

		private CacheItemNode selected_node;
		private Color last_highlight_color;
		private CacheItemNode last_highlight_node;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SetBLE(false);
			if (!FilesLoader.Load())
			{
				MessageBox.Show("Failed to load file names.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			if (Environment.OSVersion.Platform != PlatformID.Win32NT && Environment.OSVersion.Platform != PlatformID.Win32S && Environment.OSVersion.Platform != PlatformID.Win32Windows && Environment.OSVersion.Platform != PlatformID.WinCE)
			{
				file_browser.Font = new System.Drawing.Font("Arial", 8.25f);
				lblFile.Font = new Font("Arial", 8.25f);
			}
			Plugins = new PluginContainer();
			Plugins.LoadPlugins();
			lblPlugins.Text = Plugins.Plugins.Count + " Plugin" + (Plugins.Plugins.Count != 1 ? "s" : "") + " Loaded";

		}

		private void openCacheFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog f = new FolderBrowserDialog();
			f.SelectedPath = "C:\\rscache\\";
			if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			tbPlugins.TabPages.Clear();
			if (Cache != null)
				Cache.Close();
			Cache = new Cache();

			try
			{
				Cache.LoadCache(f.SelectedPath + Path.DirectorySeparatorChar);
				SetBLE(true);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading cache.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SetBLE(false);
				return;
			}

			PopulateFileBrowser();
		}

		private void SetBLE(bool b)
		{
			searchFileToolStripMenuItem.Enabled = b;
			saveToolStripMenuItem.Enabled = b;
			newFileToolStripMenuItem.Enabled = b;
			newSubArchiveToolStripMenuItem.Enabled = b;
			refreshToolStripMenuItem.Enabled = b;
			batchExportToolStripMenuItem.Enabled = b;
			tbPlugins.AllowDrop = b;
			file_browser.AllowDrop = b;
			//AllowDrop = b;
		}

		private void PopulateFileBrowser()
		{
			file_browser.Nodes.Clear();
			for (int i = 0; i < 5; i++)
			{
				CacheItemNode node = new CacheItemNode(Cache.Archives[i].ArchiveName + " (" + Cache.Archives[i].GetFileCount() + " files)", Cache.Archives[i].ArchiveName, i, -1, -1, 0);
				for (int k = 0; k < Cache.Archives[i].GetFileCount(); k++)
				{
					if (i > 0)
					{
						node.Nodes.Add(NewNode(i, -1, k));
					}
					else
					{
						byte[] buf = Cache.Archives[0].ExtractFile(k);
						Cache.SubArchives.Add(new SubArchive(Cache.Archives[0].ExtractFile(k)));
						string s_name = "Sub-archive " + k;
						if (k < Cache.SubNames.Length)
						{
							s_name = Cache.SubNames[k];
						}
						CacheItemNode sub = new CacheItemNode(s_name + " (" + Cache.SubArchives[k].FileCount + " files)", s_name, i, k, -1, 0);
						for (int j = 0; j < Cache.SubArchives[k].FileCount; j++)
						{
							sub.Nodes.Add(NewNode(i, k, j));
						}
						node.Nodes.Add(sub);
					}
				}
				file_browser.Nodes.Add(node);
			}
		}

		private CacheItemNode NewNode(int archive, int sub_archive, int file_index)
		{
			if (archive > 0)
			{
				string name = "File " + file_index;
				if (archive == 4)
				{
					Point p = Map.GetCoordinates(Cache.SubArchives[5], file_index);
					if (p.X >= 0 && p.Y >= 0)
						name = p.X + ", " + p.Y;
				}
				return new CacheItemNode(name + " (" + GetSizeLabel(Cache.Archives[archive].GetFileSize(file_index)) + ")", name, archive, -1, file_index, Cache.Archives[archive].GetFileSize(file_index));
			}
			else if (sub_archive >= 0)
			{
				string name;
				if (!FilesLoader.Files.TryGetValue(Cache.SubArchives[sub_archive].FileNames[file_index], out name))
				{
					name = "File " + file_index;
				}
				return new CacheItemNode(name + " (" + GetSizeLabel(Cache.SubArchives[sub_archive].UncompressedFileSizes[file_index]) + ")", name, archive, sub_archive, file_index, Cache.SubArchives[sub_archive].UncompressedFileSizes[file_index]);
			}

			return null;
		}

		private void searchFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (search_box == null)
				search_box = new InputBox("Search File", 3, "File name:", "Archive index (optional):", "Sub-archive (optional):");
			((TextBox)search_box.Controls[0]).SelectAll();
			if (search_box.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			string file = search_box.GetText(0);
			search_count = 0;

			int archive_index = -1;
			if (search_box.GetText(1) != "")
			{
				int.TryParse(search_box.GetText(1), out archive_index);
				if (archive_index >= Cache.Archives.Length)
				{
					lblSearch.Text = "Archive index too big";
					return;
				}
			}

			int sub_archive = -1;
			if (search_box.GetText(2) != "")
			{
				if (!int.TryParse(search_box.GetText(2), out sub_archive))
					sub_archive = -1;
			}

			file = file.ToUpper();
			int file_int = 0;
			for (int j = 0; j < file.Length; j++)
				file_int = (file_int * 61 + file[j]) - 32;

			foreach (CacheItemNode n in file_browser.Nodes)
				ResetSearch(n);
			if (archive_index != -1)
				Search((CacheItemNode)file_browser.Nodes[archive_index], file_int, archive_index, sub_archive, file.ToLower());
			else if (sub_archive != -1)
				Search((CacheItemNode)file_browser.Nodes[0], file_int, archive_index, sub_archive, file.ToLower());
			else
			{
				foreach (CacheItemNode n in file_browser.Nodes)
					Search(n, file_int, archive_index, sub_archive, file.ToLower());
			}
			lblSearch.Text = "Found " + search_count + " result" + (search_count != 1 ? "s" : "") + " for \"" + search_box.GetText(0) + "\"";
		}

		private void ResetSearch(CacheItemNode node)
		{
			node.BackColor = SystemColors.Window;
			foreach (CacheItemNode n in node.Nodes)
				ResetSearch(n);
		}

		private bool Search(CacheItemNode node, int file, int archive, int sub, string file_text)
		{
			if (node.FileIndex != -1)
			{
				if ((node.SubArchive != -1 && Cache.SubArchives[node.SubArchive].GetFileIndex(file) == node.FileIndex) || node.Name.ToLower().Contains(file_text))
				{
					node.BackColor = Color.Cyan;
					//node.Text = search_box.GetText(0).ToLower();
					search_count++;
					if (search_count == 1)
						file_browser.SelectedNode = node;
					return true;
				}
				return false;
			}

			if (archive != -1 && node.Archive != archive)
				return false;
			if (sub != -1 && node.SubArchive != sub && node.SubArchive != -1)
				return false;

			bool found = false;
			foreach (CacheItemNode archive_node in node.Nodes)
			{
				if (Search(archive_node, file, archive, sub, file_text))
				{
					node.BackColor = Color.Cyan;
					found = true;
				}
			}

			return found;
		}

		private string GetSizeLabel(int amount)
		{
			if (amount < Math.Pow(2, 10))
				return amount + " bytes";
			if (amount < Math.Pow(2, 20))
				return (int)(amount / Math.Pow(2, 10)) + " KB";
			return (int)(amount / Math.Pow(2, 20)) + " MB";
		}

		private void DeterminePlugins()
		{
			tbPlugins.TabPages.Clear();
			CacheItemNode node = (CacheItemNode)file_browser.SelectedNode;
			foreach (IPlugin p in Plugins.Plugins)
			{
				if (p.Classifications != null)
				{
					if (p.Classifications.Archives.Length != 0 && !p.Classifications.Archives.Any(node.Archive.Equals))
						continue;
					if (p.Classifications.SubArchives.Length != 0 && !p.Classifications.SubArchives.Any(node.SubArchive.Equals))
						continue;
					if (p.Classifications.Filenames.Length != 0)
					{
						bool found = false;
						foreach (string s in p.Classifications.Filenames)
						{
							if (node.Name.ToLower() == s.ToLower())
							{
								found = true;
								break;
							}
						}
						if (!found)
							continue;
					}
					if (p.Classifications.FileExtensions.Length != 0)
					{
						int ext = node.Name.LastIndexOf(".");
						if (ext == -1)
							continue;
						string s = node.Name.Substring(ext + 1).ToLower();
						bool found = false;
						foreach (string e in p.Classifications.FileExtensions)
						{
							if (e.ToLower() != s)
								continue;
							found = true;
							break;
						}
						if (!found)
							continue;
					}
				}

				p.Cache = Cache;
				p.Data = new DataBuffer(GetNodeData(node));
				p.Node = node;
				PluginTabPage page = new PluginTabPage(p.Name, p);
				foreach (Control c in p.Controls)
					page.Controls.Add(c);

				if (p.Dominant)
					tbPlugins.TabPages.Insert(0, page);
				else
					tbPlugins.TabPages.Add(page);
			}
			tbPlugins.SelectedIndex = (tbPlugins.TabPages.Count > 0 ? 0 : -1);
			file_browser.Focus();
			tbPlugins_SelectedIndexChanged(this, null);
		}

		private byte[] GetNodeData(CacheItemNode node)
		{
			byte[] b = null;
			if (node.SubArchive == 0)
				return new byte[0];
			if (node.SubArchive == -1)
			{
				b = Cache.Archives[node.Archive].ExtractFile(node.FileIndex);
			}
			else
			{
				b = Cache.SubArchives[node.SubArchive].ExtractFile(node.FileIndex).Buffer;
			}
			return b;
		}

		private void UpdateFileInfo()
		{
			StringBuilder text = new StringBuilder();
			text.AppendLine("File: " + selected_node.Name);
			text.AppendLine("Size: " + selected_node.Size + " bytes");
			bool compressed = false;
			if (selected_node.SubArchive != -1)
			{
				compressed = !Cache.SubArchives[selected_node.SubArchive].ArchiveDecompressed;
			}
			if (selected_node.SubArchive != -1)
			{
				text.AppendLine("Compression: " + (compressed ? "BZip2 (file)" : "BZip2 (archive)"));
			}
			else
				text.AppendLine("Compresion: GZip");
			string name_hash = "N/A";
			if (selected_node.SubArchive != -1)
				name_hash = Cache.SubArchives[selected_node.SubArchive].FileNames[selected_node.FileIndex].ToString();
			text.AppendLine("Name Hash: " + name_hash);
			lblFile.Text = text.ToString();
		}

		private void file_browser_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node == null)
				return;
			if (((CacheItemNode)e.Node).SubArchive != -1)
				saveCurrentToolStripMenuItem.Enabled = true;
			else
				saveCurrentToolStripMenuItem.Enabled = false;
			if (e.Node.Nodes.Count > 0 || ((CacheItemNode)e.Node).FileIndex == -1)
				return;

			selected_node = (CacheItemNode)e.Node;
			btnImport.Enabled = true;
			btnExport.Enabled = true;
			if (selected_node.SubArchive != -1)
			{
				btnRename.Enabled = true;
			}
			else
			{
				btnRename.Enabled = false;
				saveCurrentToolStripMenuItem.Enabled = false;
			}

			DeterminePlugins();
			UpdateFileInfo();
		}

		private void tbPlugins_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tbPlugins.SelectedIndex != -1)
			{
				groupToolbox.Controls.Clear();
				foreach (Control c in ((PluginTabPage)tbPlugins.TabPages[tbPlugins.SelectedIndex]).Plugin.ToolControls)
					groupToolbox.Controls.Add(c);
				((PluginTabPage)tbPlugins.TabPages[tbPlugins.SelectedIndex]).Plugin.PluginSelected();
			}
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			if (tbPlugins.SelectedIndex != -1 && selected_node != null)
			{
				StringBuilder filter = new StringBuilder();
				foreach (PluginTabPage t in tbPlugins.TabPages)
				{
					if (t.Plugin.FileExtensions != "")
					{
						if (filter.Length > 0 && !filter.ToString().EndsWith("|"))
							filter.Append("|");
						filter.Append(t.Plugin.FileExtensions);
					}
				}

				if (filter.Length == 0)
				{
					MessageBox.Show("Sorry, no loaded plugins support exporting.", "Cannot Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				SaveFileDialog o = new SaveFileDialog();
				o.Title = "Export " + selected_node.Name;
				o.Filter = filter.ToString();
				if (o.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;

				string ext = Path.GetExtension(o.FileName).ToLower();
				foreach (PluginTabPage t in tbPlugins.TabPages)
				{
					if (t.Plugin.FileExtensions != "")
					{
						string[] tes = t.Plugin.FileExtensions.Split('|');
						foreach (string s in tes)
						{
							if (s.Replace("*", "").ToLower() == ext.ToLower())
							{
								t.Plugin.OnExport(o.FileName);
								return;
							}
						}
					}
				}

				MessageBox.Show("Failed to find plugin.", "Error Exporting", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PluginSummary summary = new PluginSummary();
			foreach (IPlugin p in Plugins.Plugins)
			{
				TabPage t = new TabPage();
				t.Text = p.Name;
				PluginDescription desc = new PluginDescription();
				desc.SetPlugin(p);
				desc.Dock = DockStyle.Fill;
				t.Controls.Add(desc);
				summary.Tabs.TabPages.Add(t);
			}

			summary.ShowDialog();
		}

		private void btnRename_Click(object sender, EventArgs e)
		{
			InputBox i = new InputBox("Rename \'" + selected_node.Name + "\'", 1, "New name:");
			if (i.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			if (i.GetText(0).Length == 0)
				return;

			selected_node.Text = i.GetText(0);
			int hash = 0;
			string name = i.GetText(0).ToUpper();
			for (int j = 0; j < name.Length; j++)
				hash = (hash * 61 + name[j]) - 32;
			Cache.SubArchives[selected_node.SubArchive].RenameFile(selected_node.FileIndex, hash);
			UpdateFileInfo();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < Cache.SubArchives.Count; i++)
			{
				Cache.SubArchives[i].RewriteArchive();
				byte[] b = Cache.SubArchives[i].CompressedBuffer;
				Cache.Archives[0].WriteFile(i, b, b.Length);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//Cache.Archives[0].DataFile.Close();
			//Cache.Archives[0].IndexFile.Close();
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			if (tbPlugins.SelectedIndex != -1 && selected_node != null)
			{
				StringBuilder filter = new StringBuilder();
				foreach (PluginTabPage t in tbPlugins.TabPages)
				{
					if (t.Plugin.FileExtensions.Length > 0)
					{
						if (filter.Length > 0 && !filter.ToString().EndsWith("|"))
							filter.Append("|");
						filter.Append(t.Plugin.FileExtensions);
					}
				}

				if (filter.Length == 0)
				{
					MessageBox.Show("Sorry, no loaded plugins support importing.", "Cannot Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				OpenFileDialog o = new OpenFileDialog();
				o.Title = "Import " + selected_node.Name;
				o.Filter = filter.ToString();
				if (o.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;

				ImportFile(o.FileName);
			}
		}

		private void saveCurrentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (file_browser.SelectedNode == null || ((CacheItemNode)file_browser.SelectedNode).SubArchive == -1)
				return;
			int i = ((CacheItemNode)file_browser.SelectedNode).SubArchive;
			Cache.SubArchives[i].RewriteArchive();
			byte[] b = Cache.SubArchives[i].CompressedBuffer;
			Cache.Archives[0].WriteFile(i, b, b.Length);
		}

		private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (file_browser.SelectedNode == null)
				return;
			CacheItemNode c = (CacheItemNode)file_browser.SelectedNode;
			if (c.SubArchive == -1 && c.Archive != -1)
			{
				if (!Cache.Archives[c.Archive].CreateFile())
				{
					MessageBox.Show("Failed to create file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				int k = Cache.Archives[c.Archive].GetFileCount() - 1;
				string name = "File " + k;
				if (c.Archive == 4)
				{
					Point p = Map.GetCoordinates(Cache.SubArchives[5], k);
					if (p.X >= 0 && p.Y >= 0)
						name = p.X + ", " + p.Y;
				}
				file_browser.Nodes[c.Archive].Nodes.Add(new CacheItemNode(name + " (" + GetSizeLabel(Cache.Archives[c.Archive].GetFileSize(k)) + ")", name, c.Archive, -1, k, Cache.Archives[c.Archive].GetFileSize(k)));
				file_browser.SelectedNode = file_browser.Nodes[c.Archive].Nodes[k];
				string f = file_browser.Nodes[c.Archive].Text.Substring(0, file_browser.Nodes[c.Archive].Text.IndexOf('('));
				file_browser.Nodes[c.Archive].Text = f + "(" + Cache.Archives[c.Archive].GetFileCount() + " files)";
			}
			else if (c.SubArchive != -1)
			{
				string name = "File " + Cache.SubArchives[c.SubArchive].FileCount;
				Cache.SubArchives[c.SubArchive].CreateFile(name);
				int size = Cache.SubArchives[c.SubArchive].UncompressedFileSizes[Cache.SubArchives[c.SubArchive].FileCount - 1];
				file_browser.Nodes[0].Nodes[c.SubArchive].Nodes.Add(new CacheItemNode(name + " (" + GetSizeLabel(size) + ")", name, c.Archive, c.SubArchive, Cache.SubArchives[c.SubArchive].FileCount - 1, size));
				file_browser.SelectedNode = file_browser.Nodes[0].Nodes[c.SubArchive].Nodes[file_browser.Nodes[0].Nodes[c.SubArchive].Nodes.Count - 1];
				string f = file_browser.Nodes[c.Archive].Nodes[c.SubArchive].Text.Substring(0, file_browser.Nodes[c.Archive].Nodes[c.SubArchive].Text.IndexOf('('));
				file_browser.Nodes[c.Archive].Nodes[c.SubArchive].Text = f + "(" + Cache.SubArchives[c.SubArchive].FileCount + " files)";
			}
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			file_browser.Nodes.Clear();
			PopulateFileBrowser();
		}

		private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string suite_version = "Unknown";
			try
			{
				AssemblyName name = this.GetType().Assembly.GetName();
				suite_version = name.Version.ToString();
			}
			catch (Exception)
			{
			}

			string api_version = "Unknown";
			try
			{
				AssemblyName name = typeof(Cache_Editor_API.Cache).Assembly.GetName();
				api_version = name.Version.ToString();
			}
			catch (Exception)
			{
			}
			string text = "Lin's Cache Suite\nVersion " + suite_version + "\nAPI Version " + api_version + "\n\nCredits:\nThe people behind rename317 (naming)\nSharpZipLib (BZip2 Library)\nbernhardelbl (Be.HexEditor)\nnQuant (Color Quantizer)\nMicrosoft Visual Studio Team (VS2013 Icons)\n\nFor licencing information, look them up specifically.\nI don't care what you do with this.";
			MessageBox.Show(text, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ImportFile(string filename)
		{
			string ext = Path.GetExtension(filename).ToLower();
			foreach (PluginTabPage t in tbPlugins.TabPages)
			{
				if (t.Plugin.FileExtensions != "")
				{
					string[] tes = t.Plugin.FileExtensions.Split('|');
					foreach (string s in tes)
					{
						if (s.Replace("*", "").ToLower() == ext.ToLower())
						{
							if (t.Plugin.OnImport(filename))
							{
								foreach (PluginTabPage z in tbPlugins.TabPages)
								{
									if (z != t)
										z.Plugin.Data.Buffer = GetNodeData(z.Plugin.Node);
								}
								t.Plugin.Node.Size = t.Plugin.Data.Buffer.Length;
								t.Plugin.Node.Text = t.Plugin.Node.Name + " (" + GetSizeLabel(t.Plugin.Node.Size) + ")";
								UpdateFileInfo();
								((PluginTabPage)tbPlugins.SelectedTab).Plugin.PluginSelected();
							}
							return;
						}
					}
				}
			}

			MessageBox.Show("Failed to find plugin.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		//ugh this function is a nightmare
		private void CreateAndWriteFile(string filename, int archive, int sub_archive = -1)
		{
			if (archive == -1 && sub_archive == -1)
				return;

			bool found = false;
			if (sub_archive == -1 && archive > 0)
			{
				if (!Cache.Archives[archive].CreateFile())
				{
					MessageBox.Show("Failed to create file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			else if (sub_archive != -1)
			{
				if (!Cache.SubArchives[sub_archive].FileExists(Path.GetFileName(filename)))
					Cache.SubArchives[sub_archive].CreateFile(Path.GetFileName(filename));
				else
				{
					if (MessageBox.Show(Path.GetFileName(filename) + " already exists. Would you like to replace it?", "Confirm Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
						return;
					found = true;
				}
			}
			else
				return;

			CacheItemNode n = null;
			if (sub_archive == -1)
				n = NewNode(archive, sub_archive, Cache.Archives[archive].GetFileCount() - 1);
			else
			{
				if (!found)
				{
					n = NewNode(0, sub_archive, Cache.SubArchives[sub_archive].FileCount - 1);
					n.Name = Path.GetFileName(filename);
				}
				else
				{
					int name = Cache.SubArchives[sub_archive].GetHashCode(Path.GetFileName(filename));
					foreach (CacheItemNode node in file_browser.Nodes[0].Nodes[sub_archive].Nodes)
					{
						if (Cache.SubArchives[sub_archive].FileNames[node.FileIndex] == name)
						{
							n = node;
							break;
						}
					}
				}
			}

			if (WriteFile(filename, archive, sub_archive, n))
			{
				if (sub_archive != -1)
				{
					if (!found)
					{
						file_browser.Nodes[archive].Nodes[sub_archive].Nodes.Add(n);
						string f = file_browser.Nodes[archive].Nodes[sub_archive].Text.Substring(0, file_browser.Nodes[archive].Nodes[sub_archive].Text.IndexOf('('));
						file_browser.Nodes[archive].Nodes[sub_archive].Text = f + "(" + Cache.SubArchives[sub_archive].FileCount + " files)";
					}
					else
					{
						string f = n.Text.Substring(0, n.Text.LastIndexOf('('));
						n.Text = f + "(" + GetSizeLabel(n.Size) + ")";
					}
				}
				else
				{
					file_browser.Nodes[archive].Nodes.Add(n);
					string f = file_browser.Nodes[archive].Text.Substring(0, file_browser.Nodes[archive].Text.IndexOf('('));
					file_browser.Nodes[archive].Text = f + "(" + Cache.Archives[archive].GetFileCount() + " files)";
				}
			}
			else
				MessageBox.Show("Failed to find plugin.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);

			if (n == selected_node)
			{
				DeterminePlugins();
				UpdateFileInfo();
			}
		}

		private bool WriteFile(string filename, int archive, int sub_archive, CacheItemNode n)
		{
			string ext = Path.GetExtension(filename).ToLower();
			foreach (IPlugin plugin in Plugins.Plugins)
			{
				if (plugin.FileExtensions != "")
				{
					string[] tes = plugin.FileExtensions.Split('|');
					foreach (string s in tes)
					{
						if (s.Replace("*", "").ToLower() == ext.ToLower())
						{
							CacheItemNode temp = plugin.Node;
							plugin.Node = n;
							plugin.Cache = Cache;
							bool b = plugin.OnImport(filename);
							if (b)
							{
								plugin.Node.Size = plugin.Data.Buffer.Length;
								plugin.Node.Text = plugin.Node.Name + " (" + GetSizeLabel(plugin.Node.Size) + ")";
							}
							plugin.Node = temp;
							return b;
						}
					}
				}
			}

			return false;
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (files.Length > 0)
					ImportFile(files[0]);
			}
			catch (Exception)
			{
				MessageBox.Show("An error occurred when importing file. It may still be imported but one or more plugins may be broken.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void file_browser_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void file_browser_DragDrop(object sender, DragEventArgs e)
		{
			Point relative = file_browser.PointToClient(new Point(e.X, e.Y));
			CacheItemNode node = (CacheItemNode)file_browser.GetNodeAt(relative.X, relative.Y);
			file_browser_DragLeave(this, null);

			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (node.FileIndex == -1 || files.Length > 1)
			{
				for (int i = 0; i < files.Length; i++)
				{
					try
					{
						CreateAndWriteFile(files[i], node.Archive, node.SubArchive);
					}
					catch (Exception ex)
					{
						MessageBox.Show("An error occurred when importing file. It may still be imported but one or more plugins may be broken.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			else if (files.Length == 1 && node.FileIndex != -1)
			{
				try
				{
					if (!WriteFile(files[0], node.Archive, node.SubArchive, node))
						MessageBox.Show("Failed to find plugin.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
					{
						if (node == selected_node)
						{
							DeterminePlugins();
							UpdateFileInfo();
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("An error occurred when importing file. It may still be imported but one or more plugins may be broken.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void batchExportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BatchExport b = new BatchExport(Cache, Plugins);
			b.ShowDialog();
		}

		private void file_browser_DragOver(object sender, DragEventArgs e)
		{
			Point relative = file_browser.PointToClient(new Point(e.X, e.Y));
			CacheItemNode node = (CacheItemNode)file_browser.GetNodeAt(relative.X, relative.Y);
			if (node != null && node != last_highlight_node)
			{
				if (last_highlight_node != null && last_highlight_node != node)
				{
					last_highlight_node.BackColor = last_highlight_color;
				}
				last_highlight_color = node.BackColor;
				last_highlight_node = node;
				node.BackColor = SystemColors.ButtonShadow;
			}
		}

		private void file_browser_DragLeave(object sender, EventArgs e)
		{
			if (last_highlight_node != null)
			{
				last_highlight_node.BackColor = last_highlight_color;
				last_highlight_node = null;
			}
		}
	}
}
