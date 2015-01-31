using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpgApi;
using System.Security;

namespace GPGTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GpgInterface.ExePath = @"C:\Program Files (x86)\GNU\GnuPG\pub\gpg2.exe";


            GpgImportKey decrypt = new GpgImportKey(@"C:\Users\william\Desktop\gnu.txt");

            {
                // The current thread is blocked until the decryption is finished.
                GpgInterfaceResult result = decrypt.Execute();
                Callback(result);
                System.Console.WriteLine(decrypt.FingerPrint);
                System.Console.ReadLine();
            }

        }

        public static SecureString GetPassword(AskPassphraseInfo info)
        {
            return GpgInterface.GetSecureStringFromString("a");
        }

        public static void Callback(GpgInterfaceResult result)
        {
            if (result.Status == GpgInterfaceStatus.Success)
            {
                //do something if necessary
            }
            else
            {
                // ...
            }
        }
    }
}
