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
        List<Kaarten_Stapel> veldkaartenSpeler = new List<Kaarten_Stapel>();
        List<Image> imagesHandkaartenSpeler = new List<Image>();
        List<Image> imagesVeldkaartenSpeler = new List<Image>();
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

            

            handkaartenSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
            foreach (var handkaart in handkaartenSpeler)
            {
                Image picture = new Image();            
                Uri uri = new Uri(handkaart.Kaart.Afbeelding, UriKind.Relative);
                picture.Source = new BitmapImage(uri);

                imagesHandkaartenSpeler.Add(picture);
                
            }
            handkaartenGrid.ItemsSource = imagesHandkaartenSpeler;

            veldkaartenSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Veldkaarten_Id);
            foreach (var veldkaart in veldkaartenSpeler)
            {
                Image picture = new Image();
                Uri uri = new Uri(veldkaart.Kaart.Afbeelding, UriKind.Relative);
                picture.Source = new BitmapImage(uri);

                imagesVeldkaartenSpeler.Add(picture);

            }
            veldkaartenGrid.ItemsSource = imagesHandkaartenSpeler;
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
                                       
                                        handkaart.KaartVanStapelWisselen(GlobalVariables.wedstrijd.Kerkerkaarten_Aflegstapel);
                                        handkaartenSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                                        imagesHandkaartenSpeler.Clear();
                                        foreach (var kaart in handkaartenSpeler)
                                        {
                                            Image foto = new Image();
                                            Uri uri = new Uri(handkaart.Kaart.Afbeelding, UriKind.Relative);
                                            picture.Source = new BitmapImage(uri);

                                            imagesHandkaartenSpeler.Add(foto);

                                        }
                                        handkaartenGrid.ItemsSource = imagesHandkaartenSpeler;
                                    }
                                }

                                foreach (var schatkaart in GlobalVariables.alleSchatkaarten)
                                {
                                    if (schatkaart.Id == handkaart.Kaart.Id)
                                    {
                                       
                                        handkaart.KaartVanStapelWisselen(GlobalVariables.wedstrijd.Schatkaarten_Aflegstapel);
                                        handkaartenSpeler = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                                        imagesHandkaartenSpeler.Clear();
                                        foreach (var kaart in handkaartenSpeler)
                                        {
                                            Image foto1 = new Image();
                                            Uri uri = new Uri(handkaart.Kaart.Afbeelding, UriKind.Relative);
                                            picture.Source = new BitmapImage(uri);

                                            imagesHandkaartenSpeler.Add(foto1);

                                        }
                                        handkaartenGrid.ItemsSource = imagesHandkaartenSpeler;
                                    }
                                }
                                
                               
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

        private void veldkaartenGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnVolgendeFase_Click(object sender, RoutedEventArgs e)
        {
            Fase2 fase = new Fase2();
            fase.Show();
            this.Close();
        }
    }
}
