namespace Clinic.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;

    public class CryptoService : IDisposable
    {
        private readonly RijndaelManaged rijndaelManaged;

        public CryptoService(byte[] key)
        {
            this.rijndaelManaged = new RijndaelManaged
            {
                KeySize = key.Length * 8,
                Key = key,
                Padding = PaddingMode.ISO10126,
                Mode = CipherMode.CBC
            };

            this.rijndaelManaged.IV = key.Take(this.rijndaelManaged.BlockSize / 8).ToArray();
        }
        
        public string Encrypt<T>(T data)
        {
            var textInBytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(data));

            using (var encryptor = this.rijndaelManaged.CreateEncryptor())
            {
                var encryptedBytes = encryptor.TransformFinalBlock(textInBytes, 0, textInBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public T Decrypt<T>(string cipher)
        {
            var encryptedBytes = Convert.FromBase64String(cipher);
            using (var decryptor = this.rijndaelManaged.CreateDecryptor())
            {
                var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return JsonConvert.DeserializeObject<T>(Encoding.Unicode.GetString(decryptedBytes));
            }
        }

        public void Dispose()
        {
            this.rijndaelManaged?.Dispose();
        }
    }
}
