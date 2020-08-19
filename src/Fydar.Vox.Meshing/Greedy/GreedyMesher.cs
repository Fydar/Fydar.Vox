using System.Collections.Generic;

namespace Fydar.Vox.Meshing.Greedy
{
	public class GreedyMesher
	{
		public GreedyMesh Optimize(GroupedMesh mesh)
		{
			var outputSurfaces = new GreedySurface[mesh.Surfaces.Length];
			var sortedBuffer = new List<GroupedSurfaceFace>();
			var outputFacesBuffer = new List<GreedySurfaceFace>();

			for (int surfaceIndex = 0; surfaceIndex < mesh.Surfaces.Length; surfaceIndex++)
			{
				var surface = mesh.Surfaces[surfaceIndex];
				sortedBuffer.Clear();
				outputFacesBuffer.Clear();

				sortedBuffer.AddRange(surface.Faces);

				sortedBuffer.Sort((lhs, rhs) =>
				{
					return lhs.Position.y != rhs.Position.y
					  ? lhs.Position.y.CompareTo(rhs.Position.y)
					  : lhs.Position.x.CompareTo(rhs.Position.x);
				});

				void AddFaceAndMergeVertically(GreedySurfaceFace faceToAdd)
				{
					bool didChainVertically = false;
					// Iterate over every previously chained face
					for (int j = outputFacesBuffer.Count - 1; j >= 0; j--)
					{
						var previousChainedFace = outputFacesBuffer[j];

						if (previousChainedFace.ConnectsWithY(faceToAdd))
						{
							previousChainedFace = new GreedySurfaceFace()
							{
								Position = previousChainedFace.Position,
								Scale = previousChainedFace.Scale + new Vector2SByte(0, faceToAdd.Scale.y)
							};

							didChainVertically = true;
							outputFacesBuffer[j] = previousChainedFace;
							break;
						}

						// If the previous face is further away than the last row that we
						// previously procesed, stop searching.
						if (faceToAdd.Position.y == previousChainedFace.Position.y - 2)
						{
							break;
						}
					}
					if (!didChainVertically)
					{
						outputFacesBuffer.Add(faceToAdd);
					}
				}

				GreedySurfaceFace? chainLastNull = null;
				for (int i = 0; i < sortedBuffer.Count; i++)
				{
					var currentFace = sortedBuffer[i];
					if (chainLastNull == null)
					{
						chainLastNull = new GreedySurfaceFace()
						{
							Position = currentFace.Position,
							Scale = new Vector2SByte(1, 1)
						};
					}
					else
					{
						var chainedFace = chainLastNull.Value;

						if (chainedFace.ConnectsWithX(currentFace))
						{
							chainLastNull = new GreedySurfaceFace()
							{
								Position = chainedFace.Position,
								Scale = chainedFace.Scale + new Vector2SByte(1, 0)
							};
						}
						else
						{
							AddFaceAndMergeVertically(chainedFace);

							chainLastNull = new GreedySurfaceFace()
							{
								Position = currentFace.Position,
								Scale = new Vector2SByte(1, 1)
							};
						}
					}
				}
				if (chainLastNull != null)
				{
					AddFaceAndMergeVertically(chainLastNull.Value);
				}
				outputSurfaces[surfaceIndex] = new GreedySurface()
				{
					Description = surface.Description,
					Faces = outputFacesBuffer.ToArray()
				};
			}

			return new GreedyMesh()
			{
				Surfaces = outputSurfaces
			};
		}
	}
}
