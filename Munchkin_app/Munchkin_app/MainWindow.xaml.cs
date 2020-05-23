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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Munckin_DAL;

namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

      
        private void btn_info_Click(object sender, RoutedEventArgs e)
        {
            //InfoWindow infoWindow = new InfoWindow();
            //infoWindow.Show();
            //this.Hide();

            Kaart testmonster = DatabaseOperations.OphalenKaartViaId(11);
            UseCardWindow useCardWindow = new UseCardWindow(1, testmonster);
            useCardWindow.Show();
            this.Close();

        }

       
        private void btn_startNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow newGameWindow = new NewGameWindow();
            newGameWindow.Show();
            this.Hide();
        }

        private void btn_startTournament_Click(object sender, RoutedEventArgs e)
        {
            StartTournamentWindow startTournamentWindow = new StartTournamentWindow();
            startTournamentWindow.Show();
            this.Hide();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //Application.Current.Shutdown();
        }
    }
}
