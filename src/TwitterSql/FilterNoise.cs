using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace TwitterSql
{
    public partial class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction(DataAccess=DataAccessKind.None,
            IsDeterministic=true, IsPrecise=true)]
        public static SqlString FilterNoise(SqlString text)
        {
            var source = text.Value.ToCharArray();
            var dest = new char[0x1000];
            var word = new char[0x1000];
            int wc = 0;
            int rc = 0;
            char ll = '\0';
            int lc = 0;
            var isnoisy = false;
            var isurl = false;
            var iswhitespace = false;
            var ispunctuation = false;
            char c = '\0';

            // Read input letter by letter and put together into the word buffer
            for (int i = 0; i <= source.Length; i++)
            {
                if (i == source.Length)
                {
                    c = '\0';
                }
                else
                {
                    c = source[i];

                    iswhitespace = Constants.Whitespace.IndexOf(c) >= 0;
                    ispunctuation = Constants.WordSeparator.IndexOf(c) >= 0;
                }

                if (iswhitespace || ispunctuation || c == '\0')
                {
                    if (wc > 3 && !isnoisy && !isurl)
                    {
                        // End of word reached, copy to output
                        for (int k = 0; k < wc; k++)
                        {
                            dest[rc++] = word[k];
                        }

                        dest[rc++] = ' ';
                    }

                    wc = 0;
                    isnoisy = false;
                    if (iswhitespace)
                    {
                        isurl = false;
                    }

                    if (c == '\0')
                    {
                        break;
                    }
                }
                else if (i < source.Length - 4 &&
                    (source[i] == 'h' || source[i] == 'H') &&
                    (source[i + 1] == 't' || source[i + 1] == 'T') &&
                    (source[i + 2] == 't' || source[i + 2] == 'T') &&
                    (source[i + 3] == 'p' || source[i + 3] == 'P'))
                {
                    isurl = true;
                }
                else
                {
                    if (c == ll)
                    {
                        lc++;
                    }
                    else
                    {
                        ll = c;
                        lc = 1;
                    }

                    isnoisy |= lc >= 3;							// letter repeated more than thrice

                    var r = CharUtil.GetRange(c);
                    isnoisy |= r == CharacterRange.Control;
                    isnoisy |= r == CharacterRange.Digit;
                    isnoisy |= r == CharacterRange.Symbol;
                    isnoisy |= r == CharacterRange.NonEuropean;

                    // append to the word
                    word[wc++] = source[i];
                }
            }



            return new SqlString(new String(dest, 0, rc));
        }
    };

}