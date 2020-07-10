using System;
using System.Security.Cryptography;
using System.Text;

namespace Tracking.Common.Extensions
{
    public static class CryptoExtensions
    {
        private const bool _optimalAsymmetricEncryptionPadding = false;
        private const string PublicXML = "2048!<RSAKeyValue><Modulus>eUnS7mZ/UQjR7lpXHqfWqTDR9+hDZqiC395u+gBZ3F3TBp/pwxYhi9prySYwX5U5ATz5Pgxrgg00LSLv0Cpr5dfP5BNNhAyerTB3zoxxQqau3f0tg+vEiuigCBOglqu/otRwx0rPgEt8C8VqkmxiUbMTllmma5uf1hF5bso3qRJqckv4doTj7OJ2DogYRyhFTN3BfqpvqPboS8MKrAY4l9qv/mGYq3aPgpBA0LNv0LI2DRCI4J1gxlvH+rVdB3QQSxn8ambIyrrif8El+XExGwg+DiYkNccaIomnzZ1Xo1g6pp8c3IH4qRK8ktGtacy1eLcGXFqsTgAOWvHFkLFCmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string PrivateXML = "2048!<RSAKeyValue><Modulus>eUnS7mZ/UQjR7lpXHqfWqTDR9+hDZqiC395u+gBZ3F3TBp/pwxYhi9prySYwX5U5ATz5Pgxrgg00LSLv0Cpr5dfP5BNNhAyerTB3zoxxQqau3f0tg+vEiuigCBOglqu/otRwx0rPgEt8C8VqkmxiUbMTllmma5uf1hF5bso3qRJqckv4doTj7OJ2DogYRyhFTN3BfqpvqPboS8MKrAY4l9qv/mGYq3aPgpBA0LNv0LI2DRCI4J1gxlvH+rVdB3QQSxn8ambIyrrif8El+XExGwg+DiYkNccaIomnzZ1Xo1g6pp8c3IH4qRK8ktGtacy1eLcGXFqsTgAOWvHFkLFCmQ==</Modulus><Exponent>AQAB</Exponent><P>unkpwy8UMmHJDyDFT7fAelp5qi0ZqSq6yeFDTuOXXDtp/U0r/RCtNwhnijQAiJpbZ5RQCrG+bS68FnJuhf+gEGhIZxVj68jV6yluLE1A5Agcr9a4zu2ZQh4OWOSRnNC3ozuq9pS1P6sywJ8CeuxrS5ZRrucr1H88efFhBR3jwIE=</P><Q>poLGIjTM0a/gFsVf30hgJtvDTLGV/NxszHbr4XvEFwmcmQDoc4p8BocFcd6mpMGaFI9A5aIxF7uCwZFaF5wpg+ztWx5xjTnUIFgDXyWEFwVNoMTeMLaYtdBxjM/a9ar2W/zIURZyL0vzi9EZziMCsiMvLmB5HNOoRueppU64dhk=</Q><DP>lk0C9GgGB3X58U+guliJtBUo65nejRP76qy+699WKOlazOhfBGNkum7zxdmUdIa2Fg7dVUFfE/IPeKhTnX4lLhRgB3aeS3ZdtmZ2fw7ltucy+ChCXcf1N+2x8sSI+bThz5hQfN/wF5mOFuQTw7C76vkGbcu28Fh+DmwYn18wLAE=</DP><DQ>V7xocfWoRoSJavYtAhW8tDzvtyWLhmUO3t16hKEWPZ1O6j/UO60olLfoYouS+xUX/uVMqLLBc2PWDfBsrCwYo+7fUjfEVUm3QCOgAy5dX1PoB2I0QS61YHeonS2YjgG7NdUIJ9HP8KPwfCd9lWRVM0/euG0U49+St2mxtey4vWE=</DQ><InverseQ>aW7oZEyqKxISIr8VawK5IPQUCvVInv0PY2JGrapYtUeClmsCahzST1H+s9dYva2FpNvfXpJjl/BSTu/E9GXRkk8+5JuKnG4hE1LVAcOR1LHJ3aXrSH5smpZD05lfn4GRoRjb8gqlF32v27ZR6ECL7mux9BRJvl/0wUAunyw8CRU=</InverseQ><D>RzueUDmOaK8/bDdOl0iHgBKWHn0gqLgrNQPUNavjGtNXzro4dkUXKqX7S8XL/zcKpbmDwHdW5KiQjjnIkn93oyOeixrzGay3vIuMsZg2JKj0Zpf9FU9wvQxmgJfWZuczw5P1MHa1a2npzpgBQUG6dLUxucmpPeXGd9kHcpP9IKRK5GKrKPVp0bzVYQLGctr/+fIxTsfhh9DVFrMtMH3TKhQ7ihmmf/yJNrzt8JgOp7T3kzVHCMhLKfkszd65hag46W9C6S1qp9uMCQwDezjE/jZUcEW1enMWBW39uwGzqNoH0WqY6UV9PSvZp+jUqWabkAdlFS6uRDAC01pBzG04AQ==</D></RSAKeyValue>";
        private readonly static string PublicKey = PublicXML.Base64Encode();
        private readonly static string PrivateKey = PrivateXML.Base64Encode();

        public static string Encrypt(this string plainText)
        {
            int keySize;
            string publicKeyXml;
            GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plainText), keySize, publicKeyXml);
            return Convert.ToBase64String(encrypted);
        }

        private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", nameof(data));
            int maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException(string.Format("Maximum data length is {0}", maxLength), nameof(data));
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", nameof(keySize));
            if (string.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", nameof(publicKeyXml));

            using RSACryptoServiceProvider provider = new RSACryptoServiceProvider(keySize);
            //provider.FromXmlString(publicKeyXml);
            RSAKeyExtensions.FromXmlString(provider, publicKeyXml);
            return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
        }

        public static string Decrypt(string encryptedText)
        {
            int keySize;
            string publicAndPrivateKeyXml;
            GetKeyFromEncryptionString(PrivateKey, out keySize, out publicAndPrivateKeyXml);

            byte[] decrypted = Decrypt(Convert.FromBase64String(encryptedText), keySize, publicAndPrivateKeyXml);

            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using RSACryptoServiceProvider provider = new RSACryptoServiceProvider(keySize);
            //provider.FromXmlString(publicAndPrivateKeyXml);
            RSAKeyExtensions.FromXmlString(provider, publicAndPrivateKeyXml);
            return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
        }

        private static int GetMaxDataLength(int keySize) => _optimalAsymmetricEncryptionPadding ? ((keySize - 384) / 8) + 7 : ((keySize - 384) / 8) + 37;

        private static bool IsKeySizeValid(int keySize) => keySize >= 384 && keySize <= 16384 && keySize % 8 == 0;


        // Convert key from base64 to string and get size and xml key
        private static void GetKeyFromEncryptionString(string rawkey, out int keySize, out string xmlKey)
        {
            keySize = 0;
            xmlKey = "";

            if (rawkey != null && rawkey.Length > 0)
            {
                byte[] keyBytes = Convert.FromBase64String(rawkey);
                string stringKey = Encoding.UTF8.GetString(keyBytes);

                if (stringKey.Contains("!"))
                {
                    string[] splittedValues = stringKey.Split(new char[] { '!' }, 2);
                    try
                    {
                        keySize = int.Parse(splittedValues[0]);
                        xmlKey = splittedValues[1];
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
