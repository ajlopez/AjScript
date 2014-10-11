namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class InstanceOfExpression : BinaryExpression
    {
        public InstanceOfExpression(IExpression left, IExpression right)
            : base(left, right)
        {
        }

        public override object Apply(object leftValue, object rightValue)
        {
            var proto = ((DynamicObject)rightValue).GetValue("prototype");

            return ((DynamicObject)leftValue).Function.GetValue("prototype") == proto;
        }
    }
}
