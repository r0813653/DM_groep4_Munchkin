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
    /// Interaction logic for ViewPlayerWindow.xaml
    /// </summary>
    public partial class ViewPlayerWindow : Window
    {
        public ViewPlayerWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }
        List<Kaarten_Stapel> veldkaarten_stapels = new List<Kaarten_Stapel>();
        Stapel veldkaarten = new Stapel();
        Wedstrijd_Speler speler = new Wedstrijd_Speler();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            List<Wedstrijd_Speler> lijstWedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
            cmbSpeler.ItemsSource = lijstWedstrijd_Spelers;
            cmbSpeler.DisplayMemberPath = "Speler.Naam";
        }
        private void ShowVeldkaarten()
        {
            veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
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

        private void cmbSpeler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            speler = (Wedstrijd_Speler)cmbSpeler.SelectedItem;
            veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(speler.Veldkaarten_Id);
            ShowVeldkaarten();
            LabelsVeranderen();
        }
        private void LabelsVeranderen()
        {
            lblLevel.Content = $"Level: {speler.Level}";
            lblGevechtsBonus.Content = $"Gevechts Bonus: {speler.Gevechtsbonus}";
            lblRas.Content = $"Ras: {speler.Ras}";
            lblTijdelijkeBonus.Content = $"Tijdelijke Bonus: {speler.Tijdelijke_Bonus}";
            lblVluchtsBonus.Content = $"Vlucht Bonus: {speler.Vluchtbonus}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
