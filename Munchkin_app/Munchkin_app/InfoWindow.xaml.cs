using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void btn_terugNaarMainwindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
            this.Hide();
        }

        private void btn_spelregels_Click(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
            this.Hide();
        }

        private void btn_zoekFavorieteKaart_Click(object sender, RoutedEventArgs e)
        {
            SearchCard searchCard = new SearchCard();
            searchCard.Show();
            this.Hide();
        }

        private void btn_credits_Click(object sender, RoutedEventArgs e)
        {
            CreditsWindow creditsWindow = new CreditsWindow();
            creditsWindow.Show();
            this.Hide();
        }
    }
}
