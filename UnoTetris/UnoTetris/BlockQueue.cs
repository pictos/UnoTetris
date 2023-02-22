using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoTetris.Model;

namespace UnoTetris;
sealed class BlockQueue
{
	private readonly Random random = Random.Shared;

	public Block NextBlock { get; private set; }
	public BlockQueue()
	{
		NextBlock = GetRandomBlock();
	}

	public Block GetAndUpdate()
	{
		var block = NextBlock;

		do
		{
			NextBlock = GetRandomBlock();
		}
		while (block.Id == NextBlock.Id);

		return block;
	}

	Block GetRandomBlock()
	{
		return random.Next(0, 7) switch
		{
			0 => new BlockI(),
			1 => new BlockJ(),
			2 => new BlockL(),
			3 => new BlockO(),
			4 => new BlockS(),
			5 => new BlockT(),
			6 => new BlockZ(),
			_ => throw new InvalidOperationException(),
		};
	}	
}
