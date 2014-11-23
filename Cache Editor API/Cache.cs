using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cache_Editor_API
{
	public class Cache
	{
		public FileStream DataFile { get; set; }
		public Archive[] Archives { get; set; }
		public List<SubArchive> SubArchives { get; set; }

		public static string[] ArchiveNames = { "Sub-Archives", "Models", "Animations", "Midis", "Maps" };
		public static string[] SubNames = { "Sub-archive 0", "Title", "Config", "Interface", "Media", "Versions", "Textures", "Chat", "Sounds" };

		public Cache()
		{
			Archives = new Archive[5];
			SubArchives = new List<SubArchive>();
		}

		public void LoadCache(string folder)
		{
			DataFile = new FileStream(folder + "main_file_cache.dat", FileMode.Open);

			for (int i = 0; i < 5; i++)
			{
				Archives[i] = new Archive(ArchiveNames[i], DataFile, new FileStream(folder + "main_file_cache.idx" + i, FileMode.Open));
				Archives[i].ArchiveIndex = i;
			}
		}

		public void WriteFile(CacheItemNode node, byte[] data)
		{
			if(node.SubArchive != -1)
			{
				SubArchives[node.SubArchive].WriteFile(node.FileIndex, data);
			}
			else if(node.Archive != -1)
			{
				Archives[node.Archive].WriteFile(node.FileIndex, data, data.Length);
			}
		}

		public void Close()
		{
			try
			{
				DataFile.Close();
			}
			catch (Exception) { }

			if (Archives == null)
				return;

			for (int i = 0; i < Archives.Length; i++)
			{
				try
				{
					Archives[i].IndexFile.Close();
				}
				catch (Exception) { }
			}

			Archives = null;
			SubArchives = null;
		}
	}
}
