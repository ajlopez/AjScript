namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpectedException : LexerException
    {
        public NameExpectedException()
            : base("A name was expected")
        {
        }
    }
}
