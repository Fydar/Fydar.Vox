namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// A mechanism for loading chunks.
	/// </summary>
	/// <typeparam name="TChunk">A structure representing the chunk contents.</typeparam>
	public struct VoxChunkLoader<TChunk>
		where TChunk : struct, IVoxChunk
	{
		public string Name { get; }

		public VoxChunkLoader(string name)
		{
			Name = name;
		}

		public TChunk Read(VoxDocument document, ref int offset)
		{
			var chunk = new TChunk();
			chunk.Initialise(document, ref offset);
			return chunk;
		}

		public bool TryRead(VoxStructureChunk chunk, out TChunk model)
		{
			if (!CanImport(chunk.NameToString()))
			{
				model = default;
				return false;
			}

			int offset = chunk.ContentStartIndex;
			model = new TChunk();
			model.Initialise(chunk.Document, ref offset);

			return true;
		}

		public bool CanImport(string tag)
		{
			return tag == Name;
		}
	}
}
