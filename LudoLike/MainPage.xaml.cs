﻿using Microsoft.Graphics.Canvas;
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
        public CanvasBitmap CurrentDieImage;
        public List<CanvasBitmap> Tiles = new List<CanvasBitmap>();

        public LudoBoard Board;


        private Dice _dice = new Dice();
        private Random _prng = new Random();
        private GameStateManager _gameStateManager = new GameStateManager();


        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentSizeChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(1920, 1080);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Scaling.ScalingInit();
            Board = new LudoBoard();
            ControlProperties(Scaling.bWidth, Scaling.bHeight);
            _gameStateManager.GameStateInit();
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
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n+1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
            CurrentDieImage = Dice.DiceImages[2];

            // Create Tiles
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Tiles.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/peng.png")));
                }
            }
        }

        private void CurrentSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Scaling.ScalingInit(e.Size.Width, e.Size.Height);
            ControlProperties(e.Size.Width, e.Size.Height);
        }

        private void CanvasDraw(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs args)
        {
            // AVSLUTADE HÄR FORTSÄTT FIXA MED NÄSTENA

            args.DrawingSession.DrawImage(Scaling.TransformImage(BG));
            //args.DrawingSession.FillRectangle(Board.MainBoard, Windows.UI.Colors.White);
            //args.DrawingSession.FillRectangle(Board.RedNest, Windows.UI.Colors.Red);
            //args.DrawingSession.FillRectangle(Board.YellowNest, Windows.UI.Colors.Yellow);
            //args.DrawingSession.FillRectangle(Board.GreenNest, Windows.UI.Colors.LawnGreen);
            //args.DrawingSession.FillRectangle(Board.BlueNest, Windows.UI.Colors.Blue);

            //args.DrawingSession.DrawText($"bWidth: {Board.MainBoard.Width}, bHeight{Board.MainBoard.Height}", 0, 0, Windows.UI.Colors.Black);

            Board.Draw(args);

            // Denna lösningen är så jävla dålig. Måste hitta ett bättre sätt att lösa det på
            float x = 17;
            float y = 18;
            int tileNumber = 0;

            //for (int i = 0; i < 11; i++)
            //{
            //    for (int j = 0; j < 11; j++)
            //    {
            //        args.DrawingSession.DrawImage(TransformImage(Tiles[tileNumber]), Xpos(x + (float)(bWidth / 2 - Board.Width / 2)), Ypos(y + (float)(bHeight / 2 - Board.Height / 2)));
            //        x += 75;
            //        tileNumber++;
            //    }
            //    x = 17;
            //    y += 74;
            //}
            args.DrawingSession.DrawText($"bWidth: {Scaling.bWidth}, bHeight{Scaling.bHeight}", 0, 0, Windows.UI.Colors.Black);
            //args.DrawingSession.DrawImage(Scaling.TransformImage(BG));
            _gameStateManager.Draw(args);

            if (_dice.AnimationTimer == 0)
            {
                args.DrawingSession.DrawImage(CurrentDieImage, 200, 200);
            }
            else
            {
                args.DrawingSession.DrawImage(Dice.SpinningDieImage, 200, 200);
                --_dice.AnimationTimer;
            }
        }

        private void CanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(Canvas).Position;
            if(_gameStateManager.CurrentGameState == GameState.NewGame)
            {
                var action = Canvas.RunOnGameLoopThreadAsync( () =>
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
            CurrentDieImage = Dice.SpinningDieImage;
            _dice.AnimationTimer = 40;

            CurrentDieImage = Dice.DiceImages[_prng.Next(0, 5)];
        }
    }
}
