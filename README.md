dotnet-command
--------------


# Usage


"BananaLauncher.Tool" is a sample user-tool that loads and executes your project

```json
{
    "compilationOptions": {
        "emitEntryPoint": true
    },
    
    "dependencies": {
        "BananaLauncher.Tool": {
            "version": "1.0.0-*",
            "type":"build"
        },
        "Microsoft.NETCore.App": {
            "type": "platform",
            "version": "1.0.0-*"
        },
        "Microsoft.Data.Sqlite": "1.0.0-*"
    },
    
    "frameworks": {
        "netcoreapp1.0": {  "imports": ["portable-net452+win81"] }
    },
    
    "commands": {
        "banana": "BananaLauncher.Tool",
        "args": "BananaLauncher.Tool arg1 arg2 --param arg3"
    },
    
    "tools": {
        "dotnet-command": {
            "version": "1.0.0-*",
            "imports": ["portable-net452+win81"]
        }
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