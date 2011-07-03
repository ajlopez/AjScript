namespace AjScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class TopContext : Context, IObject
    {
        public TopContext()
        {
        }

        public IFunction Function
        {
            get { throw new NotImplementedException(); }
        }

        public object Invoke(string name, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public object Invoke(ICallable method, object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
