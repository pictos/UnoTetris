namespace UnoTetris;

sealed class GameGrid
{
	readonly int[,] grid;

	public int Rows { get; }
	public int Columns { get; }

	public int this[int r, int c]
	{
		get => grid[r, c];
		set => grid[r, c] = value;
	}

	public GameGrid(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;
		grid = new int[rows, columns];
	}

	public bool IsInside(int r, int c) =>
		r > 0 && r < Rows && c >= 0 && c < Columns;

	public bool IsEmpty(int r, int c) =>
		IsInside(r, c) && grid[r, c] == 0;

	public bool IsRowFull(int r)
	{
		for (var c = 0; c < Columns; c++)
		{
			if (grid[r, c] == 0)
				return false;
		}

		return true;
	}

	public bool IsRowEmpty(int r)
	{
		for (int c = 0; c < Columns; c++)
		{
			if (grid[r, c] != 0)
			{
				return false;
			}
		}

		return true;
	}

	public bool IsColumnFull(int c)
	{
		for (var r = 0; r < Rows; r++)
		{
			if (grid[r, c] == 0)
				return false;
		}

		return true;
	}

	void ClearRow(int r)
	{
		for (var i = 0; i < Columns; i++)
			grid[r, i] = 0;
	}

	void MoveRowDown(int r, int numRows)
	{
		for (int c = 0; c < Columns; c++)
		{
			grid[r + numRows, c] = grid[r, c];
			grid[r, c] = 0;
		}
	}

	public int ClearFullRows()
	{
		var cleared = 0;

		for (int r = Rows - 1; r >= 0; r--)
		{
			if (IsRowFull(r))
			{
				ClearRow(r);
				cleared++;
			}
			else if (cleared > 0)
			{
				MoveRowDown(r, cleared);
			}
		}

		return cleared;
	}

	public void Deconstruct(out int rows, out int columns)
	{
		rows = Rows;
		columns = Columns;
	}
}