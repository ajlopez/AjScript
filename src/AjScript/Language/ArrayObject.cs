namespace AjScript.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayObject : DynamicObject, IEnumerable
    {
        private IList<object> elements;

        public ArrayObject(IFunction function, IList<object> elements)
            : base(function)
        {
            this.elements = elements;
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
}
