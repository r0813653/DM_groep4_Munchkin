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
    /// Interaction logic for SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        public SellWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        int totaal;
        List<Kaarten_Stapel> kaartenStapelActieveSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
        List<Kaarten_Stapel> schatkaartenActieveSpeler = new List<Kaarten_Stapel>();
        
        Wedstrijd_Speler wedstrijd_Speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(GlobalVariables.actieveSpeler.Id);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var kaarten_stapel in kaartenStapelActieveSpeler)
            {
                if (kaarten_stapel.Kaart.Schatkaart != null)
                {
                    schatkaartenActieveSpeler.Add(kaarten_stapel);
                }
            }

            if (schatkaartenActieveSpeler.Count <=0)
            {
                MessageBox.Show("Je hebt geen schatkaarten");
            }
            else
            {
                lbSpelerKaarten.DisplayMemberPath = "Kaart.Naam";
                lbSpelerKaarten.ItemsSource = schatkaartenActieveSpeler;
            }
            
        }

        private void lbSpelerKaarten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int waarde = 0;
            foreach (var item in lbSpelerKaarten.SelectedItems)
            {
                
                Kaarten_Stapel kaart = (Kaarten_Stapel)item;
                waarde += (int)kaart.Kaart.Schatkaart.Schatkaart_Waarde;
            }
            totaal = waarde;
            lblWaarde.Content = waarde.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lbSpelerKaarten.SelectedIndex ==-1)
            {
                MessageBox.Show("Je hebt nog geen kaart geselecteerd");
            }
            else
            {
                if (GlobalVariables.actieveSpeler.Ras.ToUpper() == "HALFLING")
                {
                    totaal = totaal * 2;
                    if (totaal >= 1000)
                    {
                        wedstrijd_Speler.Level += 1;
                        DatabaseOperations.AanpassenWedstrijd_Speler(wedstrijd_Speler);
                        foreach (var item in lbSpelerKaarten.SelectedItems)
                        {
                            DatabaseOperations.VerwijderenKaarten_Stapel((Kaarten_Stapel)item);
                        }
                        MessageBox.Show("je bent een level gestegen");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("de waarde van de kaarten is niet genoeg om te verkopen");
                    }
                }
                else if (totaal >= 1000)
                {
                    wedstrijd_Speler.Level += 1;
                    DatabaseOperations.AanpassenWedstrijd_Speler(wedstrijd_Speler);
                    foreach (var item in lbSpelerKaarten.SelectedItems)
                    {
                        DatabaseOperations.VerwijderenKaarten_Stapel((Kaarten_Stapel)item);
                    }
                    MessageBox.Show("je bent een level gestegen");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("je kan niet verkopen omdat je totale waarde kleiner is dan 1000");
                }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
