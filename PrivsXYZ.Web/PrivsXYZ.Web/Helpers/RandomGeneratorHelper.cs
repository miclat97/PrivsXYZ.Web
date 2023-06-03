namespace PrivsXYZ.Web.Helpers
{
    public static class RandomGeneratorHelper
    {
        public static string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public static byte[] GetRandomSalt(int sizeInBytes)
        {
            Random rnd = new Random();
            byte[] b = new byte[sizeInBytes];
            rnd.NextBytes(b);
            return b;
        }
    }
}
