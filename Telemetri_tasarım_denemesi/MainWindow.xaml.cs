using System.Net.NetworkInformation;
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
using Telemetri.Properties;

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
            bool mainpage_is_usbPage = Telemetri.Properties.Settings.Default.Usb_hub_is_mainpage;
            if (mainpage_is_usbPage)
            {
                Usb_Click(null, null);
            }
            else
            {
                Home_Click(null, null);
            }

        }

        public async Task<bool> IsInternetAvailableViaPingAsync()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000); // Google's DNS server
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
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
            Task.Run(() => IsInternetAvailableViaPingAsync().Result)
            .ContinueWith(t =>
            {
                bool isOnline = t.Result;
                Dispatcher.Invoke(() =>
                {
                    if (isOnline)
                        MainFrame.Navigate(new HomePage());
                    else
                        MainFrame.Navigate(new HomePage_noint());
                });
            });
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
            if (AppState.RecordFlag == true)
            {
                MessageBox.Show("Hâlâ kayıt yapılmaktadır", "Uyarı !", MessageBoxButton.OK);
            }
            else
            {
               this.Close();
            }

            
            
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}