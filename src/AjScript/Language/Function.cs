namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Commands;

    public class Function : DynamicObject, IFunction
    {
        private static ICallable callFunction = new CallFunction();
        private static ICallable applyFunction = new ApplyFunction();
        private static IObject functionPrototype = null;

        private string[] parameterNames;
        private ICommand body;
        private int arity;
        private IContext context;

        static Function()
        {
            functionPrototype = new DynamicObject();
            functionPrototype.SetValue("call", callFunction);
            functionPrototype.SetValue("apply", applyFunction);
        }

        public Function(string[] parameterNames, ICommand body)
            : this(parameterNames, body, null)
        {
        }

        public Function(string[] parameterNames, ICommand body, IContext context)
        {
            // TODO Review this cyclic reference
            this.Function = this;

            DynamicObject prototype = new DynamicObject();

            this.SetValue("prototype", functionPrototype);

            this.parameterNames = parameterNames;
            this.body = body;

            if (parameterNames == null)
                this.arity = 0;
            else
                this.arity = parameterNames.Length;

            this.context = context;
        }

        public int Arity { get { return this.parameterNames == null ? 0 : this.parameterNames.Length; } }

        public string[] ParameterNames { get { return this.parameterNames; } }

        public ICommand Body { get { return this.body; } }

        public virtual object Invoke(IContext context, object @this, object[] arguments)
        {
            // TODO review: any case for use context parameter?
            IContext newctx = new Context(this.context);

            // Set this and arguments
            newctx.DefineVariable("this");
            newctx.SetValue("this", @this);
            newctx.DefineVariable("arguments");
            newctx.SetValue("arguments", arguments);

            for (int k = 0; arguments != null && k < arguments.Length && k < this.parameterNames.Length; k++)
            {
                newctx.DefineVariable(this.parameterNames[k]);
                newctx.SetValue(this.parameterNames[k], arguments[k]);
            }

            // TODO Review: it should be null?
            if (this.Body != null)
                this.Body.Execute(newctx);

            // TODO Review: return undefined it not this?
            if (newctx.ReturnValue == null)
                if (@this != null)
                    return @this;
                else
                    return Undefined.Instance;

            return newctx.ReturnValue.Value;
        }

        public virtual object NewInstance(object[] parameters)
        {
            DynamicObject obj = new DynamicObject(this);
            return this.Invoke(this.context, obj, parameters);
        }

        public override object GetValue(string name)
        {
            if (name == "length")
                return this.parameterNames.Length;

            return base.GetValue(name);
        }

        private class CallFunction : ICallable
        {
            public object Invoke(IContext context, object @this, object[] arguments)
            {
                object newthis = arguments[0];
                object[] args = null;

                if (arguments.Length > 1)
                {
                    object argvalues = arguments[1];
                    if (argvalues is ArrayObject)
                        args = ((ArrayObject)argvalues).ToArray();
                    else
                        args = (object[])argvalues;
                }

                return ((ICallable)@this).Invoke(context, newthis, args);
            }
        }

        private class ApplyFunction : ICallable
        {
            public int Arity
            {
                get { return -1; }
            }

            public object Invoke(IContext context, object @this, object[] arguments)
            {
                object newthis = arguments[0];

                return ((ICallable)@this).Invoke(context, newthis, arguments.Skip(1).ToArray());
            }
        }
    }
}
