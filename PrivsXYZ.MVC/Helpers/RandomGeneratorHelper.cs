using System.Security.Cryptography;

namespace PrivsXYZ.MVC.Helpers
{
    public static class RandomGeneratorHelper
    {
        public static string RandomString()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var bytes = new byte[128 / 8];
            randomNumberGenerator.GetBytes(bytes);
            return bytes.ToString()!;
        }

        public static byte[] GetRandomSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var bytes = new byte[128 / 8];
            randomNumberGenerator.GetBytes(bytes);
            return bytes;
        }
    }
}
