Radar is a SDR software which uses ambiguity function which is usually it is called passive radar. This is C# code, created with use of VS2022 for Windows 11 64bit The program uses MonoGame framework for graphisc The program uses CATALOG for multiple dongle suport, created by Joanne If you want to play with ambiguity function for NVIDIA you have to see the directory ambiguity and install CUDA tolkit https://developer.nvidia.com/cuda-toolkit There is also AMD version of fast ambiguity function byt at the moment is not compatibile with this new Radar version and not as strong optymised as MVIDIA The exe file is SDRdue but to start the software correctly first you have to activete SDR dongle The main program is Radar

Please read how to get two dongles pdf included.

Changes comparing to previous version:

-Fixed bug with custom color map

-Corrected scales in Radar window, cod optimisation, improved graphical design with help of Alexander Borisenko 

-Code cleaning (name clarification, replease SDRdue with PasiveRadar and so on)


Compilation:
The solution is set to the Windows 11 but it is possible to complie it for Windows 10 with correct settings.
The program should be started as a 64bits and Release. The Debug can be also run but before that, all the files from Release should be cpied to the Debug directory. The catalog should be umblocked in Windows and in case of antivirus also in  such software. 


- the code is compiled for 64bit version. The Executable directory needs some files:
- rtlsdr.dll
- libusb-1.0.dll
- Ambiguity.dll
- ft.xnb
- settings.ext (optionally) 

Two file needs unblocking:
- Form1.resx
- RadarControl.resx

do it in windows explorer, go to properties and than in protection section remove the blocking
see: https://learn.microsoft.com/pl-pl/visualstudio/msbuild/errors/msb3821?view=vs-2022
