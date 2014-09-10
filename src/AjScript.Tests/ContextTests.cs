namespace AjScript.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void SetAndGetValue()
        {
            Context context = new Context();
            context.SetValue("a", "One");

            Assert.AreEqual("One", context.GetValue("a"));
        }

        [TestMethod]
        public void GetUndefined()
        {
            Context context = new Context();

            Assert.AreEqual(Undefined.Instance, context.GetValue("a"));
        }

        [TestMethod]
        public void GetTopContextWhenParentIsNull()
        {
            Context context = new Context();

            Assert.AreEqual(context, context.RootContext);
        }

        [TestMethod]
        public void GetTopContext()
        {
            Context top = new Context();
            Context context = new Context(top);

            Assert.AreEqual(top, context.RootContext);
        }

        [TestMethod]
        public void SetAndGetValues()
        {
            Context context = new Context();
            string[] names = { "one", "two", "three" };
            int[] values = { 1, 2, 3 };

            for (int k = 0; k < names.Length; k++)
                context.SetValue(names[k], values[k]);

            for (int k = 0; k < names.Length; k++)
                Assert.AreEqual(values[k], context.GetValue(names[k]));
        }

        [TestMethod]
        public void DefineVariable()
        {
            Context context = new Context();

            context.DefineVariable("x");

            Assert.AreEqual(Undefined.Instance, context.GetValue("x"));
        }

        [TestMethod]
        public void DefineSetAndGetVariable()
        {
            Context context = new Context();

            context.DefineVariable("x");

            context.SetValue("x", 123);
            Assert.AreEqual(123, context.GetValue("x"));
        }

        [TestMethod]
        public void GetNames()
        {
            Context context = new Context();
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
        public void GetRootContextAsItSelf()
        {
            Context context = new Context();
            Assert.AreEqual(context, context.RootContext);
        }
    }
}
