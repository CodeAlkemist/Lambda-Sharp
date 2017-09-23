namespace System.Security.Cryptography
{
    public static class QuickAes
    {
        public static CryptoProvider cp;

        public static void Setup() => cp = CryptoProvider.GetCryptoProvider();

        public static byte[] QuickEncrypt(this byte[] data) => cp.Encrypt(data);

        public static string QuickEncrypt(this string data) => cp.Encrypt(data);

        public static byte[] QuickDecrypt(this byte[] data) => cp.Decrypt(data);

        public static string QuickDecrypt(this string data) => cp.Decrypt(data);

        public static byte[] Key { get => cp.Key; }
        public static byte[] Iv { get => cp.Iv; }
    }
}