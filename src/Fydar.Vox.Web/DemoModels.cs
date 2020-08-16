using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fydar.Vox.Web
{
	public struct DemoModel
	{
		public GroupedMesh Grouped;
		public GreedyMesh Greedy;

		public DemoModel(DataVoxelizerDriver voxelDriver)
		{
			var voxelizer = new FaceGrouper(voxelDriver);
			Grouped = voxelizer.Voxelize();
			Greedy = new GreedyMesher().Optimize(Grouped);
		}
	}

	public static class DemoModels
	{
		public static DemoModel CubeA;
		public static DemoModel CubeB;
		public static DemoModel CubeC;
		public static DemoModel CubeD;

		public static DemoModel Donut;
		public static DemoModel ShapeA;

		public static async Task Init(HttpClient client)
		{
			CubeA = new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 0, 0)));
			CubeB = new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 2, 0)));
			CubeC = new DemoModel(new CubeVoxelDriver(new Vector3SByte(2, 0, 0)));
			CubeD = new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 0, 2)));

			Donut = new DemoModel(new DonutDataVoxelizer());

			var response = await client.GetAsync("sample-data/demo-scene.vox");
			var document = new VoxDocument(await response.Content.ReadAsByteArrayAsync());

			ShapeA = new DemoModel(new ImporterVoxeliser(document));
		}

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

		private class ImporterVoxeliser : DataVoxelizerDriver
		{
			private readonly VoxDocument document;

			public ImporterVoxeliser(VoxDocument document)
			{
				this.document = document;
			}

			public override void Process<T>(ref T buffer)
			{
				var scene = new VoxelScene(document);

				var model = scene.Models[1];

				for (int x = 0; x < model.Width; x++)
				{
					for (int y = 0; y < model.Height; y++)
					{
						for (int z = 0; z < model.Depth; z++)
						{
							var voxel = model.Voxels[x, z, y];
							var pos = new Vector3SByte(x, y, z);

							if (voxel != null)
							{
								var colorSource = model.VoxelColourPallette.Colours[voxel.Value.Index];
								var color = new Colour24(colorSource.R, colorSource.G, colorSource.B);

								var forwardPos = pos + Vector3SByte.Forward;
								var forward = model.TryGetVoxel(forwardPos.x, forwardPos.z, forwardPos.y);
								if (forward == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Forward, color, FaceEdgeFlags.None);
								}

								var backPos = pos + Vector3SByte.Back;
								var back = model.TryGetVoxel(backPos.x, backPos.z, backPos.y);
								if (back == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Back, Colour24.Lerp(color, Colour24.Black, 0.5f), FaceEdgeFlags.None);
								}

								var leftPos = pos + Vector3SByte.Left;
								var left = model.TryGetVoxel(leftPos.x, leftPos.z, leftPos.y);
								if (left == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Left, Colour24.Lerp(color, Colour24.Black, 0.2f), FaceEdgeFlags.None);
								}

								var rightPos = pos + Vector3SByte.Right;
								var right = model.TryGetVoxel(rightPos.x, rightPos.z, rightPos.y);
								if (right == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Right, Colour24.Lerp(color, Colour24.Black, 0.3f), FaceEdgeFlags.None);
								}

								var upPos = pos + Vector3SByte.Up;
								var up = model.TryGetVoxel(upPos.x, upPos.z, upPos.y);
								if (up == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Up, Colour24.Lerp(color, Colour24.Black, 0.1f), FaceEdgeFlags.None);
								}

								var downPos = pos + Vector3SByte.Down;
								var down = model.TryGetVoxel(downPos.x, downPos.z, downPos.y);
								if (down == null)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Down, Colour24.Lerp(color, Colour24.Black, 0.4f), FaceEdgeFlags.None);
								}
							}
						}
					}
				}
			}
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
