﻿namespace AjScript.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeUtilitiesTests
    {
        [TestMethod]
        public void GetTypeByName()
        {
            Type type = TypeUtilities.GetType("System.Int32");

            Assert.IsNotNull(type);
            Assert.AreEqual(type, typeof(int));
        }

        [TestMethod]
        public void GetTypeInAnotherAssembly()
        {
            Type type = TypeUtilities.GetType(new Context(), "System.Data.DataSet");

            Assert.IsNotNull(type);
            Assert.AreEqual(type, typeof(System.Data.DataSet));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Unknown Type 'Foo.Bar'")]
        public void RaiseIfUnknownType()
        {
            TypeUtilities.GetType(new Context(), "Foo.Bar");
        }

        [TestMethod]
        public void AsType()
        {
            Assert.IsNotNull(TypeUtilities.AsType("System.IO.File"));
            Assert.IsNull(TypeUtilities.AsType("Foo.Bar"));
        }

        [TestMethod]
        public void IsNamespace()
        {
            Assert.IsTrue(TypeUtilities.IsNamespace("System"));
            Assert.IsTrue(TypeUtilities.IsNamespace("AjScript"));
            Assert.IsTrue(TypeUtilities.IsNamespace("AjScript.Language"));
            Assert.IsTrue(TypeUtilities.IsNamespace("System.IO"));
            Assert.IsTrue(TypeUtilities.IsNamespace("System.Data"));

            Assert.IsFalse(TypeUtilities.IsNamespace("Foo.Bar"));
        }

        [TestMethod]
        public void GetValueFromType()
        {
            Assert.IsFalse((bool)TypeUtilities.InvokeTypeMember(typeof(System.IO.File), "Exists", new object[] { "unknown.txt" }));
        }

        [TestMethod]
        public void GetValueFromEnum()
        {
            Assert.AreEqual(System.UriKind.RelativeOrAbsolute, TypeUtilities.InvokeTypeMember(typeof(System.UriKind), "RelativeOrAbsolute", null));
        }
    }
}
