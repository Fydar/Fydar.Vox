using Fydar.Vox.VoxFiles;
using Pastel;
using System;
using System.Drawing;

namespace Fydar.Vox.ConsoleDemo
{
	public static class VoxelConsoleRenderer
	{
		public static void RenderSmallColourPallette(VoxelColourPallette voxelColourPallette)
		{
			int countPerRow = 8;
			int vertials = 256 / countPerRow;

			for (int y = vertials - 1; y >= 0; y -= 2)
			{
				for (int x = 0; x < countPerRow; x++)
				{
					int topIndex = (y * countPerRow) + x + 1;
					int bottomIndex = ((y - 1) * countPerRow) + x + 1;

					VoxDocumentColour top;
					if (topIndex == 256)
					{
						top = new VoxDocumentColour();
					}
					else
					{
						top = voxelColourPallette.Colours[topIndex];
					}
					var bottom = voxelColourPallette.Colours[bottomIndex];

					Console.Write("\u2580".Pastel(Color.FromArgb(top.R, top.G, top.B))
						.PastelBg(Color.FromArgb(bottom.R, bottom.G, bottom.B)));
				}
				Console.Write("\n");
			}
		}

		public static void RenderLargeColourPallette(VoxelColourPallette voxelColourPallette)
		{
			int countPerRow = 8;
			int vertials = 256 / countPerRow;

			for (int y = vertials - 1; y >= 0; y--)
			{
				for (int x = 0; x < countPerRow; x++)
				{
					int index = (y * countPerRow) + x + 1;

					VoxDocumentColour colour;
					if (index == 256)
					{
						colour = new VoxDocumentColour();
					}
					else
					{
						colour = voxelColourPallette.Colours[index];
					}
					Console.Write("  ".PastelBg(Color.FromArgb(colour.R, colour.G, colour.B)));
				}
				Console.WriteLine();
			}
		}
	}
}
