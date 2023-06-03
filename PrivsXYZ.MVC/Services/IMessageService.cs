namespace PrivsXYZ.MVC.Services
{
    public interface IMessageService
    {
        Task<string> CreateAndEncryptMessage(string message, string ipv4, string ipv6, string hostname);
        Task<string> DeleteAndDecryptMessage(int id, string key, string ipv4, string hostname);
        string Encrypt(string plainTextString, byte[] salt, string encryptionKey);
        string Decrypt(string encryptedText, byte[] salt, string encryptionKey);
    }
}
