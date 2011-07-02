namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualBasic.CompilerServices;

    public class ConcatenateExpression : BinaryExpression
    {
        public ConcatenateExpression(IExpression left, IExpression right)
            : base(left, right)
        {
        }

        public override object Apply(object leftValue, object rightValue)
        {
            if (leftValue == null)
                leftValue = string.Empty;

            if (rightValue == null)
                rightValue = string.Empty;

            return Operators.ConcatenateObject(leftValue, rightValue);
        }
    }
}
