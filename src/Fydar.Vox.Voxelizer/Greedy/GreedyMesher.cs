using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer.Greedy
{
	public class GreedyMesher
	{
		public GreedyMesh Optimize(GroupedMesh mesh)
		{
			var outputSurfaces = new List<GreedySurface>();
			var sortedBuffer = new List<GroupedSurfaceFace>();
			var outputFacesBuffer = new List<GreedySurfaceFace>();

			foreach (var surface in mesh.Surfaces)
			{
				sortedBuffer.Clear();
				outputFacesBuffer.Clear();

				sortedBuffer.AddRange(surface.Faces);

				sortedBuffer.Sort((lhs, rhs) =>
				{
					if (lhs.Position.y != rhs.Position.y)
					{
						return lhs.Position.y.CompareTo(rhs.Position.y);
					}

					return lhs.Position.x.CompareTo(rhs.Position.x);
				});

				GreedySurfaceFace? chainLast = null;
				foreach (var currentFace in sortedBuffer)
				{
					if (chainLast == null)
					{
						chainLast = new GreedySurfaceFace()
						{
							Position = currentFace.Position,
							Scale = new Vector2SByte(1, 1)
						};
					}
					else
					{
						var lastFace = chainLast.Value;

						if (lastFace.ConnectsWithX(currentFace))
						{
							chainLast = new GreedySurfaceFace()
							{
								Position = lastFace.Position,
								Scale = lastFace.Scale + new Vector2SByte(1, 0)
							};
						}
						else
						{
							outputFacesBuffer.Add(lastFace);

							chainLast = new GreedySurfaceFace()
							{
								Position = currentFace.Position,
								Scale = new Vector2SByte(1, 1)
							};
						}
					}
				}
				if (chainLast != null)
				{
					outputFacesBuffer.Add(chainLast.Value);
				}
				outputSurfaces.Add(new GreedySurface()
				{
					Description = surface.Description,
					Faces = outputFacesBuffer.ToArray()
				});
			}

			return new GreedyMesh()
			{
				Surfaces = outputSurfaces.ToArray()
			};
		}
	}
}
