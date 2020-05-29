using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for StartTournamentWindow.xaml
    /// </summary>
    public partial class StartTournamentWindow : Window
    {
        public StartTournamentWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        List<int> lijstAantalSpelers = new List<int>();
        
        private void btn_terugNaarMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int optie1 = 2;
            int optie2 = 4;
            int optie3 = 8;

            lijstAantalSpelers.Add(optie1);
            lijstAantalSpelers.Add(optie2);
            lijstAantalSpelers.Add(optie3);

            cmb_AantalSpelers.ItemsSource = lijstAantalSpelers;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Toernooi is nog niet beschikbaar");
        }
    }
}
