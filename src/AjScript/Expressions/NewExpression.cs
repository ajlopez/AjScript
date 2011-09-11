namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Language;

    [Serializable]
    public class NewExpression : IExpression
    {
        private IExpression expression;
        private ICollection<IExpression> arguments;

        public NewExpression(IExpression expression, ICollection<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public IExpression Expression { get { return this.expression; } }

        public ICollection<IExpression> Arguments { get { return this.arguments; } }

        public object Evaluate(IContext context)
        {
            object value = null;

            if (this.expression is DotExpression)
            {
                DotExpression dotexpr = (DotExpression)this.expression;
                value = dotexpr.TryEvaluate(context);
            }
            else
                value = this.expression.Evaluate(context);

            Type type = null;

            if (!(value is IFunction))
            {
                if (this.expression is VariableExpression)
                {
                    type = TypeUtilities.GetType(context, ((VariableExpression)this.expression).Name);
                }
                else if (this.expression is DotExpression)
                {
                    type = ((DotExpression)this.expression).AsType();
                }
            }

            object[] parameters = null;

            if (this.arguments != null && this.arguments.Count > 0)
            {
                List<object> values = new List<object>();

                foreach (IExpression argument in this.arguments)
                    values.Add(argument.Evaluate(context));

                parameters = values.ToArray();
            }

            if (value is IFunction)
                return ((IFunction)value).NewInstance(parameters);

            return Activator.CreateInstance(type, parameters);
        }
    }
}
