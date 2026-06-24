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
using System.IO.Ports;
using Telemetri.Properties;

namespace Telemetri_tasarım_denemesi
{
    /// <summary>
    /// Interaction logic for UsbPage.xaml
    /// </summary>

    public partial class UsbPage : Page
    {

        // Store the original background of the cancel button
        private Brush _originalCancelButtonBackground;
        
        public UsbPage()
        {
            InitializeComponent();
        }


        private async Task UyarıBlink(string firstColor, string secondColor, string lastColor = "nothing")
        {
            if (lastColor == "nothing")
            {
                lastColor = firstColor;
            }
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(firstColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(secondColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(firstColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(secondColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(firstColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(secondColor);
            await Task.Delay(300);
            BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom(lastColor);
        }

        private void UsbPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Store the original button background (if any)
            _originalCancelButtonBackground = PortBaglantıIptalButon.Background;

            MakeThisHomePage.IsChecked = Telemetri.Properties.Settings.Default.Usb_hub_is_mainpage;

            foreach (string port in SerialPort.GetPortNames())
            {
                PortComboBox.Items.Add(port);
            }

            RateBox.Items.Add("9600");
            RateBox.Items.Add("19200");
            RateBox.Items.Add("57600");
            RateBox.Items.Add("115200");
            

            if (AppState.SerialPort != null && AppState.SerialPort.IsOpen)
            {
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#4CAF7D");
            }

            if (AppState.SecilenPort != null)
            {
                PortComboBox.SelectedItem = AppState.SecilenPort;
            }
            
            if (AppState.SecilenRate != null)
            {
                RateBox.SelectedItem = AppState.SecilenRate;
            }
        }

        private async void PortBaglanButon_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (PortComboBox.SelectedItem != null && RateBox.SelectedItem != null)
                {
                    if (AppState.SerialPort == null || !AppState.SerialPort.IsOpen)
                    {
                        string portDeger = PortComboBox.SelectedItem.ToString();
                        int rateDeger = int.Parse(RateBox.SelectedItem.ToString());
                        AppState.SecilenPort = portDeger;
                        AppState.SecilenRate = rateDeger.ToString();
                        AppState.SerialPort = new SerialPort(portDeger, rateDeger);
                        AppState.SerialPort.Open();
                        AppState.StartListening();
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#4CAF7D");
                        PortBaglantıIptalButon.IsEnabled = true;
            
                    }
                    else
                    {
                        MessageBox.Show("Dostum, seçtiğin port doluymuş zaten.");

                        await UyarıBlink("#E6B84A", "#262835");

                      /*  BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                        await Task.Delay(300);
                        BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#4CAF7D");*/

                    }                }
                else
                {

                    MessageBox.Show("Dostum, ya baud rate seçmedin yada port seçmedin.", "Error  ");

                    await UyarıBlink("#E6B84A", "#262835");

                    /*BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");*/
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Bağlanırken ufak Bir hata oluştu");

                await UyarıBlink("#E6B84A", "#262835", "#E05C5C");

               /* BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");*/
            }
        }

       

        private async void PortIptalButon_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (AppState.SerialPort != null && AppState.SerialPort.IsOpen)
                {
                    AppState.StopListening();
                    AppState.SerialPort.Close();

                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");
                    PortBaglantıIptalButon.IsEnabled = false;
        
                }
                else
                {
                    MessageBox.Show("Dostum, daha port seçmedin ki");

                    await UyarıBlink("#E6B84A", "#262835", "#E05C5C");

                   /* BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                    await Task.Delay(300);
                    BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");*/
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "İptal ederken ufak Bir hata oluştu");

                await UyarıBlink("#E6B84A", "#262835", "#E05C5C");

                /*BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E6B84A");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#262835");
                await Task.Delay(300);
                BaglantıDurumuLblRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");*/
            }
        }

        private void PortYenileButon_Click(object sender, RoutedEventArgs e)
        {
            PortComboBox.Items.Clear();
            RateBox.Items.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                PortComboBox.Items.Add(port);

            }

            RateBox.Items.Add("9600");
            RateBox.Items.Add("19200");
            RateBox.Items.Add("57600");
            RateBox.Items.Add("115200");
        }

        private void PortComboBox_opened(object sender, EventArgs e)
        {
            PortComboBox.Items.Clear();
            //RateBox.Items.Clear(); //erdem yazmış yorumlu kısımları
            foreach (string port in SerialPort.GetPortNames())
            {
                PortComboBox.Items.Add(port);

            }

            //RateBox.Items.Add("9600");
            //RateBox.Items.Add("115200");
            //RateBox.Items.Add("19200");
            //RateBox.Items.Add("57600");

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //make this page homepage permanently
            Telemetri.Properties.Settings.Default.Usb_hub_is_mainpage = true;
            Telemetri.Properties.Settings.Default.Save();
        }

        private void CheckBox_MouseEnter(object sender, MouseEventArgs e)
        {
            MakeThisHomePage.Opacity = 100;
        }

        private void MakeThisHomePage_MouseLeave(object sender, MouseEventArgs e)
        {
            MakeThisHomePage.Opacity = 0;
        }

        private void MakeThisHomePage_Unchecked(object sender, RoutedEventArgs e)
        {
            Telemetri.Properties.Settings.Default.Usb_hub_is_mainpage = false;
            Telemetri.Properties.Settings.Default.Save();

        }

        private void PortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RateBox.SelectedIndex = 0;
        }

        // Helper method to update the cancel button's background based on IsEnabled
        private void UpdateCancelButtonAppearance(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PortBaglantıIptalButon.IsEnabled)
            {
                // Restore original background (or fallback to a default)
                PortBaglantıIptalButon.Background = _originalCancelButtonBackground ?? SystemColors.ControlBrush;
                PortBaglantıIptalButon.Foreground = new SolidColorBrush(Color.FromRgb(226, 232, 240));
            }
            else
            {
                // Desaturate – use a gray shade (e.g., #CCCCCC)
                PortBaglantıIptalButon.Background = new SolidColorBrush(Color.FromRgb(18, 23, 27));
                PortBaglantıIptalButon.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            }

        }
    }


}




