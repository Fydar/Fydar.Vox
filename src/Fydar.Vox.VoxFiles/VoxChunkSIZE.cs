using System.Runtime.InteropServices;

namespace Fydar.Vox.VoxFiles
{
	public struct VoxChunkLoader<TChunk>
		where TChunk : IVoxChunk, new()
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

	public interface IVoxChunk
	{
		void Initialise(VoxDocument document, ref int offset);
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct VoxChunkSIZE : IVoxChunk
	{
		[FieldOffset(0)] public int X;
		[FieldOffset(4)] public int Y;
		[FieldOffset(8)] public int Z;

		public void Initialise(VoxDocument document, ref int offset)
		{
			X = document.ReadInt32(ref offset);
			Y = document.ReadInt32(ref offset);
			Z = document.ReadInt32(ref offset);

			// MemoryMarshal.Read()
		}
	}

	public static class VoxChunks
	{
		public static VoxChunkLoader<VoxChunkSIZE> SIZE = new VoxChunkLoader<VoxChunkSIZE>("SIZE");
		public static VoxChunkLoader<VoxChunkSIZE> XYZI = new VoxChunkLoader<VoxChunkSIZE>("XYZI");
	}

	public static class Resolver
	{
		public static void Run()
		{
			var tag = "SIZE";

			if (VoxChunks.SIZE.CanImport(tag))
			{

			}
		}
	}
}
