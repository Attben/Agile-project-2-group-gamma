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
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        // Track all open subwindows
        public static Dictionary<UIContext, AppWindow> AppWindows = new Dictionary<UIContext, AppWindow>();

        // Event for invoking minigames
        public delegate void MiniGameDelegate(Player invokingPlayer);
        public static event MiniGameDelegate MiniGameEvent;

        public static CanvasBitmap BackGround;
        public int NumberOfPlayers;
        private Dice _dice;

        // Stores the current pointer position
        public double PointerX;
        public double PointerY;

        // Variables used for checking where on the board the user is clicking
        public static bool UserClickedBoard;
        public static Vector2? CurrentTileVector;
        public static Vector2? ClickedTileVector;


        private Game _game;

        public GameBoard()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentSizeChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Scaling.ScalingInit();
            ControlProperties(Scaling.bWidth, Scaling.bHeight);
            MiniGameEvent += StartMiniGame;
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
            await LoadTurnGraphics(sender);
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
        /// Loads turn graphics.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        async Task LoadTurnGraphics(CanvasAnimatedControl sender)
        {
            Player.RedTurn = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RedTurn.png"));
            Player.BlueTurn = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/BlueTurn.png"));
            Player.GreenTurn = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/GreenTurn.png"));
            Player.YellowTurn = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/YellowTurn.png"));
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
            AnimationHandler.UpdateEffectOpacity();
            drawArgs.DrawingSession.DrawImage(Scaling.TransformImage(BackGround));
            _dice.Draw(drawArgs, _game.CurrentPlayerTurn);
            
            _game.DrawMainContent(drawArgs);
            if (CurrentTileVector.HasValue)
            {
                drawArgs.DrawingSession.DrawText(($"CurrentTileVector X: {CurrentTileVector.Value.X}, Y: {CurrentTileVector.Value.Y}"), 300, 100, Windows.UI.Colors.Black);

            }
            if (ClickedTileVector.HasValue)
            {
                drawArgs.DrawingSession.DrawText(($"ClickedTileVector X: {ClickedTileVector.Value.X}, Y: {ClickedTileVector.Value.Y}"), 300, 150, Windows.UI.Colors.Black);

            }
        }



        /// <summary>
        /// Checks where the user clicks on the canvas. If a board tile is clicked, it stores it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //Currently unused. Including a few commented-out lines
            //as an example of what this method might be used for later.
            //var action = Canvas.RunOnGameLoopThreadAsync(() =>
            //{

            //});
            if (ClickedTileVector != null)
            {
                if (_game._players[_game.CurrentPlayerTurn].ChosenPiece != null)
                {
                    if (_game._players[_game.CurrentPlayerTurn].ChosenPiece.AllowedDestinationTileVector != null)
                    {
                        if (CurrentTileVector == _game._players[_game.CurrentPlayerTurn].ChosenPiece.AllowedDestinationTileVector.Value)
                        {
                            // Fire move function here
                            //_game.TakeTurn(_game._players[_game.CurrentPlayerTurn]);
                            _game.TakeTurn(_game._players[_game.CurrentPlayerTurn]);
                        }
                    }
                }
            }
            ClickedTileVector = CurrentTileVector;
            
        }

        private void CanvasUpdate(
            ICanvasAnimatedControl sender,
            CanvasAnimatedUpdateEventArgs args)
        {
            //Currently unused.
        }

        /// <summary>
        /// Event for clicking the die to roll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollDie(object sender, RoutedEventArgs e)
        {
            if (!Game.CurrentDiceRoll.HasValue)
            {
                Game.CurrentDiceRoll = _dice.Roll() + 1;
                if (!_game._players[_game.CurrentPlayerTurn].CheckPossibilityToMove(Game.CurrentDiceRoll.Value))
                {
                    Game.CurrentDiceRoll = null;
                    _game.NextPlayerTurn();
                }
            }
            CheckEndGame();
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

        private void OpenMinigame(object sender, RoutedEventArgs e)
        {
            MiniGameEvent.Invoke(_game._players[0]);
        }

        public static void InvokeMiniGameEvent(Player player)
        {
            MiniGameEvent.Invoke(player);
        }

        public async void StartMiniGame(Player invokingPlayer)
        {
            MiniGameNavigationParams navParams = new MiniGameNavigationParams
            {
                InvokingPlayer = invokingPlayer,
                OtherPlayers = _game._players.Where(player => player != invokingPlayer).ToList()
            };
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame appWindowContentFrame = new Frame();

            appWindowContentFrame.Navigate(typeof(MiniGamePage), navParams);
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            AppWindows.Add(appWindowContentFrame.UIContext, appWindow);

            // This is for removing the AppWindow from the tracked windows
            appWindow.Closed += delegate
            {
                RollButton.IsEnabled = true;
                AppWindows.Remove(appWindowContentFrame.UIContext);
                appWindowContentFrame.Content = null;
                appWindow = null;
            };

            RollButton.IsEnabled = false;
            await appWindow.TryShowAsync();
        }

        private void CheckEndGame()
        {
            if (_game.piecesInGoal == 0)
            {
                this.Frame.Navigate(typeof(gameover), _game._players);
            }
        }
        
        /// <summary>
        /// Used to update the position of the cursor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckCursorPosition(object sender, PointerRoutedEventArgs e)
        {
            PointerX = e.GetCurrentPoint(Canvas).Position.X;
            PointerY = e.GetCurrentPoint(Canvas).Position.Y;
            // Check if pointer is inside the mainboard
            if (e.GetCurrentPoint(Canvas).Position.X > _game.Board.MainBoard.X && e.GetCurrentPoint(Canvas).Position.X < _game.Board.MainBoard.X + _game.Board.MainBoard.Width)
            {
                if (e.GetCurrentPoint(Canvas).Position.Y > _game.Board.MainBoard.Y && e.GetCurrentPoint(Canvas).Position.Y < _game.Board.MainBoard.Y + _game.Board.MainBoard.Height)
                {
                    CalculateCurrentTileVector();
                    UserClickedBoard = true;
                }
                else
                {
                    UserClickedBoard = false;
                    CurrentTileVector = null;
                }
            }
            else
            {
                UserClickedBoard = false;
                CurrentTileVector = null;
            }
        }

        /// <summary>
        /// Calculates what grid the cursor is on when hovering over the main board.
        /// </summary>
        private void CalculateCurrentTileVector()
        {
            CurrentTileVector = new Vector2((float)Math.Floor((PointerX - _game.Board.MainBoard.X) / (_game.Board.MainBoard.Width / 11)), (float)Math.Floor((PointerY - _game.Board.MainBoard.Y) / (_game.Board.MainBoard.Height / 11)));
        }
    }
}
