namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class ArrayFunction : Function
    {
        private static ICallable pushFunction = new PushFunction();
        private static ICallable popFunction = new PopFunction();
        private static ICallable unshiftFunction = new UnshiftFunction();
        private static ICallable shiftFunction = new ShiftFunction();
        private static ICallable joinFunction = new JoinFunction();
        private static ICallable sliceFunction = new SliceFunction();

        public ArrayFunction(IContext context)
            : base(null, null, context)
        {
            var prototype = new DynamicObject();

            this.SetValue("prototype", prototype);
            prototype.SetValue("push", pushFunction);
            prototype.SetValue("pop", popFunction);
            prototype.SetValue("unshift", unshiftFunction);
            prototype.SetValue("shift", shiftFunction);
            prototype.SetValue("join", joinFunction);
            prototype.SetValue("slice", sliceFunction);
        }

        public override object NewInstance(object[] parameters)
        {
            return new ArrayObject(this, new List<object>());
        }

        private class PushFunction : ICallable
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

        private class PopFunction : ICallable
        {
            public int Arity
            {
                get { return 0; }
            }

            public IContext Context
            {
                get { return null; }
            }

            public object Invoke(IContext context, object @this, object[] arguments)
            {
                ArrayObject array = (ArrayObject)@this;
                var result = array.Elements[array.Elements.Count - 1];
                array.Elements.RemoveAt(array.Elements.Count - 1);
                return result;
            }
        }

        private class UnshiftFunction : ICallable
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
                array.Elements.Insert(0, newelement);
                return newelement;
            }
        }

        private class ShiftFunction : ICallable
        {
            public int Arity
            {
                get { return 0; }
            }

            public IContext Context
            {
                get { return null; }
            }

            public object Invoke(IContext context, object @this, object[] arguments)
            {
                ArrayObject array = (ArrayObject)@this;
                var result = array.Elements[0];
                array.Elements.RemoveAt(0);
                return result;
            }
        }

        private class JoinFunction : ICallable
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
                ArrayObject array = (ArrayObject)@this;
                string result = string.Empty;
                string join;

                if (arguments != null && arguments.Length > 0)
                    join = arguments[0].ToString();
                else
                    join = ",";

                foreach (var element in array.Elements)
                {
                    if (result.Length > 0)
                        result += join;

                    result += element.ToString();
                }

                return result;
            }
        }

        private class SliceFunction : ICallable
        {
            public int Arity
            {
                get { return 0; }
            }

            public IContext Context
            {
                get { return null; }
            }

            public object Invoke(IContext context, object @this, object[] arguments)
            {
                ArrayObject array = (ArrayObject)@this;

                if (arguments != null && arguments.Length == 1)
                {
                    int from = (int)arguments[0];
                    return new ArrayObject(array.Function, array.Elements.Skip(from));
                }

                return new ArrayObject(array.Function, array.Elements);
            }
        }
    }
}
