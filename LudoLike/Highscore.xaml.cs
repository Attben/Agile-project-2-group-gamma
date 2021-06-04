using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        public static StorageFile HighscoreFile;

        public Highscore()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ReadHighscoresFromFile();
        }

        private async void ReadHighscoresFromFile()
        {
            try
            {
                //_linesFromHighscoreFile = System.IO.File.ReadAllLines(location);
                IList<string> linesFromHighscoreFile = await FileIO.ReadLinesAsync(HighscoreFile);

                //TODO: Improve text formatting
                StringBuilder names = new StringBuilder();
                StringBuilder scores = new StringBuilder();
                for (int n = 0; n < linesFromHighscoreFile.Count; n += 2)
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
                    names.Append(linesFromHighscoreFile[n] + "\n");
                    scores.Append(linesFromHighscoreFile[n + 1] + "\n");
                }
                HighscoreNames.Text = names.ToString();
                HighscoreScores.Text = scores.ToString();
            }
            catch (IOException)
            {
                ErrorPopup("Error: Couldn't read from highscore file.");
            }
        }

        public static async void AddHighscores(List<Player> players)
        {
            try
            {
                //Read current highscores
                IList<string> lines = await FileIO.ReadLinesAsync(HighscoreFile);
                var nameScorePairs = new List<Tuple<string, int>>();
                for (int n = 0; n < lines.Count; n += 2)
                {
                    nameScorePairs.Add(new Tuple<string, int>(lines[n], int.Parse(lines[n + 1])));
                }

                //Compare incoming player scores to current highscores

                //Add()ing directly to nameScorePairs in the loop crashes the program because it
                //modifies the list while it's being enumerated. Using temp storage as a workaround.
                var newHighscoreTempStorage = new List<Tuple<string, int>>();

                foreach (Player p in players)
                {
                    //Highscore list will be top 10 scores.
                    if (nameScorePairs.Count + newHighscoreTempStorage.Count < 10)
                    {
                        newHighscoreTempStorage.Add(new Tuple<string, int>(await NameEntryDialog(p), p.Score));
                    }
                    else
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
                }
                //Add temp values to highscore list
                foreach (var nameScorePair in newHighscoreTempStorage)
                {
                    nameScorePairs.Add(nameScorePair);
                }

                //Sort the list by Item2 (score), descending.
                nameScorePairs.Sort((lhs, rhs) => rhs.Item2.CompareTo(lhs.Item2));

                //Write top 10 to highscore
                StringBuilder outputText = new StringBuilder();
                for(int n = 0; n < nameScorePairs.Count && n < 10; ++n)
                {
                    outputText.AppendLine(nameScorePairs[n].Item1);
                    outputText.AppendLine(nameScorePairs[n].Item2.ToString());
                }
                await FileIO.WriteTextAsync(HighscoreFile, outputText.ToString());
            }
            catch (IOException)
            {
                ErrorPopup("Error: Couldn't write to highscore file.");
            }
        }

        public static async void ErrorPopup(string message)
        {
            await new ContentDialog()
            {
                Title = message,
                PrimaryButtonText = "Ok",
                IsSecondaryButtonEnabled = false
            }.ShowAsync();
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
