using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Extensions
{
    public static class DictionaryExtenstions
    {
#nullable enable
        /// <summary>
        /// Attempts to find a value in the dictionary with the provided key. If the value exists within the dictionary the <see cref="Type"/> is checked, if the <see cref="Type"/> has an indentity conversion compatible with <typeparamref name="U"/> the type is converted and returned. Otherwise returns <see langword="null"/>.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static U? FindAndCast<U>(this IDictionary<string, object> dictionary, string Key)
        {
            if (dictionary.TryGetValue(Key, out object? value))
            {
                if (value is U rightType)
                {
                    return rightType;
                }
            }

            return default;
        }
#nullable disable
    }
}
