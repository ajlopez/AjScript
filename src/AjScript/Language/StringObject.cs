namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringObject : DynamicObject
    {
        private static string[] nativeMethods = new string[] { "SetValue", "GetValue", "ToString", "GetNames", "Invoke", "GetHashCode", "Equals" };

        private Dictionary<string, object> values = new Dictionary<string, object>();
        private IFunction function;

        public StringObject()
        {
            this.SetValue("toUpperCase", new LambdaCallable(ToUpperCase));
            this.SetValue("toLowerCase", new LambdaCallable(ToLowerCase));
            this.SetValue("substring", new LambdaCallable(Substring));
        }

        private static object ToUpperCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToUpper();
        }

        private static object ToLowerCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToLower();
        }

        private static object Substring(IContext context, object @this, object[] arguments)
        {
            string str = (string)@this;
            
            int from = 0;
            int to = str.Length;

            if (arguments.Length > 0)
                try
                {
                    from = Convert.ToInt32(arguments[0]);
                }
                catch
                {
                }

            if (arguments.Length > 1)
                try
                {
                    to = Convert.ToInt32(arguments[1]);
                }
                catch
                {
                }

            if (from < 0)
                from = 0;

            if (to > str.Length)
                to = str.Length;

            return str.Substring(from, to - from);
        }
    }
}
