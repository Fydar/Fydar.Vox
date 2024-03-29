﻿namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file model size.
	/// </summary>
	public struct VoxChunkSIZE : IVoxChunk
	{
		public int X;
		public int Y;
		public int Z;

		public void Initialise(VoxDocument document, ref int offset)
		{
			X = document.ReadInt32(ref offset);
			Y = document.ReadInt32(ref offset);
			Z = document.ReadInt32(ref offset);
		}
	}
}
