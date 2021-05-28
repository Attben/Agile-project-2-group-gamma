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
    /// <summary>
    /// Creates a static tile with no special acivation. Differs from regular tile by its color.
    /// </summary>
    public class StaticTile : Tile
    {
        /// <summary>
        /// Creates a standard Tile with no activation. Valid Colors are found in the assets.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="color"></param>
        /// <param name="gridPosition"></param>
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
