using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new HomaPage());
            SolarBorder.Background = Brushes.Transparent;
            UsbBorder.Background = Brushes.Transparent;
            dataBorder.Background = Brushes.Transparent;
            RecordBorder.Background = Brushes.Transparent;
            HomeBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));

        }
      
        private void Usb_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsbPage());
            SolarBorder.Background = Brushes.Transparent;
            UsbBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));
            dataBorder.Background = Brushes.Transparent;
            RecordBorder.Background = Brushes.Transparent;
            HomeBorder.Background = Brushes.Transparent;

        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DataPage());
            SolarBorder.Background = Brushes.Transparent;
            HomeBorder.Background = Brushes.Transparent;
            UsbBorder.Background = Brushes.Transparent;
            dataBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));
            RecordBorder.Background = Brushes.Transparent;
        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CameraPage());
            SolarBorder.Background = Brushes.Transparent;
            UsbBorder.Background = Brushes.Transparent;
            HomeBorder.Background = Brushes.Transparent;
            dataBorder.Background = Brushes.Transparent;
            RecordBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));
        }

        private void SolarTeam_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TeamPage());
            SolarBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));
            UsbBorder.Background = Brushes.Transparent;
            dataBorder.Background = Brushes.Transparent;
            RecordBorder.Background = Brushes.Transparent;
            HomeBorder.Background = Brushes.Transparent;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomaPage());
            SolarBorder.Background = Brushes.Transparent;
            UsbBorder.Background = Brushes.Transparent;
            dataBorder.Background = Brushes.Transparent;
            RecordBorder.Background = Brushes.Transparent;
            HomeBorder.Background = new SolidColorBrush(Color.FromRgb(20, 25, 31));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}