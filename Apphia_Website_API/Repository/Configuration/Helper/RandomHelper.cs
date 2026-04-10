using System.Text;

namespace Apphia_Website_API.Repository.Configuration.Helper
{
    public class RandomHelper
    {
        private readonly Random _random = new Random();
        private readonly string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string GenerateRandomString(int length)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(_characters[_random.Next(_characters.Length)]);
            }
            return result.ToString();
        }
    }
}
