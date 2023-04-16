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
    /// Nemcsak makronom.moszkvater.com alrovat számára hanem
    /// precedens.moszkvater.com és sport.moszkvater.com alrovatok számára is.
    /// </summary>
    public class Moszkvater
    {
        public void ArchiveAll(string urlAlapCim)
        {
            Console.WriteLine($"moszkvater.com archiválása {Environment.NewLine}");

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
                //client.DownloadString  // Cserélni erre!! @@@ SzaboZs
                Stream data = client.OpenRead(urlAlapCim);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                //Console.WriteLine(s);
                data.Close();
                reader.Close();
            }
            catch (Exception) // networkre cserélni @@@ SzaboZs
            {
                Console.WriteLine($"Nem érhető el a {urlAlapCim} így archiválni sem lehet.");
                outputText2Log.Add($"Nem érhető el a {urlAlapCim} így archiválni sem lehet.");              
            }           

            //Console.WriteLine(s); // Teljes webtartalom kiírása

            //Console.ReadKey();

            string[] urlCimek = s.Split("href=\"");

            List<string> rejectLista = new List<string>
                {
                    "https://moszkvater.com/impresszum/",
                    "https://moszkvater.com/feed/",
                    "https://moszkvater.com/comments/feed/",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/jopvv1e4/btr73.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/felmhspx/4hkga.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/g4kc9xki/btr73.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/lnejur16/btr73.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/8aaa6nav/btr73.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/2yi2is08/btr73.css",
                    "//moszkvater.com/wp-content/cache/wpfc-minified/fs7t38zs/4hkga.css",
                    "https://moszkvater.com/wp-json/",
                    "https://moszkvater.com/wp-json/wp/v2/pages/121",
                    "https://moszkvater.com/xmlrpc.php?rsd",
                    "https://moszkvater.com/wp-includes/wlwmanifest.xml",
                    "https://moszkvater.com/wp-json/oembed/1.0/embed?url=https%3A%2F%2Fmoszkvater.com%2F",
                    "https://moszkvater.com/wp-json/oembed/1.0/embed?url=https%3A%2F%2Fmoszkvater.com%2F&#038;format=xml",
                    "https://moszkvater.com/wp-content/uploads/2018/06/cropped-favicon-1-32x32.jpg",
                    "https://moszkvater.com/wp-content/uploads/2018/06/cropped-favicon-1-192x192.jpg",
                    "https://moszkvater.com/wp-content/uploads/2018/06/cropped-favicon-1-180x180.jpg",
                    "http://vitallon.hu",
                    "http://www.1000ut.hu",
                    "https://www.cookieyes.com/",
                    "https://moszkvater.com/checkout/",
                    "https://moszkvater.com/order-confirmation/",
                    "https://moszkvater.com/order-failed/",
                    "https://moszkvater.com",
                    "mailto:info@moszkvater.com",
                    "https://moszkvater.com/adatkezelesi-tajekoztato/",
                    "https://moszkvater.com/suti-szabalyzat-cookie-policy/"
                };

            var valogatottCimek = (from elem in urlCimek
                                   where elem.Contains("moszkvater.com") && !elem.Contains("<!DOCTYPE html>")
                                   select Regex.Replace(elem.Split('"')
                                  .FirstOrDefault(), "#comments$", "")
                                  )
                                  .Distinct()
                                  .Except(rejectLista);

            var valogatottMoszkvaterCimek = (from elem in valogatottCimek
                                          where elem.Contains("moszkvater.com") && !elem.Contains("moszkvater.com/cimke/") && !elem.Contains("<!DOCTYPE html>")
                                          select elem)
                                         .Except(rejectLista);

            var valogatottCimkekMoszkvater = (from elem in valogatottCimek
                                             where elem.Contains("moszkvater.com") && elem.Contains("moszkvater.com/cimke/") && !elem.Contains("<!DOCTYPE html>")
                                             select elem)
                             .Except(rejectLista);

            //var eredmeny = valogatottAlrovatCimek.ToList().RemoveAll(x => x.Contains("utm_source="));

            int sorszam = 0;
            foreach (var item in valogatottCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            Console.WriteLine("========================================================");

            Console.WriteLine("========================================================");
            sorszam = 0;

            foreach (var item in valogatottMoszkvaterCimek)
            {
                Console.WriteLine(sorszam + ". " + item);
                sorszam++;
            }

            if (TimeStamp.Instance.TheDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("========================================================");
                sorszam = 0;

                foreach (var item in valogatottCimkekMoszkvater)
                {
                    Console.WriteLine(sorszam + ". " + item);
                    sorszam++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("=========================BEGIN==========================");
            outputText2Log.Add("=========================BEGIN==========================");

            // A mandiner.hu főoldallal szemben a moszkvater.com főoldal belekerül a valogatottMoszkvaterCimek listába, így felesleges külön előbb lementeni a főoldalát.
            //try
            //{
            //    Stream data = client.OpenRead("http://web.archive.org/save/" + urlAlapCim);
            //    StreamReader reader = new StreamReader(data);
            //    s = reader.ReadToEnd();
            //    Console.WriteLine("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + urlAlapCim);
            //    outputText2Log.Add("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + urlAlapCim);
            //    data.Close();
            //    reader.Close();
            //    System.Threading.Thread.Sleep(50000);
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine(sorszam + " Hiba itt: " + urlAlapCim);
            //    outputText2Log.Add(sorszam + " Hiba itt: " + urlAlapCim);
            //    if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add(urlAlapCim);
            //}

            Mandiner mandinerSeged = new Mandiner(); // Az archiválást végző .ListArchivator() a Mandiner osztályban van, már így marad

            mandinerSeged.ListArchivator(outputText2Log, valogatottMoszkvaterCimek.ToList(), hibasUrlArchivalasok);

            if (TimeStamp.Instance.TheDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                mandinerSeged.ListArchivator(outputText2Log, valogatottCimkekMoszkvater.ToList(), hibasUrlArchivalasok);
            }

            if (hibasUrlArchivalasok != null && hibasUrlArchivalasok.Count > 0)
            {
                Console.WriteLine("======retry=======");
                outputText2Log.Add("======retry=======");
                mandinerSeged.ListArchivator(outputText2Log, hibasUrlArchivalasok);
                Console.WriteLine("==moszkvater end==");
                outputText2Log.Add("==moszkvater end==");
            }

            DateTime mainTimeTrackerStop = DateTime.Now; // Fő stopper leállítása.
            outputText2Log.Add("==================End of Transmission!==================");
            Console.WriteLine($"Moszkvater.com futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");
            outputText2Log.Add($"Moszkvater.com futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");

            System.IO.File.WriteAllLines($"moszkvater.com-log-{TimeStamp.Instance.TheTimeStamp}.txt", outputText2Log);

            Console.WriteLine("==================End of Transmission!==================");
        }
    }
}
