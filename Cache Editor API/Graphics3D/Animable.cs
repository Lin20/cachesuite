using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API.Graphics3D
{
	public class Animable
	{
		public virtual void Render(int i, int j, int k, int l, int i1, int j1, int k1, int l1, int i2)
		{
			Model model = getRotatedModel();
			if (model != null)
			{
				modelHeight = model.modelHeight;
				model.Render(i, j, k, l, i1, j1, k1, l1, i2);
			}
		}

		public virtual Model getRotatedModel()
		{
			return null;
		}

		public Animable()
		{
			modelHeight = 1000;
		}

		public VertexNormal[] aVertexNormalArray1425;
		public int modelHeight;
	}
}
