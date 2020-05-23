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
    /// Interaction logic for UseCardWindow.xaml
    /// </summary>
    public partial class UseCardWindow : Window
    {
        int wedstrijd_Id;
        //monsterkaart op null zetten zodat je met een simpele if kan check of er een monster meegegeven is of niet
        Kaart monsterKaart = null;

        //wedstrijd id moet meegegeven worden om spelers te kunnen ophalen
        public UseCardWindow(int wedstrijdId)
        {
            InitializeComponent();
            wedstrijd_Id = wedstrijdId;
        }

        //als je in een gevecht zit moet er een monster meegegeven worden zodat daar ook kaarten op gespeeld kunnen worden
        public UseCardWindow(int wedstrijdId, Kaart monster)
        {
            InitializeComponent();
            wedstrijd_Id = wedstrijdId;
            monsterKaart = monster;
        }
        List<Kaarten_Stapel> lijstHandkaartenGebruikskaarten = new List<Kaarten_Stapel>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //spelers van deze wedstrij ophalen
            List<Wedstrijd_Speler> lijstWedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(wedstrijd_Id);
            //spelers toevoegen aan combobox gebruikers
            cmbGebruiker.ItemsSource = lijstWedstrijd_Spelers;
            cmbGebruiker.DisplayMemberPath = "Speler.Naam";
            //spelers + eventueel monster toevoegen aan combobox doel
            foreach (Wedstrijd_Speler speler in lijstWedstrijd_Spelers)
            {
                cmbDoel.Items.Add(speler);
            }
            if (monsterKaart != null)
            {
                cmbDoel.Items.Add(monsterKaart);
            }
        }

        private void cmbGebruiker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //als er een speler gekozen is ook de handkaarten van die speler in combobox steken
            cmbKaarten.IsEnabled = true;
            var speler = (Wedstrijd_Speler)cmbGebruiker.SelectedItem;
            Stapel handkaarten = DatabaseOperations.OphalenStapelViaId(speler.Handkaarten_Id);
            foreach (Kaarten_Stapel kaarten_Stapel in handkaarten.Kaarten_Stapels)
            {
                string type = DatabaseOperations.OphalenType(kaarten_Stapel.Kaart.Type_id).Soort.ToUpper();
                if (type.ToUpper().Contains("GEBRUIKSKAARTEN"))
                {
                    lijstHandkaartenGebruikskaarten.Add(kaarten_Stapel);
                }
            }
            cmbKaarten.ItemsSource = lijstHandkaartenGebruikskaarten;
            cmbKaarten.DisplayMemberPath = "Kaart.Naam";
        }

        private void cmbKaarten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var kaart = (Kaarten_Stapel)cmbKaarten.SelectedItem;
            string path = kaart.Kaart.Afbeelding;
            imgKaart.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
            imgKaart.Height = 400;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string foutmelding = "";
            Kaart kaart = new Kaart();
            var speler = (Wedstrijd_Speler)cmbGebruiker.SelectedItem;
            var kaartenStapel = (Kaarten_Stapel)cmbKaarten.SelectedItem;
            var doel = cmbDoel.SelectedItem;

            if (cmbGebruiker.SelectedIndex == -1)
            {
                foutmelding += "Gelieve een speler te selecteren die een kaart gebruikt\n";
            }
            else if (!(speler is Wedstrijd_Speler))
            {
                foutmelding += "De speler die je hebt aangeduid is geen geldige speler\n";
            }
            else if (cmbKaarten.SelectedIndex == -1)
            {
                foutmelding += "Gelieve een kaart te selecteren die je wil spelen\n";
            }
            else if (kaartenStapel is Kaarten_Stapel)
            {
                kaart = DatabaseOperations.OphalenKaartViaId(kaartenStapel.Kaart_Id);
            }
            else
            {
                foutmelding += "De kaart die je selecteerde is geen geldige kaart\n";
            }
            if (cmbDoel.SelectedIndex == -1)
            {
                foutmelding += "gelieve een doel voor je kaart te selecteren\n";
            }
            
            
            if (string.IsNullOrEmpty(foutmelding))
            {
                if (doel is Wedstrijd_Speler)
                {
                    Wedstrijd_Speler slachtoffer = doel as Wedstrijd_Speler;
                    if (slachtoffer.Equals(speler))
                    {
                        MessageBox.Show(kaart.SpeelKaart(speler));
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(kaart.SpeelKaart(speler, slachtoffer));
                        this.Close();
                    }
                }
                else if (doel is Kaart)
                {
                    Kaart monster = doel as Kaart;
                    MessageBox.Show(kaart.SpeelKaart(speler, monster));
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Het doel dat je hebt gekozen is geen geldig doel");
                }

            }
            else
            {
                MessageBox.Show(foutmelding);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
