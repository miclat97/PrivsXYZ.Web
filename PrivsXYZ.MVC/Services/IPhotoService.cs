using PrivsXYZ.MVC.Models;

namespace PrivsXYZ.MVC.Services
{
    public interface IPhotoService
    {
        Task<string> CreateAndEncryptPhoto(PhotoSendModel model);
        Task<byte[]> Encrypt(byte[] bytes, byte[] salt, string keyToDecrypt);
        Task<byte[]> Decrypt(byte[] bytes, byte[] salt, string keyToDecrypt);
        Task<string> DeleteAndDecryptPhoto(PhotoViewModel model);
    }
}
