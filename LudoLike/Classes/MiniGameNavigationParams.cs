using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LudoLike
{
    class MiniGameNavigationParams
    {
        public Type MiniGamePage;
        public Player InvokingPlayer;
        public List<Player> OtherPlayers = new List<Player>();
        public int PlayersToChallenge;
        public List<Player> ChallengedPlayers = new List<Player>();
        public MiniGameNavigationParams()
        {

        }
    }
}
