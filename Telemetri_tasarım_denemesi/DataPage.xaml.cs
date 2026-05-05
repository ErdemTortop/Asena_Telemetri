using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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
using System.Windows.Threading;


namespace Telemetri_tasarım_denemesi
{
    public partial class DataPage : Page
    {

         DispatcherTimer timer;

        public DataPage()
        {
            InitializeComponent();
        }

        private void DataPage_Loaded(object sender, RoutedEventArgs e)
        {
           

            Dispatcher.Invoke(() =>
            {
                VolLbl.Content = $"{AppState.vol} V";
                CurrLbl.Content = $"{AppState.curr} A";
                rpmLbl.Content = $"{AppState.hiz_rpm} r/min";
                powLbl.Content = $"{AppState.pow} W";
                torrLbl.Content = $"{AppState.torr} Nm";
                effLbl.Content = $"{AppState.eff} %";
            });

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void DataPage_Unloaded(object sender, RoutedEventArgs e)
        {

            timer.Stop();

        }


        private void Timer_Tick(object sender, EventArgs e)
        {

            VolLbl.Content = $"{AppState.vol} V";
            CurrLbl.Content = $"{AppState.curr} A";
            rpmLbl.Content = $"{AppState.hiz_rpm} r/min";
            powLbl.Content = $"{AppState.pow} W";
            torrLbl.Content = $"{AppState.torr} Nm";
            effLbl.Content = $"{AppState.eff} %";

        }




    }
}