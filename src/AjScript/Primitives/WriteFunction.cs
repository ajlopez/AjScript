namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class WriteFunction : ICallable
    {
        private TextWriter writer;

        public WriteFunction()
            : this(System.Console.Out)
        {
        }

        public WriteFunction(TextWriter writer)
        {
            this.writer = writer;
        }

        public object Invoke(IContext context, object @this, object[] arguments)
        {
            if (arguments != null)
                foreach (object argument in arguments)
                    this.writer.Write(argument);

            // TODO Review return value
            return null;
        }
    }
}
