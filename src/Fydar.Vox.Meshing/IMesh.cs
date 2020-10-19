using System.Collections.Generic;

namespace Fydar.Vox.Meshing
{
	public interface IMesh
	{
		IEnumerable<ISurface> Surfaces { get; }
	}
}
