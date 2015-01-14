namespace AjScript.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public class StringFunction : Function
    {
        public StringFunction(IContext context)
            : base(null, null, context)
        {
            var prototype = new StringObject();

            this.SetValue("prototype", prototype);
        }

        public override object NewInstance(object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
