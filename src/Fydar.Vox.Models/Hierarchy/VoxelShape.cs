using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles.Hierarchy
{
	public class VoxelShape : VoxelNode
	{
		public List<VoxelModel> Models { get; }

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

		public VoxelShape(int nodeId)
		{
			NodeId = nodeId;
			NodeAttributes = new Dictionary<string, string>();

			Models = new List<VoxelModel>();
		}

		public VoxelShape(VoxDocument document, int startIndex)
		{
			Models = new List<VoxelModel>();

			int offset = startIndex;
			NodeId = document.ReadInt32(ref offset);
			var voxDictionary = document.ReadStructure<VoxStructureDictionary>(ref offset);

			NodeAttributes = new Dictionary<string, string>();
			foreach (var kvp in voxDictionary.KeyValuePairs)
			{
				NodeAttributes.Add(kvp.Key, kvp.Value);
			}
		}

		public override string ToString()
		{
			return $"S-{NodeId:00}: {Name}";
		}
	}
}
