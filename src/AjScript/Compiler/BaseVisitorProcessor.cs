namespace AjScript.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BaseVisitorProcessor<C> : IVisitorProcessor<C> where C : IVisitorContext
    {
        private IDictionary<Type, IVisitor<C>> visitors = new Dictionary<Type, IVisitor<C>>();

        public void RegisterVisitor(Type type, IVisitor<C> visitor)
        {
            visitors[type] = visitor;
        }

        public void RegisterVisitor<T>(IVisitor<T, C> visitor)
        {
            visitors[typeof(T)] = visitor;
        }

        public void Process(C context, object element)
        {
            IVisitor<C> visitor = this.visitors[element.GetType()];
            visitor.Process(this, context, element);
        }
    }
}
