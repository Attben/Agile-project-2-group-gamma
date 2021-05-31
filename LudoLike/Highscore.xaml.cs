using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LudoLike.Classes
{
    /// <summary>
    /// A Page for reading and displaying stored highscore lists.
    /// </summary>
    public sealed partial class Highscore : Page
    {
        private string[] _linesFromHighscoreFile;

        public Highscore()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ReadHighscoresFromFile(@"Data\Highscores.txt");
        }

        private void ReadHighscoresFromFile(string location)
        {
            try
            {
                _linesFromHighscoreFile = System.IO.File.ReadAllLines(location);

                //TODO: Improve text formatting
                StringBuilder names = new StringBuilder();
                StringBuilder scores = new StringBuilder();
                for (int n = 0; n < _linesFromHighscoreFile.Length; n += 2)
                {
                    string placement;
                    switch (n / 2)
                    {
                        case 0:
                            placement = "🥇: ";
                            break;
                        case 1:
                            placement = "🥈: ";
                            break;
                        case 2:
                            placement = "🥉: ";
                            break;
                        default:
                            placement = $"{n / 2 + 1}: ";
                            break;
                    }
                    names.Append(placement);
                    names.Append(_linesFromHighscoreFile[n] + "\n");
                    scores.Append(_linesFromHighscoreFile[n + 1] + "\n");
                }
                HighscoreNames.Text = names.ToString();
                HighscoreScores.Text = scores.ToString();
            }
            catch (IOException e)
            {
                //TODO: Do something. Maybe log the error somewhere?
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }
    }
}
