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
        private bool _isConsumed = false;   // Used to remove special pieces that are only to be activated once

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
            TurnHistoryString = "💰";
        }

        public override void TileEvent(Player player)
        {
            base.TileEvent(player);
            if (!this._isConsumed)
            {
                player.ChangeScore(_amount);
                this._isConsumed = true;
                _tileEventSound = Tile.TileEventSounds["Tile"];
                TurnHistoryString = null;
            }
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            if (this._isConsumed)
            {
                base.Draw(drawArgs);
            }
            else
            {
                drawArgs.DrawingSession.DrawImage(TileImage, TargetRectangle);
            }
        }
    }
}
