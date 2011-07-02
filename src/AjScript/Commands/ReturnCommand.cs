namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public class ReturnCommand : ICommand
    {
        private IExpression expression;

        public ReturnCommand()
            : this(null)
        {
        }

        public ReturnCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            context.ReturnValue = new ReturnValue(this.expression.Evaluate(context));
        }
    }
}
