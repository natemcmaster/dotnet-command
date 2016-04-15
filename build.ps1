$ErrorActionPreference='Stop'

$version = get-date -UFormat %Y%m%d%H%M%S

Write-Host "Restoring..." -ForegroundColor Cyan
dotnet restore src/ --verbosity error
dotnet restore test/ --verbosity error
if(!(test-path artifacts/)) {
    mkdir artifacts/
}

Write-Host "Compiling..." -ForegroundColor Cyan
dotnet pack src/dotnet-command/ -o artifacts --version-suffix t$version
dotnet pack test/BananaLauncher.Tool/ -o artifacts --version-suffix t$version

Write-Host "Restore again..." -ForegroundColor Cyan
dotnet restore sample/ -f ./artifacts --verbosity error

Write-Host "Compile test..." -ForegroundColor Cyan

try
{
    pushd sample/UserProject/
    dotnet build
    Write-Host "Run tests..." -ForegroundColor Cyan
    dotnet command -f netcoreapp1.0 banana "Hello world" "I am a .NET Core banana"
    dotnet command -f net451 banana "Hello world" "I am a Desktop .NET banana"
}
finally
{
    popd
}
