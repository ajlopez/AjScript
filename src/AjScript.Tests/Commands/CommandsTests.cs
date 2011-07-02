namespace AjScript.Commands.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using AjScript;
    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Language;

    [TestClass]
    public class CommandsTests
    {
        [TestMethod]
        public void ExecuteCompositeCommand()
        {
            Context context = new Context();

            SetVariableCommand command1 = new SetVariableCommand("a", new ConstantExpression("bar"));
            SetVariableCommand command2 = new SetVariableCommand("b", new ConstantExpression(1));
            SetVariableCommand command3 = new SetVariableCommand("c", new VariableExpression("a"));

            List<ICommand> commands = new List<ICommand>();
            commands.Add(command1);
            commands.Add(command2);
            commands.Add(command3);

            CompositeCommand command = new CompositeCommand(commands);

            command.Execute(context);

            Assert.AreEqual("bar", context.GetValue("a"));
            Assert.AreEqual(1, context.GetValue("b"));
            Assert.AreEqual("bar", context.GetValue("c"));
        }

        [TestMethod]
        public void ExecuteIfCommandWhenTrue()
        {
            IExpression condition = new ConstantExpression(true);
            ICommand setCommand = new SetVariableCommand("a", new ConstantExpression(1));
            IfCommand command = new IfCommand(condition, setCommand);

            Context context = new Context();

            command.Execute(context);

            Assert.AreEqual(1, context.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteIfCommandWhenFalse()
        {
            IExpression condition = new ConstantExpression(false);
            ICommand setCommand = new SetVariableCommand("a", new ConstantExpression(1));
            IfCommand command = new IfCommand(condition, setCommand);

            Context context = new Context();

            command.Execute(context);

            Assert.AreEqual(Undefined.Instance, context.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteIfCommandElseWhenFalse()
        {
            IExpression condition = new ConstantExpression(false);
            ICommand setXCommand = new SetVariableCommand("a", new ConstantExpression(1));
            ICommand setYCommand = new SetVariableCommand("b", new ConstantExpression(2));
            IfCommand command = new IfCommand(condition, setXCommand, setYCommand);

            Context context = new Context();

            command.Execute(context);

            Assert.AreEqual(Undefined.Instance, context.GetValue("a"));
            Assert.AreEqual(2, context.GetValue("b"));
        }

        [TestMethod]
        public void ExecuteWhileCommand()
        {
            IExpression incrementX = new ArithmeticBinaryExpression(ArithmeticOperator.Add, new ConstantExpression(1), new VariableExpression("a"));
            IExpression decrementY = new ArithmeticBinaryExpression(ArithmeticOperator.Subtract, new VariableExpression("b"), new ConstantExpression(1));
            ICommand setX = new SetVariableCommand("a", incrementX);
            ICommand setY = new SetVariableCommand("b", decrementY);
            List<ICommand> commands = new List<ICommand>();
            commands.Add(setX);
            commands.Add(setY);
            ICommand command = new CompositeCommand(commands);
            IExpression yexpr = new VariableExpression("b");

            WhileCommand whilecmd = new WhileCommand(yexpr, command);

            Context context = new Context();

            context.SetValue("a", 0);
            context.SetValue("b", 5);

            whilecmd.Execute(context);

            Assert.AreEqual(0, context.GetValue("b"));
            Assert.AreEqual(5, context.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteForEachCommand()
        {
            IExpression addToX = new ArithmeticBinaryExpression(ArithmeticOperator.Add, new VariableExpression("b"), new VariableExpression("a"));
            ICommand setX = new SetVariableCommand("a", addToX);
            IExpression values = new ConstantExpression(new int [] { 1, 2, 3 } );

            ForEachCommand foreachcmd = new ForEachCommand("b", values, setX);

            Context context = new Context();

            context.SetValue("a", 0);

            foreachcmd.Execute(context);

            Assert.AreEqual(6, context.GetValue("a"));
        }

        [TestMethod]
        public void ExecuteForCommand()
        {
            ICommand setX = new SetVariableCommand("x", new ConstantExpression(0));
            ICommand setY = new SetVariableCommand("y", new ConstantExpression(0));
            List<ICommand> commands = new List<ICommand>();
            commands.Add(setX);
            commands.Add(setY);
            ICommand initialCommand = new CompositeCommand(commands);

            IExpression condition = new CompareExpression(ComparisonOperator.Less, new VariableExpression("x"), new ConstantExpression(6));

            IExpression addXtoY = new ArithmeticBinaryExpression(ArithmeticOperator.Add, new VariableExpression("y"), new VariableExpression("x"));
            ICommand addToY = new SetVariableCommand("y", addXtoY);

            ICommand endCommand = new SetVariableCommand("x", new ArithmeticBinaryExpression(ArithmeticOperator.Add, new VariableExpression("x"), new ConstantExpression(1)));

            ForCommand forcmd = new ForCommand(initialCommand, condition, endCommand, addToY);

            Context context = new Context();

            context.SetValue("y", null);

            forcmd.Execute(context);

            Assert.AreEqual(15, context.GetValue("y"));
        }

        [TestMethod]
        public void ExecuteSetLocalVariableCommandWithVariable()
        {
            Context context = new Context();
            SetVariableCommand command = new SetVariableCommand("a", new ConstantExpression("bar"));

            command.Execute(context);

            Assert.AreEqual("bar", context.GetValue("a"));
        }
    }
}
