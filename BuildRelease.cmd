@set PROGSDIR=%PROGRAMFILES%
@set TOOLSVER=14.0
@if exist "%PROGRAMFILES(x86)%" set PROGSDIR=%PROGRAMFILES(x86)%
"%PROGSDIR%\MSBuild\%TOOLSVER%\Bin\MSBuild" /m Build.proj /t:Build /p:Configuration=Release "/p:Platform=Any CPU" /nr:false %*
@if %ERRORLEVEL% NEQ 0 GOTO failed
@exit /B 0
:failed
@PAUSE
@exit /B 1