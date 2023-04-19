using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Windows.Web.Http; // windows only
using System.Threading.Tasks;
//using System.Net.Http;

namespace DailyNewsArchivator
{
    public class Mandiner
    {
        public void ArchiveAll()
        {
            DateTime mainTimeTrackerStart = DateTime.Now; // Fő stopper indítása.

            List<string> outputText2Log = new List<string>();

            List<string> hibasUrlArchivalasok = new List<string>();

            //if (args == null || args.Length == 0)
            //{
            //    throw new ApplicationException("Specify the URI of the resource to retrieve.");
            //}
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            string s = "";

            try
            {
                //Stream data = client.OpenRead(args[0]);
                Stream data = client.OpenRead("https://mandiner.hu");
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                //Console.WriteLine(s);
                data.Close();
                reader.Close();
            }
            catch (Exception)
            {
                Console.WriteLine($"Nem érhető el a https://mandiner.hu így archiválni sem lehet.");
                outputText2Log.Add($"Nem érhető el a https://mandiner.hu így archiválni sem lehet.");
            }

            // Console.WriteLine(s); // Teljes webtartalom kiírása

            //Console.ReadKey();

            string[] urlCimek = s.Split("href=\"");

            List<string> rejectLista = new List<string>
                {
                    "https://mandiner.hu/impresszum",
                    "https://mandiner.hu/mediaajanlat",
                    "https://mandiner.hu/gyik",
                    "https://mandiner.hu/jatekszabalyzat",
                    "https://mandiner.hu/felhasznalasi-feltetelek",
                    "https://mandiner.hu/adatvedelmi-tajekoztato",
                    "https://mandiner.hu/elofizetoi-aszf",
                    "https://mandiner.hu/reklamszolgaltatasokra-vonatkozo-aszf",
                    "https://mandiner.hu/images/mandiner-nlogo3.png",
                    "https://mandiner.hu/rss/",
                    "https://makronom.mandiner.hu/",
                    "https://precedens.mandiner.hu/",
                    "https://sport.mandiner.hu/",
                    "https://mandiner.hu/podcast",
                    "https://mandiner.hu/mandiner-java",
                    "https://mandiner.hu",
                    "https://mandiner.hu/hetilap",
                    "https://mandiner.hu/elofizetes",
                    "https://www.facebook.com/mandiner.hu/",
                    "https://www.instagram.com/mandiner.hu/?hl=hu",
                    "https://www.instagram.com/mandiner.hu/",
                    "<!DOCTYPE html>"
                };

            var valogatottCimek = (from elem in urlCimek
                                   where elem.Contains("mandiner.hu") && !elem.Contains("<!DOCTYPE html>")
                                   select Regex.Replace(elem.Split('"')
                                  .FirstOrDefault(), "#comments$", "")
                                  )
                                  .Distinct();

            var valogatottAlrovatCimek = (from elem in valogatottCimek
                                          where elem.Contains("mandiner.hu") && !elem.Contains("utm_source=") && !elem.Contains("<!DOCTYPE html>")
                                          select elem)
                                         .Except(rejectLista);

            //var eredmeny = valogatottAlrovatCimek.ToList().RemoveAll(x => x.Contains("utm_source="));

            var valogatottKulsosCimek = from elem in valogatottCimek
                                        where elem.Contains("utm_source=")
                                        select elem.Split("?utm_source").FirstOrDefault();

            var valogatottFooldalCimek = from elem in valogatottCimek
                                         where !elem.Contains("mandiner.hu")
                                         select "https://mandiner.hu" + elem;

            int sorszam = 0;
            foreach (var item in valogatottCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            Console.WriteLine("========================================================");
            Console.WriteLine("========================================================");
            sorszam = 0;

            foreach (var item in valogatottFooldalCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            Console.WriteLine("========================================================");
            sorszam = 0;

            foreach (var item in valogatottAlrovatCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            Console.WriteLine("========================================================");
            sorszam = 0;

            foreach (var item in valogatottKulsosCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            Console.WriteLine();
            Console.WriteLine("=========================BEGIN==========================");
            outputText2Log.Add("=========================BEGIN==========================");

            try
            {
                Stream data = client.OpenRead("http://web.archive.org/save/https://mandiner.hu/");
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                Console.WriteLine("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/https://mandiner.hu/");
                outputText2Log.Add("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/https://mandiner.hu/");
                data.Close();
                reader.Close();
                //System.Threading.Thread.Sleep(50000);
                new System.Threading.ManualResetEvent(false).WaitOne(50000);

            }
            catch (Exception)
            {
                Console.WriteLine(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": https://mandiner.hu/");
                outputText2Log.Add(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": https://mandiner.hu/");
                if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add("https://mandiner.hu/");
            }

            ListArchivator(outputText2Log, valogatottFooldalCimek.ToList(), hibasUrlArchivalasok);

            ListArchivator(outputText2Log, valogatottAlrovatCimek.ToList(), hibasUrlArchivalasok);

            ListArchivator(outputText2Log, valogatottKulsosCimek.ToList(), hibasUrlArchivalasok);

            if (hibasUrlArchivalasok != null && hibasUrlArchivalasok.Count > 0)
            {
                Console.WriteLine("======retry=====");
                outputText2Log.Add("======retry=====");
                ListArchivator(outputText2Log, hibasUrlArchivalasok);
                Console.WriteLine("==mandiner end==");
                outputText2Log.Add("==mandiner end==");
            }

            DateTime mainTimeTrackerStop = DateTime.Now; // Fő stopper leállítása.
            Console.WriteLine($"Mandiner főoldal futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");
            outputText2Log.Add("==================End of Transmission!==================");
            outputText2Log.Add($"Mandiner főoldal futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");

            System.IO.File.WriteAllLines($"mandiner.hu-log-{TimeStamp.Instance.TheTimeStamp}.txt", outputText2Log);

            Console.WriteLine("==================End of Transmission!==================");
        }
        internal void ListArchivator(List<string> outputText2Log, List<string> urlList, List<string> hibasUrlArchivalasok = null)
        {
            int sorszam;
            sorszam = 0;
            foreach (var item in urlList.ToList())
            {
                Activate(sorszam, outputText2Log, item, hibasUrlArchivalasok);
                new System.Threading.ManualResetEvent(false).WaitOne(50000);
                /*
                try
                {
                    WebClient client = new WebClient();
                    Stream data = client.OpenRead("http://web.archive.org/save/" + item);
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    Console.WriteLine(sorszam + ". Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + item);
                    outputText2Log.Add(sorszam + ". Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + item);
                    data.Close();
                    reader.Close();
                    //System.Threading.Thread.Sleep(50000);
                    new System.Threading.ManualResetEvent(false).WaitOne(50000);
                }
                catch (WebException ex)
                {
                    //var response = ex.Response.GetResponseStream();
                    //if (response != null)
                    //{
                    //    Console.WriteLine("Hiba: " + response);
                    //    new StreamReader(response).ReadToEnd();
                    //    client.Dispose();
                    //}
                    Console.WriteLine(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + item);
                    outputText2Log.Add(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + item);
                    if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add(item);
                    new System.Threading.ManualResetEvent(false).WaitOne(35000);
                }
                */

                sorszam++;
            }
        }
        internal async void Activate(int sorszam, List<string> outputText2Log, string urlItem, List<string> hibasUrlArchivalasok = null)
        {
            await Task.Run(() => WebAchivator.OneArchivateAsync(sorszam, outputText2Log, urlItem, hibasUrlArchivalasok));
        }
    }
}
