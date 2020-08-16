using System.Collections.Generic;

namespace Fydar.Vox.Meshing
{
	public struct GroupedMesh : IMesh
	{
		public GroupedSurface[] Surfaces;

		public int CountFaces()
		{
			int total = 0;
			foreach (var surface in Surfaces)
			{
				total += surface.Faces.Length;
			}
			return total;
		}

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
