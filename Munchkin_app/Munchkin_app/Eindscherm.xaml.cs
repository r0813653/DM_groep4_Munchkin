using Munckin_DAL;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Eindscherm.xaml
    /// </summary>
    public partial class Eindscherm : Window
    {
        public Eindscherm()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewGameWindow newGameWindow = new NewGameWindow();
            newGameWindow.Show();
            this.Close();
        }
        List<Wedstrijd_Speler> wedstrijd_Spelers = new List<Wedstrijd_Speler>();
        int wedstrijdId = GlobalVariables.WedstrijdId;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Wedstrijd_Speler> winnaars = new List<Wedstrijd_Speler>();
            wedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(wedstrijdId);
            List<Wedstrijd_Speler> orderedList = wedstrijd_Spelers.OrderByDescending(x => x.Level).ToList();
            foreach (Wedstrijd_Speler speler in orderedList)
            {
                if (speler.Level >= 10)
                {
                    winnaars.Add(speler);
                }
            }
            if (winnaars.Count() == 1)
            {
                lblWinnaar.Content = $"{winnaars[0].Speler.Naam} heeft gewonnen!";
            }
            else
            {
                lblWinnaar.Content = $"{winnaars[0].Speler.Naam} en {winnaars[1].Speler.Naam} hebben gewonnen!";
            }
            datagridScores.ItemsSource = orderedList;
        }
    }
}
