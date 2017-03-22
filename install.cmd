@call "vars.bat"

vsixutil /install "%VsixPath%" /v 15 /s Enterprise
vsixutil /install "%VsixPath%" /v 15 /s Professional
vsixutil /install "%VsixPath%" /v 15 /s Community
