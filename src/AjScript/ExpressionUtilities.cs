namespace AjScript
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Expressions;
    using AjScript.Language;

    public static class ExpressionUtilities
    {
        public static void SetValue(IExpression expression, object value, IContext context)
        {
            if (expression is VariableExpression)
            {
                SetValue((VariableExpression)expression, value, context);
                return;
            }

            if (expression is DotExpression)
            {
                SetValue((DotExpression)expression, value, context);
                return;
            }

            throw new InvalidOperationException("Invalid left value");
        }

        public static object ResolveToObject(IExpression expression, IContext context)
        {
            if (expression is VariableExpression)
                return ResolveToObject((VariableExpression)expression, context);

            if (expression is DotExpression)
                return ResolveToObject((DotExpression)expression, context);

            return expression.Evaluate(context);
        }

        public static object ResolveToList(IExpression expression, IContext context)
        {
            if (expression is VariableExpression)
                return ResolveToList((VariableExpression)expression, context);

            if (expression is DotExpression)
                return ResolveToList((DotExpression)expression, context);

            return expression.Evaluate(context);
        }

        public static IDictionary ResolveToDictionary(IExpression expression, IContext context)
        {
            if (expression is VariableExpression)
                return ResolveToDictionary((VariableExpression)expression, context);

            if (expression is DotExpression)
                return ResolveToDictionary((DotExpression)expression, context);

            return (IDictionary)expression.Evaluate(context);
        }

        private static void SetValue(VariableExpression expression, object value, IContext context)
        {
            context.SetValue(expression.Name, value);
        }

        private static void SetValue(DotExpression expression, object value, IContext context)
        {
            if (expression.Arguments != null)
                throw new InvalidOperationException("Invalid left value");

            object obj = ResolveToObject(expression.Expression, context);

            ObjectUtilities.SetValue(obj, expression.Name, value);
        }

        private static object ResolveToObject(VariableExpression expression, IContext context)
        {
            string name = expression.Name;

            object obj = context.GetValue(name);

            if (obj == null || obj == Undefined.Instance)
            {
                obj = new DynamicObject();

                // TODO Review if Local or not
                context.SetValue(name, obj);
            }

            return obj;
        }

        private static object ResolveToObject(DotExpression expression, IContext context)
        {
            object obj = ResolveToObject(expression.Expression, context);

            if (obj is DynamicObject)
            {
                DynamicObject dynobj = (DynamicObject)obj;

                obj = dynobj.GetValue(expression.Name);

                if (obj == null || obj == Undefined.Instance)
                {
                    obj = new DynamicObject();
                    dynobj.SetValue(expression.Name, obj);
                }

                return obj;
            }

            return ObjectUtilities.GetValue(obj, expression.Name);
        }

        private static object ResolveToList(VariableExpression expression, IContext context)
        {
            string name = expression.Name;

            object obj = context.GetValue(name);

            if (obj == null || obj == Undefined.Instance)
            {
                obj = new ArrayList();

                // TODO Review if Local or not
                context.SetValue(name, obj);
            }

            return obj;
        }

        private static IList ResolveToList(DotExpression expression, IContext context)
        {
            object obj = ResolveToObject(expression.Expression, context);

            if (obj is DynamicObject)
            {
                DynamicObject dynobj = (DynamicObject)obj;

                obj = dynobj.GetValue(expression.Name);

                if (obj == null || obj == Undefined.Instance)
                {
                    obj = new ArrayList();
                    dynobj.SetValue(expression.Name, obj);
                }

                return (IList) obj;
            }

            return (IList) ObjectUtilities.GetValue(obj, expression.Name);
        }

        private static IDictionary ResolveToDictionary(VariableExpression expression, IContext context)
        {
            string name = expression.Name;

            object obj = context.GetValue(name);

            if (obj == null || obj == Undefined.Instance)
            {
                obj = new Hashtable();

                // TODO Review if Local or not
                context.SetValue(name, obj);
            }

            return (IDictionary)obj;
        }

        private static IDictionary ResolveToDictionary(DotExpression expression, IContext context)
        {
            object obj = ResolveToObject(expression.Expression, context);

            if (obj is DynamicObject)
            {
                DynamicObject dynobj = (DynamicObject)obj;

                obj = dynobj.GetValue(expression.Name);

                if (obj == null || obj == Undefined.Instance)
                {
                    obj = new Hashtable();
                    dynobj.SetValue(expression.Name, obj);
                }

                return (IDictionary)obj;
            }

            return (IDictionary)ObjectUtilities.GetValue(obj, expression.Name);
        }
    }
}
