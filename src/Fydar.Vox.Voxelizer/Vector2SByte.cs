using System;

namespace Fydar.Vox.Voxelizer
{
	public struct Vector2SByte : IEquatable<Vector2SByte>
	{
		public static Vector2SByte Zero => new Vector2SByte(0, 0);
		public static Vector2SByte One => new Vector2SByte(1, 1);

		public static Vector2SByte Left => new Vector2SByte(-1, 0);
		public static Vector2SByte Right => new Vector2SByte(1, 0);
		public static Vector2SByte Up => new Vector2SByte(0, 1);
		public static Vector2SByte Down => new Vector2SByte(0, -1);

		public sbyte x;
		public sbyte y;

		public Vector2SByte(sbyte x, sbyte y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"({x}, {y})";
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2SByte @byte && Equals(@byte);
		}

		public bool Equals(Vector2SByte other)
		{
			return x == other.x &&
				   y == other.y;
		}

		public override int GetHashCode()
		{
			int hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public static Vector2SByte operator +(Vector2SByte left, Vector2SByte right)
		{
			return new Vector2SByte(
				(sbyte)(left.x + right.x),
				(sbyte)(left.y + right.y)
			);
		}

		public static Vector2SByte operator -(Vector2SByte left, Vector2SByte right)
		{
			return new Vector2SByte(
				(sbyte)(left.x - right.x),
				(sbyte)(left.y - right.y)
			);
		}

		public static bool operator ==(Vector2SByte left, Vector2SByte right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector2SByte left, Vector2SByte right)
		{
			return !(left == right);
		}
	}
}
