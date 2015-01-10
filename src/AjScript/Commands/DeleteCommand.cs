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
        private IExpression expression;

        public DeleteCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            if (this.expression is DotExpression)
            {
                DotExpression dexpr = (DotExpression)this.expression;
                var target = (DynamicObject)dexpr.Expression.Evaluate(context);
                target.RemoveValue(dexpr.Name);
                return;
            }

            context.RemoveValue(((VariableExpression)this.expression).Name);
        }
    }
}
