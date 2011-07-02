namespace AjScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IVisitorProcessor<C> where C : IVisitorContext
    {
        void Process(C context, object element);
    }
}
