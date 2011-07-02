namespace AjScript.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using AjScript.Expressions;
    using AjScript.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayExpressionTests
    {
        [TestMethod]
        public void EvaluateArrayVariableExpression()
        {
            Context context = new Context();

            context.SetValue("array", new string[] { "one", "two", "three" });

            IExpression expression = new ArrayExpression(new VariableExpression("array"), new IExpression[] { new ConstantExpression(1) });

            object result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual("two", result);
        }

        [TestMethod]
        public void EvaluateArrayDotExpression()
        {
            Context context = new Context();

            DynamicObject data = new DynamicObject();
            data.SetValue("Numbers", new string[] { "one", "two", "three" });

            context.SetValue("data", data);

            IExpression expression = new ArrayExpression(new DotExpression(new VariableExpression("data"), "Numbers"), new IExpression[] { new ConstantExpression(1) });

            object result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual("two", result);
        }
    }
}
