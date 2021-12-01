using PasswordGenerator;
using System;
using System.Web;
using System.Web.Mobile;

namespace RandomPasswdGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomPasswd = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().LengthRequired(128);
            var result = randomPasswd.Next();

            Console.WriteLine(result);
        }
    }
}
