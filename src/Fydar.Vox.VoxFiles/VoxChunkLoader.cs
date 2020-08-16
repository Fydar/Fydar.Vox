namespace Fydar.Vox.VoxFiles
{
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

		public bool CanImport(string tag)
		{
			return tag == Name;
		}
	}
}
