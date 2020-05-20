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
using Munckin_DAL;
namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for CurseWindow.xaml
    /// </summary>
    public partial class CurseWindow : Window
    {
        public CurseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int wedstrijdid = 1;
            //spelerlijst aanmaken
            List<Wedstrijd_Speler> LijstSpelers = new List<Wedstrijd_Speler>();
            //spelerlijst opvullen
            LijstSpelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(wedstrijdid);
            
            cmbKiesSpeler.ItemsSource = LijstSpelers;
            cmbKiesSpeler.DisplayMemberPath = "LijstSpelers.Naam";
            //combobox opvullen met spelers
            foreach (Wedstrijd_Speler wedstrijd_Speler in LijstSpelers)
            {
                //Console.WriteLine(wedstrijd_Speler);
                cmbKiesSpeler.Items.Add(wedstrijd_Speler.Speler.Naam);

            } 
            
        }
        

    }
}
