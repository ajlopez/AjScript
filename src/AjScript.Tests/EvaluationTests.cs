namespace AjScript.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Interpreter;
    using AjScript.Language;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EvaluationTests
    {
        private Machine machine;

        [TestInitialize]
        public void Setup()
        {
            this.machine = new Machine();
        }

        [TestMethod]
        public void GetInteger()
        {
            Assert.AreEqual(3, this.EvaluateExpression("3"));
        }

        [TestMethod]
        public void GetNegativeInteger()
        {
            Assert.AreEqual(-3, this.EvaluateExpression("-3"));
        }

        [TestMethod]
        public void GetReal()
        {
            Assert.AreEqual(3.14, this.EvaluateExpression("3.14"));
        }

        [TestMethod]
        public void GetNegativeReal()
        {
            Assert.AreEqual(-3.14, this.EvaluateExpression("-3.14"));
        }

        [TestMethod]
        public void AddSimpleExpression()
        {
            Assert.AreEqual(3, this.EvaluateExpression("1+2"));
        }

        [TestMethod]
        public void AddRealsSimpleExpression()
        {
            Assert.AreEqual(1.2 + 3.4, this.EvaluateExpression("1.2+3.4"));
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
        public void DivideIntegers()
        {
            Assert.AreEqual(3, this.EvaluateExpression("9/3"));
            Assert.AreEqual(1.0/2.0, this.EvaluateExpression("1/2"));
        }

        [TestMethod]
        public void GetNull()
        {
            Assert.IsNull(this.EvaluateExpression("null"));
        }

        [TestMethod]
        public void ConcatenateNull()
        {
            Assert.AreEqual("foonull", this.EvaluateExpression("'foo' + null"));
            Assert.AreEqual("nullfoo", this.EvaluateExpression("null + 'foo'"));
        }

        [TestMethod]
        public void AddNull()
        {
            Assert.AreEqual(10, this.EvaluateExpression("10 + null"));
            Assert.AreEqual(10, this.EvaluateExpression("null + 10"));
        }

        [TestMethod]
        public void GetUndefined()
        {
            Assert.AreSame(Undefined.Instance, this.EvaluateExpression("undefined"));
        }

        [TestMethod]
        public void EvaluateVar()
        {
            this.EvaluateCommands("var x;");
            Assert.AreEqual(Undefined.Instance, this.machine.Context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVar()
        {
            this.EvaluateCommands("var x=1;");
            Assert.AreEqual(1, this.machine.Context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVarWithInitialValue()
        {
            this.EvaluateCommands("var x=1+2;");
            Assert.AreEqual(3, this.machine.Context.GetValue("x"));
        }

        [TestMethod]
        public void DefineVarWithInitialExpressionValue()
        {
            this.EvaluateCommands("var x=1+2;");
            Assert.AreEqual(3, this.machine.Context.GetValue("x"));
        }

        [TestMethod]
        public void SetUndefinedVar()
        {
            this.EvaluateCommands("x = 1+2;");
            Assert.AreEqual(3, this.machine.Context.GetValue("x"));
        }

        [TestMethod]
        public void PreIncrementVar()
        {
            this.EvaluateCommands("var x = 0; y = ++x;");
            Assert.AreEqual(1, this.machine.Context.GetValue("x"));
            Assert.AreEqual(1, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void PostIncrementVar()
        {
            this.EvaluateCommands("var x = 0; y = x++;");
            Assert.AreEqual(1, this.machine.Context.GetValue("x"));
            Assert.AreEqual(0, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void EmptyObject()
        {
            object result = this.EvaluateExpression("{}");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));
        }

        [TestMethod]
        public void Typeof()
        {
            Assert.AreEqual("object", this.EvaluateExpression("typeof {}"));
            Assert.AreEqual("object", this.EvaluateExpression("typeof []"));
            Assert.AreEqual("number", this.EvaluateExpression("typeof 42"));
            Assert.AreEqual("number", this.EvaluateExpression("typeof 3.14159"));
            Assert.AreEqual("null", this.EvaluateExpression("typeof null"));
            Assert.AreEqual("undefined", this.EvaluateExpression("typeof undefined"));
        }

        [TestMethod]
        public void IsArray()
        {
            Assert.AreEqual(true, this.EvaluateExpression("Array.isArray([])"));
            Assert.AreEqual(true, this.EvaluateExpression("Array.isArray([1, 2, 3])"));

            Assert.AreEqual(false, this.EvaluateExpression("Array.isArray({})"));
            Assert.AreEqual(false, this.EvaluateExpression("Array.isArray(42)"));
            Assert.AreEqual(false, this.EvaluateExpression("Array.isArray('foo')"));
            Assert.AreEqual(false, this.EvaluateExpression("Array.isArray(null)"));
            Assert.AreEqual(false, this.EvaluateExpression("Array.isArray(undefined)"));
        }

        [TestMethod]
        public void PreDecrementVar()
        {
            this.EvaluateCommands("var x = 0; y = --x;");
            Assert.AreEqual(-1, this.machine.Context.GetValue("x"));
            Assert.AreEqual(-1, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void PostDecrementVar()
        {
            this.EvaluateCommands("var x = 0; y = x--;");
            Assert.AreEqual(-1, this.machine.Context.GetValue("x"));
            Assert.AreEqual(0, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void SimpleFor()
        {
            this.EvaluateCommands("var y = 1; for (var x=1; x<4; x++) y = y*x;");
            Assert.AreEqual(4, this.machine.Context.GetValue("x"));
            Assert.AreEqual(6, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void SimpleForWithBlock()
        {
            this.EvaluateCommands("var y = 1; for (var x=1; x<4; x++) { y = y*x; y = y*2; }");
            Assert.AreEqual(4, this.machine.Context.GetValue("x"));
            Assert.AreEqual(48, this.machine.Context.GetValue("y"));
        }

        [TestMethod]
        public void IntegerToString()
        {
            Assert.AreEqual("42", this.EvaluateExpression("42.toString()"));
        }

        [TestMethod]
        public void StringToString()
        {
            Assert.AreEqual("foo", this.EvaluateExpression("'foo'.toString()"));
        }

        [TestMethod]
        public void EmptyToString()
        {
            Assert.AreEqual("[object Object]", this.EvaluateExpression("{}.toString()"));
        }

        [TestMethod]
        public void AddFunction()
        {
            Assert.AreEqual(3, this.EvaluateExpression("function (x) { return x+1;} (2)"));
        }

        [TestMethod]
        public void DefineAndEvaluateAddFunction()
        {
            this.EvaluateCommands("var add1 = function (x) { return x+1;}; result = add1(2);");
            Assert.AreEqual(3, this.machine.Context.GetValue("result"));
        }

        [TestMethod]
        public void DefineAndEvaluateFunctionWithClosure()
        {
            this.EvaluateCommands("var addx = function (x) { return function(y) { return x+y;}; }; result = addx(2)(3);");
            Assert.AreEqual(5, this.machine.Context.GetValue("result"));
        }

        [TestMethod]
        public void NewObject()
        {
            object result = this.EvaluateExpression("new Object()");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));
        }

        [TestMethod]
        public void NewObjectUsingPrototype()
        {
            this.EvaluateCommands("var x = new Object(); Object.prototype.y = 10; result = x.y;");
            Assert.AreEqual(10, this.machine.Context.GetValue("result"));
        }

        [TestMethod]
        public void NewEmptyObjectUsingPrototype()
        {
            this.EvaluateCommands("var x = {}; Object.prototype.y = 10; result = x.y;");
            Assert.AreEqual(10, this.machine.Context.GetValue("result"));
        }

        [TestMethod]
        public void SetValueUsingArrayNotation()
        {
            this.EvaluateCommands("var x = new Object(); x['y'] = 10; result = x.y;");
            Assert.AreEqual(10, this.machine.Context.GetValue("result"));
        }

        [TestMethod]
        public void NewDirectoryInfo()
        {
            var expr = this.EvaluateExpression("new System.IO.DirectoryInfo('.')");
            Assert.IsNotNull(expr);
            Assert.IsInstanceOfType(expr, typeof(System.IO.DirectoryInfo));
        }

        [TestMethod]
        public void EvaluateSimpleBooleans()
        {
            Assert.AreEqual(true, this.EvaluateExpression("true && true"));
            Assert.AreEqual(true, this.EvaluateExpression("false || true"));
            Assert.AreEqual(false, this.EvaluateExpression("true && false"));
            Assert.AreEqual(false, this.EvaluateExpression("false || false"));
        }

        [TestMethod]
        public void EvaluateFunctionWithGlobalThis()
        {
            this.EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            this.EvaluateExpression("Person()");
            Assert.AreEqual("Adam", this.machine.Context.GetValue("name"));
            Assert.AreEqual(800, this.machine.Context.GetValue("age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingCallWithOneParameterAsThis()
        {
            this.EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            this.EvaluateCommands("var adam = {};");
            this.EvaluateExpression("Person.call(adam)");
            Assert.AreEqual("Adam", this.EvaluateExpression("adam.name"));
            Assert.AreEqual(800, this.EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingCallWithParameters()
        {
            this.EvaluateCommands("function MakePerson(name, age) { this.name = name; this.age = age; }");
            this.EvaluateCommands("var adam = {};");
            this.EvaluateExpression("MakePerson.call(adam, ['Adam', 800])");
            Assert.AreEqual("Adam", this.EvaluateExpression("adam.name"));
            Assert.AreEqual(800, this.EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingApplyWithOneParameterAsThis()
        {
            this.EvaluateCommands("function Person() { this.name = 'Adam'; this.age = 800; }");
            this.EvaluateCommands("var adam = {};");
            this.EvaluateExpression("Person.apply(adam)");
            Assert.AreEqual("Adam", this.EvaluateExpression("adam.name"));
            Assert.AreEqual(800, this.EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateFunctionUsingApplyWithParameters()
        {
            this.EvaluateCommands("function MakePerson(name, age) { this.name = name; this.age = age; }");
            this.EvaluateCommands("var adam = {};");
            this.EvaluateExpression("MakePerson.apply(adam, 'Adam', 800)");
            Assert.AreEqual("Adam", this.EvaluateExpression("adam.name"));
            Assert.AreEqual(800, this.EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void EvaluateNewWithFunction()
        {
            this.EvaluateCommands("function foo() { this.name = 'Adam'; this.age = 800; }");
            object result = this.EvaluateExpression("new foo()");
            Assert.IsInstanceOfType(result, typeof(IObject));
            IObject obj = (IObject)result;

            Assert.AreEqual("Adam", obj.GetValue("name"));
            Assert.AreEqual(800, obj.GetValue("age"));
        }

        [TestMethod]
        public void EvaluateNewWithObjectFunction()
        {
            this.EvaluateCommands("var obj = new Object(); obj.foo = function() { this.name = 'Adam'; this.age = 800; };");
            object result = this.EvaluateExpression("new obj.foo()");
            Assert.IsInstanceOfType(result, typeof(IObject));
            IObject obj = (IObject)result;

            Assert.AreEqual("Adam", obj.GetValue("name"));
            Assert.AreEqual(800, obj.GetValue("age"));
        }

        [TestMethod]
        public void NewObjectWithPrototype()
        {
            this.EvaluateCommands("function Person() { this.name = 'Adam'; }");
            this.EvaluateCommands("Person.prototype.age = 800; var adam = new Person();");
            Assert.AreEqual(800, this.EvaluateExpression("adam.age"));
        }

        [TestMethod]
        public void NewObjectWithArgument()
        {
            this.EvaluateCommands("function Person(name) { this.name = name; }");
            this.EvaluateCommands("var adam = new Person('Adam');");
            Assert.AreEqual("Adam", this.EvaluateExpression("adam.name"));
        }

        [TestMethod]
        public void NewObjectOverridingPrototypeValue()
        {
            this.EvaluateCommands("function Person() { this.name = 'Adam'; }");
            this.EvaluateCommands("Person.prototype.age = 800; var adam = new Person();");
            this.EvaluateCommands("adam.age = 600;");
            Assert.AreEqual(600, this.EvaluateExpression("adam.age"));
            Assert.AreEqual(800, this.EvaluateExpression("Person.prototype.age"));
        }

        [TestMethod]
        public void AddElementToArray()
        {
            this.EvaluateCommands("var arr = []; arr.push(1); arr.push(2);");
            Assert.AreEqual(2, this.EvaluateExpression("arr.length"));
            Assert.AreEqual(1, this.EvaluateExpression("arr[0]"));
            Assert.AreEqual(2, this.EvaluateExpression("arr[1]"));
        }

        [TestMethod]
        public void ChangeElementInArray()
        {
            this.EvaluateCommands("var arr = [1, 2, 3]; arr[1] = 5;");
            Assert.AreEqual(3, this.EvaluateExpression("arr.length"));
            Assert.AreEqual(1, this.EvaluateExpression("arr[0]"));
            Assert.AreEqual(5, this.EvaluateExpression("arr[1]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr[2]"));
        }

        [TestMethod]
        public void SliceCompleteArray()
        {
            this.EvaluateCommands("var arr = [1, 2, 3]; var arr2 = arr.slice();");
            Assert.AreEqual(3, this.EvaluateExpression("arr2.length"));
            Assert.AreEqual(1, this.EvaluateExpression("arr2[0]"));
            Assert.AreEqual(2, this.EvaluateExpression("arr2[1]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr2[2]"));
        }

        [TestMethod]
        public void SliceWithOneArgument()
        {
            this.EvaluateCommands("var arr = [1, 2, 3]; var arr2 = arr.slice(1);");
            Assert.AreEqual(2, this.EvaluateExpression("arr2.length"));
            Assert.AreEqual(2, this.EvaluateExpression("arr2[0]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr2[1]"));
        }

        [TestMethod]
        public void SliceWithTwoArgument()
        {
            this.EvaluateCommands("var arr = [1, 2, 3]; var arr2 = arr.slice(1, 2);");
            Assert.AreEqual(1, this.EvaluateExpression("arr2.length"));
            Assert.AreEqual(2, this.EvaluateExpression("arr2[0]"));
        }

        [TestMethod]
        public void SliceWithNegativeFirstArgument()
        {
            this.EvaluateCommands("var arr = [1, 2, 3, 4]; var arr2 = arr.slice(-2);");
            Assert.AreEqual(2, this.EvaluateExpression("arr2.length"));
            Assert.AreEqual(3, this.EvaluateExpression("arr2[0]"));
            Assert.AreEqual(4, this.EvaluateExpression("arr2[1]"));
        }

        [TestMethod]
        public void SliceWithNegativeSecondArgument()
        {
            this.EvaluateCommands("var arr = [1, 2, 3, 4]; var arr2 = arr.slice(1, -1);");
            Assert.AreEqual(2, this.EvaluateExpression("arr2.length"));
            Assert.AreEqual(2, this.EvaluateExpression("arr2[0]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr2[1]"));
        }

        [TestMethod]
        public void UnshiftAddElementIntoArray()
        {
            this.EvaluateCommands("var arr = [1, 2, 3];");
            Assert.AreEqual(4, this.EvaluateExpression("arr.unshift(4)"));
            Assert.AreEqual(4, this.EvaluateExpression("arr.length"));
            Assert.AreEqual(4, this.EvaluateExpression("arr[0]"));
            Assert.AreEqual(1, this.EvaluateExpression("arr[1]"));
            Assert.AreEqual(2, this.EvaluateExpression("arr[2]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr[3]"));
        }

        [TestMethod]
        public void ShiftRemoveElementFromArray()
        {
            this.EvaluateCommands("var arr = [1, 2, 3];");
            Assert.AreEqual(1, this.EvaluateExpression("arr.shift()"));
            Assert.AreEqual(2, this.EvaluateExpression("arr.length"));
            Assert.AreEqual(2, this.EvaluateExpression("arr[0]"));
            Assert.AreEqual(3, this.EvaluateExpression("arr[1]"));
        }

        [TestMethod]
        public void PopRemoveElementFromArray()
        {
            this.EvaluateCommands("var arr = [1, 2, 3];");
            Assert.AreEqual(3, this.EvaluateExpression("arr.pop()"));
            Assert.AreEqual(2, this.EvaluateExpression("arr.length"));
            Assert.AreEqual(1, this.EvaluateExpression("arr[0]"));
            Assert.AreEqual(2, this.EvaluateExpression("arr[1]"));
        }

        [TestMethod]
        public void InstanceOf()
        {
            Assert.AreEqual(false, this.EvaluateExpression("1 instanceof Object"));
            Assert.AreEqual(true, this.EvaluateExpression("[] instanceof Array"));
            Assert.AreEqual(true, this.EvaluateExpression("[] instanceof Object"));
            Assert.AreEqual(true, this.EvaluateExpression("{} instanceof Object"));
            Assert.AreEqual(false, this.EvaluateExpression("1 instanceof Number"));
            Assert.AreEqual(false, this.EvaluateExpression("'foo' instanceof String"));
        }

        [TestMethod]
        public void NonStrictEqualNullUndefined()
        {
            Assert.AreEqual(true, this.EvaluateExpression("null == undefined"));
            Assert.AreEqual(true, this.EvaluateExpression("undefined == null"));
            Assert.AreEqual(false, this.EvaluateExpression("undefined == false"));
            Assert.AreEqual(false, this.EvaluateExpression("null == false"));
            Assert.AreEqual(false, this.EvaluateExpression("false == undefined"));
            Assert.AreEqual(false, this.EvaluateExpression("false == null"));
        }

        [TestMethod]
        public void NonStrictNotEqualNullUndefined()
        {
            Assert.AreEqual(false, this.EvaluateExpression("null != undefined"));
            Assert.AreEqual(false, this.EvaluateExpression("undefined != null"));
            Assert.AreEqual(true, this.EvaluateExpression("undefined != false"));
            Assert.AreEqual(true, this.EvaluateExpression("null != false"));
            Assert.AreEqual(true, this.EvaluateExpression("false != undefined"));
            Assert.AreEqual(true, this.EvaluateExpression("false != null"));
        }

        [TestMethod]
        public void JoinArrayUsingDefaultComma()
        {
            this.EvaluateCommands("var arr = [1, 2, 3];");
            Assert.AreEqual("1,2,3", this.EvaluateExpression("arr.join()"));
        }

        [TestMethod]
        public void JoinArrayUsingDefaultSeparator()
        {
            this.EvaluateCommands("var arr = [1, 2, 3];");
            Assert.AreEqual("1 2 3", this.EvaluateExpression("arr.join(' ')"));
        }

        private void EvaluateCommands(string text)
        {
            Parser parser = new Parser(text);

            for (ICommand cmd = parser.ParseCommand(); cmd != null; cmd = parser.ParseCommand())
                cmd.Execute(this.machine.Context);
        }

        private object EvaluateExpression(string text)
        {
            Parser parser = new Parser(text);
            IExpression expression = parser.ParseExpression();
            Assert.IsNull(parser.ParseExpression());
            return this.machine.Evaluate(expression);
        }
    }
}
