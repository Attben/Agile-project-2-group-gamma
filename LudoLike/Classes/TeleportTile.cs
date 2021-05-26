using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    class TeleportTile : Tile
    {
        private Rect _destinationTile;

        public TeleportTile(Rect targetRectangle, CanvasBitmap tileImage, Rect destination) : base(targetRectangle, tileImage)
        {
            _destinationTile = destination;
        }

        public override void TileEvent()
        {
            //Do something to transport a Player to _destinationTile.
        }
    }
}
