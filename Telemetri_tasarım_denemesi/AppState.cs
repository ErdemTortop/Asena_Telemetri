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
        static byte[] buffer = new byte[15];
        public static SerialPort SerialPort { get; set; }
        public static float hiz_rpm { get; set; }
        public static float vol { get; set; }
        public static float curr { get; set; }
        public static float torr { get; set; }
        public static float eff { get; set; }
        public static float pow { get; set; }
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
                    BekleyenSatırlar.Add ("Zaman_ms; Zaman_Saat; hiz_rpm; voltaj_V; Akım_A; guc_W; Tork_Nm; Verim_%"); 
                }

                YeniSatırlar =
                     $"{AracStopWatch.ElapsedMilliseconds} ms;" +
                     $"{DateTime.Now:HH:mm:ss};" +
                     $"{AppState.hiz_rpm} ;" +
                     $"{AppState.vol} ;" +
                     $"{AppState.curr} ;" +
                     $"{AppState.pow} ;" +
                     $"{AppState.torr} ;" +
                     $"{AppState.eff} ;";
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


                if (gelen == 0xFF) //float voltage = ((buffer[2] << 8) | buffer[3]) / 100.0f;
                {
                    buffer[0] = gelen;// 0xFF
                    gelen = (byte)SerialPort.ReadByte();
                    if (gelen == 6)
                    {
                        buffer[1] = gelen;//6
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[2] = gelen;//volt ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[3] = gelen;//volt ikinci parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[4] = gelen;// curr ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[5] = gelen;// curr ikinci parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[6] = gelen;// rpm ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[7] = gelen;// rpm ikinci parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[8] = gelen; // pow ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[9] = gelen;// pow ikinci parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[10] = gelen;//tor ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[11] = gelen;//tor ikinci parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[12] = gelen;//eff ilk parça
                        gelen = (byte)SerialPort.ReadByte();
                        buffer[13] = gelen;//eff ikince parca
                        gelen = (byte)SerialPort.ReadByte();


                        buffer[14] = gelen;// CRC
                        int CRC = (byte)(buffer[2] + buffer[3] + buffer[4] + buffer[5] + buffer[6] + buffer[7] + buffer[8] + buffer[9] + buffer[10] + buffer[11] + buffer[12] + buffer[13]) & 0xFF;
                        if (CRC == buffer[14])
                        {
                            if (AppState.IlkVeriGeldi == false)
                            {
                                AracStopWatch.Start();
                                IlkVeriGeldi = true;
                            }

                            vol = ((buffer[2] << 8) | buffer[3] ) / 100.0f;
                            curr = ((buffer[4] << 8) | buffer[5]) / 100.0f;
                            hiz_rpm = ((buffer[6] << 8) | buffer[7]) / 10.0f;
                            pow = ((buffer[8] << 8) | buffer[9]) / 10.0f;
                            torr = ((buffer[10] << 8) | buffer[11]) / 100.0f;
                            eff = ((buffer[12] << 8) | buffer[13]) / 100.0f;

                        }
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

