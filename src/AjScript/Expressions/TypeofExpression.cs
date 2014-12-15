namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class TypeOfExpression : IExpression
    {
        private IExpression expression;

        public TypeOfExpression(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public object Evaluate(IContext context)
        {
            var value = this.expression.Evaluate(context);

            if (value == null)
                return "null";

            if (value is Undefined)
                return "undefined";

            if (value is string)
                return "string";

            if (Predicates.IsNumber(value))
                return "number";

            return "object";
        }
    }
}
