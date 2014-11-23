using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache_Editor_API;

namespace Cache_Editor
{
	public partial class PluginDescription : UserControl
	{
		IPlugin Plugin { get; set; }

		public PluginDescription()
		{
			InitializeComponent();
		}

		private void lblDescription_Click(object sender, EventArgs e)
		{

		}

		public void SetPlugin(IPlugin p)
		{
			Plugin = p;
			lblName.Text = p.Name;
			lblAuthor.Text = p.Author;
			lblVersion.Text = p.Version;
			lblDescription.Text = p.Description;
			lblFiles.Text = p.StaticFileExtensions;

			StringBuilder cond = new StringBuilder();
			if (p.Classifications.Archives.Length == 0)
				cond.AppendLine("Archives: Any");
			else
			{
				cond.Append("Archives: ");
				for (int i = 0; i < p.Classifications.Archives.Length - 1; i++)
					cond.Append(p.Classifications.Archives[i] + ", ");
				cond.AppendLine(p.Classifications.Archives[p.Classifications.Archives.Length - 1].ToString());
			}

			if (p.Classifications.SubArchives.Length == 0)
				cond.AppendLine("Sub-Archives: Any");
			else
			{
				cond.Append("Sub-Archives: ");
				for (int i = 0; i < p.Classifications.SubArchives.Length - 1; i++)
					cond.Append(p.Classifications.SubArchives[i] + ", ");
				cond.AppendLine(p.Classifications.SubArchives[p.Classifications.SubArchives.Length - 1].ToString());
			}

			if (p.Classifications.Filenames.Length == 0)
				cond.AppendLine("Files: Any");
			else
			{
				cond.Append("Files: ");
				for (int i = 0; i < p.Classifications.Filenames.Length - 1; i++)
				{
					cond.Append("\"");
					cond.Append(p.Classifications.Filenames[i]);
					cond.Append("\", ");
				}
				cond.Append("\"");
				cond.Append(p.Classifications.Filenames[p.Classifications.Filenames.Length - 1]);
				cond.AppendLine("\"");
			}

			if (p.Classifications.FileExtensions.Length == 0)
				cond.AppendLine("Extensions: Any");
			else
			{
				cond.Append("Extensions: ");
				for (int i = 0; i < p.Classifications.FileExtensions.Length - 1; i++)
				{
					cond.Append("\"");
					cond.Append(p.Classifications.FileExtensions[i]);
					cond.Append("\", ");
				}
				cond.Append("\"");
				cond.Append(p.Classifications.FileExtensions[p.Classifications.FileExtensions.Length - 1]);
				cond.AppendLine("\"");
			}

			lblConditions.Text = cond.ToString();
			btnConfigure.Enabled = p.ConfigureForm != null;
		}

		private void btnConfigure_Click(object sender, EventArgs e)
		{
			if (Plugin.ConfigureForm != null)
			{
				if (Plugin.ConfigureForm.ShowDialog() == DialogResult.OK)
				{
					Plugin.OnConfigure();
				}
			}
		}
	}
}
