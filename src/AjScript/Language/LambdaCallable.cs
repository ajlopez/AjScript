namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LambdaCallable : ICallable
    {
        private Func<IContext, object, object[], object> lambda;

        public LambdaCallable(Func<IContext, object, object[], object> lambda)
        {
            this.lambda = lambda;
        }

        public object Invoke(IContext context, object @this, object[] arguments)
        {
            return this.lambda(context, @this, arguments);
        }
    }
}
