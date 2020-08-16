using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;

namespace Fydar.Voxelizer.Demo
{
	internal class FirstDemo
	{
		public static void Run()
		{
			var topLeft = new FaceCornerFlags() { TopLeft = true };
			var TopRight = new FaceCornerFlags() { TopRight = true };
			var bottomLeft = new FaceCornerFlags() { BottomLeft = true };
			var bottomRight = new FaceCornerFlags() { BottomRight = true };

			var opp = topLeft | bottomRight;
			var tab = !opp;



			var voxelizer = new FaceGrouper(new DonutDataVoxelizer());
			var grouped = voxelizer.Voxelize();

			var greedyMesher = new GreedyMesher();
			var greedy = greedyMesher.Optimize(grouped);
		}

		private class DonutDataVoxelizer : DataVoxelizerDriver
		{
			public override void Process<T>(ref T buffer)
			{
				buffer.AddVoxelFace(
					new Vector3SByte(-1, -1, 1), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(0, -1, 1), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(1, -1, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());


				buffer.AddVoxelFace(
					new Vector3SByte(-1, 0, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(1, 0, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());


				buffer.AddVoxelFace(
					new Vector3SByte(-1, 0, 0), VoxelNormal.Right,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(1, 0, 0), VoxelNormal.Left,
					Colour24.White, new FaceEdgeFlags());


				buffer.AddVoxelFace(
					new Vector3SByte(-1, 1, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(0, 1, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());

				buffer.AddVoxelFace(
					new Vector3SByte(1, 1, 0), VoxelNormal.Forward,
					Colour24.White, new FaceEdgeFlags());
			}
		}
	}
}
