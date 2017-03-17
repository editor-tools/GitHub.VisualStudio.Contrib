@echo off

set VsixPath=%cd%\GitHub.VisualStudio.Contrib.vsix
set SolutionFile=%cd%\GitHub.VisualStudio.Contrib.sln

rem Utility for re/installing VSIX
set path=%cd%\tools\VsixUtil;%path%

rem Which SKU to use
set VisualStudioVersionPath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017
if "%VisualStudioSKUPath%" == "" if exist "%VisualStudioVersionPath%\Enterprise" set VisualStudioSKUPath=%VisualStudioVersionPath%\Enterprise
if "%VisualStudioSKUPath%" == "" if exist "%VisualStudioVersionPath%\Professional" set VisualStudioSKUPath=%VisualStudioVersionPath%\Professional
if "%VisualStudioSKUPath%" == "" if exist "%VisualStudioVersionPath%\Community" set VisualStudioSKUPath=%VisualStudioVersionPath%\Community

rem Add path to MSBuild Binaries
set PATH=%VisualStudioSKUPath%\MSBuild\15.0\Bin;%PATH%

rem Add path to Visual Studio
set PATH=%VisualStudioSKUPath%\Common7\IDE;%PATH%
