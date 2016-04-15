@echo off

call :GetUnixTime version
@echo on
dotnet restore src/ --verbosity error
dotnet restore test/ --verbosity error
mkdir artifacts/
dotnet pack src/dotnet-command/ -o artifacts --version-suffix t%version%
dotnet pack test/BananaLauncher.Tool/ -o artifacts --version-suffix t%version%
dotnet restore sample/ -f ./artifacts --verbosity error

cd sample/UserProject/
dotnet build
dotnet command banana "Hello world" "I am a banana"
cd ../..

exit /b 0


:GetUnixTime
@echo off
setlocal enableextensions
for /f %%x in ('wmic path win32_utctime get /format:list ^| findstr "="') do (
    set %%x)
set /a z=(14-100%Month%%%100)/12, y=10000%Year%%%10000-z
set /a ut=y*365+y/4-y/100+y/400+(153*(100%Month%%%100+12*z-3)+2)/5+Day-719469
set /a ut=ut*86400+100%Hour%%%100*3600+100%Minute%%%100*60+100%Second%%%100
endlocal & set "%1=%ut%" & goto :EOF