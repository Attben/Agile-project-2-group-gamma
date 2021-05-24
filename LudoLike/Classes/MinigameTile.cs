using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class MinigameTile : Tile
    {
        private Minigame _minigame;

        public MinigameTile(Minigame game)
        {
            _minigame = game;
            //this.index = index;
        }

        public override void TileEvent()
        {
            //Do something to start the _minigame.
        }
    }
}
