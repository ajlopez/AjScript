namespace AjScript.Tests.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringFunctionTests
    {
        [TestMethod]
        public void HasPrototype()
        {
            StringFunction function = new StringFunction(null);

            var result = function.GetValue("prototype");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StringObject));
        }

        [TestMethod]
        public void CreateEmptyInstance()
        {
            StringFunction function = new StringFunction(null);

            var result = function.NewInstance(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dynobj = (DynamicObject)result;

            Assert.AreEqual(0, dynobj.GetNames().Count);
        }

        [TestMethod]
        public void CreateInstanceFromString()
        {
            StringFunction function = new StringFunction(null);

            var result = function.NewInstance(new object[] { "foo" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dynobj = (DynamicObject)result;

            Assert.AreEqual(3, dynobj.GetNames().Count);
            Assert.AreEqual("f", dynobj.GetValue("0"));
            Assert.AreEqual("o", dynobj.GetValue("1"));
            Assert.AreEqual("o", dynobj.GetValue("2"));
        }

        [TestMethod]
        public void CreateInstanceFromNullString()
        {
            StringFunction function = new StringFunction(null);

            var result = function.NewInstance(new object[] { null });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dynobj = (DynamicObject)result;

            Assert.AreEqual(4, dynobj.GetNames().Count);
            Assert.AreEqual("n", dynobj.GetValue("0"));
            Assert.AreEqual("u", dynobj.GetValue("1"));
            Assert.AreEqual("l", dynobj.GetValue("2"));
            Assert.AreEqual("l", dynobj.GetValue("3"));
        }

        [TestMethod]
        public void InvokeWithoutArguments()
        {
            StringFunction function = new StringFunction(null);

            var result = function.Invoke(null, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void InvokeWithStringArgument()
        {
            StringFunction function = new StringFunction(null);

            var result = function.Invoke(null, null, new object[] { "foo" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void InvokeWithIntegerArgument()
        {
            StringFunction function = new StringFunction(null);

            var result = function.Invoke(null, null, new object[] { 42 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("42", result);
        }

        [TestMethod]
        public void InvokeWithNullArgument()
        {
            StringFunction function = new StringFunction(null);

            var result = function.Invoke(null, null, new object[] { null });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("null", result);
        }
    }
}
