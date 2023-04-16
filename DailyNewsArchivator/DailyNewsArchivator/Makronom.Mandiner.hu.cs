using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace DailyNewsArchivator
{
    /// <summary>
    /// Nemcsak makronom.mandiner.hu alrovat számára hanem
    /// precedens.mandiner.hu és sport.mandiner.hu alrovatok számára is.
    /// </summary>
    public class Makronom
    {
        public void ArchiveAll(string urlAlapCim)
        {
            Console.WriteLine($"{urlAlapCim.Split('.').FirstOrDefault().Replace("https://", "")}.mandiner.hu archiválása {Environment.NewLine}");

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
                Stream data = client.OpenRead(urlAlapCim);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                //Console.WriteLine(s);
                data.Close();
                reader.Close();
            }
            catch (Exception)
            {
                Console.WriteLine($"Nem érhető el a {urlAlapCim} így archiválni sem lehet.");
                outputText2Log.Add($"Nem érhető el a {urlAlapCim} így archiválni sem lehet.");
            }

            //Console.WriteLine(s); // Teljes webtartalom kiírása

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
                                         select urlAlapCim + elem;

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
                Stream data = client.OpenRead("http://web.archive.org/save/" + urlAlapCim);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                Console.WriteLine("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + urlAlapCim);
                outputText2Log.Add("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + urlAlapCim);
                data.Close();
                reader.Close();
                //System.Threading.Thread.Sleep(50000);
                new System.Threading.ManualResetEvent(false).WaitOne(50000);
            }
            catch (Exception)
            {
                Console.WriteLine(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + urlAlapCim);
                outputText2Log.Add(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + urlAlapCim);
                if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add(urlAlapCim);
            }



            Mandiner mandinerSeged = new Mandiner(); // Az archiválást végző .ListArchivator() a Mandiner osztályban van, már így marad

            mandinerSeged.ListArchivator(outputText2Log, valogatottFooldalCimek.ToList(), hibasUrlArchivalasok);

            mandinerSeged.ListArchivator(outputText2Log, valogatottAlrovatCimek.ToList(), hibasUrlArchivalasok);

            mandinerSeged.ListArchivator(outputText2Log, valogatottKulsosCimek.ToList(), hibasUrlArchivalasok);

            if (hibasUrlArchivalasok != null)
            {
                mandinerSeged.ListArchivator(outputText2Log, hibasUrlArchivalasok);
            }

            DateTime mainTimeTrackerStop = DateTime.Now; // Fő stopper leállítása.
            Console.WriteLine($"Mandiner {urlAlapCim.Split('.').FirstOrDefault().Replace("https://", "")} futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");
            outputText2Log.Add($"Mandiner {urlAlapCim.Split('.').FirstOrDefault().Replace("https://", "")} futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");

            System.IO.File.WriteAllLines($"{urlAlapCim.Split('.').FirstOrDefault().Replace("https://", "")}.mandiner.hu-log-{TimeStamp.Instance.TheTimeStamp}.txt", outputText2Log);

            Console.WriteLine("==================End of Transmission!==================");
        }        
    }
}
