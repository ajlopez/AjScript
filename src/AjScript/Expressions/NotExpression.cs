namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Language;

    using Microsoft.VisualBasic.CompilerServices;

    public class NotExpression : UnaryExpression
    {
        public NotExpression(IExpression expression)
            : base(expression)
        {
        }

        public override object Apply(object value)
        {
            return !Predicates.IsTrue(value);
        }
    }
}
