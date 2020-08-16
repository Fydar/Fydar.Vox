using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer
{
	public interface IMesh
	{
		IEnumerable<ISurface> Surfaces { get; }
	}
}
