using System;
using UnoTetris.Model;

namespace UnoTetris;
sealed class GameState
{
	Block currentBlock;

	public Block CurrentBlock
	{
		get => currentBlock;
		private set
		{
			currentBlock = value;
			currentBlock.Reset();

			for (var i = 0; i < 2; i++)
			{
				currentBlock.Move(1, 0);
				if (!BlockFits())
				{
					currentBlock.Move(-1, 0);
				}
			}
		}
	}

	public GameGrid GameGrid { get; } = new(22, 10);

	public BlockQueue BlockQueue { get; } = new();

	public bool GameOver { get; private set; }

	public int Score { get; private set; }

	public Block? HeldBlock { get; private set; }

	public bool CanHold { get; private set; } = true;

	public GameState()
	{
		currentBlock = BlockQueue.GetAndUpdate();
	}

	public void HoldBlock()
	{
		if (!CanHold)
			return;

		if (HeldBlock is null) 
		{
			HeldBlock = CurrentBlock;
			CurrentBlock = BlockQueue.GetAndUpdate();
		}
		else
		{
			(CurrentBlock, HeldBlock) = (HeldBlock, CurrentBlock);
		}

		CanHold = false;
	}

	bool BlockFits()
	{
		foreach (var p in CurrentBlock.TilePositions())
		{
			if (!GameGrid.IsEmpty(p.Row, p.Column))
				return false;
		}

		return true;
	}

	public void RotateBlockCW()
	{
		CurrentBlock.RotateCW();

		if (!BlockFits())
		{
			CurrentBlock.RotateCCW();
		}
	}

	public void RotateBlockCCW()
	{
		CurrentBlock.RotateCCW();

		if (!BlockFits())
		{
			CurrentBlock.RotateCW();
		}
	}

	public void MoveBlockLeft()
	{
		CurrentBlock.Move(0, -1);
		if (!BlockFits())
		{
			CurrentBlock.Move(0, 1);
		}
	}

	public void MoveBlockRight()
	{
		CurrentBlock.Move(0, 1);
		if (!BlockFits())
		{
			CurrentBlock.Move(0, -1);
		}
	}

	bool IsGameOver()
	{
		return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
	}

	void PlaceBlock()
	{
		foreach (var p in CurrentBlock.TilePositions())
		{
			GameGrid[p.Row, p.Column] = CurrentBlock.Id;
		}

		Score += GameGrid.ClearFullRows();

		if (IsGameOver())
		{
			GameOver = true;
		}
		else
		{
			CurrentBlock = BlockQueue.GetAndUpdate();
			CanHold = true;
		}
	}

	public void MoveBlockDown()
	{
		CurrentBlock.Move(1, 0);

		if (!BlockFits())
		{
			CurrentBlock.Move(-1, 0);
			PlaceBlock();
		}
	}

	int TileDropDistance(Position p)
	{
		var drop = 0;

		while(GameGrid.IsEmpty(p.Row + drop +1, p.Column))
		{
			drop++;
		}

		return drop;
	}

	public int BlockDropDistance()
	{
		var drop = GameGrid.Rows;

		foreach (var p in CurrentBlock.TilePositions())
		{
			drop = Math.Min(drop,TileDropDistance(p));
		}

		return drop;
	}

	public void DropBlock()
	{
		CurrentBlock.Move(BlockDropDistance(), 0);
		PlaceBlock();
	}
}
