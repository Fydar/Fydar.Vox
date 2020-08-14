using System;

namespace Fydar.Vox.Voxelizer
{
	public struct Colour24 : IEquatable<Colour24>
	{
		public static Colour24 White => new Colour24(255, 255, 255);
		public static Colour24 Black => new Colour24(0, 0, 0);

		public static Colour24 Red => new Colour24(255, 0, 0);
		public static Colour24 Green => new Colour24(0, 255, 0);
		public static Colour24 Orange => new Colour24(255, 128, 0);
		public static Colour24 Blue => new Colour24(0, 0, 255);

		public byte r;
		public byte g;
		public byte b;

		public Colour24(byte r, byte g, byte b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public override string ToString()
		{
			return $"({r}, {g}, {b})";
		}

		public string ToHexString()
		{
			return $"#{r:X2}{g:X2}{b:X2}";
		}

		public override bool Equals(object obj)
		{
			return obj is Colour24 colour && Equals(colour);
		}

		public bool Equals(Colour24 other)
		{
			return r == other.r &&
				   g == other.g &&
				   b == other.b;
		}

		public override int GetHashCode()
		{
			int hashCode = -839137856;
			hashCode = hashCode * -1521134295 + r.GetHashCode();
			hashCode = hashCode * -1521134295 + g.GetHashCode();
			hashCode = hashCode * -1521134295 + b.GetHashCode();
			return hashCode;
		}

		public static Colour24 Lerp(Colour24 from, Colour24 to, float time)
		{
			static float Lerp(float firstFloat, float secondFloat, float by)
			{
				return firstFloat * (1 - by) + secondFloat * by;
			}

			return new Colour24(
				(byte)Lerp(from.r, to.r, time),
				(byte)Lerp(from.g, to.g, time),
				(byte)Lerp(from.b, to.b, time)
			);
		}

		public static Colour24 operator +(Colour24 left, Colour24 right)
		{
			return new Colour24(
				(byte)(left.r + right.r),
				(byte)(left.g + right.g),
				(byte)(left.b + right.b)
			);
		}

		public static Colour24 operator -(Colour24 left, Colour24 right)
		{
			return new Colour24(
				(byte)(left.r - right.r),
				(byte)(left.g - right.g),
				(byte)(left.b - right.b)
			);
		}

		public static bool operator ==(Colour24 left, Colour24 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Colour24 left, Colour24 right)
		{
			return !(left == right);
		}
	}
}
