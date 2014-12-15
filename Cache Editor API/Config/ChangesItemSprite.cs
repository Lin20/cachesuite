using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API.Config
{
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class ChangesItemSprite : Attribute
	{
		private bool changes;
		public bool Changes { get { return changes; } set { changes = value; } }

		public ChangesItemSprite(bool changes_sprite)
		{
			changes = changes_sprite;
		}
	}
}
