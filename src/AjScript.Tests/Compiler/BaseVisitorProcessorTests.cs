using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AjScript.Compiler;
using AjScript.Commands;
using AjScript.Interpreter;

namespace AjScript.Tests.Compiler
{
    [TestClass]
    public class BaseVisitorProcessorTests
    {
        [TestMethod]
        public void ProcessVarCommands()
        {
            MyVisitorProcessor processor = new MyVisitorProcessor();
            ICommand command = ParseCommands("var x; var y;");
            MyVisitorContext context = new MyVisitorContext();
            processor.Process(context, command);
            Assert.AreEqual(3, context.Count);
        }

        private static ICommand ParseCommands(string text)
        {
            Parser parser = new Parser(text);
            return parser.ParseCommands();
        }
    }

    class MyVisitorContext : IVisitorContext
    {
        private int count;

        public int Count { get { return this.count; } }

        public void Increment()
        {
            this.count++;
        }
    }

    class MyVisitorProcessor : BaseVisitorProcessor<MyVisitorContext>
    {
        internal MyVisitorProcessor()
        {
            this.RegisterVisitor(new MyCompositeCommandVisitor());
            this.RegisterVisitor(new MyVarCommandVisitor());
        }
    }

    class MyCompositeCommandVisitor : BaseVisitor<CompositeCommand, MyVisitorContext>
    {
        public override void Process(IVisitorProcessor<MyVisitorContext> processor, MyVisitorContext context, CompositeCommand command)
        {
            context.Increment();

            if (command.HoistedCommands != null)
                foreach (ICommand cmd in command.HoistedCommands)
                    processor.Process(context, cmd);

            if (command.Commands != null)
                foreach (ICommand cmd in command.Commands)
                    processor.Process(context, cmd);
        }
    }

    class MyVarCommandVisitor : BaseVisitor<VarCommand, MyVisitorContext>
    {
        public override void Process(IVisitorProcessor<MyVisitorContext> processor, MyVisitorContext context, VarCommand command)
        {
            context.Increment();
        }
    }
}
