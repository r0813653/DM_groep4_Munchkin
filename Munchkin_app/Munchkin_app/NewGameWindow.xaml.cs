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
                        if (radiobutton.Name == "rbMan"+teller)
                        {
                            if (radiobutton.IsChecked==true)
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

                    lijstSpelers.Add(speler);
                }

                
                teller++;
            }

            // check of elke speler in lijst geldig is
            if (string.IsNullOrWhiteSpace(foutmelding))
            {
                int counter = 1;
                        foreach (Speler speler in lijstSpelers)
                        {
                            if (speler.IsGeldig())
                            {
                                int ok = DatabaseOperations.ToevoegenSpelers(speler);

                                if (ok <= 0)
                                {
                                    foutmelding+="toevoegen speler "+counter+" is niet gelukt omdat deze methode nog in commentaar staat" + Environment.NewLine;
                                }

                                else
                                {
                                    MessageBox.Show("speler is toegevoegd");
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
                MessageBox.Show("Zolang alle velden niet correct zijn ingevuld kan het spel niet gestart worden");
            }

            MessageBox.Show(foutmelding);
            /////next line of code


        }
    }
}
