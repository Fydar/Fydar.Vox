using System;
using System.Text;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxStructureString : IVoxDocumentStructure
	{
		private VoxDocument document;
		private int startIndex;

		VoxDocument IVoxDocumentStructure.Document { get => document; set => document = value; }
		int IVoxDocumentStructure.StartIndex { get => startIndex; set => startIndex = value; }
		int IVoxDocumentStructure.Length
		{
			get
			{
				return StringLength + 4;
			}
		}

		public int StringLength
		{
			get
			{
				return BitConverter.ToInt32(document.Content, startIndex);
			}
		}

		public override string ToString()
		{
			return Encoding.ASCII.GetString(document.Content, startIndex + 4, StringLength);
		}
	}
}
