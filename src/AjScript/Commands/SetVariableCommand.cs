namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public class SetVariableCommand : ICommand
    {
        private string name;
        private IExpression expression;

        public SetVariableCommand(string name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public string Name { get { return this.name; } }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            object value = this.expression.Evaluate(context);
            context.SetValue(this.name, value);
        }
    }
}
