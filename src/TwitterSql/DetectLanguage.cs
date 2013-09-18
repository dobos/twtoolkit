using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using Microsoft.WindowsAPICodePack.ExtendedLinguisticServices;

public partial class UserDefinedFunctions
{
    private static readonly Regex UrlRegex = new Regex(@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?");
    private static readonly Regex UserMentionRegex = new Regex("@[a-zA-Z0-9_]+");
    private static readonly Regex HashtagRegex = new Regex("#[a-zA-Z0-9_]+");

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RecognizeText(SqlString text)
    {
        if (string.IsNullOrEmpty(text.Value))
        {
            return "??";
        }

        var languages = RecognizeTextInternal(text.Value);

        if (languages.Length > 0)
        {
            return new SqlString(languages[0]);
        }
        else
        {
            return new SqlString("??");
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RecognizeText2(SqlString text, SqlInt16 i)
    {
        if (string.IsNullOrEmpty(text.Value))
        {
            return "??";
        }

        var languages = RecognizeTextInternal(text.Value);

        if (languages.Length > i.Value)
        {
            return new SqlString(languages[i.Value]);
        }
        else
        {
            return new SqlString("??");
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RemoveUrl(SqlString text)
    {
        return new SqlString(RemoveRegexMatches(text.Value, UrlRegex));
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString RemoveUserMention(SqlString text)
    {
        return new SqlString(RemoveRegexMatches(text.Value, UserMentionRegex));
    }

    private static string[] RecognizeTextInternal(string text)
    {
        var languageDetection = new MappingService(MappingAvailableServices.LanguageDetection);

        using (var bag = languageDetection.RecognizeText(text, null))
        {
            return bag.GetResultRanges()[0].FormatData(new StringArrayFormatter());
        }
    }

    private static string RemoveRegexMatches(string text, Regex regex)
    {
        var t = text;
        int i = 0;
        var r = new StringBuilder();

        while (true)
        {
            var m = regex.Match(t, i);

            if (!m.Success)
            {
                r.Append(t.Substring(i));
                return r.ToString();
            }
            else
            {
                if (m.Index - i > 0)
                {
                    r.Append(t.Substring(i, m.Index - i));
                }

                i = m.Index + m.Length;
            }
        }
    }
};

