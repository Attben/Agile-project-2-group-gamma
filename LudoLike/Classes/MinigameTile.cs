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
    /// Represents a tile that wich is stepped on activates a mini game for the players to play.
    /// </summary>
    class MinigameTile : Tile
    {
        private Minigame _minigame;

        /// <summary>
        /// Creates a mini game tile with its target game to be activated when stepped on.
        /// </summary>
        /// <param name="targetRectangle"></param>
        /// <param name="game"></param>
        /// <param name="gridPosition"></param>
        public MinigameTile(Rect targetRectangle, Minigame game, Vector2 gridPosition) : base(targetRectangle, gridPosition)
        {
            _minigame = game;
        }

        public override bool TileEvent(Player player)
        {
            base.TileEvent(player);
            GameBoard.InvokeMiniGameEvent(player);
            //Do something to start the _minigame.
            return false;
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImages["MiniGame"], TargetRectangle);
        }
    }
}
