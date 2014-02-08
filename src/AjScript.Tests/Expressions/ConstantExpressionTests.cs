namespace AjScript.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConstantExpressionTests
    {
        [TestMethod]
        public void CreateIntegerConstantExpression()
        {
            ConstantExpression expression = new ConstantExpression(1);

            Assert.AreEqual(1, expression.Value);
        }

        [TestMethod]
        public void EvaluateStringConstantExpression()
        {
            ConstantExpression expression = new ConstantExpression("One");

            Assert.AreEqual("One", expression.Evaluate(null));
        }
    }
}
