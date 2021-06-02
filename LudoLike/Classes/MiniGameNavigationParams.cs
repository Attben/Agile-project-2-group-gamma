using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class MiniGameNavigationParams
    {
        public Player InvokingPlayer;
        public List<Player> OtherPlayers = new List<Player>();
        public int PlayersToChallenge;
        public MiniGameNavigationParams()
        {

        }
    }
}
