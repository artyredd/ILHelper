using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter.Extensions
{
    public static class EnumExtentions
    {
        public static bool Contains<T>(this T container, T Value) where T : Enum, IConvertible
        {
            return (container.ToInt32(null) & Value.ToInt32(null)) != 0;
        }
    }
}
