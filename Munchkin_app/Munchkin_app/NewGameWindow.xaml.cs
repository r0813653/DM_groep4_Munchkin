using Munchkin_MODELS;
using Munckin_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window 
    {   

        public NewGameWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        List<TextBox> lijstNamenTextboxen = new List<TextBox>(); // lijst is nodig om achteraf gegevens op te vragen 
        List<RadioButton> lijstRadiobuttons = new List<RadioButton>();
        
        
        private void txt_aantalSpelers_KeyUp(object sender, KeyEventArgs e)
        {
            if (int.TryParse(txt_aantalSpelers.Text, out int aantalSpelers))
            {
                lijstNamenTextboxen.Clear();
                lijstRadiobuttons.Clear();
                if (aantalSpelers<=6 && aantalSpelers>=3)
                {
                    stackpanel.Children.Clear();
                    stackpanel.Children.Clear();
                    for (int i = 1; i <= aantalSpelers; i++)
                    {
                       
                        // create the label
                        Label labelNaam = new Label();                      
                        labelNaam.FontSize = 40;
                        labelNaam.Height = 50;
                        labelNaam.Content = "naam Speler:";                       
                        // add the label             
                        stackpanel.Children.Add(labelNaam);                     
                        

                        // create the textbox
                        TextBox textbox = new TextBox();
                        textbox.Name = "speler" + i;
                        textbox.FontSize = 40;
                        textbox.Background = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE4B6"));
                        textbox.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C0900"));
                        //textbox.Text = "test";
                        // add textboxes to  list
                        lijstNamenTextboxen.Add(textbox);
                        // add the textbox to grid
                        stackpanel.Children.Add(textbox);


                        //create the checkboxes
                        string groupname = "rbGroup" + i;
                       
                        RadioButton radiobuttonMan = new RadioButton();
                        RadioButton radiobuttonVrouw = new RadioButton();
                        radiobuttonMan.Name = "rbMan" +i;
                        radiobuttonMan.Content = "man";
                        radiobuttonMan.IsChecked = true;
                        radiobuttonMan.FontSize = 30;
                        radiobuttonMan.GroupName = groupname;
                        radiobuttonMan.VerticalContentAlignment = VerticalAlignment.Center;
                        radiobuttonMan.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C0900"));
                        radiobuttonVrouw.Content = "vrouw";
                        radiobuttonVrouw.Name = "rbVrouw" + i;
                        radiobuttonVrouw.GroupName = groupname;
                        radiobuttonVrouw.FontSize = 30;
                        radiobuttonVrouw.VerticalContentAlignment = VerticalAlignment.Center;
                        radiobuttonVrouw.Foreground= new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C0900"));
                        // add checkboxes to list
                        lijstRadiobuttons.Add(radiobuttonMan);
                        lijstRadiobuttons.Add(radiobuttonVrouw);
                        // add the radiobuttons to grid
                        stackpanel.Children.Add(radiobuttonMan);
                        stackpanel.Children.Add(radiobuttonVrouw);

                    }
                }
                else if (aantalSpelers<3)
                {
                    MessageBox.Show("sorry, er is een minimum van 3 spelers.");
                }
                else if (aantalSpelers>6)
                {
                    MessageBox.Show("Sorry, er is een maximum van 6 spelers.");
                }
                
                
            }
            else if (e.Key == Key.Back)
            {
                stackpanel.Children.Clear();
            }
            else
            {
                MessageBox.Show("Gelieve een getal in te voeren.");
            }
        }

        private void btn_terugMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        // list of players
        List<Speler> lijstSpelers = new List<Speler>();
        // lists of kaarten
        List<Kerkerkaart> kerkerkaartenLijst = new List<Kerkerkaart>();
        List<Schatkaart> schatkaartenLijst = new List<Schatkaart>();

        private void btn_verder_Click(object sender, RoutedEventArgs e)
        {                      
            string foutmelding = "";
            int teller = 1;
            // check of er een naam is ingevuld
            foreach (var item in lijstNamenTextboxen)
            {

                if (string.IsNullOrWhiteSpace(item.Text))
                {
                    foutmelding += "gelieve een naam in te vullen voor speler " + teller + Environment.NewLine;
                }
                else
                {
                    //speler aanmaken
                    Speler speler = new Speler();
                    speler.Naam = item.Text;

                    foreach (RadioButton radiobutton in lijstRadiobuttons)
                    {
                        if (radiobutton.Name == "rbMan" + teller)
                        {
                            if (radiobutton.IsChecked == true)
                            {
                                speler.Geslacht = "M";
                                break;
                            }
                            else
                            {
                                speler.Geslacht = "V";
                                break;
                            }
                        }

                    }
                    //speler bij in lijstspelers zetten
                    lijstSpelers.Add(speler);
                }
                teller++;
            }
            // enkel verdergaan als er geen foutmeldingen waren
            if (string.IsNullOrWhiteSpace(foutmelding))
            {
                // om bij te houden welke speler het is
                int counter = 1;
                        
                foreach (Speler speler in lijstSpelers)
                        {  
                        // check of elke speler in lijst geldig is
                            if (speler.IsGeldig())
                            {
                                int ok = DatabaseOperations.ToevoegenSpelers(speler);

                                if (ok <= 0)
                                {
                                    foutmelding+="toevoegen speler "+counter+" is niet gelukt omdat deze methode nog in commentaar staat" + Environment.NewLine;
                                }

                                else
                                {
                                    MessageBox.Show("speler "+counter+" is toegevoegd");
                                }
                            }
                            else
                            {
                                foutmelding += "de spelers properties zijn niet correct" + Environment.NewLine;
                            }
                            counter++;
                        }             

            }
            else
            {
                foutmelding+="Zolang alle velden niet correct zijn ingevuld kan het spel niet gestart worden";
            }

            // MessageBox.Show(foutmelding);

            List<int> lijstIdSpelerHandkaarten = new List<int>();
            List<int> lijstIdSpelerSchatkaarten = new List<int>();


            //enkel verdergaan en kaarten ophalen wanneer er nog steeds geen foutmelding is
            if (string.IsNullOrWhiteSpace(foutmelding))
            {
                // ophalen alle kerkerkaarten
                kerkerkaartenLijst = DatabaseOperations.OphalenKerkerkaarten();
                // ophalen alle schatkaarten
                schatkaartenLijst = DatabaseOperations.OphalenSchatkaarten();

                // methode om te shuffelen uit Listextensions.cs class
                kerkerkaartenLijst.Shuffle();
                schatkaartenLijst.Shuffle();

                

                //voor elke speler 2 stapels( 1 handkaartstapel + 1 veldkaartstapel )
                foreach (Speler speler in lijstSpelers)
                {
                    int tellerke = 1;

                    Stapel handkaarten = new Stapel();
                    Stapel veldkaarten = new Stapel();

                    handkaarten.Naam = "handkaarten " + speler.Naam;
                    veldkaarten.Naam = "veldkaarten " + speler.Naam;

                    //toevoegen van stapels via databaseoperations
                    int okey = DatabaseOperations.ToevoegenStapel(handkaarten);
                    int okey2 = DatabaseOperations.ToevoegenStapel(veldkaarten);

                    //id toevoegen aan lijst om hierbuiten te gebruiken.
                    lijstIdSpelerHandkaarten.Add(handkaarten.Id);
                    lijstIdSpelerSchatkaarten.Add(handkaarten.Id);

                    if (okey<=0)
                    {
                        foutmelding += "handkaarten stapel toevoegen van speler " + tellerke + " is niet gelukt." + Environment.NewLine;                       
                    }
                    else if (okey2<=0)
                    {
                        foutmelding += "veldkaarten stapel toevoegen van speler " + tellerke + " is niet gelukt." + Environment.NewLine;
                    }
                    else if (okey>0 && okey2 >0 && string.IsNullOrEmpty(foutmelding))
                    {       
                        //2x een kerkerkaart toevoegen bij de handkaarten van speler
                        //2x een schatkaart toevoegen bij de handkaarten van speler
                        // in totaal krijgt speler 4 kaarten in handkaarten.
                        for (int i = 1; i <= 2; i++)
                        {                               //kerkerkaart_stapel maken
                            Kaarten_Stapel kerkerkaarten_Stapel = new Kaarten_Stapel();

                            // kerkerkaart_stapel properties toewijzen
                            kerkerkaarten_Stapel.Stapel_Id = handkaarten.Id;
                            kerkerkaarten_Stapel.Kaart_Id = kerkerkaartenLijst[0].Id;

                            // kerkerkaart die nu in lijst van kerkerkaart_stapel is geplaatst, wissen uit de lijst van kerkerkaarten
                            kerkerkaartenLijst.RemoveAt(0);

                            // Kaarten_stapel effectief toevoegen aan database
                            DatabaseOperations.ToevoegenKaartenStapel(kerkerkaarten_Stapel);



                            //schatkaart_stapel maken
                            Kaarten_Stapel schatkaarten_Stapel = new Kaarten_Stapel();

                            //schatkaart_stapel properties toewijzen
                            schatkaarten_Stapel.Stapel_Id = handkaarten.Id;
                            schatkaarten_Stapel.Kaart_Id = schatkaartenLijst[0].Id;

                            // schatkaart die nu in lijst van schatkaart_stapel is geplaatst, wissen uit de lijst van schatkaarten
                            schatkaartenLijst.RemoveAt(0);

                            // Kaarten_stapel  toevoegen in database
                            DatabaseOperations.ToevoegenKaartenStapel(schatkaarten_Stapel);

                        }

                    }
                }

                /////next line of code after foreach speler

                //4 stapels bij aanmaken 
                // aflegstapel_kerker 
                // aflegstapel_schat 
                // trekstapel_kerker
                // trekstapel_schat
                Stapel aflegstapel_kerker = new Stapel();
                Stapel aflegstapel_schat = new Stapel();
                Stapel trekstapel_kerker = new Stapel();
                Stapel trekstapel_schat = new Stapel();

                // 4 stapels toevoegen in database
                DatabaseOperations.ToevoegenStapel(aflegstapel_kerker);
                DatabaseOperations.ToevoegenStapel(aflegstapel_schat);
                DatabaseOperations.ToevoegenStapel(trekstapel_kerker);
                DatabaseOperations.ToevoegenStapel(aflegstapel_schat);

                foreach (var kerkerkaart in kerkerkaartenLijst)
                {
                    Kaarten_Stapel kaarten_Stapel = new Kaarten_Stapel();
                    kaarten_Stapel.Stapel_Id = trekstapel_kerker.Id;
                    kaarten_Stapel.Kaart_Id = kerkerkaart.Id;
                    DatabaseOperations.ToevoegenKaartenStapel(kaarten_Stapel);
                }
                foreach (var schatkaart in schatkaartenLijst)
                {
                    Kaarten_Stapel kaarten_Stapel = new Kaarten_Stapel();
                    kaarten_Stapel.Stapel_Id = trekstapel_schat.Id;
                    kaarten_Stapel.Kaart_Id = schatkaart.Id;
                    DatabaseOperations.ToevoegenKaartenStapel(kaarten_Stapel);
                }

                //next line of code

                Wedstrijd wedstrijd = new Wedstrijd();
                wedstrijd.Kerkerkaarten_Aflegstapel_Id = aflegstapel_kerker.Id;
                wedstrijd.Kerkerkaarten_Trekstapel_Id = trekstapel_kerker.Id;
                wedstrijd.Schatkaarten_Aflegstapel_Id = aflegstapel_schat.Id;
                wedstrijd.Schatkaarten_Trekstapel_Id = trekstapel_schat.Id;

                int gelukt = DatabaseOperations.ToevoegenWedstrijd(wedstrijd);

               
                
               
                //DatabaseOperations.ToevoegenWedstrijdSpelers(wedstrijd.Id);

                if (gelukt >0)
                {
                   

                    foreach (Speler speler in lijstSpelers)
                    {
                        Wedstrijd_Speler wedstrijd_Speler = new Wedstrijd_Speler();
                        wedstrijd_Speler.Speler_Id = speler.Id;
                        wedstrijd_Speler.Wedstrijd_Id = wedstrijd.Id;
                        wedstrijd_Speler.Handkaarten_Id = lijstIdSpelerHandkaarten[0];
                       

                        lijstIdSpelerSchatkaarten.Remove(lijstIdSpelerHandkaarten[0]);


                    }

                    



                    
                }
                
            }




        }
    }
}
