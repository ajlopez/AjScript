namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class LexerException : Exception
    {
        protected LexerException(string msg)
            : base(msg)
        {
        }
    }
}
