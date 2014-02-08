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
