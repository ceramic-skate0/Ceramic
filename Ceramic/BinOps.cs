using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ceramic
{
    class BinOps
    {
        private static string ReplaceHexString(List<string> BadStrings, string hexRepresentation)
        {
            string NEWhexRepresentation = hexRepresentation;
            foreach (string sigString in BadStrings)
            {
                string hexReplace = BitConverter.ToString(Encoding.UTF8.GetBytes(sigString)).Replace("-", string.Empty);
                string newReplaceValue = Utils.RandomString(sigString.Length);
                string newData = BitConverter.ToString(Encoding.UTF8.GetBytes(newReplaceValue)).Replace("-", string.Empty);
                string ValueThisPass = NEWhexRepresentation;
                NEWhexRepresentation = NEWhexRepresentation.Replace(hexReplace, newData);

                if (NEWhexRepresentation != ValueThisPass)
                {
                    Console.WriteLine("[+] Replaced in Binary HEX value for '" + sigString + "' with '" + newReplaceValue + "'");
                }
            }
            if (NEWhexRepresentation == hexRepresentation)
            {
                Console.WriteLine("[ERROR] The new binary HEX string equals the old one, thus no bad string have been changed in compiled bin.");
            }
            return NEWhexRepresentation;
        }

        public static void ReplaceBinString(string inputFile, string outputfile, string ListOfBadStringsURL = "https://raw.githubusercontent.com/lunarobliq/BadStrings/master/AV")
        {
            try
            {
                Console.WriteLine("[*] Randomizing known bad strings...");
                try
                {
                    WebClient webClient = new WebClient();
                    Console.WriteLine("[*] Downloading bad strings List<string> from '" + ListOfBadStringsURL + "'");
                    List<string> BadStrings = webClient.DownloadString(ListOfBadStringsURL).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    string hexRepresentation = ReplaceHexString(BadStrings, BitConverter.ToString(File.ReadAllBytes(inputFile)).Replace("-", string.Empty));

                    File.WriteAllBytes(outputfile, Utils.StringToByteArray(hexRepresentation));

                    Console.WriteLine("[*] Output File Path: " + outputfile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] ERROR FAILED to download and apply string replacement for list of bad strings from " + ListOfBadStringsURL + "[!] ERROR Reason: " + e.Message.ToString());
                    throw new IOException("[ERROR] ERROR FAILED to download and apply string replacement for list of bad strings");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to write assembly to disk. Reason, " + e.Message.ToString());
                throw new IOException("[ERROR] Unable to write assembly to disk.");
            }
        }
    }
}
