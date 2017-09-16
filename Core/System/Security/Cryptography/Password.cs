// In short apache 2 license, check LICENSE file for the legalese

using System.Linq;

namespace System.Security.Cryptography
{
    /// <summary>
    /// Note: !!!THIS CLASS IS ONLY AS SECURE AS THE PASSWORD HANDLING DONE BEFORE IT!!!
    /// </summary>
    public class Password
    {
        private readonly byte[] _hash;
        private readonly byte[] _salt;

        public Password(byte[] rawBytes, ISalt salt)
        {
            var sha = SHA512.Create();
            _salt = salt.Salt;
            _hash = sha.ComputeHash(rawBytes);
            _hash.ToList().AddRange(Salt);
            _hash = sha.ComputeHash(_hash);
        }

        public static byte[] CheckPassword(byte[] rawBytes, byte[] salt)
        {
            var sha_ = SHA512.Create();
            var salt_ = salt;
            var hash = sha_.ComputeHash(rawBytes);
            hash.ToList().AddRange(salt_);
            hash = sha_.ComputeHash(hash);
            return hash;
        }

        public byte[] Salt => _salt;

        public byte[] Hash => _hash;
    }
}