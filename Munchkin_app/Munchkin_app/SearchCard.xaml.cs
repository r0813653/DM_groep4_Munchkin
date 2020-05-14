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
    /// Interaction logic for SearchCard.xaml
    /// </summary>
    public partial class SearchCard : Window
    {
        public SearchCard()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string ZoekKaart = txtZoekKaart.Text;
            
                
                List<Kaart> ZoekKaarten = DatabaseOperations.OphalenKaartenViaNaam(ZoekKaart);
                dgZoekKaart.ItemsSource = ZoekKaarten;
                
            //if (string.IsNullOrWhiteSpace(ZoekKaart))
            //{
            //}
            //else
            //{
            //    MessageBox.Show("Zoekbalk is nog leeg");
            //}
        }

        private void txtZoekKaart_GotFocus(object sender, RoutedEventArgs e)
        {
            txtZoekKaart.Text = "";
        }
    }
}
