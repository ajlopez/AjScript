namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class InvalidInputException : LexerException
    {
        public InvalidInputException(string text)
            : base(string.Format(CultureInfo.CurrentCulture, "Invalid Input '{0}'", text))
        {
        }
    }
}
