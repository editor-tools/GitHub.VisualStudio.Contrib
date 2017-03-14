@call "vars.bat"

msbuild "%SolutionFile%" "/p:TargetVsixContainer=%VsixPath%"
