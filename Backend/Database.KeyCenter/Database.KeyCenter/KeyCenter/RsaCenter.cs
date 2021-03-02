using Database.KeyCenter.Entity;
using EasyEncrypt.RSA;
using System.Security.Cryptography;

namespace Database.KeyCenter.KeyCenter
{
    public class RsaCenter
    {
        public static Keys GetKeys()
        {
            var rsa = RSA.Create();
            var rsaProperties = rsa.ExportParameters(true);

            return new Keys()
            {
                D = rsaProperties.D,
                DP = rsaProperties.DP,
                DQ = rsaProperties.DQ,
                Exponent = rsaProperties.Exponent,
                InverseQ = rsaProperties.InverseQ,
                Modulus = rsaProperties.Modulus,
                P = rsaProperties.P,
                Q = rsaProperties.Q
            };
        }

        public static byte[] Sign(byte[] hash, Keys keys)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(ToRSAParameters(keys));

            var sign = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
            return sign;
        }

        public static bool Verify(byte[] hash, byte[] sign, Keys keys)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(ToRSAParameters(keys));

            var isSignValid = rsa.VerifyHash(hash, sign, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
            return isSignValid;
        }

        private static RSAParameters ToRSAParameters(Keys keys)
        {
            var rsaProperties = new RSAParameters()
            {
                D = keys.D,
                DP = keys.DP,
                DQ = keys.DQ,
                Exponent = keys.Exponent,
                InverseQ = keys.InverseQ,
                Modulus = keys.Modulus,
                P = keys.P,
                Q = keys.Q
            };
            return rsaProperties;
        }
    }
}
