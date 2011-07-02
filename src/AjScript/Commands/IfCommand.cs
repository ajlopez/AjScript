namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;

    public class IfCommand : ICommand
    {
        private IExpression condition;
        private ICommand thenCommand;
        private ICommand elseCommand;

        public IfCommand(IExpression condition, ICommand thenCommand)
            : this(condition, thenCommand, null)
        {
        }

        public IfCommand(IExpression condition, ICommand thenCommand, ICommand elseCommand)
        {
            this.condition = condition;
            this.thenCommand = thenCommand;
            this.elseCommand = elseCommand;
        }

        public IExpression Condition { get { return this.condition; } }

        public ICommand ThenCommand { get { return this.thenCommand; } }

        public ICommand ElseCommand { get { return this.elseCommand; } }

        public void Execute(IContext context)
        {
            object result = this.condition.Evaluate(context);

            if (Predicates.IsTrue(result))
                this.thenCommand.Execute(context);
            else if (this.elseCommand != null)
                this.elseCommand.Execute(context);
        }
    }
}
