namespace AjScript.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TopContextTests
    {
        [TestMethod]
        public void GetNames()
        {
            TopContext context = new TopContext();
            context.DefineVariable("a");
            context.DefineVariable("b");
            context.DefineVariable("c");

            var names = context.GetNames();

            Assert.IsNotNull(names);
            Assert.AreEqual(3, names.Count);
            Assert.IsTrue(names.Contains("a"));
            Assert.IsTrue(names.Contains("b"));
            Assert.IsTrue(names.Contains("c"));
        }

        [TestMethod]
        public void GetTopContextAsItSelf()
        {
            TopContext context = new TopContext();
            Assert.AreEqual(context, context.RootContext);
        }
    }
}
