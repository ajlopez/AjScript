namespace AjScript.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IReference
    {
        void SetValue(object value);

        object GetValue();
    }
}
