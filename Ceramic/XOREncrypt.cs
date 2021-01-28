using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ceramic
{
    class XOREncrypt
    {
        // Simple XOR routine from https://github.com/djhohnstein/CSharpCreateThreadExample
        private static byte[] XorByteArray(byte[] origBytes, char[] cryptor)
        {
            byte[] result = new byte[origBytes.Length];
            int j = 0;
            for (int i = 0; i < origBytes.Length; i++)
            {
                // If we're at the end of the encryption key, move
                // pointer back to beginning.
                if (j == cryptor.Length - 1)
                {
                    j = 0;
                }
                // Perform the XOR operation
                byte res = (byte)(origBytes[i] ^ Convert.ToByte(cryptor[j]));
                // Store the result
                result[i] = res;
                // Increment the pointer of the XOR key
                j += 1;
            }
            // Return results
            return result;
        }
        public static void XORShellcodeFile(string FileLocation, string key)
        {
            if (!File.Exists(FileLocation))
            {
                Console.WriteLine("Could not find path to shellcode bin file: {0}", FileLocation);
                Environment.Exit(1);
            }
            byte[] shellcodeBytes = File.ReadAllBytes(FileLocation);
            // This is the encryption key. If changed, must also be changed in the
            // project that runs the shellcode.
            char[] cryptor = key.ToCharArray();
            byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
            File.WriteAllBytes("XorShellcode.bin", encShellcodeBytes);
            Console.WriteLine("Wrote encoded binary to XorShellcode.bin.");
        }
    }
}
