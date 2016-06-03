dotnet-command
--------------

# Usage
```
dotnet command [arguments] [options]
```

Command aliases are placed in project.json.

To install, add "dotnet-command" to the "tools" section of project.json.

```json
{
   "commands": {
       "alias": "FullCommandName"
   },
   "tools": {
        "dotnet-command": "1.0.0-alpha"
   }
}
```

For more info:
```
dotnet command --help
```

# Example

dotnet-command can be used as an alterate way to launch dependencies as the main entry point.
In this example, Entity Framework Core command line tools can be run as the alternate entry point.

```json
{
  "tools": {
    "dotnet-command": "1.0.0-alpha"
  },
  
  "commands": {
    "ef": "Microsoft.EntityFrameworkCore.Tools.Cli --framework netcoreapp1.0",
    "update-db": "Microsoft.EntityFrameworkCore.Tools.Cli --framework netcoreapp1.0 database update --verbose"
  },
  
  "buildOptions": {
    "emitEntryPoint": true
  },
  "dependencies": {
    "Microsoft.EntityFrameworkCore.Sqlite": "1.0.0-rc2-final",
    "Microsoft.EntityFrameworkCore.Tools.Cli": "1.0.0-preview1-final",
    "Microsoft.NETCore.App": {
        "type": "platform",
        "version": "1.0.0-*"
    }
  },
  "frameworks": {
    "netcoreapp1.0": {
      "imports": "portable-net451+win8"
    }
  }
}
```

# Command line syntax
```
Usage: dotnet command [arguments] [options]
Arguments:
  <COMMAND> [arguments]  The command to execute
Options:
  -?|-h|--help                        Show help information
  -c|--configuration <CONFIGURATION>  Configuration under which to run
  -f|--framework <FRAMEWORK>          Looks for command binaries for a specific framework
  -o|--output <OUTPUT_DIR>            Directory in which to find the binaries to be run
  -b|--build-base-path <OUTPUT_DIR>   Directory in which to find temporary outputs
  -p|--project <PROJECT>              The project to execute command on, defaults to the current directory. Can be a path to a project.json or a project directory.
  --parentProcessId                   Used by IDEs to specify their process ID. Command will exit if the parent process does.
```