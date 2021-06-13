using Fydar.Vox.Export.ToHtml.Internal.ImagePacker;
using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fydar.Vox.Export.ToHtml
{
	public class LayeredModel
	{
		private class SpriteSizeSorting : IComparer<LayeredModelLayer>
		{
			public static readonly SpriteSizeSorting Default = new();

			public int Compare(LayeredModelLayer left, LayeredModelLayer right)
			{
				int delta = -left.Position.Width.CompareTo(right.Position.Width);
				if (delta != 0)
				{
					return delta;
				}

				delta = -left.Position.Height.CompareTo(right.Position.Height);
				return delta != 0 ? delta : 0;
			}
		}

		public VoxelModel Model { get; }
		public LayeredModelLayer[] Sprites { get; }
		public Size SpriteAtlasResolution { get; }

		public int Width => Model.Width;
		public int Depth => Model.Depth;
		public int Height => Model.Height;

		public LayeredModel(VoxelModel model, LayeredModelLayer[] sprites, Size spriteAtlasResolution)
		{
			Model = model;
			Sprites = sprites;
			SpriteAtlasResolution = spriteAtlasResolution;
		}

		public static LayeredModel Create(VoxelModel model)
		{
			var voxelDriver = new ImporterVoxeliser(model);
			var voxelizer = new GroupedMesher(voxelDriver);
			var grouped = voxelizer.Voxelize();

			var sprites = new List<LayeredModelLayer>();

			foreach (var surfaceGroup in grouped.Surfaces.GroupBy(surf => new SurfaceDescription()
			{
				Normal = surf.Description.Normal,
				Depth = surf.Description.Depth
			}))
			{
				var faces = new List<LayeredModelFace>();

				foreach (var surface in surfaceGroup)
				{
					foreach (var face in surface.Faces)
					{
						faces.Add(new LayeredModelFace()
						{
							Position = new Point(face.Position.x, face.Position.y),
							Color = new Rgba32(
								surface.Description.Colour.r,
								surface.Description.Colour.g,
								surface.Description.Colour.b,
								255)
						});
					}
				}

				int maxX = 0;
				int maxY = 0;
				int minX = int.MaxValue;
				int minY = int.MaxValue;

				foreach (var face in faces)
				{
					minX = Math.Min(minX, face.Position.X);
					minY = Math.Min(minY, face.Position.Y);
					maxX = Math.Max(maxX, face.Position.X + 1);
					maxY = Math.Max(maxY, face.Position.Y + 1);
				}

				sprites.Add(new LayeredModelLayer()
				{
					Model = model,
					Depth = surfaceGroup.Key.Depth,
					Normal = surfaceGroup.Key.Normal,

					Position = Rectangle.FromLTRB(minX, minY, maxX, maxY),
					Faces = faces.ToArray(),
				});
			}

			sprites.Sort(SpriteSizeSorting.Default);

			var packer = new MarchingAnchorRectanglePacker(512, 512);

			for (int i = 0; i < sprites.Count; i++)
			{
				var sprite = sprites[i];
				if (packer.TryPack(sprite.Position.Width, sprite.Position.Height, out var placement))
				{
					sprite.UV = new Rectangle(placement.X, placement.Y, sprite.Position.Width, sprite.Position.Height);
				}
				else
				{
					throw new InvalidOperationException("Failed to pack sprite");
				}
				sprites[i] = sprite;
			}

			var outputSize = new Size(
				CeilPower2(packer.ConsumedWidth),
				CeilPower2(packer.ConsumedHeight));

			var layeredModel = new LayeredModel(model, sprites.ToArray(), outputSize);
			return layeredModel;
		}

		public string RenderImageString()
		{
			// Creates a new image with empty pixel data.
			using var image = new Image<Rgba32>(SpriteAtlasResolution.Width, SpriteAtlasResolution.Height);
			foreach (var sprite in Sprites)
			{
				if (sprite.Normal == VoxelNormal.Up
					|| sprite.Normal == VoxelNormal.Back)
				{
					foreach (var pixel in sprite.Faces)
					{
						var position = new Point(
							sprite.UV.X + (pixel.Position.X - sprite.Position.Left),
							sprite.UV.Y + (pixel.Position.Y - sprite.Position.Top));

						image[position.X, position.Y] = pixel.Color;
					}
				}
				else if (sprite.Normal == VoxelNormal.Right)
				{
					foreach (var face in sprite.Faces)
					{
						var position = new Point(
							sprite.UV.X + (sprite.UV.Width - (face.Position.X - sprite.Position.Left) - 1),
							sprite.UV.Y + (sprite.UV.Height - (face.Position.Y - sprite.Position.Top) - 1));

						image[position.X, position.Y] = face.Color;
					}
				}
				else
				{
					foreach (var face in sprite.Faces)
					{
						var position = new Point(
							sprite.UV.X + (face.Position.X - sprite.Position.Left),
							sprite.UV.Y + (sprite.Position.Height - (face.Position.Y - sprite.Position.Top) - 1));

						image[position.X, position.Y] = face.Color;
					}
				}
			}
			return image.ToBase64String(PngFormat.Instance);
		}

		private static int CeilPower2(int x)
		{
			if (x < 2)
			{
				return 1;
			}
			return (int)Math.Pow(2, (int)Math.Log(x - 1, 2) + 1);
		}
	}
}
