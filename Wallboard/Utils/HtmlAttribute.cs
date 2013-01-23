using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wallboard.Utils
{
    public class HtmlAttribute : IHtmlString
    {
        private string _InternalValue = String.Empty;
        private string _Seperator;

        public string Name { get; set; }
        public string Value { get; set; }
        public bool Condition { get; set; }

        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        public HtmlAttribute(string name, string seperator)
        {
            Name = name;
            _Seperator = seperator ?? " ";
        }

        public HtmlAttribute Add(string value)
        {
            return Add(value, true);
        }

        public HtmlAttribute Add(string value, bool condition)
        {
            if (!String.IsNullOrWhiteSpace(value) && condition)
                _InternalValue += value + _Seperator;

            return this;
        }

        public string ToHtmlString()
        {
            if (!String.IsNullOrWhiteSpace(_InternalValue))
                _InternalValue = String.Format("{0}=\"{1}\"", Name, _InternalValue.Substring(0, _InternalValue.Length - _Seperator.Length));
            return _InternalValue;
        }
    }
}