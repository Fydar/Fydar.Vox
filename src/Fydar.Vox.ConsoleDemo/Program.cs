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
			FirstDemo.Run();


			var fileInfo = new FileInfo("demo-scene.vox");

			var document = new VoxDocument(File.ReadAllBytes(fileInfo.FullName));
			Console.WriteLine(document.FileVersionNumber);

			PrintStructure(document.Main);

			var voxelScene = new VoxelScene(document);

			Console.WriteLine("Colour Pallette");
			VoxelConsoleRenderer.RenderLargeColourPallette(voxelScene.Pallette);

			Console.WriteLine();

			Console.WriteLine("Layers");
			PrintLayers(voxelScene);

			Console.WriteLine();

			Console.WriteLine("Hierarchy");
			PrintHierarchy(voxelScene);

			Console.WriteLine("Models");
			foreach (var model in voxelScene.Models)
			{
				Console.WriteLine(model.Shape.Parent.Name);
			}
		}

		private static void PrintLayers(VoxelScene voxelScene)
		{
			for (int i = 0; i < voxelScene.Layers.Length; i++)
			{
				var layer = voxelScene.Layers[i];
				if (layer.Hidden)
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
				}

				if (i == voxelScene.Layers.Length - 1)
				{
					Console.Write(" └─ ");
				}
				else
				{
					Console.Write(" ├─ ");
				}
				Console.WriteLine($"{layer}");
				Console.ResetColor();
			}
		}

		private static void PrintStructure(VoxStructureChunk chunk, int indent = 0)
		{
			Console.WriteLine(new string(' ', indent * 2) + chunk.NameToString());

			foreach (var child in chunk.Children)
			{
				PrintStructure(child, indent + 1);
			}
		}

		private static void PrintHierarchy(VoxelScene voxelScene)
		{
			PrintHierarchyRecursive(voxelScene.Root, 0, true, 0);
		}

		private static void PrintHierarchyRecursive(VoxelNode voxelNode, byte depth, bool last, ulong carryMask)
		{
			switch (voxelNode)
			{
				case VoxelShape voxelShape:
					// Console.WriteLine(voxelShape.Name);

					foreach (var model in voxelShape.Models)
					{
						// WriteIndent(depth, last, carryMask);
						// Console.WriteLine(model.GetHashCode().ToString());
					}
					break;

				case VoxelGrouping voxelGrouping:
					WriteIndent(depth, last, carryMask);
					Console.WriteLine(voxelGrouping.ToString());

					for (int i = 0; i < voxelGrouping.Children.Count; i++)
					{
						var child = voxelGrouping.Children[i];
						bool childIsLast = i == voxelGrouping.Children.Count - 1;
						ulong childMask = carryMask;
						if (!childIsLast)
						{
							childMask |= (ulong)1 << depth;
						}

						PrintHierarchyRecursive(child, (byte)(depth + 1), childIsLast, childMask);
					}
					break;

				case VoxelTransform voxelTransform:
					WriteIndent(depth, last, carryMask);
					Console.WriteLine(voxelTransform.ToString());

					if (voxelTransform.ChildNode != null)
					{
						PrintHierarchyRecursive(voxelTransform.ChildNode, (byte)(depth + 1), true, carryMask);
					}
					break;
			}
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
					Console.Write(" │  ");
				}
				else
				{
					Console.Write("    ");
				}
			}
			if (last)
			{
				Console.Write(" └─ ");
			}
			else
			{
				Console.Write(" ├─ ");
			}
		}
	}
}
