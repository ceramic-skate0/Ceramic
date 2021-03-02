using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ceramic
{    
    static class AESEncryptShellcode
    {
        //private static string aes_iv = "bsxnWolsAyO7kCfWuyrnqg==";
        //private static string aes_key = "AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc=";
        
        public static byte[] Encrypt(byte[] bytesToEncrypt, string password,string iv)
        {
            byte[] ivSeed = Guid.NewGuid().ToByteArray();

            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            //byte[] Key = Convert.FromBase64String(aes_key);
            //byte[] IV = Convert.FromBase64String(aes_iv);;
            byte[] Key = Encoding.Unicode.GetBytes(password);
            byte[] IV = Encoding.Unicode.GetBytes(iv);

            byte[] encrypted;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                encrypted = mstream.ToArray();
            }

            //var messageLengthAs32Bits = Convert.ToInt32(bytesToEncrypt.Length);
            //var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

            //encrypted = encrypted.Prepend(ivSeed);
            //encrypted = encrypted.Prepend(messageLength);

            return encrypted;
        }

        //c# code to return unencrypted byte code
        public static byte[] Decrypt(byte[] bytesToDecrypt, string password,string iv)
        {
            byte[] tmp;

            var length = bytesToDecrypt.Length;
            byte[] ivSeed = Guid.NewGuid().ToByteArray();
            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            //byte[] Key = Encoding.Unicode.GetBytes(password);

            byte[] Key = Encoding.Unicode.GetBytes(password);
            byte[] IV = Encoding.Unicode.GetBytes(iv);

            using (MemoryStream mStream = new MemoryStream(bytesToDecrypt))
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider() { Padding = PaddingMode.None })
            using (CryptoStream cryptoStream = new CryptoStream(mStream, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
            {
                cryptoStream.Read(bytesToDecrypt, 0, length);
                tmp = mStream.ToArray().Take(length).ToArray();
            }
            return tmp;
        }

    }
}
