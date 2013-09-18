using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib
{
    public static class JsonUtil
    {
        public static object GetValue(object obj, string id)
        {
            var parts = id.Split('.');


            for (int i = 0; i < parts.Length; i++)
            {
                if (obj == null)
                {
                    return null;
                }

                var dict = (Dictionary<string, object>)obj;

                if (dict.ContainsKey(parts[i]))
                {
                    obj = dict[parts[i]];
                }
                else
                {
                    return null;
                }
            }

            return obj;
        }

        public static bool GetBoolean(object obj, string id)
        {
            return (bool)GetValue(obj, id);
        }

        public static bool? GetNullableBoolean(object obj, string id)
        {
            return (bool?)GetValue(obj, id);
        }

        public static Int32 GetInt32(object obj, string id)
        {
            return Int32.Parse((string)GetValue(obj, id));
        }

        public static Int32? GetNullableInt32(object obj, string id)
        {
            Int32 res;
            string value = (string)GetValue(obj, id);
            if (Int32.TryParse(value, out res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static Int64 GetInt64(object obj, string id)
        {
            return Int64.Parse((string)GetValue(obj, id));
        }

        public static Int64? GetNullableInt64(object obj, string id)
        {
            Int64 res;
            string value = (string)GetValue(obj, id);
            if (Int64.TryParse(value, out res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static DateTime GetDateTime(object obj, string id)
        {
            DateTime value;
            Util.TryParseDateTime((string)GetValue(obj, id), out value);
            return value;
        }

        public static string GetString(object obj, string id)
        {
            return (string)GetValue(obj, id);
        }
    }
}
