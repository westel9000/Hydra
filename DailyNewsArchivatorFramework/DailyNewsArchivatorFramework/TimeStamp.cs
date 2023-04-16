using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsArchivatorFramework
{
    /// <summary>
    /// Singleton Időbélyegző osztály.
    /// </summary>
    public sealed class TimeStamp
    {
        private static TimeStamp instance = null;
        private static readonly object padlock = new object();

        public string TheTimeStamp { get; set; }

        public DateTime TheDateTime { get; set; }

        public static TimeStamp Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TimeStamp();
                    }
                    return instance;
                }
            }
        }
        private TimeStamp()
        {
            this.TheTimeStamp = TimeStampOnly();
            this.TheDateTime = DateTime.Now;
        }

        /// <summary>
        /// DateTime.Now időbélyegzővel látja el a paraméterben megkapott fájl nevét.
        /// </summary>
        /// <param name="fileNameAndPath">string Fájlnév teljes path -al.</param>
        /// <returns>string Időbélyeggel ellátott fájlnév teljes path- al.</returns>

        public string TimeStampToFilename(string fileNameAndPath)
        {
            Path.GetFileName(fileNameAndPath);
            var returnPathAndFilenameValue = Path.GetDirectoryName(fileNameAndPath) + "\\" + Path.GetFileNameWithoutExtension(fileNameAndPath) +
                "_" + this.TheTimeStamp + Path.GetExtension(fileNameAndPath);
            return returnPathAndFilenameValue;
        }

        /// <summary>      
        /// DateTime.Now -ból képezett időbélyegzőt ad vissza stringben.
        /// A teljes alkalmazás szintjén itt lehet központilag beállítani a könyvtárnevekbe, 
        /// fájlnevekbe kerülő időbélyegzők megjelenését, illetve logokban szereplő időbélyegzőket is.
        /// Jelenleg Magyarországon használatos ÉvHóNapÓraPercMásodperc megjelenésre beállítva.
        /// </summary>
        /// <returns>string Időbélyeg.</returns>
        public string TimeStampOnly()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }
    }
}
