# Ceramic
A simple .Netcore application i built to do Red Teaming things for me. shhh this mean you can run it on Limnux and Windows :)
Also its a compile it your self kinda thing. If your unaware of how to do this Please Google it like a Red Teamer.

# Contrib

- Open an Issue
- Do it yourself via a Pull Request


# Usage:
            CeramicSkate0's 1 stop shop in dotnet core to do random Red Team tasks via 1 app.  
            Cuz I cant remember all the random commandline ares to do it.      
            So yes things are case sensitive and yes the commandline inputs must be in order shown below.

            Commandline Params:

            -b64 {Input File Path}
            The command above will base64 encode a input file and save it to an output file.

            -xor {Input .bin File Path} {XOR KEY}
            The command above will xor a .bin file with a key and output it to a file. This mean when you un xor it you will need the same key.

            -far {Input File or the file you want to search thru} {What you want to change} {What you want to change it to (File or string)(Will check to see if file exists if not assumes you wanted to use a string)}
            'far' (Find And Replace) will take a input file(1st arg) and then replace in that file the 2nd arg you specify with either the string your specify or the conents of a file you specify in the 3rd arg.

              

