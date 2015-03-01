namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class StringFunction : Function
    {
        public StringFunction(IContext context)
            : base(null, null, context)
        {
            var prototype = new StringObject();

            this.SetValue("prototype", prototype);
            prototype.SetValue("toString", new LambdaCallable(ToString));
        }

        public override object NewInstance(object[] parameters)
        {
            var dynobj = new DynamicObject(this);

            if (parameters == null || parameters.Length == 0)
                return dynobj;

            var arg = parameters[0];

            if (arg == null)
                arg = "null";
            else
                arg = arg.ToString();

            var str = (string)arg;

            for (int k = 0; k < str.Length; k++)
                dynobj.SetValue(k.ToString(), str[k].ToString());

            dynobj.SetValue("length", str.Length, false);

            return dynobj;
        }

        public override object Invoke(IContext context, object @this, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return string.Empty;

            var arg = arguments[0];

            if (arg == null)
                return "null";

            return arg.ToString();
        }

        private static object ToString(IContext context, object @this, object[] arguments)
        {
            var dynobj = (DynamicObject)@this;

            var result = string.Empty;
            int k = 0;

            for (var value = dynobj.GetValue(k.ToString()); value != null && value != Undefined.Instance; k++, value = dynobj.GetValue(k.ToString()))
                result += dynobj.GetValue(k.ToString()).ToString();

            return result;
            
            return ((string)@this).ToUpper();
        }
    }
}
