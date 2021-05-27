using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    public class StaticTile : Tile
    {

        public StaticTile(Rect targetRectangle, string color, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            TileImage = TileImages[color];
        }

        public override void TileEvent()
        {
            //Do something to transport a Player to _destinationTile.
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
