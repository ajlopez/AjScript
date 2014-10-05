namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICallable
    {
        object Invoke(IContext context, object @this, object[] arguments);
    }
}
