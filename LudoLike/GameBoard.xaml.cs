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
        private int _numberOfPlayers;
        private int _piecesPerPlayer;
        private Dice _dice;

        // Stores the current pointer position
        public double PointerX;
        public double PointerY;

        // Variables used for checking where on the board the user is clicking
        public static bool UserClickedBoard;
        public static Vector2? CurrentTileVector;
        public static Vector2? ClickedTileVector;

        public static bool OverDice = false;

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
        /// Frees all allocated game assets.
        /// Currently mostly used to prevent CreateResourcesAsync() from crashing
        /// when a new game is launched after one has already finished.
        /// </summary>
        public static void DisposeGameAssets()
        {
            BackGround?.Dispose();
            for (int n = 0; n < Dice.DiceImages.Length; ++n)
            {
                Dice.DiceImages[n]?.Dispose();
            }
            Piece.Red?.Dispose();
            Piece.Blue?.Dispose();
            Piece.Yellow?.Dispose();
            Piece.Green?.Dispose();

            foreach (CanvasBitmap image in Dice.GlowEffects)
            {
                image?.Dispose();
            }
            Dice.GlowEffects.Clear();
            Game.BackgroundMusic?.Dispose();
            foreach (MediaSource sound in Player.PieceCollisionSounds)
            {
                sound?.Dispose();
            }
            Player.PieceCollisionSounds.Clear();
            foreach (MediaSource sound in Player.PieceMovingSounds)
            {
                sound?.Dispose();
            }
            Player.PieceMovingSounds.Clear();

            foreach (MediaSource sound in Tile.TileEventSounds.Values)
            {
                sound?.Dispose();
            }
            Tile.TileEventSounds.Clear();
            foreach (CanvasBitmap image in Tile.TileImages.Values)
            {
                image?.Dispose();
            }
            Tile.TileImages.Clear();

            Player.RedTurn?.Dispose();
            Player.BlueTurn?.Dispose();
            Player.GreenTurn?.Dispose();
            Player.YellowTurn?.Dispose();
        }

        /// <summary>
        /// Creates a game instance and saves slider value of players from play menu.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int[] gameParams = (int[])e.Parameter;
            _numberOfPlayers = gameParams[0];
            _piecesPerPlayer = gameParams[1];
            _game = new Game();
        }

        /// <summary>
        /// Calls for creation of necessary resources to display on the canvas e.g CanvasBitmaps etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CanvasCreateResources(
            CanvasAnimatedControl sender,
            CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        /// <summary>
        /// Sets the height and width of the canvas.
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
        private async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            BackGround = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));

            await LoadDiceImages(sender);
            await LoadPlayerPieceImages(sender);
            await LoadGlowEffects(sender);
            await LoadTileImages(sender);
            await LoadTurnGraphics(sender);
            LoadSounds();
            _dice = new Dice(0, 6);
            _game.AddPlayers(_numberOfPlayers, _piecesPerPlayer);
            _game.CreateStaticTiles();
            _game.CreateDynamicTiles();

            SoundMixer.PlaySound(Game.BackgroundMusic, SoundChannels.music);
        }

        /// <summary>
        /// Loads dice images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private async Task LoadDiceImages(CanvasAnimatedControl sender)
        {
            for (int n = 0; n < 6; ++n)
            {
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n + 1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
            Dice.StandardDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Diestandard.png"));
        }
        /// <summary>
        /// Loads Player piece images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private async Task LoadPlayerPieceImages(CanvasAnimatedControl sender)
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
        private async Task LoadGlowEffects(CanvasAnimatedControl sender)
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
            Tile.TileEventSounds["Tile"] = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PlasticTap1.wav"));
            Tile.TileEventSounds["StaticTile"] = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PlasticTap2.wav"));
            Tile.TileEventSounds["MinigameTile"] = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Challenge.wav"));
            Tile.TileEventSounds["ScoreTile"] = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/MoneyShake.wav"));
            Tile.TileEventSounds["TeleportTile"] = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Teleport.wav"));
        }

        /// <summary>
        /// Loads tile images.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private async Task LoadTileImages(CanvasAnimatedControl sender)
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
        private async Task LoadTurnGraphics(CanvasAnimatedControl sender)
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
            AnimationHandler.UpdateGlowHolderAddedSize();
            drawArgs.DrawingSession.DrawImage(Scaling.TransformImage(BackGround));
            _dice.Draw(drawArgs, _game.CurrentPlayerTurn);

            _game.DrawMainContent(drawArgs);
        }

        /// <summary>
        /// Checks where the user clicks on the canvas. If a board tile is clicked, it stores it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Currently unused example of what this method might
            // be used for later:
            // var action = Canvas.RunOnGameLoopThreadAsync(() =>
            // {

            //});
            
            if (OverDice)   // If the cursor is hovered over the dice image when clicked
            {
                RollDie();
                ClickedTileVector = CurrentTileVector;
                return;
            }
            UserClickedBoard = true;
            if (ClickedTileVector != null)  // If we have already clicked a tile on the mainboard before
            {
                if (_game.Players[_game.CurrentPlayerTurn].ChosenPiece != null)     // If the current player currently has a piece selected
                {
                    if (_game.Players[_game.CurrentPlayerTurn].ChosenPiece.AllowedDestinationTileVector != null)    // If the piece is actually able to move
                    {
                        if (CurrentTileVector == _game.Players[_game.CurrentPlayerTurn].ChosenPiece.AllowedDestinationTileVector.Value) // If the clicked event happened on the 
                        {                                                                                                              // movable position of the piece
                            _game.TakeTurn(_game.Players[_game.CurrentPlayerTurn]);
                            _dice.SetStandardDieImage();
                            return;
                        }
                    }
                }
                else    // If the user does not have a chosen piece already
                {
                    ClickedTileVector = CurrentTileVector;
                    foreach(Piece piece in _game.Players[_game.CurrentPlayerTurn].Pieces)
                    {
                        if (piece.Position == ClickedTileVector)
                        {
                            _game.Players[_game.CurrentPlayerTurn].ChosenPiece = piece;     // Set the clicked piece to chosen
                            return;
                        }
                    }
                }
            }
            ClickedTileVector = CurrentTileVector;  // This happens when the user has no stored value of the previous clicked tile vector
            foreach (Piece piece in _game.Players[_game.CurrentPlayerTurn].Pieces)
            {
                if (piece.Position == ClickedTileVector)
                {
                    _game.Players[_game.CurrentPlayerTurn].ChosenPiece = piece;
                    return;
                }
            }
            _game.Players[_game.CurrentPlayerTurn].ChosenPiece = null;  // If the clicked vector did not have the players piece on it
            ClickedTileVector = CurrentTileVector;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            //Currently unused, but the system requires it to exist.
        }

        private void CanvasUpdate(
            ICanvasAnimatedControl sender,
            CanvasAnimatedUpdateEventArgs args)
        {
            //Currently unused, but the system requires it to exist.
        }

        /// <summary>
        /// Event for clicking the die to roll.
        /// </summary>
        private void RollDie()
        {
            if (!Game.CurrentDiceRoll.HasValue)
            {
                Game.CurrentDiceRoll = _dice.Roll() + 1; //Dice.Roll() is not nullable, so this is an actual int.
                _game.AddRollToHistory(Game.CurrentDiceRoll.Value);
                SoundMixer.PlaySound(Tile.TileEventSounds["Tile"], SoundChannels.sfx2); //The die currently has no sound effect of its own, so we borrow one from Tile.
                if (!_game.Players[_game.CurrentPlayerTurn].CheckPossibilityToMove(Game.CurrentDiceRoll.Value))
                {
                    Game.CurrentDiceRoll = null;
                    _game.NextPlayerTurn();
                    _dice.SetStandardDieImage();
                }
            }
            CheckEndGame();
        }

        public static void InvokeMiniGameEvent(Player player)
        {
            MiniGameEvent.Invoke(player);
        }

        /// <summary>
        /// Starts the mini game inside a new window.
        /// </summary>
        /// <param name="invokingPlayer"></param>
        public async void StartMiniGame(Player invokingPlayer)
        {
            MiniGameNavigationParams navParams = new MiniGameNavigationParams
            {
                InvokingPlayer = invokingPlayer,
                OtherPlayers = _game.Players.Where(player => player != invokingPlayer).ToList()
            };
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame appWindowContentFrame = new Frame();

            appWindowContentFrame.Navigate(typeof(MiniGamePage), navParams);
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            AppWindows.Add(appWindowContentFrame.UIContext, appWindow);

            // This is for removing the AppWindow from the tracked windows
            appWindow.Closed += delegate
            {
                AppWindows.Remove(appWindowContentFrame.UIContext);
                appWindowContentFrame.Content = null;
                appWindow = null;
            };

            await appWindow.TryShowAsync();
        }

        /// <summary>
        /// Checks if the game does has any pieces left. If not, end the game.
        /// </summary>
        private void CheckEndGame()
        {
            if (_game.RemainingPieces == 0)
            {
                this.Frame.Navigate(typeof(gameover), _game.Players);
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
                    if (_game.Players.Count != 0 && Game.CurrentDiceRoll != null)  // This check has to be done to prevent from crashing when game starts.
                    {
                        var currentPlayerPiecesPositions = _game.Players[_game.CurrentPlayerTurn].Pieces.Select(piece => piece.Position).ToList();
                        if (currentPlayerPiecesPositions.Contains(CurrentTileVector.Value))  // Check if we hover over a players piece
                        {
                            SwitchCursorStyle(CoreCursorType.Hand);
                        }
                        else
                        {
                            SwitchCursorStyle(CoreCursorType.Arrow);
                        }
                    }
                    return;
                }
            }
            else if ((PointerX > _dice.DiceHolder.X && PointerX < _dice.DiceHolder.X + _dice.DiceHolder.Width))   // If pointer is inside dice image in X-axis
            {
                if ((PointerY > _dice.DiceHolder.Y && PointerY < _dice.DiceHolder.Y + _dice.DiceHolder.Height))
                {
                    OverDice = true;
                    SwitchCursorStyle(CoreCursorType.Hand);
                    return;
                }
            }

            SwitchCursorStyle(CoreCursorType.Arrow);
            UserClickedBoard = false;
            CurrentTileVector = null;
            OverDice = false;
        }

        /// <summary>
        /// Calculates what grid the cursor is on when hovering over the main board.
        /// </summary>
        private void CalculateCurrentTileVector()
        {
            CurrentTileVector = new Vector2((float)Math.Floor((PointerX - _game.Board.MainBoard.X) / (_game.Board.MainBoard.Width / 11)), 
                                            (float)Math.Floor((PointerY - _game.Board.MainBoard.Y) / (_game.Board.MainBoard.Height / 11)));
        }

        /// <summary>
        /// Changes the cursor type to the one sent in.
        /// </summary>
        /// <param name="cursor"></param>
        public void SwitchCursorStyle(CoreCursorType cursor)
        {
            switch (cursor)
            {
                case CoreCursorType.Arrow:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
                    break;
                case CoreCursorType.Hand:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
                    break;
                default:
                    break;
            }
        }
    }
}
