using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace editor.attr
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LinkTo : PropertyAttribute
    {
        public string pattern;

        public LinkTo(string pattern)
        {
            this.pattern = pattern;
        }

        public bool Match(string s)
        {
            return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
        }
    }
}


