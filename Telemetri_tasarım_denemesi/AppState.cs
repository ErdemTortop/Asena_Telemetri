using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Telemetri_tasarım_denemesi
{
    public static class AppState
    {
        static byte[] buffer = new byte[7];
        public static SerialPort SerialPort { get; set; }
        public static byte hiz { get; set; }
        public static byte voltaj { get; set; }
        public static byte sicaklik { get; set; }
        public static byte enerji { get; set; }
        public static string SecilenPort { get; set; }
        public static string SecilenRate { get; set; }

        public static bool RecordFlag;

        public static string dosyaYolu = "telemetri_log.csv";


        public static string KayitYap()

        {
            try {

                string satir = "";
                if (RecordFlag == true)
                {
                    if (!File.Exists(dosyaYolu))
                    {
                        satir = "Zaman; Hiz; Voltaj; Sıcaklık; Enerji\n";

                        File.AppendAllText(dosyaYolu, satir, System.Text.Encoding.UTF8);



                        satir = $"{DateTime.Now:HH:mm:ss}; {AppState.hiz} km/h; {AppState.voltaj} V; {AppState.sicaklik} °C; {AppState.enerji} wh\n";
                        File.AppendAllText(dosyaYolu, satir, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        satir = $"{DateTime.Now:HH:mm:ss}; {AppState.hiz} km/h; {AppState.voltaj} V; {AppState.sicaklik} °C; {AppState.enerji} wh\n";
                        File.AppendAllText(dosyaYolu, satir, System.Text.Encoding.UTF8);
                    }
                }

                return satir;


            }
            catch (Exception ex)
            {

                MessageBox.Show("Selamlar, Ben Erdem. Şimdi muhtemelen programın çökmesine sebep olan bir hata ile karşılaştın. En kısa sürede getiriceğimiz bir güncelleme ile hataları ayıklicaz. Lütfen programı baştan çalıştırır mısınız? Yada muhtemelen arkada açık olan excel dosyasını kapatırsanız sorun çözülücektir :)", ex.Message);

             
            }
            return "";
        }



        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)

        {
            while (SerialPort.BytesToRead > 0)
            {
                byte gelen = (byte)SerialPort.ReadByte();

                if (gelen == 0xFF)
                {
                    buffer[0] = gelen;// 0xFF
                    gelen = (byte)SerialPort.ReadByte();
                    if (gelen == 4)
                    {
                        buffer[1] = gelen;//4
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[2] = gelen;// hız
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[3] = gelen;//voltaj
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[4] = gelen;//sıcaklık
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[5] = gelen;//enerji
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[6] = gelen;//CRC
                       // int CRC = (byte)(buffer[2] + buffer[3] + buffer[4] + buffer[5]) & 0xFF;
                        //if (CRC == buffer[6])
                        //{
                            hiz = buffer[2];
                            voltaj = buffer[3];
                            sicaklik = buffer[4];
                            enerji = buffer[5];


                        //}
                    }
                    else
                    {
                        buffer[0] = 0;
                    }
                }

            }
        }

        public static void StartListening()
        {
            if (SerialPort != null)
            {
                SerialPort.DataReceived += SerialPort_DataReceived;
            }



        }

        public static void StopListening()
        {
            if (SerialPort != null)
            {
                SerialPort.DataReceived -= SerialPort_DataReceived;
            }
        }
    }






}

