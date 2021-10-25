using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubtitleFilesConverterToUTF8
{
    class Program
    {
        static void Main(string[] args)
        {
            //string VidLibraryToPrecess = @"d:\AmazonCD5\DL\Hősök S01-S04 XviD\Heroes.S04.HUN.BDRip.XviD-HSF";
            string VidLibraryToPrecess = @"G:\mos\The.Rain.S01.1080p.NF.WEB-DL.DD5.1.x264-SCIFI";            

            DirectoryInfo vidLibrary = new DirectoryInfo(VidLibraryToPrecess);
            FileInfo[] vidLibraryFiles = vidLibrary.GetFiles();

            List<FileInfo> vidList = vidLibraryFiles.ToList();

            var srtVideos = from srtfiles in vidLibraryFiles.ToList()
                            where srtfiles.Extension == ".srt"
                            select srtfiles;

            foreach (var subItem in srtVideos)
            {
                Console.WriteLine($"iconv -f ISO-8859-2 -t UTF-8//TRANSLIT {subItem.Name} -o {subItem.Name.Replace(".srt", "_utf8.srt")}");
            }

            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}
