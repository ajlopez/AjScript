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

        public StringObject()
        {
            this.SetValue("toUpperCase", new LambdaCallable(ToUpperCase));
            this.SetValue("toLowerCase", new LambdaCallable(ToLowerCase));
            this.SetValue("substring", new LambdaCallable(Substring));
            this.SetValue("charAt", new LambdaCallable(CharAt));
            this.SetValue("concat", new LambdaCallable(Concat));
        }

        private static object ToUpperCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToUpper();
        }

        private static object ToLowerCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToLower();
        }

        private static object CharAt(IContext context, object @this, object[] arguments)
        {
            string str = (string)@this;

            int index = Convert.ToInt32(arguments[0]);

            if (index < 0 || index >= str.Length)
                return string.Empty;

            return str.Substring(index, 1);
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

        private static object Concat(IContext context, object @this, object[] arguments)
        {
            string result = (string)@this;

            for (int k = 0; k < arguments.Length; k++)
                result += StringUtilities.AsString(arguments[k]);

            return result;
        }
    }
}
