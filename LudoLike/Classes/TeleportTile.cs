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
        private Vector2 _destinationTile;

        /// <summary>
        /// Creates a Tile on a grid position, connected to the destination Tile.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="destination"></param>
        /// <param name="gridPosition"></param>
        public TeleportTile(Rect targetRectangle, Vector2 destination, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            _destinationTile = destination;
            TileImage = TileImages["Teleport"];
        }

        public override bool TileEvent(Player player)
        {
            base.TileEvent(player);
            //Do something to transport a Player to _destinationTile.
            player._pieces[0].Move(_destinationTile);
            return false;
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
