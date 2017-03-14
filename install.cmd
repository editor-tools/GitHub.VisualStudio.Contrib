@call "vars.bat"

vsixutil /install "%VsixPath%" /v 15 /s Enterprise;Professional;Community
