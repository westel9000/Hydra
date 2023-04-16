using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading_Sleep_replacement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Váralkozás kezdődik");
            new System.Threading.ManualResetEvent(false).WaitOne(5000);
            Console.WriteLine("Várakozás befejeződött");

            Console.ReadKey();
        }
    }
}
