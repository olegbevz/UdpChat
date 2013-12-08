// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cryptography.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Cryptography type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UdpChat.Common.Messages
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class Cryptography
    {
        private static byte[] CryptKey = System.Text.Encoding.Unicode.GetBytes("ABCD");

        private static byte[] InitVector = System.Text.Encoding.Unicode.GetBytes("ABCD");

        public static byte[] Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");

            }

            using (var cryptoProvider = new DESCryptoServiceProvider())
            {
                cryptoProvider.Key = CryptKey;
                cryptoProvider.IV = InitVector;

                var encryptor = cryptoProvider.CreateEncryptor(cryptoProvider.Key, cryptoProvider.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(data);
                        }

                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public static string Decrypt(byte[] bytes)
        {
            // Check arguments.
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException("bytes");
            }

            using (var cryptoProvider = new DESCryptoServiceProvider())
            {
                cryptoProvider.Key = CryptKey;
                cryptoProvider.IV = InitVector;

                var decryptor = cryptoProvider.CreateDecryptor(cryptoProvider.Key, cryptoProvider.IV);

                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}