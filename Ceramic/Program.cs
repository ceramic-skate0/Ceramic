using System;
using System.IO;

namespace Ceramic
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<=0)
            {
                Usage();
                Environment.Exit(0);
            }
            try
            {
                for (int x = 0; x < args.Length; ++x)
                {
                    switch (args[x])
                    {
                        case "-b64":
                            B64.Base64File(args[x + 1]);
                            break;
                        case "-xor":
                            XOREncrypt.XORShellcodeFile(args[x + 1], args[x + 2]);
                            break;
                        case "-far":
                            FindAndReplace.ReadFileReplaceString(args[x + 1], args[x + 2], args[x + 3]);
                            break;
                        case "?":
                            Usage();
                            break;
                        case "-AVFileCheck":
                            AVChunkTest.AVTest(args[x + 1]);
                            break;
                        case "-DefenderCheck":
                            DefenderCheck.DefenderCheckScan(args[x + 1]);
                            break;
                        case "-aes":
                            try
                            {
                                if (File.Exists(args[x + 1]))
                                { 
                                    byte[] tmp = AESEncryptShellcode.Encrypt(File.ReadAllBytes(args[x + 1]), args[x + 2],args[x+3]);
                                    Console.WriteLine("[*] Writing File 'EncryptedShellcode.bin' and 'EncryptedShellcodeB64.txt' to current dir");
                                    File.WriteAllText("EncryptedShellcodeB64.txt", Convert.ToBase64String(tmp));
                                    File.WriteAllBytes("EncryptedShellcode.bin", tmp);
                                }
                                else
                                {
                                    Console.WriteLine("[!] So you files isnt where you said it was. " + args[x + 1]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("[!] SHIT SOMETHING WENT WRONG! "+e.Message.ToString());
                            }
                            break;
                        case "-h":
                            Usage();
                            break;
                        case "-help":
                            Usage();
                            break;
                    }
                }
            }
            catch
            {
                Usage();
            }
        }

        static void Usage()
        {
            Console.WriteLine(@"
            CeramicSkate0's 1 stop shop in dotnet core to do random Red Team tasks via 1 app.  
            Cuz I cant remember all the random commandline ares to do it.      
            So yes things are case sensitive and yes the commandline inputs must be in order shown below.

            Commandline Params:

            -b64 {Input File Path}
            The command above will base64 encode a input file and save it to an output file.

            -xor {Input .bin File Path} {XOR KEY}
            The command above will xor a .bin file with a key and output it to a file. This mean when you un xor it you will need the same key.

            -aes {Input .bin File Path} {AES KEY} {AES IV}

            -far {Input File or the file you want to search thru} {What you want to change} {What you want to change it to (File or string)(Will check to see if file exists if not assumes you wanted to use a string)}
            'far' (Find and Replace) will take a input file(1st arg) and then replace in that file the 2nd arg you specify with either the string your specify or the conents of a file you specify in the 3rd arg.
            ");
        }
    }
}
