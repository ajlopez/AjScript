namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Commands;
    using AjScript.Language;

    public class FunctionExpression : IExpression
    {
        private string[] parameterNames;
        private ICommand body;
        private string name;

        public FunctionExpression(string name, string[] parameterNames, ICommand body)
        {
            this.name = name;
            this.parameterNames = parameterNames;
            this.body = body;
        }

        public string[] ParameterNames { get { return this.parameterNames; } }

        public ICommand Body { get { return this.body; } }

        public string Name { get { return this.name; } }

        public object Evaluate(IContext context)
        {
            object value = new Function(this.parameterNames, this.body, context);
            if (this.name != null)
            {
                context.DefineVariable(this.name);
                context.SetValue(this.name, value);
            }
            return value;
        }
    }
}
