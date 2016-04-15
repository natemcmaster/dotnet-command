using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.ProjectModel;

namespace Microsoft.DotNet.Tools.ProjectCommand
{
    public class ProjectCommand
    {
        public static int Main(string[] args)
        {
            HandleVerboseOption(ref args);
            DebugHelper.HandleDebugSwitch(ref args);
             
            var parameters = new DotnetCommandParams();

            parameters.Parse(args);
            
            try
            {
                if (parameters.Help)
                {
                    return 0;
                }
                
                if (string.IsNullOrEmpty(parameters.Command))
                {
                    Reporter.Error.WriteLine("Missing argument <COMMAND>");
                    parameters.ShowHint();
                    return 1;
                }

                // Register for parent process's exit event
                if (parameters.ParentProcessId.HasValue)
                {
                    RegisterForParentProcessExit(parameters.ParentProcessId.Value);
                }

                var projectContexts = CreateProjectContexts(parameters.ProjectPath);
                
                
                var projectContext =  parameters.Framework != null
                     ? projectContexts.First(p => p.TargetFramework == parameters.Framework)
                     : projectContexts.First(); // TODO select netcoreapp1.0 if possible
                
                if (projectContext == null)
                {
                    Reporter.Error.WriteLine($"Project does not support framework {projectContext.TargetFramework.GetShortFolderName()}");
                    return 1;    
                }
                
                Reporter.Verbose.WriteLine($"Running tool on {projectContext.TargetFramework.GetShortFolderName()}");
                
                // TODO implement build
                var runner = new CommandRunner(projectContext);

                return runner.Run(parameters);
            }
            catch (Exception ex)
            {
                // TestHostTracing.Source.TraceEvent(TraceEventType.Error, 0, ex.ToString());
                Reporter.Error.WriteLine(ex.Message);
                return -1;
            }
        }
        
        private static IEnumerable<ProjectContext> CreateProjectContexts(string projectPath)
        {
            projectPath = projectPath ?? Directory.GetCurrentDirectory();

            if (!projectPath.EndsWith(Project.FileName))
            {
                projectPath = Path.Combine(projectPath, Project.FileName);
            }

            if (!File.Exists(projectPath))
            {
                throw new InvalidOperationException($"{projectPath} does not exist.");
            }

            return ProjectContext.CreateContextForEachFramework(projectPath);
        }
        
        private static void RegisterForParentProcessExit(int id)
        {
            var parentProcess = Process.GetProcesses().FirstOrDefault(p => p.Id == id);

            if (parentProcess != null)
            {
                parentProcess.EnableRaisingEvents = true;
                parentProcess.Exited += (sender, eventArgs) =>
                {
                    // TestHostTracing.Source.TraceEvent(
                    //     TraceEventType.Information,
                    //     0,
                    //     "Killing the current process as parent process has exited.");

                    Process.GetCurrentProcess().Kill();
                };
            }
            else
            {
                // TestHostTracing.Source.TraceEvent(
                //     TraceEventType.Information,
                //     0,
                //     "Failed to register for parent process's exit event. " +
                //     $"Parent process with id '{id}' was not found.");
            }
        }
        
        
        private static void HandleVerboseOption(ref string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-v" || args[i] == "--verbose")
                {
                    Environment.SetEnvironmentVariable(CommandContext.Variables.Verbose, bool.TrueString);
                    args = args.Take(i).Concat(args.Skip(i + 1)).ToArray();

                    return;
                }
            }
        }
    }
}