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
    class TeleportTile : Tile
    {
        private Rect _destinationTile;

        public TeleportTile(Rect targetRectangle, Rect destination) : base(targetRectangle)
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
