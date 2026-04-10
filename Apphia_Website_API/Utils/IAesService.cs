namespace Apphia_Website_API.Utils
{
    public interface IAesService
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}
