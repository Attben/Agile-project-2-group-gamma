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
    public sealed partial class gameover : Page
    {
        List<Player> list;

        public gameover()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            list = (List<Player>)e.Parameter;
            PopulateList();
        }

        private void PopulateList()
        {
            var blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);

            foreach (Player player in list)//changes to the second list scorelist
            {
                PlayerColors color = player.PlayerColor;
                int score = player.Score;
                ListViewItem playername = new ListViewItem();
                ListViewItem scoreitem = new ListViewItem();
                playername.Content = color.ToString();
                scoreitem.Content = score;

                playername.Foreground = blackBrush;
                scoreitem.Foreground = blackBrush;

                playername.FontSize = 90;
                scoreitem.FontSize = 90;

                scoreitem.HorizontalContentAlignment = HorizontalAlignment.Right;

                playerlist.Items.Add(playername);
                scorelist.Items.Add(scoreitem);
            }
        }

        private void testing()
        {
            var blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);

            for (int i = 0; i < 4; i++)
            {
                //ListBoxItem playername = new ListBoxItem();
                //ListBoxItem scoreitem = new ListBoxItem();

                ListViewItem playername = new ListViewItem();
                ListViewItem scoreitem = new ListViewItem();

                playername.Content = "namnHello";
                scoreitem.Content = 56;

                playername.Foreground = blackBrush;
                scoreitem.Foreground = blackBrush;

                playername.FontSize = 90;
                scoreitem.FontSize = 90;//change?

                scoreitem.HorizontalContentAlignment = HorizontalAlignment.Right;

                playerlist.Items.Add(playername);
                scorelist.Items.Add(scoreitem);
            }
            
        }


    }
}
