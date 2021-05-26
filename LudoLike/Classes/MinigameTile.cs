using Microsoft.Graphics.Canvas;
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

        public MinigameTile(Rect targetRectangle, CanvasBitmap tileImage, Minigame game) : base(targetRectangle, tileImage)
        {
            _minigame = game;
        }

        public override void TileEvent()
        {
            //Do something to start the _minigame.
        }
    }
}
