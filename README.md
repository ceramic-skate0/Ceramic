# Ceramic
A simple .MET application I built to do common Red Teaming things for me. 

# Contrib
- Open an Issue
- Do it yourself via a Pull Request

## FYSA:
As of 01Jul21 ummm defender flags this tools when its compiled as 'VirTool:MSIL/BytzChk.C!MTB' says this tool is used to make malware. Seems a little much but when your crawling Github looking to flag repo's with Key words guess this fits the bill.

# Usage:
            CeramicSkate0's 1 stop shop in dotnet to do random Red Team tasks via 1 app.  
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


## Install dotnet on Ubuntu:

Most up to date Reference for installing dotnet and support for other Linux platforms is https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu

Below is what was used/tested:

``wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb``

``sudo dpkg -i packages-microsoft-prod.deb``

``sudo apt-get update``

``sudo apt-get install -y apt-transport-https``

``sudo apt-get install -y dotnet-sdk-5.0``
  
``sudo apt-get install -y aspnetcore-runtime-5.0``

``sudo apt-get install -y dotnet-runtime-5.0``

``$ ~ > git clone --recurse-submodules https://github.com/ceramicskate0/Ceramic``

``$ ~ > cd Ceramic/Ceramic``

``$ ~/Ceramic/Ceramic > dotnet Ceramic.exe``
