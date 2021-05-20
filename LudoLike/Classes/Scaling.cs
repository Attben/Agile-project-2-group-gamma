using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LudoLike
{
    /// <summary>
    /// Helper class that provides methods for rescaling images when the size of the parent window changes.
    /// </summary>
    public static class Scaling
    {
        public static double bWidth = Window.Current.Bounds.Width;
        public static double bHeight = Window.Current.Bounds.Height;
        public static float scaleWidth, scaleHeight;
        public static int DesignWidth = 1920;
        public static int DesignHeight = 1080;

        public static void ScalingInit(double width = 1920, double height = 1080)
        {
            bWidth = width;
            bHeight = height;
            SetScale(bWidth, bHeight);
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
