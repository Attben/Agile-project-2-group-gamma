using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoLike
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int NumberOfPlayers;

        public static CanvasBitmap BG;
        public List<Tile> Tiles;

        //public LudoBoard Board;

        private Dice _dice;
        //private Random _prng = new Random();
        private GameStateManager _gameStateManager = new GameStateManager();

        private Game _game;
        //private Piece _piece;
        

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentSizeChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Scaling.ScalingInit();
            

            ControlProperties(Scaling.bWidth, Scaling.bHeight);
            //_gameStateManager.GameStateInit();
            //Canvas.IsFixedTimeStep = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NumberOfPlayers = int.Parse(e.Parameter.ToString());
            _game = new Game();
        }
        private void CanvasCreateResources(
            CanvasAnimatedControl sender,
            CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        public void ControlProperties(double width, double height)
        {
            Canvas.Width = width;
            Canvas.Height = height;
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            

            //Load background image
            BG = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));

            //Load static images belonging to the Dice class
            for (int n = 0; n < 6; ++n)
            {
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n + 1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
            _dice = new Dice(1, 6);

            Piece.Red = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RedPiece.png"));
            Piece.Blue = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/BluePiece.png"));
            Piece.Yellow = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/YellowPiece.png"));
            Piece.Green = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/GreenPiece.png"));
            _game.AddPlayers(NumberOfPlayers);
            //_piece = new Piece(new Vector2 (0, 5), 0);

            await LoadTileImages(sender);
        }

        async Task LoadTileImages(CanvasAnimatedControl sender)
        {
            Tile.TileImages["Red"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/redtile.png"));
            Tile.TileImages["Blue"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/bluetile.png"));
            Tile.TileImages["Yellow"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/yellowtile.png"));
            Tile.TileImages["Green"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/greentile.png"));
            Tile.TileImages["Middle"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/middletile.png"));
            Tile.TileImages["Regular"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/regulartile.png"));
        }

        private void CurrentSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Scaling.ScalingInit(e.Size.Width, e.Size.Height);
            ControlProperties(e.Size.Width, e.Size.Height);
        }

        private void CanvasDraw(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(Scaling.TransformImage(BG));

            _game._board.Draw(drawArgs);
            drawArgs.DrawingSession.DrawText($"bWidth: {Scaling.bWidth}, bHeight{Scaling.bHeight}", 0, 0, Windows.UI.Colors.Black);
            drawArgs.DrawingSession.DrawText($"Board X: {_game._board.MainBoard.X}, Board Y{_game._board.MainBoard.Y}", 50, 50, Windows.UI.Colors.Black);
            _dice.Draw(drawArgs);
            //_piece.Draw(drawArgs);

            _game.Tiles = new List<Tile>();
            _game.CreateStaticTiles();
            _game.CreateDynamicTiles();

            // Test drawing pengs
            foreach (Tile tile in _game.Tiles)
            {
                tile.Draw(drawArgs);
            }

            foreach (Player player in _game._players)
            {
                foreach (Piece piece in player.pieces)
                {
                    piece.Draw(drawArgs, _game._board.TileGrid[piece.position]);
                }
            }
            //foreach (KeyValuePair<Vector2, Rect> tileHolder in Board.TileGrid)
            //{
            //    drawArgs.DrawingSession.DrawText($"({tileHolder.Key.X}, {tileHolder.Key.Y})", new Vector2((float)tileHolder.Value.X, (float)tileHolder.Value.Y), Windows.UI.Colors.Black);
            //}
            //_gameStateManager.Draw(args);
        }

        private void CanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(Canvas).Position;
            if (_gameStateManager.CurrentGameState == GameState.NewGame)
            {
                var action = Canvas.RunOnGameLoopThreadAsync(() =>
               {
                   _gameStateManager.CurrentGameState = GameState.Playing;
               });
            }
            if (_gameStateManager.CurrentGameState == GameState.Playing)
            {
                var action = Canvas.RunOnGameLoopThreadAsync(() =>
                {
                    _gameStateManager.CurrentGameState = GameState.GameOver;
                });
            }
        }

        private void CanvasUpdate(
            ICanvasAnimatedControl sender,
            CanvasAnimatedUpdateEventArgs args)
        {
            _gameStateManager.Update();
        }
        private void RollDie(object sender, RoutedEventArgs e)
        {
            _dice.Roll();
        }

        private void TestMovePiece(object sender, RoutedEventArgs e)
        {
            // Detta måste anpassas till den nya tilegridlogiken
            //_piece.Move(100f, 100f);
            _game._players[0].pieces[0].Move(_game._board.RedPath[2]);
        }
        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
