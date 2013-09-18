using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib
{
    class TwitterReaderException : Exception
    {
        public TwitterReaderException(string message)
            : base(message)
        {
        }
    }
}
