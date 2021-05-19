using Microsoft.Graphics.Canvas;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoLike
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public double bWidth = ApplicationView.GetForCurrentView().VisibleBounds.Width;
        public double bHeight = ApplicationView.GetForCurrentView().VisibleBounds.Height;
        public static float scaleWidth, scaleHeight;
        public static int DesignWidth = 1920;
        public static int DesignHeight = 1080;

        public CanvasBitmap BG;
        public CanvasBitmap CurrentDieImage;

        private Dice _dice = new Dice();
        private Random _prng = new Random();

        public MainPage()
        {
            this.InitializeComponent();
            ScalingInit();
        }

        private void CanvasCreateResources(
            CanvasAnimatedControl sender,
            CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            //Load background image
            BG = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/TestBackground.png"));

            //Load static images belonging to the Dice class
            for (int n = 0; n < 6; ++n)
            {
                Dice.DiceImages[n] = await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/Images/Die{n+1}.png"));
            }
            Dice.SpinningDieImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/SpinningDie.png"));
            CurrentDieImage = Dice.DiceImages[2];
        }

        private void CanvasDraw(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(TransformImage(BG));
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

        }

        private void CanvasUpdate(
            ICanvasAnimatedControl sender,
            CanvasAnimatedUpdateEventArgs args)
        {

        }

        public void ScalingInit()
        {
            SetScale(bWidth, bHeight);
            ControlProperties(bWidth, bHeight);
            Window.Current.SizeChanged += CurrentSizeChanged;
        }

        private void CurrentSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            SetScale(e.Size.Width, e.Size.Height);
            ControlProperties(e.Size.Width, e.Size.Height);
        }

        private void ControlProperties(double width, double height)
        {
            Canvas.Width = width;
            Canvas.Height = height;
        }

        private void RollDie(object sender, RoutedEventArgs e)
        {
            CurrentDieImage = Dice.SpinningDieImage;
            _dice.AnimationTimer = 40;

            CurrentDieImage = Dice.DiceImages[_prng.Next(0, 5)];
        }

        public static void SetScale(double width, double height)
        {
            scaleWidth = (float)(width / DesignWidth);
            scaleHeight = (float)(height / DesignHeight);
        }

        public static Transform2DEffect TransformImage(CanvasBitmap sourceImage)
        {
            Transform2DEffect image = new Transform2DEffect() { Source = sourceImage };
            image.TransformMatrix = Matrix3x2.CreateScale(scaleWidth, scaleHeight);
            return image;
        }

        public static float Xpos(float x)
        {
            float output = x * scaleWidth;
            return output;
        }

        public static float Ypos(float y)
        {
            float output = y * scaleHeight;
            return output;
        }
    }
}
