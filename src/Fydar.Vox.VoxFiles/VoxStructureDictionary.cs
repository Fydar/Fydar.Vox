using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureDictionary : IVoxDocumentStructure
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
				int numberOfKeyValuePairs = document.ReadInt32(ref offset);
				for (int i = 0; i < numberOfKeyValuePairs; i++)
				{
					document.ReadStructure<VoxStructureString>(ref offset);
					document.ReadStructure<VoxStructureString>(ref offset);
				}
				return offset - startIndex;
			}
		}

		public IEnumerable<KeyValuePair<string, string>> KeyValuePairs
		{
			get
			{
				int offset = startIndex;
				int numberOfKeyValuePairs = document.ReadInt32(ref offset);
				for (int i = 0; i < numberOfKeyValuePairs; i++)
				{
					var key = document.ReadStructure<VoxStructureString>(ref offset);
					var value = document.ReadStructure<VoxStructureString>(ref offset);

					yield return new KeyValuePair<string, string>(key.ToString(), value.ToString());
				}
			}
		}
	}
}
