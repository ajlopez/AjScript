namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NoOperationCommand : ICommand
    {
        private static NoOperationCommand instance = new NoOperationCommand();

        private NoOperationCommand()
        {
        }

        public static NoOperationCommand Instance { get { return instance; } }

        public void Execute(IContext context)
        {
        }
    }
}
