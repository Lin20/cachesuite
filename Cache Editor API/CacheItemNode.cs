using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cache_Editor_API
{
	public class CacheItemNode : TreeNode
	{
		public int Archive { get; set; }
		public int SubArchive { get; set; }
		public int FileIndex { get; set; }
		public int Size { get; set; }

		public CacheItemNode(string text, string name, int archive, int sub_archive, int file_index, int size)
			: base(text)
		{
			this.Name = name;
			this.Archive = archive;
			this.SubArchive = sub_archive;
			this.FileIndex = file_index;
			this.Size = size;
		}
	}
}
