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
    public class ArrayFunctionTests
    {
        [TestMethod]
        public void NewInstance()
        {
            ArrayFunction function = new ArrayFunction(null);
            object instance = function.NewInstance(null);

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(ArrayObject));
        }
    }
}
