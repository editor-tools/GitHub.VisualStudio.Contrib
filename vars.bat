@echo off

set VsixPath=%cd%\GitHub.VisualStudio.Contrib.vsix
set SolutionFile=%cd%\GitHub.VisualStudio.Contrib.sln

rem Utility for re/installing VSIX
set path=%cd%\tools\VsixUtil;%path%

rem Which SKU to use
set VisualStudioSKU=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise

rem Add path to MSBuild Binaries
set PATH=%VisualStudioSKU%\MSBuild\15.0\Bin;%PATH%

rem Add path to Visual Studio
set PATH=%VisualStudioSKU%\Common7\IDE;%PATH%
