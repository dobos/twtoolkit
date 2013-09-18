using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.ExtendedLinguisticServices;

namespace TwitterLib
{
    public static class LanguageUtil
    {
        public static bool DetectLanguage(string text, out int words, out string lang1, out string lang2)
        {
            text = RemoveTags(text, out words);

            if (text == null)
            {
                lang1 = "??";
                lang2 = "??";
                return false;
            }
            else
            {
                var langs = RecognizeText(text);

                if (langs.Length > 0)
                {
                    lang1 = langs[0];
                }
                else
                {
                    lang1 = "??";
                }

                if (langs.Length > 1)
                {
                    lang2 = langs[1];
                }
                else
                {
                    lang2 = "??";
                }

                return true;
            }
        }

        private static string RemoveTags(string text, out int words)
        {
            // Remove user_mentions and hashtags and urls

            var word = new char[1024];
            int wi = 0;
            words = 0;

            var res = new char[1024];
            int ri = 0;

            int len = Math.Min(1024, text.Length);

            for (int i = 0; i < len; i++)
            {
                var c = text[i];

                if (char.IsWhiteSpace(c) || i == text.Length - 1)
                {
                    if (wi > 0)
                    {
                        if (word[0] != '@' && word[0] != '#' &&
                            !(wi >= 4 && word[0] == 'h' && word[1] == 't' && word[2] == 't' && word[3] == 'p'))
                        {
                            // Copy word to results
                            for (int j = 0; j < wi; j++)
                            {
                                res[ri++] = word[j];
                            }

                            res[ri++] = ' ';        // Append a space
                            words++;
                        }

                        wi = 0;                     // Rewind buffer
                    }
                }
                else
                {
                    word[wi++] = c;
                }
            }

            if (ri == 0)
            {
                return null;
            }
            else
            {
                return new String(res, 0, ri);
            }
        }


        private static string[] RecognizeText(string text)
        {
            var languageDetection = new MappingService(MappingAvailableServices.LanguageDetection);

            using (var bag = languageDetection.RecognizeText(text, null))
            {
                return bag.GetResultRanges()[0].FormatData(new StringArrayFormatter());
            }
        }
    }
}
