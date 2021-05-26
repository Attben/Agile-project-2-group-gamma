using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    class MinigameTile : Tile
    {
        private Minigame _minigame;

        public MinigameTile(Rect targetRectangle, Minigame game) : base(targetRectangle)
        {
            _minigame = game;
        }

        public override void TileEvent()
        {
            //Do something to start the _minigame.
        }

        public override void Draw(CanvasAnimatedDrawEventArgs drawArgs)
        {
            base.Draw(drawArgs);
            drawArgs.DrawingSession.DrawImage(TileImages["MiniGame"], TargetRectangle);
        }
    }
}
