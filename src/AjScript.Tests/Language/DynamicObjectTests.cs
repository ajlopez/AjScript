namespace AjScript.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript;
    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Language;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void GetUndefinedForUndefinedValue()
        {
            DynamicObject dynobj = new DynamicObject();
            Assert.AreSame(Undefined.Instance, dynobj.GetValue("Foo"));
        }

        [TestMethod]
        public void HasUndefinedName()
        {
            DynamicObject dynobj = new DynamicObject();
            Assert.IsFalse(dynobj.HasName("name"));
        }

        [TestMethod]
        public void HasName()
        {
            DynamicObject dynobj = new DynamicObject();
            dynobj.SetValue("name", "Adam");
            Assert.IsTrue(dynobj.HasName("name"));
        }

        [TestMethod]
        public void HasNameInPrototype()
        {
            Function function = new Function(null, null);
            DynamicObject dynobj = new DynamicObject(function);
            DynamicObject prototype = new DynamicObject();
            function.SetValue("prototype", prototype);
            prototype.SetValue("name", "Adam");
            Assert.IsTrue(dynobj.HasName("name"));
        }

        [TestMethod]
        public void SetAndGetValue()
        {
            DynamicObject dynobj = new DynamicObject();

            dynobj.SetValue("Foo", "Bar");

            Assert.AreEqual("Bar", dynobj.GetValue("Foo"));
        }

        [TestMethod]
        public void RemoveValue()
        {
            DynamicObject dynobj = new DynamicObject();
            dynobj.SetValue("name", "Adam");
            Assert.IsTrue(dynobj.HasName("name"));
            dynobj.RemoveValue("name");
            Assert.IsFalse(dynobj.HasName("name"));
            Assert.AreSame(Undefined.Instance, dynobj.GetValue("name"));
        }

        [TestMethod]
        public void RemoveUndefinedValue()
        {
            DynamicObject dynobj = new DynamicObject();
            dynobj.RemoveValue("name");
            Assert.IsFalse(dynobj.HasName("name"));
            Assert.AreSame(Undefined.Instance, dynobj.GetValue("name"));
        }

        [TestMethod]
        public void GetNames()
        {
            DynamicObject dynobj = new DynamicObject();

            dynobj.SetValue("FirstName", "Adam");
            dynobj.SetValue("LastName", "Genesis");

            ICollection<string> names = dynobj.GetNames();

            Assert.IsNotNull(names);
            Assert.AreEqual(2, names.Count);

            Assert.IsTrue(names.Contains("FirstName"));
            Assert.IsTrue(names.Contains("LastName"));
        }

        [TestMethod]
        public void DefineMethod()
        {
            DynamicObject dynobj = new DynamicObject();

            ICommand body = new ReturnCommand(new VariableExpression("Name"));
            Function function = new Function(null, body);

            dynobj.SetValue("GetName", function);

            object result = dynobj.GetValue("GetName");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ICallable));
            Assert.IsTrue(result == function);
        }

        [TestMethod]
        public void InvokeMethod()
        {
            DynamicObject dynobj = new DynamicObject();

            ICommand body = new ReturnCommand(new DotExpression(new VariableExpression("this"), "Name"));
            Function function = new Function(null, body);

            dynobj.SetValue("Name", "Adam");
            dynobj.SetValue("GetName", function);

            object result = dynobj.Invoke("GetName", new object[] { });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Adam", result);
        }

        [TestMethod]
        public void InvokeNativeMethod()
        {
            DynamicObject dynobj = new DynamicObject();

            ICommand body = new ReturnCommand(new VariableExpression("Name"));
            Function function = new Function(null, body);

            dynobj.SetValue("Name", "Adam");

            object result = dynobj.Invoke("GetValue", new object[] { "Name" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Adam", result);
        }

        [TestMethod]
        public void DetectNativeMethods()
        {
            DynamicObject dynobj = new DynamicObject();

            Assert.IsTrue(dynobj.IsNativeMethod("ToString"));
            Assert.IsTrue(dynobj.IsNativeMethod("GetHashCode"));
            Assert.IsTrue(dynobj.IsNativeMethod("Equals"));

            Assert.IsTrue(dynobj.IsNativeMethod("GetValue"));
            Assert.IsTrue(dynobj.IsNativeMethod("SetValue"));
            Assert.IsTrue(dynobj.IsNativeMethod("GetNames"));
            Assert.IsTrue(dynobj.IsNativeMethod("Invoke"));

            Assert.IsFalse(dynobj.IsNativeMethod("Foo"));
        }

        [TestMethod]
        public void UsePrototypeForGetValue()
        {
            Function function = new Function(null, null);
            DynamicObject dynobj = new DynamicObject(function);

            DynamicObject prototype = new DynamicObject();
            function.SetValue("prototype", prototype);
            prototype.SetValue("x", 10);

            Assert.AreEqual(10, dynobj.GetValue("x"));
        }

        [TestMethod]
        public void GetValueFromPrototypeAndSetValueInObject()
        {
            Function function = new Function(null, null);
            DynamicObject dynobj = new DynamicObject(function);
            DynamicObject prototype = new DynamicObject();
            function.SetValue("prototype", prototype);
            prototype.SetValue("x", 10);
            dynobj.SetValue("x", 20);
            Assert.AreEqual(20, dynobj.GetValue("x"));
            Assert.AreEqual(10, prototype.GetValue("x"));
        }
    }
}
