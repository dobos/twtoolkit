using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwitterLib.Logging
{
    public class TextWriterDispatch : TextWriter
    {
        private TextWriter[] writers;

        public override Encoding Encoding
        {
            get { throw new NotImplementedException(); }
        }

        public override void Write(string format, object arg0)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].Write(format, arg0);
            }
        }

        public override void Write(string format, object arg0, object arg1)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].Write(format, arg0, arg1);
            }
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].Write(format, arg0, arg1, arg2);
            }
        }

        public override void Write(string format, params object[] arg)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].Write(format, arg);
            }
        }

        public override void WriteLine()
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].WriteLine();
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].WriteLine(format, arg0);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].WriteLine(format, arg0, arg1);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].WriteLine(format, arg0, arg1, arg2);
            }
        }

        public override void WriteLine(string format, params object[] arg)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                writers[i].WriteLine(format, arg);
            }
        }
    }
}
