using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;

namespace Fydar.Vox.WebDemo.Services.ImagePacker
{
	public class MarchingAnchorRectanglePacker : IRectanglePacker
	{
		private class AnchorRankComparer : IComparer<Point>
		{
			public static readonly AnchorRankComparer Default = new();

			public int Compare(Point left, Point right)
			{
				return (left.X + left.Y) - (right.X + right.Y);
			}
		}

		private int actualPackingAreaHeight = 1;
		private int actualPackingAreaWidth = 1;

		private readonly List<Point> anchors;
		private readonly List<Rectangle> packedRectangles;

		public int MaxWidth { get; }
		public int MaxHeight { get; }

		public int ConsumedWidth { get; private set; }
		public int ConsumedHeight { get; private set; }

		public MarchingAnchorRectanglePacker(int maxWidth, int maxHeight)
		{
			MaxWidth = maxWidth;
			MaxHeight = maxHeight;

			anchors = new List<Point> { new Point(0, 0) };
			packedRectangles = new List<Rectangle>();
		}

		public bool TryPack(int width, int height, out Point placement)
		{
			width++;
			height++;

			int anchorIndex = SelectAnchorRecursive(width, height, actualPackingAreaWidth, actualPackingAreaHeight);

			if (anchorIndex == -1)
			{
				placement = new Point();
				return false;
			}

			placement = anchors[anchorIndex];

			OptimizePlacement(ref placement, width, height);

			bool blocksAnchor =
				((placement.X + width) > anchors[anchorIndex].X) &&
				((placement.Y + height) > anchors[anchorIndex].Y);

			if (blocksAnchor)
			{
				anchors.RemoveAt(anchorIndex);
			}

			InsertAnchor(new Point(placement.X + width, placement.Y));
			InsertAnchor(new Point(placement.X, placement.Y + height));

			var rectangle = new Rectangle(placement.X, placement.Y, width, height);
			packedRectangles.Add(rectangle);

			ConsumedWidth = Math.Max(ConsumedWidth, rectangle.Right);
			ConsumedHeight = Math.Max(ConsumedHeight, rectangle.Bottom);

			return true;
		}

		private void OptimizePlacement(ref Point placement, int rectangleWidth, int rectangleHeight)
		{
			var rectangle = new Rectangle(placement.X, placement.Y, rectangleWidth, rectangleHeight);

			int leftMost = placement.X;
			while (IsFree(ref rectangle, MaxWidth, MaxHeight))
			{
				leftMost = rectangle.X;
				--rectangle.X;
			}

			rectangle.X = placement.X;

			int topMost = placement.Y;
			while (IsFree(ref rectangle, MaxWidth, MaxHeight))
			{
				topMost = rectangle.Y;
				--rectangle.Y;
			}

			if (placement.X - leftMost > placement.Y - topMost)
			{
				placement.X = leftMost;
			}
			else
			{
				placement.Y = topMost;
			}
		}

		private int SelectAnchorRecursive(int width, int height, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			int freeAnchorIndex = FindFirstFreeAnchor(width, height, testedPackingAreaWidth, testedPackingAreaHeight);

			if (freeAnchorIndex != -1)
			{
				actualPackingAreaWidth = testedPackingAreaWidth;
				actualPackingAreaHeight = testedPackingAreaHeight;

				return freeAnchorIndex;
			}

			bool canEnlargeWidth = testedPackingAreaWidth < MaxWidth;
			bool canEnlargeHeight = testedPackingAreaHeight < MaxHeight;
			bool shouldEnlargeHeight = (!canEnlargeWidth) || (testedPackingAreaHeight < testedPackingAreaWidth);

			if (canEnlargeHeight && shouldEnlargeHeight)
			{
				return SelectAnchorRecursive(width, height, testedPackingAreaWidth, Math.Min(testedPackingAreaHeight * 2, MaxHeight));
			}

			if (canEnlargeWidth)
			{
				return SelectAnchorRecursive(width, height, Math.Min(testedPackingAreaWidth * 2, MaxWidth), testedPackingAreaHeight);
			}

			return -1;
		}

		private int FindFirstFreeAnchor(int width, int height, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			var potentialLocation = new Rectangle(0, 0, width, height);

			for (int index = 0; index < anchors.Count; ++index)
			{
				potentialLocation.X = anchors[index].X;
				potentialLocation.Y = anchors[index].Y;

				if (IsFree(ref potentialLocation, testedPackingAreaWidth, testedPackingAreaHeight))
				{
					return index;
				}
			}

			return -1;
		}

		private bool IsFree(ref Rectangle rectangle, int testedPackingAreaWidth, int testedPackingAreaHeight)
		{
			var packingArea = new Rectangle(0, 0, testedPackingAreaWidth, testedPackingAreaHeight);

			bool leavesPackingArea = !packingArea.Contains(rectangle);

			if (leavesPackingArea)
			{
				return false;
			}

			for (int i = 0; i < packedRectangles.Count; i++)
			{
				if (packedRectangles[i].IntersectsWith(rectangle))
				{
					return false;
				}
			}
			return true;
		}

		private void InsertAnchor(Point anchor)
		{
			int insertIndex = anchors.BinarySearch(anchor, AnchorRankComparer.Default);

			if (insertIndex < 0)
			{
				insertIndex = ~insertIndex;
			}

			anchors.Insert(insertIndex, anchor);
		}
	}
}
