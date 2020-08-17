using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;
using System.Collections.Generic;
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
		public static List<DemoModel> Models = new List<DemoModel>();

		public static async Task Init(HttpClient client)
		{
			Models.Add(new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 0, 0))));
			Models.Add(new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 2, 0))));
			Models.Add(new DemoModel(new CubeVoxelDriver(new Vector3SByte(2, 0, 0))));
			Models.Add(new DemoModel(new CubeVoxelDriver(new Vector3SByte(0, 0, 2))));

			Models.Add(new DemoModel(new DonutDataVoxelizer()));

			var response = await client.GetAsync("sample-data/demo-scene.vox");
			var document = new VoxDocument(await response.Content.ReadAsByteArrayAsync());
			var scene = new VoxelScene(document);
			foreach (var model in scene.Models)
			{
				Models.Add(new DemoModel(new ImporterVoxeliser(model)));
			}
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
							var voxel = model[x, z, y];
							var pos = new Vector3SByte(x, y, z);

							if (!voxel.IsEmpty)
							{
								var colorSource = model.VoxelColourPallette.Colours[voxel.Index];
								var color = new Colour24(colorSource.R, colorSource.G, colorSource.B);

								var forward = model[pos.x, pos.z + 1, pos.y];
								if (forward.IsEmpty)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Forward, color, FaceEdgeFlags.None);
								}

								var back = model[pos.x, pos.z - 1, pos.y];
								if (back.IsEmpty)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Back, Colour24.Lerp(color, Colour24.Black, 0.5f), FaceEdgeFlags.None);
								}

								var left = model[pos.x - 1, pos.z, pos.y];
								if (left.IsEmpty)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Left, Colour24.Lerp(color, Colour24.Black, 0.2f), FaceEdgeFlags.None);
								}

								var right = model[pos.x + 1, pos.z, pos.y];
								if (right.IsEmpty)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Right, Colour24.Lerp(color, Colour24.Black, 0.3f), FaceEdgeFlags.None);
								}

								var up = model[pos.x, pos.z, pos.y + 1];
								if (up.IsEmpty)
								{
									buffer.AddVoxelFace(pos, VoxelNormal.Up, Colour24.Lerp(color, Colour24.Black, 0.1f), FaceEdgeFlags.None);
								}

								var down = model[pos.x, pos.z, pos.y - 1];
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
