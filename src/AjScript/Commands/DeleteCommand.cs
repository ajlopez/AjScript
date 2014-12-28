namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public class DeleteCommand : ICommand
    {
        private DotExpression expression;

        public DeleteCommand(DotExpression expression)
        {
            this.expression = expression;
        }

        public DotExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            var target = (DynamicObject)this.expression.Expression.Evaluate(context);
            target.RemoveValue(this.expression.Name);
        }
    }
}
