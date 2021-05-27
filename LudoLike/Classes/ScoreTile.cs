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
    class ScoreTile : Tile
    {
        private int _amount;

        public ScoreTile(Rect targetRectangle, int points, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            _amount = points;
            TileImage = TileImages["Score"];
        }

        public override void TileEvent()
        {
            //Do something with _amount
        }

        public void TileEvent(Player player)
        {
            //Do something with _amount
            player.ChangeScore(_amount);
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
