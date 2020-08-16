using Fydar.Vox.Voxelizer;
using NUnit.Framework;
using System.Linq;

namespace Fydar.Vox.Voxeliser.UnitTests
{
	[TestFixture]
	public class Tests
	{
		private class CubeVoxelDriver : DataVoxelizerDriver
		{
			private readonly Vector3SByte position;

			public CubeVoxelDriver(Vector3SByte position)
			{
				this.position = position;
			}

			public override void Process<T>(ref T buffer)
			{
				buffer.AddVoxelFace(position, VoxelNormal.Up, Colour24.Red, FaceEdgeFlags.None);
				buffer.AddVoxelFace(position, VoxelNormal.Down, Colour24.Blue, FaceEdgeFlags.None);
				buffer.AddVoxelFace(position, VoxelNormal.Left, Colour24.Green, FaceEdgeFlags.None);
				buffer.AddVoxelFace(position, VoxelNormal.Right, Colour24.Orange, FaceEdgeFlags.None);
				buffer.AddVoxelFace(position, VoxelNormal.Forward, Colour24.Black, FaceEdgeFlags.None);
				buffer.AddVoxelFace(position, VoxelNormal.Back, Colour24.White, FaceEdgeFlags.None);
			}
		}

		[Test]
		public void ShouldVoxeliseCube()
		{
			foreach (var offset in new Vector3SByte[] {
				new Vector3SByte(0, 0, 0),
				new Vector3SByte(0, 5, 0),
				new Vector3SByte(0, -5, 0),
				new Vector3SByte(0, 0, 5),
				new Vector3SByte(0, 0, -5),
				new Vector3SByte(5, 0, 0),
				new Vector3SByte(-5, 0, 0),
				new Vector3SByte(1, 1, 1),
				new Vector3SByte(-1, -1, -1),
			})
			{
				var cube = new FaceGrouper(new CubeVoxelDriver(offset)).Voxelize();

				foreach (var surface in cube.Surfaces)
				{
					var cubeFace = surface.TransformedFaces.First();

					if (surface.Description.Normal == VoxelNormal.Up)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(0, 1, 1)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(1, 1, 1)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(0, 1, 0)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(1, 1, 0)));
					}
					else if (surface.Description.Normal == VoxelNormal.Down)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(0, 0, 0)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(1, 0, 0)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(0, 0, 1)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(1, 0, 1)));
					}
					else if (surface.Description.Normal == VoxelNormal.Back)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(0, 1, 0)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(1, 1, 0)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(0, 0, 0)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(1, 0, 0)));
					}
					else if (surface.Description.Normal == VoxelNormal.Forward)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(1, 1, 1)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(0, 1, 1)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(1, 0, 1)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(0, 0, 1)));
					}
					else if (surface.Description.Normal == VoxelNormal.Left)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(0, 1, 1)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(0, 1, 0)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(0, 0, 1)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(0, 0, 0)));
					}
					else if (surface.Description.Normal == VoxelNormal.Right)
					{
						Assert.That(cubeFace.TopLeft, Is.EqualTo(offset + new Vector3SByte(1, 1, 0)));
						Assert.That(cubeFace.TopRight, Is.EqualTo(offset + new Vector3SByte(1, 1, 1)));
						Assert.That(cubeFace.BottomLeft, Is.EqualTo(offset + new Vector3SByte(1, 0, 0)));
						Assert.That(cubeFace.BottomRight, Is.EqualTo(offset + new Vector3SByte(1, 0, 1)));
					}
				}
			}
		}
	}
}
