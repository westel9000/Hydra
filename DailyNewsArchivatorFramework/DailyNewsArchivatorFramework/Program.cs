﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace DailyNewsArchivatorFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeStamp TheTimeStamp = TimeStamp.Instance;
            DateTime mainTimeTrackerStart = DateTime.Now; // Fő stopper indítása.

            CsakFooldalakStarter csakFooldalakStarter = new CsakFooldalakStarter();
            Thread csakfooldalakThread = new Thread(new ThreadStart(csakFooldalakStarter.ArchiveFooldalak));
            csakfooldalakThread.Start();

            Moszkvater moszkvater = new Moszkvater();
            moszkvater.ArchiveAll("https://moszkvater.com");

            Mandiner mandiner = new Mandiner();
            mandiner.ArchiveAll();

            if ((TheTimeStamp.TheDateTime.DayOfWeek == DayOfWeek.Saturday))
            {
                Makronom makronom = new Makronom();
                makronom.ArchiveAll("https://makronom.mandiner.hu");
                makronom.ArchiveAll("https://precedens.mandiner.hu");
                makronom.ArchiveAll("https://sport.mandiner.hu");
            }

            DateTime mainTimeTrackerStop = DateTime.Now; // Fő stopper leállítása.
            Console.WriteLine($"Teljes futásidő: {mainTimeTrackerStop - mainTimeTrackerStart}");

            Console.WriteLine("End of Transmission!");
        }
    }
}
