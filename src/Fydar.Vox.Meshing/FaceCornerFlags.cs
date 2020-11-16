using System;

namespace Fydar.Vox.Meshing
{
	public struct FaceCornerFlags : IEquatable<FaceCornerFlags>
	{
		public static FaceCornerFlags None => new FaceCornerFlags();

		private static readonly string[] iconography = new string[]
		{
			" ",
			"▘",
			"▝",
			"▀",
			"▖",
			"▍",
			"▞",
			"▛",
			"▗",
			"▚",
			"▐",
			"▜",
			"▃",
			"▙",
			"▟",
			"▉"
		};

		private byte data;

		public bool TopLeft
		{
			get => GetBit(ref data, 0);
			set => SetBit(ref data, 0, value);
		}

		public bool TopRight
		{
			get => GetBit(ref data, 1);
			set => SetBit(ref data, 1, value);
		}

		public bool BottomLeft
		{
			get => GetBit(ref data, 2);
			set => SetBit(ref data, 2, value);
		}

		public bool BottomRight
		{
			get => GetBit(ref data, 3);
			set => SetBit(ref data, 3, value);
		}

		public FaceEdgeFlags TouchedEdges => new FaceEdgeFlags()
		{
			Top = TopLeft || TopRight,
			Left = TopLeft || BottomLeft,
			Right = TopRight || BottomRight,
			Bottom = BottomLeft || BottomRight
		};

		public FaceEdgeFlags ConnectedEdges => new FaceEdgeFlags()
		{
			Top = TopLeft && TopRight,
			Left = TopLeft && BottomLeft,
			Right = TopRight && BottomRight,
			Bottom = BottomLeft && BottomRight
		};

		public override string ToString()
		{
			return iconography[data];
		}

		public override bool Equals(object obj)
		{
			return obj is FaceCornerFlags flags && Equals(flags);
		}

		public bool Equals(FaceCornerFlags other)
		{
			return data == other.data;
		}

		public override int GetHashCode()
		{
			return 1768953197 + data.GetHashCode();
		}

		public static FaceCornerFlags operator |(FaceCornerFlags lhs, FaceCornerFlags rhs)
		{
			return new FaceCornerFlags()
			{
				data = (byte)(lhs.data | rhs.data)
			};
		}

		public static FaceCornerFlags operator &(FaceCornerFlags lhs, FaceCornerFlags rhs)
		{
			return new FaceCornerFlags()
			{
				data = (byte)(lhs.data & rhs.data)
			};
		}

		public static bool operator ==(FaceCornerFlags left, FaceCornerFlags right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FaceCornerFlags left, FaceCornerFlags right)
		{
			return !(left == right);
		}

		public static FaceCornerFlags operator !(FaceCornerFlags original)
		{
			return new FaceCornerFlags()
			{
				data = original.data ^= 0b_1111
			};
		}

		private static bool GetBit(ref byte source, byte index)
		{
			return (source & (1 << index)) != 0;
		}

		private static void SetBit(ref byte data, byte index, bool value)
		{
			data = (byte)(value
				? data | 1 << index
				: data & ~(1 << index));
		}
	}
}
