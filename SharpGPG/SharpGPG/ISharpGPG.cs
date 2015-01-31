using GpgApi;

namespace SharpGPG
{
    public interface IGPG
    {
        string encryptString(string toEncrypt, string target, string sign = null, CipherAlgorithm algorithm = CipherAlgorithm.Aes256, bool armour = true, bool hideuserid = false);
        GpgImportKey importKey(string publickey);
        string decryptString(string toDecrypt);
    }
}
