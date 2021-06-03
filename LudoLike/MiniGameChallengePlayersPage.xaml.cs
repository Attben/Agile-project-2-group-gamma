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
        private MiniGameNavigationParams _navParams = new MiniGameNavigationParams();
        private List<Player> _challengedPlayers = new List<Player>();
        public MiniGameChallengePlayersPage()
        {
            this.InitializeComponent();
        }


        private void CreatePlayerButtons()
        {
            if(_navParams.OtherPlayers.Count() == 3)
            {
                SetButtonProperties(_navParams.OtherPlayers[0]);
                SetButtonProperties(_navParams.OtherPlayers[1]);
                SetButtonProperties(_navParams.OtherPlayers[2]);
            } 
            else if (_navParams.OtherPlayers.Count() == 2)
            {
                SetButtonProperties(_navParams.OtherPlayers[0]);
                SetButtonProperties(_navParams.OtherPlayers[1]);
                Player3Button.Visibility = Visibility.Collapsed;
            }
            else if(_navParams.OtherPlayers.Count() == 1)
            {
                SetButtonProperties(_navParams.OtherPlayers[0]);
                Player2Button.Visibility = Visibility.Collapsed;
                Player3Button.Visibility = Visibility.Collapsed;
            }
        }

        private void SetButtonProperties(Player player)
        {
            Button button;
            if (_navParams.OtherPlayers.IndexOf(player) == 0)
            {
                button = Player1Button;
            }
            else if (_navParams.OtherPlayers.IndexOf(player) == 1)
            {
                button = Player2Button;
            }
            else
            {
                button = Player3Button;
            }

            button.Content = $"Player {player.PlayerColor}";
            button.FontSize = 26;
            button.Margin = new Thickness(10);
            button.Height = 100;
            button.Width = 250;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Background = new SolidColorBrush(Windows.UI.Colors.GhostWhite);
            button.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            button.BorderBrush = CreateButtonBorder(player);
            button.BorderThickness = new Thickness(0);
            button.Opacity = 0.8;
        }

        private SolidColorBrush CreateButtonBorder(Player player)
        {
            if (player.PlayerColor == PlayerColors.Red) 
            { 
                return new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else if (player.PlayerColor == PlayerColors.Blue)
            {
                return new SolidColorBrush(Windows.UI.Colors.Blue);
            }
            else if (player.PlayerColor == PlayerColors.Yellow)
            {
                return new SolidColorBrush(Windows.UI.Colors.Yellow);
            }
            else
            {
                return new SolidColorBrush(Windows.UI.Colors.Green);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navParams = (MiniGameNavigationParams)e.Parameter;
            CreatePlayerButtons();
        }

        private void PlayerButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "Player1Button":
                    if (_challengedPlayers.Contains(_navParams.OtherPlayers[0]))
                    {
                        _challengedPlayers.RemoveAt(_challengedPlayers.IndexOf(_navParams.OtherPlayers[0]));
                        button.BorderThickness = new Thickness(0);
                        button.Opacity = 0.8;
                        return;
                    }
                    else if (_challengedPlayers.Count() >= _navParams.PlayersToChallenge)
                    {
                        ErrorMessage.Text = $"YOU CAN ONLY CHALLENGE {_navParams.PlayersToChallenge} PLAYERS";
                        return;
                    }
                    else
                    {
                        _challengedPlayers.Add(_navParams.OtherPlayers[0]);
                    }
                    break;
                case "Player2Button":
                    if (_challengedPlayers.Contains(_navParams.OtherPlayers[1]))
                    {
                        _challengedPlayers.RemoveAt(_challengedPlayers.IndexOf(_navParams.OtherPlayers[1]));
                        button.BorderThickness = new Thickness(0);
                        button.Opacity = 0.8;
                        return;
                    }
                    else if (_challengedPlayers.Count() >= _navParams.PlayersToChallenge)
                    {
                        ErrorMessage.Text = $"YOU CAN ONLY CHALLENGE {_navParams.PlayersToChallenge} PLAYERS";
                        return;
                    }
                    else
                    {
                        _challengedPlayers.Add(_navParams.OtherPlayers[1]);
                    }
                    break;
                case "Player3Button":
                    if (_challengedPlayers.Contains(_navParams.OtherPlayers[2]))
                    {
                        _challengedPlayers.RemoveAt(_challengedPlayers.IndexOf(_navParams.OtherPlayers[2]));
                        button.BorderThickness = new Thickness(0);
                        button.Opacity = 0.8;
                        return;
                    }
                    else if (_challengedPlayers.Count() >= _navParams.PlayersToChallenge)
                    {
                        ErrorMessage.Text = $"YOU CAN ONLY CHALLENGE {_navParams.PlayersToChallenge} PLAYERS";
                        return;
                    }
                    else
                    {
                        _challengedPlayers.Add(_navParams.OtherPlayers[2]);
                    }
                    break;
                default:
                    break;
            }
            button.BorderThickness = new Thickness(10);
            button.Opacity = 1;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            _navParams.ChallengedPlayers = _challengedPlayers;
            Frame.Navigate(_navParams.MiniGamePage, _navParams);
        }
    }
}
