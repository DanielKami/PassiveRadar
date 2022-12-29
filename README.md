Radar is a SDR software which uses ambiguity function which is usually it is called passive radar. This is C# code, created with use of VS2022 for Windows 11 64bit The program uses MonoGame framework for graphisc The program uses CATALOG for multiple dongle suport, created by Joanne If you want to play with ambiguity function for NVIDIA you have to see the directory ambiguity and install CUDA tolkit https://developer.nvidia.com/cuda-toolkit There is also AMD version of fast ambiguity function byt at the moment is not compatibile with this new Radar version and not as strong optymised as MVIDIA The exe file is SDRdue but to start the software correctly first you have to activete SDR dongle The main program is Radar

Please read how to get two dongles pdf included.

Changes comparing to previous version:

-Fixed bug with custom color map

-Corrected scales in Radar window, cod optimisation, improved graphical design with help of Alexander Borisenko 

-Code cleaning (name clarification, replease SDRdue with PasiveRadar and so on)

It can be sometimes necessary to unblock files:

-WindowsRadio.resx and Form1.resx just go to the directory /PassiveRadar and right mouse click, chose settings and in the bottom right corner chose the box (see Change atributes 1.jpg and Change atributes 2.jpg)


- the code is compiled for 64bit version. The Executable directory needs some files:
- rtlsdr.dll
- libusb-1.0.dll
- Ambiguity.dll
- ft.xnb
- settings.ext (optionally) 
