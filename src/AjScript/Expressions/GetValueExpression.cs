namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class GetValueExpression : IExpression
    {
        private IExpression expression;

        public GetValueExpression(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public object Evaluate(IContext context)
        {
            if (this.expression == null)
                return null;

            object obj = this.expression.Evaluate(context);

            if (obj == null)
                return null;

            return ((IReference)obj).GetValue();
        }
    }
}
