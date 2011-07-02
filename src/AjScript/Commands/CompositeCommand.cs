namespace AjScript.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CompositeCommand : ICommand
    {
        private ICollection<ICommand> hoistedCommands;
        private ICollection<ICommand> commands;

        public CompositeCommand(ICollection<ICommand> commands)
            : this(null, commands)
        {
        }

        public CompositeCommand(ICollection<ICommand> hoistedCommands, ICollection<ICommand> commands)
        {
            this.hoistedCommands = hoistedCommands;
            this.commands = commands;
        }

        public int CommandCount { get { if (this.commands == null) return 0; return this.commands.Count; } }

        public ICollection<ICommand> Commands { get { return this.commands; } }

        public int HoistedCommandCount { get { if (this.hoistedCommands == null) return 0; return this.hoistedCommands.Count; } }

        public ICollection<ICommand> HoistedCommands { get { return this.hoistedCommands; } }

        public virtual void Execute(IContext context)
        {
            if (this.hoistedCommands != null)
                foreach (ICommand command in this.hoistedCommands)
                    command.Execute(context);

            foreach (ICommand command in this.commands)
            {
                command.Execute(context);

                if (context.ReturnValue != null)
                    return;
            }
        }
    }
}
