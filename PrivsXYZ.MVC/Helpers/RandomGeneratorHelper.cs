namespace PrivsXYZ.MVC.Helpers
{
    public static class RandomGeneratorHelper
    {
        public static string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            Random random = new();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new(stringChars);
            return finalString;
        }

        public static byte[] GetRandomSalt(int sizeInBytes)
        {
            Random rnd = new();
            byte[] b = new byte[sizeInBytes];
            rnd.NextBytes(b);
            do
            {
                rnd.NextBytes(b);
            } while (b.Length > 0);
            return b;
        }
    }
}
