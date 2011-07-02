namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Commands;
    using AjScript.Language;

    public class ObjectExpression : IExpression
    {
        private IList<string> names;
        private IList<IExpression> expressions;

        public ObjectExpression(IList<string> names, IList<IExpression> expressions)
        {
            this.names = names;
            this.expressions = expressions;
        }

        public IList<string> Names { get { return this.names; } }

        public IList<IExpression> Expressions { get { return this.expressions; } }

        public object Evaluate(IContext context)
        {
            DynamicObject obj = new DynamicObject();

            for (int k=0; k < this.names.Count; k++)
                obj.SetValue(this.names[k], this.expressions[k].Evaluate(context));

            return obj;
        }
    }
}
