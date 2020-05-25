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
    /// Interaction logic for Fase3.xaml
    /// </summary>
    public partial class Fase3 : Window
    {
        public Fase3()
        {
            InitializeComponent();
        }
        //List<Kaarten_Stapel> handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
        //List<Kaarten_Stapel> veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
        //Stapel aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Kerkerkaarten_Aflegstapel_Id);
        //Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Schatkaarten_Aflegstapel_Id);
        List<Kaarten_Stapel> handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(671);
        List<Kaarten_Stapel> veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(672);
        Stapel aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(3);
        Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(4);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void btnKaart_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wil je deze kaart weggooien ?", "opgelet", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Kaarten_Stapel kaarten_Stapel = (Kaarten_Stapel)((Button)sender).Tag;
                    if (kaarten_Stapel.Kaart.Kerkerkaart == null)
                    {
                        kaarten_Stapel.KaartVanStapelWisselen(aflegstapelSchatkaarten);
                    }
                    else
                    {
                        kaarten_Stapel.KaartVanStapelWisselen(aflegstapelKerkerkaarten);
                    }
                    handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(671);
                    veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(672);
                    ShowVeldkaarten();
                    ShowHandkaarten();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;

            }
        }
    }
}
