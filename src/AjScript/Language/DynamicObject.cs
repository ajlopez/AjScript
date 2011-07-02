namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject : IObject
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();
        private static string[] nativeMethods = new string[] { "SetValue", "GetValue", "ToString", "GetNames", "Invoke", "GetHashCode", "Equals" };
        private IFunction function;

        public DynamicObject()
        {
        }

        public DynamicObject(IFunction function)
        {
            this.function = function;
        }

        public IFunction Function { get { return this.function; } internal set { this.function = value; } }

        public virtual void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public virtual object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            if (this.function == null)
                return Undefined.Instance;

            object prototype = this.function.GetValue("prototype");

            if (prototype == null || prototype == Undefined.Instance)
                return Undefined.Instance;

            return ((IObject)prototype).GetValue(name);
        }

        public virtual ICollection<string> GetNames()
        {
            return this.values.Keys;
        }

        public virtual bool IsNativeMethod(string name)
        {
            return nativeMethods.Contains(name);
        }

        public virtual object Invoke(string name, object[] parameters)
        {
            object value = this.GetValue(name);

            if ((value == null || value == Undefined.Instance) && this.IsNativeMethod(name))
                return ObjectUtilities.GetNativeValue(this, name, parameters);

            if (value == null || value == Undefined.Instance)
                throw new InvalidOperationException(string.Format("Unknown member '{0}'", name));

            if (!(value is ICallable))
            {
                if (parameters == null)
                    return value;

                throw new InvalidOperationException(string.Format("'{0}' is not a method", name));
            }

            ICallable method = (ICallable)value;

            return method.Invoke(null, this, parameters);

            //TODO set up object environment if needed
            //IContext objenv = new ObjectEnvironment(this, method.Environment);

            //return method.Invoke(objenv, parameters);
        }

        public virtual object Invoke(ICallable method, object[] parameters)
        {
            return method.Invoke(null, this, parameters);
        }
    }
}
