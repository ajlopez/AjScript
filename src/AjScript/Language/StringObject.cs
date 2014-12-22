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
        }

        private static object ToUpperCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToUpper();
        }

        private static object ToLowerCase(IContext context, object @this, object[] arguments)
        {
            return ((string)@this).ToLower();
        }
    }
}
