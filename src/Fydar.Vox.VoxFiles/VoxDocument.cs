using System;
using System.Text;

namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// https://github.com/ephtracy/voxel-model/blob/master/MagicaVoxel-file-format-vox.txt
	/// </summary>
	public class VoxDocument
	{
		public byte[] Content { get; }

		public int FileVersionNumber { get; }
		public VoxStructureChunk Main { get; }

		public VoxDocument(byte[] content)
		{
			Content = content;

			string fileTypeHeader = Encoding.ASCII.GetString(content, 0, 4);

			if (fileTypeHeader != "VOX ")
			{
				throw new InvalidOperationException("Document is not in the \"VOX\" file format.");
			}

			int offset = 4;
			FileVersionNumber = ReadInt32(ref offset);
			Main = ReadStructure<VoxStructureChunk>(ref offset);
		}

		public byte ReadByte(ref int offset)
		{
			byte value = Content[offset];
			offset += 1;
			return value;
		}

		public int ReadInt32(ref int offset)
		{
			int value = BitConverter.ToInt32(Content, offset);
			offset += 4;
			return value;
		}

		public int ReadChar(ref int offset)
		{
			int value = BitConverter.ToChar(Content, offset);
			offset += 1;
			return value;
		}

		public TStructure ReadStructure<TStructure>(ref int offset)
			where TStructure : struct, IVoxDocumentStructure
		{
			var structure = new TStructure
			{
				Document = this,
				StartIndex = offset
			};

			offset += structure.Length;
			return structure;
		}
	}
}
