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
                        textbox.Text = "test";
                        // add to the list
                        lijstNamenTextboxen.Add(textbox);
                        // add the textbox
                        stackpanel.Children.Add(textbox);
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
    }
}
