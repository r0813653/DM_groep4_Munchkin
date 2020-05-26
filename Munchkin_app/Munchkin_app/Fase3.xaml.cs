using Munckin_DAL;
using Munchkin_MODELS;
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
    /// Interaction logic for Fase3.xaml
    /// </summary>
    public partial class Fase3 : Window
    {
        public Fase3()
        {
            InitializeComponent();
            //Enkel nodig om te testen zonder door fase1 te gaan
            GlobalVariables.wedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
            GlobalVariables.actieveSpeler = GlobalVariables.wedstrijd_Spelers[GlobalVariables.indexer];
        }

        List<Kaarten_Stapel> handkaarten_stapels = new List<Kaarten_Stapel>();
        List<Kaarten_Stapel> veldkaarten_stapels = new List<Kaarten_Stapel>();
        Stapel veldkaarten = new Stapel();
        Stapel aflegstapelKerkerkaarten = new Stapel();
        Stapel aflegstapelSchatkaarten = new Stapel();
        Wedstrijd_Speler speler = new Wedstrijd_Speler();
        bool showCards = true;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
            veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
            aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Schatkaarten_Aflegstapel_Id);
            lblSpeler.Content = GlobalVariables.actieveSpeler.Speler.Naam;
            speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(GlobalVariables.actieveSpeler.Id);
            LabelsVeranderen();
            ShowHandkaarten();
            ShowVeldkaarten();

        }

        private void ShowHandkaarten()
        {
            for (int i = 0; i < 13; i++)
            {
                Image img = this.FindName("imgHand" + i) as Image;
                img.Visibility = Visibility.Collapsed;

            }
            for (int i = 0; i < handkaarten_stapels.Count(); i++)
            {
                Image img = this.FindName("imgHand" + i) as Image;
                string path = handkaarten_stapels[i].Kaart.Afbeelding;
                img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                img.Visibility = Visibility.Visible;
                Image tooltip = this.FindName("toolTipHand" + i) as Image;
                tooltip.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                Button btn = this.FindName("btnHand" + i) as Button;
                btn.Tag = handkaarten_stapels[i];
            }
        }

        private void HideHandkaarten()
        {
            for (int i = 0; i < 13; i++)
            {
                Image img = this.FindName("imgHand" + i) as Image;
                img.Visibility = Visibility.Collapsed;

            }
            for (int i = 0; i < handkaarten_stapels.Count(); i++)
            {
                string path = "";
                Image img = this.FindName("imgHand" + i) as Image;
                if (handkaarten_stapels[i].Kaart.Kerkerkaart == null)
                {
                    path = "images/Schatkaart.png";
                }
                else
                {
                    path = "images/Kerkerkaart.png";
                }
                img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                img.Visibility = Visibility.Visible;
                Image tooltip = this.FindName("toolTipHand" + i) as Image;
                tooltip.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                Button btn = this.FindName("btnHand" + i) as Button;
                btn.Tag = handkaarten_stapels[i];
            }
        }

        private void ShowVeldkaarten()
        {
            Image img1 = this.FindName("imgHoofddeksel") as Image;
            img1.Visibility = Visibility.Collapsed;
            img1 = this.FindName("imgHarnas") as Image;
            img1.Visibility = Visibility.Collapsed;
            img1 = this.FindName("imgSchoeisel") as Image;
            img1.Visibility = Visibility.Collapsed;
            img1 = this.FindName("imgHand1_2") as Image;
            img1.Visibility = Visibility.Collapsed;
            img1 = this.FindName("imgHand1_1") as Image;
            img1.Visibility = Visibility.Collapsed;
            img1 = this.FindName("imgRas") as Image;
            img1.Visibility = Visibility.Collapsed;
            string hand = "Hand1_1";
            int extra = 0;
            for (int i = 0; i < 6; i++)
            {
                Image img = this.FindName("imgExtra" + i) as Image;
                img.Visibility = Visibility.Collapsed;

            }
            for (int i = 0; i < veldkaarten_stapels.Count(); i++)
            {
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "RAS")
                {
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    imgRas.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    imgRas.Visibility = Visibility.Visible;
                    btnRas.Tag = veldkaarten_stapels[i];
                    toolTipRas.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "HOOFDDEKSEL")
                {
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    imgHoofddeksel.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    imgHoofddeksel.Visibility = Visibility.Visible;
                    toolTipHoofddeksel.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    btnHoofddeksel.Tag = veldkaarten_stapels[i];
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "HARNAS")
                {
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    imgHarnas.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    imgHarnas.Visibility = Visibility.Visible;
                    toolTipHarnas.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    btnHarnas.Tag = veldkaarten_stapels[i];
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "SCHOEISEL")
                {
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    imgSchoeisel.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    imgSchoeisel.Visibility = Visibility.Visible;
                    toolTipSchoeisel.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    btnSchoeisel.Tag = veldkaarten_stapels[i];
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "1HAND" || veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "2HANDEN")
                {
                    Image img = this.FindName("img" + hand) as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                    Image tooltip = this.FindName("toolTip" + hand) as Image;
                    tooltip.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    Button btn = this.FindName("btn" + hand) as Button;
                    btn.Tag = veldkaarten_stapels[i];
                    hand = "Hand1_2";
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "EXTRA")
                {
                    Image img = this.FindName("imgExtra" + extra) as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                    Image tooltip = this.FindName("toolTipExtra" + extra) as Image;
                    tooltip.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    Button btn = this.FindName("btnExtra" + extra) as Button;
                    btn.Tag = veldkaarten_stapels[i];
                    extra += 1;
                }
            }
        }

        private void btnKaart_ClickRight(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wil je deze kaart weggooien ?", "opgelet", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Kaarten_Stapel kaarten_Stapel = (Kaarten_Stapel)((Button)sender).Tag;
                    if (kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "RAS")
                    {
                        speler.Ras = "Mens";
                        if (speler.IsGeldig())
                        {
                            int ok = DatabaseOperations.AanpassenWedstrijd_Speler(speler);
                            if (ok <= 0)
                            {
                                MessageBox.Show("Je bent niet van ras kunnen veranderen");
                            }
                        }
                        else
                        {
                            MessageBox.Show(speler.Error);
                        }

                    }
                    if (kaarten_Stapel.Kaart.Schatkaart == null)
                    {
                        kaarten_Stapel.KaartVanStapelWisselen(aflegstapelSchatkaarten);
                    }
                    else
                    {
                        kaarten_Stapel.KaartVanStapelWisselen(aflegstapelKerkerkaarten);
                    }
                    
                    handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                    veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
                    veldkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
                    speler.HerberekenBonussenVeldkaarten(veldkaarten);
                    LabelsVeranderen();
                    ShowVeldkaarten();
                    ShowHandkaarten();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;

            }
        }

        private void btnVervloeking_Click(object sender, RoutedEventArgs e)
        {
            CurseWindow curseWindow = new CurseWindow();
            curseWindow.Show();
        }

        private void btnKaart_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wil je deze kaart aan je veldkaarten toevoegen of gebruiken?", "opgelet", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Kaarten_Stapel kaarten_Stapel = (Kaarten_Stapel)((Button)sender).Tag;
                    if (kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "RAS" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "HOOFDDEKSEL" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "SCHOEISEL" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "HARNAS" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "EXTRA" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "1HAND" || kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "2HANDEN")
                    {
                        string message = kaarten_Stapel.Kaart.SpeelKaart(speler);
                        handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                        veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
                        ShowVeldkaarten();
                        ShowHandkaarten();
                        LabelsVeranderen();
                        MessageBox.Show(message);
                    }
                    else if (kaarten_Stapel.Kaart.Type.Soort.ToUpper() == "GEBRUIKSKAARTEN" && kaarten_Stapel.Kaart.Wanneer_Bruikbaar.ToUpper() == "ALTIJD")
                    {
                        string message = kaarten_Stapel.Kaart.SpeelKaart(speler);
                        handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                        veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
                        ShowVeldkaarten();
                        ShowHandkaarten();
                        LabelsVeranderen();
                        MessageBox.Show(message);
                    }
                    else
                    {
                        MessageBox.Show("Je kan deze kaart niet op deze manier gebruiken");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;

            }
        }

        private void btnRuilen_Click(object sender, RoutedEventArgs e)
        {
            TradeWindow tradeWindow = new TradeWindow();
            tradeWindow.Show();
        }

        private void btnVerkopen_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new SellWindow();
            sellWindow.Show();
        }

        private void btnSpelers_Click(object sender, RoutedEventArgs e)
        {
            ViewPlayerWindow viewPlayerWindow = new ViewPlayerWindow();
            viewPlayerWindow.Show();
        }

        private void btnVerstopKaarten_Click(object sender, RoutedEventArgs e)
        {
            if (showCards == true)
            {
                HideHandkaarten();
                showCards = false;
            }
            else
            {
                ShowHandkaarten();
                showCards = true;
            }
        }

        private void LabelsVeranderen()
        {
            lblLevel.Content = $"Level: {speler.Level}";
            lblGevechtsBonus.Content = $"Gevechts Bonus: {speler.Gevechtsbonus}";
            lblRas.Content = $"Ras: {speler.Ras}";
            lblTijdelijkeBonus.Content = $"Tijdelijke Bonus: {speler.Tijdelijke_Bonus}";
            lblVluchtsBonus.Content = $"Vlucht Bonus: {speler.Vluchtbonus}";
        }

        private void btnEindeBeurt_Click(object sender, RoutedEventArgs e)
        {
            int aantalKaarten = 5;
            if (speler.Ras.ToUpper() == "DWERG")
            {
                aantalKaarten = 6;
            }
            if (handkaarten_stapels.Count() <= aantalKaarten)
            {
                List<Wedstrijd_Speler> spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
                foreach (Wedstrijd_Speler speler in spelers)
                {
                    speler.Tijdelijke_Bonus = 0;
                    if (speler.IsGeldig())
                    {
                        int ok = DatabaseOperations.AanpassenWedstrijd_Speler(speler);
                        if (ok <= 0)
                        {
                            MessageBox.Show("Je bent niet van ras kunnen veranderen");
                        }
                    }
                    else
                    {
                        MessageBox.Show(speler.Error);
                    }
                }
                GlobalVariables.indexer += 1;
                if (GlobalVariables.indexer >= GlobalVariables.wedstrijd_Spelers.Count())
                {
                    GlobalVariables.indexer = 0;
                }
                
                Stapel trekstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Kerkerkaarten_Trekstapel_Id);
                Stapel trekstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Schatkaarten_Trekstapel_Id);
                trekstapelKerkerkaarten.kerkerkaartTrekstapelsChecken();
                trekstapelSchatkaarten.schatkaartTrekstapelChecken();

                Fase1 fase1 = new Fase1();
                fase1.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show($"Je mag maar {aantalKaarten} in je hand hebben, gelieve er eerst weg te doen");
            }
        }
    }
}
