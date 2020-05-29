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
using WECPOFLogic;

namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for TradeWindow.xaml
    /// </summary>
    public partial class TradeWindow : Window
    {
        public TradeWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        List<Kaarten_Stapel> handKaarten_Stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(GlobalVariables.actieveSpeler.Handkaarten_Id);
        Kaarten_Stapel stapel = new Kaarten_Stapel();
        Kaarten_Stapel stapelTegenstander = new Kaarten_Stapel();
        List<Wedstrijd_Speler> tegenstanders = new List<Wedstrijd_Speler>();
        List<Kaarten_Stapel> handkaartenSelectedTegenstander = new List<Kaarten_Stapel>();
        Wedstrijd_Speler tegenstander = new Wedstrijd_Speler();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tegenstanders = GlobalVariables.wedstrijd_Spelers;
            tegenstanders.Remove(GlobalVariables.actieveSpeler);

            lbSpeler1Kaarten.ItemsSource = handKaarten_Stapels;
            lbSpelers.ItemsSource = tegenstanders;
        }



        private void lbSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSpelers.SelectedItem != null)
            {
                tegenstander = (Wedstrijd_Speler)lbSpelers.SelectedItem;
                handkaartenSelectedTegenstander = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(tegenstander.Handkaarten_Id);
                lbSpeler2Kaarten.ItemsSource = handkaartenSelectedTegenstander;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lbSpeler1Kaarten.SelectedIndex != -1 && lbSpelers.SelectedIndex != -1 && lbSpeler2Kaarten.SelectedIndex != -1)
            {
                Stapel stapelActieveSpeler = DatabaseOperations.OphalenStapelViaId(GlobalVariables.actieveSpeler.Handkaarten_Id);
                Stapel stapelAndereSpeler = DatabaseOperations.OphalenStapelViaId(tegenstander.Handkaarten_Id);

                stapel = lbSpeler1Kaarten.SelectedItem as Kaarten_Stapel;
                stapelTegenstander = lbSpeler2Kaarten.SelectedItem as Kaarten_Stapel;

                stapel.KaartVanStapelWisselen(stapelAndereSpeler);
                stapelTegenstander.KaartVanStapelWisselen(stapelActieveSpeler);

                this.Close();
            }
            else
            {
                MessageBox.Show("gelieve overal een selectie te maken");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
