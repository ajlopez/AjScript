namespace AjScript.Tests.Primitives
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjScript.Primitives;
    using AjScript.Language;

    [TestClass]
    public class ObjectFunctionTests
    {
        [TestMethod]
        public void NewInstance()
        {
            ObjectFunction function = new ObjectFunction(null);
            object instance = function.NewInstance(null);

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(DynamicObject));
        }
    }
}
