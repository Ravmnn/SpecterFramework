using System;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


[Flags]
public enum Alignment
{
	None = 0,

	CenterHorizontal = 0b_0000_0001,
	Top = 0b_0000_0010,
	Bottom = 0b_0000_0100,

	CenterVertical = 0b_0000_1000,
	Left = 0b_0001_0000,
	Right = 0b_0010_0000,

	Center = CenterHorizontal | CenterVertical,

	TopLeft = Top | Left,
	TopRight = Top | Right,
	BottomLeft = Bottom | Left,
	BottomRight = Bottom | Right
}


public static class AlignmentExtensions
{
	private static uint CalculateCentralizedValue(uint parent, uint child)
	{
		double diff = parent - child;
		double position = diff / 2.0;
		
		bool hasRemainder = diff % 2 != 0;

		return (uint)(hasRemainder ? position + 1 : position);
	}


	public static Point CalculatePosition(this Alignment alignment, Rect parent, Rect rect)
	{
		Point finalPosition = rect.position;
		Size finalSize = rect.size;

		bool hasCenterH = alignment.HasFlag(Alignment.CenterHorizontal);
		bool hasCenterV = alignment.HasFlag(Alignment.CenterVertical);

		if (hasCenterH)
			finalPosition.col = CalculateCentralizedValue(parent.size.width, rect.size.width);

		if (hasCenterV)
			finalPosition.row = CalculateCentralizedValue(parent.size.height, rect.size.height);


		if (!hasCenterV)
		{
			if (alignment.HasFlag(Alignment.Top))
				finalPosition.row = 0;

			else if (alignment.HasFlag(Alignment.Bottom))
				finalPosition.row = parent.size.height - finalSize.height + 1;
		}
			

		if (!hasCenterH)
		{
			if (alignment.HasFlag(Alignment.Left))
				finalPosition.col = 0;
	
			else if (alignment.HasFlag(Alignment.Right))
				finalPosition.col = parent.size.width - finalSize.width + 1;
		}


		return finalPosition;
	}


	public static Point CalculatePosition(this Alignment alignment, Component parent, Component component)
		=> CalculatePosition(alignment, parent.Rect, component.Rect);
	


	public static Point CalculatePosition(this Alignment alignment, Component component)
	{
		if (component.Parent is null)
			return Point.None;

		return CalculatePosition(alignment, component.Parent.Rect, component.Rect);
	}
}