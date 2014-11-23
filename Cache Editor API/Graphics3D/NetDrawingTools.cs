using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API.Graphics3D
{
	public class NetDrawingTools
	{
		public static T[][] Create2DArray<T>(int x, int y)
		{
			T[][] array = new T[x][];
			for (int i = 0; i < x; i++)
				array[i] = new T[y];

			return array;
		}

		public static T[][][] Create3DArray<T>(int x, int y, int z)
		{
			T[][][] array = new T[x][][];
			for (int i = 0; i < x; i++)
			{
				array[i] = new T[y][];
				for (int k = 0; k < y; k++)
					array[i][k] = new T[z];
			}

			return array;
		}

		public static T[][][][] Create4DArray<T>(int x, int y, int z, int w)
		{
			T[][][][] array = new T[x][][][];
			for (int i = 0; i < x; i++)
			{
				array[i] = new T[y][][];
				for (int k = 0; k < y; k++)
				{
					array[i][k] = new T[z][];
					for (int j = 0; j < z; j++)
						array[i][k][z] = new T[w];
				}
			}

			return array;
		}
	}
}
