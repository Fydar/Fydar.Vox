using System;
using System.Collections.Generic;
using System.Text;

namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// https://github.com/ephtracy/voxel-model/blob/master/MagicaVoxel-file-format-vox.txt
	/// </summary>
	public struct VoxStructureChunk : IVoxDocumentStructure
	{
		private VoxDocument document;
		private int startIndex;

		VoxDocument IVoxDocumentStructure.Document { get => document; set => document = value; }
		int IVoxDocumentStructure.StartIndex { get => startIndex; set => startIndex = value; }
		int IVoxDocumentStructure.Length
		{
			get
			{
				return 12 + ContentSize + ChildrenSize;
			}
		}

		public int ContentSize => BitConverter.ToInt32(document.Content, startIndex + 4);
		public int ChildrenSize => BitConverter.ToInt32(document.Content, startIndex + 8);

		public int ContentStartIndex => startIndex + 12;

		public ReadOnlySpan<byte> Content
		{
			get
			{
				return document.Content.AsSpan(startIndex + 12, ContentSize);
			}
		}

		public ReadOnlySpan<byte> NameBytes
		{
			get
			{
				return document.Content.AsSpan().Slice(startIndex, 4);
			}
		}

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
