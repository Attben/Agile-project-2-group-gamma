using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LudoLike
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MiniGameChallengePlayersPage : Page
    {
        private Player _invokingPlayer;
        private List<Player> _otherPlayers = new List<Player>();
        private List<Player> _challengedPlayers = new List<Player>();
        private int _numberOfPlayersToChallenge;
        public MiniGameChallengePlayersPage()
        {
            this.InitializeComponent();
            try
            {
                SetPlayerButtonProperites(Player1Button, _otherPlayers[0]);
                SetPlayerButtonProperites(Player2Button, _otherPlayers[1]);
                SetPlayerButtonProperites(Player3Button, _otherPlayers[2]);
                
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private void CreatePlayerButtons(Button button, Player player)
        {
            Player1Button.Content = $"Player {player.PlayerColor}";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MiniGameNavigationParams navParams = (MiniGameNavigationParams)e.Parameter;
            _invokingPlayer = navParams.InvokingPlayer;
            _otherPlayers = navParams.OtherPlayers;
        }

        private void Player1Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Player2Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Player3Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
