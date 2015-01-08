using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{

    private class str_idx
    {
        public string word;
        public short str_id;
        public short place;
    }

   

    [Microsoft.SqlServer.Server.SqlFunction(TableDefinition="word nvarchar(140), id smallint, place smallint", FillRowMethodName="fWordBreaker_Fill")]
    public static IEnumerable fWordBreaker(SqlString text)
    {
        var words = new List<str_idx>();

        string _text=(string)text;
        int count=_text.Length;
       
        const string whitespace = " \t\n\r";
	    const string punctuation = ",.!?;:\"-/+(){}[]\\`´_^~";

        short word_idx = 0; //word count
	    int ws = 0;		// word start
	    int wc = 0;		// word letter count
	    char ll = '\0';	// last letter processed
	    uint lc = 0;		// same letter count
	    bool isnoisy = false;
	    bool isurl = false;
        byte[] bsource = new byte[_text.Length * sizeof(char)];
        System.Buffer.BlockCopy(_text.ToCharArray(), 0, bsource, 0, bsource.Length);
	    int i = 0;


	    char c;
	    byte bu=0x00;
	    byte bl=0x00;
	    bool isWhitespace=false;
	    bool isPunctuation=false;

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
		    return words;
	    }

	    // Read input letter by letter and put together into the word buffer
	    for (; i <= count; i ++)
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
			    if (wc > 3 && !isnoisy && !isurl)
			    {
				    //pWordSink->PutWord(wc, source + ws, wc, ws);
				    //pWordSink->PutBreak(WORDREP_BREAK_EOW);
                    str_idx tmp = new str_idx();
                    tmp.word = _text.Substring(ws,wc);
                    tmp.place = (short) ws;
                    tmp.str_id = word_idx++;
                    words.Add(tmp);
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

			    isnoisy |= lc >= 3;							// letter repeated more than trice

			    isnoisy |= (bl > 0x05);						// Not latin greek or cyrillic
			    isnoisy |= (bl == 0x00 && bu < 0x41);					// Symbols
			    isnoisy |= (bl == 0x00 && 0x7B <= bu && bu < 0xC0);		// More symbols
			    isnoisy |= (bl == 0x03 && bu < 0x80);		// More symbols

			    // append to the word
			    if (wc == 0)
			    {
				    ws = i;
			    }
			    wc++;
		    }
	    }



        return words;
    }

    public static void fWordBreaker_Fill(object item,  out SqlString word, out SqlInt16 id, out SqlInt16 place)
    {
        word=new SqlString(((str_idx)item).word);
        id = new SqlInt16(((str_idx)item).str_id);
        place = new SqlInt16(((str_idx)item).place);
    }
}
