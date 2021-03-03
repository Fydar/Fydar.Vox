using SixLabors.ImageSharp;

namespace Fydar.Vox.WebDemo.Services.ImagePacker
{
	public interface IRectanglePacker
	{
		int ConsumedWidth { get; }
		int ConsumedHeight { get; }

		bool TryPack(int width, int height, out Point placement);
	}
}
