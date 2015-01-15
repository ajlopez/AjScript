namespace AjScript.Tests
{
    using System;
    using AjScript.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MachineTests
    {
        [TestMethod]
        public void CreateMachineWithContext()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine.Context);

            var result = machine.Context.GetValue("Object");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectFunction));
        }

        [TestMethod]
        public void ArrayFunction()
        {
            Machine machine = new Machine();

            var result = machine.Context.GetValue("Array");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ArrayFunction));
        }

        [TestMethod]
        public void StringFunction()
        {
            Machine machine = new Machine();

            var result = machine.Context.GetValue("String");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StringFunction));
        }
    }
}
