namespace System.Security.Cryptography
{
    public static class AesExtentions
    {
        public static CryptoProvider cp = CryptoProvider.GetCryptoProvider();

        public static byte[] QuickEncrypt(this byte[] data, bool generateKeyAndIV = true)
        {
            if (generateKeyAndIV)
                cp.PopulateKeyAndIV();
            return cp.Encrypt(data);
        }

        public static string QuickEncrypt(this string data, bool generateKeyAndIV = true)
        {
            if (generateKeyAndIV)
                cp.PopulateKeyAndIV();
            return cp.Encrypt(data);
        }

        public static byte[] QuickDecrypt(this byte[] data) => cp.Decrypt(data);

        public static string QuickDecrypt(this string data) => cp.Decrypt(data);
    }
}