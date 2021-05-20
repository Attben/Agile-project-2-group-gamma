using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class TeleportTile : Tile
    {
        private Tile _destinationTile;

        public override void TileEvent()
        {
            //Do something to transport a Player to _destinationTile.
        }
    }
}
