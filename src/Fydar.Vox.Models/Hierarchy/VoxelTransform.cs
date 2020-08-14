using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles.Hierarchy
{
	public class VoxelTransform : VoxelNode
	{
		public VoxelNode ChildNode { get; set; }
		public VoxelLayer Layer { get; set; }

		public int ReservedId { get; set; }

		public VoxelRotation Rotation { get; set; }
		public VoxelTranslation Translation { get; set; }

		public override string Name
		{
			get
			{
				if (!NodeAttributes.TryGetValue("_name", out string name))
				{
					name = "";
				}
				return name;
			}
		}

		public VoxelTransform(int nodeId)
		{
			NodeId = nodeId;
			NodeAttributes = new Dictionary<string, string>();
		}

		public override string ToString()
		{
			return $"T-{NodeId:00}: {Name}";
		}
	}
}
