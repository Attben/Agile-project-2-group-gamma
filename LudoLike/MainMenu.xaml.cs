using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
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
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Maximized;
            this.InitializeComponent();
            var view = ApplicationView.GetForCurrentView();
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            LoadHighscoreLocation();
            //view.TryEnterFullScreenMode();
        }

        public async void LoadHighscoreLocation()
        {
            StorageFolder AppDataFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
                "LudoLike",
                CreationCollisionOption.OpenIfExists);
            Classes.Highscore.HighscoreFile = await AppDataFolder.CreateFileAsync(
                "Highscores.txt",
                CreationCollisionOption.OpenIfExists);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PlayMenu));
        }

        private void Scores_Click(object sender, RoutedEventArgs e)
        {
            //switch page
            this.Frame.Navigate(typeof(Classes.Highscore));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //switch page
            //this.Frame.Navigate(typeof(@pagenamehere@));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
