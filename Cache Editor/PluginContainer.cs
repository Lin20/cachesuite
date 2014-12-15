using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache_Editor_API;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace Cache_Editor
{
	public class PluginContainer
	{
		public List<IPlugin> Plugins { get; set; }

		public void LoadPlugins()
		{
			Plugins = new List<IPlugin>();

			if (!Directory.Exists("./Plugins"))
				Directory.CreateDirectory("./Plugins");
			else
			{
				DirectoryInfo di = new DirectoryInfo("./Plugins");
				foreach (FileInfo s in di.GetFiles("*.dll"))
				{
					try
					{
						LoadPlugin(s);
					}
					catch (Exception e)
					{
						MessageBox.Show("Failed to load plugin " + s + "\nThe cause of this is most likely Windows security. If this is a valid plugin, right click on it -> click properties -> click \"Unblock\" at the bottom.\n\n" + e.Message + "\n\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void LoadPlugin(string s)
		{
			LoadPlugin(new FileInfo(s));
		}

		private void LoadPlugin(FileInfo s)
		{
			Assembly assembly = Assembly.LoadFrom(s.FullName);
			if (assembly != null)
			{
				Type pt = typeof(IPlugin);
				foreach (Type t in assembly.GetTypes())
				{
					if (t.GetInterface(pt.FullName) != null)
					{
						IPlugin p = (IPlugin)Activator.CreateInstance(t);
						Plugins.Add(p);
					}
				}
			}
		}
	}
}
