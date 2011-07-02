namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OrExpression : IExpression
    {
        private IExpression leftExpression;
        private IExpression rigthExpression;

        public OrExpression(IExpression left, IExpression right)
        {
            this.leftExpression = left;
            this.rigthExpression = right;
        }

        public IExpression LeftExpression { get { return this.leftExpression; } }

        public IExpression RightExpression { get { return this.rigthExpression; } }

        public object Evaluate(IContext context)
        {
            object leftValue = this.leftExpression.Evaluate(context);

            if (Predicates.IsTrue(leftValue))
                return true;

            return Predicates.IsTrue(this.rigthExpression.Evaluate(context));
        }
    }
}
