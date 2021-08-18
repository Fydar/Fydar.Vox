using Fydar.Vox.VoxFiles;
using System.IO;
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
			float scale = 1.0f;

			var fileInfo = new FileInfo(outputPath);

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
			output.WriteLine($"  }}");

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


			output.WriteLine($"<div style=\"height: 100vh\"></div>");


			output.WriteLine($"<div class=\"scene scene-screen-perspective fsa-relative\" style = \"padding: 50px 50px 50px 50px;\">");


			output.WriteLine($"<style>");
			output.WriteLine($"  .mesh.mesh-{name} .surface {{");
			output.WriteLine($"    background-image: url({spriteAtlasImage});");
			output.WriteLine($"    background-size: {layeredModel.SpriteAtlasResolution.Width * 16}px {layeredModel.SpriteAtlasResolution.Height * 16}px;");
			output.WriteLine($"  }}");
			output.WriteLine($"</style>");

			output.WriteLine($"");

			// output.Write($"<div class=\"idle-anim\">");

			output.Write($"<div class=\"mesh mesh-{name} anim-rotate-left\" style=\"");
			output.Write($"width: {model.Width * 16}px;");
			output.Write($"height: {model.Height * 16}px;");
			output.WriteLine($"transform: rotateX({rotation.Y}deg) rotateY({rotation.X}deg) scale3d({scale}, {scale}, {scale})\">");

			output.WriteLine($"");

			foreach (var sprite in layeredModel.Sprites)
			{
				output.Write($"    <div class=\"surface\" style=\"");
				output.Write($"background-position: -{sprite.UV.X * 16}px -{sprite.UV.Y * 16}px;");
				output.Write($"width: {sprite.Position.Width * 16}px;");
				output.Write($"height: {sprite.Position.Height * 16}px;");
				output.WriteLine($"transform: {sprite.ToTransform()}\"></div>");
			}
			output.WriteLine($"</div>");

			// output.WriteLine($"</div>");


			output.WriteLine($"</div>");


			output.WriteLine($"</div>");


			output.WriteLine($"<div style=\"height: 100vh\"></div>");


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
	}
}
