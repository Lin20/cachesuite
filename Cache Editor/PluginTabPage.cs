using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache_Editor_API;

namespace Cache_Editor
{
	public class PluginTabPage : TabPage
	{
		public IPlugin Plugin { get; set; }

		public PluginTabPage(string text, IPlugin p) : base(text)
		{
			Plugin = p;
		}
	}
}
