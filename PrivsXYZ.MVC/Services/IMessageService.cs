using PrivsXYZ.MVC.Models;

namespace PrivsXYZ.MVC.Services
{
    public interface IMessageService
    {
        Task<string> CreateAndEncryptMessage(MessageSendModel model);
        Task<string> DeleteAndDecryptMessage(MessageViewModel model);
        Task<string> Encrypt(string plainTextString, byte[] salt, string encryptionKey);
        Task<string> Decrypt(string encryptedText, byte[] salt, string encryptionKey);
    }
}
