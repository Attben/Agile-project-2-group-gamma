using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class gameover : Page
    {
        List<Player> _players;

        public gameover()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _players = (List<Player>)e.Parameter;
            PopulateLists();
        }

        private void PopulateLists()
        {
            Brush BlackBrush = new SolidColorBrush(Windows.UI.Colors.Black);

            foreach (Player player in _players)
            {
                int score = player.Score;
                string color = player.PlayerColor.ToString();

                ListViewItem playername = new ListViewItem();
                ListViewItem playerscore = new ListViewItem();

                playername.Content = color;
                playerscore.Content = score;

                playername.Foreground = BlackBrush;
                playerscore.Foreground = BlackBrush;

                playername.FontSize = 90;
                playerscore.FontSize = 90;

                playerscore.HorizontalContentAlignment = HorizontalAlignment.Right;

                playerlist.Items.Add(playername);
                scorelist.Items.Add(playerscore);

            }
        }

        private void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            Classes.Highscore.AddHighscores(_players);
            this.Frame.Navigate(typeof(MainMenu));
        }

        private async void MainGrid_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { KeyDownControl.Focus(FocusState.Keyboard); });
        }
    }
}
