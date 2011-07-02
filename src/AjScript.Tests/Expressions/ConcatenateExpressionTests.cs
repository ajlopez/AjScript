namespace AjScript.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using AjScript.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConcatenateExpressionTests
    {
        [TestMethod]
        public void EvaluateConcatenateExpressionUsingNulls()
        {
            IExpression expression = new ConcatenateExpression(new ConstantExpression(null), new ConstantExpression(null));

            Assert.AreEqual(string.Empty, expression.Evaluate(null));

            expression = new ConcatenateExpression(new ConstantExpression("foo"), new ConstantExpression(null));

            Assert.AreEqual("foo", expression.Evaluate(null));

            expression = new ConcatenateExpression(new ConstantExpression(null), new ConstantExpression("bar"));

            Assert.AreEqual("bar", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateConcatenateExpressionUsingStrings()
        {
            IExpression expression = new ConcatenateExpression(new ConstantExpression("foo"), new ConstantExpression("bar"));

            Assert.AreEqual("foobar", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateConcatenateExpressionUsingIntegers()
        {
            IExpression expression = new ConcatenateExpression(new ConstantExpression(12), new ConstantExpression(34));

            Assert.AreEqual("1234", expression.Evaluate(null));
        }
    }
}
