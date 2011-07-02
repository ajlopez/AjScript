namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;
    using System.IO;

    public class WriteLineFunction : ICallable
    {
        private TextWriter writer;

        public WriteLineFunction()
            : this(System.Console.Out)
        {
        }

        public WriteLineFunction(TextWriter writer)
        {
            this.writer = writer;
        }

        public int Arity
        {
            get { return 1; }
        }

        public IContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public object Invoke(IContext context, object @this, object[] arguments)
        {
            if (arguments!=null)
                foreach (object argument in arguments)
                    this.writer.Write(argument);

            this.writer.WriteLine();

            // TODO Review return value
            return null;
        }
    }
}
