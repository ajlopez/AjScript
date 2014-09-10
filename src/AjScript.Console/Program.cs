namespace AjScript.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Commands;
    using AjScript.Interpreter;
    using AjScript.Primitives;

    public class Program
    {
        public static void Main(string[] args)
        {
            IContext context = new Context();
            Parser parser = new Parser(System.Console.In);

            context.DefineVariable("write");
            context.SetValue("write", new WriteFunction());
            context.DefineVariable("writeln");
            context.SetValue("writeln", new WriteLineFunction());
            context.DefineVariable("Object");
            context.SetValue("Object", new ObjectFunction(context));

            Console.WriteLine("AjScript 0.1 (JavaScript-like Interpreter in C#)");

            for (ICommand cmd = parser.ParseCommand(); cmd != null; cmd = parser.ParseCommand())
                cmd.Execute(context);
        }
    }
}
