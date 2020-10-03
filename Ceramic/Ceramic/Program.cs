using System;

namespace Ceramic
{
    class Program
    {
        static void Main(string[] args)
        {
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
                        case "?":
                            Usage();
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

                
            ");
        }
    }
}
