using System.Collections.Generic;

namespace Fydar.Vox.Meshing
{
	public interface ISurface
	{
		SurfaceDescription Description { get; set; }
		IEnumerable<GroupedSurfaceTransformedFace> TransformedFaces { get; }
	}
}
