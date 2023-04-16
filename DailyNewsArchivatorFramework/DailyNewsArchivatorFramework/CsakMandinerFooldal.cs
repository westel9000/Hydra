﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace DailyNewsArchivatorFramework
{
    public class CsakMandinerFooldal
    {
        static readonly object lockObject = new object();

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

            Console.WriteLine();
            //Console.WriteLine("=========================BEGIN==========================");
            outputText2Log.Add("=========================BEGIN==========================");

            lock (lockObject)
            {
                try
                {
                    Stream data = client.OpenRead("http://web.archive.org/save/https://mandiner.hu/");
                    StreamReader reader = new StreamReader(data);
                    //s = reader.ReadToEnd();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("Archiválva csak főoldal " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/https://mandiner.hu/");
                    Console.ResetColor();
                    Console.Write(Environment.NewLine);
                    outputText2Log.Add("Archiválva " + DateTime.Now.ToString("yyMMdd.HHmm") + ": " + "http://web.archive.org/save/https://mandiner.hu/");
                    data.Close();
                    reader.Close();
                    System.Threading.Thread.Sleep(50000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Hiba itt {DateTime.Now.ToString("yyMMdd.HHmm")}: https://mandiner.hu/");
                    outputText2Log.Add($" Hiba itt {DateTime.Now.ToString("yyMMdd.HHmm")}: https://mandiner.hu/");
                    if (hibasUrlArchivalasok != null) hibasUrlArchivalasok.Add("https://mandiner.hu/");
                }
            }

            Mandiner mandinerSeged = new Mandiner(); // Az archiválást végző .ListArchivator() a Mandiner osztályban van, már így marad

            lock (lockObject)
            {
                if (hibasUrlArchivalasok != null)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    mandinerSeged.ListArchivator(outputText2Log, hibasUrlArchivalasok);
                    Console.ResetColor();
                }
            }
            DateTime mainTimeTrackerStop = DateTime.Now; // Fő stopper leállítása.
            //Console.WriteLine($"Mandiner csak a főoldal futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");
            outputText2Log.Add($"Mandiner csak a főoldal futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");

            outputText2Log.Add("==================End of Transmission!==================");
            System.IO.File.WriteAllLines($"mandiner.hu-fooldal-log-{TimeStamp.Instance.TheTimeStamp}.txt", outputText2Log);
        }
    }
}