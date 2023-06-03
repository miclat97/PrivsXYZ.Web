using Microsoft.EntityFrameworkCore;
using PrivsXYZ.Web.Database;
using PrivsXYZ.Web.Database.Entity;
using PrivsXYZ.Web.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace PrivsXYZ.Web.Services
{
    public class MessageService : IMessageService
    {
        private readonly PrivsXYZDbContext _dbContext;

        public MessageService(PrivsXYZDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAndEncryptMessage(string message, string ipv4, string ipv6, string hostname)
        {
            string keyToDecrypt = RandomGeneratorHelper.RandomString(30); //length of key string in URL

            var salt = RandomGeneratorHelper.GetRandomSalt(256);

            MessageEntity newMessage = new MessageEntity()
            {
                CreateDate = DateTime.Now,
                UploaderIPAddress = ipv4,
                UploaderHostname = hostname,
                Message = Encrypt(message, salt, keyToDecrypt),
                Salt = salt
            };

            await _dbContext.Message.AddAsync(newMessage);
            await _dbContext.SaveChangesAsync();
            return $"{newMessage.Id}@{keyToDecrypt}";
        }

        public async Task<string> DeleteAndDecryptMessage(int id, string key, string ipv4, string hostname)
        {
            var messageEntityInDb =
                await _dbContext.Message.FirstOrDefaultAsync(f => f.Id == id);
            if (messageEntityInDb == null)
            {
                return "No message with this ID, or bad key!";
            }

            string decryptedMessage;

            try
            {
                decryptedMessage = Decrypt(messageEntityInDb.Message, messageEntityInDb.Salt, key);
            }
            catch (Exception)
            {
                return "No message with this ID, or bad key!";
            }

            try
            {
                messageEntityInDb.Message = null;
                messageEntityInDb.Salt = null;
                messageEntityInDb.ViewerIPAddress = ipv4;
                messageEntityInDb.ViewerHostname = hostname;
                messageEntityInDb.OpenDate = DateTime.Now;
                _dbContext.Message.Update(messageEntityInDb);
                await _dbContext.SaveChangesAsync();
                return decryptedMessage;
            }
            catch (Exception)
            {
                return
                    "Error when trying to delete message from database, please try again or contact with administrator.";
            }
        }


        public string Encrypt(string plainTextString, byte[] salt, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(plainTextString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
                encryptor.Key = pdb.GetBytes(256);
                encryptor.IV = pdb.GetBytes(256);
                encryptor.KeySize = 256;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    plainTextString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return plainTextString;
        }

        public string Decrypt(string encryptedText, byte[] salt, string encryptionKey)
        {
            encryptedText = encryptedText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);
            using (Aes decryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
                decryptor.Key = pdb.GetBytes(256);
                decryptor.IV = pdb.GetBytes(256);
                decryptor.KeySize = 256;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedText;
        }
    }
}