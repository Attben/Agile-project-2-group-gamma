using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            //view.TryEnterFullScreenMode();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PlayMenu));
        }

        private void Scores_Click(object sender, RoutedEventArgs e)
        {
            //switch page
            //this.Frame.Navigate(typeof(@pagenamehere@));
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
