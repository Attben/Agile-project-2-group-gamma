using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{
    class ScoreTile : Tile
    {
        private int _amount;

        public ScoreTile(Rect targetRectangle, CanvasBitmap tileImage, int points) : base(targetRectangle, tileImage)
        {
            _amount = points;
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
    }
}
