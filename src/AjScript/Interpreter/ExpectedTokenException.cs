namespace AjScript.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class ExpectedTokenException : LexerException
    {
        public ExpectedTokenException(string token)
            : base(string.Format(CultureInfo.CurrentCulture, "Expected '{0}'", token))
        {
        }
    }
}
