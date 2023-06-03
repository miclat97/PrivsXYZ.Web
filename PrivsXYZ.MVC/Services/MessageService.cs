using Microsoft.EntityFrameworkCore;
using PrivsXYZ.MVC.Database;
using PrivsXYZ.MVC.Database.Entites;
using PrivsXYZ.MVC.Helpers;
using PrivsXYZ.MVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace PrivsXYZ.MVC.Services
{
    public class MessageService : IMessageService
    {
        private readonly PrivsXYZDbContext _dbContext;

        public MessageService(PrivsXYZDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAndEncryptMessage(MessageSendModel model)
        {
            string keyToDecrypt = RandomGeneratorHelper.RandomString(30); //length of key string in URL

            var salt = RandomGeneratorHelper.GetRandomSalt(256);

            MessageEntity newMessage = new MessageEntity()
            {
                CreateDate = DateTime.Now,
                UploaderIPAddress = model.SenderIPv4Address,
                UploaderHostname = model.SenderHostname,
                Message = await Encrypt(model.Message!, salt, keyToDecrypt),
                Salt = salt
            };

            await _dbContext.Message.AddAsync(newMessage);
            await _dbContext.SaveChangesAsync();
            return $"{newMessage.Id}@{keyToDecrypt}";
        }

        public async Task<string> DeleteAndDecryptMessage(MessageViewModel model)
        {
            var messageEntityInDb =
                await _dbContext.Message.FirstOrDefaultAsync(f => f.Id == model.MessageId);
            if (messageEntityInDb == null)
            {
                return "Brak wiadomości o tym ID bądź nieprawidłowy klucz!";
            }

            string decryptedMessage;

            try
            {
                decryptedMessage = await Decrypt(messageEntityInDb.Message!, messageEntityInDb.Salt!, model.Key!);
            }
            catch (Exception)
            {
                return "Brak wiadomości o tym ID bądź nieprawidłowy klucz!";
            }

            try
            {
                messageEntityInDb.Message = null;
                messageEntityInDb.Salt = null;
                messageEntityInDb.ViewerIPAddress = model.ViewerIPv4Address;
                messageEntityInDb.ViewerHostname = model.ViewerHostname;
                messageEntityInDb.OpenDate = DateTime.Now;
                _dbContext.Message.Update(messageEntityInDb);
                await _dbContext.SaveChangesAsync();
                return decryptedMessage;
            }
            catch (Exception)
            {
                return
                    "Wystąpił błąd podczas próby usunięcia wiadomości po jej odczytaniu z bazy danych." +
                    " Spróbuj ponownie.";
            }
        }


        public async Task<string> Encrypt(string plainTextString, byte[] salt, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(plainTextString);
            string encryptedString;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptedString;
        }

        public async Task<string> Decrypt(string encryptedText, byte[] salt, string encryptionKey)
        {
            encryptedText = encryptedText.Replace(" ", "+");
            string decryptedText;
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);
            using (Aes decryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
                decryptor.Key = pdb.GetBytes(32);
                decryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(cipherBytes, 0, cipherBytes.Length);
                        //cs.Close();
                    }
                    decryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return decryptedText;
        }
    }
}
