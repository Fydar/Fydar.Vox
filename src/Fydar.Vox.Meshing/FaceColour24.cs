using System;

namespace Fydar.Vox.Meshing
{
	public struct FaceColour24 : IEquatable<FaceColour24>
	{
		public Colour24 TopLeft;
		public Colour24 TopRight;
		public Colour24 BottomLeft;
		public Colour24 BottomRight;

		public FaceColour24(Colour24 colour)
		{
			TopLeft = colour;
			TopRight = colour;
			BottomLeft = colour;
			BottomRight = colour;
		}

		public FaceColour24(Colour24 topLeft, Colour24 topRight, Colour24 bottomLeft, Colour24 bottomRight)
		{
			TopLeft = topLeft;
			TopRight = topRight;
			BottomLeft = bottomLeft;
			BottomRight = bottomRight;
		}

		public override bool Equals(object obj)
		{
			return obj is FaceColour24 colour && Equals(colour);
		}

		public bool Equals(FaceColour24 other)
		{
			return TopLeft.Equals(other.TopLeft) &&
				   TopRight.Equals(other.TopRight) &&
				   BottomLeft.Equals(other.BottomLeft) &&
				   BottomRight.Equals(other.BottomRight);
		}

		public override int GetHashCode()
		{
			int hashCode = -505697310;
			hashCode = hashCode * -1521134295 + TopLeft.GetHashCode();
			hashCode = hashCode * -1521134295 + TopRight.GetHashCode();
			hashCode = hashCode * -1521134295 + BottomLeft.GetHashCode();
			hashCode = hashCode * -1521134295 + BottomRight.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(FaceColour24 left, FaceColour24 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FaceColour24 left, FaceColour24 right)
		{
			return !(left == right);
		}
	}
}
