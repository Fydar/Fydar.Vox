using System;

namespace Fydar.Vox.Meshing
{
	public struct VoxelNormal : IEquatable<VoxelNormal>
	{
		private static readonly Vector3SByte[] vectorMapping = new Vector3SByte[]
		{
			new Vector3SByte(0, 1, 0), // Up
			new Vector3SByte(0, -1, 0), // Down
			new Vector3SByte(-1, 0, 0), // Left
			new Vector3SByte(1, 0, 0), // Right
			new Vector3SByte(0, 0, 1), // Forward
			new Vector3SByte(0, 0, -1), // Back
		};
		private static readonly string[] nameMapping = new string[]
		{
			"Up",
			"Down",
			"Left",
			"Right",
			"Forward",
			"Back",
		};

		public static VoxelNormal Up => new VoxelNormal(0);
		public static VoxelNormal Down => new VoxelNormal(1);
		public static VoxelNormal Left => new VoxelNormal(2);
		public static VoxelNormal Right => new VoxelNormal(3);
		public static VoxelNormal Forward => new VoxelNormal(4);
		public static VoxelNormal Back => new VoxelNormal(5);

		public byte data;

		public VoxelNormal(byte data)
		{
			this.data = data;
		}

		public Vector3SByte ToVector()
		{
			return vectorMapping[data];
		}

		public override string ToString()
		{
			return nameMapping[data];
		}

		public static Vector3SByte RotateAroundOrigin(Vector3SByte position, VoxelNormal normal)
		{
			if (normal == Forward)
			{
				return new Vector3SByte(position.x, position.y, position.z);
			}
			else if (normal == Back)
			{
				return new Vector3SByte(position.x, position.y, -position.z);
			}
			else if (normal == Left)
			{
				return new Vector3SByte(position.z, position.y, -position.x);
			}
			else if (normal == Right)
			{
				return new Vector3SByte(position.z, position.y, position.x);
			}
			else if (normal == Up)
			{
				return new Vector3SByte(position.x, position.z, position.y);
			}
			else if (normal == Down)
			{
				return new Vector3SByte(position.x, position.z, -position.y);
			}

			throw new InvalidOperationException($"The normal {normal} is unsupported.");
		}

		public override bool Equals(object obj)
		{
			return obj is VoxelNormal normal && Equals(normal);
		}

		public bool Equals(VoxelNormal other)
		{
			return data == other.data;
		}

		public override int GetHashCode()
		{
			return 1768953197 + data.GetHashCode();
		}

		public static bool operator ==(VoxelNormal left, VoxelNormal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(VoxelNormal left, VoxelNormal right)
		{
			return !(left == right);
		}
	}
}
