using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Cache_Editor_API
{
	public class EditorClassifications
	{
		public int[] Archives { get; set; }
		public int[] SubArchives { get; set; }
		public string[] Filenames { get; set; }
		public string[] FileExtensions { get; set; }

		[Description("Sets the editor classifications to work with every file.")]
		public EditorClassifications()
		{
			Archives = new int[0];
			SubArchives = new int[0];
			Filenames = new string[0];
			FileExtensions = new string[0];
		}
	}
}
