namespace AjScript.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjScript.Language;
    using AjScript.Interpreter;
    using AjScript.Commands;
    using AjScript.Expressions;
    using AjScript.Primitives;
    using System.IO;

    [TestClass]
    public class FilesTests
    {
        private IContext context;

        [TestInitialize]
        public void Setup()
        {
            this.context = new Context();
            this.context.SetValue("Object", new ObjectFunction(this.context));
            this.context.SetValue("Function", new Function(null, null, this.context));
        }

        [TestMethod]
        [DeploymentItem(@"Files\FunctionAddOne.js")]
        public void FunctionAddOne()
        {
            Assert.AreEqual(3, this.EvaluateFile("FunctionAddOne.js"));
        }

        [TestMethod]
        [DeploymentItem(@"Files\FunctionAddXWithClosure.js")]
        public void FunctionAddXWithClosure()
        {
            Assert.AreEqual(5, this.EvaluateFile("FunctionAddXWithClosure.js"));
        }

        [TestMethod]
        [DeploymentItem(@"Files\FunctionInnerAddXWithClosure.js")]
        public void FunctionInnerAddXWithClosure()
        {
            Assert.AreEqual(5, this.EvaluateFile("FunctionInnerAddXWithClosure.js"));
        }

        [TestMethod]
        [DeploymentItem(@"Files\ObjectWithObjects.js")]
        public void ObjectWithObjects()
        {
            object result = this.EvaluateFile("ObjectWithObjects.js");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));

            IObject obj = (IObject)result;
            Assert.IsInstanceOfType(obj.GetValue("stooge"), typeof(IObject));
            Assert.IsInstanceOfType(obj.GetValue("flight"), typeof(IObject));
        }

        [TestMethod]
        [DeploymentItem(@"Files\ObjectWithPrivateValues.js")]
        public void ObjectWithPrivateValues()
        {
            Assert.AreEqual(5, this.EvaluateFile("ObjectWithPrivateValues.js"));
        }

        [TestMethod]
        [DeploymentItem(@"Files\FunctionGetStatus.js")]
        public void FunctionGetStatus()
        {
            Assert.AreEqual("amazed", this.EvaluateFile("FunctionGetStatus.js"));
        }

        [TestMethod]
        [DeploymentItem(@"Files\FunctionPrototypeMethod.js")]
        public void FunctionPrototypeMethod()
        {
            Assert.AreEqual(3, this.EvaluateFile("FunctionPrototypeMethod.js"));
        }

        private object EvaluateFile(string filename)
        {
            Parser parser = new Parser(new StreamReader(filename));

            for (ICommand cmd = parser.ParseCommand(); cmd != null; cmd = parser.ParseCommand())
                cmd.Execute(this.context);

            return this.context.GetValue("result");
        }
    }
}
