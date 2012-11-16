milo-portscanner
================

A simple command line port scanner for Windows


[Mm]ilo is a simple port scanner for Windows. It take a host (IP address or domain name) and will scan for open ports. To use, the project builds the binaries in the project bin directory (milo.command\bin). For simplicity, add the bin path to your Windows Environment Variables. i.e, ``C:\Project\milo.command\bin``

![Environment Variables](http://dl.dropbox.com/u/5699280/img/ev-screenshot.JPG)


***

If you have entered the bin to your System Path, you can call milo from your prompt and utilize in two ways:

``milo -host 10.10.10.1 -from 0 -to 9000``

You can ignore the -from and -to parameters, and you will be asked
``Do you want to scan the default ports? [y/n]``
The default ports scanned will be from ports 0 to 1023