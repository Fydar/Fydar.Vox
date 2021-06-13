using BenchmarkDotNet.Attributes;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;
using System;
using System.Collections.Generic;

namespace Fydar.Vox.Meshing.Benchmarks
{
	public class MeshingBenchmarks
	{
		[ParamsSource(nameof(TestCases))]
		public MeshingTestCase TestCase { get; set; }

		public List<MeshingTestCase> TestCases { get; }

		public MeshingBenchmarks()
		{
			var scene = new VoxelScene
			{
				Pallette = VoxelColourPallette.GenerateDefault()
			};

			var testModels = new List<VoxelModel>();

			int[] cubeSizes = new int[] { 1, 8, 16, 32 };

			foreach (int cubeSize in cubeSizes)
			{
				var cubeModel = new EditableVoxelModel(scene, cubeSize, cubeSize, cubeSize);

				for (int x = 0; x < cubeSize; x++)
				{
					for (int y = 0; y < cubeSize; y++)
					{
						for (int z = 0; z < cubeSize; z++)
						{
							cubeModel.Voxels[x, y, z] = new VoxelModelVoxel() { Index = 1 };
						}
					}
				}
				testModels.Add(cubeModel);
			}

			scene.Models = testModels.ToArray();

			TestCases = new List<MeshingTestCase>();
			MeshingTestCase? defaultTestCase = null;
			foreach (var model in scene.Models)
			{
				var newTestCase = new MeshingTestCase($"Cube {model.Width}x{model.Height}x{model.Depth}", model);
				defaultTestCase = newTestCase;
				TestCases.Add(newTestCase);
			}

			TestCase = defaultTestCase ?? throw new InvalidOperationException("Unable to locate any test cases");
		}

		[Benchmark]
		public void GroupedMeshing()
		{
			var dataDriver = new VoxelModelImporter(TestCase.Model);
			var groupedMesher = new GroupedMesher(dataDriver);

			groupedMesher.Voxelize();
		}

		[Benchmark]
		public void GreedyMeshing()
		{
			var greedyMesher = new GreedyMesher();
			greedyMesher.Optimize(TestCase.GroupedMesh);
		}
	}
}
