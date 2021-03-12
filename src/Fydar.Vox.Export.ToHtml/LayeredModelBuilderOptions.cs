using System.Numerics;

namespace Fydar.Vox.Export.ToHtml
{
	public class LayeredModelBuilderOptions
	{
		public Vector3 Position { get; set; }
		public Vector2 Rotation { get; set; }
		public float Scale { get; set; }
		public int FaceSize { get; set; }
	}
}
