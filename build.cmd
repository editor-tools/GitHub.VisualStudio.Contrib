@call "vars.bat"

msbuild "%SolutionFile%" "/p:TargetVsixContainer=%VsixPath%" /t:src\GitHub_VisualStudio_Contrib_Vsix
