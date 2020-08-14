using Fydar.Vox.VoxFiles.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fydar.Vox.VoxFiles
{
	public class VoxelScene
	{
		public VoxelModel[] Models { get; set; }
		public VoxelColourPallette Pallette { get; set; }
		public VoxelLayer[] Layers { get; set; }

		public VoxelNode Root { get; set; }

		public List<VoxDocumentNodeChunk> HierarchyNodes;

		public VoxelScene(VoxDocument document)
		{
			Pallette = LoadPallette(document);
			Layers = LoadLayers(document);
			Models = LoadModels(this, document);
			Root = LoadHierarchy(this, document, Layers);
		}

		private static VoxelNode LoadHierarchy(VoxelScene scene, VoxDocument document, VoxelLayer[] layers)
		{
			var transformChunks = new List<VoxDocumentTransformNodeChunk>();
			var groupChunks = new List<VoxDocumentGroupNodeChunk>();
			var shapeChunks = new List<VoxDocumentShapeNodeChunk>();

			// Find and construct nodes from the file
			foreach (var chunk in document.Main.Children)
			{
				switch (chunk.NameToString())
				{
					case "nTRN":
					{
						int offset = chunk.ContentStartIndex;

						int nodeId = document.ReadInt32(ref offset);
						var nodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
						int childNodeId = document.ReadInt32(ref offset);
						int reservedId = document.ReadInt32(ref offset);
						int layerId = document.ReadInt32(ref offset);
						int numberOfFrames = document.ReadInt32(ref offset);

						var voxelTransform = new VoxDocumentTransformNodeChunk()
						{
							NodeId = nodeId,
							NodeAttributes = nodeAttributes,
							ChildNodeId = childNodeId,
							LayerId = layerId,
							NumberOfFrames = numberOfFrames,
							ReservedId = reservedId
						};
						transformChunks.Add(voxelTransform);
						break;
					}
					case "nGRP":
					{
						int offset = chunk.ContentStartIndex;

						int nodeId = document.ReadInt32(ref offset);
						var nodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
						var childrenIds = document.ReadStructure<VoxStructureIntArray>(ref offset);

						var voxelGroup = new VoxDocumentGroupNodeChunk()
						{
							NodeId = nodeId,
							NodeAttributes = nodeAttributes,
							ChildrenIds = childrenIds
						};
						groupChunks.Add(voxelGroup);
						break;
					}
					case "nSHP":
					{
						int offset = chunk.ContentStartIndex;

						int nodeId = document.ReadInt32(ref offset);
						var nodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
						var models = document.ReadStructure<VoxStructureShapeModelArray>(ref offset);

						var voxelShape = new VoxDocumentShapeNodeChunk()
						{
							NodeId = nodeId,
							NodeAttributes = nodeAttributes,
							Models = models,
						};
						shapeChunks.Add(voxelShape);
						break;
					}
				}
			}

			VoxelTransform root = null;
			var nodes = new Dictionary<int, VoxelNode>();

			foreach (var transformChunk in transformChunks)
			{
				var transformNode = new VoxelTransform(transformChunk.NodeId)
				{
					NodeAttributes = transformChunk.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
					Layer = layers.FirstOrDefault(layer => layer.LayerId == transformChunk.LayerId),
					ReservedId = transformChunk.ReservedId,
				};
				if (root == null)
				{
					root = transformNode;
				}
				nodes.Add(transformNode.NodeId, transformNode);
			}
			foreach (var shapeChunk in shapeChunks)
			{
				var shapeNode = new VoxelShape(shapeChunk.NodeId)
				{
					NodeAttributes = shapeChunk.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
					Models = shapeChunk.Models.Elements
						.Select(model => scene.Models[model.ModelId])
						.ToList()
				};
				foreach (var model in shapeNode.Models)
				{
					model.Shape = shapeNode;
				}
				nodes.Add(shapeNode.NodeId, shapeNode);
			}
			foreach (var groupChunk in groupChunks)
			{
				var groupNode = new VoxelGrouping(groupChunk.NodeId)
				{
					NodeAttributes = groupChunk.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				};
				nodes.Add(groupNode.NodeId, groupNode);
			}

			foreach (var transformChunk in transformChunks)
			{
				var transformNode = (VoxelTransform)nodes[transformChunk.NodeId];

				var transformedChild = nodes[transformChunk.ChildNodeId];
				transformNode.ChildNode = transformedChild;
				transformedChild.Parent = transformNode;
			}
			foreach (var groupChunk in groupChunks)
			{
				var groupNode = (VoxelGrouping)nodes[groupChunk.NodeId];
				foreach (int child in groupChunk.ChildrenIds.Values)
				{
					var groupChildEntry = nodes[child];
					groupNode.Children.Add(groupChildEntry);
					groupChildEntry.Parent = groupNode;
				}
			}

			scene.Root = root;
			return root;
		}

		private static VoxelModel[] LoadModels(VoxelScene scene, VoxDocument document)
		{
			var models = new List<VoxelModel>();
			VoxDocumentDiamentions? lastDiamentions = null;

			// Find and construct VoxelModels from the model.
			foreach (var chunk in document.Main.Children)
			{
				var content = chunk.Content;

				switch (chunk.NameToString())
				{
					case "SIZE":
					{
						int offset = chunk.ContentStartIndex;

						int x = document.ReadInt32(ref offset);
						int y = document.ReadInt32(ref offset);
						int z = document.ReadInt32(ref offset);

						lastDiamentions = new VoxDocumentDiamentions()
						{
							X = x,
							Y = y,
							Z = z
						};
						break;
					}
					case "XYZI":
					{
						int offset = chunk.ContentStartIndex;

						int voxelsCount = document.ReadInt32(ref offset);

						var voxelArray = new VoxDocumentVoxel[voxelsCount];
						for (int i = 0; i < voxelsCount; i++)
						{
							byte x = document.ReadByte(ref offset);
							byte y = document.ReadByte(ref offset);
							byte z = document.ReadByte(ref offset);
							byte index = document.ReadByte(ref offset);

							voxelArray[i] = new VoxDocumentVoxel(x, y, z, index);
						}

						var voxels = new VoxDocumentVoxels()
						{
							Voxels = voxelArray
						};

						if (lastDiamentions != null)
						{
							models.Add(new VoxelModel(scene, lastDiamentions.Value, voxels));
						}
						else
						{
							throw new InvalidOperationException("Read an XYZI before a SIZE chunk.");
						}
						break;
					}
				}
			}
			return models.ToArray();
		}

		private static VoxelLayer[] LoadLayers(VoxDocument document)
		{
			var layers = new List<VoxelLayer>();

			foreach (var chunk in document.Main.Children)
			{
				if (chunk.NameToString() == "LAYR")
				{
					int offset = chunk.ContentStartIndex;

					int layerId = document.ReadInt32(ref offset);
					var voxDictionary = document.ReadStructure<VoxStructureDictionary>(ref offset);
					int reservedId = document.ReadInt32(ref offset);

					var layerAttributes = new Dictionary<string, string>();
					foreach (var kvp in voxDictionary.KeyValuePairs)
					{
						layerAttributes.Add(kvp.Key, kvp.Value);
					}

					var layer = new VoxelLayer(layerId)
					{
						LayerAttributes = layerAttributes,
						ReservedId = reservedId
					};
					layers.Add(layer);
				}
			}
			return layers.ToArray();
		}

		private static VoxelColourPallette LoadPallette(VoxDocument document)
		{
			// Load the colour pallette from the file.
			foreach (var chunk in document.Main.Children)
			{
				if (chunk.NameToString() == "RGBA")
				{
					int offset = chunk.ContentStartIndex;

					var colours = new VoxDocumentColour[256];
					for (int i = 0; i < 255; i++)
					{
						byte r = document.ReadByte(ref offset);
						byte g = document.ReadByte(ref offset);
						byte b = document.ReadByte(ref offset);
						byte a = document.ReadByte(ref offset);

						var colour = new VoxDocumentColour()
						{
							R = r,
							G = g,
							B = b,
							A = a
						};

						colours[i + 1] = colour;
					}

					return new VoxelColourPallette()
					{
						Colours = colours
					};
				}
			}

			// Couldn't find any chunks with the name "RGBA"; using default colour pallette
			return VoxelColourPallette.GenerateDefault();
		}
	}
}
