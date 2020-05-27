using Munckin_DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for Fase2.xaml
    /// </summary>
    public partial class Fase2 : Window
    {
        public Fase2()
        {
            InitializeComponent();
            //this is to change active player 
            //GlobalVariables.indexer += 1;
            //GlobalVariables.actieveSpeler = GlobalVariables.wedstrijd_Spelers[GlobalVariables.indexer];
            this.WindowState = WindowState.Maximized;
        }
        List<Kaarten_Stapel> handkaarten_stapels = new List<Kaarten_Stapel>();
        List<Kaarten_Stapel> veldkaarten_stapels = new List<Kaarten_Stapel>();
        Stapel veldkaarten = new Stapel();
        Stapel aflegstapelKerkerkaarten = new Stapel();
        Stapel aflegstapelSchatkaarten = new Stapel();
        List<Kaarten_Stapel> trekstapelKerkerkaartenstapel = new List<Kaarten_Stapel>();
        Kaart monster = new Kaart();
        Wedstrijd_Speler speler = new Wedstrijd_Speler();
        Wedstrijd_Speler helper = new Wedstrijd_Speler();
        List<Wedstrijd_Speler> lijstWedstrijd_Spelers = new List<Wedstrijd_Speler>();
        bool showCards = true;
        bool ikvlucht = new Boolean();
        bool isGeholpen = new Boolean();
        int AantalKeerGetrokken = 0;
        int AantalKeerGevochten = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
            aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Schatkaarten_Aflegstapel_Id);
            trekstapelKerkerkaartenstapel = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.wedstrijd.Kerkerkaarten_Trekstapel_Id);
            lblSpeler.Content = GlobalVariables.actieveSpeler.Speler.Naam;
            speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(GlobalVariables.actieveSpeler.Id);
            lijstWedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
            LabelsVeranderen();
            ShowHandkaarten();
            ShowVeldkaarten();
        }
        private void ShowHandkaarten()
        {
            handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);

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
            veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
            for (int i = 0; i < 13; i++)
            {
                Image img = this.FindName("imgVeld" + i) as Image;
                img.Visibility = Visibility.Collapsed;

            }
            for (int i = 0; i < veldkaarten_stapels.Count(); i++)
            {
                Image img = this.FindName("imgVeld" + i) as Image;
                string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                img.Visibility = Visibility.Visible;
                Image tooltip = this.FindName("toolTipVeld" + i) as Image;
                tooltip.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                Button btn = this.FindName("btnVeld" + i) as Button;
                btn.Tag = veldkaarten_stapels[i];
            }
        }






        private void btnHand0_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void btnHand0_Click(object sender, RoutedEventArgs e)
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
                    else if (kaarten_Stapel.Kaart.Type_id == 3 && AantalKeerGetrokken == 1)
                    {

                        monster = kaarten_Stapel.Kaart;
                        DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel);
                        string path = monster.Afbeelding;
                        imgGetrokkenKaart.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                        imgGetrokkenKaart.Visibility = Visibility.Visible;

                        toolTipGetrokkenKaart0.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                        handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                        veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
                        ShowVeldkaarten();
                        ShowHandkaarten();
                        LabelsVeranderen();
                        AantalKeerGetrokken += 1;
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

        private void LabelsVeranderen()
        {
            speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(GlobalVariables.actieveSpeler.Id);
            lblLevel.Content = $"Level: {speler.Level}";
            lblGevechtsBonus.Content = $"Gevechts Bonus: {speler.Gevechtsbonus}";
            lblRas.Content = $"Ras: {speler.Ras}";
            lblTijdelijkeBonus.Content = $"Tijdelijke Bonus: {speler.Tijdelijke_Bonus}";
            lblVluchtsBonus.Content = $"Vlucht Bonus: {speler.Vluchtbonus}";

        }

        private void btnVervloeking_Click(object sender, RoutedEventArgs e)
        {

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

        private void btnHand1_1_Click(object sender, RoutedEventArgs e)
        {

            string path = trekstapelKerkerkaartenstapel[AantalKeerGetrokken].Kaart.Afbeelding;
            if (AantalKeerGetrokken == 0)
            {



                if (trekstapelKerkerkaartenstapel[AantalKeerGetrokken].Kaart.Type_id == 3)//kijken of het een monster is
                {
                    monster = trekstapelKerkerkaartenstapel[AantalKeerGetrokken].Kaart;
                    AantalKeerGetrokken += 2;

                    imgGetrokkenKaart.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                    imgGetrokkenKaart.Visibility = Visibility.Visible;
                    lblTijdelijkeBonusMonster.Visibility = Visibility.Visible;
                    lblTijdelijkeBonusMonster.Content = "Tijdelijke bonus: " + monster.Kerkerkaart.Tijdelijke_Bonus;
                    //monster.Kerkerkaart.Tijdelijke_Bonus
                }
                else if (trekstapelKerkerkaartenstapel[AantalKeerGetrokken].Kaart.Type_id == 4)
                {
                    MessageBox.Show(trekstapelKerkerkaartenstapel[AantalKeerGetrokken].Kaart.KrijgVervloeking(speler));
                    AantalKeerGetrokken += 1;
                }
                else
                {
                    trekstapelKerkerkaartenstapel[AantalKeerGetrokken].KaartVanStapelWisselen(speler.Stapel_Handkaarten);
                    AantalKeerGetrokken += 1;
                }
            }
            else if (AantalKeerGetrokken == 1)
            {
                trekstapelKerkerkaartenstapel[AantalKeerGetrokken].KaartVanStapelWisselen(speler.Stapel_Handkaarten);
                AantalKeerGetrokken += 1;
            }
            ShowHandkaarten();
            LabelsVeranderen();
        }

        private void Dobbelsteen_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)lblGedobbeld.Content.ToString()))
            {
                int aantalgedobbeld = Roldobbelsteen();
                lblGedobbeld.Content = aantalgedobbeld;
                MessageBox.Show(monster.VluchtMonster(speler, aantalgedobbeld));
                monster = null;
                imgGetrokkenKaart.Visibility = Visibility.Hidden;
                lblTijdelijkeBonusMonster.Visibility = Visibility.Hidden;
                StPDobbelsteen.Visibility = Visibility.Hidden;
                ShowHandkaarten();
                LabelsVeranderen();
                ShowVeldkaarten();
            }
            else
            {
                MessageBox.Show("u heeft al gedobbeld");
            }

        }
        private int Roldobbelsteen()
        {
            Random Dobbelsteen = new Random();
            int worp = Dobbelsteen.Next(1, 7);
            return worp;
        }

        private void btnVechten_Click(object sender, RoutedEventArgs e)
        {
            if (!isGeholpen)
            {


                if (!ikvlucht)
                {
                    if (monster.Type_id == 3)
                    {
                        if (AantalKeerGevochten == 0)
                        {
                            var resultaat = monster.VechtMonster(speler).ToString();
                            if (resultaat == "Je kan niet winnen tegen " + monster.Naam.ToString())
                            {
                                MessageBox.Show(resultaat);
                            }
                            else
                            {
                                MessageBox.Show(resultaat);
                                imgGetrokkenKaart.Visibility = Visibility.Hidden;
                                monster = new Kaart();
                                AantalKeerGevochten += 1;
                            }

                        }
                        else
                        {
                            MessageBox.Show("u heeft al gevochten tegen een monster");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Er is geen monster om tegen te vechten");
                    }
                    LabelsVeranderen();
                    ShowHandkaarten();
                }
                else
                {
                    MessageBox.Show("je kan niet vechten van iets dan je van gevlucht bent");
                }
            }
            else
            {
                MessageBox.Show("Er is geen monster om tegen te vechten");
            }
        }
        private void btnVluchten_Click(object sender, RoutedEventArgs e)
        {

            if (!isGeholpen)
            {


                if (monster.Type_id == 3)
                {
                    var resultaat = monster.VluchtMonster(speler, 10);
                    if (resultaat == "Je kan winnen tegen " + monster.Naam)
                    {
                        MessageBox.Show("Je kan winnen van dit monster");
                    }
                    else
                    {
                        ikvlucht = true;
                        StPDobbelsteen.Visibility = Visibility.Visible;
                        MessageBox.Show("Rol om te kunne vluchten");

                        LabelsVeranderen();
                        ShowHandkaarten();
                        ShowVeldkaarten();
                    }
                }
                else
                {
                    MessageBox.Show("Er is niets om van te vluchten");
                }
            }
            else
            {
                MessageBox.Show("Er is niets om van te vluchten");
            }

        }

        private void btnVraagHulp_Click(object sender, RoutedEventArgs e)
        {
            if (!isGeholpen)
            {
                if (monster.Type_id == 3)
                {
                    foreach (Wedstrijd_Speler wedstrijd_Speler in lijstWedstrijd_Spelers)
                    {
                        if (!isGeholpen)
                        {
                            helper = wedstrijd_Speler;

                            if (helper.Id != speler.Id)
                            {

                                MessageBoxResult result = MessageBox.Show($"wil je dat {helper} je helpt", "Vraag om hulp", MessageBoxButton.YesNo);
                                switch (result)
                                {
                                    case MessageBoxResult.Yes:
                                        VechtenMetHulp();
                                        break;
                                    default:
                                        isGeholpen = false;
                                        break;
                                }
                            }
                        }

                    }
 
                }
                else
                {
                    MessageBox.Show("Er is geen monster om tegen te vechten");
                }

            }
            else
            {
                MessageBox.Show("je bent al geholpen geweest");
            }



        }

        private void VechtenMetHulp()
        {
            if (isGeholpen)
            {

            }
            else
            {
                var resultaat = "Je kan niet winnen tegen " + monster.Naam;
                if (resultaat == monster.VechtMonster(speler, helper))
                {
                    isGeholpen = true;
                    MessageBox.Show(monster.VluchtMonster(speler, Roldobbelsteen()));
                    MessageBox.Show(monster.VluchtMonster(helper, Roldobbelsteen()));
                    monster = null;
                    imgGetrokkenKaart.Visibility = Visibility.Hidden;
                    lblTijdelijkeBonusMonster.Visibility = Visibility.Hidden;
                    ShowHandkaarten();
                    ShowVeldkaarten();
                    LabelsVeranderen();
                }

            }
        }
        private void btnGebruiksvoorwerpen_Click(object sender, RoutedEventArgs e)
        {
            if (ikvlucht)
            {
                MessageBox.Show("Je kan niets gebruiken als je gaat vluchten");
            }
            else
            {
                UseCardWindow useCardWindow = new UseCardWindow(monster);
                useCardWindow.Show();

            }
            LabelsVeranderen();
            ShowHandkaarten();
            ShowVeldkaarten();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (monster.Type_id == null)
            {
                
            }
            else if (monster.Type_id == 3)
            {
                lblTijdelijkeBonusMonster.Content = "Tijdelijke bonus: " + monster.Kerkerkaart.Tijdelijke_Bonus;
                lblTijdelijkeBonusMonster.Visibility = Visibility.Visible;
            }
            else
            {

            }

            LabelsVeranderen();
            ShowHandkaarten();
        }
    }
}
