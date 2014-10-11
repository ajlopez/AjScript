namespace AjScript.Tests.Expressions
{
    using System;
    using AjScript.Expressions;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InstanceOfExpressionTests
    {
        [TestMethod]
        public void EvaluateObject()
        {
            Context context = new Context();
            var objfn = new ObjectFunction(context);
            context.SetValue("Object", objfn);

            InstanceOfExpression expr = new InstanceOfExpression(new ConstantExpression(objfn.NewInstance(null)), new VariableExpression("Object"));
            var result = expr.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }
}
