using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache_Editor_API;

namespace Cache_Editor_API.Config
{
	public class VarBit
	{
		public static void UnpackVariables(Cache loaded_cache, DataBuffer data_file)
		{
			int cacheSize = data_file.ReadShort();
			if (cache == null)
				cache = new VarBit[cacheSize];
			Count = cacheSize;
			for (int j = 0; j < cacheSize; j++)
			{
				if (cache[j] == null)
					cache[j] = new VarBit();
				cache[j].ReadVariable(data_file);
				//if (cache[j].aBoolean651)
				//	Varp.cache[cache[j].anInt648].aBoolean713 = true;
			}
		}

		private void ReadVariable(DataBuffer stream)
		{
			do
			{
				int j = stream.ReadByte();
				if (j == 0)
					return;
				if (j == 1)
				{
					Variable = stream.ReadShort();
					LowerBit = stream.ReadByte();
					UpperBit = stream.ReadByte();
				}
				else if (j == 10)
					stream.ReadString();
				else if (j == 2)
					aBoolean651 = true;
				else if (j == 3)
					stream.ReadInteger();
				else if (j == 4)
					stream.ReadInteger();
			} while (true);
		}

		public static VarBit GetVarBit(int index)
		{
			if (index < cache.Length)
				return cache[index];
			return null;
		}

		private VarBit()
		{
			aBoolean651 = false;
		}

		public static VarBit[] cache;
		public static int Count { get; private set; }

		public int Variable { get; set; }
		public int LowerBit { get; set; }
		public int UpperBit { get; set; }
		public bool aBoolean651 { get; set; }
	}
}
