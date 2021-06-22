namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// A collection of <see cref="VoxChunkLoader{TChunk}"/> used to load voxel data.
	/// </summary>
	public static class VoxChunks
	{
		/// <summary>
		/// Represents chunks of data for .vox file colour palletes.
		/// </summary>
		public static VoxChunkLoader<VoxChunkRGBA> RGBA { get; } = new VoxChunkLoader<VoxChunkRGBA>("RGBA");

		/// <summary>
		/// Represents chunks of data for .vox file model size.
		/// </summary>
		public static VoxChunkLoader<VoxChunkSIZE> SIZE { get; } = new VoxChunkLoader<VoxChunkSIZE>("SIZE");

		/// <summary>
		/// Represents chunks of data for .vox file voxel data.
		/// </summary>
		public static VoxChunkLoader<VoxChunkXYZI> XYZI { get; } = new VoxChunkLoader<VoxChunkXYZI>("XYZI");

		/// <summary>
		/// Represents chunks of data for .vox file layer configuration.
		/// </summary>
		public static VoxChunkLoader<VoxChunkLAYR> LAYR { get; } = new VoxChunkLoader<VoxChunkLAYR>("LAYR");

		/// <summary>
		/// Represents chunks of data for .vox file transform nodes.
		/// </summary>
		public static VoxChunkLoader<VoxChunknTRN> NTRN { get; } = new VoxChunkLoader<VoxChunknTRN>("nTRN");

		/// <summary>
		/// Represents chunks of data for .vox file group nodes.
		/// </summary>
		public static VoxChunkLoader<VoxChunknGRP> NGRP { get; } = new VoxChunkLoader<VoxChunknGRP>("nGRP");

		/// <summary>
		/// Represents chunks of data for .vox file shape nodes.
		/// </summary>
		public static VoxChunkLoader<VoxChunknSHP> NSHP { get; } = new VoxChunkLoader<VoxChunknSHP>("nSHP");
	}
}
