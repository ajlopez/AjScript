namespace AjScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Primitives;

    public class Machine
    {
        private IContext context;

        public Machine()
        {
            this.context = new Context();
            this.context.SetValue("Object", new ObjectFunction(this.context));
        }

        public IContext Context { get { return this.context; } }
    }
}
