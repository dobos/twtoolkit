﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwitterLib.Methods
{
    public class Follower : TwitterCursoredMethodBase
    {
        private long userId;
        private string screenName;

        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string ScreenName
        {
            get { return screenName; }
            set { screenName = value; }
        }

        public Follower()
            :base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.userId = 0;
            this.screenName = null;
        }

        protected override string GetHttpMethod()
        {
            return "GET";
        }

        protected override Uri GetUrl()
        {
            var url = new Uri("https://api.twitter.com/1.1/followers/ids.json ");

            AppendCursoredParameters(ref url);

            if (userId > 0)
            {
                AppendUrlParameter(ref url, "user_id", userId.ToString());
            }

            if (screenName != null)
            {
                AppendUrlParameter(ref url, "screen_name", screenName);
            }

            return url;
        }

        protected override string GetPostData()
        {
            return null;
        }
    }
}
