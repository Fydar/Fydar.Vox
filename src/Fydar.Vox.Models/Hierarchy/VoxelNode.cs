using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles.Hierarchy
{
	public abstract class VoxelNode
	{
		public VoxelNode? Parent { get; internal set; }

		public int NodeId { get; protected set; }
		public Dictionary<string, string> NodeAttributes { get; set; } = new Dictionary<string, string>();

		public abstract string Name { get; }
	}
}
