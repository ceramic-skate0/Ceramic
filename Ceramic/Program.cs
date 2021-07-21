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
                        case "-obfuscateDotNETbin":
                            Obfuscatedotnet.Obfuscate(args[x + 1], args[x + 2]);
                            break;
                        case "-b64":
                            Compress.Base64File(args[x + 1]);
                            break;
                        case "-GZIP":
                            if (args.Length==2)
                            Compress.GZIP(args[x + 1]);
                            else
                            Compress.GZIP(args[x + 1],args[x+2]);
                            break;
                        case "-xor":
                            Crypto.XORShellcodeFile(args[x + 1], args[x + 2]);
                            break;
                        case "-far":
                            FindAndReplace.ReadFileReplaceString(args[x + 1], args[x + 2], args[x + 3]);
                            break;
                        case "-AVFileCheck":
                            AVChecks.AVTest(args[x + 1]);
                            break;
                        case "-DefenderCheck":
                            DefenderCheck.DefenderCheckScan(args[x + 1]);
                            break;
                        case "-ChunkHTAShellcode":
                            HTA.ChunkRAWShellcode_HTA(args[x + 1], Convert.ToInt32(args[x + 2]));
                            break;
                        case "-ChunckRAWtoVBArrys":
                            VBA.ChunckRAWtoVBArrys(args[x + 1]);
                            break;
                        case "-ConvertToIntArray":
                            File.WriteAllText("ConvertedINTArray.txt",BinaryOperations.ByteShellcodeToInt(File.ReadAllBytes(args[x+1])));
                            break;
                        case "-ConvertToINT64Array":
                            File.WriteAllText("ConvertedINT64Array.txt", BinaryOperations.ByteToInt64(File.ReadAllBytes(args[x + 1])));
                            break;
                        case "-aes":
                            try
                            {
                                if (File.Exists(args[x + 1]))
                                { 
                                    byte[] tmp = Crypto.Encrypt(File.ReadAllBytes(args[x + 1]), args[x + 2],args[x+3]);
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
                        case "-ConvertToHEX":
                            try
                            {
                                if (File.Exists(args[x + 1]))
                                {
                                    byte[] tmp = File.ReadAllBytes(args[x + 1]);
                                    Console.WriteLine("[*] Writing File 'HexCodeOuput.txt' to current dir");
                                    File.WriteAllText("HexCodeOuput.txt", BinaryOperations.ByteArrayToString(tmp));
                                }
                                else
                                {
                                    Console.WriteLine("[!] So you files isnt where you said it was. " + args[x + 1]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("[!] SHIT SOMETHING WENT WRONG! " + e.Message.ToString());
                            }
                            break;
                        case "-Reverse":
                            if (File.Exists(args[x + 1]))
                            {
                                string filename = Path.GetFileName(args[x + 1]).Split('.')[0] + "reverse";
                                string Dir = Path.GetDirectoryName(args[x + 1]);
                                string ext = Path.GetExtension(args[x + 1]);
                                Console.WriteLine("[*] Writing File with reverse string "+Dir+"\\"+filename+ext+" to current dir");
                                File.WriteAllText(Dir+"\\"+filename+ext, Utils.ReverseString(File.ReadAllText(args[x + 1])));
                            }
                            else
                             {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[x + 1]);
                             }
                            break;
                        case "-AddJunk":
                            if (File.Exists(args[x + 1]))
                            {
                                string filename = Path.GetFileName(args[x + 1]).Split('.')[0] + "reverse";
                                string Dir = Path.GetDirectoryName(args[x + 1]);
                                string ext = Path.GetExtension(args[x + 1]);

                                Console.WriteLine("[*] Writing File with Junk in string " + Dir + "\\" + filename + ext + " to current dir");
                                File.WriteAllText(Dir + "\\" + filename + ext, Utils.AddJunkToString(File.ReadAllText(args[x + 1])));
                            }
                            else
                            {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[x + 1]);
                            }
                            break;
                        case "-BitReplaceBin":
                            if (File.Exists(args[x + 1]))
                            {
                                if (args.Length == 2)
                                {
                                    BinaryOperations.ReplaceBinString(args[x + 1], args[x + 2]);
                                }
                                else
                                {
                                    BinaryOperations.ReplaceBinString(args[x + 1], args[x + 2],args[x+3]);
                                }
                            }
                            else
                            {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[x + 1]);
                            }
                            break;
                        case "-h":
                            Usage();
                            break;
                        case "-help":
                            Usage();
                            break;
                        case "?":
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
            Cuz I cant remember all the random commandline args and tools to do it.      
            So yes things are case sensitive and yes the commandline inputs must be in order shown below.

            Commandline Params:
            
            -AVFileCheck {Input File Path}
            Run a modified version of DefenderCheck by matterpreter to try and trigger and AV response to test a file           

            -DefenderCheck {Input File Path}
             matterpreter tool. Takes a binary as input and splits it until it pinpoints that exact byte that Microsoft Defender will flag on, and then prints those offending bytes to the screen. This can be helpful when trying to identify the specific bad pieces of code in your tool/payload.
            
            -b64 {Input File Path}
            The command above will base64 encode a input file and save it to an output file.

            -xor {Input .bin File Path} {XOR KEY}
            The command above will xor a .bin file with a key and output it to a file. This mean when you un xor it you will need the same key.

            -aes {Input .bin File Path} {AES KEY} {AES IV}

            -far {Input File or the file you want to search thru} {What you want to change} {What you want to change it to (File or string)(Will check to see if file exists if not assumes you wanted to use a string)}
            'far' (Find and Replace) will take a input file(1st arg) and then replace in that file the 2nd arg you specify with either the string your specify or the conents of a file you specify in the 3rd arg.

            -ChunkHTAShellcode {Input a already B64 encoded shellcode File Path} {Optional: Number of chunks}
            Attempts chunk and encode a shellcode input file and output it into a HTA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.
            
            -ChunckRAWtoVBArrys {Input .RAW File Path to shellcode file}
            Attempts chunk and encode a shellcode input file and output it into a VBA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.

            -Reverse {Input File Path}
            Reads the entire file as 1 string and will write another file with the first files contents reversed.
    
            -AddJunk {Input File Path}
            Reads a file and will randomly add a randomly generated junk string into the files contents and then output a new file with the junk in it.

            -BitReplaceBin {Input File Path/Name} {Ouput File Path/Name} {OPTIONAL: URL with list of bad strings to randomly replace 1 per line}
            Will read in a compiled (.Net prefered) file and bin replace the bad strings from a list via a URL of the found in the file then write new copy.

            -obfuscateDotNETbin {input dotnet dll/exe} {Output name of dotnet dll/exe}
            Takes in a dotnet exe or dll and scrambles it to look diffrent

            -GZIP {Input File Path} {Output File Path}(optional)
            Take a file readf all the bytes in it and gzip the file and output a compressed version of it. Optional output file can be given. Default output file 'GZIPFileOutput.gz' in cwd

            -ConvertToHEX {Input File Path}
            Take a byte file and output a hex version of it.Ouputs the byte file to a file of string HEX called 'HexCodeOuput.txt'

            -ConvertToIntArray {Input File Path}
            Take a bin file and output a txt file with an array of INT's. Oupput file is ConvertedINTArray.txt

            -ConvertToINT64Array {Input File Path}
            Take a bin file and output a txt file with an array of INT64's. Oupput file is ConvertedINT64Array.txt

            ");
        }
    }
}
