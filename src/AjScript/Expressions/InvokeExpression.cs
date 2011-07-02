namespace AjScript.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Commands;
    using AjScript.Language;

    [Serializable]
    public class InvokeExpression : IExpression
    {
        private IExpression expression;
        private ICollection<IExpression> arguments;

        public InvokeExpression(IExpression expression, ICollection<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public IExpression Expression { get { return this.expression; } }

        public ICollection<IExpression> Arguments { get { return this.arguments; } }

        public object Evaluate(IContext context)
        {
            object obj = null;
            ICallable callable;

            if (this.expression is ArrayExpression)
                callable = (ICallable)((ArrayExpression)this.expression).Evaluate(context, ref obj);
            else
                callable = (ICallable)this.expression.Evaluate(context);

            List<object> parameters = new List<object>();

            foreach (IExpression expression in this.arguments)
            {
                object parameter = expression.Evaluate(context);

                parameters.Add(parameter);
            }

            if (obj != null && obj is DynamicObject)
            {
                DynamicObject dobj = (DynamicObject)obj;
                return dobj.Invoke(callable, parameters.ToArray());
            }

            return callable.Invoke(context, null, parameters.ToArray());
        }
    }
}
