using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;
using SixLabors.ImageSharp;

namespace Fydar.Vox.Export.ToHtml
{
	public struct LayeredModelLayer
	{
		public VoxelModel Model { get; set; }
		public VoxelNormal Normal { get; set; }
		public int Depth { get; set; }
		public Rectangle Position { get; set; }
		public Rectangle UV { get; set; }
		public LayeredModelFace[] Faces { get; set; }

		public string LayerStyle
		{
			get
			{
				var normal = Normal;
				if (normal == VoxelNormal.Up)
				{
					return "up";
				}
				else if (normal == VoxelNormal.Down)
				{
					return "down";
				}
				else if (normal == VoxelNormal.Left)
				{
					return "left";
				}
				else if (normal == VoxelNormal.Right)
				{
					return "right";
				}
				else if (normal == VoxelNormal.Back)
				{
					return "back";
				}
				else if (normal == VoxelNormal.Forward)
				{
					return "forward";
				}
				return "";
			}
		}
	}
}
