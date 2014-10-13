namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Language;

    using Microsoft.VisualBasic.CompilerServices;

    public class CompareExpression : BinaryExpression
    {
        private Func<object, object, bool, object> function;
        private ComparisonOperator operation;

        public CompareExpression(ComparisonOperator operation, IExpression left, IExpression right)
            : base(left, right)
        {
            this.operation = operation;

            switch (operation)
            {
                case ComparisonOperator.SemiEqual:
                    this.function = SemiEqual;
                    break;
                case ComparisonOperator.Equal:
                    this.function = Operators.CompareObjectEqual;
                    break;
                case ComparisonOperator.NotEqual:
                    this.function = Operators.CompareObjectNotEqual;
                    break;
                case ComparisonOperator.Less:
                    this.function = Operators.CompareObjectLess;
                    break;
                case ComparisonOperator.LessEqual:
                    this.function = Operators.CompareObjectLessEqual;
                    break;
                case ComparisonOperator.Greater:
                    this.function = Operators.CompareObjectGreater;
                    break;
                case ComparisonOperator.GreaterEqual:
                    this.function = Operators.CompareObjectGreaterEqual;
                    break;
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        public ComparisonOperator Operation { get { return this.operation; } }

        public override object Apply(object leftValue, object rightValue)
        {
            return this.function(leftValue, rightValue, false);
        }

        private static object SemiEqual(object left, object right, bool txtcompare)
        {
            if (left == null && right is Undefined)
                return true;

            if (left is Undefined && right == null)
                return true;

            return left == right;
        }
    }
}
