// In short apache 2 license, check LICENSE file for the legalese

using System.IO;
using System.Text;
using System.Threading;
using Lambda;

namespace System.Security.Cryptography
{
    public class CryptoProvider
    {
        private byte[] _key;
        private byte[] _iv;

        private CryptoProvider(byte[] key, byte[] iv)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _iv = iv ?? throw new ArgumentNullException(nameof(iv));
        }

        private CryptoProvider()
        {
        }

        public byte[] Key { get => _key; }
        public byte[] Iv { get => _iv; }

        public static CryptoProvider GetCryptoProvider()
        {
            var cp = new CryptoProvider();
            cp.PopulateKeyAndIV();
            return cp;
        }

        public static CryptoProvider GetCryptoProvider(byte[] key, byte[] iv)
        {
            var cp = new CryptoProvider(key, iv);
            return cp;
        }

        public byte[] Encrypt(byte[] data)
        {
            var sha = SHA256.Create();
            var md5 = MD5.Create();
            using (var algorithm = Aes.Create())
            using (var encryptor = algorithm.CreateEncryptor(sha.ComputeHash(_key), md5.ComputeHash(_iv)))
                return Crypt(data, encryptor);
        }

        public byte[] Decrypt(byte[] data)
        {
            try
            {
                var sha = SHA256.Create();
                var md5 = MD5.Create();
                using (var algorithm = Aes.Create())
                using (var decryptor = algorithm.CreateDecryptor(sha.ComputeHash(_key), md5.ComputeHash(_iv)))
                    return Crypt(data, decryptor);
            }
            catch (CryptographicException e)
            {
                var ex = new Exception("Failed to decrypt usually means a wrong key or iv", e);
                throw ex;
            }
        }

        private byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            var m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
                c.Write(data, 0, data.Length);
            return m.ToArray();
        }

        public string Encrypt(string data)
        {
            return Encrypt(Encoding.UTF8.GetBytes(data)).ToBase64();
        }

        public string Decrypt(string data)
        {
            return Encoding.UTF8.GetString(Decrypt(data.FromBase64()));
        }

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used key and iv
        /// </summary>
        public void PopulateKeyAndIV()
        {
            PopulateKey();
            PopulateIV();
        }

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used key
        /// </summary>
        public void PopulateIV()
        {
            var rng = RandomNumberGenerator.Create();
            _iv = new byte[64];
            rng.GetBytes(_iv);
        }

        /// <summary>
        /// Warning: This function will overwrite the internal state of this class so make sure to save any previously used iv
        /// </summary>
        public void PopulateKey()
        {
            var rng = RandomNumberGenerator.Create();
            _key = new byte[64];
            rng.GetBytes(_key);
        }

        public void SetKey(byte[] key) => _key = key;

        public void SetIV(byte[] iv) => _iv = iv;

        public void EncryptAsync(byte[] data)
        {
            byte[] result;
            var t = new Thread(() =>
            {
                result = this.Encrypt(data);
                EncryptionDone.Invoke(result);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        public delegate void EncryptionFinished(byte[] result);

        public event EncryptionFinished EncryptionDone;
    }
}