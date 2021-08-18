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

		public string ToTransform()
		{
			float faceInset = 0.0f;

			var normal = Normal;
			int depthOffset = Depth;

			float translationZ = Model.Depth * 0.5f;

			if (normal == VoxelNormal.Up)
			{
				return $"rotateX(90deg) translate3d({(Position.Left) * 16}px, {(Position.Top - translationZ) * 16}px, {(depthOffset - Model.Height - faceInset) * 16}px)";
			}
			else if (normal == VoxelNormal.Down)
			{
				return $"rotateX(-90deg) translate3d({(Position.Left) * 16}px, {(Model.Depth - Position.Bottom - translationZ) * 16}px, {(depthOffset + Model.Height - faceInset) * 16}px)";
			}
			else if (normal == VoxelNormal.Left)
			{
				return $"rotateY(-90deg) translate3d({(Position.Left - translationZ) * 16}px, {(Model.Height - Position.Bottom) * 16}px, {(depthOffset - faceInset) * 16}px)";
			}
			else if (normal == VoxelNormal.Right)
			{
				return $"rotateY(90deg) translate3d({(Model.Depth - Position.Right - translationZ) * 16}px, {(Model.Height - Position.Bottom) * 16}px, {(depthOffset - faceInset) * 16}px)";
			}
			else if (normal == VoxelNormal.Back)
			{
				return $"rotateX(180deg) translate3d({(Position.Left) * 16}px, {(Position.Top - Model.Height) * 16}px, {(depthOffset + translationZ - faceInset) * 16}px)";
			}
			else if (normal == VoxelNormal.Forward)
			{
				return $"translate3d({(Position.Left) * 16}px, {(Model.Height - Position.Bottom) * 16}px, {(depthOffset - translationZ - faceInset) * 16}px)";
			}
			return "";
		}
	}
}
