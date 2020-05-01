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
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        public NewGameWindow()
        {
            InitializeComponent();
        }

        List<TextBox> lijstNamenTextboxen = new List<TextBox>(); // lijst is nodig om achteraf gegevens op te vragen 
        List<CheckBox> lijstCheckboxen = new List<CheckBox>();
        
        
        private void txt_aantalSpelers_KeyUp(object sender, KeyEventArgs e)
        {
            if (int.TryParse(txt_aantalSpelers.Text, out int aantalSpelers))
            {
                if (aantalSpelers<=6)
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
                        textbox.Text = "test";
                        // add textboxes to  list
                        lijstNamenTextboxen.Add(textbox);
                        // add the textbox to grid
                        stackpanel.Children.Add(textbox);


                        //create the checkboxes
                        CheckBox radiobuttonMan = new CheckBox();
                        CheckBox radiobuttonVrouw = new CheckBox();
                        radiobuttonMan.Name = "radiobuttonMan";
                        radiobuttonMan.Content = "man" + i;
                        radiobuttonMan.FontSize = 30;
                        radiobuttonMan.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C0900"));
                        radiobuttonVrouw.Content = "vrouw";
                        radiobuttonVrouw.Name = "vrouw" + i;
                        radiobuttonVrouw.FontSize = 30;
                        radiobuttonVrouw.Foreground= new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C0900"));
                        // add checkboxes to list
                        lijstCheckboxen.Add(radiobuttonMan);
                        lijstCheckboxen.Add(radiobuttonVrouw);
                        // add the radiobuttons to grid
                        stackpanel.Children.Add(radiobuttonMan);
                        stackpanel.Children.Add(radiobuttonVrouw);

                    }
                }
                else
                {
                    MessageBox.Show("Sorry, er is een maximum van 6 spelers.");
                }
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
    }
}
