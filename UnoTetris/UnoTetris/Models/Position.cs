namespace UnoTetris.Model;
public sealed class Position
{
    public int Row { get; set; }
    public int Column { get; set; }

    public Position(int row, int column)
    {
		Row = row;
		Column = column;
	}
}
