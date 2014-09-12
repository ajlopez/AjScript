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
        }

        public override object NewInstance(object[] parameters)
        {
            return new ArrayObject(new List<object>());
        }
    }
}
