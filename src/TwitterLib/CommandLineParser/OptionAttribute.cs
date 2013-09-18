using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : ParameterAttribute
    {
    }
}
