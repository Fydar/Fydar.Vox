using Fydar.Vox.Export.ToHtml;
using Fydar.Vox.VoxFiles;
using Fydar.Vox.VoxFiles.Hierarchy;
using Fydar.Voxelizer.Demo;
using System;
using System.IO;

namespace Fydar.Vox.ConsoleDemo
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			foreach (var modelFile in Directory.EnumerateFiles("models", "*.vox"))
			{
				var name = new FileInfo(modelFile).Name.ToLower().Replace(".vox", "");
				ExportFile(name, modelFile, $"output/{name}.html");
			}

			FirstDemo.Run();

			var fileInfo = new FileInfo("models/demo-scene.vox");

			var document = new VoxDocument(File.ReadAllBytes(fileInfo.FullName));

			var voxelScene = new VoxelScene(document);

			Console.WriteLine("Colour Pallette");
			// VoxelConsoleRenderer.RenderLargeColourPallette(voxelScene.Pallette);
			VoxelConsoleRenderer.RenderSmallColourPallette(voxelScene.Pallette);

			Console.WriteLine();

			Console.WriteLine("Layers");
			PrintLayers(voxelScene);

			Console.WriteLine();

			Console.WriteLine("Hierarchy");
			PrintHierarchy(voxelScene);

			Console.WriteLine();

			Console.WriteLine("Models");
			PrintModels(voxelScene);
		}

		private static void PrintModels(VoxelScene voxelScene)
		{
			for (int i = 0; i < voxelScene.Models.Length; i++)
			{
				var model = voxelScene.Models[i];
				if (i == voxelScene.Models.Length - 1)
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(" └─ ");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(" ├─ ");
				}
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine(model.Parents[0].Parent?.Name ?? "");
			}
			Console.ResetColor();
		}

		private static void ExportFile(string name, string source, string destination)
		{
			var fileInfo = new FileInfo(source);

			var voxDocument = new VoxDocument(File.ReadAllBytes(fileInfo.FullName));
			var voxelScene = new VoxelScene(voxDocument);
			var vodel = voxelScene.Models[0];

			ToHtmlExporter.WriteToFile(name, vodel, destination);
		}

		private static void PrintLayers(VoxelScene voxelScene)
		{
			for (int i = 0; i < voxelScene.Layers.Length; i++)
			{
				var layer = voxelScene.Layers[i];

				if (i == voxelScene.Layers.Length - 1)
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(" └─ ");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(" ├─ ");
				}
				Console.ForegroundColor = layer.Hidden ? ConsoleColor.DarkGray : ConsoleColor.Gray;
				Console.WriteLine($"{layer}");
			}
			Console.ResetColor();
		}

		private static void PrintHierarchy(VoxelScene voxelScene)
		{
			PrintHierarchyRecursive(voxelScene.Root, 0, true, 0);
		}

		private static void PrintHierarchyRecursive(VoxelNode voxelNode, byte depth, bool last, ulong carryMask)
		{
			switch (voxelNode)
			{
				case VoxelGrouping voxelGrouping:
				{
					for (int i = 0; i < voxelGrouping.Children.Count; i++)
					{
						var child = voxelGrouping.Children[i];
						bool childIsLast = i == voxelGrouping.Children.Count - 1;
						ulong childMask = carryMask;
						if (!childIsLast)
						{
							childMask |= (ulong)1 << (depth - 1);
						}
						PrintHierarchyRecursive(child, depth, childIsLast, childMask);
					}
					break;
				}
				case VoxelTransform voxelTransform:
				{
					WriteIndent(depth, last, carryMask);
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine(voxelTransform.Name);

					if (voxelTransform.ChildNode != null)
					{
						PrintHierarchyRecursive(voxelTransform.ChildNode, (byte)(depth + 1), true, carryMask);
					}
					break;
				}
			}
			Console.ResetColor();
		}

		private static void WriteIndent(int depth, bool last, ulong carryMask)
		{
			if (depth == 0)
			{
				return;
			}

			for (int i = 0; i < depth - 1; i++)
			{
				ulong targetMask = (ulong)1 << i;

				if ((carryMask & targetMask) == targetMask)
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(" │  ");
				}
				else
				{
					Console.Write("    ");
				}
			}
			if (last)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write(" └─ ");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write(" ├─ ");
			}
		}
	}
}
