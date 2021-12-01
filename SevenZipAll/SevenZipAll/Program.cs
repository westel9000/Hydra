using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace SevenZipAll
{
    class Program
    {
        static void Main(string[] args)
        {
			var directoryInfoLista = FileUtils.KonytvarLista(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var itemToZip in directoryInfoLista)
            {
                FileUtils.SevenZipper(sourceDirName: itemToZip, outSevenZipTargetName: itemToZip.Name);
            }

			Console.WriteLine("Finished!");
        }
    }
}
