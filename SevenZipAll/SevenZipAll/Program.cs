using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
//using System.Reflection;

namespace SevenZipAll
{
    class Program
    {
        static void Main(string[] args)
        {
			//var directoryInfoLista = FileUtils.KonytvarLista(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			var directoryInfoLista = FileUtils.KonytvarLista(Directory.GetCurrentDirectory());

            foreach (var itemToZip in directoryInfoLista)
            {
                var parancssor = FileUtils.SevenZipper(sourceDirName: itemToZip, outSevenZipTargetName: itemToZip.Name);
                Console.WriteLine(parancssor);
            }

			Console.WriteLine("Finished!");
        }
    }
}
