dotnet-command
--------------


# Usage


"BananaLauncher.Tool" is a sample user-tool that loads and executes your project

```json
{
    "buildOptions": {
        "emitEntryPoint": true
    },
    
    "dependencies": {
        "BananaLauncher.Tool": {
            "version": "1.0.0-*",
            "type":"build"
        },
        "Microsoft.NETCore.App": {
            "type": "platform",
            "version": "1.0.0-rc2-*"
        },
        "Microsoft.Data.Sqlite": "1.0.0-rc2-final"
    },
    
    "frameworks": {
        "netcoreapp1.0": { }
    },
    
    "commands": {
        "banana": "BananaLauncher.Tool",
        "args": "BananaLauncher.Tool arg1 arg2 --param arg3"
    },
    
    "tools": {
        "dotnet-command": "1.0.0-rc2-final"
    }
}
```


# Command Line Syntax
```
dotnet command --help
```

## Examples
```
# using project.json command alias
dotnet command banana 

# using full name of tool assembly
dotnet command BananaLauncher.Tool

# Run tool on a specific framework
dotnet command -f netcoreapp1.0 banana
```