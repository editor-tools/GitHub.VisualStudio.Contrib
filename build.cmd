@call "vars.bat"

msbuild "%SolutionFile%" "/p:TargetVsixContainer=%VsixPath%" /t:GitHub_VisualStudio_Contrib_Vsix
