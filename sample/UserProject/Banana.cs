using System;

namespace UserProject
{
    public class Banana
    {
        public Banana(string[] args)
        {
            if(args.Length == 0 )
            {
                Console.WriteLine("Add  some args to 'dotnet command jungle'!");
            }
            foreach(var a in args)
            {
                Console.WriteLine("Banana: " + a);
            }
        }
    }
    
}