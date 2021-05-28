using Microsoft.Graphics.Canvas;
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
    /// Represents a tile on which a player piece is teleported to or from.
    /// </summary>
    class TeleportTile : Tile
    {
        private Rect _destinationTile;

        /// <summary>
        /// Creates a Tile on a grid position, connected to the destination Tile.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="destination"></param>
        /// <param name="gridPosition"></param>
        public TeleportTile(Rect targetRectangle, Rect destination, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            _destinationTile = destination;
            TileImage = TileImages["Teleport"];
        }

        public override void TileEvent()
        {
            //Do something to transport a Player to _destinationTile.
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
