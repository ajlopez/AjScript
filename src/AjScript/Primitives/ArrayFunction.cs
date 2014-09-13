namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class ArrayFunction : Function
    {
        public ArrayFunction(IContext context)
            : base(null, null, context)
        {
            var prototype = new DynamicObject();

            this.SetValue("prototype", prototype);
            prototype.SetValue("shift", new ShiftFunction());
        }

        public override object NewInstance(object[] parameters)
        {
            return new ArrayObject(this, new List<object>());
        }

        private class ShiftFunction : ICallable
        {
            public int Arity
            {
                get { return 0; }
            }

            public IContext Context
            {
                get { return null; }
            }

            public object Invoke(IContext context, object @this, object[] arguments)
            {
                ArrayObject array = (ArrayObject)@this;
                var result = array.Elements[0];
                array.Elements.RemoveAt(0);
                return result;
            }
        }
    }
}
