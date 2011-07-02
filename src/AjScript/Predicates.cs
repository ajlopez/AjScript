namespace AjScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class Predicates
    {
        public static bool IsFalse(object obj)
        {
            if (obj == null)
                return true;

            if (obj == Undefined.Instance)
                return true;

            if (obj is bool)
                return !(bool)obj;

            if (obj is int)
                return (int)obj == 0;

            if (obj is string)
                return string.IsNullOrEmpty((string)obj);

            if (obj is long)
                return (long)obj == 0;

            if (obj is short)
                return (short)obj == 0;

            if (obj is double)
                return (double)obj == 0;

            if (obj is float)
                return (float)obj == 0;

            return false;
        }

        public static bool IsTrue(object obj)
        {
            return !IsFalse(obj);
        }
    }
}
