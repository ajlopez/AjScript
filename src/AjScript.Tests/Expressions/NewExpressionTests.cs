namespace AjScript.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using AjScript.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using AjScript.Interpreter;

    [TestClass]
    public class NewExpressionTests
    {
        [TestMethod]
        public void EvaluateSimpleNewExpression()
        {
            IExpression expression = new NewExpression("System.Data.DataSet", null);

            object result = expression.Evaluate(new Context());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.Data.DataSet));
        }

        [TestMethod]
        public void EvaluateNewExpressionWithArguments()
        {
            IExpression expression = new NewExpression("System.IO.DirectoryInfo", new IExpression[] { new ConstantExpression(".") });

            object result = expression.Evaluate(new Context());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.IO.DirectoryInfo));

            DirectoryInfo di = (DirectoryInfo)result;

            DirectoryInfo current = new DirectoryInfo(".");

            Assert.AreEqual(current.FullName, di.FullName);
        }

        [TestMethod]
        public void EvaluateNewExpressionWithAliasedType()
        {
            IExpression expression = new NewExpression("Lexer", new List<IExpression>() { new ConstantExpression(string.Empty) });
            Context context = new Context();
            context.SetValue("Lexer", typeof(Lexer));

            object result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Lexer));
        }
    }
}
