namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayExpression : IExpression
    {
        private IExpression expression;
        private ICollection<IExpression> arguments;

        public ArrayExpression(IExpression expression, ICollection<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public IExpression Expression { get { return this.expression; } }

        public ICollection<IExpression> Arguments { get { return this.arguments; } }

        public object Evaluate(IContext context)
        {
            object obj = null;

            return this.Evaluate(context, ref obj);
        }

        public object Evaluate(IContext context, ref object obj)
        {
            obj = this.Expression.Evaluate(context);

            object[] parameters = null;

            if (this.arguments != null)
            {
                List<object> values = new List<object>();

                foreach (IExpression argument in this.arguments)
                    values.Add(argument.Evaluate(context));

                parameters = values.ToArray();
            }

            // TODO if undefined, do nothing
            if (obj == null)
                return null;

            return ObjectUtilities.GetIndexedValue(obj, parameters);
        }

        private static Type AsType(IExpression expression)
        {
            string name = AsName(expression);

            if (name == null)
                return null;

            return TypeUtilities.AsType(name);
        }

        private static string AsName(IExpression expression)
        {
            if (expression is DotExpression)
            {
                DotExpression dot = (DotExpression)expression;

                return AsName(dot.Expression) + "." + dot.Name;
            }

            return null;
        }
    }
}
