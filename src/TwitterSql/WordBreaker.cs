using System;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace TwitterSql
{
    public partial class UserDefinedFunctions
    {

        private struct IndexWord
        {
            public string Word;
            public short ID;
            public short Pos;
        }


        [Microsoft.SqlServer.Server.SqlFunction(TableDefinition = "word nvarchar(140), id smallint, pos smallint", FillRowMethodName = "WordBreaker_Fill")]
        public static IEnumerable WordBreaker(SqlString text)
        {
            return WordBreaker3(text, 4, 2, false, 0);
        }

        [Microsoft.SqlServer.Server.SqlFunction(TableDefinition = "word nvarchar(140), id smallint, pos smallint", FillRowMethodName = "WordBreaker_Fill")]
        public static IEnumerable WordBreaker2(SqlString text, SqlInt16 minlen, SqlInt16 repmax, SqlBoolean hashtaguser)
        {
            return WordBreaker3(text, minlen, repmax, hashtaguser, 0);
        }

        [Microsoft.SqlServer.Server.SqlFunction(TableDefinition = "word nvarchar(140), id smallint, pos smallint", FillRowMethodName = "WordBreaker_Fill")]
        public static IEnumerable WordBreaker3(SqlString text, SqlInt16 minlen, SqlInt16 repmax, SqlBoolean hashtaguser, SqlInt16 maxlen)
        {
            var words = new List<IndexWord>();

            string _text = (string)text;
            int count = _text.Length;

            const string whitespace = " \t\n\r";
            const string punctuation = ",.!?;:\"-/+(){}[]\\`?_^~<>";

            short word_idx = 0;     //word count
            int ws = 0;		        // word start
            int wc = 0;		        // word letter count
            char ll = '\0';	        // last letter processed
            uint lc = 0;		    // same letter count
            bool isnoisy = false;
            bool isurl = false;
            byte[] bsource = new byte[_text.Length * sizeof(char)];
            System.Buffer.BlockCopy(_text.ToCharArray(), 0, bsource, 0, bsource.Length);
            int i = 0;


            char c;
            byte bu = 0x00;
            byte bl = 0x00;
            bool isWhitespace = false;
            bool isPunctuation = false;

            if (count > 2)
            {
                // Check unicode signature, skip if any
                if (bsource[0] == 0xFF || bsource[0] == 0xFE)
                {
                    i = 1;
                }
            }
            else
            {
                count = 0;
                yield break;
            }

            // Read input letter by letter and put together into the word buffer
            for (; i <= count; i++)
            {
                if (i == count)
                {
                    c = '\0';
                }
                else
                {
                    c = _text[i];
                    bu = bsource[2 * i];
                    bl = bsource[2 * i + 1];

                    isWhitespace = whitespace.IndexOf(c) != -1;
                    isPunctuation = punctuation.IndexOf(c) != -1;
                }

                if (isWhitespace || isPunctuation || c == '\0')
                {
                    if (wc >= (int)minlen && !isnoisy && !isurl)
                    {
                        if (maxlen == 0 || wc <= (int)maxlen)
                        {
                            IndexWord tmp = new IndexWord();
                            tmp.Word = _text.Substring(ws, wc);
                            tmp.Pos = (short)ws;
                            tmp.ID = word_idx++;
                            yield return tmp;
                        }
                    }

                    wc = 0;
                    isnoisy = false;
                    if (isWhitespace)
                    {
                        isurl = false;
                    }

                    if (c == '\0')
                    {
                        break;
                    }
                }
                else if (i < count - 4 &&
                    (_text[i] == 'h' || _text[i] == 'H') &&
                    (_text[i + 1] == 't' || _text[i + 1] == 'T') &&
                    (_text[i + 2] == 't' || _text[i + 2] == 'T') &&
                    (_text[i + 3] == 'p' || _text[i + 3] == 'P'))
                {
                    isurl = true;
                }
                else
                {
                    if (c == ll || c == ll + 0x20 || c == ll - 0x20)	// do not distinguish upper and lower case, might cause some false negatives, but only a few
                    {
                        lc++;
                    }
                    else
                    {
                        ll = c;
                        lc = 1;
                    }

                    isnoisy |= lc > (int)repmax;							// letter repeated more than trice

                    if (wc == 0 && hashtaguser) if (c == '#' || c == '@') goto nosymbols;
                    isnoisy |= (bl > 0x05);						// Not latin greek or cyrillic
                    isnoisy |= (bl == 0x00 && bu < 0x41);					// Symbols
                    isnoisy |= (bl == 0x00 && 0x7B <= bu && bu < 0xC0);		// More symbols
                    isnoisy |= (bl == 0x03 && bu < 0x80);		// More symbols
                nosymbols:

                    // append to the word
                    if (wc == 0)
                    {
                        ws = i;
                    }
                    wc++;
                }
            }

            yield break;
        }

        public static void WordBreaker_Fill(object item, out SqlString word, out SqlInt16 id, out SqlInt16 pos)
        {
            var iw = (IndexWord)item;

            word = new SqlString(iw.Word);
            id = new SqlInt16(iw.ID);
            pos = new SqlInt16(iw.Pos);
        }
    }

}

