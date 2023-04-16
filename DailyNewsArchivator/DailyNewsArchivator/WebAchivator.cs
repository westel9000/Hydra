using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http; // windows only
//using System.Threading.Tasks;
//using System.Net.Http;

namespace DailyNewsArchivator
{
    public static class WebAchivator
    {
        internal static async Task OneArchivateAsync(int sorszam, List<string> outputText2Log, string oneurl, List<string> hibasUrlArchivalasok = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri myuri = new Uri("http://web.archive.org/save/" + oneurl);
                Stream data = (await client.GetInputStreamAsync(myuri)).AsStreamForRead();
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                Console.WriteLine(sorszam + ". Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + oneurl);
                outputText2Log.Add(sorszam + ". Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/" + oneurl);
                data.Close();
                reader.Close();
                //System.Threading.Thread.Sleep(50000);
                new System.Threading.ManualResetEvent(false).WaitOne(50000);

            }
            catch (WebException wex)
            {
                Console.WriteLine(sorszam + " Web hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + oneurl);
                outputText2Log.Add(sorszam + " Web hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + oneurl);
                if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add(oneurl);
                new System.Threading.ManualResetEvent(false).WaitOne(35000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + oneurl);
                outputText2Log.Add(sorszam + " Hiba itt " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + oneurl);
                if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add(oneurl);
                new System.Threading.ManualResetEvent(false).WaitOne(35000);
            }
        }
    }
}
