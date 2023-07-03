using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class program
{
    // her classtan ulaşılabilecek bir thread kuyrugu oluşturma
    static ConcurrentQueue<string> ThreadKuyruk = new ConcurrentQueue<string>();



    static void Main(string[] args)
    {
        Console.WriteLine("Thread oluştıulacak txt dosya yolunu giriniz:");
        string dosyaYolu = Console.ReadLine();
        // StreamReader methodu okuma işlemi yapar.

        try 
        {
            using (StreamReader okuyucu = new StreamReader(dosyaYolu))
            {
                string kelime;
                while ((kelime = okuyucu.ReadLine()) != null)
                {

                    string[] kelimeler = kelime.Split(' ');
                    foreach (string s in kelimeler)
                    {   //Enqueune işlemi threada ekleme yapar
                    ThreadKuyruk.Enqueue(s.Trim());
                    }
                }
            }

            // threadları sırasıyla çalıştırma
            Thread[] calisacakThread = new Thread[ThreadKuyruk.Count];
            int threadIndex = 1;
            foreach (string kelime in ThreadKuyruk)
            {   
                Console.WriteLine("Thread {0} Başarıyla çalıştı. İşlem Sonucu aşağıdadır.", threadIndex);
                calisacakThread[threadIndex-1] = new Thread(() => KelimeFonksiyonu(kelime));
                calisacakThread[threadIndex-1].Start();
                Thread.Sleep(3000);
                threadIndex++;
            }
            //threadların tamamlanma işlemi join ile yapılıyor.
            foreach (Thread thread in calisacakThread)
            {
                thread.Join();
            }
            Console.WriteLine("İşlem tamamlandı.");
        }
        catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        
    }
    // threadların içinde çalışacak fonksiyon
    
    static void KelimeFonksiyonu(string kelime)
    {
        Console.WriteLine("{0}:{1}", kelime, kelime.Length);
    }
}