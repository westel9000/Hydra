using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SevenZipAll
{
    public static class FileUtils
    {
        public static List<DirectoryInfo> KonytvarLista(string altualisKonytvar)
        {
            List<DirectoryInfo> directoryInfosList = new List<DirectoryInfo>();

            string[] eredmenyKonyvtarak = Directory.GetDirectories(altualisKonytvar);

            foreach (var item in eredmenyKonyvtarak)
            {
                Console.WriteLine(item);
                DirectoryInfo di = new DirectoryInfo(item);
                directoryInfosList.Add(di);
            }

            return directoryInfosList;
        }

        public static string SevenZipper(DirectoryInfo sourceDirName, string outSevenZipTargetName)
        {
            string sourceName = "ExampleText.txt";
            string targetName = "Example.7z";

            // 1
            // Initialize process information.
            //
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "7za.exe";

            // 2
            // Use 7-zip
            // specify a=archive and -tgzip=gzip
            // and then target file in quotes followed by source file in quotes
            //
            //p.Arguments = "a -t7z \"" + targetName + "\" \"" + sourceName + "\" -mx=1";
            string argumentumok = " a -t7z \"" + outSevenZipTargetName + ".7z\" \"" + sourceDirName + "\" -mx=1";
            p.Arguments = argumentumok;
            //p.Arguments = "a -tgzip \"" + targetName + "\" \"" + sourceName + "\" -mx=9";
            //Console.WriteLine("7za a -t7z \"" + outSevenZipTargetName + ".7z\" \"" + sourceDirName + "\" -mx=1");
            p.WindowStyle = ProcessWindowStyle.Hidden;

            // 3.
            // Start process and wait for it to exit
            //
            Process x = Process.Start(p);
            x.WaitForExit();

            return p.FileName.ToString() + argumentumok;
        }
    }
}
