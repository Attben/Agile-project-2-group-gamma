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

        public static CanvasBitmap BG;
        public List<CanvasBitmap> Tiles = new List<CanvasBitmap>();

        public LudoBoard Board;


        private Dice _dice;
        private Random _prng = new Random();
        private GameStateManager _gameStateManager = new GameStateManager();

        private Game _game;
        private Piece _piece;


        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentSizeChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Scaling.ScalingInit();
            Board = new LudoBoard();
            ControlProperties(Scaling.bWidth, Scaling.bHeight);
            //_gameStateManager.GameStateInit();
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
            //Board = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Spelplan.png"));

            //Load static images belonging to the Dice class
            for (int n = 0; n < 6; ++n)
            {
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n + 1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
            _dice = new Dice(1, 6);

            // Create Tiles
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Tiles.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/peng.png")));
                }
            }

            Piece.Red = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RedPiece.png"));
            Piece.Blue = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/BluePiece.png"));
            Piece.Yellow = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/YellowPiece.png"));
            Piece.Green = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/GreenPiece.png"));
            _piece = new Piece(new Tile(), 0);
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

            Board.Draw(drawArgs);
            drawArgs.DrawingSession.DrawText($"bWidth: {Scaling.bWidth}, bHeight{Scaling.bHeight}", 0, 0, Windows.UI.Colors.Black);
            _dice.Draw(drawArgs);
            _piece.Draw(drawArgs);

            //args.DrawingSession.DrawImage(Scaling.TransformImage(BG));
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
    }
}
