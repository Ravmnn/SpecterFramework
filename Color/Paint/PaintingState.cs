﻿namespace Specter.Color.Paint;


public struct PaintingState(ColorObject color)
{
	public ColorObject Color { get; set; } = color;
	public ColorObject DefaultColor { get; set; } = ColorValue.Reset;

	public TokenLexemeSet? PaintUntilToken { get; set; }
	public int PaintLength { get; set; }
	public int DefaultPaintLength { get; set; } = 1;
	public int PaintCounter { get; private set; }
	private bool _countingPaint = false;

	public readonly bool ShouldIgnoreRuleMatching =>
		PaintUntilToken is not null || _countingPaint;

	public bool IgnoreCurrentToken { get; set; }


	public void Update(Token currentToken)
	{
		if (IgnoreCurrentToken)
		{
			IgnoreCurrentToken = false;
			return;
		}

		if (PaintUntilToken is TokenLexemeSet validSet && !validSet.Match(currentToken))
			return;

		if (PaintUntilToken is not null)
			PaintLength = PaintUntilToken?.Set.Length ?? 1;

		PaintUntilToken = null;

		if (++PaintCounter < PaintLength)
		{
			_countingPaint = true;
			return;
		}

		ResetState();
	}


	private void ResetState()
	{
		_countingPaint = false;
		PaintCounter = 0;
		PaintLength = DefaultPaintLength;

		Color = DefaultColor;
	}
}