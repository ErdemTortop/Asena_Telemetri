using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Interaction logic for HomaPage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();  
        }

        private async void Test_Start(object sender, RoutedEventArgs e)
        {
            AppState.TestFlag = true; 
            AppState.AracStopWatch.Restart();

            AppState.rpm = 10;
            AppState.voltage = 5;
            AppState.current = 1;

            while (AppState.TestFlag ==true) 
            { 
                

                AppState.rpm += 10;
                if (AppState.rpm > 800) AppState.rpm = 10;

                AppState.voltage++;
                if (AppState.voltage > 90) AppState.voltage = 5;

                AppState.current++;
                if (AppState.current > 30) AppState.current = 1;

                


                AppState.omega = AppState.rpm * 2.0f * 3.14159f / 60.0f;
                AppState.power_elec = AppState.voltage * AppState.current;
                AppState.kayıplar = AppState.current * AppState.current * 0.2f;
                AppState.power_mech = AppState.power_elec - AppState.kayıplar;
                if (AppState.omega > 0.05f)
                {
                    AppState.torque = AppState.power_mech / AppState.omega;
                }
                else
                {
                    AppState.torque = 0.0f;
                }
                if (AppState.power_elec > 0.1f)
                { 
                    AppState.efficiency = (AppState.power_mech / AppState.power_elec) * 100.0f;
                }
                else
                {
                    AppState.efficiency = 0.0f;
                }
                AppState.kmh = AppState.rpm * (3.14159f * AppState.tekerlek_cap) * 60f / 1000f;

                if (AppState.IlkVeriGeldi == false)  // ← bunu ekle
                {
                    AppState.IlkVeriGeldi = true;
                }

                AppState.KayitYap();
                await Task.Delay(500);
            }
        }

        private void Test_stop(object sender, RoutedEventArgs e)
        {
            AppState.TestFlag = false;
        }
    }
}
