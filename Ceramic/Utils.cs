using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ceramic
{
    class Utils
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static int GetRandomInt(int max)
        {
            return random.Next(5, max);
        }
        /// <summary>
        /// Method to create a random string with no spaces or special chars
        /// </summary>
        /// <param name="length">If provided is the max length of string. If not provided, will create one randomly of length 5-10</param>
        /// <returns></returns>
        public static string RandomString(int length = 0)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm";

            if (length > 0)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(chars, GetRandomInt(10)).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }

        public static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, new byte[] { 0xDE, 0xAD, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF }, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }

        // From: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.7.2
        public static byte[] EncryptStringToBytes_Aes(string plainTextInputData, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainTextInputData == null || plainTextInputData.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() IV");
            byte[] encrypted;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the crypto IO stream.
                            swEncrypt.Write(plainTextInputData);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}


