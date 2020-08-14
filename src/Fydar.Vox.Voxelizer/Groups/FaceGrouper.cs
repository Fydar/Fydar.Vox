using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer
{
	public static class SpaceHelper
	{
		public static void RotatePosition(Vector3SByte position, VoxelNormal normal, out Vector2SByte planePosition, out int depth)
		{
			var rotated = VoxelNormal.RotateAroundOrigin(position, normal);
			planePosition = new Vector2SByte(rotated.x, rotated.y);
			depth = rotated.z;
		}
	}

	public sealed class FaceGrouper
	{
		private struct SinglePassOverallocaingBuffer : IDriverBuffer
		{
			public Dictionary<SurfaceDescription, List<GroupedSurfaceFace>> FaceGroups;

			public void AddVoxelFace(Vector3SByte voxelPosition, VoxelNormal normal, Colour24 colour, FaceEdgeFlags edges)
			{
				SpaceHelper.RotatePosition(voxelPosition, normal, out var groupPosition, out int groupDepth);

				if (normal == VoxelNormal.Up ||
					normal == VoxelNormal.Forward ||
					normal == VoxelNormal.Right)
				{
					groupDepth += 1;
				}

				var faceDescription = new SurfaceDescription()
				{
					Colour = colour,
					Normal = normal,
					Depth = groupDepth
				};

				if (!FaceGroups.TryGetValue(faceDescription, out var faces))
				{
					faces = new List<GroupedSurfaceFace>();
					FaceGroups.Add(faceDescription, faces);
				}

				faces.Add(new GroupedSurfaceFace()
				{
					Position = groupPosition,
					Neighbours = edges.Corners,
				});
			}

			public GroupedMesh Build()
			{
				var groups = new GroupedSurface[FaceGroups.Count];

				int index = 0;
				foreach (var faceGroup in FaceGroups)
				{
					groups[index] = new GroupedSurface()
					{
						Description = faceGroup.Key,
						Faces = faceGroup.Value.ToArray()
					};

					index++;
				}

				return new GroupedMesh()
				{
					Surfaces = groups
				};
			}
		}


		public DataVoxelizerDriver Driver { get; }

		public FaceGrouper(DataVoxelizerDriver driver)
		{
			Driver = driver;
		}

		public GroupedMesh Voxelize()
		{
			var buffer = new SinglePassOverallocaingBuffer
			{
				FaceGroups = new Dictionary<SurfaceDescription, List<GroupedSurfaceFace>>()
			};

			Driver.Process(ref buffer);

			return buffer.Build();
		}
	}
}
