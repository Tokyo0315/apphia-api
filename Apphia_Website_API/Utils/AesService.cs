using System.Security.Cryptography;
using System.Text;

namespace Apphia_Website_API.Utils {
    // IAesService → Utils/IAesService.cs

    public class AesService(string key) : IAesService {
        private readonly string _key = key;

        public string Encrypt(string plainText) {
            using Aes aes = Aes.Create();
            string normalizedKey = _key.PadRight(32).Substring(0, 32);
            byte[] keyBytes = Encoding.UTF8.GetBytes(normalizedKey);
            aes.Key = keyBytes;
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs)) { sw.Write(plainText); }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText) {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            using Aes aes = Aes.Create();
            string normalizedKey = _key.PadRight(32).Substring(0, 32);
            byte[] keyBytes = Encoding.UTF8.GetBytes(normalizedKey);
            aes.Key = keyBytes;
            byte[] iv = fullCipher.Take(16).ToArray();
            byte[] cipher = fullCipher.Skip(16).ToArray();
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
