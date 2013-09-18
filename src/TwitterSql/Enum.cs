using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSql
{
    public enum CharacterRange
    {
        Control,
        Symbol,
        Digit,
        LatinNoAccent,
        LatinAccent,
        NonLatin,
        NonEuropean,
    }
}
