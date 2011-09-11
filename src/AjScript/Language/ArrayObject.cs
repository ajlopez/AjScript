namespace AjScript.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayObject : DynamicObject, IEnumerable
    {
        private static ICallable pushFunction = new PushFunction();

        private IList<object> elements;

        public ArrayObject(IList<object> elements)
        {
            this.elements = elements;

            // TODO set this values in the Function IDynamicObject prototype
            this.SetValue("push", pushFunction);
        }

        public IList<object> Elements { get { return this.elements; } }

        public object[] ToArray()
        {
            return this.elements.ToArray();
        }

        public override object GetValue(string name)
        {
            if (name == "length")
                return this.elements.Count;

            return base.GetValue(name);
        }

        public IEnumerator GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }
    }

    internal class PushFunction : ICallable
    {
        public int Arity
        {
            get { return 1; }
        }

        public IContext Context
        {
            get { return null; }
        }

        public object Invoke(IContext context, object @this, object[] arguments)
        {
            object newelement = arguments[0];
            ArrayObject array = (ArrayObject)@this;
            array.Elements.Add(newelement);
            return array.Elements.Count;
        }
    }
}
