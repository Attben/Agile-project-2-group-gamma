using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
        public const string HighscoreFileLocation = @"Data\Highscores.txt";

        public Highscore()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ReadHighscoresFromFile(HighscoreFileLocation);
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

        public static async void AddHighscores(string location, List<Player> players)
        {
            try
            {
                //Read current highscores
                //string[] highscoreLines = System.IO.File.ReadAllLines(location);
                var nameScorePairs = new List<Tuple<string, int>>();
                using (StreamReader sr = new StreamReader(HighscoreFileLocation))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        nameScorePairs.Add(new Tuple<string, int>(line, int.Parse(sr.ReadLine())));
                    }
                }

                //Compare incoming player scores to current highscores

                //Add()ing directly to NameScorePair in the loop crashes the program because of
                //some async shenanigans. Using temp storage as a workaround.
                var newHighscoreTempStorage = new List<Tuple<string, int>>();
                foreach (Player p in players)
                {
                    foreach (Tuple<string, int> nameScorePair in nameScorePairs)
                    {
                        if (p.Score > nameScorePair.Item2)
                        {
                            //The player's score was higher than an entry in the highscore list, so we save it.
                            newHighscoreTempStorage.Add(new Tuple<string, int>(await NameEntryDialog(p), p.Score));
                            break;
                        }
                    }
                }
                //Add temp values to highscore list
                foreach(var nameScorePair in newHighscoreTempStorage)
                {
                    nameScorePairs.Add(nameScorePair);
                }
                //Sort the list by Item2 (score)
                nameScorePairs.Sort((lhs, rhs) => lhs.Item2.CompareTo(rhs.Item2));
                //Write top 10 to highscore file
                using (StreamWriter sw = new StreamWriter(HighscoreFileLocation))
                {
                    for (int n = 0; n < 10; ++n)
                    {
                        //Write name, then score, on separate lines.
                        sw.WriteLine(nameScorePairs[n].Item1);
                        sw.WriteLine(nameScorePairs[n].Item2);
                    }
                }
            }
            catch (IOException e)
            {
                //TODO: Log error or something
            }
        }

        private static async Task<string> NameEntryDialog(Player player)
        {
            TextBox input = new TextBox()
            {
                PlaceholderText = "Enter name"
            };
            ContentDialog dialog = new ContentDialog()
            {
                Title = $"{player.PlayerColor} has achieved a new highscore of {player.Score}!",
                PrimaryButtonText = "Submit",
                Content = input,
                IsSecondaryButtonEnabled = false
            };
            return (await dialog.ShowAsync() == ContentDialogResult.Primary) ? input.Text : "";
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));
        }
    }
}
