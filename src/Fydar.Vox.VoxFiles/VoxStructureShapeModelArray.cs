using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureShapeModelArray : IVoxDocumentStructure
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
				for (int i = 0; i < elements; i++)
				{
					document.ReadInt32(ref offset);
					document.ReadStructure<VoxStructureDictionary>(ref offset);
				}
				return offset - startIndex;
			}
		}

		public IEnumerable<VoxDocumentShapeModel> Elements
		{
			get
			{
				int offset = startIndex;
				int elementsCount = document.ReadInt32(ref offset);
				for (int i = 0; i < elementsCount; i++)
				{
					int modelId = document.ReadInt32(ref offset);
					var modelAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);

					yield return new VoxDocumentShapeModel()
					{
						ModelId = modelId,
						ModelAttributes = modelAttributes
					};
				}
			}
		}
	}
}
