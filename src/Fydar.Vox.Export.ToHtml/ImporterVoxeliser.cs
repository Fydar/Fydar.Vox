using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;

namespace Fydar.Vox.Export.ToHtml
{
	internal class ImporterVoxeliser : DataVoxelizerDriver
	{
		private readonly VoxelModel model;

		public ImporterVoxeliser(VoxelModel model)
		{
			this.model = model;
		}

		public override void Process<T>(ref T buffer)
		{
			for (int x = 0; x < model.Width; x++)
			{
				for (int y = 0; y < model.Height; y++)
				{
					for (int z = 0; z < model.Depth; z++)
					{
						var voxel = model.GetWithRangeCheck(x, z, y);
						var pos = new Vector3SByte(x, y, z);

						if (!voxel.IsEmpty)
						{
							var colorSource = model.VoxelColourPallette.Colours[voxel.Index];
							var color = new Colour24(colorSource.R, colorSource.G, colorSource.B);

							var forward = model.GetWithRangeCheck(pos.x, pos.z + 1, pos.y);
							if (forward.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Forward, color, FaceEdgeFlags.None);
							}

							var back = model.GetWithRangeCheck(pos.x, pos.z - 1, pos.y);
							if (back.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Back, Colour24.Lerp(color, Colour24.Black, 0.5f), FaceEdgeFlags.None);
							}

							var left = model.GetWithRangeCheck(pos.x - 1, pos.z, pos.y);
							if (left.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Left, Colour24.Lerp(color, Colour24.Black, 0.2f), FaceEdgeFlags.None);
							}

							var right = model.GetWithRangeCheck(pos.x + 1, pos.z, pos.y);
							if (right.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Right, Colour24.Lerp(color, Colour24.Black, 0.3f), FaceEdgeFlags.None);
							}

							var up = model.GetWithRangeCheck(pos.x, pos.z, pos.y + 1);
							if (up.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Up, Colour24.Lerp(color, Colour24.Black, 0.1f), FaceEdgeFlags.None);
							}

							var down = model.GetWithRangeCheck(pos.x, pos.z, pos.y - 1);
							if (down.IsEmpty)
							{
								buffer.AddVoxelFace(pos, VoxelNormal.Down, Colour24.Lerp(color, Colour24.Black, 0.4f), FaceEdgeFlags.None);
							}
						}
					}
				}
			}
		}
	}
}
