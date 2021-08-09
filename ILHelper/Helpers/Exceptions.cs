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

        public static string Consume(Exception e, StringBuilder exceptionBuilder = null, bool includeCallerInfo = true, [CallerMemberName] string Name = "", [CallerFilePath] string FilePath = "", [CallerLineNumber] int LineNumber = -1)
        {
            StringBuilder builder = exceptionBuilder ?? new();

            if (includeCallerInfo)
            {
                builder.Append($"Exception consumed in {Name}, {FilePath} [{LineNumber}]");
            }


            builder.AppendLine(e.Message);
            builder.Append('\t');
            builder.Append(e.StackTrace);

            if (e.InnerException is not null)
            {
                builder.Append(Consume(e.InnerException, builder, false));
            }

            string result = builder.ToString();

            if (includeCallerInfo)
            {
                Console.WriteLine(result);
            }

            return result;
        }
    }
}
