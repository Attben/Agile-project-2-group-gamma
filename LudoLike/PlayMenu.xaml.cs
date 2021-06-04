﻿using System;
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

namespace LudoLike
{
    /// <summary>
    /// A page that is reached py pressing "Play" in the main menu.
    /// Lets the user choose the amount of players and start the game.
    /// </summary>
    public sealed partial class PlayMenu : Page
    {
        public PlayMenu()
        {
            this.InitializeComponent();
        }

        private void SliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            string numberFormatString = "0";
            this.amountOfPlayersBox.Text = _slider.Value.ToString(numberFormatString);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            //switch page
            this.Frame.Navigate(typeof(GameBoard), _slider.Value);
        }
    }
}
