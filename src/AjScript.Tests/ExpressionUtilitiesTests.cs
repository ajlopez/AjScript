namespace AjScript.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using AjScript.Expressions;
    using AjScript.Language;

    [TestClass]
    public class ExpressionUtilitiesTests
    {
        [TestMethod]
        public void ResolveVariableExpressionToObject()
        {
            Context context = new Context();
            IExpression expression = new VariableExpression("foo");

            object obj = ExpressionUtilities.ResolveToObject(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IObject));

            Assert.AreEqual(obj, context.GetValue("foo"));
        }

        [TestMethod]
        public void ResolveVariableExpressionToList()
        {
            Context context = new Context();
            IExpression expression = new VariableExpression("foo");

            object obj = ExpressionUtilities.ResolveToList(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IList));

            Assert.AreEqual(obj, context.GetValue("foo"));
        }

        [TestMethod]
        public void ResolveVariableExpressionToDictionary()
        {
            Context context = new Context();
            IExpression expression = new VariableExpression("foo");

            object obj = ExpressionUtilities.ResolveToDictionary(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));

            Assert.AreEqual(obj, context.GetValue("foo"));
        }

        [TestMethod]
        public void ResolveDotExpressionToObject()
        {
            Context context = new Context();
            IExpression expression = new DotExpression(new VariableExpression("Project"), "Title");

            object obj = ExpressionUtilities.ResolveToObject(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IObject));

            object project = context.GetValue("Project");

            Assert.IsNotNull(project);
            Assert.IsInstanceOfType(project, typeof(IObject));

            object title = ((IObject)project).GetValue("Title");

            Assert.IsNotNull(title);
            Assert.IsInstanceOfType(title, typeof(IObject));

            Assert.AreEqual(obj, title);
        }

        [TestMethod]
        public void ResolveDotExpressionToList()
        {
            Context context = new Context();
            IExpression expression = new DotExpression(new VariableExpression("Project"), "Entities");

            object obj = ExpressionUtilities.ResolveToList(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IList));

            object project = context.GetValue("Project");

            Assert.IsNotNull(project);
            Assert.IsInstanceOfType(project, typeof(IObject));

            object entities = ((IObject)project).GetValue("Entities");

            Assert.IsNotNull(entities);
            Assert.IsInstanceOfType(entities, typeof(IList));

            Assert.AreEqual(obj, entities);
        }

        [TestMethod]
        public void ResolveDotExpressionToDictionary()
        {
            Context context = new Context();
            IExpression expression = new DotExpression(new VariableExpression("Project"), "Entities");

            object obj = ExpressionUtilities.ResolveToDictionary(expression, context);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));

            object project = context.GetValue("Project");

            Assert.IsNotNull(project);
            Assert.IsInstanceOfType(project, typeof(IObject));

            object entities = ((IObject)project).GetValue("Entities");

            Assert.IsNotNull(entities);
            Assert.IsInstanceOfType(entities, typeof(IDictionary));

            Assert.AreEqual(obj, entities);
        }
    }
}
