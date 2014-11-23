using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API.Graphics3D
{
	public class ModelTransform
	{
		public ModelTransform(DataBuffer stream)
		{
			int anInt341 = stream.ReadByte();
			anIntArray342 = new int[anInt341];
			anIntArrayArray343 = new int[anInt341][];
			for (int j = 0; j < anInt341; j++)
				anIntArray342[j] = stream.ReadByte();

			for (int k = 0; k < anInt341; k++)
			{
				int l = stream.ReadByte();
				anIntArrayArray343[k] = new int[l];
				for (int i1 = 0; i1 < l; i1++)
					anIntArrayArray343[k][i1] = stream.ReadByte();

			}

		}

		public int[] anIntArray342;
		public int[][] anIntArrayArray343;
	}
}
