using GpgApi;

namespace GPGTest
{
    public interface IGPG
    {
        string encryptString(string toEncrypt, string target, string sign = defaultsign, CipherAlgorithm algorithm = CipherAlgorithm.Aes256, bool armour = true, bool hideuserid = false);
        GpgImportKey importKey(string publickey);
    }
}
