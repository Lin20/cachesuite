using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API.Config
{
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class ChangesModel : Attribute
	{
		private bool changes;
		public bool Changes { get { return changes; } set { changes = value; } }

		public ChangesModel(bool changes_model)
		{
			changes = changes_model;
		}
	}
}
