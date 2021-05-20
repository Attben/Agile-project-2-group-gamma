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
    public sealed partial class PlayMenu : Page
    {
        public PlayMenu()
        {
            this.InitializeComponent();
        }

        private void SliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double text = this.Slider.Value;
            string ok = "0";
            this.amountOfPlayersBox.Text = text.ToString(ok);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            string ok = "0";
            double text = this.Slider.Value;
            Game game = new Game(int.Parse(text.ToString(ok)));
            //game.AddPlayers(int.Parse(text.ToString(ok)));

            //switch page
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
