using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer
{
	public interface ISurface
	{
		SurfaceDescription Description { get; set; }
		IEnumerable<GroupedSurfaceTransformedFace> TransformedFaces { get; }
	}
}
