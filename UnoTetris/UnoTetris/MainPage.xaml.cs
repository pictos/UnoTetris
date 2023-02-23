using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using UnoTetris.Services;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoTetris;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
	static readonly Uri baseUrl = new("ms-appx:///UnoTetris/");
	readonly ImageSource[] tileImages = new ImageSource[]
	{
		new BitmapImage(new Uri(baseUrl, "Assets/TileEmpty.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileCyan.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileBlue.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileOrange.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileYellow.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileGreen.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TilePurple.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/TileRed.png")),
	};

	readonly ImageSource[] blockImages = new ImageSource[]
	{
		new BitmapImage(new Uri(baseUrl, "Assets/Block-Empty.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-I.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-J.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-L.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-O.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-S.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-T.png")),
		new BitmapImage(new Uri(baseUrl, "Assets/Block-Z.png")),
	};

	readonly Image[,] imageControls;
	readonly int maxDelay = 1_000;
	readonly int minDelay = 75;
	readonly int delayDecrease = 25;

	GameState gameState = new();
	public MainPage()
	{
		this.InitializeComponent();
		this.MinWidth = this.MinHeight = 600;
		imageControls = SetupGameCanvas(gameState.GameGrid);


#if !__MOBILE__
		SillyDependencyService.Get<IInputService>()!.ProcessKeyDown(this, OnKeyDown);
#endif
	}

	Image[,] SetupGameCanvas(GameGrid gameGrid)
	{
		var (rows, columns) = gameGrid;
		var imgControls = new Image[rows, columns];
		var cellSize = 25;

		for (int r = 0; r < rows; r++)
		{
			for (int c = 0; c < columns; c++)
			{
				var imageControl = new Image
				{
					Width = cellSize,
					Height = cellSize
				};

				Canvas.SetTop(imageControl, (r - 2) * cellSize);
				Canvas.SetLeft(imageControl, c * cellSize);
				this.gameCanvas.Children.Add(imageControl);
				imgControls[r, c] = imageControl;
			}
		}

		return imgControls;
	}

	void DrawGhostBlock(Block block)
	{
		var dropDistance = gameState.BlockDropDistance();

		foreach (var p in block.TilePositions())
		{
			imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
			imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
		}
	}

	void DrawGrid(GameGrid grid)
	{
		var (rows, columns) = grid;
		for (int r = 0; r < rows; r++)
		{
			for (int c = 0; c < columns; c++)
			{
				var id = grid[r, c];
				imageControls[r, c].Opacity = 1;
				imageControls[r, c].Source = tileImages[id];
			}
		}
	}

	void DrawBlock(Block block)
	{
		foreach (var p in block.TilePositions())
		{
			imageControls[p.Row, p.Column].Opacity = 1;
			imageControls[p.Row, p.Column].Source = tileImages[block.Id];
		}
	}

	void DrawNextBlock(BlockQueue blockQueue)
	{
		var next = blockQueue.NextBlock;
		nextImage.Source = blockImages[next.Id];
	}

	void DrawHeldBlock(Block heldBlock)
	{
		holdImage.Source = heldBlock is null ? blockImages[0] : blockImages[heldBlock.Id];
	}

	void Draw(GameState gameState)
	{
		DrawGrid(gameState.GameGrid);
		DrawGhostBlock(gameState.CurrentBlock);
		DrawBlock(gameState.CurrentBlock);
		DrawNextBlock(gameState.BlockQueue);
		DrawHeldBlock(gameState.HeldBlock);
		scoreText.Text = $"Score: {gameState.Score}";
	}

	void OnKeyDown(VirtualKey key)
	{
		if (gameState.GameOver)
			return;


		switch (key)
		{
			case VirtualKey.Left:
				gameState.MoveBlockLeft();
				break;
			case VirtualKey.Right:
				gameState.MoveBlockRight();
				break;
			case VirtualKey.Down:
				gameState.MoveBlockDown();
				break;
			case VirtualKey.Up:
				gameState.RotateBlockCW();
				break;
			case VirtualKey.Z:
				gameState.RotateBlockCCW();
				break;
			case VirtualKey.C:
				gameState.HoldBlock();
				break;
			case VirtualKey.Space:
				gameState.DropBlock();
				break;
			default:
				return;
		}

		Draw(gameState);
	}

	async Task GameLoopAsync()
	{
		Draw(gameState);

		while (!gameState.GameOver)
		{
			var delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
			await Task.Delay(delay);
			gameState.MoveBlockDown();
			Draw(gameState);
		}

		GameOverMenu.Visibility = Visibility.Visible;
		FinalScoreText.Text = $"Score: {gameState.Score}";
	}

	async void gameCanvas_Loaded(object sender, RoutedEventArgs e)
	{
		await GameLoopAsync();
	}

	async void Button_Click(object sender, RoutedEventArgs e)
	{
		gameState = new();
		GameOverMenu.Visibility = Visibility.Collapsed;
		await GameLoopAsync();
	}
}