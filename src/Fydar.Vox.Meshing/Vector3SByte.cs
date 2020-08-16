using System;

namespace Fydar.Vox.Meshing
{
	public struct Vector3SByte : IEquatable<Vector3SByte>
	{
		public static Vector3SByte Zero => new Vector3SByte(0, 0, 0);
		public static Vector3SByte One => new Vector3SByte(1, 1, 1);

		public static Vector3SByte Left => new Vector3SByte(-1, 0, 0);
		public static Vector3SByte Right => new Vector3SByte(1, 0, 0);
		public static Vector3SByte Up => new Vector3SByte(0, 1, 0);
		public static Vector3SByte Down => new Vector3SByte(0, -1, 0);
		public static Vector3SByte Forward => new Vector3SByte(0, 0, 1);
		public static Vector3SByte Back => new Vector3SByte(0, 0, -1);

		public sbyte x;
		public sbyte y;
		public sbyte z;

		public Vector3SByte(sbyte x, sbyte y, sbyte z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3SByte(int x, int y, int z)
		{
			this.x = (sbyte)x;
			this.y = (sbyte)y;
			this.z = (sbyte)z;
		}

		public override string ToString()
		{
			return $"({x}, {y}, {z})";
		}

		public override bool Equals(object obj)
		{
			return obj is Vector3SByte vector3SByte && Equals(vector3SByte);
		}

		public bool Equals(Vector3SByte other)
		{
			return x == other.x &&
				   y == other.y &&
				   z == other.z;
		}

		public override int GetHashCode()
		{
			int hashCode = 373119288;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			hashCode = hashCode * -1521134295 + z.GetHashCode();
			return hashCode;
		}

		public static Vector3SByte operator +(Vector3SByte left, Vector3SByte right)
		{
			return new Vector3SByte(
				(sbyte)(left.x + right.x),
				(sbyte)(left.y + right.y),
				(sbyte)(left.z + right.z)
			);
		}

		public static Vector3SByte operator -(Vector3SByte left, Vector3SByte right)
		{
			return new Vector3SByte(
				(sbyte)(left.x - right.x),
				(sbyte)(left.y - right.y),
				(sbyte)(left.z - right.z)
			);
		}

		public static bool operator ==(Vector3SByte left, Vector3SByte right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector3SByte left, Vector3SByte right)
		{
			return !(left == right);
		}

		public static Vector3SByte Cross(Vector3SByte v1, Vector3SByte v2)
		{
			int crossX = v1.y * v2.z - v2.y * v1.z;
			int crossY = (v1.x * v2.z - v2.x * v1.z) * -1;
			int crossZ = v1.x * v2.y - v2.x * v1.y;

			return new Vector3SByte((sbyte)crossX, (sbyte)crossY, (sbyte)crossZ);
		}
	}
}
