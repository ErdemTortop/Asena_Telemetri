using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {

        DispatcherTimer timer;


        string dosyaYolu = "telemetri_log.csv";
        public CameraPage()
        {
            InitializeComponent();

        }


        private void CameraPage_Loaded(object sender, RoutedEventArgs e)
        {

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();

        }


        private void CameraPage_Unloaded(object sender, RoutedEventArgs e)
        {
            //timer.Stop(); //hata verirse burayı aç ve başka bir şey düşün.
        }

        private void Kayit_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.RecordFlag == false)
            {
                AppState.RecordFlag = true;
                KayıtDurumuRenk.Background = (Brush)new BrushConverter().ConvertFrom("#4CAF7D");
            }

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.RecordFlag == true)
            {
                AppState.RecordFlag = false;
                KayıtDurumuRenk.Background = (Brush)new BrushConverter().ConvertFrom("#E05C5C");
            }
        }


        private async void FileTemizle_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (AppState.RecordFlag == false)
                {
                    if (File.Exists(dosyaYolu))
                    {
                        File.Delete(dosyaYolu);


                        KayitTextBox.Text = "";
                        KayitTextBox.Text = "Kayıtlar Temizlendi";
                        await Task.Delay(1500);
                        KayitTextBox.Text = "";


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Selamlar, Ben Erdem. Şimdi muhtemelen programın çökmesine sebep olan bir hata ile karşılaştın. En kısa sürede getiriceğimiz bir güncelleme ile hataları ayıklicaz. Lütfen programı baştan çalıştırır mısınız? Yada muhtemelen arkada açık olan excel dosyasını kapatırsanız sorun çözülücektir :)", ex.Message);


            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            string satir = AppState.KayitYap();
            if (!string.IsNullOrEmpty(satir))
            {
                KayitTextBox.Text += satir;
            }

        }


        private void File_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start("explorer.exe",
                System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(dosyaYolu)));

            }
            catch (Exception ex)
            {

                MessageBox.Show("Selamlar, Ben Erdem. Şimdi muhtemelen programın çökmesine sebep olan bir hata ile karşılaştın. En kısa sürede getiriceğimiz bir güncelleme ile hataları ayıklicaz. Lütfen programı baştan çalıştırır mısınız? Yada muhtemelen arkada açık olan excel dosyasını kapatırsanız sorun çözülücektir :)", ex.Message);


            }
        }

    }
}





