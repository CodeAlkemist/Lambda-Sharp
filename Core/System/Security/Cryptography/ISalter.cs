// In short apache 2 license, check LICENSE file for the legalese

namespace System.Security.Cryptography
{
    public interface ISalt
    {
        byte[] Salt { get; }

        void GenerateSalt();

        void GenerateSalt(int seed);

        void GenerateSalt(byte[] seed);

        void Dispose();
    }
}