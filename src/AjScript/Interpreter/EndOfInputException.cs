namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EndOfInputException : Exception
    {
        public EndOfInputException()
            : this("End of Input")
        {
        }

        public EndOfInputException(string msg)
            : base(msg)
        {
        }
    }
}
