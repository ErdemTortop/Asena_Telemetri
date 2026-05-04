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

        public static string dosyaYolu;

        public static string KayıtDosya;

        public static string ExKayıtDosya;

        public static bool BaslıkYazıldi = false;

        public static int KayıtSayaci = 0;

        public static string TopluYazi;

        public static string YeniSatırlar;

        public static System.Diagnostics.Stopwatch AracStopWatch = new System.Diagnostics.Stopwatch();

        public static bool IlkVeriGeldi = false;

        public static List<string> BekleyenSatırlar = new List<string>();

        public static string KayitYap()
        {
            try 
            {
                if (RecordFlag == false)
                {
                    return "";
                }

                

                if (BaslıkYazıldi == false)
                {
                    KayıtDosya = dosyaYolu;
                    ExKayıtDosya = dosyaYolu;
                    BaslıkYazıldi = true;
                    BekleyenSatırlar.Add ("Zaman_ms; Zaman_Clcok; hiz_kmh; V_bat_C; T_bat_C; kalan_enerji_Wh"); pipi
                }

                   YeniSatırlar = 
                    $"{AracStopWatch.ElapsedMilliseconds} ms;" +
                    $"{DateTime.Now:HH:mm:ss};" +
                    $"{AppState.hiz} km/h;" + 
                    $"{AppState.voltaj} V;" +
                    $"{AppState.sicaklik} °C;" +
                    $"{AppState.enerji} wh";
                BekleyenSatırlar.Add(YeniSatırlar);
                KayıtSayaci ++;

                if (KayıtSayaci >= 15)
                {
                    TopluYazi = string.Join("\n", BekleyenSatırlar) + "\n";
                    File.AppendAllText(KayıtDosya, TopluYazi, System.Text.Encoding.UTF8);
                    BekleyenSatırlar.Clear();
                    KayıtSayaci = 0;
                }
                return YeniSatırlar; 
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.Message, "Hata");
                return"";
            }
           /* try {

                string satir = "";
                if (RecordFlag == true)
                {
                    if (!File.Exists(KayıtDosya))
                    {

                        KayıtDosya = dosyaYolu;

                        ExKayıtDosya = KayıtDosya;
                        
                        satir = "Zaman_ms; hiz_kmh; V_bat_C; T_bat_C; kalan_enerji_Wh\n";

                        File.AppendAllText(KayıtDosya, satir, System.Text.Encoding.UTF8);

                        satir = $"{DateTime.Now:HH:mm:ss}; {AppState.hiz} km/h; {AppState.voltaj} V; {AppState.sicaklik} °C; {AppState.enerji} wh\n";
                        
                        File.AppendAllText(KayıtDosya, satir, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        satir = $"{DateTime.Now:HH:mm:ss}; {AppState.hiz} km/h; {AppState.voltaj} V; {AppState.sicaklik} °C; {AppState.enerji} wh\n";
                        File.AppendAllText(KayıtDosya, satir, System.Text.Encoding.UTF8);
                    }
                }

                return satir;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Hata");

             
            }
            return ""; */
        }



        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)

        {


            System.Diagnostics.Debug.WriteLine($"[UART] BytesToRead={SerialPort.BytesToRead}");
            while (SerialPort.BytesToRead > 0)
            {
                 byte gelen = (byte)SerialPort.ReadByte();
                System.Diagnostics.Debug.WriteLine($"[UART] gelen=0x{gelen:X2}");


                if (gelen == 0xFF)
                {
                    buffer[0] = gelen;// 0xFF
                    gelen = (byte)SerialPort.ReadByte();
                    if (gelen == 4)
                    {
                        buffer[1] = gelen;//4
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[2] = gelen;// hız(rpm- açısal hız)
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[3] = gelen;//voltaj
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[4] = gelen;//sıcaklık(opsiyonel)
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[5] = gelen;//akım
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[6] = gelen;//CRC
                        int CRC = (byte)(buffer[2] + buffer[3] + buffer[4] + buffer[5]) & 0xFF;
                        if (CRC == buffer[6])
                        {
                            if (AppState.IlkVeriGeldi == false)
                            {
                                AracStopWatch.Start();
                                IlkVeriGeldi = true;
                            }

                        
                            hiz = buffer[2];
                            voltaj = buffer[3];
                            sicaklik = buffer[4];
                            enerji = buffer[5];
                        }
                        // I*V = P -- açısal hız*T = P : Güç Tork 
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

