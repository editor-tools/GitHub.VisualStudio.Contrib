@call vars.bat

@echo Visual Studio will use live builds of 'GitHub.VisualStudio.Contrib.dll'
set GitHub_VisualStudio_Contrib_Path=%cd%\src\GitHub.VisualStudio.Contrib\obj\Debug\GitHub.VisualStudio.Contrib.dll
start devenv.exe "%SolutionFile%" %*
