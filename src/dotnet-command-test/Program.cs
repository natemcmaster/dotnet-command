using System;
using System.IO;
using System.Reflection;
using Microsoft.DotNet.ProjectModel;
using Newtonsoft.Json;
using System.Linq;

namespace TestCommand
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
            
            var bananaSql = userAssembly.DefinedTypes.Single(t => t.FullName.Contains("BananaSql"));
            Activator.CreateInstance(bananaSql.AsType(),new[]{ args[0] });
        }
    }
}