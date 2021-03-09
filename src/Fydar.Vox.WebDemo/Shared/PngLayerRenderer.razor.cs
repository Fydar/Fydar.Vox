using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;
using Fydar.Vox.WebDemo.Services.ImagePacker;
using Microsoft.AspNetCore.Components;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Fydar.Vox.WebDemo.Shared
{
	public struct SurfaceSprite
	{
		public SurfaceDescription Description { get; set; }
		public Rectangle RelativePosition { get; set; }
		public Point UVPosition { get; set; }
		public SurfacePixel[] Faces { get; set; }
	}
	public struct SurfacePixel
	{
		public Point Position { get; set; }
		public Rgba32 Color { get; set; }
	}

	public partial class PngLayerRenderer : ComponentBase
	{
		private class SpriteSizeSorting : IComparer<SurfaceSprite>
		{
			public static readonly SpriteSizeSorting Default = new();

			public int Compare(SurfaceSprite left, SurfaceSprite right)
			{
				int delta = -left.RelativePosition.Width.CompareTo(right.RelativePosition.Width);
				if (delta != 0)
				{
					return delta;
				}

				delta = -left.RelativePosition.Height.CompareTo(right.RelativePosition.Height);
				if (delta != 0)
				{
					return delta;
				}
				return 0;
			}
		}

		private DemoModel model;

		[Parameter]
		public DemoModel Model
		{
			get => model;
			set
			{
				if (Model != value)
				{
					model = value;
					Regenerate();
				}
			}
		}

		[Parameter]
		public Vector3 Position { get; set; }

		[Parameter]
		public Vector2 Rotation { get; set; }

		[Parameter]
		public float Scale { get; set; }

		[Parameter]
		public int FaceSize { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await Task.Run(Regenerate);
		}

		private void Regenerate()
		{
			if (Model.Sprites != null)
			{
				return;
			}

			var sprites = new List<SurfaceSprite>();

			Model.Sprites = sprites;

			foreach (var surfaceGroup in Model.Grouped.Surfaces.GroupBy(surf => new SurfaceDescription()
			{
				Normal = surf.Description.Normal,
				Depth = surf.Description.Depth
			}))
			{
				var faces = new List<SurfacePixel>();

				foreach (var surface in surfaceGroup)
				{
					foreach (var face in surface.Faces)
					{
						faces.Add(new SurfacePixel()
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

				sprites.Add(new SurfaceSprite()
				{
					Description = surfaceGroup.Key,
					RelativePosition = Rectangle.FromLTRB(minX, minY, maxX, maxY),
					Faces = faces.ToArray(),
				});
			}

			sprites.Sort(SpriteSizeSorting.Default);

			var packer = new MarchingAnchorRectanglePacker(512, 512);

			for (int i = 0; i < sprites.Count; i++)
			{
				var sprite = sprites[i];
				if (packer.TryPack(sprite.RelativePosition.Width, sprite.RelativePosition.Height, out var placement))
				{
					sprite.UVPosition = placement;
				}
				else
				{
					throw new InvalidOperationException("Failed to pack sprite");
				}
				sprites[i] = sprite;
			}

			Model.OutputWidth = CeilPower2(packer.ConsumedWidth);
			Model.OutputHeight = CeilPower2(packer.ConsumedHeight);

			// Creates a new image with empty pixel data.
			using (var image = new Image<Rgba32>(Model.OutputWidth, Model.OutputHeight))
			{
				foreach (var sprite in Model.Sprites)
				{
					if (sprite.Description.Normal == VoxelNormal.Up
						|| sprite.Description.Normal == VoxelNormal.Back)
					{
						foreach (var face in sprite.Faces)
						{
							var position = new Point(
								sprite.UVPosition.X + (face.Position.X - sprite.RelativePosition.Left),
								sprite.UVPosition.Y + (face.Position.Y - sprite.RelativePosition.Top));

							image[position.X, position.Y] = face.Color;
						}
					}
					else if (sprite.Description.Normal == VoxelNormal.Right)
					{
						foreach (var face in sprite.Faces)
						{
							var position = new Point(
								sprite.UVPosition.X + (sprite.RelativePosition.Width - (face.Position.X - sprite.RelativePosition.Left) - 1),
								sprite.UVPosition.Y + (sprite.RelativePosition.Height - (face.Position.Y - sprite.RelativePosition.Top) - 1));

							image[position.X, position.Y] = face.Color;
						}
					}
					else
					{
						foreach (var face in sprite.Faces)
						{
							var position = new Point(
								sprite.UVPosition.X + (face.Position.X - sprite.RelativePosition.Left),
								sprite.UVPosition.Y + (sprite.RelativePosition.Height - (face.Position.Y - sprite.RelativePosition.Top) - 1));

							image[position.X, position.Y] = face.Color;
						}
					}
				}

				Model.ImageSource = image.ToBase64String(PngFormat.Instance);
			}
		}

		private static int CeilPower2(int x)
		{
			if (x < 2)
			{
				return 1;
			}
			return (int)Math.Pow(2, (int)Math.Log(x - 1, 2) + 1);
		}

		private string ToTransform(VoxelModel model, SurfaceSprite sprite)
		{
			var normal = sprite.Description.Normal;
			int depth = sprite.Description.Depth;
			int depthOffset = depth * FaceSize;

			float translationZ = model.Depth * 0.5f;

			if (normal == VoxelNormal.Up)
			{
				return $"rotateX(90deg) translate3d({sprite.RelativePosition.Left}px, {sprite.RelativePosition.Top - translationZ}px, {depthOffset - model.Height}px)";
			}
			else if (normal == VoxelNormal.Down)
			{
				return $"rotateX(-90deg) translate3d({sprite.RelativePosition.Left}px, {model.Depth - sprite.RelativePosition.Bottom - translationZ}px, {depthOffset + model.Height}px)";
			}
			else if (normal == VoxelNormal.Left)
			{
				return $"rotateY(-90deg) translate3d({sprite.RelativePosition.Left - translationZ}px, {model.Height - sprite.RelativePosition.Bottom}px, {depthOffset * 1.0f}px)";
			}
			else if (normal == VoxelNormal.Right)
			{
				return $"rotateY(90deg) translate3d({(model.Depth- sprite.RelativePosition.Right) - translationZ}px, {model.Height - sprite.RelativePosition.Bottom}px, {depthOffset * 1.0f}px)";
			}
			else if (normal == VoxelNormal.Back)
			{
				return $"rotateX(180deg) translate3d({sprite.RelativePosition.Left}px, {sprite.RelativePosition.Top - model.Height}px, {(depthOffset * 1.0f) + translationZ}px)";
			}
			else if (normal == VoxelNormal.Forward)
			{
				return $"translate3d({sprite.RelativePosition.Left}px, {model.Height - sprite.RelativePosition.Bottom}px, {(depthOffset * 1.0f) - translationZ}px)";
			}
			return "";
		}
	}
}
