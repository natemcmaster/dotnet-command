$ErrorActionPreference='Stop'
$installDir = Join-Path $PSScriptRoot .dotnet
$dotnet="$installDir/dotnet.exe"
mkdir $installDir -ErrorAction Ignore | Out-Null
if (!(Test-Path $dotnet)) {
    $version = Get-Content $PSScriptRoot/.dotnet-version
    iwr https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview1/scripts/obtain/dotnet-install.ps1 -OutFile $installDir/dotnet-install.ps1
    & $installDir/dotnet-install.ps1 -Channel beta -Version $version -InstallDir $installDir
}

function _dotnet {
    Write-Host -ForegroundColor Gray "dotnet $args"
    & $dotnet $args
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

rm -Recurse artifacts

$time=[long](get-date -UFormat %y%m%d%H%M%S)
$suffix = "t{0:X0}" -f $time

Write-Host "Restoring..." -ForegroundColor Cyan
_dotnet restore src/ --verbosity error
_dotnet restore test/ --verbosity error
if(!(test-path artifacts/)) {
    mkdir artifacts/ | out-null
}

Write-Host "Compiling..." -ForegroundColor Cyan
_dotnet pack src/dotnet-command/ -o artifacts --version-suffix $suffix
_dotnet pack test/BananaLauncher.Tool/ -o artifacts --version-suffix $suffix

Write-Host "Restore again..." -ForegroundColor Cyan
_dotnet restore sample/ -f ./artifacts --verbosity error

Write-Host "Compile test..." -ForegroundColor Cyan

try
{
    pushd sample/UserProject/ | Out-Null
    _dotnet build
    Write-Host "Run tests..." -ForegroundColor Cyan
    _dotnet command -f netcoreapp1.0 banana "Hello world" "I am a .NET Core banana"
    _dotnet command -f net451 banana "Hello world" "I am a Desktop .NET banana"
}
finally
{
    popd
}
