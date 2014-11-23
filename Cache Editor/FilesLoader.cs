using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Cache_Editor
{
	public class FilesLoader
	{
		public static Dictionary<int, string> Files { get; set; }

		public static bool Load()
		{
			Files = new Dictionary<int, string>();
			try
			{
				StreamReader sr = new StreamReader(File.OpenRead("files.txt"));
				string[] lines = sr.ReadToEnd().Replace("\r", "").Split('\n');
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Length == 0)
						continue;
					string file = lines[i].ToUpper();
					int file_int = 0;
					for (int j = 0; j < file.Length; j++)
						file_int = (file_int * 61 + file[j]) - 32;
					if (!Files.ContainsKey(file_int))
						Files.Add(file_int, lines[i]);
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
