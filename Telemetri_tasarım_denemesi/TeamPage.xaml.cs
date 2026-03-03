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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Telemetri_tasarım_denemesi
{
    /// <summary>
    /// Interaction logic for TeamPage.xaml
    /// </summary>
    public partial class TeamPage : Page
    {
        public TeamPage()
        {
            InitializeComponent();
            
        }

        private void Onr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Onur_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OnurFurkan());   
        }

        private void fotoileri_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
