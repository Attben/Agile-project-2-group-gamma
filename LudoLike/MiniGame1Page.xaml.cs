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
    public sealed partial class MiniGame1Page : Page
    {
        private CanvasBitmap _backGround;
        private CanvasTextFormat _textFormat = new CanvasTextFormat();
        private int _p1Hand;
        private int _p2Hand;
        private int _drawSessions;
        private bool _countDrawingSessions = false;
        private string _winner;
        public MiniGame1Page()
        {
            this.InitializeComponent();

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

        private void CanvasDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(_backGround, new Rect(0, 0, sender.Size.Width, sender.Size.Height));
            if (_countDrawingSessions == true)
            {
                _drawSessions += 1;

            }
            if (_drawSessions <= 360)
            {
                Rect p1Hand = new Rect(sender.Size.Width / 5, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawRectangle(p1Hand, Windows.UI.Colors.Red);
                args.DrawingSession.DrawText($"{_p1Hand}", (float)(p1Hand.X + p1Hand.Width / 2 - _textFormat.FontSize), (float)(p1Hand.Y + p1Hand.Height / 2 - _textFormat.FontSize), Windows.UI.Colors.Black, _textFormat);

                Rect p2Hand = new Rect(sender.Size.Width / 5 * 3, sender.Size.Height / 2, sender.Size.Width / 5, sender.Size.Height / 5);
                args.DrawingSession.DrawText($"{_p2Hand}", (float)(p2Hand.X + p2Hand.Width / 2 - _textFormat.FontSize), (float)(p2Hand.Y + p2Hand.Height / 2 - _textFormat.FontSize), Windows.UI.Colors.Black, _textFormat);
                args.DrawingSession.DrawRectangle(p2Hand, Windows.UI.Colors.Blue);

                args.DrawingSession.DrawText($"iterations Time: {Math.Floor((decimal)_drawSessions/60)}", (float)sender.Size.Width/2 - 25, (float)sender.Size.Height/3, Windows.UI.Colors.Black);

            }
            else if(_drawSessions == 361)
            {
                _winner = CheckWinner();
            }
            else
            {
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

        private void MiniGame1Grid_KeyDown(object sender, KeyRoutedEventArgs e)
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
                    _countDrawingSessions = true;
                    break;
                default:
                    break;
            }

        }
    }
}
