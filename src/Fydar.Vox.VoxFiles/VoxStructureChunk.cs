using System;
using System.Collections.Generic;
using System.Text;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureChunk : IVoxDocumentStructure
	{
		private VoxDocument document;
		private int startIndex;

		VoxDocument IVoxDocumentStructure.Document { get => document; set => document = value; }
		int IVoxDocumentStructure.StartIndex { get => startIndex; set => startIndex = value; }
		int IVoxDocumentStructure.Length => 12 + ContentSize + ChildrenSize;

		public int ContentSize => BitConverter.ToInt32(document.Content, startIndex + 4);
		public int ChildrenSize => BitConverter.ToInt32(document.Content, startIndex + 8);

		public int ContentStartIndex => startIndex + 12;
		public VoxDocument Document => document;

		public IEnumerable<VoxStructureChunk> Children
		{
			get
			{
				int offset = startIndex + 12 + ContentSize;
				int maximum = offset + ChildrenSize;

				while (offset < maximum)
				{
					var newChunk = document.ReadStructure<VoxStructureChunk>(ref offset);
					yield return newChunk;
				}
			}
		}

		public string NameToString()
		{
			return Encoding.ASCII.GetString(document.Content, startIndex, 4);
		}
	}
}
