using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace TwitterSql
{
    public partial class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.None)]
        public static float Break(SqlString text)
        {
            // break into individual words
            var parts = text.Value.Split(Constants.WordSeparators);

            //

            // Put your code here
            return 1;
        }

        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.None)]
        public static float LangProb_en(SqlString text)
        {
            string t = text.Value;

            for (int i = 0; i < text.Value.Length; i++)
            {
                char c = t[i];


            }

            // Put your code here
            return 1;
        }

    }

}