using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Cache_Editor_API
{
	public class Map
	{
		private static int[] map_indices1;
		private static int[] map_indices2;
		private static int[] map_indices3;
		private static int[] map_indices4;

		public static Point GetCoordinates(SubArchive archive, int index)
		{
			if (map_indices1 == null)
			{
				DataBuffer data = archive.ExtractFile("map_index");
				if (data == null)
					return new Point(-1, -1);
				map_indices1 = new int[data.Buffer.Length / 7];
				map_indices2 = new int[data.Buffer.Length / 7];
				map_indices3 = new int[data.Buffer.Length / 7];
				map_indices4 = new int[data.Buffer.Length / 7];

				for (int i = 0; i < data.Buffer.Length / 7; i++)
				{
					map_indices1[i] = data.ReadShort();
					map_indices2[i] = data.ReadShort();
					map_indices3[i] = data.ReadShort();
					map_indices4[i] = data.ReadByte();
				}
			}

			if (index < map_indices1.Length)
			{
				int value = map_indices1[index];
				return new Point((value >> 8) & 0xFF, value & 0xFF);
			}

			return new Point(-1, -1);
		}
	}
}
