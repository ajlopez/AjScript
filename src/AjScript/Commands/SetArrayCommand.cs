namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public class SetArrayCommand : ICommand
    {
        private IExpression leftValue;
        private ICollection<IExpression> arguments;
        private IExpression expression;

        public SetArrayCommand(IExpression leftValue, ICollection<IExpression> arguments, IExpression expression)
        {
            this.leftValue = leftValue;
            this.arguments = arguments;
            this.expression = expression;
        }

        public IExpression LeftValue { get { return this.leftValue; } }

        public IExpression Expression { get { return this.expression; } }

        public ICollection<IExpression> Arguments { get { return this.arguments; } }

        public void Execute(IContext context)
        {
            object value = this.expression.Evaluate(context);
            object[] indexes = null;
            List<object> values = new List<object>();

            foreach (IExpression expression in this.arguments)
                values.Add(expression.Evaluate(context));

            indexes = values.ToArray();

            object obj = null;

            if (ObjectUtilities.IsNumber(indexes[0]))
                obj = ExpressionUtilities.ResolveToList(this.leftValue, context);
            else if (indexes.Length == 1)
            {
                IObject iobj = (IObject) ExpressionUtilities.ResolveToObject(this.leftValue, context);
                iobj.SetValue(indexes[0].ToString(), value);
                return;
            }
            else
                obj = ExpressionUtilities.ResolveToDictionary(this.leftValue, context);

            ObjectUtilities.SetIndexedValue(obj, indexes, value);
        }
    }
}
