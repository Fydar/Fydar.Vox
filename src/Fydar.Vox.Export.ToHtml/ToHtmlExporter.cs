using Fydar.Vox.Meshing;
using Fydar.Vox.VoxFiles;
using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Fydar.Vox.Export.ToHtml
{
	public static class ToHtmlExporter
	{
		public static void WriteToFile(string name, VoxelModel model, string outputPath)
		{
			var layeredModel = LayeredModel.Create(model);
			string spriteAtlasImage = layeredModel.RenderImageString();

			var rotation = new Vector2(55, -30);

			var fileInfo = new FileInfo(outputPath);

			if (!fileInfo.Directory?.Exists ?? false)
			{
				fileInfo.Directory?.Create();
			}

			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			using var outputStream = fileInfo.OpenWrite();
			using var output = new StreamWriter(outputStream);

			output.WriteLine($"<style>");
			output.WriteLine($"  .scene {{");
			output.WriteLine($"    display: inline-block;");
			output.WriteLine($"    overflow: hidden;");
			output.WriteLine($"    box-sizing: border-box;");
			output.WriteLine($"    perspective: 1024px;");
			output.WriteLine($"    width: 100%;");
			output.WriteLine($"    --light-up: 0.95;");
			output.WriteLine($"    --light-down: 0.6;");
			output.WriteLine($"    --light-forward: 1.0;");
			output.WriteLine($"    --light-back: 0.7;");
			output.WriteLine($"    --light-left: 0.8;");
			output.WriteLine($"    --light-right: 0.65;");
			output.WriteLine($"  }}");

			output.WriteLine($"");

			output.WriteLine($"    .scene .transform {{");
			output.WriteLine($"      transform-style: preserve-3d;");
			output.WriteLine($"    }}");

			output.WriteLine($"");

			output.WriteLine($"    .scene .mesh {{");
			output.WriteLine($"      position: relative;");
			output.WriteLine($"      margin: auto;");
			output.WriteLine($"      transform-style: preserve-3d;");
			output.WriteLine($"      transition: transform 0.6s;");
			output.WriteLine($"    }}");

			output.WriteLine($"");

			output.WriteLine($"      .scene .mesh .surface {{");
			output.WriteLine($"        transform-origin: 0% 0%;");
			output.WriteLine($"        position: absolute;");
			output.WriteLine($"        pointer-events: none;");
			output.WriteLine($"        touch-action: none;");
			output.WriteLine($"        image-rendering: pixelated;");
			output.WriteLine($"        user-select: none;");
			output.WriteLine($"        backface-visibility: hidden;");
			output.WriteLine($"      }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-up {{");
			output.WriteLine($"          filter: brightness(var(--light-up));");
			output.WriteLine($"        }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-down {{");
			output.WriteLine($"          filter: brightness(var(--light-down));");
			output.WriteLine($"        }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-forward {{");
			output.WriteLine($"          filter: brightness(var(--light-forward));");
			output.WriteLine($"        }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-back {{");
			output.WriteLine($"          filter: brightness(var(--light-back));");
			output.WriteLine($"        }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-left {{");
			output.WriteLine($"          filter: brightness(var(--light-left));");
			output.WriteLine($"        }}");
			output.WriteLine($"");
			output.WriteLine($"        .scene .mesh .surface.surface-right {{");
			output.WriteLine($"          filter: brightness(var(--light-right));");
			output.WriteLine($"        }}");


			output.WriteLine($"  .anim-rotate-left {{");
			output.WriteLine($"    animation: anim-rotate-left 1s linear normal;");
			output.WriteLine($"    animation-play-state: paused;");
			output.WriteLine($"    animation-delay: calc(var(--animation-time) * -1s);");
			output.WriteLine($"  }}");

			output.WriteLine($"");

			output.WriteLine($"    @keyframes anim-rotate-left {{");
			output.WriteLine($"     from {{");
			output.WriteLine($"       transform: translate3d(0px, 0px, -100px) rotateX(0deg) rotateY(50deg);");
			output.WriteLine($"     }}");
			output.WriteLine($"");
			output.WriteLine($"     to {{");
			output.WriteLine($"       transform: translate3d(0px, 0px, -100px) rotateX(0deg) rotateY(10deg);");
			output.WriteLine($"     }}");
			output.WriteLine($"    }}");



			output.WriteLine($"  .idle-anim {{");
			output.WriteLine($"    animation: idle-anim 3s ease-in-out infinite;");
			output.WriteLine($"    transform-style: preserve-3d;");
			output.WriteLine($"    transform-origin: bottom;");
			output.WriteLine($"  }}");

			output.WriteLine($"");

			output.WriteLine($"    @keyframes idle-anim {{");
			output.WriteLine($"     0% {{");
			output.WriteLine($"       transform: scale(1.0);");
			output.WriteLine($"     }}");
			output.WriteLine($"");
			output.WriteLine($"     50% {{");
			output.WriteLine($"       transform: scale(0.95);");
			output.WriteLine($"     }}");
			output.WriteLine($"");
			output.WriteLine($"     100% {{");
			output.WriteLine($"       transform: scale(1.0);");
			output.WriteLine($"     }}");
			output.WriteLine($"    }}");


			output.WriteLine($"</style>");


			output.WriteLine($"<div style=\"height: 110vh\"></div>");


			output.WriteLine($"<div class=\"scene scene-screen-perspective fsa-relative\" style=\"padding: 50px 50px 50px 50px;\">");
			output.WriteLine($"  <div class=\"transform anim-rotate-left\">");

			output.WriteLine($"");
			output.WriteLine($"");

			// output.Write($"<div class=\"idle-anim\">");

			output.WriteLine($"  <!-- Mesh '{name}' -->");
			output.Write($"  <div class=\"mesh mesh-{name}\">");

			output.WriteLine($"    <style>");
			output.WriteLine($"      .mesh.mesh-{name} {{");
			output.WriteLine($"        width: {ToPixels(model.Width * 16)};");
			output.WriteLine($"        height: {ToPixels(model.Height * 16)};");
			output.WriteLine($"        --half-depth: translateZ({ToPixels(-model.Depth * 16 / 2)});");

			output.WriteLine($"      }}");

			output.WriteLine($"        .mesh.mesh-{name} .surface {{");
			output.WriteLine($"          background-image: url({spriteAtlasImage});");
			output.WriteLine($"          background-size: {ToPixels(layeredModel.SpriteAtlasResolution.Width * 16)} {ToPixels(layeredModel.SpriteAtlasResolution.Height * 16)};");
			output.WriteLine($"        }}");
			output.WriteLine($"    </style>");

			foreach (var sprite in layeredModel.Sprites.OrderBy(s => s.LayerStyle))
			{
				output.Write($"    <div class=\"surface surface-{sprite.LayerStyle}\" style=\"");
				output.Write($"background-position: {ToPixels(sprite.UV.X * -16)} {ToPixels(sprite.UV.Y * -16)};");
				output.Write($"width: {ToPixels(sprite.Position.Width * 16)};");
				output.Write($"height: {ToPixels(sprite.Position.Height * 16)};");
				output.WriteLine($"transform: {ToTransform(sprite)}\"></div>");
			}
			output.WriteLine($"  </div>");

			output.WriteLine($"");
			output.WriteLine($"");

			output.WriteLine($"</div>");
			output.WriteLine($"</div>");

			output.WriteLine($"</div>");

			output.WriteLine($"<div style=\"height: 110vh\"></div>");


			output.WriteLine("<script>");
			output.WriteLine("    const clamp = (a, min = 0, max = 1) => Math.min(max, Math.max(min, a));");
			output.WriteLine("    const invlerp = (x, y, a) => clamp((a - x) / (y - x));");
			output.WriteLine("");
			output.WriteLine("    function UpdateRelativeElements() {");
			output.WriteLine("      var fsaRelativeObjects = document.getElementsByClassName(\"fsa-relative\");");
			output.WriteLine("");
			output.WriteLine("      for (let i = 0; i < fsaRelativeObjects.length; i++) {");
			output.WriteLine("        var fsaRelative = fsaRelativeObjects[i];");
			output.WriteLine("");
			output.WriteLine("        var bounds = fsaRelative.getBoundingClientRect();");
			output.WriteLine("        var scroll = invlerp(window.innerHeight, -bounds.height, bounds.top);");
			output.WriteLine("");
			output.WriteLine("        fsaRelative.style.setProperty(\"--animation-time\", scroll);");
			output.WriteLine("      }");
			output.WriteLine("    }");
			output.WriteLine("");
			output.WriteLine("    function UpdateScreenPerspective() {");
			output.WriteLine("      var screenPerspectives = document.getElementsByClassName(\"scene-screen-perspective\");");
			output.WriteLine("");
			output.WriteLine("      for (let i = 0; i < screenPerspectives.length; i++) {");
			output.WriteLine("        var screenPerspective = screenPerspectives[i];");
			output.WriteLine("");
			output.WriteLine("        var bounds = screenPerspective.getBoundingClientRect();");
			output.WriteLine("        var scroll = invlerp(-bounds.height, window.innerHeight, bounds.top);");
			output.WriteLine("");
			output.WriteLine("        screenPerspective.style.perspectiveOrigin = \"50% \" + (((window.innerHeight / bounds.height) * ((scroll * 2) - 1) * -100) + 50) + \"%\";");
			output.WriteLine("        screenPerspective.style.perspective = (1024 / window.devicePixelRatio) + \"px\";");
			output.WriteLine("      }");
			output.WriteLine("    }");
			output.WriteLine("");
			output.WriteLine("    window.addEventListener(\"scroll\",");
			output.WriteLine("      () => {");
			output.WriteLine("        UpdateRelativeElements();");
			output.WriteLine("        UpdateScreenPerspective();");
			output.WriteLine("      }, false");
			output.WriteLine("    );");
			output.WriteLine("    window.addEventListener(\"resize\",");
			output.WriteLine("      () => {");
			output.WriteLine("        UpdateRelativeElements();");
			output.WriteLine("        UpdateScreenPerspective();");
			output.WriteLine("      }, false");
			output.WriteLine("    );");
			output.WriteLine("    UpdateRelativeElements();");
			output.WriteLine("    UpdateScreenPerspective();");
			output.WriteLine("  </script>");
		}

		public static string ToTransform(LayeredModelLayer model)
		{
			float faceInset = 0.0f;

			var normal = model.Normal;
			int depthOffset = model.Depth;

			float translationZ = model.Model.Depth * 0.5f;
			if (normal == VoxelNormal.Up)
			{
				return $"rotateX(90deg) translate3d({ToPixels(model.Position.Left * 16)}, {ToPixels((model.Position.Top - translationZ) * 16)}, {ToPixels((depthOffset - model.Model.Height - faceInset) * 16)})";
			}
			else if (normal == VoxelNormal.Down)
			{
				return $"rotateX(-90deg) translate3d({ToPixels(model.Position.Left * 16)}, {ToPixels((model.Model.Depth - model.Position.Bottom - translationZ) * 16)}, {ToPixels((depthOffset + model.Model.Height - faceInset) * 16)})";
			}
			else if (normal == VoxelNormal.Left)
			{
				return $"rotateY(-90deg) translate3d({ToPixels((model.Position.Left - translationZ) * 16)}, {ToPixels((model.Model.Height - model.Position.Bottom) * 16)}, {ToPixels((depthOffset - faceInset) * 16)})";
			}
			else if (normal == VoxelNormal.Right)
			{
				return $"rotateY(90deg) translate3d({ToPixels((model.Model.Depth - model.Position.Right - translationZ) * 16)}, {ToPixels((model.Model.Height - model.Position.Bottom) * 16)}, {ToPixels((depthOffset - faceInset) * 16)})";
			}
			else if (normal == VoxelNormal.Back)
			{
				return $"rotateX(180deg) translate3d({ToPixels(model.Position.Left * 16)}, {ToPixels((model.Position.Top - model.Model.Height) * 16)}, {ToPixels((depthOffset + translationZ - faceInset) * 16)})";
			}
			else if (normal == VoxelNormal.Forward)
			{
				return $"translate3d({ToPixels(model.Position.Left * 16)}, {ToPixels((model.Model.Height - model.Position.Bottom) * 16)}, {ToPixels((depthOffset - translationZ - faceInset) * 16)})";
			}
			return "";
		}

		private static string ToPixels(int pixels)
		{
			return pixels == 0 ? "0" : $"{pixels}px";
		}

		private static string ToPixels(float pixels)
		{
			return Math.Abs(pixels) <= 0.01 ? "0" : $"{pixels}px";
		}

		private static string ToDegs(float degrees)
		{
			return Math.Abs(degrees) <= 0.01 ? "0" : $"{degrees:0.###}deg";
		}
	}
}
