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
            static byte[] buffer = new byte[9];
            public static SerialPort SerialPort { get; set; }
            public static int rpm { get; set; }
            public static int vol { get; set; }
            public static int curr { get; set; }
            public static string SecilenPort { get; set; }
            public static string SecilenRate { get; set; }

            public static bool RecordFlag;

            public static object RecordLock = new object();

            public static string dosyaYolu;

            public static string KayıtDosya;

            public static string ExKayıtDosya;


            public static float omega = 0.0f;

            public static float torque = 0.0f;

            public static float power_elec = 0.0f;

            public static float power_mech = 0.0f;

            public static float efficiency = 0.0f;

            public static float voltage = 0.0f;

            public static float current = 0.0f;

            public static float kayıplar = 0.0f;

            public static float tekerlek_cap = 0.566f; //metre

            public static float kmh = 0.0f;

            public static float rpm_float = 0.0f;


            public static long KayıtMs;

            public static long ExKayıtMs;

            public static long KopmaBaslangıcı;

            public static long KopmaBitis;

            public static bool BaslıkYazıldi = false;

            public static bool KopmaVar = false;

            public static bool TestFlag = false;

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

                    lock(RecordLock)
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
                            BekleyenSatırlar.Add("Zaman_ms; Zaman_Clcok; Hiz_rpm; Voltaj_V; Akım_A; Guc_W; Tork_Nm; Verim_%");
                        }

                        if (KopmaVar == true)
                        {
                            KopmaBitis = AracStopWatch.ElapsedMilliseconds;
                            AppState.BekleyenSatırlar.Add($"Kopma Yaşandı; Başlangıç:{AppState.KopmaBaslangıcı} ; Bitiş: {AppState.KopmaBitis}; Kopma Süresi : {AppState.KopmaBitis - AppState.KopmaBaslangıcı}");
                            BekleyenSatırlar.Add("Bağlantı Yeniden Sağlandı");
                        }

                        KayıtMs = AracStopWatch.ElapsedMilliseconds;
                    


                    YeniSatırlar =
                         $"{KayıtMs} ms;" +
                         $"{DateTime.Now:HH:mm:ss};" +
                         $"{AppState.rpm};" +
                         $"{AppState.voltage};" +
                         $"{AppState.current};" +
                         $"{AppState.power_elec};" +
                         $"{AppState.torque};" +
                         $"{AppState.efficiency}";
                        BekleyenSatırlar.Add(YeniSatırlar);
                        KayıtSayaci++;

                        if (KayıtSayaci >= 15)
                        {
                            TopluYazi = string.Join("\n", BekleyenSatırlar) + "\n";
                            File.AppendAllText(KayıtDosya, TopluYazi, System.Text.Encoding.UTF8);
                            BekleyenSatırlar.Clear();
                            KayıtSayaci = 0;
                        }
                        KopmaVar = false;
                        return YeniSatırlar;
                    
                    }
                
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
                        if (gelen == 3)
                        {
                            buffer[1] = gelen; 
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[2] = gelen; //vol high
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[3] = gelen; // vol low
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[4] = gelen; //curr high
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[5] = gelen; // curr low
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[6] = gelen; // rpm high
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[7] = gelen; //rpm low
                            gelen = (byte)SerialPort.ReadByte();
                            buffer[8] = gelen; // crc

                        int CRC = (byte)(buffer[2] + buffer[3] + buffer[4] + buffer[5] + buffer[6] + buffer[7] ) & 0xFF;
                            if (CRC == buffer[8])
                            {
                                if (AppState.IlkVeriGeldi == false)
                                {
                                    AracStopWatch.Start();
                                    IlkVeriGeldi = true;
                                }

                        
                                vol = ((buffer[2] << 8) | buffer[3]);
                                curr = ((buffer[4] << 8) | buffer[5]);
                                rpm = ((buffer[6] << 8) | buffer[7]);
                                rpm_float = rpm / 10;

                            voltage = (vol / 4095.0f) * 3.3f * (482.0f / 12.0f);
                            current = ((curr / 4095.0f) * 3.3f * (16.8f / 6.8f) - 2.5f) / 0.0133f;


                            omega = rpm * 2.0f * 3.14159f / 60.0f;
                            power_elec = voltage * current;
                            kayıplar = current * current * 0.2f;
                            power_mech = power_elec - kayıplar;
                            if (omega > 0.05f)
                            { // Eğer motor dönüyorsa tork hesapla
                                torque = power_mech / omega;
                            }
                            else
                            {
                                torque = 0.0f;
                            }
                            if (power_elec > 0.1f)
                            { // Eğer elektrik gücü varsa verim hesapla
                                efficiency = (power_mech / power_elec) * 100.0f;
                            }
                            else
                            {
                                efficiency = 0.0f;
                            }
                            kmh = rpm * (2f * 3.14159f * tekerlek_cap) * 60f / 1000f;

                            AppState.KayitYap();
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

