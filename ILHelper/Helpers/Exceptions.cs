using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace ILHelper.Helpers
{
    public static class Exceptions
    {
        public static Exception Generic(string Message, [CallerMemberName] string Name = "", [CallerFilePath] string FilePath = "", [CallerLineNumber] int LineNumber = -1)
        {
            return new Exception($"{Message}{Environment.NewLine}\tin{Name}{Environment.NewLine}\tat{FilePath} [{LineNumber}]");
        }
    }
}
