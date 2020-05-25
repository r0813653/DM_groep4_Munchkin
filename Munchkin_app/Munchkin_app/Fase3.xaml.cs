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
        List<Kaarten_Stapel> handkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(671);
        List<Kaarten_Stapel> veldkaarten_stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(672);
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
            string hand = "imgHand1_1";
            int extra = 0;
            for (int i = 0; i < veldkaarten_stapels.Count(); i++)
            {
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "HOOFDDEKSEL")
                {
                    Image img = this.FindName("imgHoofddeksel") as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "HARNAS")
                {
                    Image img = this.FindName("imgHarnas") as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "SCHOEISEL")
                {
                    Image img = this.FindName("imgSchoeisel") as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "1HAND" || veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "2HANDEN")
                {
                    Image img = this.FindName(hand) as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                    hand = "imgHand1_2";
                }
                if (veldkaarten_stapels[i].Kaart.Type.Soort.ToUpper().ToString() == "EXTRA")
                {
                    Image img = this.FindName("imgExtra" + extra) as Image;
                    string path = veldkaarten_stapels[i].Kaart.Afbeelding;
                    img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
                    img.Visibility = Visibility.Visible;
                    extra += 1;
                }
            }
        }

        //private void ShowVeldkaarten()
        //{
        //    for (int i = 0; i < 13; i++)
        //    {
        //        Image img = this.FindName("imgVeld" + i) as Image;
        //        img.Visibility = Visibility.Collapsed;
        //    }
        //    for (int i = 0; i < veldkaarten_stapels.Count(); i++)
        //    {
        //        Image img = this.FindName("imgVeld" + i) as Image;
        //        string path = veldkaarten_stapels[i].Kaart.Afbeelding;
        //        img.Source = new BitmapImage(new Uri(@path, UriKind.Relative));
        //        img.Visibility = Visibility.Visible;
        //    }
        //}
    }
}
