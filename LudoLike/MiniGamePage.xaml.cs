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
    /// This is a page shown when a player piece steps on a minigametile.
    /// </summary>
    public sealed partial class MiniGamePage : Page
    {
        private MiniGameNavigationParams _navParams;


        public MiniGamePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navParams = (MiniGameNavigationParams)e.Parameter;
        }

        private void RockPaperScissorsButton_Click(object sender, RoutedEventArgs e)
        {
            _navParams.PlayersToChallenge = 1;
            _navParams.MiniGamePage = typeof(RockPaperScissorsPage);
            Frame.Navigate(typeof(MiniGameChallengePlayersPage), _navParams);
        }
    }
}
