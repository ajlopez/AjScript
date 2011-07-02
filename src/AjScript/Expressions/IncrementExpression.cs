namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Language;

    using Microsoft.VisualBasic.CompilerServices;

    public class IncrementExpression : UnaryExpression
    {
        private IncrementOperator oper;

        public IncrementExpression(IExpression expression, IncrementOperator oper)
            : base(expression)
        {
            this.oper = oper;
        }

        public IncrementOperator Operator { get { return this.oper; } }

        public override object Apply(object value)
        {
            throw new NotImplementedException();
        }

        public object Apply(object value, IContext context)
        {
            if (value == null)
                value = 0;

            object newvalue = null;
            object retvalue = null;

            switch (this.oper)
            {
                case IncrementOperator.PostIncrement:
                case IncrementOperator.PreIncrement:
                    newvalue = Operators.AddObject(value, 1);
                    break;
                case IncrementOperator.PostDecrement:
                case IncrementOperator.PreDecrement:
                    newvalue = Operators.SubtractObject(value, 1);
                    break;
            }

            ExpressionUtilities.SetValue(this.Expression, newvalue, context);

            switch (this.oper)
            {
                case IncrementOperator.PreIncrement:
                case IncrementOperator.PreDecrement:
                    retvalue = newvalue;
                    break;
                case IncrementOperator.PostIncrement:
                case IncrementOperator.PostDecrement:
                    retvalue = value;
                    break;
            }

            return retvalue;
        }

        public override object Evaluate(IContext context)
        {
            return this.Apply(this.Expression.Evaluate(context), context);
        }
    }
}
