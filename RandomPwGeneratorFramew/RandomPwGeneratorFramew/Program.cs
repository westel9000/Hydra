using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebMatrix.WebData;
using System.Web.Security;


namespace RandomPwGeneratorFramew
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 1 right click on References in Solution Explorer and press add reference...
             * 2 choose the browse tab and go to C:\Windows\assembly\GAC_32\System.Web\System.Web.dll and add the dll file to your references.
             */
            //System.Web.Security.Membership.GeneratePassword(int length, int numberOfNonAlphanumericCharacters).

            var randomPasswd = Membership.GeneratePassword(22, 5);

            Console.WriteLine(randomPasswd);

            Console.ReadKey();
        }
    }
}
