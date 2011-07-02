namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public class VarCommand : ICommand
    {
        private string name;

        public VarCommand(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void Execute(IContext context)
        {
            context.DefineVariable(this.name);
        }
    }
}
