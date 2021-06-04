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
    /// Represents a Tile which hold score for the player to gain when stepped on.
    /// </summary>
    class ScoreTile : Tile
    {
        private readonly int _amount;

        /// <summary>
        /// Creates a Tile on the rectangle. Points represents the points to gain when stepped on.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="points"></param>
        /// <param name="gridPosition"></param>
        public ScoreTile(Rect targetRectangle, int points, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            _amount = points;
            TileImage = TileImages["Score"];
        }


        public override bool TileEvent(Player player)
        {
            base.TileEvent(player);
            player.ChangeScore(_amount);
            return true;
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
        }
    }
}
