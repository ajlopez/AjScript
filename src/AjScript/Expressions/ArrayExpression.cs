namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayExpression : IExpression
    {
        private ICollection<IExpression> expressions;

        public ArrayExpression(ICollection<IExpression> expressions)
        {
            this.expressions = expressions;
        }

        public ICollection<IExpression> Expressions { get { return this.expressions; } }

        public object Evaluate(IContext context)
        {
            List<object> values = new List<object>();

            foreach (IExpression expression in this.expressions)
                values.Add(expression.Evaluate(context));

            return values.ToArray();
        }
    }
}
