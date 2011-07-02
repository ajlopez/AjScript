namespace AjScript.Tests.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjScript.Commands;
    using AjScript.Interpreter;
    using AjScript.Expressions;
    using AjScript.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseConstantExpressions()
        {
            IExpression expression;

            expression = ParseExpression("1");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));
            Assert.AreEqual(1, expression.Evaluate(null));

            expression = ParseExpression("1.2");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));
            Assert.AreEqual(1.2, expression.Evaluate(null));

            expression = ParseExpression("false");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));
            Assert.IsFalse((bool)expression.Evaluate(null));

            expression = ParseExpression("\"foo\"");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));
            Assert.AreEqual("foo", expression.Evaluate(null));

            Assert.IsNull(ParseExpression(""));
        }

        [TestMethod]
        public void ParseSimpleUnaryExpression()
        {
            IExpression expression = ParseExpression("-2");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticUnaryExpression));

            ArithmeticUnaryExpression operation = (ArithmeticUnaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Minus, operation.Operation);
            Assert.IsNotNull(operation.Expression);
            Assert.IsInstanceOfType(operation.Expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseFirstDefinedVariable()
        {
            IExpression expression = ParseExpression("a");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(VariableExpression));

            VariableExpression varexpr = (VariableExpression)expression;
            Assert.AreEqual("a", varexpr.Name);
        }

        [TestMethod]
        public void ParseNullAsConstant()
        {
            IExpression expression = ParseExpression("null");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression consexpr = (ConstantExpression)expression;
            Assert.IsNull(consexpr.Value);
        }

        [TestMethod]
        public void ParseFalseAsConstant()
        {
            IExpression expression = ParseExpression("false");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression consexpr = (ConstantExpression)expression;
            Assert.IsInstanceOfType(consexpr.Value, typeof(bool));
            Assert.IsFalse((bool)consexpr.Value);
        }

        [TestMethod]
        public void ParseTrueAsConstant()
        {
            IExpression expression = ParseExpression("true");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression consexpr = (ConstantExpression)expression;
            Assert.IsInstanceOfType(consexpr.Value, typeof(bool));
            Assert.IsTrue((bool)consexpr.Value);
        }

        [TestMethod]
        public void ParseSecondDefinedVariable()
        {
            IExpression expression = ParseExpression("b");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(VariableExpression));

            VariableExpression varexpr = (VariableExpression)expression;
            Assert.AreEqual("b", varexpr.Name);
        }

        [TestMethod]
        public void ParseNewVariableDefinition()
        {
            ICommand command = ParseCommand("var x;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(VarCommand));

            VarCommand varcmd = (VarCommand)command;
            Assert.AreEqual("x", varcmd.Name);
        }

        [TestMethod]
        public void ParseNewVariableDefinitionAndInitialization()
        {
            ICommand command = ParseCommand("var x = 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));

            CompositeCommand composite = (CompositeCommand)command;
            Assert.AreEqual(2, composite.CommandCount);
            Assert.IsInstanceOfType(composite.Commands.First(), typeof(VarCommand));
            VarCommand varcmd = (VarCommand)composite.Commands.First();
            Assert.AreEqual("x", varcmd.Name);
            Assert.IsInstanceOfType(composite.Commands.ElementAt(1), typeof(SetVariableCommand));
            SetVariableCommand setcmd = (SetVariableCommand)composite.Commands.ElementAt(1);
            Assert.IsInstanceOfType(setcmd.Expression, typeof(ConstantExpression));

            ConstantExpression consexpr = (ConstantExpression)setcmd.Expression;

            Assert.AreEqual(1, consexpr.Value);
        }

        [TestMethod]
        public void ParseSimpleBinaryExpression()
        {
            IExpression expression = ParseExpression("a + 2");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression operation = (ArithmeticBinaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Add, operation.Operation);
            Assert.IsNotNull(operation.LeftExpression);
            Assert.IsInstanceOfType(operation.LeftExpression, typeof(VariableExpression));
            Assert.IsNotNull(operation.RightExpression);
            Assert.IsInstanceOfType(operation.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseModExpression()
        {
            IExpression expression = ParseExpression("a % 2");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression operation = (ArithmeticBinaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Modulo, operation.Operation);
            Assert.IsNotNull(operation.LeftExpression);
            Assert.IsInstanceOfType(operation.LeftExpression, typeof(VariableExpression));
            Assert.IsNotNull(operation.RightExpression);
            Assert.IsInstanceOfType(operation.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseSimpleCompareExpression()
        {
            IExpression expression = ParseExpression("b <= 1");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CompareExpression));

            CompareExpression operation = (CompareExpression)expression;

            Assert.AreEqual(ComparisonOperator.LessEqual, operation.Operation);
            Assert.IsNotNull(operation.LeftExpression);
            Assert.IsInstanceOfType(operation.LeftExpression, typeof(VariableExpression));
            Assert.IsNotNull(operation.RightExpression);
            Assert.IsInstanceOfType(operation.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseSimpleBinaryExpressionWithParenthesis()
        {
            IExpression expression = ParseExpression("((a) + (2))");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression operation = (ArithmeticBinaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Add, operation.Operation);
            Assert.IsNotNull(operation.LeftExpression);
            Assert.IsInstanceOfType(operation.LeftExpression, typeof(VariableExpression));
            VariableExpression varexpr = (VariableExpression)operation.LeftExpression;
            Assert.AreEqual("a", varexpr.Name);
            Assert.IsNotNull(operation.RightExpression);
            Assert.IsInstanceOfType(operation.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseTwoBinaryExpression()
        {
            IExpression expression = ParseExpression("a + 2 - 3");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression operation = (ArithmeticBinaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Subtract, operation.Operation);
            Assert.IsNotNull(operation.LeftExpression);
            Assert.IsInstanceOfType(operation.LeftExpression, typeof(ArithmeticBinaryExpression));
            Assert.IsNotNull(operation.RightExpression);
            Assert.IsInstanceOfType(operation.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseTwoBinaryExpressionDifferentLevels()
        {
            IExpression expression = ParseExpression("a + 2 * 3");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression arithmeticExpression = (ArithmeticBinaryExpression)expression;

            Assert.AreEqual(ArithmeticOperator.Add, arithmeticExpression.Operation);
            Assert.IsNotNull(arithmeticExpression.LeftExpression);
            Assert.IsInstanceOfType(arithmeticExpression.LeftExpression, typeof(VariableExpression));
            Assert.IsNotNull(arithmeticExpression.RightExpression);
            Assert.IsInstanceOfType(arithmeticExpression.RightExpression, typeof(ArithmeticBinaryExpression));

            ArithmeticBinaryExpression rigthExpression = (ArithmeticBinaryExpression) arithmeticExpression.RightExpression;

            Assert.AreEqual(ArithmeticOperator.Multiply, rigthExpression.Operation);
            Assert.IsInstanceOfType(rigthExpression.LeftExpression, typeof(ConstantExpression));
            Assert.IsInstanceOfType(rigthExpression.RightExpression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseSetVariableCommand()
        {
            ICommand command = ParseCommand("a = 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetCommand));

            SetCommand setcmd = (SetCommand)command;

            Assert.IsInstanceOfType(setcmd.LeftValue, typeof(VariableExpression));
            Assert.AreEqual("a", ((VariableExpression)setcmd.LeftValue).Name);
            Assert.IsNotNull(setcmd.Expression);
            Assert.IsInstanceOfType(setcmd.Expression, typeof(ConstantExpression));
            Assert.AreEqual(1, setcmd.Expression.Evaluate(null));
        }

        [TestMethod]
        public void ParseReturnCommand()
        {
            ICommand command = ParseCommand("return;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ReturnCommand));

            ReturnCommand retcmd = (ReturnCommand)command;

            Assert.IsNull(retcmd.Expression);
        }

        [TestMethod]
        public void ParseReturnCommandWithExpression()
        {
            ICommand command = ParseCommand("return 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ReturnCommand));

            ReturnCommand retcmd = (ReturnCommand)command;

            Assert.IsNotNull(retcmd.Expression);
            Assert.IsInstanceOfType(retcmd.Expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseIfCommand()
        {
            ICommand command = ParseCommand("if (c<=1) return 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)command;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNull(ifcmd.ElseCommand);
        }

        [TestMethod]
        public void ParseIfCommandWithElse()
        {
            ICommand command = ParseCommand("if (a<=1) return 1; else return a * (b-1);");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)command;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNotNull(ifcmd.ElseCommand);
        }

        [TestMethod]
        public void ParseSimpleWhile()
        {
            ICommand command = ParseCommand("while (a<10) a=a+1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(WhileCommand));

            WhileCommand whilecmd = (WhileCommand)command;

            Assert.IsNotNull(whilecmd.Condition);
            Assert.IsNotNull(whilecmd.Command);
            Assert.IsInstanceOfType(whilecmd.Command, typeof(SetCommand));
        }

        [TestMethod]
        public void ParseSimpleForIn()
        {
            ICommand command = ParseCommand("for (var k in b) a=a+k;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));

            IList<ICommand> cmds = ((CompositeCommand)command).Commands.ToList();
            Assert.IsInstanceOfType(cmds[0], typeof(VarCommand));
            Assert.IsInstanceOfType(cmds[1], typeof(ForEachCommand));

            ForEachCommand foreachcmd = (ForEachCommand) cmds[1];

            Assert.IsNotNull(foreachcmd.Expression);
            Assert.IsInstanceOfType(foreachcmd.Expression, typeof(VariableExpression));
            Assert.IsNotNull(foreachcmd.Command);
            Assert.IsInstanceOfType(foreachcmd.Command, typeof(SetCommand));
        }

        [TestMethod]
        public void ParseSimpleForInWithLocalVar()
        {
            ICommand command = ParseCommand("for (var x in b) c=c+x;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));

            CompositeCommand ccmd = (CompositeCommand)command;

            Assert.AreEqual(2, ccmd.CommandCount);
            IList<ICommand> cmds = ccmd.Commands.ToList();

            Assert.IsInstanceOfType(cmds[0], typeof(VarCommand));
            Assert.IsInstanceOfType(cmds[1], typeof(ForEachCommand));

            ForEachCommand foreachcmd = (ForEachCommand)cmds[1];

            Assert.IsNotNull(foreachcmd.Expression);
            Assert.IsInstanceOfType(foreachcmd.Expression, typeof(VariableExpression));
            Assert.IsNotNull(foreachcmd.Command);
            Assert.IsInstanceOfType(foreachcmd.Command, typeof(SetCommand));
        }

        [TestMethod]
        public void ParseSimpleIncrement()
        {
            ICommand command = ParseCommand("b++;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            ExpressionCommand expcmd = (ExpressionCommand)command;

            Assert.IsNotNull(expcmd.Expression);
            Assert.IsInstanceOfType(expcmd.Expression, typeof(IncrementExpression));
        }

        [TestMethod]
        public void ParseSimpleFor()
        {
            ICommand command = ParseCommand("for (var k=1; k<=5; k++) a=a+k;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ForCommand));

            ForCommand forcommand = (ForCommand)command;

            Assert.IsNotNull(forcommand.InitialCommand);
            Assert.IsNotNull(forcommand.Condition);
            Assert.IsNotNull(forcommand.EndCommand);
            Assert.IsNotNull(forcommand.Body);
        }

        [TestMethod]
        public void ParseCompositeCommand()
        {
            ICommand command = ParseCommand("{ a=1; b=2; }");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));

            CompositeCommand compcmd = (CompositeCommand)command;

            Assert.AreEqual(2, compcmd.CommandCount);
            Assert.IsNotNull(compcmd.Commands);
            Assert.AreEqual(2, compcmd.Commands.Count);

            foreach (ICommand cmd in compcmd.Commands)
            {
                Assert.IsNotNull(cmd);
                Assert.IsInstanceOfType(cmd, typeof(SetCommand));
            }
        }

        [TestMethod]
        public void ParseWriteCommand()
        {
            ICommand command = ParseCommand("write(1);");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));
        }

        [TestMethod]
        public void ParseSimpleDotExpression()
        {
            IExpression expression = ParseExpression("a.length");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DotExpression));            
        }

        [TestMethod]
        public void ParseSimpleDotExpressionWithArguments()
        {
            IExpression expression = ParseExpression("a.c(1,2)");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DotExpression));
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void RaiseIfUnexpectedTokenDot()
        {
            ParseExpression(".");
        }

        [TestMethod]
        public void ParseSetPropertyCommand()
        {
            ICommand command = ParseCommand("a.FirstName = \"Adam\";");

            Assert.IsNotNull(command);            
        }

        [TestMethod]
        public void ParsePreIncrementExpressionWithVariable()
        {
            IExpression expression = ParseExpression("++b");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IncrementExpression));

            IncrementExpression incexpr = (IncrementExpression)expression;

            Assert.AreEqual(IncrementOperator.PreIncrement, incexpr.Operator);
            Assert.IsNotNull(incexpr.Expression);
            Assert.IsInstanceOfType(incexpr.Expression, typeof(VariableExpression));
        }

        [TestMethod]
        public void ParsePreDecrementExpressionWithDotName()
        {
            IExpression expression = ParseExpression("--a.Age");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IncrementExpression));

            IncrementExpression incexpr = (IncrementExpression)expression;

            Assert.AreEqual(IncrementOperator.PreDecrement, incexpr.Operator);
            Assert.IsNotNull(incexpr.Expression);
            Assert.IsInstanceOfType(incexpr.Expression, typeof(DotExpression));
        }

        [TestMethod]
        public void ParsePostIncrementExpressionWithVariable()
        {
            IExpression expression = ParseExpression("b++");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IncrementExpression));

            IncrementExpression incexpr = (IncrementExpression)expression;

            Assert.AreEqual(IncrementOperator.PostIncrement, incexpr.Operator);
            Assert.IsNotNull(incexpr.Expression);
            Assert.IsInstanceOfType(incexpr.Expression, typeof(VariableExpression));
        }

        [TestMethod]
        public void ParsePostDecrementExpressionWithDotName()
        {
            IExpression expression = ParseExpression("a.Age--");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IncrementExpression));

            IncrementExpression incexpr = (IncrementExpression)expression;

            Assert.AreEqual(IncrementOperator.PostDecrement, incexpr.Operator);
            Assert.IsNotNull(incexpr.Expression);
            Assert.IsInstanceOfType(incexpr.Expression, typeof(DotExpression));
        }

        [TestMethod]
        public void ParseSetArrayCommand()
        {
            ICommand command = ParseCommand("b[0] = 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetArrayCommand));

            SetArrayCommand setcmd = (SetArrayCommand) command;

            Assert.IsInstanceOfType(setcmd.LeftValue, typeof(VariableExpression));
            Assert.AreEqual(1, setcmd.Arguments.Count);
            Assert.IsInstanceOfType(setcmd.Expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseSetArrayCommandWithDotExpression()
        {
            ICommand command = ParseCommand("b.Values[0] = 1;");

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetArrayCommand));

            SetArrayCommand setcmd = (SetArrayCommand)command;

            Assert.IsInstanceOfType(setcmd.LeftValue, typeof(DotExpression));
            Assert.AreEqual(1, setcmd.Arguments.Count);
            Assert.IsInstanceOfType(setcmd.Expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void ParseNotExpression()
        {
            IExpression expression = ParseExpression("!a");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NotExpression));

            NotExpression notexpr = (NotExpression)expression;

            Assert.IsInstanceOfType(notexpr.Expression, typeof(VariableExpression));
        }

        [TestMethod]
        public void ParseAndExpression()
        {
            IExpression expression = ParseExpression("a===1 && b===1");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(AndExpression));

            AndExpression andexpr = (AndExpression)expression;

            Assert.IsInstanceOfType(andexpr.LeftExpression, typeof(CompareExpression));
            Assert.IsInstanceOfType(andexpr.RightExpression, typeof(CompareExpression));
        }

        [TestMethod]
        public void ParseOrExpression()
        {
            IExpression expression = ParseExpression("a===1 || b===1");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(OrExpression));

            OrExpression orexpr = (OrExpression)expression;

            Assert.IsInstanceOfType(orexpr.LeftExpression, typeof(CompareExpression));
            Assert.IsInstanceOfType(orexpr.RightExpression, typeof(CompareExpression));
        }

        [TestMethod]
        public void ParseOrAndExpression()
        {
            IExpression expression = ParseExpression("a===1 || b===1 && c===1");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(OrExpression));

            OrExpression orexpr = (OrExpression)expression;

            Assert.IsInstanceOfType(orexpr.LeftExpression, typeof(CompareExpression));
            Assert.IsInstanceOfType(orexpr.RightExpression, typeof(AndExpression));
        }

        [TestMethod]
        public void ParseAndOrExpression()
        {
            IExpression expression = ParseExpression("a===1 && b===1 || c===1");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(OrExpression));

            OrExpression orexpr = (OrExpression)expression;

            Assert.IsInstanceOfType(orexpr.LeftExpression, typeof(AndExpression));
            Assert.IsInstanceOfType(orexpr.RightExpression, typeof(CompareExpression));
        }

        [TestMethod]
        public void ParseNewObject()
        {
            IExpression expression = ParseExpression("new Object()");

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NewExpression));

            NewExpression newexpr = (NewExpression)expression;

            Assert.AreEqual("Object", newexpr.TypeName);
        }

        [TestMethod]
        public void ParseEmptyFunction()
        {
            IExpression expression = ParseExpression("function() {}");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(FunctionExpression));

            FunctionExpression funexpr = (FunctionExpression)expression;

            Assert.IsInstanceOfType(funexpr.Body, typeof(CompositeCommand));
            Assert.AreEqual(0, funexpr.ParameterNames.Length);
            Assert.IsNull(funexpr.Name);
        }

        [TestMethod]
        public void ParseSimpleFunction()
        {
            IExpression expression = ParseExpression("function(x) { return x+1;}");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(FunctionExpression));

            FunctionExpression funexpr = (FunctionExpression)expression;

            Assert.IsInstanceOfType(funexpr.Body, typeof(CompositeCommand));
            Assert.AreEqual(1, funexpr.ParameterNames.Length);
            Assert.IsNull(funexpr.Name);
        }

        [TestMethod]
        public void ParseSimpleNamedFunction()
        {
            IExpression expression = ParseExpression("function add1(x) { return x+1;}");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(FunctionExpression));

            FunctionExpression funexpr = (FunctionExpression)expression;

            Assert.IsInstanceOfType(funexpr.Body, typeof(CompositeCommand));
            Assert.AreEqual(1, funexpr.ParameterNames.Length);
            Assert.AreEqual("add1", funexpr.Name);
        }

        [TestMethod]
        public void ParseInnerFunctions()
        {
            IExpression expression = ParseExpression("function add1(x) { function bar() {} return x+1; function foo() {}}");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(FunctionExpression));

            FunctionExpression funexpr = (FunctionExpression)expression;

            Assert.IsInstanceOfType(funexpr.Body, typeof(CompositeCommand));
            Assert.AreEqual(1, funexpr.ParameterNames.Length);
            Assert.AreEqual("add1", funexpr.Name);
        }

        [TestMethod]
        public void ParseSimpleObject()
        {
            IExpression expression = ParseExpression("{ name: \"Adam\", age: 800 }");
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ObjectExpression));

            ObjectExpression objexpr = (ObjectExpression)expression;
            Assert.AreEqual(2, objexpr.Names.Count);
            Assert.AreEqual(2, objexpr.Expressions.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfUseThisInAssignment()
        {
            ParseCommand("this = 1;");
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfUseThisAsVariable()
        {
            ParseCommand("var this;");
        }

        [TestMethod]
        public void HoistingVarCommands()
        {
            ICommand command = ParseCommands("x = 1; y=2; var x; var y;");
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
            CompositeCommand composite = (CompositeCommand)command;
            Assert.AreEqual(2, composite.CommandCount);
            Assert.AreEqual(2, composite.HoistedCommandCount);
            Assert.IsInstanceOfType(composite.HoistedCommands.First(), typeof(VarCommand));
        }

        private IExpression ParseExpression(string text)
        {
            Parser parser = CreateParser(text);

            IExpression expression = parser.ParseExpression();

            Assert.IsNull(parser.ParseExpression());

            return expression;
        }

        private ICommand ParseCommand(string text)
        {
            Parser parser = CreateParser(text);

            ICommand command = parser.ParseCommand();

            Assert.IsNull(parser.ParseCommand());

            return command;
        }

        private ICommand ParseCommands(string text)
        {
            Parser parser = CreateParser(text);
            return parser.ParseCommands();
        }

        private Parser CreateParser(string text)
        {
            return new Parser(text);
        }
    }
}
