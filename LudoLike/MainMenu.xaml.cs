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
    /// Main Menu for application
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        private static bool _gameAssetsLoaded = false;

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

        /// <summary>
        /// creates a highscore file, opens exisitng file
        /// </summary>
        public async void LoadHighscoreLocation()
        {
            StorageFolder AppDataFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
                "LudoLike",
                CreationCollisionOption.OpenIfExists);
            Classes.Highscore.HighscoreFile = await AppDataFolder.CreateFileAsync(
                "Highscores.txt",
                CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Changes the player turn to the one next in line.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (_gameAssetsLoaded)
            {
                GameBoard.DisposeGameAssets();
                _gameAssetsLoaded = false;
            }
        }

        /// <summary>
        /// starts the game, navigates to PlayMenu for additional game settings
        /// </summary>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _gameAssetsLoaded = true;
            this.Frame.Navigate(typeof(PlayMenu));
        }

        /// <summary>
        /// navigates to Highscore Page for current Highscores
        /// </summary>
        private void Scores_Click(object sender, RoutedEventArgs e)
        {
            //switch page
            this.Frame.Navigate(typeof(Classes.Highscore));
        }

        /// <summary>
        /// exits the application
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
