namespace AjScript.Commands
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;

    public class ForEachCommand : ICommand
    {
        private string name;
        private IExpression expression;
        private ICommand command;

        public ForEachCommand(string name, IExpression expression, ICommand command)
        {
            this.name = name;
            this.expression = expression;
            this.command = command;
        }

        public string Name { get { return this.name; } }

        public IExpression Expression { get { return this.expression; } }

        public ICommand Command { get { return this.command; } }

        public void Execute(IContext context)
        {
            foreach (object result in (IEnumerable)this.expression.Evaluate(context))
            {
                context.SetValue(this.name, result);
                this.command.Execute(context);
            }
        }
    }
}
