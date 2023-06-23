using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Paddings;
using PrivsXYZ.MVC.Database;
using PrivsXYZ.MVC.Database.Entites;
using PrivsXYZ.MVC.Helpers;
using PrivsXYZ.MVC.Models;
using System.Security.Cryptography;

namespace PrivsXYZ.MVC.Services
{
    public class PhotoService
    {
        private readonly PrivsXYZDbContext _dbContext;

        public PhotoService(PrivsXYZDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAndEncryptPhoto(PhotoSendModel model)
        {
            string keyToDecrypt = RandomGeneratorHelper.RandomString(30); //length of key string in URL

            var salt = RandomGeneratorHelper.GetRandomSalt(256);

            PhotoEntity newPhoto = new PhotoEntity()
            {
                CreateDate = DateTime.Now,
                UploaderIPAddress = model.SenderIPv4Address,
                UploaderHostname = model.SenderHostname,
                Photo = await Encrypt(model.Photo!, salt, keyToDecrypt),
                Salt = Convert.ToBase64String(salt)
            };

            await _dbContext.Photo.AddAsync(newPhoto);
            await _dbContext.SaveChangesAsync();
            return $"{newPhoto.Id}@{keyToDecrypt}";
        }

        private async Task<byte[]> Encrypt(byte[] bytes, byte[] salt, string keyToDecrypt)
        {
            string key = keyToDecrypt;
            byte[] encrypted;
            using (var stream = new MemoryStream())
            {
                string password = key;
                string saltString = Convert.ToBase64String(salt);
                Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] keyBytes = keyDerivationFunction.GetBytes(32);
                byte[] ivBytes = keyDerivationFunction.GetBytes(16);
                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;
                    using (var cs = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                    }
                    encrypted = stream.ToArray();
                }
                return encrypted;
            }
        }

        private async Task<byte[]> Decrypt(byte[] bytes, byte[] salt, string keyToDecrypt)
        {
            string key = keyToDecrypt;
            byte[] decrypted;
            using (var stream = new MemoryStream())
            {
                string password = key;
                string saltString = Convert.ToBase64String(salt);
                Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] keyBytes = keyDerivationFunction.GetBytes(32);
                byte[] ivBytes = keyDerivationFunction.GetBytes(16);
                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;
                    using (var cs = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                    }
                    decrypted = stream.ToArray();
                }
                return decrypted;
            }
        }
        
        public async Task<byte[]> DeleteAndDecryptPhoto(string photoId, string key)
        {
            string[] photoIdAndKey = photoId.Split('@');
            if (photoIdAndKey.Length != 2)
            {
                return null;
            }

            var idAsInt = int.Parse(photoIdAndKey[0]);

            var photoEntityInDb =
                await _dbContext.Photo.FirstOrDefaultAsync(f => f.Id == idAsInt);
            if (photoEntityInDb == null)
            {
                return null;
            }
            byte[] salt = Convert.FromBase64String(photoEntityInDb.Salt);
            
            var decryptedPhoto = await Decrypt(photoEntityInDb.Photo!, salt, key);
            _dbContext.Photo.Remove(photoEntityInDb);
            await _dbContext.SaveChangesAsync();

            return decryptedPhoto;
        }
    }
}
