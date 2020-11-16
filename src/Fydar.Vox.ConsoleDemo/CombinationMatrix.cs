using System.Collections;
using System.Collections.Generic;

namespace Fydar.Vox.Voxeliser.UnitTests
{
	public class MatrixColumn<T>
	{
		public List<T> Variations = new List<T>();

		public int Count => Variations.Count;

		public override string ToString()
		{
			return string.Join(", ", Variations);
		}
	}

	public class CombinationMatrix<T> : IEnumerable<CombinationMatrix<T>.MatrixSet>
	{
		public struct MatrixSet
		{
			private readonly CombinationMatrix<T> matrix;
			private readonly int[] indexes;

			public int Count => indexes.Length;

			public T this[int index] => matrix.Matrix[index].Variations[indexes[index]];

			internal MatrixSet(CombinationMatrix<T> matrix, int[] indexes)
			{
				this.matrix = matrix;
				this.indexes = indexes;
			}
		}

		public List<MatrixColumn<T>> Matrix;

		public MatrixColumn<T> this[int set]
		{
			get
			{
				while (Matrix.Count <= set)
				{
					Matrix.Add(new MatrixColumn<T>());
				}
				return Matrix[set];
			}
		}

		public int Count
		{
			get
			{
				int current = Matrix[0].Count;

				for (int i = 1; i < Matrix.Count; i++)
				{
					var set = Matrix[i];
					current *= set.Count;
				}

				return current;
			}
		}

		public CombinationMatrix()
		{
			Matrix = new List<MatrixColumn<T>>();
		}

		public CombinationMatrix(List<MatrixColumn<T>> matrix)
		{
			Matrix = matrix;
		}

		public IEnumerator<MatrixSet> GetEnumerator()
		{
			int[] indexes = new int[Matrix.Count];

			for (int i = 0; i < Count; i++)
			{
				int tally = i;

				yield return new MatrixSet(this, indexes);

				for (int j = 0; j < indexes.Length; j++)
				{
					var matrix = Matrix[j];

					indexes[j] = tally % matrix.Count;
					tally /= matrix.Count;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
