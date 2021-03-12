using SixLabors.ImageSharp;

namespace Fydar.Vox.Export.ToHtml.Internal.ImagePacker
{
	public interface IRectanglePacker
	{
		int ConsumedWidth { get; }
		int ConsumedHeight { get; }

		bool TryPack(int width, int height, out Point placement);
	}
}
