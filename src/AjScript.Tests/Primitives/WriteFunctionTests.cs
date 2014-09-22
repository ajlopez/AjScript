namespace AjScript.Tests.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WriteFunctionTests
    {
        [TestMethod]
        public void WriteString()
        {
            StringWriter writer = new StringWriter();
            WriteFunction wfunc = new WriteFunction(writer);
            wfunc.Invoke(null, null, new object[] { "Hello!" });
            writer.Close();
            Assert.AreEqual("Hello!", writer.ToString());
        }
    }
}
