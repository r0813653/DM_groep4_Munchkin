using Munckin_DAL;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


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
            GlobalVariables.alleKerkerkaarten = DatabaseOperations.OphalenKerkerkaarten();
            GlobalVariables.alleSchatkaarten = DatabaseOperations.OphalenSchatkaarten();

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

        private void handkaartenGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handkaartenGrid.SelectedItem is Image picture)
            {
                MessageBoxResult result = MessageBox.Show("Wil je deze kaart weggooien ?", "opgelet", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        foreach (var handkaart in handkaartenSpeler)
                        {
                            if (picture.Source.ToString() == "pack://application:,,,/Munchkin_app;component/" + handkaart.Kaart.Afbeelding )
                            {
                                foreach (var kerkerkaart in GlobalVariables.alleKerkerkaarten)
                                {
                                    if (kerkerkaart.Id == handkaart.Kaart_Id)
                                    {
                                        MessageBox.Show("u made it so far voor handkaarten");
                                        handkaart.KaartVanStapelWisselen(GlobalVariables.wedstrijd.Kerkerkaarten_Trekstapel);
                                    }
                                }

                                foreach (var schatkaart in GlobalVariables.alleSchatkaarten)
                                {
                                    if (schatkaart.Id == handkaart.Kaart.Id)
                                    {
                                        MessageBox.Show("u made it so far voor schatkaarten");
                                        handkaart.KaartVanStapelWisselen(GlobalVariables.wedstrijd.Kerkerkaarten_Trekstapel);
                                    }
                                }
                                
                                //handkaart.KaartVanStapelWisselen(GlobalVariables.wedstrijd.)
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }


            }
        }
    }
}
