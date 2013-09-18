using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSql
{
    static class CharUtil
    {
        public static CharacterRange GetRange(char c)
        {
            if (c < 0x20)
            {
                return CharacterRange.Control;
            }
            else if (0x30 <= c && c <= 0x39)
            {
                return CharacterRange.Digit;
            }
            else if (0x41 <= c && c <= 0x5A || 0x61 <= c && c <= 0x7A)
            {
                return CharacterRange.LatinNoAccent;
            }
            else if (c < 0xC0)
            {
                return CharacterRange.Symbol;
            }
            else if (c < 0x200)
            {
                return CharacterRange.LatinAccent;
            }
            else if (c < 0x500)
            {
                return CharacterRange.NonLatin;
            }
            else
            {
                return CharacterRange.NonEuropean;
            }
        }
    }
}
