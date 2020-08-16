namespace Fydar.Vox.VoxFiles
{
	public static class VoxChunks
	{
		public static VoxChunkLoader<VoxChunkRGBA> RGBA = new VoxChunkLoader<VoxChunkRGBA>("RGBA");
		public static VoxChunkLoader<VoxChunkSIZE> SIZE = new VoxChunkLoader<VoxChunkSIZE>("SIZE");
		public static VoxChunkLoader<VoxChunkXYZI> XYZI = new VoxChunkLoader<VoxChunkXYZI>("XYZI");
		public static VoxChunkLoader<VoxChunkLAYR> LAYR = new VoxChunkLoader<VoxChunkLAYR>("LAYR");

		public static VoxChunkLoader<VoxChunknTRN> nTRN = new VoxChunkLoader<VoxChunknTRN>("nTRN");
		public static VoxChunkLoader<VoxChunknGRP> nGRP = new VoxChunkLoader<VoxChunknGRP>("nGRP");
		public static VoxChunkLoader<VoxChunknSHP> nSHP = new VoxChunkLoader<VoxChunknSHP>("nSHP");
	}
}
