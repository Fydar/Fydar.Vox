using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureColorPaletteArray : IVoxDocumentStructure
	{
		private VoxDocument document;
		private int startIndex;

		VoxDocument IVoxDocumentStructure.Document { get => document; set => document = value; }
		int IVoxDocumentStructure.StartIndex { get => startIndex; set => startIndex = value; }
		int IVoxDocumentStructure.Length
		{
			get
			{
				return 256 * 4;
			}
		}

		public int Count
		{
			get
			{
				int offset = startIndex;
				return document.ReadInt32(ref offset);
			}
		}

		public IEnumerable<VoxDocumentColour> Colours
		{
			get
			{
				int offset = startIndex;
				for (int i = 0; i < 255; i++)
				{
					yield return new VoxDocumentColour()
					{
						R = document.ReadByte(ref offset),
						G = document.ReadByte(ref offset),
						B = document.ReadByte(ref offset),
						A = document.ReadByte(ref offset),
					};
				}
			}
		}
	}
}
