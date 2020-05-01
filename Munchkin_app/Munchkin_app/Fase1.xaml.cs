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

namespace Munchkin_app
{
    /// <summary>
    /// Interaction logic for Fase1.xaml
    /// </summary>
    public partial class Fase1 : Window
    {
        public Fase1()
        {
            InitializeComponent();
            control();
        }
        Image image1;
        Image image2;
        Image image3;
        Button btntest;
        public void control()
        {
            
            image1 = new Image();
            image1.Height = 100;
            image1.Width = 50;
            
            
        }

    }
}
