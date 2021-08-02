using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Extensions
{
    public static class DynamicAssemblyExtensions
    {
        public static T CreateType<T>(this IDynamicAssemblyBuilder assemblyBuilder, string Name)
        {
            return (T)assemblyBuilder.CreateType(Name);
        }

    }
}
