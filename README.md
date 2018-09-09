
# TAP - Touch And Play

## What is this?
This game was developed and designed specifically for burns rehabilitation patients using Kinect 1.5 circa 2012 for a school project. The accompanying literature for this project is [in this link](http://ieeexplore.ieee.org/document/6623680/).

**IMPORTANT**: This project **now has minimal support** because Microsoft XNA, the framework that this game uses, has been retired.

## Prerequisites for running the app

1. Windows OS - Windows XP SP3/ Windows 7 or higher
2. At least 2GB RAM
3. 2.3GHz processor
4. ~2MB Memory space
5. Kinect for Windows SDK v1.5
6. Microsoft .NET Framework 4.0

## Prerequisites for development
1. Microsoft Visual Studio **2010** (This project has not been ported to Visual Studio 2012 and higher yet)
2. Microsoft XNA Game Studio 4.0. *This includes Microsoft .NET Framework 4.0*
3. Kinect for Windows SDK 1.5

## Running locally

### Method 1: By locally compiling the app
1. Install, ideally in the same order, all items listed in **Prerequisites for development**
2. Install the font inside <root>\ContentDependencies
3. Open Microsoft Visual Studio 2010 and open the `TouchAndPlay.csproject` file inside <root>\TouchAndPlay\
4. Build the solution or debug the app

**Note**: When running the application, ensure you have Kinect sensors connected and active on your machine. If you do not have Kinect sensors connected, you can still interact with the game using your mouse. However, functionality will be limited for certain parts of the game.

### Method 2: By using a pre-built binary

1. Download and extract [this zip file](https://www.dropbox.com/s/vkscdqq62fp91qx/TouchAndPlay.zip?dl=0)
2. Install **Kinect for Windows SDK v1.5** and **Microsoft .NET Framework 4.0** if you haven't done so. You can find the installers for these on Microsoft's website.
3. Run TouchAndPlay.exe

**Note**: The *Dependencies* folder contains the Kinect driver and the Microsoft .NET 4.0 installers

## Contributing and reporting issues

For any issues, concerns, or ideas, please send an email to davecroman@gmail.com

## Acknowledgements

This application was developed with the help of Kai Salamanca, Diana Pacapac, and Dr. Jaime Caro. Design was driven by feedback from the team in the Department of Rehabilitation Medicine of Philippine General Hospital.

## License
This software is distributed under the MIT License. 

Copyright 2013-2018 Armond Ave, Kyla Salamanca, Diana Pacapac

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.