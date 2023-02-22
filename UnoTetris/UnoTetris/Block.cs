using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoTetris.Model;

namespace UnoTetris;
public abstract class Block
{
	protected abstract Position[][] Tiles { get; }
	protected abstract Position StartOffset { get; }
	public abstract int Id { get; }

	int rotationState;
	Position offset;

    public Block()
    {
        offset = new (StartOffset.Row, StartOffset.Column);
    }

	public IEnumerable<Position> TilePositions()
	{
		foreach (var p in Tiles[rotationState])
			yield return new(p.Row + offset.Row, p.Column + offset.Column);
	}

	public void RotateCW() =>
		rotationState = (rotationState + 1) % Tiles.Length;

	public void RotateCCW()
	{
		if (rotationState is 0)
		{
			rotationState = Tiles.Length - 1;
		}
		else
		{
			rotationState--;
		}
	}

	public void Move(int rows, int columns)
	{
		offset.Row += rows;
		offset.Column += columns;
	}

	public void Reset()
	{
		rotationState = 0;
		offset.Row = StartOffset.Row;
		offset.Column = StartOffset.Column;
	}
}