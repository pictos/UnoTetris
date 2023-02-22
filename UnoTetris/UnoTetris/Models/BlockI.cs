namespace UnoTetris.Model;
public sealed class BlockI : Block
{
	public override int Id => 1;

	protected override Position[][] Tiles => new Position[][]
	{
		new Position[] {new (1,0), new (1,1), new (1,2), new (1,3)},
		new Position[] {new (0,2), new (1,2), new (2,2), new (3,2)},
		new Position[] {new (2,0), new (2,1), new (2,2), new (2,3)},
		new Position[] {new (0,1), new (1,1), new (2,1), new (3,1)},
	};

	protected override Position StartOffset => new(-1, 3);
}
