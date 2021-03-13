using Fydar.Vox.VoxFiles;
using System.IO;
using System.Numerics;

namespace Fydar.Vox.Export.ToHtml
{
	public static class ToHtmlExporter
	{
		public static void WriteToFile(VoxelModel model, string outputPath)
		{
			var layeredModel = LayeredModel.Create(model);
			string spriteAtlasImage = layeredModel.RenderImageString();

			var rotation = new Vector2(55, -30);
			float scale = 22.5f;

			using (var outputStream = new FileInfo(outputPath).OpenWrite())
			using (var output = new StreamWriter(outputStream))
			{
				output.WriteLine($"<style>");
				output.WriteLine($"		.scene {{");
				output.WriteLine($"			display: inline-block;");
				output.WriteLine($"			overflow: hidden;");
				output.WriteLine($"			box-sizing: border-box;");
				output.WriteLine($"			perspective: 2048px;");
				output.WriteLine($"			width: 100%;");
				output.WriteLine($"			height: 650px;");
				output.WriteLine($"		}}");

				output.WriteLine($"");

				output.WriteLine($"		.scene .mesh {{");
				output.WriteLine($"			position: relative;");
				output.WriteLine($"			margin: auto;");
				output.WriteLine($"			transform-style: preserve-3d;");
				output.WriteLine($"			transition: transform 0.6s;");
				output.WriteLine($"		}}");

				output.WriteLine($"");

				output.WriteLine($"			.scene .mesh .surface {{");
				output.WriteLine($"				transform-origin: 0% 0%;");
				output.WriteLine($"				position: absolute;");
				output.WriteLine($"				pointer-events: none;");
				output.WriteLine($"				touch-action: none;");
				output.WriteLine($"				image-rendering: pixelated;");
				output.WriteLine($"				user-select: none;");
				output.WriteLine($"				backface-visibility: hidden;");
				output.WriteLine($"			}}");


				output.WriteLine($".anim-rotate-left {{");
				output.WriteLine($"	animation: anim-rotate-left 1s linear normal;");
				output.WriteLine($"	animation-play-state: paused;");
				output.WriteLine($"	animation-delay: calc(var(--scroll) * -1s);");
				output.WriteLine($"}}");

				output.WriteLine($"");

				output.WriteLine($"@keyframes anim-rotate-left {{");
				output.WriteLine($"	from {{");
				output.WriteLine($"		transform: rotateX(20deg) rotateY(50deg) scale3d({scale}, {scale}, {scale});");
				output.WriteLine($"	}}");
				output.WriteLine($"");
				output.WriteLine($"	to {{");
				output.WriteLine($"		transform: rotateX(-45deg) rotateY(10deg) scale3d({scale}, {scale}, {scale});");
				output.WriteLine($"	}}");
				output.WriteLine($"}}");

				output.WriteLine($"");

				output.WriteLine($".anim-rotate-vertical {{");
				output.WriteLine($"	animation: anim-rotate-vertical 1s linear normal;");
				output.WriteLine($"	animation-play-state: paused;");
				output.WriteLine($"	animation-delay: calc(var(--scroll) * -1s);");
				output.WriteLine($"}}");

				output.WriteLine($"");

				output.WriteLine($"@keyframes anim-rotate-vertical {{");
				output.WriteLine($"	from {{");
				output.WriteLine($"		transform: rotateX(20deg) rotateY(45deg) scale3d({scale}, {scale}, {scale});");
				output.WriteLine($"	}}");
				output.WriteLine($"");
				output.WriteLine($"	to {{");
				output.WriteLine($"		transform: rotateX(-45deg) rotateY(45deg) scale3d({scale}, {scale}, {scale});");
				output.WriteLine($"	}}");
				output.WriteLine($"}}");

				output.WriteLine($"</style>");

				output.WriteLine($"<div style=\"height: 100vh\"></div>");

				output.WriteLine($"<div class=\"scene\" style = \"padding: 320px 50px 50px 50px;\">");

				output.WriteLine($"<style>");
				output.WriteLine($"	.surface {{");
				output.WriteLine($"		background-image: url({spriteAtlasImage});");
				output.WriteLine($"		background-size: {layeredModel.SpriteAtlasResolution.Width}px {layeredModel.SpriteAtlasResolution.Height}px;");
				output.WriteLine($"	}}");
				output.WriteLine($"</style>");

				output.WriteLine($"");

				output.WriteLine($"<div class=\"mesh anim-rotate-left fsa-relative\" style=\"");
				output.WriteLine($"	width: {model.Width}px;");
				output.WriteLine($"	height: {model.Height}px;");
				output.WriteLine($"	transform: rotateX({rotation.Y}deg) rotateY({rotation.X}deg) scale3d({scale}, {scale}, {scale})\">");

				output.WriteLine($"");

				foreach (var sprite in layeredModel.Sprites)
				{
					output.WriteLine($"		<div class=\"surface\" style=\"");
					output.WriteLine($"			background-position: -{sprite.UV.X}px -{sprite.UV.Y}px;");
					output.WriteLine($"			width: {sprite.Position.Width}px;		");
					output.WriteLine($"			height: {sprite.Position.Height}px;	");
					output.WriteLine($"			transform: {sprite.ToTransform()}\"></div>");
				}
				output.WriteLine($"</div>");

				output.WriteLine($"</div>");

				output.WriteLine($"</div>");

				output.WriteLine($"<div style=\"height: 100vh\"></div>");

				output.WriteLine("<script>");
				output.WriteLine("	const clamp = (a, min = 0, max = 1) => Math.min(max, Math.max(min, a));");
				output.WriteLine("	const invlerp = (x, y, a) => clamp((a - x) / (y - x));");
				output.WriteLine("	");
				output.WriteLine("	function UpdateRelativeElements() {");
				output.WriteLine("		var fsaRelativeObjects = document.getElementsByClassName(\"fsa-relative\");");
				output.WriteLine("	");
				output.WriteLine("		for (let p = 0; p < fsaRelativeObjects.length; p++) {");
				output.WriteLine("			var fsaRelative = fsaRelativeObjects[p];");
				output.WriteLine("	");
				output.WriteLine("			var bounds = fsaRelative.getBoundingClientRect();");
				output.WriteLine("			var scroll = invlerp(-bounds.height, screen.height, bounds.top);");
				output.WriteLine("	");
				output.WriteLine("			fsaRelative.style.setProperty(\"--scroll\", scroll);");
				output.WriteLine("		}");
				output.WriteLine("	}");
				output.WriteLine("	");
				output.WriteLine("	window.addEventListener(\"scroll\",");
				output.WriteLine("		() => { UpdateRelativeElements(); }, false");
				output.WriteLine("	);");
				output.WriteLine("	window.addEventListener(\"resize\",");
				output.WriteLine("		() => { UpdateRelativeElements(); }, false");
				output.WriteLine("	);");
				output.WriteLine("	UpdateRelativeElements();");
				output.WriteLine("</script>");

			}
		}
	}
}
