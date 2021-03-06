﻿namespace AjScript.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Expressions;
    using AjScript.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IndexedExpressionTests
    {
        [TestMethod]
        public void EvaluateArrayVariableExpression()
        {
            Context context = new Context();

            context.SetValue("array", new string[] { "one", "two", "three" });

            IExpression expression = new IndexedExpression(new VariableExpression("array"), new IExpression[] { new ConstantExpression(1) });

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

            IExpression expression = new IndexedExpression(new DotExpression(new VariableExpression("data"), "Numbers"), new IExpression[] { new ConstantExpression(1) });

            object result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual("two", result);
        }
    }
}
