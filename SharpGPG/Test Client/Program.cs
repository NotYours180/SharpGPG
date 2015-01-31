using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGPG;

namespace Test_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IGPGService gpg = new IGPGService(@"C:\Program Files (x86)\GNU\GnuPG\pub\gpg.exe", "79B4DA8B36D17F856BA7288079AC04CAB334A781", "testpassword");

            string test = gpg.encryptString("test text","DB99C147E15EB898C0D5C81635DB284E21D84A25");

            System.Console.WriteLine(test);
            System.Console.ReadLine();
        }
    }
}
