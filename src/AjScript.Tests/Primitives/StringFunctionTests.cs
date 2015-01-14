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
    }
}
