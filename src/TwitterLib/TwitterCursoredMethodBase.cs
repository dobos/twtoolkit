using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib
{
    public abstract class TwitterCursoredMethodBase : TwitterMethodBase
    {
        private int count;
        private int cursor;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public int Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        public TwitterCursoredMethodBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.count = -1;
            this.cursor = -1;
        }

        protected override void GetQueryParameters(Dictionary<string, string> parameters)
        {
            if (count > 0)
            {
                parameters.Add("count", count.ToString());
            }

            if (cursor > 0)
            {
                parameters.Add("cursor", cursor.ToString());
            }
        }
    }
}
