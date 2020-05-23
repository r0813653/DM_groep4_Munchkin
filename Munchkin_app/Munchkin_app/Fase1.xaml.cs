using Munckin_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
using Munchkin_app;


namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for Fase1.xaml
    /// </summary>
    public partial class Fase1 : Window
    {
        


        List<Kaarten_Stapel> handkaartenSpeler = new List<Kaarten_Stapel>();
        public Fase1()
        {
            InitializeComponent();
           
            GlobalVariables.wedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(GlobalVariables.WedstrijdId);
            GlobalVariables.actieveSpeler = GlobalVariables.wedstrijd_Spelers[GlobalVariables.indexer];
            lblSpeler.Content = GlobalVariables.actieveSpeler.Speler.Naam;
            lblLevel.Content = GlobalVariables.actieveSpeler.Level;
            lblRas.Content = GlobalVariables.actieveSpeler.Ras;

            List<Image> images = new List<Image>();

            handkaartenSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
            foreach (var handkaart in handkaartenSpeler)
            {
                Image picture = new Image();            
                Uri uri = new Uri(handkaart.Kaart.Afbeelding, UriKind.Relative);
                picture.Source = new BitmapImage(uri);

                images.Add(picture);
                
            }
            handkaartenGrid.ItemsSource = images;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
