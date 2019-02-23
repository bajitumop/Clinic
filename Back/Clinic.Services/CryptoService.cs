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
                return BytesToString(encryptedBytes);
            }
        }

        public T Decrypt<T>(string cipher)
        {
            var encryptedBytes = StringToBytes(cipher);
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

        private static string BytesToString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", string.Empty);
        }

        private static byte[] StringToBytes(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
