namespace AjScript.Tests.Expressions
{
    using System;
    using AjScript.Expressions;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeofExpressionTests
    {
        [TestMethod]
        public void EvaluateNull()
        {
            TypeofExpression expr = new TypeofExpression(new ConstantExpression(null));
            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void EvaluateString()
        {
            TypeofExpression expr = new TypeofExpression(new ConstantExpression("foo"));
            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("string", result);
        }

        [TestMethod]
        public void EvaluateInteger()
        {
            TypeofExpression expr = new TypeofExpression(new ConstantExpression(42));
            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("number", result);
        }

        [TestMethod]
        public void EvaluateReal()
        {
            TypeofExpression expr = new TypeofExpression(new ConstantExpression(Math.PI));
            var result = expr.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("number", result);
        }

        [TestMethod]
        public void EvaluateUndefined()
        {
            TypeofExpression expr = new TypeofExpression(new VariableExpression("foo"));
            var result = expr.Evaluate(new Context());

            Assert.IsNotNull(result);
            Assert.AreEqual("undefined", result);
        }

        [TestMethod]
        public void EvaluateArray()
        {
            TypeofExpression expr = new TypeofExpression(new VariableExpression("foo"));
            Context ctx = new Context();
            ctx.SetValue("foo", (new ArrayFunction(ctx)).NewInstance(new object[] { }));
            var result = expr.Evaluate(ctx);

            Assert.IsNotNull(result);
            Assert.AreEqual("object", result);
        }
    }
}
