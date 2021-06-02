using Microsoft.Graphics.Canvas;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LudoLike
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RockPaperScissors : Page
    {
        private Player _player1;
        private Player _player2;
        private CanvasBitmap _backGround;
        private CanvasTextFormat _textFormat = new CanvasTextFormat();
        private List<CanvasBitmap> _rightHandImages = new List<CanvasBitmap>();
        private List<CanvasBitmap> _leftHandImages = new List<CanvasBitmap>();
        private Random _rngImage = new Random();
        private int _p1Hand;
        private int _p2Hand;
        private int _drawSessions;
        private bool _countDrawingSessions = false;
        private string _winner;
        public RockPaperScissors()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private void CanvasCreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            CreateTextFormat();
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        private void CreateTextFormat()
        {
            _textFormat.FontFamily = "Helvetica";
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            _backGround = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/rockright.png")));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/paperright.png")));
            _rightHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/scissorright.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/rockleft.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/paperleft.png")));
            _leftHandImages.Add(await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/RPS/scissorleft.png")));
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

            if (_countDrawingSessions == true)
            {
                _drawSessions += 1;

            }
            if (_drawSessions <= 360)
            {

                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[0], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[0], p2HandHolder);

                args.DrawingSession.DrawText($"iterations Time: {Math.Floor((decimal)_drawSessions / 60)}", (float)sender.Size.Width / 2 - 25, (float)sender.Size.Height / 3, Windows.UI.Colors.Black);

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
            else
            {
                Rect p1HandHolder = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_leftHandImages[_p1Hand], p1HandHolder);

                Rect p2HandHolder = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawImage(_rightHandImages[_p2Hand], p2HandHolder);

                args.DrawingSession.DrawText($"Game Over", (float)sender.Size.Width / 2, (float)sender.Size.Height / 2, Windows.UI.Colors.Black);
                args.DrawingSession.DrawText($"{_winner}", (float)sender.Size.Width / 2, (float)sender.Size.Height / 2 + 50, Windows.UI.Colors.Black);
            }
        }

        private string CheckWinner()
        {

            if ((_p1Hand == 1 && _p2Hand == 0) || (_p1Hand == 2 && _p2Hand == 1) || (_p1Hand == 0 && _p2Hand == 2))
            {
                return "Player 1 Wins!";
            }
            else if ((_p1Hand == 0 && _p2Hand == 1) || (_p1Hand == 1 && _p2Hand == 2) || (_p1Hand == 2 && _p2Hand == 0))
            {
                return "Player 2 Wins!";
            }
            else
            {
                return "No Winner!";
            }
        }

        private void RockPaperScissorsGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Number1:
                    _p1Hand = 0;
                    break;
                case VirtualKey.Number2:
                    _p1Hand = 1;
                    break;
                case VirtualKey.Number3:
                    _p1Hand = 2;
                    break;
                case VirtualKey.Number7:
                    _p2Hand = 0;
                    break;
                case VirtualKey.Number8:
                    _p2Hand = 1;
                    break;
                case VirtualKey.Number9:
                    _p2Hand = 2;
                    break;
                case VirtualKey.Space:
                    if (_countDrawingSessions == false)
                    {
                        _countDrawingSessions = true;
                    }
                    break;
                default:
                    break;
            }

        }

        private void CanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {

        }

        private void CanvasPointerPressed(object sender, PointerRoutedEventArgs e)
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
    }
}
