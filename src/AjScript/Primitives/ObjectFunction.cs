namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class ObjectFunction : Function
    {
        private static ICallable toStringFunction = new ToStringFunction();

        public ObjectFunction(IContext context)
            : base(null, null, context)
        {
            var prototype = new DynamicObject();

            this.SetValue("prototype", prototype);
            prototype.SetValue("toString", toStringFunction);
        }

        public override object NewInstance(object[] parameters)
        {
            return new DynamicObject(this);
        }

        private class ToStringFunction : ICallable
        {
            public object Invoke(IContext context, object @this, object[] arguments)
            {
                return "[object Object]";
            }
        }
    }
}
