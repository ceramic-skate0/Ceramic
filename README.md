# Ceramic
A simple dotNET application I built to do common Red Teaming things for me. 

# Contrib
- Open an Issue
- Do it yourself via a Pull Request

## FYSA:
As of 01Jul21 ummm defender flags this tools when its compiled as 'VirTool:MSIL/BytzChk.C!MTB' says this tool is used to make malware. Seems a little much but when your crawling Github looking to flag repo's with Key words guess this fits the bill. This is likely because i left a compiled version in the releases XD and some automated scraper got it and said bad strings.

# Usage:


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

``git clone --recurse-submodules https://github.com/ceramicskate0/Ceramic``

``cd Ceramic/Ceramic``

``$ ~/Ceramic/Ceramic > dotnet run``
