// In short apache 2 license, check LICENSE file for the legalese

using System.IO;
using System.Text;

namespace System.Security.Cryptography
{
    public static class AesExtensions
    {
        private static byte[] _key;
        private static byte[] _iv;

        public static byte[] Key { get => _key;}
        public static byte[] Iv { get => _iv;}

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            var sha = SHA256.Create();
            var md5 = MD5.Create();
            using (var algorithm = Aes.Create())
            using (var encryptor = algorithm.CreateEncryptor(sha.ComputeHash(key), md5.ComputeHash(iv)))
                return Crypt(data, encryptor);
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            try{
                var sha = SHA256.Create();
                var md5 = MD5.Create();
                using (var algorithm = Aes.Create())
                using (var decryptor = algorithm.CreateDecryptor(sha.ComputeHash(key), md5.ComputeHash(iv)))
                    return Crypt(data, decryptor);
            }
            catch(CryptographicException e)
            {
                var ex = new Exception("Failed to decrypt usually means a wrong key or iv", e);
                throw ex;
            }
        }

        public static string Encrypt(string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(
            Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        private static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            var m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
                c.Write(data, 0, data.Length);
            return m.ToArray();
        }

        public static string Decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(
            Decrypt(Convert.FromBase64String(data), key, iv));
        }

        public static byte[] QuickEncrypt(this byte[] data, bool generateKeyAndIV = true)
        {
            if(generateKeyAndIV)
                PopulateKeyAndIV();
            return Encrypt(data, _key, _iv);
        }

        public static string QuickEncrypt(this string data, bool generateKeyAndIV = true)
        {
            if (generateKeyAndIV)
                PopulateKeyAndIV();
            return Encrypt(data, _key, _iv);
        }

        public static byte[] QuickDecrypt(this byte[] data) => Decrypt(data, _key, _iv);

        public static string QuickDecrypt(this string data) => Decrypt(data, _key, _iv);

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used key and iv
        /// </summary>
        public static void PopulateKeyAndIV()
        {
            PopulateKey();
            PopulateIV();
        }

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used key
        /// </summary>
        public static void PopulateIV()
        {
            var rng = RandomNumberGenerator.Create();
            _iv = new byte[64];
            rng.GetBytes(_iv);
        }

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used iv
        /// </summary>
        public static void PopulateKey()
        {
            var rng = RandomNumberGenerator.Create();
            _key = new byte[64];
            rng.GetBytes(_key);
        }

        public static void SetKey(byte[] key) => _key = key;

        public static void SetIV(byte[] iv) => _iv = iv;
    }
}