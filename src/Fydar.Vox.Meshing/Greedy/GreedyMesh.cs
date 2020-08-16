using System.Collections.Generic;

namespace Fydar.Vox.Meshing
{
	public struct GreedyMesh : IMesh
	{
		public GreedySurface[] Surfaces;

		IEnumerable<ISurface> IMesh.Surfaces
		{
			get
			{
				foreach (var surface in Surfaces)
				{
					yield return surface;
				}
			}
		}
	}
}
