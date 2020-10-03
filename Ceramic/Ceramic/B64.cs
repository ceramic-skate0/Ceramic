using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ceramic
{
    class B64
    {
        public static void Base64File(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine(FilePath + " input file does not exists where you say it is.");
                Environment.Exit(1);
            }
            byte[] bytes = File.ReadAllBytes(FilePath);
            Console.WriteLine("Writing file to Base64FileOutput.txt");
            File.WriteAllText("Base64FileOutput.txt", Convert.ToBase64String(bytes));
            //Console.WriteLine(Convert.ToBase64String(bytes));
        }

    }
}
