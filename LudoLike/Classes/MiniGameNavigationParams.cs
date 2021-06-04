using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LudoLike
{
    /// <summary>
    /// Holds data about the mini game in general which is sent through the pages connecting the main board to the mini game.
    /// </summary>
    class MiniGameNavigationParams
    {
        public Minigame MiniGame;
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
