using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class ScoreTile : Tile
    {
        private int _amount;

        public ScoreTile(int points)
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
