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
            GlobalVariables.wedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
            GlobalVariables.actieveSpeler = GlobalVariables.wedstrijd_Spelers[GlobalVariables.indexer];
        }
        Wedstrijd_Speler ActieveSpeler = new Wedstrijd_Speler();
        List<Wedstrijd_Speler> LijstSpelers = new List<Wedstrijd_Speler>();
        List<Kaarten_Stapel> SpelerHandKaarten = new List<Kaarten_Stapel>();
        List<Kaarten_Stapel> lijstSpelerVervloekingskaarten = new List<Kaarten_Stapel>();
        private void cmbKaartenSpeler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var kaart = (Kaarten_Stapel)cmbKaartenSpeler.SelectedItem;
            //string path = kaart.Kaart.Afbeelding;
            //imgInfoKaart.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string foutmelding = "";
            var doel = cmbKiesSpeler.SelectedItem;
            Kaart GeselecteerdeKaart = new Kaart();
            var KaartStapel = (Kaarten_Stapel)cmbKaartenSpeler.SelectedItem;
            if (cmbKaartenSpeler.SelectedIndex == -1)
            {
                foutmelding += "Kies een kaart om te spelen!\n";
            }
            else if (cmbKiesSpeler.SelectedIndex == -1)
            {
                foutmelding += "Kies een speler om de kaart op te gebruiken!\n";
            }
            else if (KaartStapel is Kaarten_Stapel)
            {
                GeselecteerdeKaart = DatabaseOperations.OphalenKaartViaId(KaartStapel.Kaart_Id);
            }



            if (string.IsNullOrEmpty(foutmelding))
            {
                Wedstrijd_Speler slachtoffer = doel as Wedstrijd_Speler;
                MessageBox.Show(GeselecteerdeKaart.SpeelKaart(ActieveSpeler, slachtoffer));
                this.Close();
            }
            else
            {
                MessageBox.Show(foutmelding);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int wedstrijdid = 1;
            LijstSpelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(wedstrijdid);
            //get actieve speler
            ActieveSpeler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(2);
            //Actieve Speler HandKaarten
            SpelerHandKaarten = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(ActieveSpeler.Handkaarten_Id);

            //combobox opvullen met spelers
            foreach (Wedstrijd_Speler wedstrijd_Speler in LijstSpelers)
            {

                if (wedstrijd_Speler.Id != ActieveSpeler.Id)
                {
                    cmbKiesSpeler.Items.Add(wedstrijd_Speler);
                }

            }

            


                Stapel handkaarten = DatabaseOperations.OphalenStapelViaId(ActieveSpeler.Handkaarten_Id);
                foreach (Kaarten_Stapel kaarten_Stapel in handkaarten.Kaarten_Stapels)
                {
                    string type = DatabaseOperations.OphalenType(kaarten_Stapel.Kaart.Type_id).Soort.ToUpper();
                    if (type.ToUpper().Contains("VERVLOEKING"))
                    {
                        //HandKaarten toevoegen aan combobox
                        lijstSpelerVervloekingskaarten.Add(kaarten_Stapel);
                    }
                }
            cmbKaartenSpeler.ItemsSource = lijstSpelerVervloekingskaarten;
            cmbKaartenSpeler.DisplayMemberPath = "Kaart.Naam";

        }
    }
}
