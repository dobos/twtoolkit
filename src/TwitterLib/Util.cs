using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwitterLib
{
    public class Util
    {
        public static bool TryParseDateTime(string s, out DateTime result)
        {
            // Format: "Sat Mar 24 16:05:36 +0000 2012"

            try
            {
                string[] parts = s.Split(' ');
                string[] tparts = parts[3].Split(':');

                int year = int.Parse(parts[5]);
                int month = 0;
                int day = int.Parse(parts[2]);
                int hour = int.Parse(tparts[0]);
                int min = int.Parse(tparts[1]);
                int sec = int.Parse(tparts[2]);

                switch (parts[1])
                {
                    case "Jan":
                        month = 1;
                        break;
                    case "Feb":
                        month = 2;
                        break;
                    case "Mar":
                        month = 3;
                        break;
                    case "Apr":
                        month = 4;
                        break;
                    case "May":
                        month = 5;
                        break;
                    case "Jun":
                        month = 6;
                        break;
                    case "Jul":
                        month = 7;
                        break;
                    case "Aug":
                        month = 8;
                        break;
                    case "Sep":
                        month = 9;
                        break;
                    case "Oct":
                        month = 10;
                        break;
                    case "Nov":
                        month = 11;
                        break;
                    case "Dec":
                        month = 12;
                        break;
                }

                //DateTime date = DateTime.Parse(parts[5] + '-' + parts[1] + '-' + parts[2] + ' ' + parts[3] + ' ' + parts[4]);

                var date = new DateTime(year, month, day, hour, min, sec);

                // 2013-10-07: Removed
                // result = date.ToUniversalTime();
                result = date;

                return true;
            }
            catch (Exception)
            {
                result = DateTime.MinValue;

                return false;
            }
        }

        public static string UnescapeText(string text)
        {
            if (text == null)
            {
                return text;
            }

            text = text.Replace("\0", " ");
            text = text.Replace("&gt;", ">");
            text = text.Replace("&lt;", "<");
            text = text.Replace("&amp;", "&");
            text = text.Replace("\\\\", "\\");

            // replace character escapes


            return text;
        }
    }
}
