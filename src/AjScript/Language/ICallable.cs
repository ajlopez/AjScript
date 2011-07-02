namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICallable
    {
        int Arity { get; }

        IContext Context { get; }

        object Invoke(IContext context, object @this, object[] arguments);
    }
}
