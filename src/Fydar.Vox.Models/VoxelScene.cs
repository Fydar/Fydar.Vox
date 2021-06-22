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

		public VoxelScene()
		{
			Layers = new VoxelLayer[]
			{
				new VoxelLayer(0)
			};
			Pallette = VoxelColourPallette.GenerateDefault();
			Models = Array.Empty<VoxelModel>();
			Root = new VoxelTransform(1);
		}

		public VoxelScene(VoxDocument document)
		{
			Pallette = LoadPallette(document);
			Layers = LoadLayers(document);
			Models = LoadModels(this, document);
			Root = LoadHierarchy(this, document, Layers);
		}

		private static VoxelNode LoadHierarchy(VoxelScene scene, VoxDocument document, VoxelLayer[] layers)
		{
			VoxelTransform? root = null;
			var nodes = new Dictionary<int, VoxelNode>();

			foreach (var chunk in document.Main.Children)
			{
				if (VoxChunks.NTRN.CanImport(chunk.NameToString()))
				{
					int offset = chunk.ContentStartIndex;
					var chunknTRN = VoxChunks.NTRN.Read(document, ref offset);

					var transformNode = new VoxelTransform(chunknTRN.NodeId)
					{
						NodeAttributes = chunknTRN.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
						Layer = layers.FirstOrDefault(layer => layer.LayerId == chunknTRN.LayerId),
						ReservedId = chunknTRN.ReservedId,
						childNodeId = chunknTRN.ChildNodeId
					};
					if (root == null)
					{
						root = transformNode;
					}
					nodes.Add(transformNode.NodeId, transformNode);
				}
			}
			foreach (var chunk in document.Main.Children)
			{
				if (VoxChunks.NSHP.CanImport(chunk.NameToString()))
				{
					int offset = chunk.ContentStartIndex;
					var chunknSHP = VoxChunks.NSHP.Read(document, ref offset);

					var shapeNode = new VoxelShape(chunknSHP.NodeId)
					{
						NodeAttributes = chunknSHP.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
					};
					shapeNode.Models.AddRange(chunknSHP.Models.Elements
						.Select(model => scene.Models[model.ModelId])
						.ToList());

					foreach (var model in shapeNode.Models)
					{
						model.Parents.Add(shapeNode);
					}
					nodes.Add(shapeNode.NodeId, shapeNode);
				}
			}
			foreach (var chunk in document.Main.Children)
			{
				if (VoxChunks.NGRP.CanImport(chunk.NameToString()))
				{
					int offset = chunk.ContentStartIndex;
					var chunknGRP = VoxChunks.NGRP.Read(document, ref offset);

					var groupNode = new VoxelGrouping(chunknGRP.NodeId)
					{
						NodeAttributes = chunknGRP.NodeAttributes.KeyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
					};

					foreach (int child in chunknGRP.ChildrenIds.Values)
					{
						var groupChildEntry = nodes[child];
						groupNode.Children.Add(groupChildEntry);
						groupChildEntry.Parent = groupNode;
					}

					nodes.Add(groupNode.NodeId, groupNode);
				}
			}

			foreach (var node in nodes)
			{
				if (node.Value is VoxelTransform transformNode)
				{
					var transformedChild = nodes[transformNode.childNodeId];
					transformNode.ChildNode = transformedChild;
					transformedChild.Parent = transformNode;
				}
			}

			if (root == null)
			{
				throw new InvalidOperationException("Unable to determine scene root");
			}

			scene.Root = root;
			return root;
		}

		private static EditableVoxelModel[] LoadModels(VoxelScene scene, VoxDocument document)
		{
			var models = new List<EditableVoxelModel>();
			VoxDocumentDiamentions? lastDiamentions = null;

			// Find and construct VoxelModels from the model.
			foreach (var chunk in document.Main.Children)
			{
				int offset = chunk.ContentStartIndex;

				switch (chunk.NameToString())
				{
					case "SIZE":
					{
						var chunkSIZE = VoxChunks.SIZE.Read(document, ref offset);

						lastDiamentions = new VoxDocumentDiamentions()
						{
							X = chunkSIZE.X,
							Y = chunkSIZE.Y,
							Z = chunkSIZE.Z
						};
						break;
					}
					case "XYZI":
					{
						var chunkXYZI = VoxChunks.XYZI.Read(document, ref offset);

						if (lastDiamentions != null)
						{
							models.Add(new EditableVoxelModel(scene, lastDiamentions.Value, chunkXYZI.Voxels));
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
			var layers = new List<VoxelLayer>(8);

			foreach (var chunk in document.Main.Children)
			{
				if (chunk.NameToString() == "LAYR")
				{
					int offset = chunk.ContentStartIndex;
					var chunkLAYR = VoxChunks.LAYR.Read(document, ref offset);

					var layerAttributes = new Dictionary<string, string>();
					foreach (var kvp in chunkLAYR.VoxDictionary.KeyValuePairs)
					{
						layerAttributes.Add(kvp.Key, kvp.Value);
					}

					var layer = new VoxelLayer(chunkLAYR.LayerId, chunkLAYR.VoxDictionary.KeyValuePairs)
					{
						ReservedId = chunkLAYR.ReservedId
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
					var chunkRGBA = VoxChunks.RGBA.Read(document, ref offset);

					var colours = new VoxDocumentColour[256];
					int index = 1;
					foreach (var color in chunkRGBA.Colours.Colours)
					{
						colours[index] = color;
						index++;
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
