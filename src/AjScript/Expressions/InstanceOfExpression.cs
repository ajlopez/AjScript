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
            if (!(rightValue is DynamicObject))
                return false;

            var proto = ((DynamicObject)rightValue).GetValue("prototype");

            if (!(leftValue is DynamicObject))
                return false;

            for (object lproto = ((DynamicObject)leftValue).Function.GetValue("prototype"); lproto != null && !(lproto is Undefined); lproto = ((DynamicObject)lproto).GetValue("prototype"))
                if (proto == lproto)
                    return true;

            return false;
        }
    }
}
