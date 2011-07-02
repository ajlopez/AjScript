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
        private string name;
        private ICollection<IExpression> arguments;

        public NewExpression(string name, ICollection<IExpression> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public string TypeName { get { return this.name; } }

        public ICollection<IExpression> Arguments { get { return this.arguments; } }

        public object Evaluate(IContext context)
        {
            object value = context.GetValue(this.name);

            Type type = null;

            if (!(value is IFunction))
                type = TypeUtilities.GetType(context, this.name);

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
