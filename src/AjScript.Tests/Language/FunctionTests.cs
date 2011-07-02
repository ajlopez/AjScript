namespace AjScript.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public void InvokeSimpleFunction()
        {
            ICommand body = new ReturnCommand(new VariableExpression("x"));
            Function function = new Function(new string[] { "x" }, body);

            Context context = new Context();
            context.SetValue("x", 2);

            object result = function.Invoke(context, null, new object[] { 1 });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void EvaluateFactorialFunction()
        {
            Context context = new Context();;
            ICallable factorial = BuildFactorialFunction(context);
            context.SetValue("Factorial", factorial);

            object result;

            result = factorial.Invoke(context, null, new object[] { 3 });

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result);

            result = factorial.Invoke(context, null, new object[] { 4 });

            Assert.IsNotNull(result);
            Assert.AreEqual(24, result);

            result = factorial.Invoke(context, null, new object[] { 5 });

            Assert.IsNotNull(result);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void EvaluateWithThisObject()
        {
            Context context = new Context();
            ICommand body = new ReturnCommand(new ArithmeticBinaryExpression(ArithmeticOperator.Add, new VariableExpression("x"), new DotExpression(new VariableExpression("this"), "Age")));
            Function function = new Function(new string[] { "x" }, body);
            DynamicObject person = new DynamicObject();
            person.SetValue("Age", 30);

            Assert.AreEqual(40, function.Invoke(context, person, new object[] { 10 }));
        }

        private static ICallable BuildFactorialFunction(IContext context)
        {
            IExpression condition = new CompareExpression(ComparisonOperator.LessEqual, new VariableExpression("n"), new ConstantExpression(1));

            ICommand return1 = new ReturnCommand(new ConstantExpression(1));
            ICommand return2 = new ReturnCommand(new ArithmeticBinaryExpression(ArithmeticOperator.Multiply,
                new VariableExpression("n"),
                new InvokeExpression(new VariableExpression("Factorial"), new IExpression[] { new ArithmeticBinaryExpression(ArithmeticOperator.Subtract, new VariableExpression("n"), new ConstantExpression(1)) })));

            ICommand ifcmd = new IfCommand(condition, return1, return2);
            ICallable factorial = new Function(new string[] { "n" }, ifcmd, context);

            return factorial;
        }
    }
}
