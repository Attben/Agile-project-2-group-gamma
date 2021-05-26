using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    public class Tile
    {
        public static Dictionary<string, CanvasBitmap> TileImages = new Dictionary<string, CanvasBitmap>();
        public CanvasBitmap TileImage;
        public Rect TargetRectangle;

        public Tile(Rect targetRectangle, CanvasBitmap tileImage)
        {
            TileImage = tileImage;
            TargetRectangle = targetRectangle;
        }

        public virtual void TileEvent()
        {
            //Intentionally left blank in base class.
        }

        public void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
