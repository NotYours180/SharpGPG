using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using GpgApi;

namespace SharpGPG
{
    public class IGPGService : IGPG
    {
        private string ExePath;
        private string defaultsign;
        private string password;

        public IGPGService(string ExePath, string defaultsign, string password)
        {
            this.ExePath = ExePath;
            this.defaultsign = defaultsign;
            this.password = password;
        }

        public string encryptString(string toEncrypt, string target, string sign = null, CipherAlgorithm algorithm = CipherAlgorithm.Aes256, bool armour = true, bool hideuserid = false)
        {            
            List<KeyId> recipients = new List<KeyId>();
            recipients.Add(new KeyId(target));            

            KeyId signkey = new KeyId(defaultsign);

            GpgInterface.ExePath = ExePath;

            string path = Directory.GetCurrentDirectory() + "\\" + GetUniqueKey() + ".txt";
            string pathout = path + ".out";

            System.IO.File.WriteAllText(path, toEncrypt);

            GpgEncrypt encrypt = new GpgEncrypt(path, pathout, armour, hideuserid, signkey, recipients, algorithm);

            encrypt.AskPassphrase = GetPassword;

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
                throw new Exception("Encryption Failed");
            }

        }

        public GpgImportKey importKey(string publickey)
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

        public SecureString GetPassword(AskPassphraseInfo info)
        {
            return GpgInterface.GetSecureStringFromString(password);
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
