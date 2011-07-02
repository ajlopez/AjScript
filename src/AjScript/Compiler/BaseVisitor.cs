namespace AjScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BaseVisitor<T,C> : IVisitor<T,C> where C : IVisitorContext
    {
        public void Process(IVisitorProcessor<C> processor, C context, object element)
        {
            this.Process(processor, context, (T) element);
        }

        public abstract void Process(IVisitorProcessor<C> processor, C context, T element);
    }
}
