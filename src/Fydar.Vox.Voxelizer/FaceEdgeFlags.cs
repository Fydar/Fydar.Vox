using System;

namespace Fydar.Vox.Voxelizer
{
	public struct FaceEdgeFlags : IEquatable<FaceEdgeFlags>
	{
		public static FaceEdgeFlags None
		{
			get
			{
				return new FaceEdgeFlags();
			}
		}

		private static readonly string[] iconography = new string[]
		{
			" ",
			"╵",
			"╶",
			"└",
			"╷",
			"│",
			"┌",
			"├",
			"╴",
			"┘",
			"─",
			"┴",
			"┐",
			"┤",
			"┬",
			"┼"
		};

		private byte data;

		public bool Top
		{
			get => GetBit(ref data, 0);
			set => SetBit(ref data, 0, value);
		}

		public bool Right
		{
			get => GetBit(ref data, 1);
			set => SetBit(ref data, 1, value);
		}

		public bool Bottom
		{
			get => GetBit(ref data, 2);
			set => SetBit(ref data, 2, value);
		}

		public bool Left
		{
			get => GetBit(ref data, 3);
			set => SetBit(ref data, 3, value);
		}

		public FaceCornerFlags Corners
		{
			get
			{
				return new FaceCornerFlags()
				{
					TopLeft = Top || Left,
					TopRight = Top || Right,
					BottomLeft = Bottom || Left,
					BottomRight = Bottom || Right,
				};
			}
		}

		public FaceCornerFlags OverlappingCorners
		{
			get
			{
				return new FaceCornerFlags()
				{
					TopLeft = Top && Left,
					TopRight = Top && Right,
					BottomLeft = Bottom && Left,
					BottomRight = Bottom && Right,
				};
			}
		}

		public override string ToString()
		{
			return iconography[data];
		}

		public override bool Equals(object obj)
		{
			return obj is FaceEdgeFlags flags && Equals(flags);
		}

		public bool Equals(FaceEdgeFlags other)
		{
			return data == other.data;
		}

		public override int GetHashCode()
		{
			return 1768953197 + data.GetHashCode();
		}

		public static FaceEdgeFlags operator |(FaceEdgeFlags lhs, FaceEdgeFlags rhs)
		{
			return new FaceEdgeFlags()
			{
				data = (byte)(lhs.data | rhs.data)
			};
		}

		public static FaceEdgeFlags operator &(FaceEdgeFlags lhs, FaceEdgeFlags rhs)
		{
			return new FaceEdgeFlags()
			{
				data = (byte)(lhs.data & rhs.data)
			};
		}

		public static bool operator ==(FaceEdgeFlags left, FaceEdgeFlags right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FaceEdgeFlags left, FaceEdgeFlags right)
		{
			return !(left == right);
		}

		public static FaceEdgeFlags operator !(FaceEdgeFlags original)
		{
			return new FaceEdgeFlags()
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
