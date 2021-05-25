﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike.Classes
{
    public class TestTile
    {
        public static Dictionary<string, CanvasBitmap> TileImages = new Dictionary<string, CanvasBitmap>();
        public CanvasBitmap TileImage;
        public Rect TargetRectangle;

        public TestTile(Rect targetRectangle, CanvasBitmap tileImage)
        {
            TileImage = tileImage;
            TargetRectangle = targetRectangle;
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}