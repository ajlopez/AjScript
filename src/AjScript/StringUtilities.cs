namespace AjScript
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AjScript.Language;

    public class StringUtilities
    {
        private static Regex rex = new Regex(@"\$\{[^\}]*\}");

        public static IList<string> SplitText(string text)
        {
            int lastindex = 0;
            List<string> parts = new List<string>();

            for (Match match = rex.Match(text); match.Success; match = match.NextMatch())
            {
                parts.Add(text.Substring(lastindex, match.Index - lastindex));
                parts.Add(match.ToString().Substring(2, match.Length - 3));
                lastindex = match.Index + match.Length;
            }

            if (lastindex < text.Length)
                parts.Add(text.Substring(lastindex, text.Length - lastindex));
            else if (parts.Count == 0)
                parts.Add(text);

            return parts;
        }

        public static string AsString(object obj)
        {
            if (obj == null)
                return "null";

            return obj.ToString();
        }
    }
}
