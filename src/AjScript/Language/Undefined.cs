namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Undefined
    {
        private static Undefined instance = new Undefined();

        private Undefined()
        {
        }

        public static Undefined Instance { get { return instance; } }
    }
}
