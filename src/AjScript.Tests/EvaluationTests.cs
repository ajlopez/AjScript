namespace AjScript.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjScript.Language;
    using AjScript.Interpreter;
    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Primitives;

    [TestClass]
    public class EvaluationTests
    {
        private IContext context;

        [TestInitialize]
        public void Setup()
        {
            this.context = new Context();
            this.context.SetValue("Object", new ObjectFunction(this.context));
        }

        [TestMethod]
        public void AddSimpleExpression()
        {
            Assert.AreEqual(3, this.EvaluateExpression("1+2"));
        }

        [TestMethod]
        public void ArithmeticSimpleExpressionWithPrecedence()
        {
            Assert.AreEqual(7, this.EvaluateExpression("1+2*3"));
        }

        [TestMethod]
        public void ArithmeticSimpleExpressionWithParenthesis()
        {
            Assert.AreEqual(9, this.EvaluateExpression("(1+2)*3"));
        }

        [TestMethod]
        public void GetNull()
        {
            Assert.IsNull(this.EvaluateExpression("null"));
        }

        [TestMethod]
        public void GetUndefined()
        {
            Assert.AreSame(Undefined.Instance, this.EvaluateExpression("undefined"));
        }

        [TestMethod]
        public void EvaluateVar()
        {
            EvaluateCommands("var x;");
            Assert.AreEqual(Undefined.Instance, this.context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVar()
        {
            EvaluateCommands("var x=1;");
            Assert.AreEqual(1, this.context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVarWithInitialValue()
        {
            EvaluateCommands("var x=1+2;");
            Assert.AreEqual(3, this.context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVarWithInitialExpressionValue()
        {
            EvaluateCommands("var x=1+2;");
            Assert.AreEqual(3, this.context.GetValue("x"));
        }

        [TestMethod]
        public void SetUndefinedVar()
        {
            EvaluateCommands("x = 1+2;");
            Assert.AreEqual(3, this.context.GetValue("x"));
        }

        [TestMethod]
        public void PreIncrementVar()
        {
            EvaluateCommands("var x = 0; y = ++x;");
            Assert.AreEqual(1, this.context.GetValue("x"));
            Assert.AreEqual(1, this.context.GetValue("y"));
        }

        [TestMethod]
        public void PostIncrementVar()
        {
            EvaluateCommands("var x = 0; y = x++;");
            Assert.AreEqual(1, this.context.GetValue("x"));
            Assert.AreEqual(0, this.context.GetValue("y"));
        }

        [TestMethod]
        public void PreDecrementVar()
        {
            EvaluateCommands("var x = 0; y = --x;");
            Assert.AreEqual(-1, this.context.GetValue("x"));
            Assert.AreEqual(-1, this.context.GetValue("y"));
        }

        [TestMethod]
        public void PostDecrementVar()
        {
            EvaluateCommands("var x = 0; y = x--;");
            Assert.AreEqual(-1, this.context.GetValue("x"));
            Assert.AreEqual(0, this.context.GetValue("y"));
        }

        [TestMethod]
        public void SimpleFor()
        {
            EvaluateCommands("var y = 1; for (var x=1; x<4; x++) y = y*x;");
            Assert.AreEqual(4, this.context.GetValue("x"));
            Assert.AreEqual(6, this.context.GetValue("y"));
        }

        [TestMethod]
        public void SimpleForWithBlock()
        {
            EvaluateCommands("var y = 1; for (var x=1; x<4; x++) { y = y*x; y = y*2; }");
            Assert.AreEqual(4, this.context.GetValue("x"));
            Assert.AreEqual(48, this.context.GetValue("y"));
        }

        [TestMethod]
        public void AddFunction()
        {
            Assert.AreEqual(3, EvaluateExpression("function (x) { return x+1;} (2)"));
        }

        [TestMethod]
        public void DefineAndEvaluateAddFunction()
        {
            EvaluateCommands("var add1 = function (x) { return x+1;}; result = add1(2);");
            Assert.AreEqual(3, this.context.GetValue("result"));
        }

        [TestMethod]
        public void DefineAndEvaluateFunctionWithClosure()
        {
            EvaluateCommands("var addx = function (x) { return function(y) { return x+y;}; }; result = addx(2)(3);");
            Assert.AreEqual(5, this.context.GetValue("result"));
        }

        [TestMethod]
        public void NewObject()
        {
            object result = EvaluateExpression("new Object()");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));
        }

        [TestMethod]
        public void NewObjectUsingPrototype()
        {
            EvaluateCommands("var x = new Object(); Object.prototype.y = 10; result = x.y;");
            Assert.AreEqual(10, this.context.GetValue("result"));
        }

        [TestMethod]
        public void SetValueUsingArrayNotation()
        {
            EvaluateCommands("var x = new Object(); x['y'] = 10; result = x.y;");
            Assert.AreEqual(10, this.context.GetValue("result"));
        }

        [TestMethod]
        public void EvaluateSimpleBooleans()
        {
            Assert.AreEqual(true, EvaluateExpression("true && true"));
            Assert.AreEqual(true, EvaluateExpression("false || true"));
            Assert.AreEqual(false, EvaluateExpression("true && false"));
            Assert.AreEqual(false, EvaluateExpression("false || false"));
        }

        private void EvaluateCommands(string text)
        {
            Parser parser = new Parser(text);

            for (ICommand cmd = parser.ParseCommand(); cmd != null; cmd = parser.ParseCommand())
                cmd.Execute(this.context);
        }

        private object EvaluateExpression(string text)
        {
            Parser parser = new Parser(text);
            IExpression expression = parser.ParseExpression();
            Assert.IsNull(parser.ParseExpression());
            return expression.Evaluate(this.context);
        }
    }
}
