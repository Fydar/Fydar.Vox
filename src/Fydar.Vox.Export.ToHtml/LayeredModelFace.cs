using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Fydar.Vox.Export.ToHtml
{
	public struct LayeredModelFace
	{
		public Point Position { get; set; }
		public Rgba32 Color { get; set; }
	}
}
