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

        protected void AppendCursoredParameters(ref Uri url)
        {
            string pars = String.Empty;

            if (count > 0)
            {
                AppendUrlParameter(ref url, "count", count.ToString());
                pars += String.Format("&count={0}", count);
            }

            if (cursor > 0)
            {
                AppendUrlParameter(ref url, "cursor", cursor.ToString());
            }
        }
    }
}
