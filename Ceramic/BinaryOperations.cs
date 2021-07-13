using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ceramic
{
    class BinaryOperations
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
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

    class Obfuscatedotnet
    {
        //Almost all staright from https://github.com/BinaryScary/NET-Obfuscate with minor changes

        private static Random random = new Random();
        private static List<String> names = new List<string>();

        /// <summary>
        /// fixes discrepancies in IL such as maxstack errors that occur due to instruction insertion
        /// </summary>
        private static void cleanAsm(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                                                    //method.Body.OptimizeMacros();
                }
            }
        }

        private static void obfuscateStrings(ModuleDef md)
        {
            //foreach (var type in md.Types) // only gets parent(non-nested) classes

            // types(namespace.class) in module
            foreach (var type in md.GetTypes())
            {
                // methods in type
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;
                    // iterate over instructions of method
                    for (int i = 0; i < method.Body.Instructions.Count(); i++)
                    {
                        // check for LoadString opcode
                        // CIL is Stackbased (data is pushed on stack rather than register)
                        // ref: https://en.wikipedia.org/wiki/Common_Intermediate_Language
                        // ld = load (push onto stack), st = store (store into variable)
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            // c# variable has for loop scope only
                            String regString = method.Body.Instructions[i].Operand.ToString();
                            String encString = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(regString));
                            Console.WriteLine($"{regString} -> {encString}");
                            // methodology for adding code: write it in plain c#, compile, then view IL in dnspy
                            method.Body.Instructions[i].OpCode = OpCodes.Nop; // errors occur if instruction not replaced with Nop
                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, md.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { })))); // Load string onto stack
                            method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, encString)); // Load string onto stack
                            method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, md.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                            method.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Callvirt, md.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) })))); // call method GetString with bytes parameter loaded from stack 
                            i += 4; //skip the Instructions as to not recurse on them
                        }
                    }
                    //method.Body.KeepOldMaxStack = true;
                }
            }

        }

        private static void obfuscateMethods(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                // create method to obfuscation map
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;
                    // method is a constructor
                    if (method.IsConstructor) continue;
                    // method overrides another
                    if (method.HasOverrides) continue;

                    string encName = Utils.RandomString(10);
                    Console.WriteLine($"{method.Name} -> {encName}");
                    method.Name = encName;
                }
            }
        }

        private static void obfuscateClasses(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                string encName = Utils.RandomString(10);
                Console.WriteLine($"{type.Name} -> {encName}");
                type.Name = encName;
            }

        }

        private static void obfuscateNamespace(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                string encName = Utils.RandomString(10);
                Console.WriteLine($"{type.Namespace} -> {encName}");
                type.Namespace = encName;
            }

        }

        private static void obfuscateAssemblyInfo(ModuleDef md)
        {
            // obfuscate assembly name
            string encName = Utils.RandomString(10);
            Console.WriteLine($"{md.Assembly.Name} -> {encName}");
            md.Assembly.Name = encName;

            // obfuscate Assembly Attributes(AssemblyInfo) .rc file
            string[] attri = { "AssemblyDescriptionAttribute", "AssemblyTitleAttribute", "AssemblyProductAttribute", "AssemblyCopyrightAttribute", "AssemblyCompanyAttribute", "AssemblyFileVersionAttribute" };
            // "GuidAttribute", and assembly version can also be changed
            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {
                if (attri.Any(attribute.AttributeType.Name.Contains))
                {
                    string encAttri = Utils.RandomString(10);
                    Console.WriteLine($"{attribute.AttributeType.Name} = {encAttri}");
                    attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                }
            }
        }

        /// <summary>
        /// Obfuscate ECMA CIL (.NET IL) assemblies by obfuscating names of methods, classes, namespaces, assemblyInfo and encoding strings
        /// </summary>
        /// <param name="inFile">The .Net assembly path you want to obfuscate</param>
        /// <param name="outFile">Path to the newly obfuscated file, default is "inFile".obfuscated</param>
        public static void Obfuscate(string inFile, string outFile)
        {
            if (inFile == "" || outFile == "") return;

            string pathExec = inFile;

            // cecil moduel ref(similar to dnlib): https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/faq/
            // load ECMA CIL (.NET IL)
            ModuleDef md = ModuleDefMD.Load(pathExec);
            md.Name = Utils.RandomString(10);

            obfuscateStrings(md);
            obfuscateMethods(md);
            obfuscateClasses(md);
            obfuscateNamespace(md);
            obfuscateAssemblyInfo(md);
            //obfuscateVariables(md); // md.Write already simplifies variable names to there type in effect mangling them i.e: aesSetup -> aes1, aesRun -> aes2
            //obfuscateComments(md); // comments are stripped during compile opitmization

            cleanAsm(md);

            md.Write(outFile);
        }
    }

}
