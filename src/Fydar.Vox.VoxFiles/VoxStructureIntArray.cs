using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureIntArray : IVoxDocumentStructure
	{
		private VoxDocument document;
		private int startIndex;

		VoxDocument IVoxDocumentStructure.Document { get => document; set => document = value; }
		int IVoxDocumentStructure.StartIndex { get => startIndex; set => startIndex = value; }
		int IVoxDocumentStructure.Length
		{
			get
			{
				int offset = startIndex;
				int elements = document.ReadInt32(ref offset);
				offset += elements * 4;
				return offset - startIndex;
			}
		}

		public IEnumerable<int> Values
		{
			get
			{
				int offset = startIndex;
				int elements = document.ReadInt32(ref offset);
				for (int i = 0; i < elements; i++)
				{
					yield return document.ReadInt32(ref offset);
				}
			}
		}
	}
}
