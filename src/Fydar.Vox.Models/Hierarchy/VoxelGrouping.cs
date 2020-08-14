using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles.Hierarchy
{
	public class VoxelGrouping : VoxelNode
	{
		public List<VoxelNode> Children { get; }

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

		public VoxelGrouping(int nodeId)
		{
			NodeId = nodeId;
			NodeAttributes = new Dictionary<string, string>();
			Children = new List<VoxelNode>();
		}

		public override string ToString()
		{
			return $"G-{NodeId:00}: {Name}";
		}
	}
}
