using System;
using System.IO;
using System.Reflection;
using Microsoft.DotNet.ProjectModel;
using Newtonsoft.Json;
using System.Linq;

namespace BananaLauncher.Tool
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(" args = " + string.Join(" ", args));
            Console.WriteLine(" cwd = " + Directory.GetCurrentDirectory());
            Console.WriteLine(" app context = " + AppContext.BaseDirectory);
            
            // var jsonNet = typeof(JsonConvert).GetTypeInfo().Assembly.GetName().FullName;
            // Console.WriteLine($"Newtonsoft.Json = {jsonNet}");
            
            var project = ProjectReader.GetProject(Directory.GetCurrentDirectory());
            Console.WriteLine(" name = "+ project.Name);
            var assemblypath = 
                Path.Combine(AppContext.BaseDirectory, project.Name + FileNameSuffixes.DotNet.DynamicLib);
            Console.WriteLine(" assembly path = " + assemblypath);
                
            var userAssembly = Assembly.Load(new AssemblyName { Name = project.Name});
            Console.WriteLine($" user assembly = {userAssembly.GetName().FullName}");
            foreach(var type in userAssembly.DefinedTypes)
            {
                Console.WriteLine(" type = " + type.FullName);
            }
            
            Console.WriteLine("Activating all instances of types that contain 'Banana' in their name or namespace'. Constructor should have string[] as only param");
            
            var bananaSql = userAssembly.DefinedTypes.Where(t => t.FullName.Contains("Banana"))
                .Select(t => Activator.CreateInstance(t.AsType(), new []{ args })).ToList();
        }
    }
}