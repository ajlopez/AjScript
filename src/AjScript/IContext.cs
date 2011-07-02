namespace AjScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjScript.Language;

    public interface IContext
    {
        ReturnValue ReturnValue { get; set;  }

        void SetValue(string name, object value);

        object GetValue(string name);

        void DefineVariable(string name);
    }
}

