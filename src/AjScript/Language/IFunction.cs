namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFunction : ICallable, IObject
    {
        object NewInstance(object[] parameters);
    }
}
