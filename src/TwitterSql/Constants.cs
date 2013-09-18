using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSql
{
    public static class Constants
    {
        public static readonly char[] WordSeparators = { ' ', '\t', '\r', '\n', ',', ';', ':', '=', '+', '(', ')' };

        public const string Whitespace = " \t\n\r";
        public const string WordSeparator = ",.!?;:\"'-/=()+{}[]";
    }
}
