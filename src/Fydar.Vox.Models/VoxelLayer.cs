using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public class VoxelLayer
	{
		public int LayerId { get; }
		public Dictionary<string, string> LayerAttributes { get; set; }
		public int ReservedId { get; set; }

		public bool Hidden => LayerAttributes.TryGetValue("_hidden", out string name)
					? name == "1"
					: false;

		public VoxelLayer(int layerId, IEnumerable<KeyValuePair<string, string>> tags = null)
		{
			LayerId = layerId;
			LayerAttributes = new Dictionary<string, string>();
			if (tags != null)
			{
				foreach (var tag in tags)
				{
					LayerAttributes.Add(tag.Key, tag.Value);
				}
			}
		}

		public override string ToString()
		{
			return LayerAttributes.TryGetValue("_name", out string name)
				? name
				: LayerId.ToString();
		}
	}
}
