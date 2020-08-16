using System.Collections.Generic;

namespace Fydar.Vox.Meshing.Greedy
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
							bool didChainVertically = false;
							// Iterate over every previously chained face
							for (int j = outputFacesBuffer.Count - 1; j >= 0; j--)
							{
								var previousChainedFace = outputFacesBuffer[j];

								if (previousChainedFace.ConnectsWithY(chainedFace))
								{
									previousChainedFace = new GreedySurfaceFace()
									{
										Position = previousChainedFace.Position,
										Scale = previousChainedFace.Scale + new Vector2SByte(0, chainedFace.Scale.y)
									};

									didChainVertically = true;
									outputFacesBuffer[j] = previousChainedFace;
									break;
								}

								// If the previous face is further away than the last row that we
								// previously procesed, stop searching.
								if (chainedFace.Position.y == previousChainedFace.Position.y - 2)
								{
									break;
								}
							}

							if (!didChainVertically)
							{
								outputFacesBuffer.Add(chainedFace);
							}

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
					outputFacesBuffer.Add(chainLastNull.Value);
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
