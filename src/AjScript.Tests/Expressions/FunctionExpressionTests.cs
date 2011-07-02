namespace AjScript.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using AjScript.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjScript.Language;
    using AjScript.Commands;

    [TestClass]
    public class FunctionExpressionTests
    {
        [TestMethod]
        public void EvaluateFunctionExpression()
        {
            IExpression expression = new FunctionExpression(null, new string[] { "x" }, new ReturnCommand(new VariableExpression("x")));

            object result = expression.Evaluate(new Context());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Function));

            Function function = (Function)result;

            Assert.AreEqual(1, function.ParameterNames.Length);
            Assert.AreEqual("x", function.ParameterNames[0]);
            Assert.IsNotNull(function.Body);
            Assert.IsInstanceOfType(function.Body, typeof(ReturnCommand));
        }
    }
}
