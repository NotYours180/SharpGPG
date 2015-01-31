using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpgApi;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace SharpGPG
{
    class Program
    {
        const string ExePath = @"C:\Program Files (x86)\GNU\GnuPG\pub\gpg2.exe";
        const string defaultsign = "79B4DA8B36D17F856BA7288079AC04CAB334A781";

        public static string encryptString(string toEncrypt, string target, string sign = defaultsign)
        {
            CipherAlgorithm algorithm = CipherAlgorithm.Aes256;
            List<KeyId> recipients = new List<KeyId>();
            recipients.Add(new KeyId(target));

            KeyId signkey = new KeyId(sign);

            GpgInterface.ExePath = ExePath;

            string path = Directory.GetCurrentDirectory() + GetUniqueKey() + ".txt";
            string pathout = path + ".out";

            System.IO.File.WriteAllText(path, toEncrypt);

            GpgEncrypt encrypt = new GpgEncrypt(path, pathout, true, false, signkey, recipients, algorithm);

            GpgInterfaceResult result = encrypt.Execute();

            System.IO.File.Delete(path);

            if (result.Status == GpgInterfaceStatus.Success)
            {
                string toReturn = System.IO.File.ReadAllText(pathout);
                System.IO.File.Delete(pathout);
                return toReturn;
            }
            else
            {
                throw new Exception("Import Failed");
            }

        }

        public static GpgImportKey importKey(string publickey)
        {
            GpgInterface.ExePath = ExePath;
            
            string path = Directory.GetCurrentDirectory() + GetUniqueKey() + ".txt";

            System.IO.File.WriteAllText(path, publickey);

            GpgImportKey import = new GpgImportKey(path);

            
            GpgInterfaceResult result = import.Execute();

            System.IO.File.Delete(path);

            if (result.Status == GpgInterfaceStatus.Success)
            {
                return import;
            }
            else
            {
                throw new Exception("Import Failed");
            }
            
        }

        public static SecureString GetPassword(AskPassphraseInfo info)
        {
            return GpgInterface.GetSecureStringFromString("a");
        }

        public static string GetUniqueKey(int maxSize = 16)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

    }
}
