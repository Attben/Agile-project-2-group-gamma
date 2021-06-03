using LudoLike.Classes;
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
using Windows.Media.Core;
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

namespace LudoLike
{
    /// <summary>
    /// The main page where a Game is displayed.
    /// </summary>
    public sealed partial class GameBoard : Page
    {
        public int NumberOfPlayers;
        public static CanvasBitmap BackGround;
        private Dice _dice;
        private GameStateManager _gameStateManager = new GameStateManager();

        private Game _game;

        public GameBoard()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentSizeChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Scaling.ScalingInit();
            ControlProperties(Scaling.bWidth, Scaling.bHeight);
        }

        /// <summary>
        /// Creates a game instance and saves slider value of players from play menu.
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Sets the hight and width of the canvas.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ControlProperties(double width, double height)
        {
            Canvas.Width = width;
            Canvas.Height = height;
        }

        /// <summary>
        /// Loads all neccessary images that will be used in the game.
        /// <para></para>
        /// Also adds players, static and dynamic tiles to the board which must be added after loading images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            BackGround = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));

            await LoadDiceImages(sender);
            await LoadPlayerPieceImages(sender);
            await LoadGlowEffects(sender);
            await LoadTileImages(sender);
            LoadSounds();
            
            _dice = new Dice(0, 6);
            _game.AddPlayers(NumberOfPlayers);
            _game.CreateStaticTiles();
            _game.CreateDynamicTiles();


            SoundMixer.PlaySound(Game.BackgroundMusic, SoundChannels.music);
        }

        /// <summary>
        /// Loads dice images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task LoadDiceImages(CanvasAnimatedControl sender)
        {
            for (int n = 0; n < 6; ++n)
            {
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n + 1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
        }
        /// <summary>
        /// Loads Player piece images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task LoadPlayerPieceImages(CanvasAnimatedControl sender) 
        {
            Piece.Red = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RedPiece.png"));
            Piece.Blue = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/BluePiece.png"));
            Piece.Yellow = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/YellowPiece.png"));
            Piece.Green = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/GreenPiece.png"));
        }

        /// <summary>
        /// Loads glow effects for the die.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task LoadGlowEffects(CanvasAnimatedControl sender)
        {
            Dice.GlowEffects.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/glowred.png")));
            Dice.GlowEffects.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/glowblue.png")));
            Dice.GlowEffects.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/glowyellow.png")));
            Dice.GlowEffects.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/glowgreen.png")));
        }

        /// <summary>
        /// Loads music and sound effects
        /// </summary>
        private void LoadSounds()
        {
            Game.BackgroundMusic = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/bass jam.mp3"));
            Player.PieceCollisionSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Slap1.wav")));
            Player.PieceCollisionSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Slap2.wav")));
            Player.PieceMovingSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/WoodenTap1.wav")));
            Player.PieceMovingSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/WoodenTap2.wav")));
            Player.PieceMovingSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/WoodenTap3.wav")));
            Tile.TileEventSounds.Add("Tile", MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PlasticTap1.wav")));
            Tile.TileEventSounds.Add("StaticTile", MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PlasticTap2.wav")));
            Tile.TileEventSounds.Add("MinigameTile", MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Challenge.wav")));
            Tile.TileEventSounds.Add("ScoreTile", MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/MoneyShake.wav")));
            Tile.TileEventSounds.Add("TeleportTile", MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Teleport.wav")));
        }

        /// <summary>
        /// Loads tile images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task LoadTileImages(CanvasAnimatedControl sender)
        {
            Tile.TileImages["Red"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/redtile.png"));
            Tile.TileImages["Blue"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/bluetile.png"));
            Tile.TileImages["Yellow"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/yellowtile.png"));
            Tile.TileImages["Green"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/greentile.png"));
            Tile.TileImages["Middle"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/middletile.png"));
            Tile.TileImages["Regular"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/regulartile.png"));
            Tile.TileImages["Teleport"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/teleporttile.png"));
            Tile.TileImages["MiniGame"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/gametile.png"));
            Tile.TileImages["Score"] = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/peng.png"));
        }

        /// <summary>
        /// Sets the scaling variables according to the new window size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Scaling.ScalingInit(e.Size.Width, e.Size.Height);
            ControlProperties(e.Size.Width, e.Size.Height);
            UpdateButtonSizeAndPosition();
        }

        private void UpdateButtonSizeAndPosition()
        {
            
        }

        /// <summary>
        /// Updates and draws what is to be displayed on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="drawArgs"></param>
        private void CanvasDraw(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(Scaling.TransformImage(BackGround));
            _dice.Draw(drawArgs, _game.CurrentPlayerTurn);
            _game.DrawMainContent(drawArgs);

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

        /// <summary>
        /// Event for clicking the die to roll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollDie(object sender, RoutedEventArgs e)
        {
            _game.TakeTurn(_dice.Roll() + 1);
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// Changes cursor to hand when hovering the die.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePointerHand(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        /// <summary>
        /// Changes cursor to pointer when un-hovering the die.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePointerRegular(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

    }
}
