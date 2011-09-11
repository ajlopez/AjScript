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
            this.context = new TopContext();
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
        public void EmptyObject()
        {
            object result = EvaluateExpression("{}");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));
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
        public void NewEmptyObjectUsingPrototype()
        {
            EvaluateCommands("var x = {}; Object.prototype.y = 10; result = x.y;");
            Assert.AreEqual(10, this.context.GetValue("result"));
        }

        [TestMethod]
        public void SetValueUsingArrayNotation()
        {
            EvaluateCommands("var x = new Object(); x['y'] = 10; result = x.y;");
            Assert.AreEqual(10, this.context.GetValue("result"));
        }

        [TestMethod]
        public void NewDirectoryInfo()
        {
            var expr = EvaluateExpression("new System.IO.DirectoryInfo('temp')");
            Assert.IsNotNull(expr);
            Assert.IsInstanceOfType(expr, typeof(System.IO.DirectoryInfo));
        }

        [TestMethod]
        public void EvaluateSimpleBooleans()
        {
            Assert.AreEqual(true, EvaluateExpression("true && true"));
            Assert.AreEqual(true, EvaluateExpression("false || true"));
            Assert.AreEqual(false, EvaluateExpression("true && false"));
            Assert.AreEqual(false, EvaluateExpression("false || false"));
        }

        [TestMethod]
        public void EvaluateFunctionWithGlobalThis()
        {
            EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            EvaluateExpression("Person()");
            Assert.AreEqual("Adam", this.context.GetValue("name"));
            Assert.AreEqual(800, this.context.GetValue("age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingCallWithOneParameterAsThis()
        {
            EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            EvaluateCommands("var adam = {};");
            EvaluateExpression("Person.call(adam)");
            Assert.AreEqual("Adam", EvaluateExpression("adam.name"));
            Assert.AreEqual(800, EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingCallWithParameters()
        {
            EvaluateCommands("function MakePerson(name, age) { this.name = name; this.age = age; }");
            EvaluateCommands("var adam = {};");
            EvaluateExpression("MakePerson.call(adam, ['Adam', 800])");
            Assert.AreEqual("Adam", EvaluateExpression("adam.name"));
            Assert.AreEqual(800, EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingApplyWithOneParameterAsThis()
        {
            EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            EvaluateCommands("var adam = {};");
            EvaluateExpression("Person.apply(adam)");
            Assert.AreEqual("Adam", EvaluateExpression("adam.name"));
            Assert.AreEqual(800, EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingApplyWithParameters()
        {
            EvaluateCommands("function MakePerson(name, age) { this.name = name; this.age = age; }");
            EvaluateCommands("var adam = {};");
            EvaluateExpression("MakePerson.apply(adam, 'Adam', 800)");
            Assert.AreEqual("Adam", EvaluateExpression("adam.name"));
            Assert.AreEqual(800, EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateNewWithFunction()
        {
            EvaluateCommands("function foo() { this.name = 'Adam'; this.age = 800; }");
            object result = EvaluateExpression("new foo()");
            Assert.IsInstanceOfType(result, typeof(IObject));
            IObject obj = (IObject)result;

            Assert.AreEqual("Adam", obj.GetValue("name"));
            Assert.AreEqual(800, obj.GetValue("age"));
        }

        [TestMethod]
        public void NewObjectWithPrototype()
        {
            EvaluateCommands("function Person() { this.name = 'Adam'; }");
            EvaluateCommands("Person.prototype.age = 800; var adam = new Person();");
            Assert.AreEqual(800, EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void NewObjectOverridingPrototypeValue()
        {
            EvaluateCommands("function Person() { this.name = 'Adam'; }");
            EvaluateCommands("Person.prototype.age = 800; var adam = new Person();");
            EvaluateCommands("adam.age = 600;");
            Assert.AreEqual(600, EvaluateExpression("adam.age"));
            Assert.AreEqual(800, EvaluateExpression("Person.prototype.age"));
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
