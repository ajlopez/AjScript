namespace AjScript.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringUtilitiesTests
    {
        [TestMethod]
        public void GetEmptyString()
        {
            AssertParts(string.Empty, string.Empty);
        }

        [TestMethod]
        public void GetSimpleString()
        {
            AssertParts("simple string", "simple string");
        }

        [TestMethod]
        public void GetMiddleExpansion()
        {
            AssertParts("simple ${expansion expression} string", "simple ", "expansion expression", " string");
        }

        [TestMethod]
        public void GetFullExpansion()
        {
            AssertParts("${expansion expression}", string.Empty, "expansion expression");
        }

        [TestMethod]
        public void GetExpansionAtStart()
        {
            AssertParts("${expansion expression} rest of string", string.Empty, "expansion expression", " rest of string");
        }

        [TestMethod]
        public void GetExpansionAtEnd()
        {
            AssertParts("beginning of string ${expansion expression}", "beginning of string ", "expansion expression");
        }

        [TestMethod]
        public void GetMultipleExpansion()
        {
            AssertParts("a${b}c${d}e", "a", "b", "c", "d", "e");
        }

        private static void AssertParts(string text, params string[] expectedparts)
        {
            IList<string> parts = StringUtilities.SplitText(text);

            Assert.IsNotNull(parts);
            Assert.AreEqual(expectedparts.Length, parts.Count);

            for (int k = 0; k < expectedparts.Length; k++)
                Assert.AreEqual(expectedparts[k], parts[k]);
        }
    }
}
