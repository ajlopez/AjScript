namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class TypeofExpression : IExpression
    {
        private IExpression expression;

        public TypeofExpression(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public object Evaluate(IContext context)
        {
            throw new NotImplementedException();
        }
    }
}
