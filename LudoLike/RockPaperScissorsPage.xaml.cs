﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Media.Core;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LudoLike
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RockPaperScissorsPage : Page
    {
        private MiniGameNavigationParams _navParams;
        private Player _player1;
        private Player _player2;
        private CanvasBitmap _backGround;
        private CanvasTextFormat _textFormat = new CanvasTextFormat();
        private List<CanvasBitmap> _rightHandImages = new List<CanvasBitmap>();
        private List<CanvasBitmap> _leftHandImages = new List<CanvasBitmap>();
        private List<List<MediaSource>> _soundLists = new List<List<MediaSource>>();
        private List<MediaSource> _rockSounds = new List<MediaSource>();
        private List<MediaSource> _paperSounds = new List<MediaSource>();
        private List<MediaSource> _scissorsSounds = new List<MediaSource>();
        private Random _rngImage = new Random();
        private _moveChoices _p1Hand;
        private _moveChoices _p2Hand;
        private int _drawSessions;
        private bool _countDrawingSessions = false;
        private string _winner;

        private enum _moveChoices
        {
            rock, paper, scissors
        }

        public RockPaperScissorsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navParams = (MiniGameNavigationParams)e.Parameter;
            _player1 = _navParams.InvokingPlayer;
            _player2 = _navParams.ChallengedPlayers[0];
        }
        private void CanvasCreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            CreateTextFormat();
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        private void CreateTextFormat()
        {
            _textFormat.FontFamily = "Helvetica";
            _textFormat.FontSize = 40;
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            //Images
            _backGround = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/rockright.png")));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/paperright.png")));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/scissorright.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/rockleft.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/paperleft.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/scissorleft.png")));
            //Sounds
            _rockSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Rock1.wav")));
            _rockSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Rock2.wav")));
            _rockSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Rock3.wav")));
            _paperSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PaperBag1.wav")));
            _paperSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/PaperBag2.wav")));
            _scissorsSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Scissors1.wav")));
            _scissorsSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Scissors2.wav")));
            _scissorsSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Scissors3.wav")));
            _scissorsSounds.Add(MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/Scissors4.wav")));

            _soundLists.Add(_rockSounds);
            _soundLists.Add(_paperSounds);
            _soundLists.Add(_scissorsSounds);
        }
        private void DrawControls(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            Rect p1ControlHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 3, sender.Size.Width / 5, sender.Size.Height / 7);
            args.DrawingSession.DrawImage(_leftHandImages[0], new Rect(p1ControlHolder.X, p1ControlHolder.Y - p1ControlHolder.Height/2, p1ControlHolder.Width/3, p1ControlHolder.Height/2));
            args.DrawingSession.DrawImage(_leftHandImages[1], new Rect(p1ControlHolder.X + p1ControlHolder.Width/3, p1ControlHolder.Y - p1ControlHolder.Height / 2, p1ControlHolder.Width / 3, p1ControlHolder.Height / 2));
            args.DrawingSession.DrawImage(_leftHandImages[2], new Rect(p1ControlHolder.X + p1ControlHolder.Width/3*2, p1ControlHolder.Y - p1ControlHolder.Height / 2, p1ControlHolder.Width / 3, p1ControlHolder.Height / 2));
            args.DrawingSession.DrawText("1", new Rect(p1ControlHolder.X, p1ControlHolder.Y, p1ControlHolder.Width / 3, p1ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);
            args.DrawingSession.DrawText("2", new Rect(p1ControlHolder.X + p1ControlHolder.Width / 3, p1ControlHolder.Y, p1ControlHolder.Width / 3, p1ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);
            args.DrawingSession.DrawText("3", new Rect(p1ControlHolder.X + p1ControlHolder.Width / 3 * 2, p1ControlHolder.Y, p1ControlHolder.Width / 3, p1ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);


            Rect p2ControlHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 3, sender.Size.Width / 5, sender.Size.Height / 7);
            args.DrawingSession.DrawImage(_rightHandImages[0], new Rect(p2ControlHolder.X, p2ControlHolder.Y - p2ControlHolder.Height / 2, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2));
            args.DrawingSession.DrawImage(_rightHandImages[1], new Rect(p2ControlHolder.X + p2ControlHolder.Width / 3, p2ControlHolder.Y - p2ControlHolder.Height / 2, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2));
            args.DrawingSession.DrawImage(_rightHandImages[2], new Rect(p2ControlHolder.X + p2ControlHolder.Width / 3 * 2, p2ControlHolder.Y - p2ControlHolder.Height / 2, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2));
            args.DrawingSession.DrawText("7", new Rect(p2ControlHolder.X, p2ControlHolder.Y, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);
            args.DrawingSession.DrawText("8", new Rect(p2ControlHolder.X + p2ControlHolder.Width / 3, p2ControlHolder.Y, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);
            args.DrawingSession.DrawText("9", new Rect(p2ControlHolder.X + p2ControlHolder.Width / 3 * 2, p2ControlHolder.Y, p2ControlHolder.Width / 3, p2ControlHolder.Height / 2), Windows.UI.Colors.Black, _textFormat);
        }
        private void CanvasDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(_backGround, new Rect(0, 0, sender.Size.Width, sender.Size.Height));
            DrawControls(sender, args);
            if (!_countDrawingSessions)
            {
                args.DrawingSession.DrawText($"PRESS SPACE TO START..", (float)sender.Size.Width / 2 - 200, (float)sender.Size.Height / 6, Windows.UI.Colors.Black, _textFormat);
                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[0], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[0], p2HandHolder);
            }
            if (_countDrawingSessions)
            {
                _drawSessions += 1;

            }

            
            if (_countDrawingSessions && _drawSessions <= 360)
            {
                args.DrawingSession.DrawText($"{Math.Floor((decimal)_drawSessions / 60)}", (float)sender.Size.Width / 2 - 20, (float)sender.Size.Height / 6, Windows.UI.Colors.Black, _textFormat);
                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[0], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[0], p2HandHolder);
            }
            else if (_drawSessions > 360 && _drawSessions <= 420)
            {
                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[_rngImage.Next(0,2)], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[_rngImage.Next(0, 2)], p2HandHolder);

            } 
            else if (_drawSessions == 421)
            {
                _winner = CheckWinner();
            }
            else if (_drawSessions > 421)/*if (_drawSessions > 421 && _drawSessions < 600)*/
            {
                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[(int)_p1Hand], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[(int)_p2Hand], p2HandHolder);

                args.DrawingSession.DrawText($"Game Over", (float)sender.Size.Width / 2, (float)sender.Size.Height / 2, Windows.UI.Colors.Black);
                args.DrawingSession.DrawText($"{_winner}", (float)sender.Size.Width / 2, (float)sender.Size.Height / 2 + 50, Windows.UI.Colors.Black);
            }
            
        }


        private string CheckWinner()
        {
            if(_p1Hand == _p2Hand) //If both players chose the same move, it's a draw.
            {
                return "No winner!";
            }
            else if ((_p1Hand == _moveChoices.paper && _p2Hand == _moveChoices.rock) ||
                (_p1Hand == _moveChoices.scissors && _p2Hand == _moveChoices.paper) ||
                (_p1Hand == _moveChoices.rock && _p2Hand == _moveChoices.scissors))
            {
                Classes.SoundMixer.PlayRandomSound(_soundLists[(int)_p1Hand]);
                _player1.ChangeScore(200);
                _player2.ChangeScore(-200);
                return "Player 1 Wins!";
            }
            else //It's not a draw, and player1 didn't win, so player 2 must've won.
            {
                Classes.SoundMixer.PlayRandomSound(_soundLists[(int)_p2Hand]);
                _player2.ChangeScore(200);
                _player1.ChangeScore(-200);
                return "Player 2 Wins!";
            }
        }

        private void RockPaperScissorsGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Number1:
                case VirtualKey.NumberPad1:
                    _p1Hand = _moveChoices.rock;
                    break;
                case VirtualKey.Number2:
                case VirtualKey.NumberPad2:
                    _p1Hand = _moveChoices.paper;
                    break;
                case VirtualKey.Number3:
                case VirtualKey.NumberPad3:
                    _p1Hand = _moveChoices.scissors;
                    break;
                case VirtualKey.Number7:
                case VirtualKey.NumberPad7:
                    _p2Hand = _moveChoices.rock;
                    break;
                case VirtualKey.Number8:
                case VirtualKey.NumberPad8:
                    _p2Hand = _moveChoices.paper;
                    break;
                case VirtualKey.Number9:
                case VirtualKey.NumberPad9:
                    _p2Hand = _moveChoices.scissors;
                    break;
                case VirtualKey.Space:
                    _countDrawingSessions = true;
                    break;
                default:
                    break;
            }

        }

        private void CanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {

        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            RockPaperscissorsCanvas.RemoveFromVisualTree();
            RockPaperscissorsCanvas = null;
        }


        // Source: https://stackoverflow.com/questions/58908845/keydown-event-doesnt-trigger-from-selected-grid-in-uwp
        private async void MainGrid_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { KeyDownControl.Focus(FocusState.Keyboard); });
        }
    }
}