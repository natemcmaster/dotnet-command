dotnet restore -f ../../artifacts --verbosity error
dotnet build
dotnet command -v BananaLauncher.Tool "Hello" "Hola"